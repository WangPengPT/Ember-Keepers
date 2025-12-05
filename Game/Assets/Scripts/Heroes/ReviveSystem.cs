using UnityEngine;
using EmberKeepers.Core;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 复活系统，处理英雄阵亡和复活机制
    /// </summary>
    public class ReviveSystem : MonoBehaviour
    {
        [Header("Revive Settings")]
        [SerializeField] private int baseReviveCost = 100;
        [SerializeField] private float reviveCostMultiplier = 1.5f;
        [SerializeField] private float freeReviveWaveDelay = 2f; // 等待2个波次后免费复活

        private ResourceManager resourceManager;
        private WaveManager waveManager;

        private void Awake()
        {
            resourceManager = FindFirstObjectByType<ResourceManager>();
            waveManager = FindFirstObjectByType<WaveManager>();
        }

        /// <summary>
        /// 付费即时复活
        /// </summary>
        public bool ReviveHeroImmediate(HeroBase hero, bool useStardust = false)
        {
            if (hero == null || !hero.IsDead)
                return false;

            int cost = CalculateReviveCost(hero);
            
            // 检查资源
            bool hasResources = false;
            if (useStardust)
            {
                hasResources = resourceManager != null && resourceManager.SpendStardustShards(cost);
            }
            else
            {
                hasResources = resourceManager != null && resourceManager.SpendGold(cost);
            }

            if (!hasResources)
                return false;

            // 50%生命值复活
            hero.Revive(0.5f);
            
            // 增加下次复活费用
            IncreaseReviveCost(hero);
            
            OnHeroRevived?.Invoke(hero, true);
            return true;
        }

        /// <summary>
        /// 缓慢等待复活（免费）
        /// </summary>
        public void QueueFreeRevive(HeroBase hero)
        {
            if (hero == null || !hero.IsDead)
                return;

            // 记录死亡时的波次
            int deathWave = waveManager != null ? waveManager.CurrentWave : 0;
            hero.deathTimer = deathWave + freeReviveWaveDelay;

            OnHeroQueuedForRevive?.Invoke(hero);
        }

        /// <summary>
        /// 检查并执行免费复活
        /// </summary>
        public void CheckFreeRevives()
        {
            if (waveManager == null) return;

            int currentWave = waveManager.CurrentWave;
            var allHeroes = FindObjectsOfType<HeroBase>();

            foreach (var hero in allHeroes)
            {
                if (hero.IsDead && hero.deathTimer > 0 && currentWave >= hero.deathTimer)
                {
                    // 满生命值复活
                    hero.Revive(1f);
                    hero.deathTimer = 0f;
                    OnHeroRevived?.Invoke(hero, false);
                }
            }
        }

        /// <summary>
        /// 计算复活费用
        /// </summary>
        private int CalculateReviveCost(HeroBase hero)
        {
            // 从英雄数据获取复活次数，费用递增
            int reviveCount = GetReviveCount(hero);
            return Mathf.RoundToInt(baseReviveCost * Mathf.Pow(reviveCostMultiplier, reviveCount));
        }

        /// <summary>
        /// 获取英雄复活次数
        /// </summary>
        private int GetReviveCount(HeroBase hero)
        {
            // TODO: 从英雄数据或组件获取复活次数
            return 0;
        }

        /// <summary>
        /// 增加复活费用
        /// </summary>
        private void IncreaseReviveCost(HeroBase hero)
        {
            // TODO: 记录复活次数
        }

        public event System.Action<HeroBase, bool> OnHeroRevived; // bool表示是否付费
        public event System.Action<HeroBase> OnHeroQueuedForRevive;
    }
}

