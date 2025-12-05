using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 数据管理器，负责加载和管理所有游戏数据
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        [Header("Data Assets")]
        [SerializeField] private string dataPath = "Data";
        
        private Dictionary<string, HeroData> heroDataDict = new Dictionary<string, HeroData>();
        private Dictionary<string, EquipmentData> equipmentDataDict = new Dictionary<string, EquipmentData>();
        private Dictionary<string, MonsterData> monsterDataDict = new Dictionary<string, MonsterData>();
        private List<WaveDataAsset> waveDataAssets = new List<WaveDataAsset>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadAllData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 加载所有数据
        /// </summary>
        private void LoadAllData()
        {
            LoadHeroData();
            LoadEquipmentData();
            LoadMonsterData();
            LoadWaveData();
        }

        private void LoadHeroData()
        {
            HeroData[] heroes = Resources.LoadAll<HeroData>(Path.Combine(dataPath, "Heroes"));
            foreach (var hero in heroes)
            {
                heroDataDict[hero.heroId] = hero;
            }
            Debug.Log($"Loaded {heroDataDict.Count} hero data entries");
        }

        private void LoadEquipmentData()
        {
            EquipmentData[] equipment = Resources.LoadAll<EquipmentData>(Path.Combine(dataPath, "Equipment"));
            foreach (var eq in equipment)
            {
                equipmentDataDict[eq.equipmentId] = eq;
            }
            Debug.Log($"Loaded {equipmentDataDict.Count} equipment data entries");
        }

        private void LoadMonsterData()
        {
            MonsterData[] monsters = Resources.LoadAll<MonsterData>(Path.Combine(dataPath, "Monsters"));
            foreach (var monster in monsters)
            {
                monsterDataDict[monster.monsterId] = monster;
            }
            Debug.Log($"Loaded {monsterDataDict.Count} monster data entries");
        }

        private void LoadWaveData()
        {
            WaveDataAsset[] waves = Resources.LoadAll<WaveDataAsset>(Path.Combine(dataPath, "Waves"));
            waveDataAssets.AddRange(waves);
            Debug.Log($"Loaded {waveDataAssets.Count} wave data assets");
        }

        /// <summary>
        /// 获取英雄数据
        /// </summary>
        public HeroData GetHeroData(string heroId)
        {
            heroDataDict.TryGetValue(heroId, out HeroData data);
            return data;
        }

        /// <summary>
        /// 获取装备数据
        /// </summary>
        public EquipmentData GetEquipmentData(string equipmentId)
        {
            equipmentDataDict.TryGetValue(equipmentId, out EquipmentData data);
            return data;
        }

        /// <summary>
        /// 获取怪物数据
        /// </summary>
        public MonsterData GetMonsterData(string monsterId)
        {
            monsterDataDict.TryGetValue(monsterId, out MonsterData data);
            return data;
        }

        /// <summary>
        /// 获取波次数据
        /// </summary>
        public WaveDataEntry GetWaveData(int waveNumber)
        {
            foreach (var asset in waveDataAssets)
            {
                foreach (var entry in asset.waveEntries)
                {
                    if (entry.waveNumber == waveNumber)
                        return entry;
                }
            }
            return null;
        }

        /// <summary>
        /// 从Excel导入数据（Editor工具）
        /// </summary>
        [ContextMenu("Import From Excel")]
        public void ImportFromExcel()
        {
            // TODO: 实现Excel导入功能
            // 需要使用第三方库如EPPlus或ExcelDataReader
            Debug.Log("Excel import not implemented yet. Please use Editor tools.");
        }
    }
}

