using UnityEngine;
using System;
using System.Collections.Generic;

namespace EmberKeepers.Localization
{
    /// <summary>
    /// 本地化管理器 - 单例，负责管理语言切换和文本获取
    /// </summary>
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        [Header("Localization Settings")]
        [SerializeField] private LocalizationData localizationData;
        [SerializeField] private Language currentLanguage = Language.English;
        [SerializeField] private Language defaultLanguage = Language.English;

        // 语言切换事件
        public event Action<Language> OnLanguageChanged;

        // 缓存当前语言的文本字典，提高查找效率
        private Dictionary<string, string> currentLanguageDict = new Dictionary<string, string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadLanguageSettings();
                
                // 检查LocalizationData是否分配
                if (localizationData == null)
                {
                    Debug.LogWarning("LocalizationManager: LocalizationData未分配！请在Inspector中分配LocalizationData资源。");
                }
                else
                {
                    UpdateLanguageDictionary();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 检查是否是首次启动
        /// </summary>
        public bool IsFirstLaunch()
        {
            return !PlayerPrefs.HasKey("LanguageSelected");
        }

        /// <summary>
        /// 标记语言已选择（首次启动后调用）
        /// </summary>
        public void MarkLanguageSelected()
        {
            PlayerPrefs.SetInt("LanguageSelected", 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 加载语言设置（从PlayerPrefs）
        /// </summary>
        private void LoadLanguageSettings()
        {
            if (PlayerPrefs.HasKey("CurrentLanguage"))
            {
                int langIndex = PlayerPrefs.GetInt("CurrentLanguage");
                if (System.Enum.IsDefined(typeof(Language), langIndex))
                {
                    currentLanguage = (Language)langIndex;
                }
            }
        }

        /// <summary>
        /// 保存语言设置（到PlayerPrefs）
        /// </summary>
        private void SaveLanguageSettings()
        {
            PlayerPrefs.SetInt("CurrentLanguage", (int)currentLanguage);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 更新当前语言的字典缓存
        /// </summary>
        private void UpdateLanguageDictionary()
        {
            currentLanguageDict.Clear();
            
            if (localizationData == null)
            {
                Debug.LogError("LocalizationManager: LocalizationData 未分配！");
                return;
            }

            var langData = localizationData.languageTexts.Find(l => l.language == currentLanguage);
            if (langData == null)
            {
                // 如果找不到当前语言，使用默认语言
                langData = localizationData.languageTexts.Find(l => l.language == defaultLanguage);
                if (langData == null)
                {
                    Debug.LogError($"LocalizationManager: 找不到语言数据 {currentLanguage} 和 {defaultLanguage}");
                    return;
                }
            }

            foreach (var localizedString in langData.strings)
            {
                if (!string.IsNullOrEmpty(localizedString.key))
                {
                    currentLanguageDict[localizedString.key] = localizedString.value;
                }
            }
        }

        /// <summary>
        /// 获取本地化文本
        /// </summary>
        public string GetLocalizedText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            // 先从缓存字典查找
            if (currentLanguageDict.TryGetValue(key, out string value))
            {
                return value;
            }

            // 如果缓存中没有，从LocalizationData查找
            if (localizationData != null)
            {
                string text = localizationData.GetText(key, currentLanguage);
                if (text != key) // 如果找到了文本（不是返回key本身）
                {
                    currentLanguageDict[key] = text; // 缓存起来
                }
                return text;
            }

            Debug.LogWarning($"LocalizationManager: 无法获取键 '{key}' 的本地化文本");
            return key;
        }

        /// <summary>
        /// 获取本地化文本（带参数格式化）
        /// </summary>
        public string GetLocalizedText(string key, params object[] args)
        {
            string text = GetLocalizedText(key);
            try
            {
                return string.Format(text, args);
            }
            catch (FormatException e)
            {
                Debug.LogError($"LocalizationManager: 格式化文本 '{key}' 时出错: {e.Message}");
                return text;
            }
        }

        /// <summary>
        /// 设置当前语言
        /// </summary>
        public void SetLanguage(Language language, bool markAsSelected = true)
        {
            if (currentLanguage != language)
            {
                currentLanguage = language;
                UpdateLanguageDictionary();
                SaveLanguageSettings();
                
                // 如果是首次选择语言，标记为已选择
                if (markAsSelected && IsFirstLaunch())
                {
                    MarkLanguageSelected();
                }
                
                OnLanguageChanged?.Invoke(language);
            }
        }

        /// <summary>
        /// 获取当前语言
        /// </summary>
        public Language GetCurrentLanguage()
        {
            return currentLanguage;
        }

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        public bool HasKey(string key)
        {
            if (localizationData == null) return false;
            return localizationData.HasKey(key, currentLanguage);
        }

        /// <summary>
        /// 设置本地化数据资源
        /// </summary>
        public void SetLocalizationData(LocalizationData data)
        {
            localizationData = data;
            UpdateLanguageDictionary();
        }

        /// <summary>
        /// 获取所有支持的语言
        /// </summary>
        public Language[] GetSupportedLanguages()
        {
            return (Language[])System.Enum.GetValues(typeof(Language));
        }
    }
}

