using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EmberKeepers.Core;
using EmberKeepers.Data;
using EmberKeepers.MetaProgression;
using EmberKeepers.Localization;
using EmberKeepers.Utils;
using EmberKeepers.UI;
using EmberKeepers.Base;
using EmberKeepers.Wave;
using EmberKeepers.Combat;
using EmberKeepers.Shop;
using EmberKeepers.Deployment;
using EmberKeepers.Heroes;
using System.IO;

namespace EmberKeepers.Editor
{
    /// <summary>
    /// 场景一键配置工具 - 将空场景配置为可运行的游戏场景
    /// </summary>
    public class SceneSetupTool : EditorWindow
    {
        private Vector2 scrollPosition;
        private bool showAdvancedOptions = false;

        [MenuItem("EmberKeepers/场景/一键配置游戏场景")]
        public static void ShowWindow()
        {
            GetWindow<SceneSetupTool>("场景配置工具");
        }

        private void OnGUI()
        {
            GUILayout.Label("场景一键配置工具", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "此工具将自动配置当前场景，包括：\n" +
                "• 所有核心管理器\n" +
                "• UI系统（Canvas、EventSystem）\n" +
                "• 基地核心\n" +
                "• 摄像机\n" +
                "• 怪物生成点\n" +
                "• 场景标签和层级\n\n" +
                "注意：此操作会修改当前场景，建议先保存场景。",
                MessageType.Info);

            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // 基础配置
            EditorGUILayout.LabelField("基础配置", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 高级选项
            showAdvancedOptions = EditorGUILayout.Foldout(showAdvancedOptions, "高级选项");
            if (showAdvancedOptions)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("高级选项允许自定义配置参数", MessageType.None);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            // 配置按钮
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            if (GUILayout.Button("一键配置场景", GUILayout.Height(40)))
            {
                if (EditorUtility.DisplayDialog("确认配置", 
                    "此操作将配置当前场景。是否继续？", 
                    "确定", "取消"))
                {
                    SetupScene();
                }
            }
            EditorGUI.EndDisabledGroup();

            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("请在非运行模式下使用此工具", MessageType.Warning);
            }
        }

        private void SetupScene()
        {
            // 记录操作，支持撤销
            Undo.SetCurrentGroupName("配置游戏场景");
            int group = Undo.GetCurrentGroup();

            try
            {
                // 1. 配置场景基础设置
                SetupSceneSettings();

                // 2. 创建摄像机
                SetupCamera();

                // 3. 创建基地核心
                GameObject baseCore = SetupBaseCore();

                // 4. 创建所有管理器
                SetupManagers(baseCore);

                // 5. 创建UI系统
                SetupUISystem();

                // 6. 创建怪物生成点
                SetupSpawnPoints(baseCore);

                // 7. 创建英雄部署区域
                SetupDeploymentArea(baseCore);

                // 8. 配置光照
                SetupLighting();

                Undo.CollapseUndoOperations(group);
                EditorUtility.DisplayDialog("配置完成", 
                    "场景配置完成！\n\n" +
                    "已自动配置：\n" +
                    "• 所有核心管理器（已自动创建）\n" +
                    "• LocalizationData资源（已自动查找或创建）\n" +
                    "• UI系统（所有面板引用已自动连接）\n" +
                    "• 基地核心\n" +
                    "• 摄像机\n" +
                    "• 怪物生成点\n\n" +
                    "注意：如果使用GameInitializer，请确保预制体已分配。\n" +
                    "但通常不需要GameInitializer，因为管理器已在场景中创建。\n\n" +
                    "场景已可以运行！", 
                    "确定");

                Debug.Log("场景配置完成！");
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("配置失败", 
                    $"场景配置过程中出现错误：\n{e.Message}", 
                    "确定");
                Debug.LogError($"场景配置失败: {e}");
            }
        }

        private void SetupSceneSettings()
        {
            // 确保场景有必要的标签
            if (!TagExists("BaseCore"))
            {
                CreateTag("BaseCore");
            }

            if (!TagExists("Hero"))
            {
                CreateTag("Hero");
            }

            if (!TagExists("Monster"))
            {
                CreateTag("Monster");
            }
        }

        private void SetupCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraObj = new GameObject("Main Camera");
                mainCamera = cameraObj.AddComponent<Camera>();
                mainCamera.tag = "MainCamera";
                Undo.RegisterCreatedObjectUndo(cameraObj, "创建主摄像机");
            }

            // 配置摄像机位置和参数
            mainCamera.transform.position = new Vector3(0, 10, -10);
            mainCamera.transform.rotation = Quaternion.Euler(45, 0, 0);
            mainCamera.orthographic = false;
            mainCamera.fieldOfView = 60;
            mainCamera.nearClipPlane = 0.3f;
            mainCamera.farClipPlane = 100f;

            Undo.RecordObject(mainCamera, "配置摄像机");
        }

        private GameObject SetupBaseCore()
        {
            // 查找是否已存在基地核心
            GameObject existingCore = GameObject.FindGameObjectWithTag("BaseCore");
            if (existingCore != null)
            {
                return existingCore;
            }

            // 创建基地核心
            GameObject baseCoreObj = new GameObject("BaseCore");
            baseCoreObj.tag = "BaseCore";
            baseCoreObj.transform.position = Vector3.zero;

            // 添加BaseCore组件
            BaseCore baseCore = baseCoreObj.AddComponent<BaseCore>();

            // 添加可视化（简单的立方体）
            GameObject visualObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visualObj.name = "CoreVisual";
            visualObj.transform.SetParent(baseCoreObj.transform);
            visualObj.transform.localPosition = Vector3.zero;
            visualObj.transform.localScale = new Vector3(2, 1, 2);
            
            // 移除碰撞器（如果需要）
            Collider col = visualObj.GetComponent<Collider>();
            if (col != null)
            {
                DestroyImmediate(col);
            }

            Undo.RegisterCreatedObjectUndo(baseCoreObj, "创建基地核心");

            return baseCoreObj;
        }

        private void SetupManagers(GameObject baseCore)
        {
            // 创建管理器容器
            GameObject managersObj = GameObject.Find("Managers");
            if (managersObj == null)
            {
                managersObj = new GameObject("Managers");
                Undo.RegisterCreatedObjectUndo(managersObj, "创建管理器容器");
            }

            // 1. GameManager
            CreateManagerIfNotExists<GameManager>(managersObj, "GameManager");

            // 2. DataManager
            CreateManagerIfNotExists<DataManager>(managersObj, "DataManager");

            // 3. MetaProgressionManager
            CreateManagerIfNotExists<MetaProgressionManager>(managersObj, "MetaProgressionManager");

            // 4. LocalizationManager
            GameObject localizationManagerObj = CreateManagerIfNotExists<LocalizationManager>(managersObj, "LocalizationManager");
            if (localizationManagerObj != null)
            {
                LocalizationManager localizationManager = localizationManagerObj.GetComponent<LocalizationManager>();
                SetupLocalizationManager(localizationManager);
            }

            // 5. ResourceManager
            CreateManagerIfNotExists<ResourceManager>(managersObj, "ResourceManager");

            // 6. WaveManager
            CreateManagerIfNotExists<WaveManager>(managersObj, "WaveManager");

            // 7. HeroManager
            CreateManagerIfNotExists<HeroManager>(managersObj, "HeroManager");

            // 8. MonsterSpawnManager
            GameObject spawnManagerObj = CreateManagerIfNotExists<MonsterSpawnManager>(managersObj, "MonsterSpawnManager");
            if (spawnManagerObj != null)
            {
                MonsterSpawnManager spawnManager = spawnManagerObj.GetComponent<MonsterSpawnManager>();
                // 通过反射设置baseCore引用
                SetPrivateField(spawnManager, "baseCore", baseCore.transform);
            }

            // 9. CombatSystem
            CreateManagerIfNotExists<CombatSystem>(managersObj, "CombatSystem");

            // 10. ShopSystem
            CreateManagerIfNotExists<ShopSystem>(managersObj, "ShopSystem");

            // 11. DeploymentSystem
            CreateManagerIfNotExists<DeploymentSystem>(managersObj, "DeploymentSystem");

            // 12. ReviveSystem
            CreateManagerIfNotExists<EmberKeepers.Heroes.ReviveSystem>(managersObj, "ReviveSystem");

            // 13. ObjectPool
            CreateManagerIfNotExists<ObjectPool>(managersObj, "ObjectPool");

            // 14. AudioManager
            CreateManagerIfNotExists<AudioManager>(managersObj, "AudioManager");

            // 15. UIManager（稍后在UI系统中创建）
        }

        private GameObject CreateManagerIfNotExists<T>(GameObject parent, string name) where T : MonoBehaviour
        {
            T existing = FindFirstObjectByType<T>();
            if (existing != null)
            {
                return existing.gameObject;
            }

            GameObject managerObj = new GameObject(name);
            managerObj.transform.SetParent(parent.transform);
            T component = managerObj.AddComponent<T>();

            // 如果是单例，设置DontDestroyOnLoad
            if (typeof(T).GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static) != null)
            {
                // 这些管理器通常会在Awake中设置Instance
            }

            Undo.RegisterCreatedObjectUndo(managerObj, $"创建{name}");

            return managerObj;
        }

        private void SetupUISystem()
        {
            // 创建Canvas
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                Undo.RegisterCreatedObjectUndo(canvasObj, "创建Canvas");
            }

            // 创建EventSystem
            EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystem = eventSystemObj.AddComponent<EventSystem>();
                eventSystemObj.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystemObj, "创建EventSystem");
            }

            // 创建UIManager
            UIManager uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager == null)
            {
                GameObject uiManagerObj = new GameObject("UIManager");
                uiManager = uiManagerObj.AddComponent<UIManager>();
                uiManagerObj.transform.SetParent(canvas.transform);

                Undo.RegisterCreatedObjectUndo(uiManagerObj, "创建UIManager");
            }

            // 创建UI根容器
            GameObject uiRoot = GameObject.Find("UI Root");
            if (uiRoot == null)
            {
                uiRoot = new GameObject("UI Root");
                uiRoot.transform.SetParent(canvas.transform, false);
                
                RectTransform rect = uiRoot.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.sizeDelta = Vector2.zero;
                rect.anchoredPosition = Vector2.zero;

                Undo.RegisterCreatedObjectUndo(uiRoot, "创建UI根容器");
            }

            // 创建主菜单UI容器（默认隐藏，通过逻辑加载）
            CreateUIPanel(uiRoot, "MainMenuPanel", false);
            
            // 创建战斗UI容器
            GameObject combatPanel = CreateUIPanel(uiRoot, "CombatPanel", true);
            
            // 创建顶部信息栏
            GameObject topPanel = CreateUIPanel(combatPanel, "TopInfoPanel", true);
            SetupTopInfoPanel(topPanel);

            // 创建底部信息栏
            GameObject bottomPanel = CreateUIPanel(combatPanel, "BottomInfoPanel", true);
            SetupBottomInfoPanel(bottomPanel);

            // 创建策略面板（默认隐藏）
            GameObject strategyPanel = CreateUIPanel(uiRoot, "StrategyPanel", false);

            // 创建商店面板（默认隐藏）
            GameObject shopPanel = CreateUIPanel(strategyPanel, "ShopPanel", true);

            // 连接UIManager的引用
            ConnectUIManagerReferences(uiManager, topPanel, bottomPanel, combatPanel, strategyPanel, shopPanel);
        }

        private GameObject CreateUIPanel(GameObject parent, string name, bool active)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent.transform, false);
            panel.SetActive(active);

            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;

            Undo.RegisterCreatedObjectUndo(panel, $"创建{name}");

            return panel;
        }

        private void SetupTopInfoPanel(GameObject panel)
        {
            // 添加TopInfoPanel组件
            TopInfoPanel topInfo = panel.AddComponent<TopInfoPanel>();

            // 创建资源显示区域
            GameObject resourceArea = new GameObject("ResourceArea");
            resourceArea.transform.SetParent(panel.transform, false);
            RectTransform resourceRect = resourceArea.AddComponent<RectTransform>();
            resourceRect.anchorMin = new Vector2(0.7f, 0.9f);
            resourceRect.anchorMax = new Vector2(1f, 1f);
            resourceRect.sizeDelta = Vector2.zero;
            resourceRect.anchoredPosition = Vector2.zero;

            // 创建波次信息区域
            GameObject waveArea = new GameObject("WaveArea");
            waveArea.transform.SetParent(panel.transform, false);
            RectTransform waveRect = waveArea.AddComponent<RectTransform>();
            waveRect.anchorMin = new Vector2(0.4f, 0.9f);
            waveRect.anchorMax = new Vector2(0.6f, 1f);
            waveRect.sizeDelta = Vector2.zero;
            waveRect.anchoredPosition = Vector2.zero;

            // 创建基地核心生命值区域
            GameObject coreArea = new GameObject("CoreArea");
            coreArea.transform.SetParent(panel.transform, false);
            RectTransform coreRect = coreArea.AddComponent<RectTransform>();
            coreRect.anchorMin = new Vector2(0f, 0.9f);
            coreRect.anchorMax = new Vector2(0.3f, 1f);
            coreRect.sizeDelta = Vector2.zero;
            coreRect.anchoredPosition = Vector2.zero;

            // 通过反射连接引用（UI元素会在运行时通过逻辑加载）
            Undo.RegisterCreatedObjectUndo(resourceArea, "创建资源显示区域");
            Undo.RegisterCreatedObjectUndo(waveArea, "创建波次信息区域");
            Undo.RegisterCreatedObjectUndo(coreArea, "创建基地核心区域");
        }

        private void SetupBottomInfoPanel(GameObject panel)
        {
            // 添加BottomInfoPanel组件
            BottomInfoPanel bottomInfo = panel.AddComponent<BottomInfoPanel>();

            // 创建默认面板容器
            GameObject defaultPanel = new GameObject("DefaultPanel");
            defaultPanel.transform.SetParent(panel.transform, false);
            RectTransform defaultRect = defaultPanel.AddComponent<RectTransform>();
            defaultRect.anchorMin = new Vector2(0, 0);
            defaultRect.anchorMax = new Vector2(1, 0.25f);
            defaultRect.sizeDelta = Vector2.zero;
            defaultRect.anchoredPosition = Vector2.zero;

            // 创建英雄详情面板容器（默认隐藏）
            GameObject heroDetailPanel = new GameObject("HeroDetailPanel");
            heroDetailPanel.transform.SetParent(panel.transform, false);
            heroDetailPanel.SetActive(false);
            RectTransform heroRect = heroDetailPanel.AddComponent<RectTransform>();
            heroRect.anchorMin = new Vector2(0, 0);
            heroRect.anchorMax = new Vector2(1, 0.25f);
            heroRect.sizeDelta = Vector2.zero;
            heroRect.anchoredPosition = Vector2.zero;

            // 创建基地核心面板容器（默认隐藏）
            GameObject baseCorePanel = new GameObject("BaseCorePanel");
            baseCorePanel.transform.SetParent(panel.transform, false);
            baseCorePanel.SetActive(false);
            RectTransform coreRect = baseCorePanel.AddComponent<RectTransform>();
            coreRect.anchorMin = new Vector2(0, 0);
            coreRect.anchorMax = new Vector2(1, 0.25f);
            coreRect.sizeDelta = Vector2.zero;
            coreRect.anchoredPosition = Vector2.zero;

            Undo.RegisterCreatedObjectUndo(defaultPanel, "创建默认面板");
            Undo.RegisterCreatedObjectUndo(heroDetailPanel, "创建英雄详情面板");
            Undo.RegisterCreatedObjectUndo(baseCorePanel, "创建基地核心面板");
        }

        private void ConnectUIManagerReferences(UIManager uiManager, GameObject topPanel, GameObject bottomPanel, 
            GameObject combatPanel, GameObject strategyPanel, GameObject shopPanel)
        {
            // 查找主菜单面板（在UI Root下）
            GameObject mainMenuPanel = null;
            Transform uiRoot = GameObject.Find("UI Root")?.transform;
            if (uiRoot != null)
            {
                Transform mainMenuTransform = uiRoot.Find("MainMenuPanel");
                if (mainMenuTransform != null)
                {
                    mainMenuPanel = mainMenuTransform.gameObject;
                }
            }
            
            // 如果找不到，尝试直接查找
            if (mainMenuPanel == null)
            {
                GameObject found = GameObject.Find("MainMenuPanel");
                if (found != null)
                {
                    mainMenuPanel = found;
                }
            }
            
            // 通过反射设置UIManager的字段
            SetPrivateField(uiManager, "mainMenuPanel", mainMenuPanel);
            SetPrivateField(uiManager, "topInfoPanel", topPanel);
            SetPrivateField(uiManager, "bottomInfoPanel", bottomPanel);
            SetPrivateField(uiManager, "combatPanel", combatPanel);
            SetPrivateField(uiManager, "strategyPanel", strategyPanel);
            SetPrivateField(uiManager, "shopPanel", shopPanel);

            Undo.RecordObject(uiManager, "连接UIManager引用");
            
            if (mainMenuPanel == null)
            {
                Debug.LogWarning("SceneSetupTool: 未找到MainMenuPanel，请确保主菜单面板已创建");
            }
        }

        private void SetupSpawnPoints(GameObject baseCore)
        {
            // 查找是否已存在生成点
            GameObject spawnPointsObj = GameObject.Find("SpawnPoints");
            if (spawnPointsObj != null)
            {
                return;
            }

            spawnPointsObj = new GameObject("SpawnPoints");
            spawnPointsObj.transform.position = baseCore.transform.position;

            // 创建8个生成点（围绕基地核心）
            float spawnRadius = 20f;
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * Mathf.Deg2Rad;
                Vector3 position = new Vector3(
                    Mathf.Cos(angle) * spawnRadius,
                    0f,
                    Mathf.Sin(angle) * spawnRadius
                );

                GameObject spawnPoint = new GameObject($"SpawnPoint_{i}");
                spawnPoint.transform.SetParent(spawnPointsObj.transform);
                spawnPoint.transform.position = position;

                // 添加可视化标记（可选）
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.name = "Marker";
                marker.transform.SetParent(spawnPoint.transform);
                marker.transform.localPosition = Vector3.zero;
                marker.transform.localScale = Vector3.one * 0.5f;
                
                // 移除碰撞器
                Collider col = marker.GetComponent<Collider>();
                if (col != null)
                {
                    DestroyImmediate(col);
                }

                Undo.RegisterCreatedObjectUndo(spawnPoint, $"创建生成点{i}");
            }

            Undo.RegisterCreatedObjectUndo(spawnPointsObj, "创建生成点容器");
        }

        private void SetupDeploymentArea(GameObject baseCore)
        {
            // 创建英雄部署区域
            GameObject deploymentArea = GameObject.Find("DeploymentArea");
            if (deploymentArea != null)
            {
                return;
            }

            deploymentArea = new GameObject("DeploymentArea");
            deploymentArea.transform.position = baseCore.transform.position + Vector3.back * 5f;

            // 添加可视化（可选）
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Plane);
            visual.name = "DeploymentVisual";
            visual.transform.SetParent(deploymentArea.transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localScale = Vector3.one * 2f;
            visual.transform.rotation = Quaternion.Euler(0, 0, 0);

            // 设置材质颜色（可选）
            Renderer renderer = visual.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(0, 1, 0, 0.3f);
                renderer.material = mat;
            }

            Undo.RegisterCreatedObjectUndo(deploymentArea, "创建部署区域");
        }

        private void SetupLighting()
        {
            // 查找主光源
            Light mainLight = FindFirstObjectByType<Light>();
            if (mainLight == null)
            {
                GameObject lightObj = new GameObject("Directional Light");
                mainLight = lightObj.AddComponent<Light>();
                mainLight.type = LightType.Directional;
                mainLight.transform.rotation = Quaternion.Euler(50, -30, 0);
                mainLight.intensity = 1f;
                mainLight.shadows = LightShadows.Soft;

                Undo.RegisterCreatedObjectUndo(lightObj, "创建主光源");
            }

            // 设置环境光
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.5f, 0.5f, 0.6f);
            RenderSettings.ambientEquatorColor = new Color(0.4f, 0.4f, 0.4f);
            RenderSettings.ambientGroundColor = new Color(0.3f, 0.3f, 0.3f);

            // 注意：RenderSettings不能直接Undo，但这是场景设置，通常不需要撤销
        }

        // 辅助方法
        private bool TagExists(string tag)
        {
            try
            {
                GameObject.FindGameObjectWithTag(tag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreateTag(string tag)
        {
            // Unity Editor API创建标签
            SerializedObject tagManager = new SerializedObject(
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag))
                {
                    return; // 标签已存在
                }
            }

            tagsProp.InsertArrayElementAtIndex(0);
            tagsProp.GetArrayElementAtIndex(0).stringValue = tag;
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// 设置LocalizationManager的LocalizationData
        /// </summary>
        private void SetupLocalizationManager(LocalizationManager manager)
        {
            if (manager == null) return;

            // 尝试查找现有的LocalizationData资源
            string[] guids = AssetDatabase.FindAssets("t:LocalizationData");
            LocalizationData localizationData = null;

            if (guids.Length > 0)
            {
                // 使用找到的第一个LocalizationData
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(path);
            }

            // 如果没有找到，创建一个新的
            if (localizationData == null)
            {
                string defaultPath = "Assets/Resources/LocalizationData.asset";
                string directory = System.IO.Path.GetDirectoryName(defaultPath);
                if (!AssetDatabase.IsValidFolder(directory))
                {
                    // 确保Resources文件夹存在
                    string resourcesPath = "Assets/Resources";
                    if (!AssetDatabase.IsValidFolder(resourcesPath))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }
                }

                localizationData = ScriptableObject.CreateInstance<LocalizationData>();
                AssetDatabase.CreateAsset(localizationData, defaultPath);
                AssetDatabase.SaveAssets();
                Debug.Log($"创建了新的LocalizationData资源: {defaultPath}");
            }

            // 通过反射设置LocalizationData
            SetPrivateField(manager, "localizationData", localizationData);
            Undo.RecordObject(manager, "设置LocalizationData");
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            System.Reflection.FieldInfo field = obj.GetType().GetField(
                fieldName, 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Public);
            
            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                Debug.LogWarning($"无法找到字段: {fieldName} in {obj.GetType().Name}");
            }
        }
    }
}

