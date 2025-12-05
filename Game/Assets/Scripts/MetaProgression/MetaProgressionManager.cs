using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.MetaProgression
{
    /// <summary>
    /// 元进度管理器，管理局外永久升级
    /// </summary>
    public class MetaProgressionManager : MonoBehaviour
    {
        public static MetaProgressionManager Instance { get; private set; }

        [Header("Meta Progression Data")]
        [SerializeField] public MetaProgressionData progressionData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadProgressionData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadProgressionData()
        {
            // 从本地存储加载
            if (progressionData == null)
            {
                progressionData = Utils.SaveSystem.LoadMetaProgression();
            }
        }

        private void SaveProgressionData()
        {
            // 保存到本地存储
            if (progressionData != null)
            {
                Utils.SaveSystem.SaveMetaProgression(progressionData);
            }
        }

        /// <summary>
        /// 获取初始金币加成
        /// </summary>
        public int GetInitialGoldBonus()
        {
            return progressionData.initialGoldUpgradeLevel * 20; // 每级+20金币
        }

        /// <summary>
        /// 获取英雄初始生命值加成
        /// </summary>
        public float GetHeroHealthBonus()
        {
            return progressionData.heroBloodlineUpgradeLevel * 10f; // 每级+10生命值
        }

        /// <summary>
        /// 获取商店稀有装备概率加成
        /// </summary>
        public float GetShopRareEquipmentChanceBonus()
        {
            return progressionData.shopOptimizationUpgradeLevel * 0.05f; // 每级+5%
        }

        /// <summary>
        /// 获取星火精粹获取效率加成
        /// </summary>
        public float GetEssenceEfficiencyBonus()
        {
            return 1f + progressionData.essenceEfficiencyUpgradeLevel * 0.1f; // 每级+10%
        }

        /// <summary>
        /// 升级英雄血脉强化
        /// </summary>
        public bool UpgradeHeroBloodline(int cost)
        {
            if (progressionData.starfireEssence < cost)
                return false;

            progressionData.starfireEssence -= cost;
            progressionData.heroBloodlineUpgradeLevel++;
            SaveProgressionData();
            return true;
        }

        /// <summary>
        /// 升级初始金币储备
        /// </summary>
        public bool UpgradeInitialGold(int cost)
        {
            if (progressionData.starfireEssence < cost)
                return false;

            progressionData.starfireEssence -= cost;
            progressionData.initialGoldUpgradeLevel++;
            SaveProgressionData();
            return true;
        }

        /// <summary>
        /// 升级商店优化
        /// </summary>
        public bool UpgradeShopOptimization(int cost)
        {
            if (progressionData.starfireEssence < cost)
                return false;

            progressionData.starfireEssence -= cost;
            progressionData.shopOptimizationUpgradeLevel++;
            SaveProgressionData();
            return true;
        }

        /// <summary>
        /// 升级精粹获取效率
        /// </summary>
        public bool UpgradeEssenceEfficiency(int cost)
        {
            if (progressionData.starfireEssence < cost)
                return false;

            progressionData.starfireEssence -= cost;
            progressionData.essenceEfficiencyUpgradeLevel++;
            SaveProgressionData();
            return true;
        }

        /// <summary>
        /// 添加星火精粹
        /// </summary>
        public void AddStarfireEssence(int amount)
        {
            progressionData.starfireEssence += amount;
            SaveProgressionData();
        }

        public int StarfireEssence => progressionData.starfireEssence;
    }

    [CreateAssetMenu(fileName = "MetaProgressionData", menuName = "EmberKeepers/MetaProgression/MetaProgressionData")]
    public class MetaProgressionData : ScriptableObject
    {
        [Header("Resources")]
        public int starfireEssence = 0;

        [Header("Upgrade Levels")]
        public int heroBloodlineUpgradeLevel = 0;
        public int initialGoldUpgradeLevel = 0;
        public int shopOptimizationUpgradeLevel = 0;
        public int essenceEfficiencyUpgradeLevel = 0;

        [Header("Unlocked Content")]
        public List<string> unlockedHeroIds = new List<string>();
        public List<string> unlockedEquipmentIds = new List<string>();
        public List<string> unlockedBaseUpgrades = new List<string>();
    }
}

