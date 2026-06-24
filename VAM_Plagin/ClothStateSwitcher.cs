// V11F_IGNORE_PLUGIN_OFF_APPEARANCE_RESTORE: v11e + ignore plugin-caused clothing-active OFF in Appearance Watch and restore captured active items before WEAR ALL / manual SCAN.
// V11C_EMERGENCY_ROLLBACK_NO_ACTIVE_TOGGLE: rollback to safe v10x base; removes risky DAZCharacterSelector.SetActiveClothingItem clothing checkbox toggles from v11a/v11b.
// ============================================================
// ClothStateSwitcher.cs
// Progressive clothing control system for Virt-A-Mate
//
// Features:
// - One-click progressive undress / redress
// - Supports Sim clothing with pull-off clothing effects
// - Automatic undress / automatic redress modes
// - Full external control via buttons and triggers
//
// Author : VAMT
// ver    : 0.2
// ============================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Globalization;

public class ClothStateSwitcher : MVRScript
{
    private const string PERSON_NONE = "<none>";

    private const string ORDER_OUTER_FIRST = "Outer First";
    private const string ORDER_TOP_FIRST = "Top First";
    private const string ORDER_BOTTOM_FIRST = "Bottom First";
    private const string ORDER_RANDOM = "Random";

    private const string FINAL_NONE = "None";
    private const string FINAL_KEEP_BRA_PANTY_LAST = "Keep Underwear Last";
    private const string FINAL_PANTY_THEN_BRA = "Last: Panty -> Bra";
    private const string FINAL_BRA_THEN_PANTY = "Last: Bra -> Panty";

    private const string AUTO_OFF = "OFF";
    private const string AUTO_NEXT = "NEXT";
    private const string AUTO_PREV = "PREV";
    private const string AUTO_RANDOM = "RANDOM";
    private const string AUTO_LOOP = "LOOP";
    private const float AUTO_LOOP_FULLY_WORN_EXTRA_INTERVALS = 1.0f;

    private const string PHYS_OFF = "OFF";
    private const string PHYS_DOWN = "DOWN";
    private const string PHYS_UP = "UP";

    private JSONStorableStringChooser personChooser;
    private JSONStorableStringChooser orderModeChooser;
    private JSONStorableStringChooser finalRuleChooser;
    private JSONStorableStringChooser autoModeChooser;
    private JSONStorableStringChooser physRemoveStyleChooser;

    private JSONStorableBool protectBraJSON;
    private JSONStorableBool protectPantyJSON;
    private JSONStorableBool protectShoeSockJSON;
    private JSONStorableBool bottomForceDownJSON;
    private JSONStorableBool stateReportEnabledJSON;
    private JSONStorableBool debugLogJSON;
    private JSONStorableFloat pyReportPortJSON;
    private JSONStorableFloat autoIntervalJSON;
    private JSONStorableFloat physDurationJSON;
    private JSONStorableFloat fadeSecondsJSON;
    private JSONStorableString statusJSON;
    private JSONStorableString previewJSON;

    // 外部連携用 State。UIには出さず、Trigger/他Pluginから読めるようにRegisterだけする。
    private JSONStorableBool stateAllHiddenJSON;
    private JSONStorableBool stateAllVisibleJSON;
    private JSONStorableBool stateBraHiddenJSON;
    private JSONStorableBool stateBraVisibleJSON;
    private JSONStorableBool statePantyHiddenJSON;
    private JSONStorableBool statePantyVisibleJSON;
    private JSONStorableBool stateShoesHiddenJSON;
    private JSONStorableBool stateShoesVisibleJSON;
    private JSONStorableString stateLastActionJSON;
    private JSONStorableString stateLastItemJSON;
    private JSONStorableString stateLastCategoryJSON;
    private JSONStorableString stateProgressJSON;

    // v10d: State Report集計キャッシュ。
    // 毎回全衣装を走査せず、単品hide/wear時は差分更新する。
    private bool stateCacheValid = false;
    private int cacheTotalOperable = 0;
    private int cacheHiddenOperable = 0;
    private int cacheBraTotal = 0;
    private int cacheBraHidden = 0;
    private int cachePantyTotal = 0;
    private int cachePantyHidden = 0;
    private int cacheShoeTotal = 0;
    private int cacheShoeHidden = 0;
    private readonly Dictionary<string, bool> cacheHiddenByPrefix = new Dictionary<string, bool>();
    private readonly Dictionary<string, string> cacheCategoryByPrefix = new Dictionary<string, string>();
    private readonly HashSet<string> cacheProtectedPrefixes = new HashSet<string>();

    private Atom selectedPerson;
    private readonly List<ClothItem> clothes = new List<ClothItem>();
    private readonly List<ClothItem> orderedClothes = new List<ClothItem>();
    private readonly Dictionary<string, List<SavedBool>> scanStates = new Dictionary<string, List<SavedBool>>();
    private readonly Dictionary<string, List<SavedPhysBool>> scanPhysBoolStates = new Dictionary<string, List<SavedPhysBool>>();
    private readonly Dictionary<string, List<SavedPhysFloat>> scanPhysFloatStates = new Dictionary<string, List<SavedPhysFloat>>();
    private readonly Dictionary<string, List<SavedPhysFloat>> scanFadeFloatStates = new Dictionary<string, List<SavedPhysFloat>>();

    private float autoTimer = 0.0f;
    private bool autoLoopHideDirection = true;
    private int physicalRemoveRunning = 0;
    // v8o: hideMaterialが即時反映されない/Material参照が不安定な服でも、
    // NEXT/PREVが同じ服で止まらないようにする内部状態。
    private readonly HashSet<string> forcedHiddenPrefixes = new HashSet<string>();
    // v8s: PHTYで脱がせたSIM服はPREVで無理に戻さない。
    // 物理脱衣後のSIM服をPREVで復元しようとすると、Material参照やSim状態が壊れやすいため。
    private readonly HashSet<string> phtyHiddenPrefixes = new HashSet<string>();
    // v11d: safe Clothing Item checkbox handling.
    // Capture only currently-active DAZClothingItem displayName at SCAN time, then toggle only that exact item.
    // This avoids the v11b bug where selectable/not-worn clothing was enabled by fuzzy matching.
    private readonly Dictionary<string, string> activeDazClothingNameByPrefix = new Dictionary<string, string>();
    private readonly Dictionary<string, DAZClothingItem> activeDazClothingItemByPrefix = new Dictionary<string, DAZClothingItem>();
    private readonly HashSet<string> activeDazClothingOffByPlugin = new HashSet<string>();
    private int lastScanAccepted = 0;

    // v10i: Appearance Preset / Person内部Storable差し替え検知用。
    // 同じ衣装名・同じPrefixでも、内部Storableが差し替わった場合に
    // forcedHiddenPrefixes / phtyHiddenPrefixes / Alpha/Fade残りを掃除する。
    private string lastAppearanceSignature = "";
    private float appearanceWatchTimer = 0.0f;
    private Coroutine appearanceResetRoutine = null;

    private bool IsClothOperationBusy()
    {
        return wearNoneRoutine != null || physicalHideRoutine != null || restoreAllRoutine != null || physicalRemoveRunning > 0;
    }

    private Coroutine delayedScanRoutine = null;
    private Coroutine physicalHideRoutine = null;
    private Coroutine restoreAllRoutine = null;
    private Coroutine wearNoneRoutine = null;
    private int wearNoneCancelVersion = 0;
    private List<PhysBoolBackup> activePhysBoolBackups = null;
    private List<PhysFloatBackup> activePhysFloatBackups = null;
    private List<FadeParamRef> activeFadeRefs = null;

    private UIDynamicButton autoStopButton = null;
    private UIDynamicPopup autoModePopup = null;
    private UIDynamicButton nextButton = null;
    private UIDynamicButton prevButton = null;
    private UIDynamicButton restoreAllButton = null;
    private UIDynamicButton wearNoneButton = null;
    private UIDynamicButton removeBraButton = null;
    private UIDynamicButton removePantyButton = null;
    private UIDynamicButton scanButton = null;
    private UIDynamicButton resetRuntimeButton = null;

    private class SavedBool
    {
        public string StorableId;
        public string ParamName;
        public bool Value;
    }

    private class SavedPhysBool
    {
        public string StorableId;
        public string ParamName;
        public bool Value;
    }

    private class SavedPhysFloat
    {
        public string StorableId;
        public string ParamName;
        public float Value;
    }

    private class ClothItem
    {
        public string Prefix;
        public string Name;
        public List<SavedBool> Materials = new List<SavedBool>();
    }

    private class PhysBoolBackup
    {
        public JSONStorableBool Param;
        public bool Value;
        public string Name;
    }

    private class PhysFloatBackup
    {
        public JSONStorableFloat Param;
        public float Value;
        public string Name;
    }

    private class FadeParamRef
    {
        public JSONStorableFloat Param;
        public float StartValue;
        public float EndValue;
        public string Name;
    }

    public override void Init()
    {
        personChooser = new JSONStorableStringChooser(
            "person",
            new List<string>(),
            PERSON_NONE,
            "Person",
            OnPersonChanged
        );
        RegisterStringChooser(personChooser);
        UIDynamicPopup personPopup = CreateFilterablePopup(personChooser);
        if (personPopup != null && personPopup.popup != null)
            personPopup.popup.onOpenPopupHandlers += UpdatePersonChoices;

        orderModeChooser = new JSONStorableStringChooser(
            "Order Mode",
            new List<string> { ORDER_OUTER_FIRST, ORDER_TOP_FIRST, ORDER_BOTTOM_FIRST, ORDER_RANDOM },
            ORDER_OUTER_FIRST,
            "Order Mode",
            delegate(string value) { RebuildOrderOnly(); }
        );
        RegisterStringChooser(orderModeChooser);
        CreatePopup(orderModeChooser);

        // v10d UI: SCAN CLOTHをOrder Mode直下へ配置。
        scanButton = CreateButton("SCAN CLOTH");
        scanButton.button.onClick.AddListener(RequestScanClothes);

        finalRuleChooser = new JSONStorableStringChooser(
            "Last Clothing Rule",
            new List<string> { FINAL_NONE, FINAL_KEEP_BRA_PANTY_LAST, FINAL_PANTY_THEN_BRA, FINAL_BRA_THEN_PANTY },
            FINAL_BRA_THEN_PANTY,
            "Last Clothing Rule",
            delegate(string value) { RebuildOrderOnly(); }
        );
        RegisterStringChooser(finalRuleChooser);
        // UI layout: Last Clothing Rule is created later on the right side.

        protectBraJSON = new JSONStorableBool("Protect Bra", false, delegate(bool value) { stateCacheValid = false; UpdatePreview(); UpdateExternalState(); });
        RegisterBool(protectBraJSON);
        CreateToggle(protectBraJSON);

        protectPantyJSON = new JSONStorableBool("Protect Panty", false, delegate(bool value) { stateCacheValid = false; UpdatePreview(); UpdateExternalState(); });
        RegisterBool(protectPantyJSON);
        CreateToggle(protectPantyJSON);

        protectShoeSockJSON = new JSONStorableBool("Protect Shoes/Socks", false, delegate(bool value) { stateCacheValid = false; UpdatePreview(); UpdateExternalState(); });
        RegisterBool(protectShoeSockJSON);
        CreateToggle(protectShoeSockJSON);

        // Phys UI は右側の NEXT/PREV 直下に出す。
        // 初期値は DOWN。v9ではReloadを自動使用しない。PHTYはNEXT専用。
        physRemoveStyleChooser = new JSONStorableStringChooser(
            "PHTY",
            new List<string> { PHYS_OFF, PHYS_DOWN, PHYS_UP },
            PHYS_DOWN,
            "PHTY",
            delegate(string value) { UpdatePreview(); }
        );
        RegisterStringChooser(physRemoveStyleChooser);

        physDurationJSON = new JSONStorableFloat("PHTY Seconds", 6.0f, 0.2f, 10.0f, true, true);
        RegisterFloat(physDurationJSON);

        fadeSecondsJSON = new JSONStorableFloat("Fade Seconds", 1.0f, 0.1f, 10.0f, true, true);
        RegisterFloat(fadeSecondsJSON);

        bottomForceDownJSON = new JSONStorableBool("Bottom Force DOWN", true, delegate(bool value) { UpdatePreview(); });
        RegisterBool(bottomForceDownJSON);

        stateReportEnabledJSON = new JSONStorableBool("State Report to Py", true);
        RegisterBool(stateReportEnabledJSON);
        // UI layout: State Report to Py is created later on the right side.

        debugLogJSON = new JSONStorableBool("Debug Log", false);
        RegisterBool(debugLogJSON);
        // UI layout: Debug Log is created later on the right side.

        pyReportPortJSON = new JSONStorableFloat("State Report Port", 9999f, 1024f, 65535f, true, true);
        RegisterFloat(pyReportPortJSON);
        // UI layout: State Report Port is created later on the right side.

        RegisterExternalActions();
        RegisterExternalStates();

        autoModeChooser = new JSONStorableStringChooser(
            "Auto Mode",
            new List<string> { AUTO_OFF, AUTO_NEXT, AUTO_PREV, AUTO_RANDOM, AUTO_LOOP },
            AUTO_OFF,
            "Auto Mode",
            delegate(string value) { autoTimer = 0.0f; autoLoopHideDirection = true; UpdateButtonColors(); UpdatePreview(); }
        );
        RegisterStringChooser(autoModeChooser);

        autoIntervalJSON = new JSONStorableFloat("Auto Interval", 1.0f, 0.2f, 10.0f);
        RegisterFloat(autoIntervalJSON);

        // --------------------------------------------------
        // Left column: scan / report / timing / restore
        // --------------------------------------------------
        CreateToggle(stateReportEnabledJSON);
        CreateToggle(debugLogJSON);

        CreateSlider(physDurationJSON);
        CreateSlider(fadeSecondsJSON);
        CreateSlider(autoIntervalJSON);

        // --------------------------------------------------
        // Right column: wear/hide operations
        // --------------------------------------------------
        restoreAllButton = CreateButton("WEAR ALL", true);
        restoreAllButton.button.onClick.AddListener(RestoreAllVisible);

        prevButton = CreateButton("PREV +1 WEAR", true);
        prevButton.button.onClick.AddListener(PrevWear);

        nextButton = CreateButton("NEXT -1 HIDE", true);
        nextButton.button.onClick.AddListener(NextHide);

        wearNoneButton = CreateButton("WEAR NONE", true);
        wearNoneButton.button.onClick.AddListener(WearNone);

        removeBraButton = CreateButton("REMOVE BRA", true);
        removeBraButton.button.onClick.AddListener(RemoveBra);

        removePantyButton = CreateButton("REMOVE PANTY", true);
        removePantyButton.button.onClick.AddListener(RemovePanty);

        UIDynamicPopup physPopup = CreatePopup(physRemoveStyleChooser, true);
        if (physPopup != null)
            physPopup.height = 60.0f;

        autoModePopup = CreatePopup(autoModeChooser, true);

        autoStopButton = CreateButton("STOP / RESET AUTO", true);
        autoStopButton.button.onClick.AddListener(StopAuto);

        UIDynamicPopup finalPopup = CreatePopup(finalRuleChooser, true);
        if (finalPopup != null)
            finalPopup.height = 60.0f;

        CreateToggle(bottomForceDownJSON, true);

        statusJSON = new JSONStorableString("Status", "Ready");
        RegisterString(statusJSON);
        UIDynamicTextField statusField = CreateTextField(statusJSON);
        if (statusField != null)
            statusField.height = 60.0f;

        previewJSON = new JSONStorableString("Preview", "");
        RegisterString(previewJSON);
        UIDynamicTextField previewField = CreateTextField(previewJSON, true);
        if (previewField != null)
            previewField.height = 420.0f;

        UpdatePersonChoices();   // 最初に見つかったPersonを自動選択
        delayedScanRoutine = StartCoroutine(DelayedInitialScan()); // 服Storable生成待ち
        UpdateExternalState();

        UpdateButtonColors();
        DebugLog("ready / v11g self-person fixed / v11f safe active restore / SIM_RESET_ONLY");
    }

    private IEnumerator DelayedInitialScan()
    {
        yield return new WaitForSeconds(1.5f);

        // 起動直後にユーザーがWEAR NONEを押した場合、ここでSCANすると
        // 脱衣途中のhideMaterial/物理状態を初期状態として保存してしまう。
        // そのため衣装操作中なら初回遅延SCANは捨てる。
        if (IsClothOperationBusy())
        {
            DebugLog("[INITIAL SCAN] skipped because cloth operation busy");
            delayedScanRoutine = null;
            yield break;
        }

        UpdatePersonChoices();
        ScanClothes();
        UpdateExternalState();
        SetStatus("Initial delayed scan");
        delayedScanRoutine = null;
    }

    private string ComputeAppearanceSignature()
    {
        if (selectedPerson == null)
            return "no-person";

        int total = 0;
        int material = 0;
        int sim = 0;
        int itemControl = 0;
        int hash = 17;

        try
        {
            foreach (string id in selectedPerson.GetStorableIDs())
            {
                if (string.IsNullOrEmpty(id))
                    continue;

                total++;
                string lower = id.ToLowerInvariant();

                bool use = false;
                if (lower.IndexOf("material") >= 0)
                {
                    material++;
                    use = true;
                }
                else if (lower.IndexOf("sim") >= 0)
                {
                    sim++;
                    use = true;
                }
                else if (lower.IndexOf("itemcontrol") >= 0)
                {
                    itemControl++;
                    use = true;
                }

                if (!use)
                    continue;

                unchecked
                {
                    for (int i = 0; i < id.Length; i++)
                        hash = hash * 31 + id[i];
                }
            }
        }
        catch
        {
            return "error";
        }

        return "total=" + total.ToString(CultureInfo.InvariantCulture) +
            ";mat=" + material.ToString(CultureInfo.InvariantCulture) +
            ";sim=" + sim.ToString(CultureInfo.InvariantCulture) +
            ";ctrl=" + itemControl.ToString(CultureInfo.InvariantCulture) +
            ";hash=" + hash.ToString(CultureInfo.InvariantCulture);
    }

    private void WatchAppearancePresetChange()
    {
        if (selectedPerson == null)
            return;

        appearanceWatchTimer += Time.deltaTime;
        if (appearanceWatchTimer < 1.0f)
            return;

        appearanceWatchTimer = 0.0f;

        string now = ComputeAppearanceSignature();
        if (string.IsNullOrEmpty(lastAppearanceSignature))
        {
            lastAppearanceSignature = now;
            return;
        }

        if (now == lastAppearanceSignature)
            return;

        DebugLog("[APPEARANCE WATCH] changed / before=" + lastAppearanceSignature + " / after=" + now +
            " / busy=" + (IsClothOperationBusy() ? "1" : "0") +
            " / pluginOff=" + activeDazClothingOffByPlugin.Count.ToString(CultureInfo.InvariantCulture));

        // v11f: This plugin intentionally turns Clothing Item active OFF for detached clothes.
        // That removes Material/Sim storables, so the old appearance watcher mis-detected it as
        // an Appearance Preset change and rescanned zero clothes, destroying the restore list.
        // While we have plugin-OFF items, treat the signature change as our own state change.
        if (activeDazClothingOffByPlugin.Count > 0)
        {
            lastAppearanceSignature = now;
            DebugLog("[APPEARANCE WATCH] ignored plugin clothing-active OFF state / off=" + activeDazClothingOffByPlugin.Count.ToString(CultureInfo.InvariantCulture));
            return;
        }

        // WEAR NONE / WEAR ALL / PHTY中のSCANは、非表示中の状態を「初期状態」として保存してしまう。
        // その結果、WEAR ALLで物理/Fade復元対象が消えるため、操作中のAppearance Resetは実行しない。
        if (IsClothOperationBusy())
        {
            lastAppearanceSignature = now;
            DebugLog("[APPEARANCE WATCH] ignored while cloth operation busy");
            return;
        }

        lastAppearanceSignature = now;

        if (appearanceResetRoutine != null)
        {
            try { StopCoroutine(appearanceResetRoutine); } catch { }
            appearanceResetRoutine = null;
        }

        appearanceResetRoutine = StartCoroutine(AppearancePresetResetRoutine());
    }

    private IEnumerator AppearancePresetResetRoutine()
    {
        DebugLog("[APPEARANCE RESET] ENTER");

        if (IsClothOperationBusy())
        {
            DebugLog("[APPEARANCE RESET] ABORT busy / no scan");
            appearanceResetRoutine = null;
            yield break;
        }

        // v11f: Do not reset/scan while detached clothing is represented by real Clothing Item active OFF.
        // In that state VaM temporarily has no Material storables for those clothes; scanning would accept 0 clothes.
        if (activeDazClothingOffByPlugin.Count > 0)
        {
            DebugLog("[APPEARANCE RESET] ABORT plugin clothing-active OFF / no scan / off=" + activeDazClothingOffByPlugin.Count.ToString(CultureInfo.InvariantCulture));
            appearanceResetRoutine = null;
            yield break;
        }

        forcedHiddenPrefixes.Clear();
        phtyHiddenPrefixes.Clear();
        stateCacheValid = false;

        if (wearNoneRoutine != null)
        {
            DebugLog("[APPEARANCE RESET] cancel wearNoneRoutine");
            try { StopCoroutine(wearNoneRoutine); } catch { }
            wearNoneRoutine = null;
        }

        if (physicalHideRoutine != null || physicalRemoveRunning > 0)
        {
            DebugLog("[APPEARANCE RESET] stop active physical");
            StopActivePhysicalRoutine(true);
        }

        RestorePhysicalScanStateAll();
        RestoreFadeScanStateAll();
        ForceAlphaAdjustVisibleAll();

        // Appearance Presetロード直後はMaterial Storableが作り直されるため少し待つ。
        yield return new WaitForSeconds(0.8f);

        ScanClothes();

        RestorePhysicalScanStateAll();
        RestoreFadeScanStateAll();
        ForceAlphaAdjustVisibleAll();

        for (int i = 0; i < clothes.Count; i++)
            RestoreVisibleForItem(clothes[i], "APPEARANCE_RESET_RESTORE");

        ResetSimForAllVisibleItems("APPEARANCE_RESET_FINAL_SIM_RESET");

        RebuildOrderOnly();
        PublishClothState("appearance_reset", null, true);
        UpdateButtonColors();
        UpdatePreview();

        DebugLog("[APPEARANCE RESET] EXIT / clothes=" + clothes.Count.ToString(CultureInfo.InvariantCulture));
        appearanceResetRoutine = null;
    }

    private void RequestScanClothes()
    {
        StopAutoAndResetRunning("SCAN requested");

        if (delayedScanRoutine != null)
        {
            try { StopCoroutine(delayedScanRoutine); } catch { }
            delayedScanRoutine = null;
        }

        delayedScanRoutine = StartCoroutine(DelayedManualScan());
        SetStatus("SCAN requested");
        UpdateButtonColors();
    }

    private IEnumerator DelayedManualScan()
    {
        RestorePhysicalScanStateAll();
        RestoreFadeScanStateAll();

        // v11f: If this plugin previously turned Clothing Item active OFF, restore those exact
        // captured items before SCAN. Otherwise ScanClothes sees zero Material storables and loses the list.
        int activeRestoredBeforeScan = RestoreCapturedDazClothingActiveAll("MANUAL_SCAN_PRE_ACTIVE_RESTORE");
        if (activeRestoredBeforeScan > 0)
            yield return new WaitForSeconds(0.45f);

        yield return new WaitForSeconds(0.8f);

        UpdatePersonChoices();
        ScanClothes();
        UpdateExternalState();

        // Reload直後はMaterial Storableが一瞬0件になることがある。
        // 0件なら少し待って最大2回だけ再SCANする。
        if (lastScanAccepted == 0)
        {
            SetStatus("Manual scan retry 1");
            yield return new WaitForSeconds(1.2f);
            UpdatePersonChoices();
            ScanClothes();
            UpdateExternalState();
        }

        if (lastScanAccepted == 0)
        {
            SetStatus("Manual scan retry 2");
            yield return new WaitForSeconds(2.0f);
            UpdatePersonChoices();
            ScanClothes();
            UpdateExternalState();
        }

        delayedScanRoutine = null;
        SetStatus("Manual delayed scan / accepted=" + lastScanAccepted.ToString(CultureInfo.InvariantCulture));
    }

    private void RegisterExternalActions()
    {
        RegisterAction(new JSONStorableAction("Scan Cloth", RequestScanClothes));
        RegisterAction(new JSONStorableAction("Random Order", RandomOrder));
        RegisterAction(new JSONStorableAction("Auto Stop", StopAuto));

        RegisterAction(new JSONStorableAction("Next Hide", NextHide));
        RegisterAction(new JSONStorableAction("Prev Wear", PrevWear));
        RegisterAction(new JSONStorableAction("Wear None", WearNone));
        RegisterAction(new JSONStorableAction("Remove Bra", RemoveBra));
        RegisterAction(new JSONStorableAction("Remove Panty", RemovePanty));

        RegisterAction(new JSONStorableAction("Restore All", RestoreAllVisible));
        RegisterAction(new JSONStorableAction("Reset Runtime", ResetRuntime));

        RegisterAction(new JSONStorableAction("Auto Next", StartAutoNext));
        RegisterAction(new JSONStorableAction("Auto Prev", StartAutoPrev));
        RegisterAction(new JSONStorableAction("Auto Random", StartAutoRandom));
        RegisterAction(new JSONStorableAction("Auto Loop", StartAutoLoop));

        RegisterAction(new JSONStorableAction("Set Phys OFF", SetPhysOff));
        RegisterAction(new JSONStorableAction("Set Phys DOWN", SetPhysDown));
        RegisterAction(new JSONStorableAction("Set Phys UP", SetPhysUp));
        RegisterAction(new JSONStorableAction("Phys Remove Next", NextHide));
    }

    private void RegisterExternalStates()
    {
        stateAllHiddenJSON = new JSONStorableBool("State All Hidden", false);
        stateAllVisibleJSON = new JSONStorableBool("State All Visible", false);
        stateBraHiddenJSON = new JSONStorableBool("State Bra Hidden", false);
        stateBraVisibleJSON = new JSONStorableBool("State Bra Visible", false);
        statePantyHiddenJSON = new JSONStorableBool("State Panty Hidden", false);
        statePantyVisibleJSON = new JSONStorableBool("State Panty Visible", false);
        stateShoesHiddenJSON = new JSONStorableBool("State Shoes Hidden", false);
        stateShoesVisibleJSON = new JSONStorableBool("State Shoes Visible", false);

        RegisterBool(stateAllHiddenJSON);
        RegisterBool(stateAllVisibleJSON);
        RegisterBool(stateBraHiddenJSON);
        RegisterBool(stateBraVisibleJSON);
        RegisterBool(statePantyHiddenJSON);
        RegisterBool(statePantyVisibleJSON);
        RegisterBool(stateShoesHiddenJSON);
        RegisterBool(stateShoesVisibleJSON);

        stateLastActionJSON = new JSONStorableString("State Last Action", "");
        stateLastItemJSON = new JSONStorableString("State Last Item", "");
        stateLastCategoryJSON = new JSONStorableString("State Last Category", "");
        stateProgressJSON = new JSONStorableString("State Progress", "0/0");

        RegisterString(stateLastActionJSON);
        RegisterString(stateLastItemJSON);
        RegisterString(stateLastCategoryJSON);
        RegisterString(stateProgressJSON);
    }

    private void StartAutoNext()
    {
        SetAutoMode(AUTO_NEXT);
    }

    private void StartAutoPrev()
    {
        SetAutoMode(AUTO_PREV);
    }

    private void StartAutoRandom()
    {
        SetAutoMode(AUTO_RANDOM);
    }

    private void StartAutoLoop()
    {
        SetAutoMode(AUTO_LOOP);
    }

    private void SetPhysOff()
    {
        SetPhysMode(PHYS_OFF);
    }

    private void SetPhysDown()
    {
        SetPhysMode(PHYS_DOWN);
    }

    private void SetPhysUp()
    {
        SetPhysMode(PHYS_UP);
    }

    private void SetPhysMode(string mode)
    {
        if (physRemoveStyleChooser == null)
            return;

        physRemoveStyleChooser.val = mode;
        SetStatus("Phys Remove Style: " + mode);
        UpdatePreview();
    }

    private void SetAutoMode(string mode)
    {
        if (autoModeChooser == null)
            return;

        autoModeChooser.val = mode;
        autoTimer = 0.0f;
        autoLoopHideDirection = true;
        SetStatus("Auto mode: " + mode);
        PublishClothState("auto", null, false);
        UpdateButtonColors();
        UpdatePreview();
    }

    public void Update()
    {
        WatchAppearancePresetChange();

        if (autoModeChooser == null || autoModeChooser.val == AUTO_OFF)
            return;

        autoTimer += Time.deltaTime;

        // Auto Interval は「次の開始タイミング」。PHTY中は次へ進めないが、
        // タイマー自体は進めておき、PHTY完了後に必要なら即次へ進む。
        float interval = autoIntervalJSON != null ? Mathf.Max(0.2f, autoIntervalJSON.val) : 1.0f;

        if (autoTimer < interval)
            return;

        if (physicalRemoveRunning > 0)
            return;

        autoTimer = 0.0f;

        if (autoModeChooser.val == AUTO_NEXT)
            NextHide();
        else if (autoModeChooser.val == AUTO_PREV)
            PrevWear();
        else if (autoModeChooser.val == AUTO_RANDOM)
            AutoRandomStep();
        else if (autoModeChooser.val == AUTO_LOOP)
            AutoLoopStep();
    }

    private void StopAuto()
    {
        LogButtonPressed("STOP / RESET AUTO");
        StopAutoAndResetRunning("Auto stopped");
        PublishClothState("auto_stop", null, true);
        UpdatePreview();
    }

    private void ResetRuntime()
    {
        StopAutoAndResetRunning("Runtime reset");
        PublishClothState("runtime_reset", null, true);
        UpdatePreview();
    }

    private void StopAutoAndResetRunning(string reason)
    {
        if (autoModeChooser != null)
            autoModeChooser.val = AUTO_OFF;

        autoTimer = 0.0f;
        autoLoopHideDirection = true;
        if (restoreAllRoutine != null)
        {
            try { StopCoroutine(restoreAllRoutine); } catch { }
            restoreAllRoutine = null;
        }

        if (wearNoneRoutine != null)
        {
            wearNoneCancelVersion++;
            DebugLog("[RESTORE CALLER] cancel wearNoneRoutine / cancelVersion=" + wearNoneCancelVersion.ToString(CultureInfo.InvariantCulture));
            try { StopCoroutine(wearNoneRoutine); } catch { }
            wearNoneRoutine = null;
        }

        StopActivePhysicalRoutine(true);
        physicalRemoveRunning = 0;
        SetStatus(reason);
        UpdateButtonColors();
    }

    private void StopActivePhysicalRoutine(bool restoreValues)
    {
        DebugLog("[STOP PHTY] ENTER restoreValues=" + (restoreValues ? "1" : "0") +
            " / wearNoneRoutine=" + (wearNoneRoutine != null ? "1" : "0") +
            " / physicalRoutine=" + (physicalHideRoutine != null ? "1" : "0") +
            " / busy=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture) +
            " / activeBool=" + (activePhysBoolBackups != null ? activePhysBoolBackups.Count.ToString(CultureInfo.InvariantCulture) : "null") +
            " / activeFloat=" + (activePhysFloatBackups != null ? activePhysFloatBackups.Count.ToString(CultureInfo.InvariantCulture) : "null") +
            " / activeFade=" + (activeFadeRefs != null ? activeFadeRefs.Count.ToString(CultureInfo.InvariantCulture) : "null"));

        if (physicalHideRoutine != null)
        {
            try { StopCoroutine(physicalHideRoutine); } catch { }
            physicalHideRoutine = null;
        }

        if (restoreValues)
        {
            RestorePhysicalSettings(activePhysBoolBackups, activePhysFloatBackups);
            RestoreFadeSettings(activeFadeRefs);
            RestorePhysicalScanStateAll();
            RestoreFadeScanStateAll();
            ForceAlphaAdjustVisibleAll();
        }

        activePhysBoolBackups = null;
        activePhysFloatBackups = null;
        activeFadeRefs = null;
        physicalRemoveRunning = 0;
    }

    private void AutoRandomStep()
    {
        if (physicalRemoveRunning > 0)
            return;

        // RANDOMは「順番を再抽選して、次の1枚を脱ぐ」。
        // ただし脱げる服が無ければ自動停止。
        RandomOrderNoStatus();

        if (!HasNextHideCandidate())
        {
            StopAuto();
            SetStatus("Auto Random stopped: no visible cloth");
            return;
        }

        NextHide();
    }

    private void AutoLoopStep()
    {
        if (physicalRemoveRunning > 0)
            return;

        // LOOPは「全部着ている → 順番に脱ぐ → 全裸でAuto Interval待機 → 順番に着る → 全着衣でAuto Interval待機」を繰り返す。
        if (orderedClothes.Count == 0)
        {
            StopAuto();
            SetStatus("Auto Loop stopped: no cloth list");
            return;
        }

        if (autoLoopHideDirection)
        {
            if (HasNextHideCandidate())
            {
                NextHide();
                return;
            }

            // 全部脱ぎ終わった。ここで即着始めず、Auto Intervalぶん全裸状態を維持する。
            autoLoopHideDirection = false;
            autoTimer = 0.0f;
            if (IsDebugLogEnabled())
                SetStatus("Auto Loop turn: wear");
            else if (statusJSON != null)
                statusJSON.val = "Auto Loop wait: wear";
            UpdateButtonColors();
            UpdatePreview();
            return;
        }

        if (HasPrevWearCandidate())
        {
            PrevWear();
            return;
        }

        // 全部着終わった。ここで即脱ぎ始めず、Auto Intervalぶん全着衣状態を維持する。
        autoLoopHideDirection = true;
        autoTimer = -GetAutoLoopFullyWornExtraWaitSeconds();
        if (IsDebugLogEnabled())
            SetStatus("Auto Loop turn: hide");
        else if (statusJSON != null)
            statusJSON.val = "Auto Loop wait: hide";
        UpdateButtonColors();
        UpdatePreview();
        return;
    }

    private void ResetAutoTimerAfterStep()
    {
        if (autoModeChooser != null && autoModeChooser.val != AUTO_OFF)
            autoTimer = 0.0f;
    }

    private float GetAutoLoopFullyWornExtraWaitSeconds()
    {
        float interval = autoIntervalJSON != null ? Mathf.Max(0.2f, autoIntervalJSON.val) : 1.0f;
        return interval * AUTO_LOOP_FULLY_WORN_EXTRA_INTERVALS;
    }

    private void SetStatus(string text)
    {
        if (statusJSON != null)
            statusJSON.val = text;

        if (ShouldLogStatus(text))
            SuperController.LogMessage("[ClothStateSwitcher] " + text);
    }

    private bool IsDebugLogEnabled()
    {
        return debugLogJSON != null && debugLogJSON.val;
    }

    private bool ShouldLogStatus(string text)
    {
        return IsDebugLogEnabled();
    }

    private void LogAlways(string text)
    {
        DebugLog(text);
    }

    private void LogScanListAlways(string tag)
    {
        LogAlways("[" + tag + "] count=" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture));

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
            {
                LogAlways("[" + tag + "] " + (i + 1).ToString("00") + " (null)");
                continue;
            }

            string sim = IsSimCloth(item) ? "[SIM]" : "[MAT]";
            string cat = GetClothCategory(item);
            string hidden = IsHidden(item) ? "[DONE]" : "[VISIBLE]";
            string protect = IsProtected(item) ? "[LOCK]" : "";

            LogAlways("[" + tag + "] " +
                (i + 1).ToString("00") + "/" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) + " " +
                sim + " " + hidden + " " + protect + " " +
                item.Name + " / cat=" + cat + " / prefix=" + item.Prefix);
        }
    }


    private void LogButtonPressed(string buttonName)
    {
        string autoMode = autoModeChooser != null ? autoModeChooser.val : "";
        string physMode = physRemoveStyleChooser != null ? physRemoveStyleChooser.val : "";
        string progress = stateProgressJSON != null ? stateProgressJSON.val : "";
        DebugLog("[BUTTON] " + buttonName +
            " / auto=" + autoMode +
            " / phty=" + physMode +
            " / busy=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture) +
            " / ordered=" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) +
            " / clothes=" + clothes.Count.ToString(CultureInfo.InvariantCulture) +
            " / progress=" + progress);

        DumpIndexState("[INDEX " + buttonName + "]");
    }

    private void DumpIndexState(string label)
    {
        if (orderedClothes == null)
        {
            DebugLog(label + " orderedClothes=null");
            return;
        }

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
            {
                DebugLog(label + " i=" + (i + 1).ToString("00") + " item=null");
                continue;
            }

            int validMat = CountValidMaterials(item);
            int materialCount = item.Materials != null ? item.Materials.Count : 0;
            bool hidden = IsHidden(item);
            bool anyHidden = HasAnyHiddenMaterial(item);
            bool protect = IsProtected(item);
            bool sim = IsSimCloth(item);
            string category = GetClothCategory(item);

            DebugLog(label +
                " i=" + (i + 1).ToString("00") + "/" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) +
                " name=" + item.Name +
                " prefix=" + item.Prefix +
                " cat=" + category +
                " hidden=" + (hidden ? "1" : "0") +
                " anyHidden=" + (anyHidden ? "1" : "0") +
                " protect=" + (protect ? "1" : "0") +
                " sim=" + (sim ? "1" : "0") +
                " mat=" + validMat.ToString(CultureInfo.InvariantCulture) + "/" + materialCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private int CountValidMaterials(ClothItem item)
    {
        return CountValidMaterials(item, true);
    }

    private int CountValidMaterials(ClothItem item, bool autoRefresh)
    {
        if (selectedPerson == null || item == null || item.Materials == null)
            return 0;

        int count = CountValidMaterialsNoRefresh(item);
        if (count == 0 && autoRefresh)
        {
            int refreshed = RefreshItemMaterials(item);
            if (refreshed > 0)
                count = CountValidMaterialsNoRefresh(item);
        }
        return count;
    }

    private int CountValidMaterialsNoRefresh(ClothItem item)
    {
        if (selectedPerson == null || item == null || item.Materials == null)
            return 0;

        int count = 0;
        for (int i = 0; i < item.Materials.Count; i++)
        {
            SavedBool material = item.Materials[i];
            if (material == null)
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(material.StorableId);
            if (storable == null)
                continue;

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam(material.ParamName);
            if (hideMaterial != null)
                count++;
        }
        return count;
    }

    private int RefreshItemMaterials(ClothItem item)
    {
        if (selectedPerson == null || item == null || string.IsNullOrEmpty(item.Prefix))
            return 0;

        List<SavedBool> fresh = new List<SavedBool>();
        string targetPrefix = NormalizeWearablePrefix(item.Prefix);

        foreach (string storableId in selectedPerson.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            string lower = storableId.ToLowerInvariant();
            if (IsIgnoredClothStorable(storableId) || LooksLikeHairOrFaceItem(lower) || LooksLikeAccessoryItem(lower))
                continue;
            if (lower.IndexOf("material") < 0)
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(storableId);
            if (storable == null)
                continue;

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam("hideMaterial");
            if (hideMaterial == null)
                continue;

            string prefix = GetWearablePrefix(storableId);
            if (string.IsNullOrEmpty(prefix))
                continue;

            if (prefix != targetPrefix)
                continue;

            SavedBool saved = new SavedBool();
            saved.StorableId = storableId;
            saved.ParamName = "hideMaterial";
            saved.Value = hideMaterial.val;
            fresh.Add(saved);
        }

        if (fresh.Count <= 0)
            return 0;

        int before = item.Materials != null ? item.Materials.Count : 0;
        item.Materials = fresh;
        DebugLog("[REFRESH MATERIALS] " + item.Name + " before=" + before.ToString(CultureInfo.InvariantCulture) + " after=" + fresh.Count.ToString(CultureInfo.InvariantCulture));
        return fresh.Count;
    }

    private Atom GetSelfPersonAtom()
    {
        try
        {
            if (containingAtom != null && containingAtom.type == "Person")
                return containingAtom;
        }
        catch { }

        return null;
    }

    private void UpdatePersonChoices()
    {
        if (personChooser == null)
            return;

        // v11g: This plugin is intended to operate only on its own Person.
        // Do not auto-select another Person, because clothing active OFF/ON is dangerous
        // when the target Person is ambiguous or changes after scan.
        Atom self = GetSelfPersonAtom();
        string selfUid = self != null ? self.uid : PERSON_NONE;

        List<string> choices = new List<string>();
        choices.Add(selfUid);
        personChooser.choices = choices;

        if (personChooser.val != selfUid)
            personChooser.val = selfUid;

        OnPersonChanged(selfUid);
    }

    private void OnPersonChanged(string uid)
    {
        // v11g: Fixed to self. Ignore manually supplied uid and always bind to containingAtom.
        selectedPerson = GetSelfPersonAtom();

        if (personChooser != null)
        {
            string selfUid = selectedPerson != null ? selectedPerson.uid : PERSON_NONE;
            if (personChooser.choices == null || personChooser.choices.Count != 1 || personChooser.choices[0] != selfUid)
                personChooser.choices = new List<string> { selfUid };
            if (personChooser.val != selfUid)
                personChooser.val = selfUid;
        }

        // Person自体を切り替えた場合は、次回SCAN後の状態を基準にする。
        lastAppearanceSignature = ComputeAppearanceSignature();
    }

    private void ScanClothes()
    {
        DebugLog("[BUTTON] SCAN CLOTH BODY / begin");
        clothes.Clear();
        orderedClothes.Clear();
        stateCacheValid = false;
        scanStates.Clear();
        scanPhysBoolStates.Clear();
        scanPhysFloatStates.Clear();
        scanFadeFloatStates.Clear();
        forcedHiddenPrefixes.Clear();
        phtyHiddenPrefixes.Clear();
        activeDazClothingNameByPrefix.Clear();
        activeDazClothingItemByPrefix.Clear();
        activeDazClothingOffByPlugin.Clear();

        if (selectedPerson == null)
        {
            lastScanAccepted = 0;
            SetStatus("No Person");
            UpdatePreview();
            return;
        }

        // v8k:
        // 以前は ItemControl/Sim/Wrap などから Prefix を推定してから Material を探していた。
        // VaMのロード順やReload直後だと Prefix は拾えても hideMaterial 付きMaterialに結びつかず、
        // prefix=14 accepted=0 のような状態になりやすい。
        // そのため、まず hideMaterial を持つ Material Storable を直接走査して服を確定する。
        List<string> prefixCandidates = FindWearablePrefixes();
        if (IsDebugLogEnabled())
        {
            DebugLog("[SCAN TRACE][PREFIX_CANDIDATES] count=" + prefixCandidates.Count.ToString(CultureInfo.InvariantCulture));
            for (int pi = 0; pi < prefixCandidates.Count; pi++)
                DebugLog("[SCAN TRACE][PREFIX_CANDIDATE] " + (pi + 1).ToString("00") + "/" + prefixCandidates.Count.ToString(CultureInfo.InvariantCulture) + " prefix=" + prefixCandidates[pi]);
        }

        Dictionary<string, ClothItem> map = new Dictionary<string, ClothItem>();

        int storableCount = 0;
        int materialCandidates = 0;
        int hideMaterialFound = 0;
        int ignored = 0;

        foreach (string storableId in selectedPerson.GetStorableIDs())
        {
            storableCount++;

            if (string.IsNullOrEmpty(storableId))
            {
                DebugLog("[SCAN TRACE][SKIP][EMPTY_STORABLE_ID]");
                continue;
            }

            string lower = storableId.ToLowerInvariant();
            bool realClothName = LooksLikeRealClothItem(lower);

            DebugLog("[SCAN TRACE][CHECK] storable=" + storableId + " realCloth=" + (realClothName ? "1" : "0"));

            if (IsIgnoredClothStorable(storableId))
            {
                DebugLog("[SCAN TRACE][IGNORE][IGNORED_STORABLE] storable=" + storableId);
                ignored++;
                continue;
            }

            // Skirt/Shirt/Nightwear等、明らかに衣装名を含むものは、ear/ring等の短い語で誤除外しない。
            if (!realClothName)
            {
                string hairFaceReason = GetHairFaceReason(lower);
                if (!string.IsNullOrEmpty(hairFaceReason))
                {
                    DebugLog("[SCAN TRACE][IGNORE][HAIR_FACE][" + hairFaceReason + "] storable=" + storableId);
                    ignored++;
                    continue;
                }

                string accessoryReason = GetAccessoryReason(lower);
                if (!string.IsNullOrEmpty(accessoryReason))
                {
                    DebugLog("[SCAN TRACE][IGNORE][ACCESSORY][" + accessoryReason + "] storable=" + storableId);
                    ignored++;
                    continue;
                }
            }
            else
            {
                DebugLog("[SCAN TRACE][PASS_REAL_CLOTH] storable=" + storableId);
            }

            if (lower.IndexOf("material") < 0)
                continue;

            materialCandidates++;
            DebugLog("[SCAN TRACE][MATERIAL_CANDIDATE] storable=" + storableId);

            JSONStorable storable = selectedPerson.GetStorableByID(storableId);
            if (storable == null)
            {
                DebugLog("[SCAN TRACE][SKIP][STORABLE_NULL] storable=" + storableId);
                continue;
            }

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam("hideMaterial");
            if (hideMaterial == null)
            {
                DebugLog("[SCAN TRACE][SKIP][NO_HIDEMATERIAL] storable=" + storableId);
                continue;
            }

            hideMaterialFound++;

            string prefix = GetWearablePrefix(storableId);
            if (string.IsNullOrEmpty(prefix))
            {
                DebugLog("[SCAN TRACE][SKIP][NO_PREFIX] storable=" + storableId);
                continue;
            }

            ClothItem item;
            if (!map.TryGetValue(prefix, out item) || item == null)
            {
                item = new ClothItem();
                item.Prefix = prefix;
                item.Name = CleanupName(prefix);
                map[prefix] = item;
                DebugLog("[SCAN TRACE][MAP_NEW] prefix=" + prefix + " name=" + item.Name);
            }

            SavedBool saved = new SavedBool();
            saved.StorableId = storableId;
            saved.ParamName = "hideMaterial";
            saved.Value = hideMaterial.val;
            item.Materials.Add(saved);
            DebugLog("[SCAN TRACE][ACCEPT_MATERIAL] prefix=" + prefix + " hide=" + (hideMaterial.val ? "1" : "0") + " storable=" + storableId);
        }

        // 保険: 直接Material走査で漏れたものだけ、従来のPrefix→Material探索も試す。
        for (int i = 0; i < prefixCandidates.Count; i++)
        {
            string prefix = prefixCandidates[i];
            if (string.IsNullOrEmpty(prefix))
            {
                DebugLog("[SCAN TRACE][FALLBACK_SKIP][EMPTY_PREFIX]");
                continue;
            }

            if (map.ContainsKey(prefix))
            {
                DebugLog("[SCAN TRACE][FALLBACK_SKIP][ALREADY_MAPPED] prefix=" + prefix);
                continue;
            }

            DebugLog("[SCAN TRACE][FALLBACK_CHECK] prefix=" + prefix);
            ClothItem fallback = BuildClothItem(prefix);
            if (fallback != null && fallback.Materials.Count > 0)
            {
                map[prefix] = fallback;
                DebugLog("[SCAN TRACE][FALLBACK_ACCEPT] prefix=" + fallback.Prefix + " name=" + fallback.Name + " materials=" + fallback.Materials.Count.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                DebugLog("[SCAN TRACE][FALLBACK_REJECT][NO_MATERIALS] prefix=" + prefix);
            }
        }

        List<string> keys = new List<string>(map.Keys);
        keys.Sort();

        for (int i = 0; i < keys.Count; i++)
        {
            ClothItem item = map[keys[i]];
            if (item == null)
            {
                DebugLog("[SCAN TRACE][FINAL_REJECT][ITEM_NULL] key=" + keys[i]);
                continue;
            }

            if (item.Materials.Count == 0)
            {
                DebugLog("[SCAN TRACE][FINAL_REJECT][NO_MATERIALS] prefix=" + item.Prefix + " name=" + item.Name);
                continue;
            }

            clothes.Add(item);
            DebugLog("[SCAN TRACE][FINAL_ACCEPT] " + (clothes.Count).ToString("00") + " prefix=" + item.Prefix + " name=" + item.Name + " materials=" + item.Materials.Count.ToString(CultureInfo.InvariantCulture));

            List<SavedBool> savedList = new List<SavedBool>();
            for (int m = 0; m < item.Materials.Count; m++)
            {
                SavedBool src = item.Materials[m];
                SavedBool saved = new SavedBool();
                saved.StorableId = src.StorableId;
                saved.ParamName = src.ParamName;
                saved.Value = src.Value;
                savedList.Add(saved);
            }

            scanStates[item.Prefix] = savedList;
            CapturePhysicalScanState(item);
            CaptureFadeScanState(item);
            CaptureActiveDazClothingNameAtScan(item);
        }

        lastScanAccepted = clothes.Count;
        RebuildOrderOnly();

        string scanLog =
            "Scanned: " + clothes.Count.ToString(CultureInfo.InvariantCulture) +
            " clothes / prefix=" + prefixCandidates.Count.ToString(CultureInfo.InvariantCulture) +
            " / material=" + materialCandidates.ToString(CultureInfo.InvariantCulture) +
            " / hideMat=" + hideMaterialFound.ToString(CultureInfo.InvariantCulture) +
            " / ignored=" + ignored.ToString(CultureInfo.InvariantCulture);

        SetStatus(scanLog);
        DebugLog("[SCAN] storable=" + storableCount.ToString(CultureInfo.InvariantCulture) +
            " prefix=" + prefixCandidates.Count.ToString(CultureInfo.InvariantCulture) +
            " accepted=" + clothes.Count.ToString(CultureInfo.InvariantCulture) +
            " material=" + materialCandidates.ToString(CultureInfo.InvariantCulture) +
            " hideMat=" + hideMaterialFound.ToString(CultureInfo.InvariantCulture) +
            " ignored=" + ignored.ToString(CultureInfo.InvariantCulture));

        LogScanListAlways("SCAN LIST");

        lastAppearanceSignature = ComputeAppearanceSignature();
        DebugLog("[APPEARANCE WATCH] scan baseline signature=" + lastAppearanceSignature);

        PublishClothState("scan", null, false);
    }

    private ClothItem BuildClothItem(string prefix)
    {
        if (selectedPerson == null || string.IsNullOrEmpty(prefix))
            return null;

        ClothItem item = new ClothItem();
        item.Prefix = prefix;
        item.Name = CleanupName(prefix);

        foreach (string storableId in selectedPerson.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            string lower = storableId.ToLowerInvariant();
            if (lower.IndexOf("material") >= 0 && lower.StartsWith(prefix.ToLowerInvariant()))
                DebugLog("[SCAN TRACE][FALLBACK_MATERIAL_CHECK] prefix=" + prefix + " storable=" + storableId);

            if (!LooksLikeRelatedMaterial(storableId, prefix))
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(storableId);
            if (storable == null)
            {
                DebugLog("[SCAN TRACE][FALLBACK_SKIP][STORABLE_NULL] prefix=" + prefix + " storable=" + storableId);
                continue;
            }

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam("hideMaterial");
            if (hideMaterial == null)
            {
                DebugLog("[SCAN TRACE][FALLBACK_SKIP][NO_HIDEMATERIAL] prefix=" + prefix + " storable=" + storableId);
                continue;
            }

            SavedBool saved = new SavedBool();
            saved.StorableId = storableId;
            saved.ParamName = "hideMaterial";
            saved.Value = hideMaterial.val;
            item.Materials.Add(saved);
            DebugLog("[SCAN TRACE][FALLBACK_ACCEPT_MATERIAL] prefix=" + prefix + " hide=" + (hideMaterial.val ? "1" : "0") + " storable=" + storableId);
        }

        return item;
    }

    private string CleanupName(string prefix)
    {
        if (string.IsNullOrEmpty(prefix))
            return "cloth";

        string s = prefix;
        s = s.Replace(":", " ");
        s = s.Replace("_", " ");
        s = s.Replace("-", " ");
        s = s.Trim();

        if (s.Length == 0)
            return prefix;

        return s;
    }

    private void RebuildOrderOnly()
    {
        orderedClothes.Clear();

        for (int i = 0; i < clothes.Count; i++)
            orderedClothes.Add(clothes[i]);

        ApplyOrderMode();
        ApplyFinalRule();
        UpdatePreview();
    }

    private void ApplyOrderMode()
    {
        if (orderModeChooser == null)
            return;

        string mode = orderModeChooser.val;

        if (mode == ORDER_RANDOM)
        {
            ShuffleList(orderedClothes);
            return;
        }

        orderedClothes.Sort(delegate(ClothItem a, ClothItem b)
        {
            int ca = GetOrderCategory(a, mode);
            int cb = GetOrderCategory(b, mode);
            if (ca != cb)
                return ca.CompareTo(cb);

            return string.Compare(a.Name, b.Name);
        });
    }

    private int GetOrderCategory(ClothItem item, string mode)
    {
        string s = GetLowerName(item);

        if (mode == ORDER_BOTTOM_FIRST)
        {
            if (IsShoeSock(s)) return 0;
            if (IsBottom(s)) return 1;
            if (IsPanty(s)) return 2;
            if (IsOuter(s)) return 3;
            if (IsTop(s)) return 4;
            if (IsBra(s)) return 5;
            return 6;
        }

        if (mode == ORDER_TOP_FIRST)
        {
            if (IsOuter(s)) return 0;
            if (IsTop(s)) return 1;
            if (IsBra(s)) return 2;
            if (IsBottom(s)) return 3;
            if (IsPanty(s)) return 4;
            if (IsShoeSock(s)) return 5;
            return 6;
        }

        // Outer First
        if (IsOuter(s)) return 0;
        if (IsTop(s)) return 1;
        if (IsDress(s)) return 2;
        if (IsBottom(s)) return 3;
        if (IsShoeSock(s)) return 4;
        if (IsBra(s)) return 5;
        if (IsPanty(s)) return 6;
        return 7;
    }

    private void ApplyFinalRule()
    {
        if (finalRuleChooser == null || finalRuleChooser.val == FINAL_NONE)
            return;

        List<ClothItem> bra = new List<ClothItem>();
        List<ClothItem> panty = new List<ClothItem>();
        List<ClothItem> others = new List<ClothItem>();

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            string s = GetLowerName(item);

            if (IsBra(s))
                bra.Add(item);
            else if (IsPanty(s))
                panty.Add(item);
            else
                others.Add(item);
        }

        orderedClothes.Clear();
        orderedClothes.AddRange(others);

        string rule = finalRuleChooser.val;

        if (rule == FINAL_KEEP_BRA_PANTY_LAST)
        {
            for (int i = 0; i < clothes.Count; i++)
            {
                ClothItem item = clothes[i];
                if (bra.Contains(item) || panty.Contains(item))
                    orderedClothes.Add(item);
            }
        }
        else if (rule == FINAL_PANTY_THEN_BRA)
        {
            orderedClothes.AddRange(panty);
            orderedClothes.AddRange(bra);
        }
        else if (rule == FINAL_BRA_THEN_PANTY)
        {
            orderedClothes.AddRange(bra);
            orderedClothes.AddRange(panty);
        }
    }

    private void RandomOrder()
    {
        LogButtonPressed("RANDOM ORDER");
        RandomOrderNoStatus();
        SetStatus("Random order");
        PublishClothState("random_order", null, false);
    }

    private void RandomOrderNoStatus()
    {
        ShuffleList(orderedClothes);
        ApplyFinalRule();
        UpdatePreview();
    }

    private void NextHide()
    {
        LogButtonPressed("NEXT -1 HIDE");
        if (UsePhysicalRemove() && physicalRemoveRunning > 0)
        {
            SetStatus("Physical remove is running");
            UpdatePreview();
            return;
        }

        if (orderedClothes.Count == 0)
        {
            SetStatus("No cloth list");
            StopAutoIfMode(AUTO_NEXT);
            return;
        }

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
            {
                DebugLog("[NEXT CHECK] i=" + (i + 1).ToString("00") + "/" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) + " item=null skip=1 reason=null");
                continue;
            }

            int validMat = CountValidMaterials(item);
            int materialCount = item.Materials != null ? item.Materials.Count : 0;
            bool protect = IsProtected(item);
            bool hidden = IsHidden(item);
            bool anyHidden = HasAnyHiddenMaterial(item);
            bool sim = IsSimCloth(item);
            string category = GetClothCategory(item);

            DebugLog("[NEXT CHECK] i=" + (i + 1).ToString("00") + "/" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) +
                " name=" + item.Name +
                " prefix=" + item.Prefix +
                " cat=" + category +
                " hidden=" + (hidden ? "1" : "0") +
                " anyHidden=" + (anyHidden ? "1" : "0") +
                " protect=" + (protect ? "1" : "0") +
                " sim=" + (sim ? "1" : "0") +
                " mat=" + validMat.ToString(CultureInfo.InvariantCulture) + "/" + materialCount.ToString(CultureInfo.InvariantCulture));

            if (protect)
            {
                DebugLog("[NEXT SKIP] i=" + (i + 1).ToString("00") + " name=" + item.Name + " reason=protect");
                continue;
            }

            if (validMat <= 0)
            {
                DebugLog("[NEXT SKIP] i=" + (i + 1).ToString("00") + " name=" + item.Name + " reason=no-valid-material");
                continue;
            }

            if (hidden)
            {
                DebugLog("[NEXT SKIP] i=" + (i + 1).ToString("00") + " name=" + item.Name + " reason=hidden");
                continue;
            }

            DebugLog("[NEXT SELECT] i=" + (i + 1).ToString("00") + "/" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture) +
                " name=" + item.Name +
                " prefix=" + item.Prefix +
                " phty=" + (physRemoveStyleChooser != null ? physRemoveStyleChooser.val : "") +
                " sim=" + (sim ? "1" : "0") +
                " mat=" + validMat.ToString(CultureInfo.InvariantCulture) + "/" + materialCount.ToString(CultureInfo.InvariantCulture));

            int prepFound = PrepareVisualStateForAction(item, "NEXT_PREP", false);
            if (prepFound <= 0)
            {
                SetStatus("HIDE skipped: prep material refs not found / press SCAN CLOTH: " + item.Name);
                StopAutoIfMode(AUTO_NEXT);
                StopAutoIfMode(AUTO_RANDOM);
                UpdatePreview();
                return;
            }

if (UsePhysicalRemove())
{
    StartPhysicalHide(item, "hide");
    return;
}

            int found = SetHidden(item, true);
            DebugLog("[WEAR NONE DEBUG] set hidden " + item.Name +
                " / found=" + found.ToString(CultureInfo.InvariantCulture) +
                " / nowHidden=" + (IsHidden(item) ? "1" : "0"));

            if (found <= 0)
            {
                SetStatus("HIDE skipped: stale material refs / press SCAN CLOTH: " + item.Name);
                StopAutoIfMode(AUTO_NEXT);
                StopAutoIfMode(AUTO_RANDOM);
                UpdatePreview();
                return;
            }

            SetStatus("HIDE: " + item.Name);
            PublishClothState("hide", item, true);
            ResetAutoTimerAfterStep();
            UpdatePreview();
            return;
        }

        SetStatus("No more visible cloth");
        StopAutoIfMode(AUTO_NEXT);
        StopAutoIfMode(AUTO_RANDOM);
        PublishClothState("hide_done", null, false);
        UpdatePreview();
    }

    private void PrevWear()
    {
        LogButtonPressed("PREV +1 WEAR");
        // PREVは「着る」専用。PHTY/Fade中なら、まず完全に止めてから必ず1枚着せに行く。
        // ここで return すると、PHTY中に押したPREVが「キャンセルだけ」で終わってしまう。
        if (physicalRemoveRunning > 0 || physicalHideRoutine != null)
        {
            StopActivePhysicalRoutine(true);
            SetStatus("PHTY cancelled before PREV; continue wear");
            UpdateButtonColors();
        }

        if (orderedClothes.Count == 0)
        {
            SetStatus("No cloth list");
            StopAutoIfMode(AUTO_PREV);
            return;
        }

        for (int i = orderedClothes.Count - 1; i >= 0; i--)
        {
            ClothItem item = orderedClothes[i];
            if (IsProtected(item))
                continue;

            if (HasAnyHiddenMaterial(item))
            {
                // v9: PREVはPHTY設定を無視して、見た目復元だけを試す。
                // ReloadはMaterial参照を壊すことがあるため使わない。

                // PREVは状態復元を最優先。ReloadはMaterial参照を作り直して逆に不安定化することがあるため、
                // ここでは行わない。必要なら別ボタンのRELOAD ALLを使う。
                DumpMaterialValues(item, "PREV_BEFORE_RESTORE_VISIBLE");
                int found = RestoreVisibleForItemNoVisualRestore(item, "PREV");
                DumpMaterialValues(item, "PREV_AFTER_RESTORE_VISIBLE");
                if (found <= 0)
                {
                    SetStatus("WEAR skipped: material refs not found / press SCAN CLOTH: " + item.Name);
                    UpdateButtonColors();
                    UpdatePreview();
                    return;
                }

                if (!string.IsNullOrEmpty(item.Prefix))
                    phtyHiddenPrefixes.Remove(item.Prefix);

                SetStatus("WEAR: " + item.Name);
                PublishClothState("wear", item, true);
                ResetAutoTimerAfterStep();
                UpdateButtonColors();
                UpdatePreview();
                return;
            }
        }

SetStatus("No more hidden cloth");
StopAutoIfMode(AUTO_PREV);
// PublishClothState("wear_done", null, false);
DebugLog("[PREV DONE NO PUBLISH] wear_done skipped");
UpdateButtonColors();
UpdatePreview();
    }

    private void WearNone()
    {
        LogButtonPressed("WEAR NONE");

        DebugLog("[WEAR NONE DEBUG] pressed / phty=" + (physRemoveStyleChooser != null ? physRemoveStyleChooser.val : "(null)") +
            " / running=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture) +
            " / physicalRoutine=" + (physicalHideRoutine != null ? "1" : "0") +
            " / wearNoneRoutine=" + (wearNoneRoutine != null ? "1" : "0") +
            " / ordered=" + orderedClothes.Count.ToString(CultureInfo.InvariantCulture));

        if (wearNoneRoutine != null)
        {
            DebugLog("[WEAR NONE DEBUG] ignored because wearNoneRoutine is already running");
            SetStatus("WEAR NONE is already running");
            UpdatePreview();
            return;
        }

        if (physicalRemoveRunning > 0 || physicalHideRoutine != null)
        {
            StopActivePhysicalRoutine(true);
            SetStatus("PHTY cancelled before WEAR NONE");
        }

        if (orderedClothes.Count == 0)
        {
            SetStatus("No cloth list");
            UpdateExternalState();
            UpdatePreview();
            return;
        }

        if (UsePhysicalRemove())
        {
            DebugLog("[WEAR NONE DEBUG] start grouped PHTY routine");
            wearNoneRoutine = StartCoroutine(WearNonePhysicalGroupedRoutine());
            return;
        }

        int changed = 0;
        ClothItem lastChanged = null;

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
                continue;

            if (IsProtected(item))
                continue;

            if (IsHidden(item))
                continue;

            int prepFound = PrepareVisualStateForAction(item, "WEAR_NONE_PREP", false);
            if (prepFound <= 0)
            {
                DebugLog("[WEAR NONE SKIP] prep material refs not found: " + item.Name);
                continue;
            }

            int found = SetHidden(item, true);
            if (found <= 0)
            {
                DebugLog("[WEAR NONE SKIP] hide material refs not found: " + item.Name);
                continue;
            }

            if (!string.IsNullOrEmpty(item.Prefix))
            {
                forcedHiddenPrefixes.Add(item.Prefix);
                phtyHiddenPrefixes.Add(item.Prefix);
            }

            changed++;
            lastChanged = item;
        }

        if (changed <= 0)
        {
            SetStatus("WEAR NONE: no visible cloth");
            PublishClothState("hide_done", null, false);
            UpdateButtonColors();
            UpdatePreview();
            return;
        }

        SetStatus("WEAR NONE: " + changed.ToString(CultureInfo.InvariantCulture));
        PublishClothState("wear_none_complete", lastChanged, true);
        UpdateButtonColors();
        UpdatePreview();
    }

    private IEnumerator WearNonePhysicalGroupedRoutine()
    {
        int cancelToken = ++wearNoneCancelVersion;
        DebugLog("[WEAR NONE CANCEL] new routine token=" + cancelToken.ToString(CultureInfo.InvariantCulture));
        physicalRemoveRunning++;
        UpdateButtonColors();

        DebugLog("[WEAR NONE DEBUG] grouped routine enter / running=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture));

        List<ClothItem> outerGroup = BuildWearNoneGroup(false);
        List<ClothItem> underwearGroup = BuildWearNoneGroup(true);

        DebugLog("[WEAR NONE DEBUG] group count / outer=" + outerGroup.Count.ToString(CultureInfo.InvariantCulture) +
            " / underwear=" + underwearGroup.Count.ToString(CultureInfo.InvariantCulture));

        DumpWearNoneGroupDebug("outer", outerGroup);
        DumpWearNoneGroupDebug("underwear", underwearGroup);

        int total = outerGroup.Count + underwearGroup.Count;
        if (total <= 0)
        {
            DebugLog("[WEAR NONE DEBUG] no visible cloth / routine exit");
            physicalRemoveRunning = Mathf.Max(0, physicalRemoveRunning - 1);
            wearNoneRoutine = null;
            SetStatus("WEAR NONE: no visible cloth");
            PublishClothState("hide_done", null, false);
            UpdateButtonColors();
            UpdatePreview();
            yield break;
        }

        if (outerGroup.Count > 0)
            yield return StartCoroutine(PhysicalHideGroupRoutine(outerGroup, "wear_none_group1", "outer/top", cancelToken));

        if (IsWearNoneCancelled(cancelToken))
        {
            DebugLog("[WEAR NONE CANCEL] grouped routine exit after outer / token=" + cancelToken.ToString(CultureInfo.InvariantCulture) + " / current=" + wearNoneCancelVersion.ToString(CultureInfo.InvariantCulture));
            physicalRemoveRunning = Mathf.Max(0, physicalRemoveRunning - 1);
            wearNoneRoutine = null;
            UpdateButtonColors();
            UpdatePreview();
            yield break;
        }

        if (underwearGroup.Count > 0)
            yield return StartCoroutine(PhysicalHideGroupRoutine(underwearGroup, "wear_none_group2", "underwear", cancelToken));

        if (IsWearNoneCancelled(cancelToken))
        {
            DebugLog("[WEAR NONE CANCEL] grouped routine exit after underwear / token=" + cancelToken.ToString(CultureInfo.InvariantCulture) + " / current=" + wearNoneCancelVersion.ToString(CultureInfo.InvariantCulture));
            physicalRemoveRunning = Mathf.Max(0, physicalRemoveRunning - 1);
            wearNoneRoutine = null;
            UpdateButtonColors();
            UpdatePreview();
            yield break;
        }

        DebugLog("[WEAR NONE DEBUG] grouped routine complete / total=" + total.ToString(CultureInfo.InvariantCulture) +
            " / running(before dec)=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture));

        physicalRemoveRunning = Mathf.Max(0, physicalRemoveRunning - 1);
        wearNoneRoutine = null;

        SetStatus("WEAR NONE PHTY complete: " + total.ToString(CultureInfo.InvariantCulture));
        PublishClothState("wear_none_complete", null, true);
        UpdateButtonColors();
        UpdatePreview();
    }

    private void DumpWearNoneGroupDebug(string label, List<ClothItem> group)
    {
        if (!IsDebugLogEnabled())
            return;

        int count = group != null ? group.Count : 0;
        DebugLog("[WEAR NONE DEBUG] dump group " + label + " / count=" + count.ToString(CultureInfo.InvariantCulture));

        if (group == null)
            return;

        for (int i = 0; i < group.Count; i++)
        {
            ClothItem item = group[i];
            if (item == null)
            {
                DebugLog("[WEAR NONE DEBUG] " + label + "[" + i.ToString(CultureInfo.InvariantCulture) + "] null");
                continue;
            }

            DebugLog("[WEAR NONE DEBUG] " + label + "[" + i.ToString(CultureInfo.InvariantCulture) + "] " +
                item.Name +
                " / cat=" + GetClothCategory(item) +
                " / hidden=" + (IsHidden(item) ? "1" : "0") +
                " / protected=" + (IsProtected(item) ? "1" : "0") +
                " / sim=" + (IsSimCloth(item) ? "1" : "0"));
        }
    }

    private List<ClothItem> BuildWearNoneGroup(bool underwear)
    {
        List<ClothItem> list = new List<ClothItem>();

        int total = orderedClothes != null ? orderedClothes.Count : 0;
        int skippedNull = 0;
        int skippedProtected = 0;
        int skippedAlreadyHidden = 0;
        int skippedOtherGroup = 0;
        int added = 0;

        DebugLog("[WEAR NONE RESUME] BuildWearNoneGroup enter / target=" + (underwear ? "underwear" : "outer") +
            " / ordered=" + total.ToString(CultureInfo.InvariantCulture) +
            " / progress=" + (stateProgressJSON != null ? stateProgressJSON.val : ""));

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
            {
                skippedNull++;
                DebugLog("[WEAR NONE RESUME] skip reason=null index=" + (i + 1).ToString("00") + "/" + total.ToString(CultureInfo.InvariantCulture));
                continue;
            }

            string category = GetClothCategory(item);
            bool protect = IsProtected(item);
            bool hidden = IsHidden(item);
            bool anyHidden = HasAnyHiddenMaterial(item);
            bool isUnderwear = category == "bra" || category == "panty";
            int validMat = CountValidMaterials(item);
            int materialCount = item.Materials != null ? item.Materials.Count : 0;

            DebugLog("[WEAR NONE RESUME] check index=" + (i + 1).ToString("00") + "/" + total.ToString(CultureInfo.InvariantCulture) +
                " name=" + item.Name +
                " prefix=" + item.Prefix +
                " cat=" + category +
                " hidden=" + (hidden ? "1" : "0") +
                " anyHidden=" + (anyHidden ? "1" : "0") +
                " protect=" + (protect ? "1" : "0") +
                " underwear=" + (isUnderwear ? "1" : "0") +
                " mat=" + validMat.ToString(CultureInfo.InvariantCulture) + "/" + materialCount.ToString(CultureInfo.InvariantCulture));

            if (protect)
            {
                skippedProtected++;
                DebugLog("[WEAR NONE RESUME] skip reason=protected index=" + (i + 1).ToString("00") + " name=" + item.Name);
                continue;
            }

            if (hidden)
            {
                skippedAlreadyHidden++;
                DebugLog("[WEAR NONE RESUME] skip reason=already_hidden index=" + (i + 1).ToString("00") + " name=" + item.Name);
                continue;
            }

            if (underwear != isUnderwear)
            {
                skippedOtherGroup++;
                DebugLog("[WEAR NONE RESUME] skip reason=other_group index=" + (i + 1).ToString("00") +
                    " name=" + item.Name +
                    " target=" + (underwear ? "underwear" : "outer") +
                    " itemGroup=" + (isUnderwear ? "underwear" : "outer"));
                continue;
            }

            added++;
            DebugLog("[WEAR NONE RESUME] add reason=visible_remaining index=" + (i + 1).ToString("00") +
                " name=" + item.Name +
                " cat=" + category);
            list.Add(item);
        }

        DebugLog("[WEAR NONE RESUME] BuildWearNoneGroup exit / target=" + (underwear ? "underwear" : "outer") +
            " / add=" + added.ToString(CultureInfo.InvariantCulture) +
            " / skipNull=" + skippedNull.ToString(CultureInfo.InvariantCulture) +
            " / skipProtected=" + skippedProtected.ToString(CultureInfo.InvariantCulture) +
            " / skipAlreadyHidden=" + skippedAlreadyHidden.ToString(CultureInfo.InvariantCulture) +
            " / skipOtherGroup=" + skippedOtherGroup.ToString(CultureInfo.InvariantCulture));

        return list;
    }


    private bool IsWearNoneCancelled(int token)
    {
        // StartCoroutine は、代入が完了する前に coroutine 本体が先に走ることがある。
        // そのため wearNoneRoutine == null をキャンセル判定に使うと、
        // 通常の WEAR NONE 開始直後までキャンセル扱いになって脱がなくなる。
        // キャンセル判定は version token のみで行う。
        return token != wearNoneCancelVersion;
    }

    private IEnumerator PhysicalHideGroupRoutine(List<ClothItem> group, string actionBase, string label, int cancelToken)
    {
        if (group == null || group.Count == 0)
            yield break;

        if (IsWearNoneCancelled(cancelToken))
        {
            DebugLog("[WEAR NONE CANCEL] group enter blocked / action=" + actionBase + " / token=" + cancelToken.ToString(CultureInfo.InvariantCulture));
            yield break;
        }

        string requestedStyle = physRemoveStyleChooser != null ? physRemoveStyleChooser.val : PHYS_OFF;
        float duration = physDurationJSON != null ? Mathf.Max(0.2f, physDurationJSON.val) : 3.0f;
        float fadeSeconds = fadeSecondsJSON != null ? Mathf.Clamp(fadeSecondsJSON.val, 0.0f, duration) : Mathf.Min(2.0f, duration);

        DebugLog("[WEAR NONE DEBUG] PhysicalHideGroupRoutine enter / action=" + actionBase +
            " / label=" + label +
            " / count=" + group.Count.ToString(CultureInfo.InvariantCulture) +
            " / style=" + requestedStyle +
            " / duration=" + duration.ToString("F2", CultureInfo.InvariantCulture) +
            " / fade=" + fadeSeconds.ToString("F2", CultureInfo.InvariantCulture));

        List<PhysBoolBackup> boolBackups = new List<PhysBoolBackup>();
        List<PhysFloatBackup> floatBackups = new List<PhysFloatBackup>();
        List<FadeParamRef> fadeRefs = new List<FadeParamRef>();

        activePhysBoolBackups = boolBackups;
        activePhysFloatBackups = floatBackups;
        activeFadeRefs = fadeRefs;

        SetStatus("[WEAR NONE PHTY START] " + label + " / count=" + group.Count.ToString(CultureInfo.InvariantCulture));
        PublishClothState(actionBase + "_start", null, true);
        UpdatePreview();

        int prepared = 0;
        int simCount = 0;

        for (int i = 0; i < group.Count; i++)
        {
            ClothItem item = group[i];
            if (item == null)
                continue;

            // WEAR NONE grouped PHTY must stay lightweight.
            // Do not call PrepareVisualStateForAction() here, because Debug ON makes it dump
            // material/sim state heavily and it can stall the grouped routine.
            DebugLog("[WEAR NONE DEBUG] prepare begin " + item.Name);

            int prepFound = CountValidMaterials(item, false);
            if (prepFound <= 0)
            {
                RefreshItemMaterials(item);
                prepFound = CountValidMaterials(item, false);
            }

            DebugLog("[WEAR NONE DEBUG] prepare fast " + item.Name +
                " / prepFound=" + prepFound.ToString(CultureInfo.InvariantCulture) +
                " / hidden=" + (IsHidden(item) ? "1" : "0") +
                " / cat=" + GetClothCategory(item));

            if (prepFound <= 0)
            {
                DebugLog("[WEAR NONE PHTY SKIP] prep material refs not found: " + item.Name);
                continue;
            }

            bool sim = IsSimCloth(item);
            if (sim)
            {
                string style = ResolvePhysicalStyleForItem(item, requestedStyle);
                int physChanged = ApplyPhysicalRemoveSettings(item, style, boolBackups, floatBackups);
                DebugLog("[WEAR NONE DEBUG] sim apply " + item.Name +
                    " / style=" + style +
                    " / changed=" + physChanged.ToString(CultureInfo.InvariantCulture));
                simCount++;
            }

            int beforeFadeCount = fadeRefs.Count;
            CollectFadeParams(item, -1.0f, fadeRefs);
            DebugLog("[WEAR NONE DEBUG] fade collect " + item.Name +
                " / added=" + (fadeRefs.Count - beforeFadeCount).ToString(CultureInfo.InvariantCulture) +
                " / totalFadeRefs=" + fadeRefs.Count.ToString(CultureInfo.InvariantCulture));

            prepared++;
        }

        if (prepared <= 0)
        {
            RestorePhysicalSettings(boolBackups, floatBackups);
            RestoreFadeSettings(fadeRefs);
            activePhysBoolBackups = null;
            activePhysFloatBackups = null;
            activeFadeRefs = null;
            SetStatus("[WEAR NONE PHTY SKIP] " + label + " / no prepared cloth");
            PublishClothState(actionBase + "_skip", null, true);
            yield break;
        }

        DebugLog("[WEAR NONE DEBUG] prepared summary / prepared=" + prepared.ToString(CultureInfo.InvariantCulture) +
            " / sim=" + simCount.ToString(CultureInfo.InvariantCulture) +
            " / fadeRefs=" + fadeRefs.Count.ToString(CultureInfo.InvariantCulture));

        float physicalSeconds = Mathf.Max(0.0f, duration - fadeSeconds);
        if (physicalSeconds > 0.0f)
        {
            DebugLog("[WEAR NONE DEBUG] wait physicalSeconds=" + physicalSeconds.ToString("F2", CultureInfo.InvariantCulture));
            yield return new WaitForSeconds(physicalSeconds);
            if (IsWearNoneCancelled(cancelToken))
            {
                DebugLog("[WEAR NONE CANCEL] group cancelled after physical wait / action=" + actionBase);
                RestorePhysicalSettings(boolBackups, floatBackups);
                RestoreFadeSettings(fadeRefs);
                RestorePhysicalScanStateAll();
                RestoreFadeScanStateAll();
                activePhysBoolBackups = null;
                activePhysFloatBackups = null;
                activeFadeRefs = null;
                yield break;
            }
        }

        if (fadeRefs.Count > 0 && fadeSeconds > 0.01f)
        {
            DebugLog("[WEAR NONE DEBUG] fade start / seconds=" + fadeSeconds.ToString("F2", CultureInfo.InvariantCulture) +
                " / refs=" + fadeRefs.Count.ToString(CultureInfo.InvariantCulture));

            float start = Time.time;
            while (Time.time - start < fadeSeconds)
            {
                float t = Mathf.Clamp01((Time.time - start) / fadeSeconds);
                for (int i = 0; i < fadeRefs.Count; i++)
                {
                    FadeParamRef r = fadeRefs[i];
                    if (r != null && r.Param != null)
                        r.Param.val = Mathf.Lerp(r.StartValue, r.EndValue, t);
                }
                yield return null;
                if (IsWearNoneCancelled(cancelToken))
                {
                    DebugLog("[WEAR NONE CANCEL] group cancelled during fade / action=" + actionBase);
                    RestorePhysicalSettings(boolBackups, floatBackups);
                    RestoreFadeSettings(fadeRefs);
                    RestorePhysicalScanStateAll();
                    RestoreFadeScanStateAll();
                    activePhysBoolBackups = null;
                    activePhysFloatBackups = null;
                    activeFadeRefs = null;
                    yield break;
                }
            }

            for (int i = 0; i < fadeRefs.Count; i++)
            {
                FadeParamRef r = fadeRefs[i];
                if (r != null && r.Param != null)
                    r.Param.val = r.EndValue;
            }

            DebugLog("[WEAR NONE DEBUG] fade done");
        }
        else if (duration > 0.01f && fadeRefs.Count == 0)
        {
            yield return new WaitForSeconds(fadeSeconds);
            if (IsWearNoneCancelled(cancelToken))
            {
                DebugLog("[WEAR NONE CANCEL] group cancelled after fade wait / action=" + actionBase);
                RestorePhysicalSettings(boolBackups, floatBackups);
                RestoreFadeSettings(fadeRefs);
                RestorePhysicalScanStateAll();
                RestoreFadeScanStateAll();
                activePhysBoolBackups = null;
                activePhysFloatBackups = null;
                activeFadeRefs = null;
                yield break;
            }
        }

        if (IsWearNoneCancelled(cancelToken))
        {
            DebugLog("[WEAR NONE CANCEL] group cancelled before hide / action=" + actionBase);
            RestorePhysicalSettings(boolBackups, floatBackups);
            RestoreFadeSettings(fadeRefs);
            RestorePhysicalScanStateAll();
            RestoreFadeScanStateAll();
            activePhysBoolBackups = null;
            activePhysFloatBackups = null;
            activeFadeRefs = null;
            yield break;
        }

        int hiddenCount = 0;
        ClothItem lastHidden = null;

        for (int i = 0; i < group.Count; i++)
        {
            ClothItem item = group[i];
            if (item == null)
                continue;

            if (IsHidden(item))
                continue;

            int found = SetHidden(item, true);
            if (found <= 0)
            {
                DebugLog("[WEAR NONE PHTY HIDE SKIP] material refs not found: " + item.Name);
                continue;
            }

            if (!string.IsNullOrEmpty(item.Prefix))
            {
                forcedHiddenPrefixes.Add(item.Prefix);
                phtyHiddenPrefixes.Add(item.Prefix);
            }

            hiddenCount++;
            lastHidden = item;
        }

        DebugLog("[WEAR NONE DEBUG] group hide summary / hidden=" + hiddenCount.ToString(CultureInfo.InvariantCulture) +
            " / boolBackups=" + boolBackups.Count.ToString(CultureInfo.InvariantCulture) +
            " / floatBackups=" + floatBackups.Count.ToString(CultureInfo.InvariantCulture) +
            " / fadeRefs=" + fadeRefs.Count.ToString(CultureInfo.InvariantCulture));

        RestorePhysicalSettings(boolBackups, floatBackups);
        RestoreFadeSettings(fadeRefs);

        activePhysBoolBackups = null;
        activePhysFloatBackups = null;
        activeFadeRefs = null;

        SetStatus("[WEAR NONE PHTY DONE] " + label +
            " / hidden=" + hiddenCount.ToString(CultureInfo.InvariantCulture) +
            " / sim=" + simCount.ToString(CultureInfo.InvariantCulture));

        PublishClothState(actionBase + "_done", lastHidden, true);
        UpdatePreview();
    }

    private void RemoveBra()
    {
        LogButtonPressed("REMOVE BRA");
        if (physicalRemoveRunning > 0)
        {
            SetStatus("Physical remove is running");
            return;
        }

        RemoveCategory("bra", "REMOVE BRA", "No bra cloth");
    }

    private void RemovePanty()
    {
        LogButtonPressed("REMOVE PANTY");
        if (physicalRemoveRunning > 0)
        {
            SetStatus("Physical remove is running");
            return;
        }

        RemoveCategory("panty", "REMOVE PANTY", "No panty cloth");
    }

    private void RemoveCategory(string category, string statusLabel, string noneMessage)
    {
        if (orderedClothes.Count == 0)
        {
            SetStatus("No cloth list");
            UpdateExternalState();
            UpdatePreview();
            return;
        }

        int changed = 0;
        ClothItem lastChanged = null;

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
                continue;

            if (GetClothCategory(item) != category)
                continue;

            if (IsHidden(item))
                continue;

            if (UsePhysicalRemove())
            {
                StartPhysicalHide(item, category == "bra" ? "remove_bra" : (category == "panty" ? "remove_panty" : "hide"));
            }
            else
            {
                int found = SetHidden(item, true);
                if (found <= 0)
                {
                    DebugLog("[REMOVE SKIP] stale material refs: " + item.Name);
                    continue;
                }
            }
            changed++;
            lastChanged = item;
        }

        if (changed <= 0)
        {
            SetStatus(noneMessage);
            UpdateExternalState();
            UpdatePreview();
            return;
        }

        string action = category == "bra" ? "remove_bra" : (category == "panty" ? "remove_panty" : "remove");
        SetStatus(statusLabel + ": " + changed.ToString(CultureInfo.InvariantCulture));
        PublishClothState(action, lastChanged, true);
        UpdatePreview();
    }


    private bool UsePhysicalRemove()
    {
        return physRemoveStyleChooser != null && physRemoveStyleChooser.val != PHYS_OFF;
    }

    private string ResolvePhysicalStyleForItem(ClothItem item, string requestedStyle)
    {
        if (string.IsNullOrEmpty(requestedStyle))
            requestedStyle = PHYS_OFF;

        if (requestedStyle == PHYS_UP && bottomForceDownJSON != null && bottomForceDownJSON.val && IsForceDownItem(item))
            return PHYS_DOWN;

        return requestedStyle;
    }

    private bool IsForceDownItem(ClothItem item)
    {
        if (item == null)
            return false;

        string category = GetClothCategory(item);
        return category == "bottom" || category == "shoes" || category == "panty";
    }


    private void StartPhysicalHide(ClothItem item, string finalAction)
    {
        if (item == null)
            return;

        if (IsHidden(item))
        {
            UpdatePreview();
            return;
        }

        if (physicalHideRoutine != null)
        {
            SetStatus("Physical remove is already running");
            return;
        }

        physicalHideRoutine = StartCoroutine(PhysicalHideRoutine(item, finalAction));
    }

    private IEnumerator PhysicalHideRoutine(ClothItem item, string finalAction)
    {
        physicalRemoveRunning++;
        string requestedStyle = physRemoveStyleChooser != null ? physRemoveStyleChooser.val : PHYS_OFF;
        string style = ResolvePhysicalStyleForItem(item, requestedStyle);
        float duration = physDurationJSON != null ? Mathf.Max(0.2f, physDurationJSON.val) : 3.0f;
        float fadeSeconds = fadeSecondsJSON != null ? Mathf.Clamp(fadeSecondsJSON.val, 0.0f, duration) : Mathf.Min(2.0f, duration);
        bool sim = IsSimCloth(item);

        List<PhysBoolBackup> boolBackups = new List<PhysBoolBackup>();
        List<PhysFloatBackup> floatBackups = new List<PhysFloatBackup>();
        List<FadeParamRef> fadeRefs = new List<FadeParamRef>();
        activePhysBoolBackups = boolBackups;
        activePhysFloatBackups = floatBackups;
        activeFadeRefs = fadeRefs;
        UpdateButtonColors();

        DebugLog("[TRACE PHTY] A before BEFORE_PHTY_ENTRY: " + item.Name);
        DumpMaterialValues(item, "BEFORE_PHTY_ENTRY");
        DebugLog("[TRACE PHTY] B before PHTY_START_PREP: " + item.Name);
        PrepareVisualStateForAction(item, "PHTY_START_PREP", false);
        DebugLog("[TRACE PHTY] C after PHTY_START_PREP: " + item.Name);
        DumpMaterialValues(item, "AFTER_PHTY_PREP");
        DebugLog("[TRACE PHTY] D before APPLY: " + item.Name);

        int changed = 0;
        if (sim)
            changed = ApplyPhysicalRemoveSettings(item, style, boolBackups, floatBackups);
        else
            DebugLog("[TRACE PHTY] D2 no sim apply: " + item.Name);

        DebugLog("[TRACE PHTY] E after APPLY: " + item.Name + " changed=" + changed.ToString(CultureInfo.InvariantCulture));
        DumpMaterialValues(item, "AFTER_SIM_APPLY");
        DumpSimState(item, "AFTER_SIM_APPLY");
        DebugLog("[TRACE PHTY] F after dumps before PHTY START: " + item.Name);

        SetStatus("[PHTY START] " + style + " : " + item.Name + " / " + duration.ToString("F1", CultureInfo.InvariantCulture) + " sec / sim=" + (sim ? "1" : "0") + " / changed=" + changed.ToString(CultureInfo.InvariantCulture));
        PublishClothState("phys_start", item, true);
        UpdatePreview();

        if (sim)
        {
            float physicalSeconds = Mathf.Max(0.0f, duration - fadeSeconds);
            if (physicalSeconds > 0.0f)
                yield return new WaitForSeconds(physicalSeconds);

            if (fadeSeconds > 0.01f)
                yield return StartCoroutine(FadeOutRoutine(item, fadeSeconds, fadeRefs));
            DumpMaterialValues(item, "AFTER_FADE");
        }
        else
        {
            // 非SIM服は物理脱衣できないため、PHTY Seconds 全体を使ってフェードする。
            if (duration > 0.01f)
                yield return StartCoroutine(FadeOutRoutine(item, duration, fadeRefs));
            DumpMaterialValues(item, "AFTER_FADE");
        }

        DumpMaterialValues(item, "BEFORE_HIDE");
        int foundHidden = SetHidden(item, true);
        DumpMaterialValues(item, "AFTER_HIDE");
        if (foundHidden > 0 && item != null && !string.IsNullOrEmpty(item.Prefix))
            phtyHiddenPrefixes.Add(item.Prefix);
        RestorePhysicalSettings(boolBackups, floatBackups);
        RestoreFadeSettings(fadeRefs);
        DumpMaterialValues(item, "AFTER_RESTORE_BACKUPS");
        DumpSimState(item, "AFTER_RESTORE_BACKUPS");

        // v9s: CasualDenimShoes などで SetHidden(false) が危険なケースがあるため、
        // PHTY完了時の「一度表示→再非表示」Finalizeは封印する。
        // hidden=true のまま、Alpha/物理値だけ戻して終了する。
        if (foundHidden > 0)
        {
            DebugLog("[HIDDEN FINALIZE SKIP] " + item.Name + " / keep hidden=true / no SetHidden(false)");
            DumpHideMaterialState(item, "AFTER_FINALIZE_SKIP");
        }

        physicalRemoveRunning = Mathf.Max(0, physicalRemoveRunning - 1);
        activePhysBoolBackups = null;
        activePhysFloatBackups = null;
        activeFadeRefs = null;
        physicalHideRoutine = null;
        if (foundHidden <= 0)
        {
            SetStatus("[PHTY DONE] stale material refs / press SCAN CLOTH: " + item.Name);
            StopAutoIfMode(AUTO_NEXT);
            StopAutoIfMode(AUTO_RANDOM);
            UpdateButtonColors();
            UpdatePreview();
            yield break;
        }

        SetStatus("[PHTY DONE] " + style + " : " + item.Name + " / sim=" + (sim ? "1" : "0"));
        PublishClothState(finalAction, item, true);
        ResetAutoTimerAfterStep();
        UpdateButtonColors();
        UpdatePreview();
    }

    private IEnumerator FadeOutRoutine(ClothItem item, float seconds, List<FadeParamRef> refs)
    {
        if (refs == null)
            refs = new List<FadeParamRef>();

        DumpMaterialValues(item, "FADE_ROUTINE_BEGIN");
        CollectFadeParams(item, -1.0f, refs);
        if (refs.Count == 0)
        {
            DebugLog("[FADE] params not found: " + (item != null ? item.Name : ""));
            if (seconds > 0.01f)
                yield return new WaitForSeconds(seconds);
            yield break;
        }

        DebugLog("[FADE START] " + item.Name + " / " + seconds.ToString("F2", CultureInfo.InvariantCulture) + " sec / params=" + refs.Count.ToString(CultureInfo.InvariantCulture));

        float start = Time.time;
        while (Time.time - start < seconds)
        {
            float t = Mathf.Clamp01((Time.time - start) / seconds);
            for (int i = 0; i < refs.Count; i++)
            {
                FadeParamRef r = refs[i];
                if (r != null && r.Param != null)
                    r.Param.val = Mathf.Lerp(r.StartValue, r.EndValue, t);
            }
            yield return null;
        }

        for (int i = 0; i < refs.Count; i++)
        {
            FadeParamRef r = refs[i];
            if (r != null && r.Param != null)
                r.Param.val = r.EndValue;
        }

        DebugLog("[FADE DONE] " + item.Name);
        DumpMaterialValues(item, "FADE_ROUTINE_DONE");
    }

    private void CollectFadeParams(ClothItem item, float target, List<FadeParamRef> refs)
    {
        if (item == null || selectedPerson == null || refs == null)
            return;

        string[] names = FadeFloatNames();
        List<string> ids = GetRelatedStorableIds(item);
        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat fp = st.GetFloatJSONParam(names[n]);
                if (fp == null)
                    continue;
                if (HasFadeRef(refs, fp))
                    continue;

                FadeParamRef r = new FadeParamRef();
                r.Param = fp;
                r.StartValue = fp.val;
                r.EndValue = target;
                r.Name = ids[i] + "/" + names[n];
                refs.Add(r);
            }
        }
    }

    private bool HasFadeRef(List<FadeParamRef> refs, JSONStorableFloat param)
    {
        if (refs == null || param == null)
            return false;

        for (int i = 0; i < refs.Count; i++)
        {
            if (refs[i] != null && refs[i].Param == param)
                return true;
        }
        return false;
    }

    private void RestoreFadeSettings(List<FadeParamRef> refs)
    {
        if (refs == null)
            return;

        for (int i = 0; i < refs.Count; i++)
        {
            FadeParamRef r = refs[i];
            if (r != null && r.Param != null)
                r.Param.val = r.StartValue;
        }
    }


    private int ForceAlphaAdjustVisibleAll()
    {
        int changed = 0;
        for (int i = 0; i < clothes.Count; i++)
            changed += ForceAlphaAdjustVisible(clothes[i]);
        return changed;
    }

    private void TraceAlphaAdjustState(ClothItem item, string tag)
    {
        if (!IsDebugLogEnabled())
            return;

        if (selectedPerson == null || item == null)
        {
            DebugLog("[ALPHA TRACE] " + tag + " item/person null");
            return;
        }

        string[] names = AlphaAdjustVisibleNames();
        List<string> ids = GetRelatedStorableIds(item);
        int found = 0;
        int nonZero = 0;
        float min = 9999f;
        float max = -9999f;

        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat fp = st.GetFloatJSONParam(names[n]);
                if (fp == null)
                    continue;

                found++;
                if (Mathf.Abs(fp.val) > 0.0001f)
                    nonZero++;
                if (fp.val < min) min = fp.val;
                if (fp.val > max) max = fp.val;

                DebugLog("[ALPHA TRACE] " + tag +
                    " name=" + item.Name +
                    " prefix=" + item.Prefix +
                    " storable=" + ids[i] +
                    " param=" + names[n] +
                    " value=" + fp.val.ToString("F3", CultureInfo.InvariantCulture));
            }
        }

        DebugLog("[ALPHA TRACE SUMMARY] " + tag +
            " name=" + item.Name +
            " prefix=" + item.Prefix +
            " found=" + found.ToString(CultureInfo.InvariantCulture) +
            " nonZero=" + nonZero.ToString(CultureInfo.InvariantCulture) +
            " min=" + (found > 0 ? min.ToString("F3", CultureInfo.InvariantCulture) : "NA") +
            " max=" + (found > 0 ? max.ToString("F3", CultureInfo.InvariantCulture) : "NA") +
            " hidden=" + (IsHidden(item) ? "1" : "0") +
            " anyHidden=" + (HasAnyHiddenMaterial(item) ? "1" : "0"));
    }

    private int ForceAlphaAdjustVisible(ClothItem item)
    {
        if (selectedPerson == null || item == null)
            return 0;

        DebugLog("[NUDGE ENTER] " + item.Name);

        int changed = 0;
        string[] names = AlphaAdjustVisibleNames();
        List<string> ids = GetRelatedStorableIds(item);
        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat fp = st.GetFloatJSONParam(names[n]);
                if (fp == null)
                    continue;

                float before = fp.val;
                bool wasChanged = Mathf.Abs(before - 0.0f) > 0.0001f;

                // VaM/Unity側のMaterial更新が0代入だけだと走らない服があるため、
                // 一瞬だけ -0.001 に振ってから 0.000 に戻す。
                fp.val = -0.001f;
                fp.val = 0.0f;

                DebugLog("[ALPHA NUDGE] " + item.Name + " / " + ids[i] + "/" + names[n] +
                    " : " + before.ToString("F3", CultureInfo.InvariantCulture) +
                    " -> -0.001 -> 0.000" +
                    (wasChanged ? " / changed" : " / refresh"));

                if (wasChanged)
                    changed++;
            }
        }
        DebugLog("[NUDGE EXIT] " + item.Name + " changed=" + changed.ToString(CultureInfo.InvariantCulture));
        return changed;
    }

    private void DumpMaterialValues(ClothItem item, string tag)
    {
        if (!IsDebugLogEnabled())
            return;

        if (selectedPerson == null || item == null)
            return;

        string[] names = FadeFloatNames();
        List<string> ids = GetRelatedStorableIds(item);
        int found = 0;

        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat fp = st.GetFloatJSONParam(names[n]);
                if (fp == null)
                    continue;

                found++;
                DebugLog("[MAT " + tag + "] " +
                    item.Name +
                    " / " +
                    ids[i] +
                    "/" +
                    names[n] +
                    " = " +
                    fp.val.ToString("F3", CultureInfo.InvariantCulture));
            }
        }

        DebugLog("[MAT " + tag + " SUMMARY] " +
            item.Name +
            " found=" + found.ToString(CultureInfo.InvariantCulture) +
            " alpha=" + GetAlphaStateSummary(item) +
            " hidden=" + (IsHidden(item) ? "1" : "0") +
            " anyHidden=" + (HasAnyHiddenMaterial(item) ? "1" : "0") +
            " mat=" + CountValidMaterials(item, true).ToString(CultureInfo.InvariantCulture) + "/" +
            (item.Materials != null ? item.Materials.Count.ToString(CultureInfo.InvariantCulture) : "0"));
    }

    private string[] AlphaAdjustVisibleNames()
    {
        return new string[] { "Alpha Adjust", "alphaAdjust", "alpha adjust" };
    }

    private string[] FadeFloatNames()
    {
        return new string[] {
            "Alpha Adjust", "alphaAdjust", "alpha adjust",
            "alpha", "Alpha",
            "opacity", "Opacity",
            "alphaMultiplier", "Alpha Multiplier", "alpha multiplier",
            "materialAlpha", "Material Alpha", "material alpha",
            "diffuseAlpha", "Diffuse Alpha", "diffuse alpha",
            "transparency", "Transparency",
            "translucency", "Translucency"
        };
    }

    private int ApplyPhysicalRemoveSettings(ClothItem item, string style, List<PhysBoolBackup> boolBackups, List<PhysFloatBackup> floatBackups)
    {
        string itemName = item != null ? item.Name : "(null)";
        DebugLog(
            "[PHTY APPLY BEGIN] " + itemName + " style=" + style);

        int changed = 0;
        changed += SetPhysBool(item, AllowDetachNames(), true, boolBackups);
        changed += SetPhysFloat(item, DetachThresholdNames(), 0.001f, floatBackups);

        if (style == PHYS_UP)
        {
            changed += SetPhysFloat(item, GravityNames(), -2.0f, floatBackups);
            changed += SetPhysFloat(item, WeightNames(), 10.0f, floatBackups);
            changed += SetPhysFloat(item, DragNames(), 0.06f, floatBackups);
        }
        else if (style == PHYS_DOWN)
        {
            changed += SetPhysFloat(item, GravityNames(), 2.0f, floatBackups);
            changed += SetPhysFloat(item, WeightNames(), 10.0f, floatBackups);
            changed += SetPhysFloat(item, DragNames(), 0.06f, floatBackups);
        }

        DebugLog(
            "[PHTY APPLY SUMMARY] " + itemName +
            " changed=" + changed.ToString(CultureInfo.InvariantCulture) +
            " boolBackup=" + (boolBackups != null ? boolBackups.Count.ToString(CultureInfo.InvariantCulture) : "0") +
            " floatBackup=" + (floatBackups != null ? floatBackups.Count.ToString(CultureInfo.InvariantCulture) : "0"));

        return changed;
    }

    private void RestorePhysicalSettings(List<PhysBoolBackup> boolBackups, List<PhysFloatBackup> floatBackups)
    {
        if (boolBackups != null)
        {
            for (int i = 0; i < boolBackups.Count; i++)
            {
                PhysBoolBackup b = boolBackups[i];
                if (b != null && b.Param != null)
                {
                    bool before = b.Param.val;
                    b.Param.val = b.Value;
                    DebugLog("[RESTORE PHTY BOOL VERIFY] " + b.Name + " : " + (before ? "True" : "False") + " -> " + (b.Param.val ? "True" : "False") + " / saved=" + (b.Value ? "True" : "False"));
                }
            }
        }

        if (floatBackups != null)
        {
            for (int i = 0; i < floatBackups.Count; i++)
            {
                PhysFloatBackup f = floatBackups[i];
                if (f != null && f.Param != null)
                {
                    float before = f.Param.val;
                    f.Param.val = f.Value;
                    DebugLog("[RESTORE PHTY FLOAT VERIFY] " + f.Name + " : " + before.ToString("F3", CultureInfo.InvariantCulture) + " -> " + f.Param.val.ToString("F3", CultureInfo.InvariantCulture) + " / saved=" + f.Value.ToString("F3", CultureInfo.InvariantCulture));
                }
            }
        }
    }

private void CaptureFadeScanState(ClothItem item)
{
    if (item == null || string.IsNullOrEmpty(item.Prefix))
        return;

    List<SavedPhysFloat> floats = new List<SavedPhysFloat>();
    CapturePhysFloat(item, FadeFloatNames(), floats);
    scanFadeFloatStates[item.Prefix] = floats;

    for (int i = 0; i < floats.Count; i++)
    {
        SavedPhysFloat saved = floats[i];
        if (saved == null)
            continue;

        DebugLog("[FADE CAPTURE] " +
            item.Name + " / " +
            saved.StorableId + "/" +
            saved.ParamName + " = " +
            saved.Value.ToString("F3", CultureInfo.InvariantCulture));
    }
}
    private int RestoreFadeScanStateAll()
    {
        int restored = 0;
        for (int i = 0; i < clothes.Count; i++)
            restored += RestoreFadeScanState(clothes[i]);
        return restored;
    }

private int RestoreFadeScanState(ClothItem item)
{
    if (selectedPerson == null || item == null || string.IsNullOrEmpty(item.Prefix))
        return 0;

    int restored = 0;
    List<SavedPhysFloat> floats;
    if (scanFadeFloatStates.TryGetValue(item.Prefix, out floats) && floats != null)
    {
        for (int i = 0; i < floats.Count; i++)
        {
            SavedPhysFloat saved = floats[i];
            if (saved == null)
                continue;

            JSONStorable st = selectedPerson.GetStorableByID(saved.StorableId);
            if (st == null)
                continue;

            JSONStorableFloat param = st.GetFloatJSONParam(saved.ParamName);
            if (param == null)
                continue;

            DebugLog("[FADE RESTORE CHECK] " +
                item.Name + " / " +
                saved.StorableId + "/" +
                saved.ParamName + " current=" +
                param.val.ToString("F3", CultureInfo.InvariantCulture) +
                " saved=" +
                saved.Value.ToString("F3", CultureInfo.InvariantCulture));

            if (Mathf.Abs(param.val - saved.Value) > 0.0001f)
            {
                float before = param.val;
                param.val = saved.Value;

                DebugLog("[FADE RESTORE] " +
                    item.Name + " / " +
                    saved.StorableId + "/" +
                    saved.ParamName + " : " +
                    before.ToString("F3", CultureInfo.InvariantCulture) +
                    " -> " +
                    param.val.ToString("F3", CultureInfo.InvariantCulture));

                restored++;
            }
        }
    }

    return restored;
}

    private void CapturePhysicalScanState(ClothItem item)
    {
        if (item == null || string.IsNullOrEmpty(item.Prefix))
            return;

        List<SavedPhysBool> bools = new List<SavedPhysBool>();
        List<SavedPhysFloat> floats = new List<SavedPhysFloat>();

        CapturePhysBool(item, AllowDetachNames(), bools);
        CapturePhysBool(item, SimEnabledNames(), bools);
        CapturePhysBool(item, CollisionEnabledNames(), bools);
        CapturePhysFloat(item, DetachThresholdNames(), floats);
        CapturePhysFloat(item, GravityNames(), floats);
        CapturePhysFloat(item, WeightNames(), floats);
        CapturePhysFloat(item, DragNames(), floats);

        scanPhysBoolStates[item.Prefix] = bools;
        scanPhysFloatStates[item.Prefix] = floats;
    }

    private void CapturePhysBool(ClothItem item, string[] names, List<SavedPhysBool> list)
    {
        string foundName;
        string storableId;
        JSONStorableBool param = FindPhysBool(item, names, out foundName, out storableId);
        if (param == null)
            return;

        SavedPhysBool saved = new SavedPhysBool();
        saved.StorableId = storableId;
        saved.ParamName = foundName;
        saved.Value = param.val;
        list.Add(saved);
    }

    private void CapturePhysFloat(ClothItem item, string[] names, List<SavedPhysFloat> list)
    {
        string foundName;
        string storableId;
        JSONStorableFloat param = FindPhysFloat(item, names, out foundName, out storableId);
        if (param == null)
            return;

        SavedPhysFloat saved = new SavedPhysFloat();
        saved.StorableId = storableId;
        saved.ParamName = foundName;
        saved.Value = param.val;
        list.Add(saved);
    }

    private int RestorePhysicalScanStateAll()
    {
        int restored = 0;
        for (int i = 0; i < clothes.Count; i++)
            restored += RestorePhysicalScanState(clothes[i]);
        return restored;
    }

    private int RestorePhysicalScanState(ClothItem item)
    {
        if (selectedPerson == null || item == null || string.IsNullOrEmpty(item.Prefix))
            return 0;

        int restored = 0;

        List<SavedPhysBool> bools;
        if (scanPhysBoolStates.TryGetValue(item.Prefix, out bools) && bools != null)
        {
            for (int i = 0; i < bools.Count; i++)
            {
                SavedPhysBool saved = bools[i];
                if (saved == null)
                    continue;

                JSONStorable st = selectedPerson.GetStorableByID(saved.StorableId);
                if (st == null)
                    continue;

                JSONStorableBool param = st.GetBoolJSONParam(saved.ParamName);
                if (param == null)
                    continue;

                bool beforeBool = param.val;
                param.val = saved.Value;
                DebugLog("[RESTORE PHYS BOOL VERIFY] " + item.Name + " / " + saved.StorableId + "/" + saved.ParamName + " : " +
                    (beforeBool ? "True" : "False") + " -> " + (param.val ? "True" : "False") + " / saved=" + (saved.Value ? "True" : "False"));
                if (beforeBool != saved.Value)
                    restored++;
            }
        }

        List<SavedPhysFloat> floats;
        if (scanPhysFloatStates.TryGetValue(item.Prefix, out floats) && floats != null)
        {
            for (int i = 0; i < floats.Count; i++)
            {
                SavedPhysFloat saved = floats[i];
                if (saved == null)
                    continue;

                JSONStorable st = selectedPerson.GetStorableByID(saved.StorableId);
                if (st == null)
                    continue;

                JSONStorableFloat param = st.GetFloatJSONParam(saved.ParamName);
                if (param == null)
                    continue;

                float beforeFloat = param.val;
                param.val = saved.Value;
                DebugLog("[RESTORE PHYS FLOAT VERIFY] " + item.Name + " / " + saved.StorableId + "/" + saved.ParamName + " : " +
                    beforeFloat.ToString("F3", CultureInfo.InvariantCulture) + " -> " + param.val.ToString("F3", CultureInfo.InvariantCulture) + " / saved=" + saved.Value.ToString("F3", CultureInfo.InvariantCulture));
                if (Mathf.Abs(beforeFloat - saved.Value) > 0.0001f)
                    restored++;
            }
        }

        return restored;
    }

    private int SetPhysBool(ClothItem item, string[] names, bool value, List<PhysBoolBackup> backups)
    {
        string foundName;
        string storableId;
        JSONStorableBool param = FindPhysBool(item, names, out foundName, out storableId);
        string itemName = item != null ? item.Name : "(null)";
        string nameText = (names != null && names.Length > 0) ? names[0] : "(unknown)";

        if (param == null)
        {
            DebugLog(
                "[PHTY APPLY MISS BOOL] " + itemName + " / " + nameText);
            return 0;
        }

        bool before = param.val;
        string fullName = storableId + "/" + foundName;

        if (before == value)
        {
            DebugLog(
                "[PHTY APPLY KEEP BOOL] " + itemName + " / " + fullName + " : " +
                (before ? "True" : "False") + " already");
            return 0;
        }

        AddPhysBoolBackupOnce(backups, param, fullName);
        param.val = value;

        DebugLog(
            "[PHTY APPLY BOOL] " + itemName + " / " + fullName + " : " +
            (before ? "True" : "False") + " -> " + (param.val ? "True" : "False"));

        return 1;
    }

    private int SetPhysFloat(ClothItem item, string[] names, float value, List<PhysFloatBackup> backups)
    {
        string foundName;
        string storableId;
        JSONStorableFloat param = FindPhysFloat(item, names, out foundName, out storableId);
        string itemName = item != null ? item.Name : "(null)";
        string nameText = (names != null && names.Length > 0) ? names[0] : "(unknown)";

        if (param == null)
        {
            DebugLog(
                "[PHTY APPLY MISS FLOAT] " + itemName + " / " + nameText);
            return 0;
        }

        float before = param.val;
        string fullName = storableId + "/" + foundName;

        if (Mathf.Abs(before - value) < 0.0001f)
        {
            DebugLog(
                "[PHTY APPLY KEEP FLOAT] " + itemName + " / " + fullName + " : " +
                before.ToString("F3", CultureInfo.InvariantCulture) + " already");
            return 0;
        }

        AddPhysFloatBackupOnce(backups, param, fullName);
        param.val = value;

        DebugLog(
            "[PHTY APPLY FLOAT] " + itemName + " / " + fullName + " : " +
            before.ToString("F3", CultureInfo.InvariantCulture) + " -> " +
            param.val.ToString("F3", CultureInfo.InvariantCulture));

        return 1;
    }

    private void AddPhysBoolBackupOnce(List<PhysBoolBackup> backups, JSONStorableBool param, string name)
    {
        if (backups == null || param == null)
            return;

        for (int i = 0; i < backups.Count; i++)
        {
            if (backups[i] != null && backups[i].Param == param)
                return;
        }

        PhysBoolBackup b = new PhysBoolBackup();
        b.Param = param;
        b.Value = param.val;
        b.Name = name;
        backups.Add(b);
    }

    private void AddPhysFloatBackupOnce(List<PhysFloatBackup> backups, JSONStorableFloat param, string name)
    {
        if (backups == null || param == null)
            return;

        for (int i = 0; i < backups.Count; i++)
        {
            if (backups[i] != null && backups[i].Param == param)
                return;
        }

        PhysFloatBackup f = new PhysFloatBackup();
        f.Param = param;
        f.Value = param.val;
        f.Name = name;
        backups.Add(f);
    }

    private JSONStorableBool FindPhysBool(ClothItem item, string[] names, out string foundName, out string storableId)
    {
        foundName = "";
        storableId = "";
        if (selectedPerson == null || item == null || names == null)
            return null;

        List<string> ids = GetRelatedStorableIds(item);
        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableBool b = st.GetBoolJSONParam(names[n]);
                if (b != null)
                {
                    foundName = names[n];
                    storableId = ids[i];
                    return b;
                }
            }
        }

        return null;
    }

    private JSONStorableFloat FindPhysFloat(ClothItem item, string[] names, out string foundName, out string storableId)
    {
        foundName = "";
        storableId = "";
        if (selectedPerson == null || item == null || names == null)
            return null;

        List<string> ids = GetRelatedStorableIds(item);
        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat f = st.GetFloatJSONParam(names[n]);
                if (f != null)
                {
                    foundName = names[n];
                    storableId = ids[i];
                    return f;
                }
            }
        }

        return null;
    }

    private List<string> GetRelatedStorableIds(ClothItem item)
    {
        List<string> ids = new List<string>();
        if (selectedPerson == null || item == null || string.IsNullOrEmpty(item.Prefix))
            return ids;

        string lowerPrefix = item.Prefix.ToLowerInvariant();
        foreach (string storableId in selectedPerson.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;

            string lower = storableId.ToLowerInvariant();
            if (!lower.StartsWith(lowerPrefix))
                continue;

            if (IsIgnoredClothStorable(storableId))
                continue;

            ids.Add(storableId);
        }

        ids.Sort();
        return ids;
    }

    private string[] SimEnabledNames()
    {
        return new string[] { "simEnabled", "Sim Enabled", "simulationEnabled", "Simulation Enabled", "enableSimulation", "Enable Simulation" };
    }

    private string[] CollisionEnabledNames()
    {
        return new string[] { "collisionEnabled", "Collision Enabled", "clothCollision", "Cloth Collision", "collisions", "Collisions" };
    }

    private string[] AllowDetachNames()
    {
        return new string[] { "allowDetach", "Allow Detach", "allow Undress", "Allow Undress", "undress", "Undress" };
    }

    private string[] DetachThresholdNames()
    {
        return new string[] { "detachThreshold", "Detach Threshold", "undressThreshold", "Undress Threshold" };
    }

    private string[] GravityNames()
    {
        return new string[] { "gravityMultiplier", "Gravity Multiplier", "gravity multiplier", "gravity", "Gravity" };
    }

    private string[] WeightNames()
    {
        return new string[] { "weight", "Weight" };
    }

    private string[] DragNames()
    {
        return new string[] { "drag", "Drag" };
    }

    private void StopAutoIfMode(string mode)
    {
        if (autoModeChooser != null && autoModeChooser.val == mode)
        {
            autoModeChooser.val = AUTO_OFF;
            autoTimer = 0.0f;
            autoLoopHideDirection = true;
        }
    }

    private bool HasNextHideCandidate()
    {
        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (IsProtected(item))
                continue;

            if (!IsHidden(item))
                return true;
        }

        return false;
    }

    private bool HasPrevWearCandidate()
    {
        for (int i = orderedClothes.Count - 1; i >= 0; i--)
        {
            ClothItem item = orderedClothes[i];
            if (IsProtected(item))
                continue;

            if (HasAnyHiddenMaterial(item))
                return true;
        }

        return false;
    }

    private void RestoreAllVisible()
    {
        DebugLog("[RESTORE CALLER] RestoreAllVisible ENTER / wearNoneRoutine=" +
            (wearNoneRoutine != null ? "1" : "0") +
            " / physicalRoutine=" + (physicalHideRoutine != null ? "1" : "0") +
            " / busy=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture) +
            " / auto=" + (autoModeChooser != null ? autoModeChooser.val : "(null)") +
            " / phty=" + (physRemoveStyleChooser != null ? physRemoveStyleChooser.val : "(null)"));

        LogButtonPressed("WEAR ALL");
        // WEAR ALL は「服を全部着ている見た目に戻す」専用。
        // WEAR NONE + PHTY が実行中なら、まず確実にキャンセルしてから全着衣へ戻す。
        if (wearNoneRoutine != null)
        {
            wearNoneCancelVersion++;
            DebugLog("[RESTORE CALLER] cancel wearNoneRoutine / cancelVersion=" + wearNoneCancelVersion.ToString(CultureInfo.InvariantCulture));
            try { StopCoroutine(wearNoneRoutine); } catch { }
            wearNoneRoutine = null;
        }

        if (physicalRemoveRunning > 0 || physicalHideRoutine != null)
        {
            DebugLog("[RESTORE CALLER] stop active physical before WEAR ALL");
            StopActivePhysicalRoutine(true);
            physicalRemoveRunning = 0;
        }

        // 内部状態の完全初期化は RESET RUNTIME / STOP 側に分離する。
        StopAutoAndResetRunningNoRestore("Restore all start");

        if (restoreAllRoutine != null)
        {
            try { StopCoroutine(restoreAllRoutine); } catch { }
            restoreAllRoutine = null;
        }

        restoreAllRoutine = StartCoroutine(RestoreAllVisibleRoutine());
    }
private void StopAutoAndResetRunningNoRestore(string reason)
{
    DebugLog("[NO RESTORE STOP] ENTER reason=" + reason +
        " / wearNoneRoutine=" + (wearNoneRoutine != null ? "1" : "0") +
        " / restoreAllRoutine=" + (restoreAllRoutine != null ? "1" : "0") +
        " / physicalRoutine=" + (physicalHideRoutine != null ? "1" : "0") +
        " / busy=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture));

    if (autoModeChooser != null)
        autoModeChooser.val = AUTO_OFF;

    autoTimer = 0.0f;
    autoLoopHideDirection = true;

    if (restoreAllRoutine != null)
    {
        try { StopCoroutine(restoreAllRoutine); } catch { }
        restoreAllRoutine = null;
    }

    if (wearNoneRoutine != null)
    {
        wearNoneCancelVersion++;
        DebugLog("[NO RESTORE STOP] cancel wearNoneRoutine / cancelVersion=" + wearNoneCancelVersion.ToString(CultureInfo.InvariantCulture));
        try { StopCoroutine(wearNoneRoutine); } catch { }
        wearNoneRoutine = null;
    }

    StopActivePhysicalRoutine(false);
    physicalRemoveRunning = 0;
    SetStatus(reason);
    UpdateButtonColors();
}
    private IEnumerator RestoreAllVisibleRoutine()
    {
        // v11f: First re-enable the real Clothing Item checkboxes that this plugin turned OFF.
        // Do this before clearing internal hidden flags; otherwise active-OFF clothes can lose their restore path.
        int activeRestored0 = RestoreCapturedDazClothingActiveAll("RESTORE_ALL_ACTIVE_PRE");
        if (activeRestored0 > 0)
            yield return new WaitForSeconds(0.45f);

        forcedHiddenPrefixes.Clear();
        phtyHiddenPrefixes.Clear();
        // WEAR ALL は「見た目を着ている状態に戻す」だけに集中する。
        // Reload All は環境によってMaterial参照が消えるため、自動では呼ばない。
        DumpAllSimState("RESTORE_ALL_BEFORE_PHYS_SCAN_PASS1");
        int physRestored1 = RestorePhysicalScanStateAll();
        DumpAllSimState("RESTORE_ALL_AFTER_PHYS_SCAN_PASS1");
        int fadeRestored1 = RestoreFadeScanStateAll();
        int alphaRestored1 = ForceAlphaAdjustVisibleAll();
        DebugLog("[RESTORE ALL TRACE] pass1 pre / phys=" + physRestored1.ToString(CultureInfo.InvariantCulture) +
            " / fade=" + fadeRestored1.ToString(CultureInfo.InvariantCulture) +
            " / alpha=" + alphaRestored1.ToString(CultureInfo.InvariantCulture));

        for (int i = 0; i < clothes.Count; i++)
            RestoreVisibleForItem(clothes[i], "RESTORE_ALL_PASS1");

        // Fade/Material更新が1フレーム遅れる服への保険。
        yield return new WaitForSeconds(0.25f);

        DumpAllSimState("RESTORE_ALL_BEFORE_PHYS_SCAN_PASS2");
        int physRestored2 = RestorePhysicalScanStateAll();
        DumpAllSimState("RESTORE_ALL_AFTER_PHYS_SCAN_PASS2");
        int fadeRestored2 = RestoreFadeScanStateAll();
        int alphaRestored2 = ForceAlphaAdjustVisibleAll();
        DebugLog("[RESTORE ALL TRACE] pass2 pre / phys=" + physRestored2.ToString(CultureInfo.InvariantCulture) +
            " / fade=" + fadeRestored2.ToString(CultureInfo.InvariantCulture) +
            " / alpha=" + alphaRestored2.ToString(CultureInfo.InvariantCulture));

        for (int i = 0; i < clothes.Count; i++)
            RestoreVisibleForItem(clothes[i], "RESTORE_ALL_PASS2");

        // v10w: PHTYで落ちたSIMメッシュはMaterial/Alpha/物理値だけでは戻らない服がある。
        // WEAR ALLの最後に、見えているSIM服へ reset receiver を打って布メッシュを再初期化する。
        int simResetAll = ResetSimForAllVisibleItems("RESTORE_ALL_FINAL_SIM_RESET");

        RebuildOrderOnly();
        string msg = "Wear all visible / active=" + activeRestored0.ToString(CultureInfo.InvariantCulture) +
            " / phys=" + (physRestored1 + physRestored2).ToString(CultureInfo.InvariantCulture) +
            " / fade=" + (fadeRestored1 + fadeRestored2).ToString(CultureInfo.InvariantCulture) +
            " / alpha=" + (alphaRestored1 + alphaRestored2).ToString(CultureInfo.InvariantCulture) +
            " / simReset=" + simResetAll.ToString(CultureInfo.InvariantCulture) +
            " / reload=0";
        SetStatus(msg);
        PublishClothState("restore_all", null, true);
        UpdateButtonColors();
        UpdatePreview();
        restoreAllRoutine = null;
    }

    private void ReloadAllOnly()
    {
        // v9: ReloadはMaterial参照を壊すケースがあるため通常使用しない。
        // 互換用にメソッドは残すが、UI/Actionからは呼ばない。
        SetStatus("Reload All disabled in v9 safe mode");
        UpdateButtonColors();
        UpdatePreview();
    }

    private bool IsHidden(ClothItem item)
    {
        if (selectedPerson == null || item == null)
            return true;

        // v11e: when the real Clothing Item checkbox was turned OFF by this plugin,
        // Material storables may disappear. Treat it as hidden so PREV/WEAR ALL can restore it.
        if (!string.IsNullOrEmpty(item.Prefix) && activeDazClothingOffByPlugin.Contains(item.Prefix))
            return true;

        if (item.Materials == null || item.Materials.Count == 0)
            return true;

        // Reload/Reset後はMaterial Storableが作り直され、保存IDが古くなることがある。
        // その場合は現在のStorableから同じPrefixのhideMaterialを再解決してから判定する。
        if (CountValidMaterials(item, true) == 0)
            return false;

        int found = 0;
        int hidden = 0;
        for (int i = 0; i < item.Materials.Count; i++)
        {
            SavedBool material = item.Materials[i];
            if (material == null)
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(material.StorableId);
            if (storable == null)
                continue;

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam(material.ParamName);
            if (hideMaterial == null)
                continue;

            found++;
            if (hideMaterial.val)
                hidden++;
        }

        return found > 0 && hidden >= found;
    }

    private bool HasAnyHiddenMaterial(ClothItem item)
    {
        if (selectedPerson == null || item == null)
            return false;

        // v11e: Clothing Item checkbox OFF means hidden even if hideMaterial storables are unloaded.
        if (!string.IsNullOrEmpty(item.Prefix) && activeDazClothingOffByPlugin.Contains(item.Prefix))
            return true;

        if (item.Materials == null || item.Materials.Count == 0)
            return false;

        if (CountValidMaterials(item, true) == 0)
            return false;

        for (int i = 0; i < item.Materials.Count; i++)
        {
            SavedBool material = item.Materials[i];
            if (material == null)
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(material.StorableId);
            if (storable == null)
                continue;

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam(material.ParamName);
            if (hideMaterial != null && hideMaterial.val)
                return true;
        }

        return false;
    }

    private void DumpHideMaterialState(ClothItem item, string tag)
    {
        if (!IsDebugLogEnabled())
            return;

        if (selectedPerson == null || item == null || item.Materials == null)
        {
            DebugLog("[HIDEMAT " + tag + "] item/materials null");
            return;
        }

        int found = 0;
        int hiddenCount = 0;
        int missing = 0;
        for (int i = 0; i < item.Materials.Count; i++)
        {
            SavedBool material = item.Materials[i];
            if (material == null)
            {
                missing++;
                continue;
            }

            JSONStorable storable = selectedPerson.GetStorableByID(material.StorableId);
            if (storable == null)
            {
                missing++;
                DebugLog("[HIDEMAT " + tag + "] " + item.Name + " / " + (i + 1).ToString(CultureInfo.InvariantCulture) + "/" + item.Materials.Count.ToString(CultureInfo.InvariantCulture) + " / " + material.StorableId + " / storable=null");
                continue;
            }

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam(material.ParamName);
            if (hideMaterial == null)
            {
                missing++;
                DebugLog("[HIDEMAT " + tag + "] " + item.Name + " / " + (i + 1).ToString(CultureInfo.InvariantCulture) + "/" + item.Materials.Count.ToString(CultureInfo.InvariantCulture) + " / " + material.StorableId + "/" + material.ParamName + " / param=null");
                continue;
            }

            found++;
            if (hideMaterial.val)
                hiddenCount++;

            DebugLog("[HIDEMAT " + tag + "] " + item.Name + " / " + (i + 1).ToString(CultureInfo.InvariantCulture) + "/" + item.Materials.Count.ToString(CultureInfo.InvariantCulture) + " / " + material.StorableId + "/" + material.ParamName + " = " + (hideMaterial.val ? "1" : "0"));
        }

        DebugLog("[HIDEMAT " + tag + " SUMMARY] " + item.Name +
            " found=" + found.ToString(CultureInfo.InvariantCulture) +
            " hidden=" + hiddenCount.ToString(CultureInfo.InvariantCulture) +
            " missing=" + missing.ToString(CultureInfo.InvariantCulture) +
            " isHidden=" + (IsHidden(item) ? "1" : "0") +
            " anyHidden=" + (HasAnyHiddenMaterial(item) ? "1" : "0"));
    }

    private int SetHidden(ClothItem item, bool hidden)
    {
        if (selectedPerson == null || item == null)
            return 0;

        DebugLog("[SET HIDDEN ENTER] " + item.Name + " hidden=" + (hidden ? "1" : "0"));
        DumpHideMaterialState(item, "SET_ENTER");

        int activeToggleFound = 0;
        // v11d: only re-enable the exact DAZ clothing item that this plugin previously turned OFF.
        // Do not fuzzy-search and do not enable anything that was not captured at SCAN time.
        if (!hidden)
            activeToggleFound = SetCapturedDazClothingActive(item, true, "SET_VISIBLE_PRE");

        // 操作直前にMaterial参照を再確認。古ければ現在のStorableから再解決する。
        if (CountValidMaterials(item, false) == 0)
        {
            DebugLog("[SET HIDDEN REFRESH] " + item.Name + " / material refs stale, refreshing");
            RefreshItemMaterials(item);
            DumpHideMaterialState(item, "SET_AFTER_REFRESH");
        }

        int changed = 0;
        int found = 0;
        int errors = 0;
        for (int i = 0; i < item.Materials.Count; i++)
        {
            SavedBool material = item.Materials[i];
            if (material == null)
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(material.StorableId);
            if (storable == null)
            {
                DebugLog("[HIDEMAT MISS] " + item.Name + " / " + material.StorableId + " / storable=null");
                continue;
            }

            JSONStorableBool hideMaterial = storable.GetBoolJSONParam(material.ParamName);
            if (hideMaterial == null)
            {
                DebugLog("[HIDEMAT MISS] " + item.Name + " / " + material.StorableId + "/" + material.ParamName + " / param=null");
                continue;
            }

            found++;
            bool before = hideMaterial.val;
            if (before != hidden)
                changed++;

            try
            {
                hideMaterial.val = hidden;
                DebugLog("[HIDEMAT SET] " + item.Name + " / " + material.StorableId + "/" + material.ParamName + " : " + (before ? "1" : "0") + " -> " + (hideMaterial.val ? "1" : "0"));
            }
            catch (Exception e)
            {
                errors++;
                SuperController.LogMessage("[ClothStateSwitcher] [SET HIDDEN ERROR] " + item.Name + " / " + material.StorableId + "/" + material.ParamName + " / hidden=" + (hidden ? "1" : "0") + " / " + "Exception" + " : " + e.Message);
            }
        }

        // v11d: after hideMaterial is set, turn the Clothing Item checkbox OFF, but only for the exact active item captured at SCAN.
        if (hidden)
            activeToggleFound = SetCapturedDazClothingActive(item, false, "SET_HIDE_FINAL");

        int foundTotal = Mathf.Max(found, activeToggleFound);

        DebugLog("[SET HIDDEN] " + item.Name + " hidden=" + (hidden ? "1" : "0") +
            " found=" + found.ToString(CultureInfo.InvariantCulture) +
            " activeToggle=" + activeToggleFound.ToString(CultureInfo.InvariantCulture) +
            " foundTotal=" + foundTotal.ToString(CultureInfo.InvariantCulture) +
            " changed=" + changed.ToString(CultureInfo.InvariantCulture) +
            " errors=" + errors.ToString(CultureInfo.InvariantCulture));
        DumpHideMaterialState(item, "SET_EXIT");
        DebugLog("[SET HIDDEN EXIT] " + item.Name + " hidden=" + (hidden ? "1" : "0") +
            " found=" + found.ToString(CultureInfo.InvariantCulture) +
            " activeToggle=" + activeToggleFound.ToString(CultureInfo.InvariantCulture) +
            " foundTotal=" + foundTotal.ToString(CultureInfo.InvariantCulture) +
            " changed=" + changed.ToString(CultureInfo.InvariantCulture) +
            " errors=" + errors.ToString(CultureInfo.InvariantCulture));

        return foundTotal;
    }

    private string NormalizeDazClothingKey(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        string s = value.ToLowerInvariant();
        s = s.Replace(":", " " );
        s = s.Replace("_", " " );
        s = s.Replace("-", " " );
        s = s.Replace(".", " " );
        while (s.IndexOf("  ") >= 0)
            s = s.Replace("  ", " " );
        return s.Trim();
    }

    private void AddDazMatchCandidate(List<string> list, string value)
    {
        if (list == null || string.IsNullOrEmpty(value))
            return;

        string normalized = NormalizeDazClothingKey(value);
        if (normalized.Length < 3)
            return;

        if (!list.Contains(normalized))
            list.Add(normalized);
    }

    private List<string> GetExactDazItemCandidates(ClothItem item)
    {
        List<string> list = new List<string>();
        if (item == null)
            return list;

        AddDazMatchCandidate(list, item.Prefix);
        AddDazMatchCandidate(list, item.Name);

        if (!string.IsNullOrEmpty(item.Prefix))
        {
            int colon = item.Prefix.LastIndexOf(':');
            if (colon >= 0 && colon + 1 < item.Prefix.Length)
                AddDazMatchCandidate(list, item.Prefix.Substring(colon + 1));
        }

        if (item.Materials != null)
        {
            for (int i = 0; i < item.Materials.Count; i++)
            {
                SavedBool m = item.Materials[i];
                if (m == null || string.IsNullOrEmpty(m.StorableId))
                    continue;
                string prefix = GetWearablePrefix(m.StorableId);
                AddDazMatchCandidate(list, prefix);
                AddDazMatchCandidate(list, CleanupName(prefix));
                int colon = prefix.LastIndexOf(':');
                if (colon >= 0 && colon + 1 < prefix.Length)
                    AddDazMatchCandidate(list, prefix.Substring(colon + 1));
            }
        }

        return list;
    }

    private DAZCharacterSelector GetGeometrySelectorSafe()
    {
        if (selectedPerson == null)
            return null;

        JSONStorable st = selectedPerson.GetStorableByID("geometry");
        return st as DAZCharacterSelector;
    }

    private string GetDazClothingDisplayNameSafe(DAZClothingItem clothing)
    {
        if (clothing == null)
            return "";
        try { return clothing.displayName; } catch { }
        return "";
    }

    private void CaptureActiveDazClothingNameAtScan(ClothItem item)
    {
        if (item == null || string.IsNullOrEmpty(item.Prefix))
            return;

        DAZCharacterSelector geometry = GetGeometrySelectorSafe();
        if (geometry == null || geometry.clothingItems == null)
        {
            DebugLog("[DAZ ACTIVE CAPTURE] " + item.Name + " skip=no-geometry");
            return;
        }

        List<string> candidates = GetExactDazItemCandidates(item);
        int matchCount = 0;
        string matchedDisplay = "";
        DAZClothingItem matchedItem = null;

        try
        {
            List<DAZClothingItem> all = geometry.clothingItems.ToList();
            for (int i = 0; i < all.Count; i++)
            {
                DAZClothingItem clothing = all[i];
                if (clothing == null || !clothing.active)
                    continue;

                string display = GetDazClothingDisplayNameSafe(clothing);
                string key = NormalizeDazClothingKey(display);
                if (string.IsNullOrEmpty(key))
                    continue;

                if (candidates.Contains(key))
                {
                    matchCount++;
                    matchedDisplay = display;
                    matchedItem = clothing;
                }
            }
        }
        catch (Exception e)
        {
            DebugLog("[DAZ ACTIVE CAPTURE ERROR] " + item.Name + " / " + e.Message);
            return;
        }

        if (matchCount == 1 && matchedItem != null)
        {
            activeDazClothingNameByPrefix[item.Prefix] = matchedDisplay;
            activeDazClothingItemByPrefix[item.Prefix] = matchedItem;
            DebugLog("[DAZ ACTIVE CAPTURE] " + item.Name + " prefix=" + item.Prefix + " display=" + matchedDisplay + " / ref=1");
        }
        else
        {
            DebugLog("[DAZ ACTIVE CAPTURE MISS] " + item.Name + " prefix=" + item.Prefix + " matches=" + matchCount.ToString(CultureInfo.InvariantCulture) + " candidates=" + string.Join(" | ", candidates.ToArray()));
        }
    }

    private int RestoreCapturedDazClothingActiveAll(string context)
    {
        int restored = 0;
        if (activeDazClothingOffByPlugin.Count <= 0)
            return 0;

        // Copy first because SetCapturedDazClothingActive(true) removes from activeDazClothingOffByPlugin.
        List<string> prefixes = new List<string>(activeDazClothingOffByPlugin);
        for (int i = 0; i < prefixes.Count; i++)
        {
            string prefix = prefixes[i];
            ClothItem item = FindClothItemByPrefix(prefix);
            if (item == null)
            {
                DebugLog("[DAZ ACTIVE RESTORE ALL SKIP] context=" + context + " prefix=" + prefix + " reason=no-cloth-item");
                continue;
            }

            restored += SetCapturedDazClothingActive(item, true, context);
        }

        DebugLog("[DAZ ACTIVE RESTORE ALL] context=" + context +
            " restored=" + restored.ToString(CultureInfo.InvariantCulture) +
            " remainingOff=" + activeDazClothingOffByPlugin.Count.ToString(CultureInfo.InvariantCulture));
        return restored;
    }

    private ClothItem FindClothItemByPrefix(string prefix)
    {
        if (string.IsNullOrEmpty(prefix))
            return null;

        for (int i = 0; i < clothes.Count; i++)
        {
            ClothItem item = clothes[i];
            if (item != null && item.Prefix == prefix)
                return item;
        }

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item != null && item.Prefix == prefix)
                return item;
        }

        return null;
    }

    private int SetCapturedDazClothingActive(ClothItem item, bool active, string context)
    {
        if (item == null || string.IsNullOrEmpty(item.Prefix))
            return 0;

        string capturedDisplay;
        if (!activeDazClothingNameByPrefix.TryGetValue(item.Prefix, out capturedDisplay) || string.IsNullOrEmpty(capturedDisplay))
        {
            DebugLog("[DAZ ACTIVE SET SKIP] context=" + context + " item=" + item.Name + " reason=no-captured-active-name");
            return 0;
        }

        // Re-enable only what this plugin disabled during the current runtime.
        if (active && !activeDazClothingOffByPlugin.Contains(item.Prefix))
        {
            DebugLog("[DAZ ACTIVE SET SKIP] context=" + context + " item=" + item.Name + " reason=not-disabled-by-plugin");
            return 0;
        }

        DAZCharacterSelector geometry = GetGeometrySelectorSafe();
        if (geometry == null)
        {
            DebugLog("[DAZ ACTIVE SET SKIP] context=" + context + " item=" + item.Name + " reason=no-geometry");
            return 0;
        }

        DAZClothingItem capturedItem = null;
        activeDazClothingItemByPrefix.TryGetValue(item.Prefix, out capturedItem);
        if (capturedItem == null)
        {
            DebugLog("[DAZ ACTIVE SET SKIP] context=" + context + " item=" + item.Name + " reason=no-captured-item-ref display=" + capturedDisplay);
            return 0;
        }

        try
        {
            bool before = capturedItem.active;
            if (before != active)
                geometry.SetActiveClothingItem(capturedItem, active);

            if (active)
                activeDazClothingOffByPlugin.Remove(item.Prefix);
            else
                activeDazClothingOffByPlugin.Add(item.Prefix);

            lastAppearanceSignature = ComputeAppearanceSignature();

            DebugLog("[DAZ ACTIVE SET] context=" + context + " item=" + item.Name + " display=" + capturedDisplay + " : " + (before ? "1" : "0") + " -> " + (active ? "1" : "0") + " / ref=1");
            return 1;
        }
        catch (Exception e)
        {
            DebugLog("[DAZ ACTIVE SET ERROR] context=" + context + " item=" + item.Name + " display=" + capturedDisplay + " / " + e.Message);
            return 0;
        }
    }


    private void DumpAllSimState(string tag)
    {
        if (!IsDebugLogEnabled())
            return;

        for (int i = 0; i < clothes.Count; i++)
            DumpSimState(clothes[i], tag);
    }

    private string GetSimStateSummary(ClothItem item)
    {
        if (item == null)
            return "item=null";

        string foundName;
        string storableId;
        JSONStorableBool simEnabled = FindPhysBool(item, SimEnabledNames(), out foundName, out storableId);
        string simText = simEnabled != null ? (storableId + "/" + foundName + "=" + (simEnabled.val ? "1" : "0")) : "simEnabled=NA";

        JSONStorableBool collisionEnabled = FindPhysBool(item, CollisionEnabledNames(), out foundName, out storableId);
        string collisionText = collisionEnabled != null ? (storableId + "/" + foundName + "=" + (collisionEnabled.val ? "1" : "0")) : "collision=NA";

        JSONStorableBool allowDetach = FindPhysBool(item, AllowDetachNames(), out foundName, out storableId);
        string allowText = allowDetach != null ? (storableId + "/" + foundName + "=" + (allowDetach.val ? "1" : "0")) : "allowDetach=NA";

        JSONStorableFloat gravity = FindPhysFloat(item, GravityNames(), out foundName, out storableId);
        string gravityText = gravity != null ? (storableId + "/" + foundName + "=" + gravity.val.ToString("F3", CultureInfo.InvariantCulture)) : "gravity=NA";

        JSONStorableFloat weight = FindPhysFloat(item, WeightNames(), out foundName, out storableId);
        string weightText = weight != null ? (storableId + "/" + foundName + "=" + weight.val.ToString("F3", CultureInfo.InvariantCulture)) : "weight=NA";

        return simText + " | " + collisionText + " | " + allowText + " | " + gravityText + " | " + weightText;
    }

    private void DumpSimState(ClothItem item, string tag)
    {
        if (!IsDebugLogEnabled())
            return;

        if (item == null)
            return;
        DebugLog("[SIM STATE " + tag + "] " + item.Name + " / " + GetSimStateSummary(item));
    }

    private int RestoreVisibleForItem(ClothItem item, string context)
    {
        return PrepareVisualStateForAction(item, context, true);
    }
private int RestoreVisibleForItemNoVisualRestore(ClothItem item, string context)
{
    if (item == null)
        return 0;

    int physRestored = RestorePhysicalScanState(item);
    int fadeRestored = RestoreFadeScanState(item);
    int alphaBefore = ForceAlphaAdjustVisible(item);

    int found = SetHidden(item, false);

    int alphaAfter = 0;
    int simReset = 0;
    if (found > 0)
    {
        alphaAfter = ForceAlphaAdjustVisible(item);
        simReset = ResetSimForItem(item, context + "_AFTER_SET_VISIBLE");
    }

    DebugLog("[RESTORE VISIBLE SIMPLE] context=" + context +
        " name=" + item.Name +
        " found=" + found.ToString(CultureInfo.InvariantCulture) +
        " phys=" + physRestored.ToString(CultureInfo.InvariantCulture) +
        " fade=" + fadeRestored.ToString(CultureInfo.InvariantCulture) +
        " alphaBefore=" + alphaBefore.ToString(CultureInfo.InvariantCulture) +
        " alphaAfter=" + alphaAfter.ToString(CultureInfo.InvariantCulture) +
        " simReset=" + simReset.ToString(CultureInfo.InvariantCulture));

    return found;
}
    private int PrepareVisualStateForAction(ClothItem item, string context, bool forceVisible)
    {
        if (item == null)
            return 0;

        // v10c: Debug OFF時は重い診断Dump/全状態確認を走らせない。
        // hide側は「現在見えている対象を隠す」だけなので、Material参照の有効性だけ確認する。
        if (!IsDebugLogEnabled() && !forceVisible)
        {
            int fastMat = CountValidMaterials(item, false);
            if (fastMat <= 0)
            {
                RefreshItemMaterials(item);
                fastMat = CountValidMaterials(item, false);
            }
            return fastMat;
        }

        // v10c: Debug OFFのwear側もDumpなしで必要最低限のみ。
if (!IsDebugLogEnabled() && forceVisible)
{
    int physFast = RestorePhysicalScanState(item);
    int fadeFast = RestoreFadeScanState(item);
    int alphaFastBefore = ForceAlphaAdjustVisible(item);

    int setFoundFast = SetHidden(item, false);

    int alphaFastAfter = ForceAlphaAdjustVisible(item);
    int simResetFast = 0;
    if (setFoundFast > 0)
        simResetFast = ResetSimForItem(item, context + "_FAST_AFTER_SET_VISIBLE");

    DebugLog("[WEAR VISIBLE FAST RESTORE] context=" + context +
        " name=" + item.Name +
        " found=" + setFoundFast.ToString(CultureInfo.InvariantCulture) +
        " phys=" + physFast.ToString(CultureInfo.InvariantCulture) +
        " fade=" + fadeFast.ToString(CultureInfo.InvariantCulture) +
        " alphaBefore=" + alphaFastBefore.ToString(CultureInfo.InvariantCulture) +
        " alphaAfter=" + alphaFastAfter.ToString(CultureInfo.InvariantCulture) +
        " simReset=" + simResetFast.ToString(CultureInfo.InvariantCulture));

    return setFoundFast;
}

        int matBefore = CountValidMaterials(item, true);
        bool hiddenBefore = IsHidden(item);
        bool anyBefore = HasAnyHiddenMaterial(item);
        string alphaBefore = GetAlphaStateSummary(item);

        DumpMaterialValues(item, context + "_ENTER");
        DumpSimState(item, context + "_ENTER");

        int physRestored = RestorePhysicalScanState(item);
        DumpMaterialValues(item, context + "_AFTER_RESTORE_PHYS");
        DumpSimState(item, context + "_AFTER_RESTORE_PHYS");

        int fadeRestored = RestoreFadeScanState(item);
        DumpMaterialValues(item, context + "_AFTER_RESTORE_FADE");
        DumpSimState(item, context + "_AFTER_RESTORE_FADE");

TraceAlphaAdjustState(item, context + "_BEFORE_FORCE_ALPHA_VISIBLE");
        int alphaRestored = ForceAlphaAdjustVisible(item);
        TraceAlphaAdjustState(item, context + "_AFTER_FORCE_ALPHA_VISIBLE");
        DumpMaterialValues(item, context + "_AFTER_FORCE_ALPHA_VISIBLE");

        int setFound = -1;
        if (forceVisible)
        {
            setFound = SetHidden(item, false);
            DumpMaterialValues(item, context + "_AFTER_SET_VISIBLE_BEFORE_NUDGE");
TraceAlphaAdjustState(item, context + "_AFTER_SET_VISIBLE_BEFORE_NUDGE_TRACE");
            int alphaNudgeAfterVisible = ForceAlphaAdjustVisible(item);
            TraceAlphaAdjustState(item, context + "_AFTER_SET_VISIBLE_AFTER_NUDGE_TRACE");
            DebugLog("[ALPHA NUDGE AFTER VISIBLE] context=" + context +
                " name=" + item.Name +
                " count=" + alphaNudgeAfterVisible.ToString(CultureInfo.InvariantCulture));

            int simResetAfterVisible = ResetSimForItem(item, context + "_AFTER_SET_VISIBLE");
            DumpSimState(item, context + "_AFTER_SIM_RESET_VISIBLE");

            DumpMaterialValues(item, context + "_AFTER_SET_VISIBLE_AFTER_NUDGE");
            alphaRestored += alphaNudgeAfterVisible;
            physRestored += simResetAfterVisible;
        }
        else
        {
            DumpMaterialValues(item, context + "_NO_SET_VISIBLE");
        }

        int matAfter = CountValidMaterials(item, true);
        bool hiddenAfter = IsHidden(item);
        bool anyAfter = HasAnyHiddenMaterial(item);
        string alphaAfter = GetAlphaStateSummary(item);

        DebugLog("[VISUAL PREP] context=" + context +
            " name=" + item.Name +
            " prefix=" + item.Prefix +
            " forceVisible=" + (forceVisible ? "1" : "0") +
            " mat=" + matBefore.ToString(CultureInfo.InvariantCulture) + "->" + matAfter.ToString(CultureInfo.InvariantCulture) +
            " hidden=" + (hiddenBefore ? "1" : "0") + "->" + (hiddenAfter ? "1" : "0") +
            " any=" + (anyBefore ? "1" : "0") + "->" + (anyAfter ? "1" : "0") +
            " phys=" + physRestored.ToString(CultureInfo.InvariantCulture) +
            " fade=" + fadeRestored.ToString(CultureInfo.InvariantCulture) +
            " alphaChanged=" + alphaRestored.ToString(CultureInfo.InvariantCulture) +
            " setFound=" + setFound.ToString(CultureInfo.InvariantCulture) +
            " alphaBefore=" + alphaBefore +
            " alphaAfter=" + alphaAfter);

        if (context == "PREV" || context.IndexOf("RESTORE", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            DebugLog("[RESTORE ITEM] context=" + context +
                " name=" + item.Name +
                " prefix=" + item.Prefix +
                " mat=" + matBefore.ToString(CultureInfo.InvariantCulture) + "->" + matAfter.ToString(CultureInfo.InvariantCulture) +
                " hidden=" + (hiddenBefore ? "1" : "0") + "->" + (hiddenAfter ? "1" : "0") +
                " any=" + (anyBefore ? "1" : "0") + "->" + (anyAfter ? "1" : "0") +
                " phys=" + physRestored.ToString(CultureInfo.InvariantCulture) +
                " fade=" + fadeRestored.ToString(CultureInfo.InvariantCulture) +
                " alphaChanged=" + alphaRestored.ToString(CultureInfo.InvariantCulture) +
                " setFound=" + setFound.ToString(CultureInfo.InvariantCulture) +
                " alphaBefore=" + alphaBefore +
                " alphaAfter=" + alphaAfter);
        }

        if (forceVisible)
            return setFound;

        return matAfter;
    }

    private string GetAlphaStateSummary(ClothItem item)
    {
        if (selectedPerson == null || item == null)
            return "0";

        int found = 0;
        int nonZero = 0;
        float min = 9999f;
        float max = -9999f;
        string[] names = AlphaAdjustVisibleNames();
        List<string> ids = GetRelatedStorableIds(item);

        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable st = selectedPerson.GetStorableByID(ids[i]);
            if (st == null)
                continue;

            for (int n = 0; n < names.Length; n++)
            {
                JSONStorableFloat fp = st.GetFloatJSONParam(names[n]);
                if (fp == null)
                    continue;

                found++;
                if (Mathf.Abs(fp.val) > 0.0001f)
                    nonZero++;
                if (fp.val < min) min = fp.val;
                if (fp.val > max) max = fp.val;
            }
        }

        if (found <= 0)
            return "found=0";

        return "found=" + found.ToString(CultureInfo.InvariantCulture) +
            ",nonZero=" + nonZero.ToString(CultureInfo.InvariantCulture) +
            ",min=" + min.ToString("F3", CultureInfo.InvariantCulture) +
            ",max=" + max.ToString("F3", CultureInfo.InvariantCulture);
    }

    private bool IsProtected(ClothItem item)
    {
        if (item == null)
            return false;

        string s = GetLowerName(item);

        if (protectBraJSON != null && protectBraJSON.val && IsBra(s))
            return true;

        if (protectPantyJSON != null && protectPantyJSON.val && IsPanty(s))
            return true;

        if (protectShoeSockJSON != null && protectShoeSockJSON.val && IsShoeSock(s))
            return true;

        return false;
    }

    private void PublishClothState(string action, ClothItem item, bool sendReport)
    {
        // v10c: Debug OFFかつState Report to Py OFFなら、毎回の全衣装走査を避ける。
        // ダンス中の小さな引っかかり対策として、外部通知が不要な時は状態集計を省略する。
        bool reportEnabled = stateReportEnabledJSON != null && stateReportEnabledJSON.val;
        bool needFullState = IsDebugLogEnabled() || (sendReport && reportEnabled) || action == "scan" || action == "restore_scan" || action == "restore_all";

        if (needFullState)
        {
            if (action == "scan" || action == "restore_scan" || action == "restore_all" || action == "hide_done" || action == "wear_done" || action == "remove_bra" || action == "remove_panty" || item == null || !stateCacheValid)
                RefreshStateCacheFull(action, item);
            else
                UpdateStateCacheForItem(action, item);
        }
        else
        {
            UpdateLastStateLight(action, item);
        }

        if (!sendReport)
            return;

        if (!reportEnabled)
            return;

        string report = BuildStateReport(action, item);
        SendStateReportToPython(report);
        DebugLog("[CLOTH_STATE] " + report);
    }

    private void UpdateLastStateLight(string action, ClothItem item)
    {
        if (!string.IsNullOrEmpty(action) && stateLastActionJSON != null)
            stateLastActionJSON.val = action;

        if (item != null)
        {
            if (stateLastItemJSON != null)
                stateLastItemJSON.val = item.Name;
            if (stateLastCategoryJSON != null)
                stateLastCategoryJSON.val = GetClothCategory(item);
        }
    }

    private void UpdateExternalState()
    {
        UpdateExternalState(null, null);
    }

    private void UpdateExternalState(string action, ClothItem item)
    {
        RefreshStateCacheFull(action, item);
    }

    private string GetCacheKey(ClothItem item)
    {
        if (item == null)
            return "";
        if (!string.IsNullOrEmpty(item.Prefix))
            return item.Prefix;
        return item.Name != null ? item.Name : "";
    }

    private void RefreshStateCacheFull(string action, ClothItem item)
    {
        cacheTotalOperable = 0;
        cacheHiddenOperable = 0;
        cacheBraTotal = 0;
        cacheBraHidden = 0;
        cachePantyTotal = 0;
        cachePantyHidden = 0;
        cacheShoeTotal = 0;
        cacheShoeHidden = 0;
        cacheHiddenByPrefix.Clear();
        cacheCategoryByPrefix.Clear();
        cacheProtectedPrefixes.Clear();

        for (int i = 0; i < clothes.Count; i++)
        {
            ClothItem c = clothes[i];
            if (c == null)
                continue;

            string key = GetCacheKey(c);
            if (string.IsNullOrEmpty(key))
                continue;

            bool hidden = IsHidden(c);
            string category = GetClothCategory(c);
            bool protect = IsProtected(c);

            cacheHiddenByPrefix[key] = hidden;
            cacheCategoryByPrefix[key] = category;
            if (protect)
                cacheProtectedPrefixes.Add(key);

            if (category == "bra")
            {
                cacheBraTotal++;
                if (hidden) cacheBraHidden++;
            }
            else if (category == "panty")
            {
                cachePantyTotal++;
                if (hidden) cachePantyHidden++;
            }
            else if (category == "shoes")
            {
                cacheShoeTotal++;
                if (hidden) cacheShoeHidden++;
            }

            if (protect)
                continue;

            cacheTotalOperable++;
            if (hidden)
                cacheHiddenOperable++;
        }

        stateCacheValid = true;
        ApplyStateCacheToStorables(action, item);
    }

    private void UpdateStateCacheForItem(string action, ClothItem item)
    {
        if (!stateCacheValid || item == null)
        {
            RefreshStateCacheFull(action, item);
            return;
        }

        string key = GetCacheKey(item);
        if (string.IsNullOrEmpty(key))
        {
            RefreshStateCacheFull(action, item);
            return;
        }

        bool newHidden = IsHidden(item);
        string category = GetClothCategory(item);
        bool protect = IsProtected(item);

        bool oldHidden;
        if (!cacheHiddenByPrefix.TryGetValue(key, out oldHidden))
        {
            RefreshStateCacheFull(action, item);
            return;
        }

        string oldCategory = null;
        cacheCategoryByPrefix.TryGetValue(key, out oldCategory);
        bool oldProtect = cacheProtectedPrefixes.Contains(key);

        if (oldCategory != category || oldProtect != protect)
        {
            RefreshStateCacheFull(action, item);
            return;
        }

        if (oldHidden != newHidden)
        {
            if (!oldProtect)
            {
                if (newHidden)
                    cacheHiddenOperable++;
                else
                    cacheHiddenOperable--;
            }

            if (category == "bra")
            {
                if (newHidden) cacheBraHidden++;
                else cacheBraHidden--;
            }
            else if (category == "panty")
            {
                if (newHidden) cachePantyHidden++;
                else cachePantyHidden--;
            }
            else if (category == "shoes")
            {
                if (newHidden) cacheShoeHidden++;
                else cacheShoeHidden--;
            }

            cacheHiddenByPrefix[key] = newHidden;
        }

        ApplyStateCacheToStorables(action, item);
    }

    private void ApplyStateCacheToStorables(string action, ClothItem item)
    {
        if (cacheHiddenOperable < 0) cacheHiddenOperable = 0;
        if (cacheHiddenOperable > cacheTotalOperable) cacheHiddenOperable = cacheTotalOperable;
        if (cacheBraHidden < 0) cacheBraHidden = 0;
        if (cacheBraHidden > cacheBraTotal) cacheBraHidden = cacheBraTotal;
        if (cachePantyHidden < 0) cachePantyHidden = 0;
        if (cachePantyHidden > cachePantyTotal) cachePantyHidden = cachePantyTotal;
        if (cacheShoeHidden < 0) cacheShoeHidden = 0;
        if (cacheShoeHidden > cacheShoeTotal) cacheShoeHidden = cacheShoeTotal;

        bool allHidden = cacheTotalOperable > 0 && cacheHiddenOperable >= cacheTotalOperable;
        bool allVisible = cacheTotalOperable > 0 && cacheHiddenOperable == 0;

        SetStateBool(stateAllHiddenJSON, allHidden);
        SetStateBool(stateAllVisibleJSON, allVisible);
        SetStateBool(stateBraHiddenJSON, cacheBraTotal > 0 && cacheBraHidden == cacheBraTotal);
        SetStateBool(stateBraVisibleJSON, cacheBraTotal > 0 && cacheBraHidden == 0);
        SetStateBool(statePantyHiddenJSON, cachePantyTotal > 0 && cachePantyHidden == cachePantyTotal);
        SetStateBool(statePantyVisibleJSON, cachePantyTotal > 0 && cachePantyHidden == 0);
        SetStateBool(stateShoesHiddenJSON, cacheShoeTotal > 0 && cacheShoeHidden == cacheShoeTotal);
        SetStateBool(stateShoesVisibleJSON, cacheShoeTotal > 0 && cacheShoeHidden == 0);

        if (!string.IsNullOrEmpty(action) && stateLastActionJSON != null)
            stateLastActionJSON.val = action;

        if (item != null)
        {
            if (stateLastItemJSON != null)
                stateLastItemJSON.val = item.Name;
            if (stateLastCategoryJSON != null)
                stateLastCategoryJSON.val = GetClothCategory(item);
        }

        if (stateProgressJSON != null)
            stateProgressJSON.val = cacheHiddenOperable.ToString(CultureInfo.InvariantCulture) + "/" + cacheTotalOperable.ToString(CultureInfo.InvariantCulture);
    }

    private void SetStateBool(JSONStorableBool st, bool value)
    {
        if (st != null)
            st.val = value;
    }

    private string BuildStateReport(string action, ClothItem item)
    {
        string itemName = item != null ? item.Name : "";
        string category = item != null ? GetClothCategory(item) : "";

        bool allHidden = stateAllHiddenJSON != null && stateAllHiddenJSON.val;
        bool allVisible = stateAllVisibleJSON != null && stateAllVisibleJSON.val;
        bool braHidden = stateBraHiddenJSON != null && stateBraHiddenJSON.val;
        bool braVisible = stateBraVisibleJSON != null && stateBraVisibleJSON.val;
        bool pantyHidden = statePantyHiddenJSON != null && statePantyHiddenJSON.val;
        bool pantyVisible = statePantyVisibleJSON != null && statePantyVisibleJSON.val;
        bool shoesHidden = stateShoesHiddenJSON != null && stateShoesHiddenJSON.val;
        bool shoesVisible = stateShoesVisibleJSON != null && stateShoesVisibleJSON.val;
        string progress = stateProgressJSON != null ? stateProgressJSON.val : "0/0";

        string visibleItems = BuildClothItemList(false);
        string hiddenItems = BuildClothItemList(true);

        return "STATE_REPORT" +
            "|source=vam" +
            "|kind=cloth" +
            "|level=notice" +
            "|action=" + SafeField(action) +
            "|item=" + SafeField(itemName) +
            "|category=" + SafeField(category) +
            "|all_hidden=" + BoolText(allHidden) +
            "|all_visible=" + BoolText(allVisible) +
            "|bra_hidden=" + BoolText(braHidden) +
            "|bra_visible=" + BoolText(braVisible) +
            "|panty_hidden=" + BoolText(pantyHidden) +
            "|panty_visible=" + BoolText(pantyVisible) +
            "|shoes_hidden=" + BoolText(shoesHidden) +
            "|shoes_visible=" + BoolText(shoesVisible) +
            "|progress=" + SafeField(progress) +
            "|visible_items=" + SafeField(visibleItems) +
            "|hidden_items=" + SafeField(hiddenItems) +
            "|summary=" + SafeField(BuildJapaneseSummary(action, itemName, category, allHidden, allVisible));
    }

    private string BuildClothItemList(bool hiddenList)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            if (item == null)
                continue;

            bool hidden = IsHidden(item);
            if (hidden != hiddenList)
                continue;

            if (sb.Length > 0)
                sb.Append(";");

            sb.Append(GetClothCategory(item));
            sb.Append(":");
            sb.Append(item.Name);
        }

        return sb.ToString();
    }

    private string BuildJapaneseSummary(string action, string itemName, string category, bool allHidden, bool allVisible)
    {
        string label = GetJapaneseCategoryLabel(category, itemName);

        if (action == "wear_none_group1_start")
            return "外側・上着系をまとめて物理脱衣中";

        if (action == "wear_none_group1_done")
            return "外側・上着系をまとめて脱いだ";

        if (action == "wear_none_group2_start")
            return "下着系をまとめて物理脱衣中";

        if (action == "wear_none_group2_done")
            return "下着系をまとめて脱いだ";

        if (action == "wear_none_complete")
            return "まとめ脱衣が完了した";

        if (action == "wear_none_group1_skip" || action == "wear_none_group2_skip")
            return "まとめ脱衣対象がなかった";

        if (action == "phys_start")
            return label + "を物理脱衣中";

        if (action == "hide")
        {
            if (allHidden)
                return label + "を脱いだ。操作対象の衣装はすべて脱いでいる";
            return label + "を脱いだ";
        }

        if (action == "wear")
        {
            if (allVisible)
                return label + "を着た。操作対象の衣装はすべて着ている";
            return label + "を着た";
        }

        if (action == "remove_bra")
            return "ブラを外した";
        if (action == "remove_panty")
            return "パンツを外した";

        if (action == "restore_all")
            return "衣装をすべて着た状態に戻した";
        if (action == "restore_scan")
            return "衣装をスキャン時の状態に戻した";
        if (action == "auto_stop")
            return "衣装の自動操作を停止した";

        return "衣装状態が変化した";
    }

    private string GetJapaneseCategoryLabel(string category, string itemName)
    {
        if (category == "bra") return "ブラ";
        if (category == "panty") return "パンツ";
        if (category == "shoes") return "靴下・靴";
        if (category == "top") return "上着";
        if (category == "bottom") return "下衣";
        if (category == "outer") return "外衣";
        if (category == "dress") return "ワンピース";
        return string.IsNullOrEmpty(itemName) ? "衣装" : itemName;
    }

    private string GetClothCategory(ClothItem item)
    {
        string s = GetLowerName(item);
        if (IsBra(s)) return "bra";
        if (IsPanty(s)) return "panty";
        if (IsShoeSock(s)) return "shoes";
        if (IsOuter(s)) return "outer";
        if (IsTop(s)) return "top";
        if (IsDress(s)) return "dress";
        if (IsBottom(s)) return "bottom";
        return "cloth";
    }

    private string BoolText(bool value)
    {
        return value ? "true" : "false";
    }

    private string SafeField(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        return value.Replace("|", " ").Replace("\r", " ").Replace("\n", " ").Trim();
    }

    private void SendStateReportToPython(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        UdpClient client = null;
        try
        {
            int port = pyReportPortJSON != null ? Mathf.RoundToInt(pyReportPortJSON.val) : 9999;
            client = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, "127.0.0.1", port);
        }
        catch (Exception e)
        {
            SuperController.LogMessage("!! [CLOTH_STATE SEND ERROR] " + e.Message);
        }
        finally
        {
            if (client != null)
                client.Close();
        }
    }

    private void DebugLog(string text)
    {
        if (!IsDebugLogEnabled())
            return;

        SuperController.LogMessage("[ClothStateSwitcher] " + text);
    }

    private bool IsSimCloth(ClothItem item)
    {
        if (selectedPerson == null || item == null || string.IsNullOrEmpty(item.Prefix))
            return false;

        string foundName;
        string storableId;
        if (FindPhysBool(item, AllowDetachNames(), out foundName, out storableId) != null)
            return true;
        if (FindPhysFloat(item, GravityNames(), out foundName, out storableId) != null)
            return true;
        if (FindPhysFloat(item, WeightNames(), out foundName, out storableId) != null)
            return true;

        string lowerPrefix = item.Prefix.ToLowerInvariant();
        foreach (string id in selectedPerson.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(id))
                continue;

            string s = id.ToLowerInvariant();
            if (s.StartsWith(lowerPrefix) && s.EndsWith("sim"))
                return true;
        }

        return false;
    }

    private int ReloadAllClothes()
    {
        int count = 0;
        for (int i = 0; i < clothes.Count; i++)
            count += ReloadCloth(clothes[i]);

        if (count > 0)
            DebugLog("[RELOAD ALL] executed=" + count.ToString(CultureInfo.InvariantCulture));

        return count;
    }

    private int ReloadCloth(ClothItem item)
    {
        if (selectedPerson == null || item == null)
            return 0;

        List<string> ids = GetRelatedStorableIds(item);
        ids.Sort(delegate(string a, string b)
        {
            return GetReloadStorablePriority(a).CompareTo(GetReloadStorablePriority(b));
        });

        List<string> names = GetKnownReloadActionNames();

        for (int i = 0; i < ids.Count; i++)
        {
            JSONStorable storable = selectedPerson.GetStorableByID(ids[i]);
            if (storable == null)
                continue;

            for (int n = 0; n < names.Count; n++)
            {
                JSONStorableAction action = null;
                try { action = storable.GetAction(names[n]); } catch { action = null; }
                if (action == null)
                    continue;

                try
                {
                    action.actionCallback.Invoke();
                    DebugLog("[RELOAD] " + item.Name + " / " + ids[i] + "/" + names[n]);
                    return 1;
                }
                catch (Exception e)
                {
                    DebugLog("[RELOAD ERROR] " + item.Name + " / " + ids[i] + "/" + names[n] + " / " + e.Message);
                }
            }
        }

        return 0;
    }

    private int GetReloadStorablePriority(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return 99;

        string s = storableId.ToLowerInvariant();
        if (s.IndexOf("reloader") >= 0) return 0;
        if (s.IndexOf("preset") >= 0) return 1;
        if (s.IndexOf("itemcontrol") >= 0) return 2;
        if (s.IndexOf("wrap") >= 0) return 3;
        if (s.IndexOf("sim") >= 0) return 4;
        if (s.IndexOf("material") >= 0) return 5;
        return 50;
    }

    private List<string> GetKnownReloadActionNames()
    {
        return new List<string>()
        {
            "Reload", "reload", "Reload Clothing", "Reload clothing", "Reload Item", "Reload item", "Reload Selected", "Reload selected",
            "Reset", "reset", "Reset Clothing", "Reset clothing", "Reset Item", "Reset item", "Reset Sim", "Reset sim", "Reset Physics", "Reset physics",
            "Restore", "restore", "Restore Clothing", "Restore clothing", "Restore Item", "Restore item", "Restore Preset", "Restore preset",
            "Rebuild", "rebuild", "Rebuild Clothing", "Rebuild clothing", "Rebuild Sim", "Rebuild sim",
            "Reimport", "reimport", "Reimport Clothing", "Reimport clothing",
            "Load", "load", "Load Preset", "Load preset", "Load Clothing", "Load clothing",
            "Apply", "apply", "Apply Preset", "Apply preset", "Apply Clothing", "Apply clothing",
            "Refresh", "refresh", "Refresh Clothing", "Refresh clothing", "Refresh Sim", "Refresh sim",
            "Resync", "resync", "Sync", "sync"
        };
    }


    private List<string> GetKnownSimResetActionNames()
    {
        return new List<string>()
        {
            "reset", "Reset", "Reset Sim", "Reset sim", "Reset Physics", "Reset physics",
            "Refresh", "refresh", "Refresh Sim", "Refresh sim",
            "Resync", "resync", "Sync", "sync"
        };
    }

    private bool IsLikelySimStorableId(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return false;

        string s = storableId.ToLowerInvariant();
        return s.EndsWith("sim") || s.IndexOf("sim") >= 0;
    }

    private int ResetSimForItem(ClothItem item, string context)
    {
        if (selectedPerson == null || item == null)
            return 0;

        if (!IsSimCloth(item))
            return 0;

        List<string> ids = GetRelatedStorableIds(item);
        List<string> names = GetKnownSimResetActionNames();
        int tried = 0;

        for (int i = 0; i < ids.Count; i++)
        {
            string id = ids[i];
            if (!IsLikelySimStorableId(id))
                continue;

            JSONStorable storable = selectedPerson.GetStorableByID(id);
            if (storable == null)
                continue;

            for (int n = 0; n < names.Count; n++)
            {
                JSONStorableAction action = null;
                try { action = storable.GetAction(names[n]); } catch { action = null; }
                tried++;
                if (action == null)
                    continue;

                try
                {
                    action.actionCallback.Invoke();
                    DebugLog("[SIM RESET] context=" + context +
                        " name=" + item.Name +
                        " / " + id + "/" + names[n] +
                        " tried=" + tried.ToString(CultureInfo.InvariantCulture));
                    return 1;
                }
                catch (Exception e)
                {
                    DebugLog("[SIM RESET ERROR] context=" + context +
                        " name=" + item.Name +
                        " / " + id + "/" + names[n] +
                        " / " + e.Message);
                }
            }
        }

        DebugLog("[SIM RESET MISS] context=" + context +
            " name=" + item.Name +
            " tried=" + tried.ToString(CultureInfo.InvariantCulture));
        return 0;
    }

    private int ResetSimForAllVisibleItems(string context)
    {
        int count = 0;
        for (int i = 0; i < clothes.Count; i++)
        {
            ClothItem item = clothes[i];
            if (item == null)
                continue;

            // 非表示中の衣装をresetすると、落ちたSimメッシュをその場で確定させる場合があるため、
            // WEAR ALL / Appearance後の見えている状態だけを対象にする。
            if (IsHidden(item))
                continue;

            count += ResetSimForItem(item, context);
        }
        return count;
    }


    private void UpdateButtonColors()
    {
        bool autoOn = autoModeChooser != null && autoModeChooser.val != AUTO_OFF;
        bool busy = physicalRemoveRunning > 0;

        SetPopupColor(autoModePopup, autoOn ? new Color(1.0f, 0.55f, 0.20f) : new Color(0.90f, 0.90f, 0.90f));
        SetButtonColor(autoStopButton, autoOn ? new Color(1.0f, 0.55f, 0.20f) : (busy ? new Color(1.0f, 0.80f, 0.35f) : new Color(1.0f, 0.92f, 0.55f)));
        SetButtonColor(nextButton, busy ? new Color(1.0f, 0.80f, 0.35f) : new Color(0.90f, 0.90f, 0.90f));
        SetButtonColor(prevButton, busy ? new Color(1.0f, 0.80f, 0.35f) : new Color(0.90f, 0.90f, 0.90f));
        SetButtonColor(scanButton, new Color(0.75f, 0.85f, 1.0f));
        SetButtonColor(restoreAllButton, new Color(0.65f, 1.0f, 0.70f));
        SetButtonColor(wearNoneButton, new Color(1.0f, 0.75f, 0.45f));
        SetButtonColor(removeBraButton, new Color(1.0f, 0.78f, 0.88f));
        SetButtonColor(removePantyButton, new Color(1.0f, 0.78f, 0.88f));
        SetButtonColor(resetRuntimeButton, new Color(1.0f, 0.45f, 0.45f));
    }

    private void SetPopupColor(UIDynamicPopup p, Color c)
    {
        if (p == null)
            return;

        try
        {
            UnityEngine.UI.Image[] images = p.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>(true);
            if (images == null)
                return;

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null)
                    images[i].color = c;
            }
        }
        catch { }
    }

    private void SetButtonColor(UIDynamicButton b, Color c)
    {
        if (b == null || b.button == null)
            return;

        try
        {
            if (b.button.image != null)
                b.button.image.color = c;
        }
        catch { }
    }
    private void UpdatePreview()
    {
        if (previewJSON == null)
            return;

        if (selectedPerson == null)
        {
            previewJSON.val = "No Person";
            return;
        }

        if (orderedClothes.Count == 0)
        {
            previewJSON.val = "No cloth list\nPress SCAN CLOTH";
            return;
        }

        string text = "";
        text += "Order Mode: " + (orderModeChooser != null ? orderModeChooser.val : "") + "\n";
        text += "Last Clothing Rule: " + (finalRuleChooser != null ? finalRuleChooser.val : "") + "\n";
        text += "Auto Mode: " + (autoModeChooser != null ? autoModeChooser.val : "");
        if (autoModeChooser != null && autoModeChooser.val == AUTO_LOOP)
            text += autoLoopHideDirection ? " / HIDE" : " / WEAR";
        text += "\n";
        text += "Phys Style: " + (physRemoveStyleChooser != null ? physRemoveStyleChooser.val : "") + " / " + (physDurationJSON != null ? physDurationJSON.val.ToString("F1", CultureInfo.InvariantCulture) : "") + "s";
        text += " / Fade=" + (fadeSecondsJSON != null ? fadeSecondsJSON.val.ToString("F1", CultureInfo.InvariantCulture) : "") + "s";
        text += " / Bottom Force DOWN=" + ((bottomForceDownJSON != null && bottomForceDownJSON.val) ? "ON" : "OFF");
        if (phtyHiddenPrefixes.Count > 0)
            text += " / PHTY-HIDDEN=" + phtyHiddenPrefixes.Count.ToString(CultureInfo.InvariantCulture);
        if (physicalRemoveRunning > 0)
            text += " / BUSY=" + physicalRemoveRunning.ToString(CultureInfo.InvariantCulture);
        text += "\n";
        text += "WEAR ALL=全部着る / WEAR NONE=全部脱ぐ / PREV=1枚着る / NEXT=1枚脱ぐ\n";
        text += "Count: " + orderedClothes.Count + "\n";
        text += "Progress: " + (stateProgressJSON != null ? stateProgressJSON.val : "") + "\n\n";

        bool nextMarked = false;
        for (int i = 0; i < orderedClothes.Count; i++)
        {
            ClothItem item = orderedClothes[i];
            bool hidden = IsHidden(item);
            bool protect = IsProtected(item);

            string mark = "";
            if (protect)
                mark = "[LOCK]";
            else if (hidden)
                mark = "[DONE]";
            else if (!nextMarked)
            {
                mark = "< NEXT";
                nextMarked = true;
            }

            string simMark = IsSimCloth(item) ? " [SIM]" : "";

            string physMark = "";
            if (!hidden && !protect && UsePhysicalRemove())
            {
                string req = physRemoveStyleChooser != null ? physRemoveStyleChooser.val : PHYS_OFF;
                string eff = ResolvePhysicalStyleForItem(item, req);
                if (eff != req)
                    physMark = " [PHTY " + eff + "]";
            }

            string index = (i + 1).ToString("00");
            text += index + " " + item.Name + simMark + " " + mark + physMark + "\n";
        }

        previewJSON.val = text;
    }

    private void ShuffleList(List<ClothItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            ClothItem temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private string GetLowerName(ClothItem item)
    {
        if (item == null)
            return "";

        return (item.Prefix + " " + item.Name).ToLowerInvariant();
    }

    private bool IsBra(string s)
    {
        return s.IndexOf("bra") >= 0 || s.IndexOf("brassiere") >= 0;
    }

    private bool IsPanty(string s)
    {
        return s.IndexOf("panty") >= 0 ||
            s.IndexOf("pantie") >= 0 ||
            s.IndexOf("panties") >= 0 ||
            s.IndexOf("brief") >= 0 ||
            s.IndexOf("briefs") >= 0 ||
            s.IndexOf("underwear") >= 0 ||
            s.IndexOf("underpant") >= 0 ||
            s.IndexOf("knicker") >= 0 ||
            s.IndexOf("knickers") >= 0 ||
            s.IndexOf("thong") >= 0 ||
            s.IndexOf("gstring") >= 0 ||
            s.IndexOf("g-string") >= 0;
    }

    private bool IsTop(string s)
    {
        return s.IndexOf("shirt") >= 0 ||
            s.IndexOf("top") >= 0 ||
            s.IndexOf("blouse") >= 0 ||
            s.IndexOf("sweater") >= 0 ||
            s.IndexOf("camisole") >= 0 ||
            s.IndexOf("tank") >= 0;
    }

    private bool IsOuter(string s)
    {
        return s.IndexOf("jacket") >= 0 ||
            s.IndexOf("coat") >= 0 ||
            s.IndexOf("cardigan") >= 0 ||
            s.IndexOf("hoodie") >= 0 ||
            s.IndexOf("parka") >= 0;
    }

    private bool IsDress(string s)
    {
        return s.IndexOf("dress") >= 0 ||
            s.IndexOf("onepiece") >= 0 ||
            s.IndexOf("one piece") >= 0;
    }

    private bool IsBottom(string s)
    {
        return s.IndexOf("skirt") >= 0 ||
            s.IndexOf("pants") >= 0 ||
            s.IndexOf("trouser") >= 0 ||
            s.IndexOf("jeans") >= 0 ||
            s.IndexOf("bottom") >= 0;
    }

    private bool IsShoeSock(string s)
    {
        return s.IndexOf("shoe") >= 0 ||
            s.IndexOf("boot") >= 0 ||
            s.IndexOf("sock") >= 0 ||
            s.IndexOf("stocking") >= 0 ||
            s.IndexOf("heel") >= 0;
    }

    private List<string> FindWearablePrefixes()
    {
        List<string> prefixes = new List<string>();
        if (selectedPerson == null)
            return prefixes;

        foreach (string storableId in selectedPerson.GetStorableIDs())
        {
            if (string.IsNullOrEmpty(storableId))
                continue;
            if (IsIgnoredClothStorable(storableId))
                continue;
            string lowerStorableId = storableId.ToLowerInvariant();
            if (!LooksLikeRealClothItem(lowerStorableId) && (LooksLikeHairOrFaceItem(lowerStorableId) || LooksLikeAccessoryItem(lowerStorableId)))
                continue;

            string prefix = GetWearablePrefix(storableId);
            if (string.IsNullOrEmpty(prefix))
                continue;

            if (!prefixes.Contains(prefix))
                prefixes.Add(prefix);
        }

        prefixes.Sort();
        return prefixes;
    }

    private bool LooksLikeWearableControl(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return false;

        string s = storableId.ToLowerInvariant();
        if (s.IndexOf("itemcontrol") < 0 && s.IndexOf("itemdeleter") < 0 && s.IndexOf("itemreloader") < 0)
            return false;
        if (!LooksLikeRealClothItem(s) && (LooksLikeHairOrFaceItem(s) || LooksLikeAccessoryItem(s)))
            return false;

        return true;
    }

    private string GetWearablePrefix(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return "";

        string lower = storableId.ToLowerInvariant();
        int index = lower.IndexOf("itemcontrol");
        if (index < 0)
            index = lower.IndexOf("itemdeleter");
        if (index < 0)
            index = lower.IndexOf("itemreloader");
        if (index < 0)
            index = lower.IndexOf("material");
        if (index < 0)
            index = lower.IndexOf("wrapcontrol");
        if (index < 0 && lower.EndsWith("sim", StringComparison.Ordinal))
            index = storableId.Length - 3;
        if (index < 0 && lower.EndsWith("preset", StringComparison.Ordinal))
            index = storableId.Length - 6;
        if (index < 0)
            return "";

        string prefix = storableId.Substring(0, index);
        return NormalizeWearablePrefix(prefix);
    }

    private string NormalizeWearablePrefix(string prefix)
    {
        if (string.IsNullOrEmpty(prefix))
            return "";

        string p = prefix.Trim();
        while (p.EndsWith("_", StringComparison.Ordinal) || p.EndsWith("-", StringComparison.Ordinal) || p.EndsWith(" ", StringComparison.Ordinal))
            p = p.Substring(0, p.Length - 1);

        // v8n:
        // Material/Sim派生名を別服として数えないため、末尾の装飾サフィックスを親Prefixへ寄せる。
        // 例:
        //   HeatUpTopMetal      -> HeatUpTop
        //   HeatUpTopString     -> HeatUpTop
        //   HeatUpSkirtMetalSim -> HeatUpSkirt
        //   HeatUpPantySim      -> HeatUpPanty
        // これにより NEXT/PREV は「服単位」で動き、Metal/Stringなどの部品単位で止まらない。
        p = StripVariantSuffixes(p);

        return p;
    }

    private string StripVariantSuffixes(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        string p = value.Trim();
        string[] suffixes = new string[]
        {
            "Sim",
            "Metal",
            "String",
            "Strap",
            "Main",
            "Heel",
            "Heels",
            "Trim",
            "Ribbon",
            "Buckle",
            "Lace",
            "Frill"
        };

        bool changed = true;
        while (changed)
        {
            changed = false;

            while (p.EndsWith("_", StringComparison.Ordinal) || p.EndsWith("-", StringComparison.Ordinal) || p.EndsWith(" ", StringComparison.Ordinal))
            {
                p = p.Substring(0, p.Length - 1);
                changed = true;
            }

            for (int i = 0; i < suffixes.Length; i++)
            {
                string suffix = suffixes[i];
                if (p.Length <= suffix.Length + 2)
                    continue;

                if (p.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    p = p.Substring(0, p.Length - suffix.Length);
                    changed = true;
                    break;
                }
            }
        }

        while (p.EndsWith("_", StringComparison.Ordinal) || p.EndsWith("-", StringComparison.Ordinal) || p.EndsWith(" ", StringComparison.Ordinal))
            p = p.Substring(0, p.Length - 1);

        return p;
    }

    private bool LooksLikeRelatedMaterial(string storableId, string prefix)
    {
        if (string.IsNullOrEmpty(storableId) || string.IsNullOrEmpty(prefix))
            return false;

        string lower = storableId.ToLowerInvariant();
        if (lower.IndexOf("material") < 0)
            return false;
        if (!lower.StartsWith(prefix.ToLowerInvariant()))
            return false;
        if (!LooksLikeRealClothItem(lower) && (LooksLikeHairOrFaceItem(lower) || LooksLikeAccessoryItem(lower)))
            return false;

        return true;
    }

//除外から除外をはずすsためのリスト
private bool LooksLikeRealClothItem(string lowerId)
{
    if (string.IsNullOrEmpty(lowerId))
        return false;

    return
        lowerId.IndexOf("nightwear") >= 0 ||
        lowerId.IndexOf("shirt") >= 0 ||
        lowerId.IndexOf("skirt") >= 0;
}

    private string GetAccessoryReason(string lowerId)
    {
        if (string.IsNullOrEmpty(lowerId))
            return "empty";

        // 衣装切替の対象外にするアクセサリ系。
        // Debug Log ON時は、この戻り値を使って「どの単語で除外されたか」を出す。
        if (lowerId.IndexOf("ring") >= 0) return "ring";
        if (lowerId.IndexOf("circlet") >= 0) return "circlet";
        if (lowerId.IndexOf("crown") >= 0) return "crown";
        if (lowerId.IndexOf("tiara") >= 0) return "tiara";
        if (lowerId.IndexOf("earring") >= 0) return "earring";
        if (lowerId.IndexOf("piercing") >= 0) return "piercing";
        if (lowerId.IndexOf("necklace") >= 0) return "necklace";
        if (lowerId.IndexOf("choker") >= 0) return "choker";
        if (lowerId.IndexOf("bracelet") >= 0) return "bracelet";
        if (lowerId.IndexOf("wrist") >= 0) return "wrist";
        if (lowerId.IndexOf("anklet") >= 0) return "anklet";
        if (lowerId.IndexOf("watch") >= 0) return "watch";
        if (lowerId.IndexOf("fingernail") >= 0) return "fingernail";
        if (lowerId.IndexOf("toenail") >= 0) return "toenail";
        if (lowerId.IndexOf("fingernails") >= 0) return "fingernails";
        if (lowerId.IndexOf("toenails") >= 0) return "toenails";
        if (lowerId.IndexOf("nail") >= 0) return "nail";
        if (lowerId.IndexOf("headpiece") >= 0) return "headpiece";
        if (lowerId.IndexOf("headwear") >= 0) return "headwear";
        if (lowerId.IndexOf("head ornament") >= 0) return "head ornament";

        return null;
    }

    private bool LooksLikeAccessoryItem(string lowerId)
    {
        return !string.IsNullOrEmpty(GetAccessoryReason(lowerId));
    }

    private string GetHairFaceReason(string lowerId)
    {
        if (string.IsNullOrEmpty(lowerId))
            return "empty";

        // 髪・顔・口周り・耳/角/尻尾など、衣装切替の対象外にするもの。
        // Debug Log ON時は、この戻り値を使って「どの単語で除外されたか」を出す。
        if (lowerId.IndexOf("hair") >= 0) return "hair";
        if (lowerId.IndexOf("lash") >= 0) return "lash";
        if (lowerId.IndexOf("brow") >= 0) return "brow";
        if (lowerId.IndexOf("eye") >= 0) return "eye";
        if (lowerId.IndexOf("iris") >= 0) return "iris";
        if (lowerId.IndexOf("shadow") >= 0) return "shadow";
        if (lowerId.IndexOf("scalp") >= 0) return "scalp";
        if (lowerId.IndexOf("tieback") >= 0) return "tieback";
        if (lowerId.IndexOf("twintail") >= 0) return "twintail";
        if (lowerId.IndexOf("ponytail") >= 0) return "ponytail";
        if (lowerId.IndexOf("bang") >= 0) return "bang";
        if (lowerId.IndexOf("bob") >= 0) return "bob";
        if (lowerId.IndexOf("tooth") >= 0) return "tooth";
        if (lowerId.IndexOf("teeth") >= 0) return "teeth";
        if (lowerId.IndexOf("fang") >= 0) return "fang";
        if (lowerId.IndexOf("horn") >= 0) return "horn";
        if (lowerId.IndexOf("ear") >= 0) return "ear";
        if (lowerId.IndexOf("tail") >= 0) return "tail";

        return null;
    }

    private bool LooksLikeHairOrFaceItem(string lowerId)
    {
        return !string.IsNullOrEmpty(GetHairFaceReason(lowerId));
    }

private void OnDestroy()
{
    try
    {
        if (autoModeChooser != null)
            autoModeChooser.valNoCallback = AUTO_OFF;

        autoTimer = 0.0f;
        autoLoopHideDirection = true;

        if (delayedScanRoutine != null)
        {
            try { StopCoroutine(delayedScanRoutine); } catch { }
            delayedScanRoutine = null;
        }

        if (restoreAllRoutine != null)
        {
            try { StopCoroutine(restoreAllRoutine); } catch { }
            restoreAllRoutine = null;
        }

        if (physicalHideRoutine != null)
        {
            try { StopCoroutine(physicalHideRoutine); } catch { }
            physicalHideRoutine = null;
        }

        try { StopAllCoroutines(); } catch { }

        activePhysBoolBackups = null;
        activePhysFloatBackups = null;
        activeFadeRefs = null;
        physicalRemoveRunning = 0;

        DebugLog("[ON DESTROY] cleanup only / no restore / no alpha nudge");
    }
    catch { }
}

    private bool IsIgnoredClothStorable(string storableId)
    {
        if (string.IsNullOrEmpty(storableId))
            return true;

        string s = storableId.ToLowerInvariant();
        if (s.IndexOf("plugin#") >= 0)
            return true;
        if (s.IndexOf("punchballgame") >= 0)
            return true;
        if (s.IndexOf("clothstateswitcher") >= 0)
            return true;
        if (s.IndexOf("clothofftester") >= 0)
            return true;
        if (s.IndexOf("clothingplugindestructor") >= 0)
            return true;
        if (s.IndexOf("clothingpresets") >= 0)
            return true;
        if (s.IndexOf("stopper.clothingpluginmanager") >= 0)
            return true;
        if (s.IndexOf("region") >= 0 && s.IndexOf("material") >= 0)
            return true;
        if (s.IndexOf("scalpmaterial") >= 0)
            return true;
        if (s.IndexOf("pantyregion") >= 0)
            return true;
        if (!LooksLikeRealClothItem(s) && LooksLikeAccessoryItem(s))
            return true;

        return false;
    }
}
