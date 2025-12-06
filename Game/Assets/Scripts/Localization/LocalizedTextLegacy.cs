using UnityEngine;
using UnityEngine.UI;

namespace EmberKeepers.Localization
{
    /// <summary>
    /// 本地化文本组件（Legacy Text版本）
    /// 用于不支持TextMeshPro的情况
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedTextLegacy : MonoBehaviour
    {
        [Header("Localization")]
        [SerializeField] private string localizationKey;
        [SerializeField] private bool updateOnStart = true;
        [SerializeField] private bool updateOnEnable = true;

        private Text textComponent;
        private bool isInitialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            if (updateOnStart)
            {
                UpdateText();
            }
        }

        private void OnEnable()
        {
            if (updateOnEnable && isInitialized)
            {
                UpdateText();
            }

            // 订阅语言切换事件
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
            }
        }

        private void OnDisable()
        {
            // 取消订阅语言切换事件
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        private void Initialize()
        {
            textComponent = GetComponent<Text>();

            if (textComponent == null)
            {
                Debug.LogWarning($"LocalizedTextLegacy: GameObject '{gameObject.name}' 上没有找到 Text 组件");
            }

            isInitialized = true;
        }

        /// <summary>
        /// 设置本地化键
        /// </summary>
        public void SetLocalizationKey(string key)
        {
            localizationKey = key;
            UpdateText();
        }

        /// <summary>
        /// 获取本地化键
        /// </summary>
        public string GetLocalizationKey()
        {
            return localizationKey;
        }

        /// <summary>
        /// 更新文本
        /// </summary>
        public void UpdateText()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            if (string.IsNullOrEmpty(localizationKey))
            {
                Debug.LogWarning($"LocalizedTextLegacy: GameObject '{gameObject.name}' 的本地化键为空");
                return;
            }

            if (LocalizationManager.Instance == null)
            {
                Debug.LogWarning($"LocalizedTextLegacy: LocalizationManager 实例不存在");
                return;
            }

            string localizedText = LocalizationManager.Instance.GetLocalizedText(localizationKey);

            if (textComponent != null)
            {
                textComponent.text = localizedText;
            }
        }

        /// <summary>
        /// 更新文本（带参数）
        /// </summary>
        public void UpdateText(params object[] args)
        {
            if (!isInitialized)
            {
                Initialize();
            }

            if (string.IsNullOrEmpty(localizationKey))
            {
                Debug.LogWarning($"LocalizedTextLegacy: GameObject '{gameObject.name}' 的本地化键为空");
                return;
            }

            if (LocalizationManager.Instance == null)
            {
                Debug.LogWarning($"LocalizedTextLegacy: LocalizationManager 实例不存在");
                return;
            }

            string localizedText = LocalizationManager.Instance.GetLocalizedText(localizationKey, args);

            if (textComponent != null)
            {
                textComponent.text = localizedText;
            }
        }

        /// <summary>
        /// 语言切换事件回调
        /// </summary>
        private void OnLanguageChanged(Language newLanguage)
        {
            UpdateText();
        }

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器模式下预览文本
        /// </summary>
        [ContextMenu("Preview Text")]
        private void PreviewText()
        {
            UpdateText();
        }
#endif
    }
}

