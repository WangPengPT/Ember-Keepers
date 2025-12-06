using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.Localization
{
    /// <summary>
    /// 本地化数据资源 - 存储所有语言的文本
    /// </summary>
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "EmberKeepers/Localization/LocalizationData")]
    public class LocalizationData : ScriptableObject
    {
        [System.Serializable]
        public class LanguageTexts
        {
            public Language language;
            public List<LocalizedString> strings = new List<LocalizedString>();
        }

        [System.Serializable]
        public class LocalizedString
        {
            public string key;
            public string value;
        }

        [Header("Language Texts")]
        public List<LanguageTexts> languageTexts = new List<LanguageTexts>();

        /// <summary>
        /// 获取指定语言的文本
        /// </summary>
        public string GetText(string key, Language language)
        {
            var langData = languageTexts.Find(l => l.language == language);
            if (langData == null)
            {
                // 如果找不到指定语言，尝试使用英语
                langData = languageTexts.Find(l => l.language == Language.English);
                if (langData == null)
                {
                    Debug.LogWarning($"LocalizationData: 找不到语言数据 {language}，且没有英语数据");
                    return key;
                }
            }

            var localizedString = langData.strings.Find(s => s.key == key);
            if (localizedString == null)
            {
                Debug.LogWarning($"LocalizationData: 找不到键 '{key}' 在语言 {language} 中");
                return key;
            }

            return localizedString.value;
        }

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        public bool HasKey(string key, Language language)
        {
            var langData = languageTexts.Find(l => l.language == language);
            if (langData == null) return false;
            return langData.strings.Exists(s => s.key == key);
        }
    }
}

