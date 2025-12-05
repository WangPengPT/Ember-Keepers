using UnityEngine;
using System;

namespace EmberKeepers.Core
{
    /// <summary>
    /// 资源管理器，管理金币、星火碎片、星火精粹等资源
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        [Header("Current Resources")]
        [SerializeField] private int gold = 0;
        [SerializeField] private int stardustShards = 0;
        [SerializeField] private int starfireEssence = 0; // 局外资源

        [Header("Initial Resources")]
        [SerializeField] private int initialGold = 100;

        public int Gold => gold;
        public int StardustShards => stardustShards;
        public int StarfireEssence => starfireEssence;

        public event Action<int> OnGoldChanged;
        public event Action<int> OnStardustShardsChanged;
        public event Action<int> OnStarfireEssenceChanged;

        private void Awake()
        {
            // 从元进度系统加载初始资源
            LoadMetaProgressionBonuses();
        }

        public void InitializeResources()
        {
            gold = initialGold;
            // TODO: 应用元进度的初始金币加成
            OnGoldChanged?.Invoke(gold);
        }

        private void LoadMetaProgressionBonuses()
        {
            // TODO: 从元进度系统加载加成
            // initialGold += MetaProgressionManager.Instance.GetInitialGoldBonus();
        }

        /// <summary>
        /// 添加金币
        /// </summary>
        public bool AddGold(int amount)
        {
            if (amount < 0) return false;
            
            gold += amount;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        /// <summary>
        /// 消耗金币
        /// </summary>
        public bool SpendGold(int amount)
        {
            if (amount < 0 || gold < amount)
                return false;

            gold -= amount;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        /// <summary>
        /// 添加星火碎片
        /// </summary>
        public bool AddStardustShards(int amount)
        {
            if (amount < 0) return false;

            stardustShards += amount;
            OnStardustShardsChanged?.Invoke(stardustShards);
            return true;
        }

        /// <summary>
        /// 消耗星火碎片
        /// </summary>
        public bool SpendStardustShards(int amount)
        {
            if (amount < 0 || stardustShards < amount)
                return false;

            stardustShards -= amount;
            OnStardustShardsChanged?.Invoke(stardustShards);
            return true;
        }

        /// <summary>
        /// 添加星火精粹（局外资源）
        /// </summary>
        public bool AddStarfireEssence(int amount)
        {
            if (amount < 0) return false;

            starfireEssence += amount;
            OnStarfireEssenceChanged?.Invoke(starfireEssence);
            return true;
        }

        /// <summary>
        /// 消耗星火精粹
        /// </summary>
        public bool SpendStarfireEssence(int amount)
        {
            if (amount < 0 || starfireEssence < amount)
                return false;

            starfireEssence -= amount;
            OnStarfireEssenceChanged?.Invoke(starfireEssence);
            return true;
        }

        /// <summary>
        /// 检查是否有足够的资源
        /// </summary>
        public bool HasEnoughResources(int goldCost, int shardCost = 0)
        {
            return gold >= goldCost && stardustShards >= shardCost;
        }
    }
}

