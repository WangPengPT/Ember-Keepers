using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace EmberKeepers.Localization
{
    /// <summary>
    /// 本地化系统使用示例
    /// </summary>
    public class LocalizationUsageExample : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button changeLanguageButton;

        [Header("Localized Components")]
        [SerializeField] private LocalizedText localizedTitle;
        [SerializeField] private LocalizedText localizedDescription;

        private void Start()
        {
            // 确保LocalizationManager已初始化
            if (LocalizationManager.Instance == null)
            {
                Debug.LogError("LocalizationManager未初始化！请在场景中添加LocalizationManager组件。");
                return;
            }

            // 示例1: 直接使用LocalizationManager获取文本
            if (titleText != null)
            {
                titleText.text = LocalizationManager.Instance.GetLocalizedText("ui.menu.start");
            }

            // 示例2: 使用带参数的格式化文本
            if (descriptionText != null)
            {
                descriptionText.text = LocalizationManager.Instance.GetLocalizedText("ui.stats.strength", 100);
            }

            // 示例3: 使用LocalizedText组件（推荐方式）
            // 只需在Inspector中设置Localization Key即可，无需代码

            // 示例4: 订阅语言切换事件
            LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;

            // 示例5: 设置按钮点击切换语言
            if (changeLanguageButton != null)
            {
                changeLanguageButton.onClick.AddListener(() =>
                {
                    // 循环切换语言
                    Language currentLang = LocalizationManager.Instance.GetCurrentLanguage();
                    Language[] allLangs = LocalizationManager.Instance.GetSupportedLanguages();
                    int currentIndex = System.Array.IndexOf(allLangs, currentLang);
                    int nextIndex = (currentIndex + 1) % allLangs.Length;
                    LocalizationManager.Instance.SetLanguage(allLangs[nextIndex]);
                });
            }
        }

        private void OnDestroy()
        {
            // 取消订阅事件
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        /// <summary>
        /// 语言切换事件回调
        /// </summary>
        private void OnLanguageChanged(Language newLanguage)
        {
            Debug.Log($"语言已切换为: {newLanguage}");

            // 手动更新文本（如果使用LocalizedText组件，会自动更新）
            if (titleText != null)
            {
                titleText.text = LocalizationManager.Instance.GetLocalizedText("ui.menu.start");
            }

            if (descriptionText != null)
            {
                descriptionText.text = LocalizationManager.Instance.GetLocalizedText("ui.stats.strength", 100);
            }
        }

        /// <summary>
        /// 示例：动态更新本地化文本
        /// </summary>
        public void UpdateLocalizedText(string key, params object[] args)
        {
            if (localizedTitle != null)
            {
                localizedTitle.SetLocalizationKey(key);
                if (args != null && args.Length > 0)
                {
                    localizedTitle.UpdateText(args);
                }
                else
                {
                    localizedTitle.UpdateText();
                }
            }
        }
    }
}

