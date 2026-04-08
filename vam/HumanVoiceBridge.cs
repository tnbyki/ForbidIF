// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// FILE:RM_AIVoiceController.cs
// VAM VOICE CORE CONTROLLER
// Base Voice System for AI / External Bridge
//
// Author: Original Creator
// Respect & Thanks: VAMT Integration Layer
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// DESCRIPTION
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// - Core voice playback controller inside VaM
// - Handles text-to-speech / audio playback
// - Receives trigger via JSONStorables
// - Used by external bridge systems (AI / Python)
//
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// INTEGRATION
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// This controller is used by:
//
//   HumanVoiceBridge.cs
//   (AI → TAM → PY → VAM pipeline)
//
// External systems send commands through:
//   - VoiceText
//   - VoiceRequest
//
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// SPECIAL THANKS
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// This system is built upon the original
// RM_AIVoiceController implementation.
//
// Respect and appreciation to the original author
// for enabling real-time voice control integration.
//
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// NOTES
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// - Do not remove or rename required JSONStorables
// - External bridge depends on these parameters
// - Playback is triggered by VoiceRequest increment
//
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RM_AIChatCompanionBridgeMini : MVRScript
{
    // ===== RM_AIVoiceController が監視する値 =====
    JSONStorableString _voiceText;
    JSONStorableFloat _voiceRequest;

    // ===== 外から流し込む入口 =====
    JSONStorableString _speakText;
    JSONStorableString _wavUrl;
    JSONStorableString _voiceCommand;

    JSONStorableFloat _speakerId;
    JSONStorableFloat _vvSpeed;
    JSONStorableFloat _vvPitch;
    JSONStorableFloat _vvIntonation;
    JSONStorableFloat _vvVolumeScale;

    // ===== 手動指定用（必要時のみ） =====
    JSONStorableString _voiceAtomUid;
    JSONStorableString _voiceStorableId;

    // ===== 解決済み参照 =====
    Atom _voiceAtom;
    JSONStorable _voiceStorable;

    JSONStorableFloat _targetSpeakerId;
    JSONStorableFloat _targetSpeed;
    JSONStorableFloat _targetPitch;
    JSONStorableFloat _targetIntonation;
    JSONStorableFloat _targetVolumeScale;

    // ===== UI =====
    UIDynamicButton _btnSpeak;
    UIDynamicButton _btnPlayWav;
    UIDynamicButton _btnReqUp;
    UIDynamicButton _btnAutoDetectVoice;
    UIDynamicButton _btnApplyVoiceParams;
    UIDynamicButton _btnRunVoiceCommand;

    bool _suppressCallbacks = false;
    float _nextDetectTryTime = 0f;

    public override void Init()
    {
        try
        {
            pluginLabelJSON.val = "RM_AIChatCompanionBridgeMini";

            // --------------------------------------------------
            // RM_AIVoiceController が読む値
            // --------------------------------------------------
            _voiceText = new JSONStorableString("VoiceText", "");
            RegisterString(_voiceText);

            _voiceRequest = new JSONStorableFloat("VoiceRequest", 0f, -999999f, 999999f);
            RegisterFloat(_voiceRequest);

            // --------------------------------------------------
            // 入力欄
            // --------------------------------------------------
            _speakText = new JSONStorableString("SpeakText", "");
            _speakText.setCallbackFunction = (s) =>
            {
                if (_suppressCallbacks) return;
                if (string.IsNullOrEmpty(s)) return;
                SendSpeakText(s);
            };
            RegisterString(_speakText);
            var tfSpeak = CreateTextField(_speakText);
            if (tfSpeak != null) tfSpeak.height = 60f;

            _wavUrl = new JSONStorableString("WavUrl", "");
            _wavUrl.setCallbackFunction = (s) =>
            {
                if (_suppressCallbacks) return;
                if (string.IsNullOrEmpty(s)) return;
                SendWavUrl(s);
            };
            RegisterString(_wavUrl);
            var tfWav = CreateTextField(_wavUrl);
            if (tfWav != null) tfWav.height = 60f;

            _voiceCommand = new JSONStorableString("VoiceCommand", "");
            _voiceCommand.setCallbackFunction = (s) =>
            {
                if (_suppressCallbacks) return;
                if (string.IsNullOrEmpty(s)) return;
                SendVoiceCommand(s);
            };
            RegisterString(_voiceCommand);
            var tfCmd = CreateTextField(_voiceCommand);
            if (tfCmd != null) tfCmd.height = 90f;

            // --------------------------------------------------
            // VOICEVOX パラメータ
            // --------------------------------------------------
            _speakerId = new JSONStorableFloat("SpeakerId", 80f, 0f, 200f, true, true);
            RegisterFloat(_speakerId);
            CreateSlider(_speakerId);

            _vvSpeed = new JSONStorableFloat("VV_Speed", 1.50f, 0.50f, 2.00f, true, true);
            RegisterFloat(_vvSpeed);
            CreateSlider(_vvSpeed);

            _vvPitch = new JSONStorableFloat("VV_Pitch", 0.00f, -0.50f, 0.50f, true, true);
            RegisterFloat(_vvPitch);
            CreateSlider(_vvPitch);

            _vvIntonation = new JSONStorableFloat("VV_Intonation", 1.40f, 0.50f, 2.00f, true, true);
            RegisterFloat(_vvIntonation);
            CreateSlider(_vvIntonation);

            _vvVolumeScale = new JSONStorableFloat("VV_VolumeScale", 1.20f, 0.00f, 2.00f, true, true);
            RegisterFloat(_vvVolumeScale);
            CreateSlider(_vvVolumeScale);

            // --------------------------------------------------
            // 手動指定欄
            // --------------------------------------------------
            _voiceAtomUid = new JSONStorableString("VoiceCtrlAtomUid(optional)", "");
            RegisterString(_voiceAtomUid);
            var tfAtom = CreateTextField(_voiceAtomUid);
            if (tfAtom != null) tfAtom.height = 34f;

            _voiceStorableId = new JSONStorableString("VoiceCtrlStorableId(optional)", "");
            RegisterString(_voiceStorableId);
            var tfStore = CreateTextField(_voiceStorableId);
            if (tfStore != null) tfStore.height = 34f;

            // --------------------------------------------------
            // ボタン
            // --------------------------------------------------
            _btnSpeak = CreateButton("Speak Text");
            if (_btnSpeak != null)
            {
                _btnSpeak.button.onClick.AddListener(() =>
                {
                    SendSpeakText(_speakText != null ? _speakText.val : "");
                });
            }

            _btnPlayWav = CreateButton("Play WAV URL");
            if (_btnPlayWav != null)
            {
                _btnPlayWav.button.onClick.AddListener(() =>
                {
                    SendWavUrl(_wavUrl != null ? _wavUrl.val : "");
                });
            }

            _btnRunVoiceCommand = CreateButton("Run VoiceCommand");
            if (_btnRunVoiceCommand != null)
            {
                _btnRunVoiceCommand.button.onClick.AddListener(() =>
                {
                    SendVoiceCommand(_voiceCommand != null ? _voiceCommand.val : "");
                });
            }

            _btnReqUp = CreateButton("VoiceRequest +1");
            if (_btnReqUp != null)
            {
                _btnReqUp.button.onClick.AddListener(() =>
                {
                    if (_voiceRequest != null)
                    {
                        _voiceRequest.val += 1f;
                        SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceRequest -> " + _voiceRequest.val);
                    }
                });
            }

            _btnAutoDetectVoice = CreateButton("Auto Detect VoiceController");
            if (_btnAutoDetectVoice != null)
            {
                _btnAutoDetectVoice.button.onClick.AddListener(() =>
                {
                    AutoDetectVoiceController(true);
                });
            }

            _btnApplyVoiceParams = CreateButton("Apply Voice Params");
            if (_btnApplyVoiceParams != null)
            {
                _btnApplyVoiceParams.button.onClick.AddListener(() =>
                {
                    ApplyVoiceParamsToController(true);
                });
            }

            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: Init OK");
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: Put this on the same Atom as RM_AIVoiceController.");
            _nextDetectTryTime = Time.time + 0.25f;
        }
        catch (Exception e)
        {
            SuperController.LogError("RM_AIChatCompanionBridgeMini: Init exception: " + e);
        }
    }

    void Update()
    {
        try
        {
            if (_voiceStorable == null && Time.time >= _nextDetectTryTime)
            {
                _nextDetectTryTime = Time.time + 2.0f;
                AutoDetectVoiceController(false);
            }
        }
        catch { }
    }

    // ============================================================
    // 送信本体
    // ============================================================
    void SendSpeakText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: SpeakText is empty");
            return;
        }

        if (_voiceText == null || _voiceRequest == null)
        {
            SuperController.LogError("RM_AIChatCompanionBridgeMini: Voice storables are null");
            return;
        }

        ApplyVoiceParamsToController(false);

        _voiceText.val = text;
        _voiceRequest.val += 1f;

//        SuperController.LogMessage("RM_AIChatCompanionBridgeMini: Speak -> " + text);
        SuperController.LogMessage("RM_AIChatCompanionBridgeMini: Speak -> ");

        if (_speakText != null)
        {
            _suppressCallbacks = true;
            _speakText.val = "";
            _suppressCallbacks = false;
        }
    }

    void SendWavUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: WavUrl is empty");
            return;
        }

        if (_voiceText == null || _voiceRequest == null)
        {
            SuperController.LogError("RM_AIChatCompanionBridgeMini: Voice storables are null");
            return;
        }

        ApplyVoiceParamsToController(false);

        _voiceText.val = url;
        _voiceRequest.val += 1f;

        SuperController.LogMessage("RM_AIChatCompanionBridgeMini: WAV URL -> " + url);

        if (_wavUrl != null)
        {
            _suppressCallbacks = true;
            _wavUrl.val = "";
            _suppressCallbacks = false;
        }
    }

    void SendVoiceCommand(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceCommand is empty");
            return;
        }

        try
        {
            Dictionary<string, string> map = ParseVoiceCommand(raw);

            if (map.Count == 0)
            {
                SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceCommand parse result empty");
                return;
            }

            // speaker名は今はログだけ。必要なら後で name -> id 変換表を足せる
            string speakerName = GetMapValue(map, "speaker");
            if (!string.IsNullOrEmpty(speakerName))
            {
//               SuperController.LogMessage("RM_AIChatCompanionBridgeMini: speaker = " + speakerName);
            }

            float f;
            if (TryGetFloat(map, "id", out f) && _speakerId != null)
                _speakerId.val = f;

            if (TryGetFloat(map, "speed", out f) && _vvSpeed != null)
                _vvSpeed.val = f;

            if (TryGetFloat(map, "pitch", out f) && _vvPitch != null)
                _vvPitch.val = f;

            if (TryGetFloat(map, "intonation", out f) && _vvIntonation != null)
                _vvIntonation.val = f;

            if (TryGetFloat(map, "volume", out f) && _vvVolumeScale != null)
                _vvVolumeScale.val = f;

            string wav = GetMapValue(map, "wav");
            if (string.IsNullOrEmpty(wav))
                wav = GetMapValue(map, "url");

            string text = GetMapValue(map, "text");

            // wav があれば wav 優先
            if (!string.IsNullOrEmpty(wav))
            {
                SendWavUrl(wav);
            }
            else if (!string.IsNullOrEmpty(text))
            {
                SendSpeakText(text);
            }
            else
            {
                SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceCommand has no text/url/wav");
            }

            if (_voiceCommand != null)
            {
                _suppressCallbacks = true;
                _voiceCommand.val = "";
                _suppressCallbacks = false;
            }
        }
        catch (Exception e)
        {
            SuperController.LogError("RM_AIChatCompanionBridgeMini: VoiceCommand parse exception: " + e);
        }
    }

    Dictionary<string, string> ParseVoiceCommand(string raw)
    {
        Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(raw)) return map;

        string[] parts = raw.Split('|');
        for (int i = 0; i < parts.Length; i++)
        {
            string p = (parts[i] ?? "").Trim();
            if (string.IsNullOrEmpty(p)) continue;

            // 先頭の "VOICE" は無視
            if (i == 0 && p.Equals("VOICE", StringComparison.OrdinalIgnoreCase))
                continue;

            int eq = p.IndexOf('=');
            if (eq <= 0) continue;

            string key = p.Substring(0, eq).Trim();
            string value = p.Substring(eq + 1).Trim();

            if (!string.IsNullOrEmpty(key))
            {
                map[key] = value;
            }
        }

        return map;
    }

    string GetMapValue(Dictionary<string, string> map, string key)
    {
        if (map == null || string.IsNullOrEmpty(key)) return "";
        string v;
        if (map.TryGetValue(key, out v)) return v ?? "";
        return "";
    }

    bool TryGetFloat(Dictionary<string, string> map, string key, out float value)
    {
        value = 0f;
        string s = GetMapValue(map, key);
        if (string.IsNullOrEmpty(s)) return false;
        return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    // ============================================================
    // VoiceController 検出
    // ============================================================
    void AutoDetectVoiceController(bool log)
    {
        _voiceAtom = null;
        _voiceStorable = null;

        _targetSpeakerId = null;
        _targetSpeed = null;
        _targetPitch = null;
        _targetIntonation = null;
        _targetVolumeScale = null;

        string atomUid = (_voiceAtomUid != null) ? (_voiceAtomUid.val ?? "") : "";
        string storableId = (_voiceStorableId != null) ? (_voiceStorableId.val ?? "") : "";

        if (!string.IsNullOrEmpty(atomUid))
        {
            try
            {
                Atom a = SuperController.singleton.GetAtomByUid(atomUid);
                if (a != null)
                {
                    if (!string.IsNullOrEmpty(storableId))
                    {
                        JSONStorable st = a.GetStorableByID(storableId);
                        if (TryBindVoiceController(a, st, log)) return;
                    }
                    else
                    {
                        if (TryBindVoiceControllerByScan(a, log)) return;
                    }
                }
            }
            catch { }
        }

        try
        {
            if (containingAtom != null)
            {
                if (TryBindVoiceControllerByScan(containingAtom, log)) return;
            }
        }
        catch { }

        try
        {
            List<Atom> atoms = SuperController.singleton.GetAtoms();
            if (atoms != null)
            {
                for (int i = 0; i < atoms.Count; i++)
                {
                    if (TryBindVoiceControllerByScan(atoms[i], false))
                    {
                        if (log)
                        {
                            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceController auto-detected on atom=" + atoms[i].uid);
                        }
                        return;
                    }
                }
            }
        }
        catch { }

        if (log)
        {
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceController not found.");
        }
    }

    bool TryBindVoiceControllerByScan(Atom a, bool log)
    {
        if (a == null) return false;

        try
        {
            List<string> ids = a.GetStorableIDs();
            if (ids == null) return false;

            for (int i = 0; i < ids.Count; i++)
            {
                string id = ids[i];
                if (string.IsNullOrEmpty(id)) continue;

                if (id.IndexOf("RM_AIVoiceController", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    JSONStorable st = a.GetStorableByID(id);
                    if (TryBindVoiceController(a, st, log)) return true;
                }
            }

            for (int i = 0; i < ids.Count; i++)
            {
                string id = ids[i];
                if (string.IsNullOrEmpty(id)) continue;

                JSONStorable st = a.GetStorableByID(id);
                if (TryBindVoiceController(a, st, false)) return true;
            }
        }
        catch { }

        return false;
    }

    bool TryBindVoiceController(Atom a, JSONStorable st, bool log)
    {
        if (a == null || st == null) return false;

        JSONStorableFloat spk = null;
        JSONStorableFloat spd = null;

        try { spk = st.GetFloatJSONParam("VoiceVoxSpeakerId"); } catch { }
        try { spd = st.GetFloatJSONParam("VV_Speed"); } catch { }

        if (spk == null || spd == null) return false;

        _voiceAtom = a;
        _voiceStorable = st;

        _targetSpeakerId = spk;

        try { _targetSpeed = st.GetFloatJSONParam("VV_Speed"); } catch { _targetSpeed = null; }
        try { _targetPitch = st.GetFloatJSONParam("VV_Pitch"); } catch { _targetPitch = null; }
        try { _targetIntonation = st.GetFloatJSONParam("VV_Intonation"); } catch { _targetIntonation = null; }
        try { _targetVolumeScale = st.GetFloatJSONParam("VV_VolumeScale"); } catch { _targetVolumeScale = null; }

        if (_voiceAtomUid != null) _voiceAtomUid.val = a.uid;
        if (_voiceStorableId != null) _voiceStorableId.val = st.storeId;

        if (log)
        {
            SuperController.LogMessage("RM_AIChatCompanionBridgeMini: Bound VoiceController -> Atom=" + a.uid + " Storable=" + st.storeId);
        }
        return true;
    }

    // ============================================================
    // VoiceController へパラメータ反映
    // ============================================================
    void ApplyVoiceParamsToController(bool log)
    {
        if (_voiceStorable == null)
        {
            AutoDetectVoiceController(log);
        }

        if (_voiceStorable == null)
        {
            if (log) SuperController.LogMessage("RM_AIChatCompanionBridgeMini: VoiceController not bound. param apply skipped.");
            return;
        }

        try
        {
            if (_targetSpeakerId != null && _speakerId != null)
                _targetSpeakerId.val = _speakerId.val;

            if (_targetSpeed != null && _vvSpeed != null)
                _targetSpeed.val = _vvSpeed.val;

            if (_targetPitch != null && _vvPitch != null)
                _targetPitch.val = _vvPitch.val;

            if (_targetIntonation != null && _vvIntonation != null)
                _targetIntonation.val = _vvIntonation.val;

            if (_targetVolumeScale != null && _vvVolumeScale != null)
                _targetVolumeScale.val = _vvVolumeScale.val;

            if (log)
            {
                SuperController.LogMessage(
                    "RM_AIChatCompanionBridgeMini: Voice params applied " +
                    "(SpeakerId=" + (_speakerId != null ? _speakerId.val.ToString("0.###", CultureInfo.InvariantCulture) : "?") +
                    ", Speed=" + (_vvSpeed != null ? _vvSpeed.val.ToString("0.###", CultureInfo.InvariantCulture) : "?") +
                    ", Pitch=" + (_vvPitch != null ? _vvPitch.val.ToString("0.###", CultureInfo.InvariantCulture) : "?") +
                    ", Intonation=" + (_vvIntonation != null ? _vvIntonation.val.ToString("0.###", CultureInfo.InvariantCulture) : "?") +
                    ", VolumeScale=" + (_vvVolumeScale != null ? _vvVolumeScale.val.ToString("0.###", CultureInfo.InvariantCulture) : "?") +
                    ")"
                );
            }
        }
        catch (Exception e)
        {
            SuperController.LogError("RM_AIChatCompanionBridgeMini: ApplyVoiceParamsToController exception: " + e);
        }
    }
}