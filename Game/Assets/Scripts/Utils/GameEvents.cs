using System;
using EmberKeepers.Heroes;
using EmberKeepers.Monsters;

namespace EmberKeepers.Utils
{
    /// <summary>
    /// 游戏事件系统 - 使用静态事件解耦系统间通信
    /// </summary>
    public static class GameEvents
    {
        // 游戏状态事件
        public static event Action OnGameStarted;
        public static event Action OnGamePaused;
        public static event Action OnGameResumed;
        public static event Action OnGameOver;

        // 波次事件
        public static event Action<int> OnWaveStarted;
        public static event Action<int> OnWaveCompleted;
        public static event Action OnAllWavesCompleted;

        // 英雄事件
        public static event Action<HeroBase> OnHeroSpawned;
        public static event Action<HeroBase> OnHeroDeployed;
        public static event Action<HeroBase> OnHeroDied;
        public static event Action<HeroBase> OnHeroRevived;
        public static event Action<HeroBase, int> OnHeroLevelUp;

        // 怪物事件
        public static event Action<MonsterBase> OnMonsterSpawned;
        public static event Action<MonsterBase> OnMonsterDied;
        public static event Action<MonsterBase> OnBossSpawned;

        // 资源事件
        public static event Action<int> OnGoldChanged;
        public static event Action<int> OnStardustChanged;
        public static event Action<int> OnEssenceChanged;

        // 购买事件
        public static event Action<string> OnHeroPurchased;
        public static event Action<string> OnEquipmentPurchased;
        public static event Action<AttributeType> OnAttributePurchased;

        // 触发方法
        public static void TriggerGameStarted() => OnGameStarted?.Invoke();
        public static void TriggerGamePaused() => OnGamePaused?.Invoke();
        public static void TriggerGameResumed() => OnGameResumed?.Invoke();
        public static void TriggerGameOver() => OnGameOver?.Invoke();

        public static void TriggerWaveStarted(int wave) => OnWaveStarted?.Invoke(wave);
        public static void TriggerWaveCompleted(int wave) => OnWaveCompleted?.Invoke(wave);
        public static void TriggerAllWavesCompleted() => OnAllWavesCompleted?.Invoke();

        public static void TriggerHeroSpawned(HeroBase hero) => OnHeroSpawned?.Invoke(hero);
        public static void TriggerHeroDeployed(HeroBase hero) => OnHeroDeployed?.Invoke(hero);
        public static void TriggerHeroDied(HeroBase hero) => OnHeroDied?.Invoke(hero);
        public static void TriggerHeroRevived(HeroBase hero) => OnHeroRevived?.Invoke(hero);
        public static void TriggerHeroLevelUp(HeroBase hero, int level) => OnHeroLevelUp?.Invoke(hero, level);

        public static void TriggerMonsterSpawned(MonsterBase monster) => OnMonsterSpawned?.Invoke(monster);
        public static void TriggerMonsterDied(MonsterBase monster) => OnMonsterDied?.Invoke(monster);
        public static void TriggerBossSpawned(MonsterBase boss) => OnBossSpawned?.Invoke(boss);

        public static void TriggerGoldChanged(int amount) => OnGoldChanged?.Invoke(amount);
        public static void TriggerStardustChanged(int amount) => OnStardustChanged?.Invoke(amount);
        public static void TriggerEssenceChanged(int amount) => OnEssenceChanged?.Invoke(amount);

        public static void TriggerHeroPurchased(string heroId) => OnHeroPurchased?.Invoke(heroId);
        public static void TriggerEquipmentPurchased(string equipId) => OnEquipmentPurchased?.Invoke(equipId);
        public static void TriggerAttributePurchased(AttributeType type) => OnAttributePurchased?.Invoke(type);

        /// <summary>
        /// 清除所有事件订阅
        /// </summary>
        public static void ClearAllEvents()
        {
            OnGameStarted = null;
            OnGamePaused = null;
            OnGameResumed = null;
            OnGameOver = null;
            OnWaveStarted = null;
            OnWaveCompleted = null;
            OnAllWavesCompleted = null;
            OnHeroSpawned = null;
            OnHeroDeployed = null;
            OnHeroDied = null;
            OnHeroRevived = null;
            OnHeroLevelUp = null;
            OnMonsterSpawned = null;
            OnMonsterDied = null;
            OnBossSpawned = null;
            OnGoldChanged = null;
            OnStardustChanged = null;
            OnEssenceChanged = null;
            OnHeroPurchased = null;
            OnEquipmentPurchased = null;
            OnAttributePurchased = null;
        }
    }
}

