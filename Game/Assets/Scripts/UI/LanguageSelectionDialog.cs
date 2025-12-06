using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Localization;
using System.Collections.Generic;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 语言选择对话框 - 用于首次启动时选择语言
    /// </summary>
    public class LanguageSelectionDialog : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private GameObject dialogContent;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Transform languageButtonContainer;
        [SerializeField] private GameObject languageButtonPrefab;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI confirmButtonText;
        [SerializeField] private Image backgroundOverlay;

        [Header("Localized Keys")]
        [SerializeField] private string titleKey = "ui.language_selection.title";
        [SerializeField] private string descriptionKey = "ui.language_selection.description";
        [SerializeField] private string confirmKey = "ui.language_selection.confirm";

        private Language selectedLanguage = Language.English;
        private Dictionary<Language, Button> languageButtons = new Dictionary<Language, Button>();

        private void Awake()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }
        }

        private void Start()
        {
            SetupLanguageButtons();
            SetupConfirmButton();
        }

        private void OnEnable()
        {
            // 订阅语言切换事件，以便更新UI文本
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
            }
        }

        private void OnDisable()
        {
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        /// <summary>
        /// 显示语言选择对话框
        /// </summary>
        public void ShowDialog()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(true);
            }

            if (backgroundOverlay != null)
            {
                backgroundOverlay.gameObject.SetActive(true);
            }

            // 设置默认选中语言（如果已设置）
            if (LocalizationManager.Instance != null)
            {
                selectedLanguage = LocalizationManager.Instance.GetCurrentLanguage();
            }
            else
            {
                // 首次启动时默认选择英语
                selectedLanguage = Language.English;
            }

            UpdateButtonSelection();
            UpdateTexts();
        }

        /// <summary>
        /// 隐藏语言选择对话框
        /// </summary>
        public void HideDialog()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }

            if (backgroundOverlay != null)
            {
                backgroundOverlay.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置语言按钮
        /// </summary>
        private void SetupLanguageButtons()
        {
            if (languageButtonContainer == null || languageButtonPrefab == null)
            {
                Debug.LogWarning("LanguageSelectionDialog: 语言按钮容器或预制体未设置");
                return;
            }

            if (LocalizationManager.Instance == null)
            {
                Debug.LogWarning("LanguageSelectionDialog: LocalizationManager未初始化");
                return;
            }

            Language[] languages = LocalizationManager.Instance.GetSupportedLanguages();

            foreach (Language lang in languages)
            {
                GameObject buttonObj = Instantiate(languageButtonPrefab, languageButtonContainer);
                Button button = buttonObj.GetComponent<Button>();
                
                if (button != null)
                {
                    // 设置按钮文本
                    TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                    {
                        buttonText.text = GetLanguageDisplayName(lang);
                    }

                    // 设置按钮点击事件
                    Language currentLang = lang; // 闭包变量
                    button.onClick.AddListener(() => OnLanguageButtonClicked(currentLang));

                    languageButtons[lang] = button;
                }
            }

            UpdateButtonSelection();
        }

        /// <summary>
        /// 设置确认按钮
        /// </summary>
        private void SetupConfirmButton()
        {
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(OnConfirmClicked);
            }
        }

        /// <summary>
        /// 语言按钮点击事件
        /// </summary>
        private void OnLanguageButtonClicked(Language language)
        {
            selectedLanguage = language;
            UpdateButtonSelection();
        }

        /// <summary>
        /// 更新按钮选中状态
        /// </summary>
        private void UpdateButtonSelection()
        {
            foreach (var kvp in languageButtons)
            {
                if (kvp.Value != null)
                {
                    // 可以通过改变按钮颜色或添加选中标记来显示选中状态
                    var colors = kvp.Value.colors;
                    if (kvp.Key == selectedLanguage)
                    {
                        colors.normalColor = new Color(0.4f, 0.7f, 1f); // 更明显的蓝色表示选中
                        colors.highlightedColor = new Color(0.5f, 0.8f, 1f);
                    }
                    else
                    {
                        colors.normalColor = Color.white;
                        colors.highlightedColor = new Color(0.95f, 0.95f, 0.95f);
                    }
                    kvp.Value.colors = colors;
                }
            }
        }

        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        private void OnConfirmClicked()
        {
            if (LocalizationManager.Instance != null)
            {
                // 首次启动时标记语言已选择
                LocalizationManager.Instance.SetLanguage(selectedLanguage, markAsSelected: true);
                HideDialog();
            }
            else
            {
                Debug.LogError("LanguageSelectionDialog: LocalizationManager未初始化，无法设置语言");
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
        /// 更新UI文本
        /// </summary>
        private void UpdateTexts()
        {
            if (LocalizationManager.Instance == null)
            {
                // 如果LocalizationManager未初始化，使用默认英语文本
                if (titleText != null) titleText.text = "Select Language";
                if (descriptionText != null) descriptionText.text = "Please select your preferred language";
                if (confirmButtonText != null) confirmButtonText.text = "Confirm";
                return;
            }

            if (titleText != null)
            {
                titleText.text = LocalizationManager.Instance.GetLocalizedText(titleKey);
            }

            if (descriptionText != null)
            {
                descriptionText.text = LocalizationManager.Instance.GetLocalizedText(descriptionKey);
            }

            if (confirmButtonText != null)
            {
                confirmButtonText.text = LocalizationManager.Instance.GetLocalizedText(confirmKey);
            }
        }

        /// <summary>
        /// 语言切换事件回调
        /// </summary>
        private void OnLanguageChanged(Language newLanguage)
        {
            UpdateTexts();
        }
    }
}

