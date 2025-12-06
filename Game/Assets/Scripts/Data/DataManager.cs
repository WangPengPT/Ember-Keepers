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
        /// 获取所有英雄数据
        /// </summary>
        public List<HeroData> GetAllHeroes()
        {
            return new List<HeroData>(heroDataDict.Values);
        }

        /// <summary>
        /// 根据元素类型获取英雄
        /// </summary>
        public List<HeroData> GetHeroesByElement(ElementType elementType)
        {
            List<HeroData> result = new List<HeroData>();
            foreach (var hero in heroDataDict.Values)
            {
                if (hero.elementType == elementType)
                    result.Add(hero);
            }
            return result;
        }

        /// <summary>
        /// 根据职业获取英雄
        /// </summary>
        public List<HeroData> GetHeroesByClass(HeroClass heroClass)
        {
            List<HeroData> result = new List<HeroData>();
            foreach (var hero in heroDataDict.Values)
            {
                if (hero.heroClass == heroClass)
                    result.Add(hero);
            }
            return result;
        }

        /// <summary>
        /// 获取所有装备数据
        /// </summary>
        public List<EquipmentData> GetAllEquipment()
        {
            return new List<EquipmentData>(equipmentDataDict.Values);
        }

        /// <summary>
        /// 根据装备类型获取装备
        /// </summary>
        public List<EquipmentData> GetEquipmentByType(EquipmentType equipmentType)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            foreach (var eq in equipmentDataDict.Values)
            {
                if (eq.equipmentType == equipmentType)
                    result.Add(eq);
            }
            return result;
        }

        /// <summary>
        /// 根据稀有度获取装备
        /// </summary>
        public List<EquipmentData> GetEquipmentByRarity(Rarity rarity)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            foreach (var eq in equipmentDataDict.Values)
            {
                if (eq.rarity == rarity)
                    result.Add(eq);
            }
            return result;
        }

        /// <summary>
        /// 根据职业获取可用装备
        /// </summary>
        public List<EquipmentData> GetEquipmentForClass(HeroClass heroClass)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            foreach (var eq in equipmentDataDict.Values)
            {
                // -1表示全职业
                if (eq.requiredClass == (HeroClass)(-1) || eq.requiredClass == heroClass)
                    result.Add(eq);
            }
            return result;
        }

        /// <summary>
        /// 获取所有怪物数据
        /// </summary>
        public List<MonsterData> GetAllMonsters()
        {
            return new List<MonsterData>(monsterDataDict.Values);
        }

        /// <summary>
        /// 获取所有普通怪物
        /// </summary>
        public List<MonsterData> GetNormalMonsters()
        {
            List<MonsterData> result = new List<MonsterData>();
            foreach (var monster in monsterDataDict.Values)
            {
                if (!monster.isElite && !monster.isBoss)
                    result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// 获取所有精英怪
        /// </summary>
        public List<MonsterData> GetEliteMonsters()
        {
            List<MonsterData> result = new List<MonsterData>();
            foreach (var monster in monsterDataDict.Values)
            {
                if (monster.isElite)
                    result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// 获取所有Boss
        /// </summary>
        public List<MonsterData> GetBossMonsters()
        {
            List<MonsterData> result = new List<MonsterData>();
            foreach (var monster in monsterDataDict.Values)
            {
                if (monster.isBoss)
                    result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// 获取所有波次数据
        /// </summary>
        public List<WaveDataEntry> GetAllWaveData()
        {
            List<WaveDataEntry> result = new List<WaveDataEntry>();
            foreach (var asset in waveDataAssets)
            {
                result.AddRange(asset.waveEntries);
            }
            return result;
        }

        /// <summary>
        /// 检查数据是否已加载
        /// </summary>
        public bool IsDataLoaded()
        {
            return heroDataDict.Count > 0 && equipmentDataDict.Count > 0 && 
                   monsterDataDict.Count > 0 && waveDataAssets.Count > 0;
        }

        /// <summary>
        /// 从Excel导入数据（Editor工具）
        /// </summary>
        [ContextMenu("Import From Excel")]
        public void ImportFromExcel()
        {
            Debug.Log("请使用菜单 EmberKeepers/数据导入工具 来导入数据");
        }
    }
}

