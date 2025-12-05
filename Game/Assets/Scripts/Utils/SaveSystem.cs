using UnityEngine;
using System.IO;
using EmberKeepers.MetaProgression;

namespace EmberKeepers.Utils
{
    /// <summary>
    /// 存档系统 - 保存和加载元进度数据
    /// </summary>
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        /// <summary>
        /// 保存元进度数据
        /// </summary>
        public static void SaveMetaProgression(MetaProgressionData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
                Debug.Log($"Game saved to: {SavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }

        /// <summary>
        /// 加载元进度数据
        /// </summary>
        public static MetaProgressionData LoadMetaProgression()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    string json = File.ReadAllText(SavePath);
                    MetaProgressionData data = ScriptableObject.CreateInstance<MetaProgressionData>();
                    JsonUtility.FromJsonOverwrite(json, data);
                    Debug.Log("Game loaded successfully!");
                    return data;
                }
                else
                {
                    Debug.Log("No save file found, creating new data.");
                    return ScriptableObject.CreateInstance<MetaProgressionData>();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}");
                return ScriptableObject.CreateInstance<MetaProgressionData>();
            }
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        public static void DeleteSave()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                    Debug.Log("Save file deleted.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete save: {e.Message}");
            }
        }

        /// <summary>
        /// 检查是否存在存档
        /// </summary>
        public static bool HasSaveFile()
        {
            return File.Exists(SavePath);
        }
    }
}

