using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EmberKeepers.UI;

namespace EmberKeepers.Editor
{
    /// <summary>
    /// UI Prefab生成工具 - 自动为所有UI脚本生成prefab
    /// </summary>
    public class UIPrefabGenerator : EditorWindow
    {
        private string prefabOutputPath = "Assets/Prefabs/UI";
        private Vector2 scrollPosition;
        private List<UIScriptInfo> uiScripts = new List<UIScriptInfo>();
        private bool[] selectedScripts;

        [MenuItem("EmberKeepers/UI/生成所有UI Prefab")]
        public static void ShowWindow()
        {
            GetWindow<UIPrefabGenerator>("UI Prefab生成工具");
        }

        private void OnEnable()
        {
            ScanUIScripts();
        }

        private void OnGUI()
        {
            GUILayout.Label("UI Prefab生成工具", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 输出路径设置
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab输出路径:", GUILayout.Width(120));
            prefabOutputPath = EditorGUILayout.TextField(prefabOutputPath);
            if (GUILayout.Button("浏览", GUILayout.Width(60)))
            {
                string path = EditorUtility.SaveFolderPanel("选择Prefab输出文件夹", prefabOutputPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    prefabOutputPath = "Assets" + path.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // 刷新按钮
            if (GUILayout.Button("刷新UI脚本列表"))
            {
                ScanUIScripts();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"找到 {uiScripts.Count} 个UI脚本:", EditorStyles.boldLabel);

            // 脚本列表
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            if (uiScripts.Count > 0)
            {
                if (selectedScripts == null || selectedScripts.Length != uiScripts.Count)
                {
                    selectedScripts = new bool[uiScripts.Count];
                    for (int i = 0; i < selectedScripts.Length; i++)
                    {
                        selectedScripts[i] = true; // 默认全选
                    }
                }

                for (int i = 0; i < uiScripts.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    selectedScripts[i] = EditorGUILayout.Toggle(selectedScripts[i], GUILayout.Width(20));
                    EditorGUILayout.LabelField(uiScripts[i].scriptName, GUILayout.Width(200));
                    EditorGUILayout.LabelField($"({uiScripts[i].fieldCount} 个字段)", GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("未找到UI脚本。请确保脚本在 EmberKeepers.UI 命名空间下。", MessageType.Warning);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            // 生成按钮
            EditorGUI.BeginDisabledGroup(uiScripts.Count == 0);
            if (GUILayout.Button("生成选中的UI Prefab", GUILayout.Height(30)))
            {
                GenerateSelectedPrefabs();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "此工具将：\n" +
                "1. 扫描所有UI脚本\n" +
                "2. 根据SerializeField字段自动创建UI元素\n" +
                "3. 生成对应的Prefab\n" +
                "4. 保存到指定路径",
                MessageType.Info);
        }

        private void ScanUIScripts()
        {
            uiScripts.Clear();

            // 获取所有程序集
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        // 检查是否是UI脚本（在EmberKeepers.UI命名空间下，继承MonoBehaviour，且不是UIManager）
                        if (type.Namespace == "EmberKeepers.UI" &&
                            typeof(MonoBehaviour).IsAssignableFrom(type) &&
                            type.Name != "UIManager" &&
                            !type.IsAbstract)
                        {
                            int fieldCount = CountSerializedFields(type);
                            uiScripts.Add(new UIScriptInfo
                            {
                                type = type,
                                scriptName = type.Name,
                                fieldCount = fieldCount
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"扫描程序集 {assembly.FullName} 时出错: {e.Message}");
                }
            }

            uiScripts = uiScripts.OrderBy(s => s.scriptName).ToList();
        }

        private int CountSerializedFields(Type type)
        {
            int count = 0;
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            
            foreach (FieldInfo field in fields)
            {
                if (field.IsDefined(typeof(SerializeField), true) || field.IsPublic)
                {
                    count++;
                }
            }

            return count;
        }

        private void GenerateSelectedPrefabs()
        {
            if (selectedScripts == null || selectedScripts.Length != uiScripts.Count)
            {
                EditorUtility.DisplayDialog("错误", "请先刷新UI脚本列表！", "确定");
                return;
            }

            // 确保输出目录存在
            if (!AssetDatabase.IsValidFolder(prefabOutputPath))
            {
                string[] folders = prefabOutputPath.Split('/');
                string currentPath = folders[0];
                for (int i = 1; i < folders.Length; i++)
                {
                    string newPath = currentPath + "/" + folders[i];
                    if (!AssetDatabase.IsValidFolder(newPath))
                    {
                        AssetDatabase.CreateFolder(currentPath, folders[i]);
                    }
                    currentPath = newPath;
                }
            }

            int successCount = 0;
            int failCount = 0;

            for (int i = 0; i < uiScripts.Count; i++)
            {
                if (selectedScripts[i])
                {
                    try
                    {
                        GeneratePrefabForScript(uiScripts[i]);
                        successCount++;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"生成 {uiScripts[i].scriptName} 的Prefab失败: {e.Message}\n{e.StackTrace}");
                        failCount++;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("完成",
                $"成功生成 {successCount} 个Prefab\n" +
                (failCount > 0 ? $"失败 {failCount} 个" : ""),
                "确定");
        }

        private void GeneratePrefabForScript(UIScriptInfo scriptInfo)
        {
            // 创建临时场景对象（不添加到当前场景）
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // 创建UI根对象
            GameObject uiRoot = new GameObject(scriptInfo.scriptName);
            uiRoot.transform.SetParent(canvasObj.transform, false);

            RectTransform rootRect = uiRoot.AddComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.sizeDelta = Vector2.zero;
            rootRect.anchoredPosition = Vector2.zero;

            // 添加UI脚本组件
            Component uiComponent = uiRoot.AddComponent(scriptInfo.type);

            // 获取所有序列化字段
            FieldInfo[] fields = scriptInfo.type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // 按Header分组字段
            Dictionary<string, List<FieldInfo>>> headerGroups = new Dictionary<string, List<FieldInfo>>();
            List<FieldInfo> ungroupedFields = new List<FieldInfo>();

            foreach (FieldInfo field in fields)
            {
                if (!IsSerializedField(field)) continue;

                HeaderAttribute header = field.GetCustomAttribute<HeaderAttribute>();
                if (header != null)
                {
                    if (!headerGroups.ContainsKey(header.header))
                    {
                        headerGroups[header.header] = new List<FieldInfo>();
                    }
                    headerGroups[header.header].Add(field);
                }
                else
                {
                    ungroupedFields.Add(field);
                }
            }

            // 创建分组容器
            float yOffset = -20f;
            const float groupSpacing = 30f;
            const float itemSpacing = 25f;

            // 创建未分组的字段
            if (ungroupedFields.Count > 0)
            {
                yOffset = CreateFieldGroup(uiRoot, "General", ungroupedFields, uiComponent, yOffset, itemSpacing);
                yOffset -= groupSpacing;
            }

            // 创建分组的字段
            foreach (var group in headerGroups)
            {
                yOffset = CreateFieldGroup(uiRoot, group.Key, group.Value, uiComponent, yOffset, itemSpacing);
                yOffset -= groupSpacing;
            }

            // 保存为Prefab（只保存UI根对象，不包含Canvas）
            string prefabPath = $"{prefabOutputPath}/{scriptInfo.scriptName}.prefab";
            
            // 临时将uiRoot从canvasObj分离，以便单独保存
            uiRoot.transform.SetParent(null, false);
            
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(uiRoot, prefabPath);
            
            // 清理临时对象
            DestroyImmediate(uiRoot);
            DestroyImmediate(canvasObj);

            Debug.Log($"成功生成Prefab: {prefabPath}");
        }

        private float CreateFieldGroup(GameObject parent, string groupName, List<FieldInfo> fields, Component uiComponent, float startY, float spacing)
        {
            // 创建分组容器
            GameObject groupObj = new GameObject(groupName);
            groupObj.transform.SetParent(parent.transform, false);

            RectTransform groupRect = groupObj.AddComponent<RectTransform>();
            groupRect.anchorMin = new Vector2(0, 1);
            groupRect.anchorMax = new Vector2(1, 1);
            groupRect.pivot = new Vector2(0.5f, 1);
            groupRect.anchoredPosition = new Vector2(0, startY);
            groupRect.sizeDelta = new Vector2(0, fields.Count * spacing + 40);

            // 添加背景（可选）
            Image bgImage = groupObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.3f);

            // 创建标题
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(groupObj.transform, false);
            TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
            titleText.text = groupName;
            titleText.fontSize = 12;
            titleText.fontStyle = FontStyles.Bold;
            titleText.color = new Color(0.8f, 0.8f, 0.8f, 1f);

            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.pivot = new Vector2(0, 1);
            titleRect.anchoredPosition = new Vector2(10, -5);
            titleRect.sizeDelta = new Vector2(-20, 20);

            // 创建字段UI元素
            float currentY = -30f;
            foreach (FieldInfo field in fields)
            {
                GameObject fieldObj = CreateUIElementForField(field, groupObj, currentY);
                if (fieldObj != null)
                {
                    // 将引用赋值给组件
                    AssignFieldReference(field, uiComponent, fieldObj);
                    currentY -= spacing;
                }
            }

            return startY - (fields.Count * spacing + 40);
        }

        private GameObject CreateUIElementForField(FieldInfo field, GameObject parent, float yPosition)
        {
            Type fieldType = field.FieldType;
            string fieldName = field.Name;

            GameObject elementObj = null;
            RectTransform elementRect = null;

            // 根据字段类型创建对应的UI元素
            if (fieldType == typeof(Button))
            {
                elementObj = CreateButton(fieldName);
            }
            else if (fieldType == typeof(TextMeshProUGUI) || fieldType == typeof(Text))
            {
                elementObj = CreateText(fieldName, fieldType == typeof(TextMeshProUGUI));
            }
            else if (fieldType == typeof(Slider))
            {
                elementObj = CreateSlider(fieldName);
            }
            else if (fieldType == typeof(Image))
            {
                elementObj = CreateImage(fieldName);
            }
            else if (fieldType == typeof(Toggle))
            {
                elementObj = CreateToggle(fieldName);
            }
            else if (fieldType == typeof(Transform))
            {
                elementObj = CreateContainer(fieldName);
            }
            else if (fieldType == typeof(GameObject))
            {
                elementObj = CreateGameObjectContainer(fieldName);
            }
            else if (fieldType.IsArray)
            {
                elementObj = CreateArrayContainer(fieldName, fieldType);
            }
            else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                elementObj = CreateListContainer(fieldName, fieldType);
            }
            else
            {
                // 未知类型，创建通用容器
                elementObj = CreateContainer(fieldName);
            }

            if (elementObj != null)
            {
                elementObj.transform.SetParent(parent.transform, false);
                elementRect = elementObj.GetComponent<RectTransform>();
                if (elementRect != null)
                {
                    elementRect.anchorMin = new Vector2(0, 1);
                    elementRect.anchorMax = new Vector2(1, 1);
                    elementRect.pivot = new Vector2(0.5f, 1);
                    elementRect.anchoredPosition = new Vector2(0, yPosition);
                    elementRect.sizeDelta = new Vector2(-20, 25);
                }
            }

            return elementObj;
        }

        private GameObject CreateButton(string name)
        {
            GameObject btnObj = new GameObject(name);
            Image img = btnObj.AddComponent<Image>();
            img.color = new Color(0.2f, 0.6f, 0.2f, 1f);
            Button btn = btnObj.AddComponent<Button>();

            // 添加文本子对象
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = name;
            text.fontSize = 14;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;

            return btnObj;
        }

        private GameObject CreateText(string name, bool isTMP)
        {
            GameObject textObj = new GameObject(name);
            if (isTMP)
            {
                TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
                text.text = name;
                text.fontSize = 14;
                text.color = Color.white;
            }
            else
            {
                Text text = textObj.AddComponent<Text>();
                text.text = name;
                text.fontSize = 14;
                text.color = Color.white;
            }

            return textObj;
        }

        private GameObject CreateSlider(string name)
        {
            GameObject sliderObj = new GameObject(name);
            Slider slider = sliderObj.AddComponent<Slider>();

            // 背景
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(sliderObj.transform, false);
            Image bgImg = bgObj.AddComponent<Image>();
            bgImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;

            // Fill Area
            GameObject fillAreaObj = new GameObject("Fill Area");
            fillAreaObj.transform.SetParent(sliderObj.transform, false);
            RectTransform fillAreaRect = fillAreaObj.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.sizeDelta = Vector2.zero;
            fillAreaRect.anchoredPosition = Vector2.zero;

            // Fill
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(fillAreaObj.transform, false);
            Image fillImg = fillObj.AddComponent<Image>();
            fillImg.color = new Color(0.2f, 0.8f, 0.2f, 1f);
            slider.fillRect = fillObj.GetComponent<RectTransform>();

            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0.5f, 1f);
            fillRect.sizeDelta = Vector2.zero;
            fillRect.anchoredPosition = Vector2.zero;

            // Handle Slide Area
            GameObject handleAreaObj = new GameObject("Handle Slide Area");
            handleAreaObj.transform.SetParent(sliderObj.transform, false);
            RectTransform handleAreaRect = handleAreaObj.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.sizeDelta = Vector2.zero;
            handleAreaRect.anchoredPosition = Vector2.zero;

            // Handle
            GameObject handleObj = new GameObject("Handle");
            handleObj.transform.SetParent(handleAreaObj.transform, false);
            Image handleImg = handleObj.AddComponent<Image>();
            handleImg.color = Color.white;
            slider.targetGraphic = handleImg;

            RectTransform handleRect = handleObj.GetComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(0.5f, 0.5f);
            handleRect.anchorMax = new Vector2(0.5f, 0.5f);
            handleRect.sizeDelta = new Vector2(20, 20);
            handleRect.anchoredPosition = Vector2.zero;

            slider.handleRect = handleRect;

            return sliderObj;
        }

        private GameObject CreateImage(string name)
        {
            GameObject imgObj = new GameObject(name);
            Image img = imgObj.AddComponent<Image>();
            img.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            return imgObj;
        }

        private GameObject CreateToggle(string name)
        {
            GameObject toggleObj = new GameObject(name);
            Toggle toggle = toggleObj.AddComponent<Toggle>();

            // Background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(toggleObj.transform, false);
            Image bgImg = bgObj.AddComponent<Image>();
            bgImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;

            // Checkmark
            GameObject checkObj = new GameObject("Checkmark");
            checkObj.transform.SetParent(bgObj.transform, false);
            Image checkImg = checkObj.AddComponent<Image>();
            checkImg.color = Color.green;

            RectTransform checkRect = checkObj.GetComponent<RectTransform>();
            checkRect.anchorMin = Vector2.zero;
            checkRect.anchorMax = Vector2.one;
            checkRect.sizeDelta = Vector2.zero;
            checkRect.anchoredPosition = Vector2.zero;

            toggle.graphic = checkImg;
            toggle.targetGraphic = bgImg;

            // Label
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(toggleObj.transform, false);
            TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
            labelText.text = name;
            labelText.fontSize = 14;
            labelText.color = Color.white;

            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(1.1f, 0);
            labelRect.anchorMax = new Vector2(1.1f, 1);
            labelRect.pivot = new Vector2(0, 0.5f);
            labelRect.sizeDelta = new Vector2(100, 0);
            labelRect.anchoredPosition = Vector2.zero;

            return toggleObj;
        }

        private GameObject CreateContainer(string name)
        {
            GameObject containerObj = new GameObject(name);
            RectTransform rect = containerObj.AddComponent<RectTransform>();
            Image img = containerObj.AddComponent<Image>();
            img.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
            return containerObj;
        }

        private GameObject CreateGameObjectContainer(string name)
        {
            GameObject containerObj = new GameObject(name);
            RectTransform rect = containerObj.AddComponent<RectTransform>();
            return containerObj;
        }

        private GameObject CreateArrayContainer(string name, Type arrayType)
        {
            GameObject containerObj = new GameObject($"{name} (Array)");
            RectTransform rect = containerObj.AddComponent<RectTransform>();
            Image img = containerObj.AddComponent<Image>();
            img.color = new Color(0.1f, 0.3f, 0.1f, 0.3f);

            // 添加Vertical Layout Group
            VerticalLayoutGroup layout = containerObj.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 5;
            layout.padding = new RectOffset(5, 5, 5, 5);

            return containerObj;
        }

        private GameObject CreateListContainer(string name, Type listType)
        {
            GameObject containerObj = new GameObject($"{name} (List)");
            RectTransform rect = containerObj.AddComponent<RectTransform>();
            Image img = containerObj.AddComponent<Image>();
            img.color = new Color(0.1f, 0.3f, 0.1f, 0.3f);

            // 添加Vertical Layout Group
            VerticalLayoutGroup layout = containerObj.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 5;
            layout.padding = new RectOffset(5, 5, 5, 5);

            return containerObj;
        }

        private void AssignFieldReference(FieldInfo field, Component component, GameObject uiElement)
        {
            try
            {
                Type fieldType = field.FieldType;
                object value = null;

                if (fieldType == typeof(Button))
                {
                    value = uiElement.GetComponent<Button>();
                }
                else if (fieldType == typeof(TextMeshProUGUI))
                {
                    value = uiElement.GetComponent<TextMeshProUGUI>();
                }
                else if (fieldType == typeof(Text))
                {
                    value = uiElement.GetComponent<Text>();
                }
                else if (fieldType == typeof(Slider))
                {
                    value = uiElement.GetComponent<Slider>();
                }
                else if (fieldType == typeof(Image))
                {
                    value = uiElement.GetComponent<Image>();
                }
                else if (fieldType == typeof(Toggle))
                {
                    value = uiElement.GetComponent<Toggle>();
                }
                else if (fieldType == typeof(Transform))
                {
                    value = uiElement.transform;
                }
                else if (fieldType == typeof(GameObject))
                {
                    value = uiElement;
                }

                if (value != null)
                {
                    field.SetValue(component, value);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"无法为字段 {field.Name} 赋值: {e.Message}");
            }
        }

        private bool IsSerializedField(FieldInfo field)
        {
            return field.IsDefined(typeof(SerializeField), true) || 
                   (field.IsPublic && !field.IsStatic && !field.IsLiteral);
        }

        private class UIScriptInfo
        {
            public Type type;
            public string scriptName;
            public int fieldCount;
        }
    }
}

