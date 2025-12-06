using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Localization;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 设置面板 - 包含语言选择等设置选项
    /// </summary>
    public class SettingsPanel : MonoBehaviour
    {
        [Header("Language Selection")]
        [SerializeField] private GameObject languageGroup;
        [SerializeField] private TextMeshProUGUI languageLabelText;
        [SerializeField] private Image languageIcon;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private Dropdown legacyLanguageDropdown; // Unity Legacy UI

        [Header("Localized Keys")]
        [SerializeField] private string languageLabelKey = "ui.language";

        [Header("Panel Title")]
        [SerializeField] private TextMeshProUGUI panelTitleText;

        private void Start()
        {
            SetupLanguageSelector();
        }

        private void OnEnable()
        {
            // 订阅语言切换事件
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
            }
            UpdateLanguageLabel();
        }

        private void OnDisable()
        {
            // 取消订阅
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        /// <summary>
        /// 设置语言选择器
        /// </summary>
        private void SetupLanguageSelector()
        {
            if (LocalizationManager.Instance == null)
            {
                Debug.LogWarning("SettingsPanel: LocalizationManager未初始化");
                return;
            }

            Language[] languages = LocalizationManager.Instance.GetSupportedLanguages();

            // 设置 TextMeshPro Dropdown
            if (languageDropdown != null)
            {
                languageDropdown.ClearOptions();
                var options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>();
                foreach (var lang in languages)
                {
                    options.Add(new TMP_Dropdown.OptionData(GetLanguageDisplayName(lang)));
                }
                languageDropdown.AddOptions(options);
                languageDropdown.value = (int)LocalizationManager.Instance.GetCurrentLanguage();
                languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
            }

            // 设置 Legacy Dropdown
            if (legacyLanguageDropdown != null)
            {
                legacyLanguageDropdown.ClearOptions();
                var options = new System.Collections.Generic.List<Dropdown.OptionData>();
                foreach (var lang in languages)
                {
                    options.Add(new Dropdown.OptionData(GetLanguageDisplayName(lang)));
                }
                legacyLanguageDropdown.AddOptions(options);
                legacyLanguageDropdown.value = (int)LocalizationManager.Instance.GetCurrentLanguage();
                legacyLanguageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
            }
        }

        /// <summary>
        /// 语言下拉菜单变化事件
        /// </summary>
        private void OnLanguageDropdownChanged(int index)
        {
            if (LocalizationManager.Instance != null)
            {
                Language newLanguage = (Language)index;
                LocalizationManager.Instance.SetLanguage(newLanguage);
            }
        }

        /// <summary>
        /// 获取语言的显示名称
        /// </summary>
        private string GetLanguageDisplayName(Language language)
        {
            switch (language)
            {
                case Language.English: return "English";
                case Language.SimplifiedChinese: return "简体中文";
                case Language.TraditionalChinese: return "繁體中文";
                case Language.Japanese: return "日本語";
                case Language.Korean: return "한국어";
                case Language.German: return "Deutsch";
                case Language.French: return "Français";
                case Language.Italian: return "Italiano";
                case Language.Portuguese: return "Português";
                case Language.Spanish: return "Español";
                default: return language.ToString();
            }
        }

        /// <summary>
        /// 更新语言标签文本
        /// </summary>
        private void UpdateLanguageLabel()
        {
            if (languageLabelText != null && LocalizationManager.Instance != null)
            {
                languageLabelText.text = LocalizationManager.Instance.GetLocalizedText(languageLabelKey);
            }
        }

        /// <summary>
        /// 语言切换事件回调
        /// </summary>
        private void OnLanguageChanged(Language newLanguage)
        {
            UpdateLanguageLabel();
            
            // 更新下拉菜单选中项（防止循环触发）
            if (languageDropdown != null)
            {
                languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownChanged);
                languageDropdown.value = (int)newLanguage;
                languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
            }

            if (legacyLanguageDropdown != null)
            {
                legacyLanguageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownChanged);
                legacyLanguageDropdown.value = (int)newLanguage;
                legacyLanguageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
            }
        }
    }
}

