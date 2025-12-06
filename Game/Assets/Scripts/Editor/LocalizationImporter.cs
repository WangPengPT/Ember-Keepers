using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using EmberKeepers.Localization;

namespace EmberKeepers.Editor
{
    /// <summary>
    /// 本地化数据导入工具 - 从JSON文件导入本地化数据
    /// </summary>
    public class LocalizationImporter : EditorWindow
    {
        private string jsonFilePath = "";
        private LocalizationData targetAsset;
        private Vector2 scrollPosition;

        [MenuItem("EmberKeepers/本地化/导入本地化数据")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationImporter>("本地化数据导入工具");
        }

        private void OnGUI()
        {
            GUILayout.Label("本地化数据导入工具", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 选择JSON文件
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("JSON文件路径:", GUILayout.Width(100));
            jsonFilePath = EditorGUILayout.TextField(jsonFilePath);
            if (GUILayout.Button("浏览", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFilePanel("选择JSON文件", Application.dataPath, "json");
                if (!string.IsNullOrEmpty(path))
                {
                    jsonFilePath = path;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // 选择目标资源
            targetAsset = EditorGUILayout.ObjectField("目标资源:", targetAsset, typeof(LocalizationData), false) as LocalizationData;

            EditorGUILayout.Space();

            // 创建新资源按钮
            if (GUILayout.Button("创建新的本地化数据资源"))
            {
                CreateNewLocalizationData();
            }

            EditorGUILayout.Space();

            // 导入按钮
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(jsonFilePath) || targetAsset == null);
            if (GUILayout.Button("导入JSON数据", GUILayout.Height(30)))
            {
                ImportFromJSON();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("JSON格式说明:\n" +
                "{\n" +
                "  \"strings\": [\n" +
                "    {\n" +
                "      \"key\": \"ui.menu.start\",\n" +
                "      \"en\": \"Start Game\",\n" +
                "      \"zh-CN\": \"开始游戏\",\n" +
                "      \"zh-TW\": \"開始遊戲\",\n" +
                "      \"ja\": \"ゲーム開始\",\n" +
                "      \"ko\": \"게임 시작\",\n" +
                "      \"de\": \"Spiel starten\",\n" +
                "      \"fr\": \"Démarrer le jeu\",\n" +
                "      \"it\": \"Inizia il gioco\",\n" +
                "      \"pt\": \"Iniciar jogo\",\n" +
                "      \"es\": \"Iniciar juego\"\n" +
                "    }\n" +
                "  ]\n" +
                "}", MessageType.Info);
        }

        private void CreateNewLocalizationData()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "创建本地化数据资源",
                "LocalizationData",
                "asset",
                "选择保存位置");

            if (!string.IsNullOrEmpty(path))
            {
                LocalizationData newAsset = CreateInstance<LocalizationData>();
                AssetDatabase.CreateAsset(newAsset, path);
                AssetDatabase.SaveAssets();
                targetAsset = newAsset;
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newAsset;
            }
        }

        private void ImportFromJSON()
        {
            if (!File.Exists(jsonFilePath))
            {
                EditorUtility.DisplayDialog("错误", "JSON文件不存在！", "确定");
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                LocalizationJSONData jsonData = ParseJSON(jsonContent);

                if (jsonData == null || jsonData.strings == null)
                {
                    EditorUtility.DisplayDialog("错误", "JSON格式不正确！", "确定");
                    return;
                }

                // 清空现有数据
                targetAsset.languageTexts.Clear();

                // 创建语言数据字典
                Dictionary<Language, LocalizationData.LanguageTexts> langDict = new Dictionary<Language, LocalizationData.LanguageTexts>();

                // 初始化所有语言
                foreach (Language lang in System.Enum.GetValues(typeof(Language)))
                {
                    langDict[lang] = new LocalizationData.LanguageTexts
                    {
                        language = lang,
                        strings = new List<LocalizationData.LocalizedString>()
                    };
                }

                // 导入数据
                foreach (var jsonString in jsonData.strings)
                {
                    if (string.IsNullOrEmpty(jsonString.key))
                        continue;

                    // 为每个语言添加文本
                    AddLanguageText(langDict[Language.English], jsonString.key, jsonString.en);
                    AddLanguageText(langDict[Language.SimplifiedChinese], jsonString.key, jsonString.zh_CN);
                    AddLanguageText(langDict[Language.TraditionalChinese], jsonString.key, jsonString.zh_TW);
                    AddLanguageText(langDict[Language.Japanese], jsonString.key, jsonString.ja);
                    AddLanguageText(langDict[Language.Korean], jsonString.key, jsonString.ko);
                    AddLanguageText(langDict[Language.German], jsonString.key, jsonString.de);
                    AddLanguageText(langDict[Language.French], jsonString.key, jsonString.fr);
                    AddLanguageText(langDict[Language.Italian], jsonString.key, jsonString.it);
                    AddLanguageText(langDict[Language.Portuguese], jsonString.key, jsonString.pt);
                    AddLanguageText(langDict[Language.Spanish], jsonString.key, jsonString.es);
                }

                // 添加到资源
                foreach (var langData in langDict.Values)
                {
                    if (langData.strings.Count > 0)
                    {
                        targetAsset.languageTexts.Add(langData);
                    }
                }

                EditorUtility.SetDirty(targetAsset);
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("成功", $"成功导入 {jsonData.strings.Count} 条本地化字符串！", "确定");
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("错误", $"导入失败: {e.Message}", "确定");
                Debug.LogError($"LocalizationImporter: {e}");
            }
        }

        private void AddLanguageText(LocalizationData.LanguageTexts langData, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                langData.strings.Add(new LocalizationData.LocalizedString
                {
                    key = key,
                    value = value
                });
            }
        }

        [System.Serializable]
        private class LocalizationJSONData
        {
            public List<LocalizedStringJSON> strings;
        }

        [System.Serializable]
        private class LocalizedStringJSON
        {
            public string key;
            public string en;
            public string zh_CN;
            public string zh_TW;
            public string ja;
            public string ko;
            public string de;
            public string fr;
            public string it;
            public string pt;
            public string es;
        }

        /// <summary>
        /// 使用Unity JsonUtility解析JSON
        /// </summary>
        private LocalizationJSONData ParseJSON(string jsonContent)
        {
            try
            {
                // Unity JsonUtility可以直接解析这种结构
                LocalizationJSONData data = JsonUtility.FromJson<LocalizationJSONData>(jsonContent);
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON解析失败: {e.Message}");
                throw;
            }
        }
    }
}

