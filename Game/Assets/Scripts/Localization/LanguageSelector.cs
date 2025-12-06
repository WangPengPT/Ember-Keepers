using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EmberKeepers.Localization
{
    /// <summary>
    /// 语言选择器 - 用于UI中的语言切换下拉菜单
    /// </summary>
    public class LanguageSelector : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Dropdown legacyDropdown; // Unity Legacy UI

        private void Start()
        {
            InitializeDropdown();
        }

        private void InitializeDropdown()
        {
            if (LocalizationManager.Instance == null)
            {
                Debug.LogError("LanguageSelector: LocalizationManager 实例不存在");
                return;
            }

            Language[] languages = LocalizationManager.Instance.GetSupportedLanguages();
            
            // 初始化 TextMeshPro Dropdown
            if (dropdown != null)
            {
                dropdown.ClearOptions();
                var options = new System.Collections.Generic.List<TMP_Dropdown.OptionData>();
                foreach (var lang in languages)
                {
                    options.Add(new TMP_Dropdown.OptionData(GetLanguageDisplayName(lang)));
                }
                dropdown.AddOptions(options);
                dropdown.value = (int)LocalizationManager.Instance.GetCurrentLanguage();
                dropdown.onValueChanged.AddListener(OnLanguageChanged);
            }

            // 初始化 Legacy Dropdown
            if (legacyDropdown != null)
            {
                legacyDropdown.ClearOptions();
                var options = new System.Collections.Generic.List<Dropdown.OptionData>();
                foreach (var lang in languages)
                {
                    options.Add(new Dropdown.OptionData(GetLanguageDisplayName(lang)));
                }
                legacyDropdown.AddOptions(options);
                legacyDropdown.value = (int)LocalizationManager.Instance.GetCurrentLanguage();
                legacyDropdown.onValueChanged.AddListener(OnLanguageChanged);
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
        /// 语言切换回调
        /// </summary>
        private void OnLanguageChanged(int index)
        {
            if (LocalizationManager.Instance != null)
            {
                Language newLanguage = (Language)index;
                LocalizationManager.Instance.SetLanguage(newLanguage);
            }
        }
    }
}

