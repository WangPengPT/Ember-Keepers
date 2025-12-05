using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.Base
{
    /// <summary>
    /// 基地核心（星火核心）
    /// </summary>
    public class BaseCore : MonoBehaviour
    {
        [Header("Core Stats")]
        [SerializeField] private float maxHealth = 1000f;
        [SerializeField] private float currentHealth = 1000f;

        [Header("Upgrades")]
        [SerializeField] private int healthUpgradeLevel = 0;
        [SerializeField] private int energyUpgradeLevel = 0;
        [SerializeField] private int elementUpgradeLevel = 0;

        [Header("Active Abilities")]
        [SerializeField] private bool hasPurificationStrike = false;
        [SerializeField] private float purificationStrikeCooldown = 30f;
        [SerializeField] private float purificationStrikeTimer = 0f;
        [SerializeField] private int purificationStrikeUses = 0;
        [SerializeField] private int maxPurificationStrikeUses = 3;

        [Header("Defense Systems")]
        [SerializeField] public bool hasHealingDrone = false;
        [SerializeField] private GameObject healingDronePrefab;
        [SerializeField] private GameObject activeHealingDrone;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float HealthPercent => currentHealth / maxHealth;
        public bool IsDestroyed => currentHealth <= 0;

        public bool CanUsePurificationStrike => hasPurificationStrike && 
                                                purificationStrikeTimer <= 0f && 
                                                purificationStrikeUses < maxPurificationStrikeUses;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (purificationStrikeTimer > 0f)
            {
                purificationStrikeTimer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public void TakeDamage(float damage)
        {
            if (IsDestroyed) return;

            currentHealth -= damage;
            currentHealth = Mathf.Max(0f, currentHealth);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (IsDestroyed)
            {
                OnCoreDestroyed?.Invoke();
                // 游戏结束
                Core.GameManager.Instance?.GameOver();
            }
        }

        /// <summary>
        /// 治疗核心
        /// </summary>
        public void Heal(float amount)
        {
            if (IsDestroyed) return;

            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        /// <summary>
        /// 升级星火储备（增加生命值上限）
        /// </summary>
        public bool UpgradeHealth(int cost)
        {
            // TODO: 检查资源
            healthUpgradeLevel++;
            maxHealth += 100f;
            currentHealth += 100f; // 同时恢复生命值
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            return true;
        }

        /// <summary>
        /// 升级能量汲取（增加所有英雄能量恢复速度）
        /// </summary>
        public bool UpgradeEnergyRegen(int cost)
        {
            // TODO: 检查资源
            energyUpgradeLevel++;
            // TODO: 应用全局能量恢复加成
            return true;
        }

        /// <summary>
        /// 升级元素增幅
        /// </summary>
        public bool UpgradeElementAmplification(int cost)
        {
            // TODO: 检查资源
            elementUpgradeLevel++;
            // TODO: 应用元素伤害加成
            return true;
        }

        /// <summary>
        /// 解锁净化冲击
        /// </summary>
        public bool UnlockPurificationStrike(int cost)
        {
            // TODO: 检查资源
            hasPurificationStrike = true;
            return true;
        }

        /// <summary>
        /// 使用净化冲击
        /// </summary>
        public void UsePurificationStrike()
        {
            if (!CanUsePurificationStrike)
                return;

            purificationStrikeUses++;
            purificationStrikeTimer = purificationStrikeCooldown;

            // 对全地图所有怪物造成伤害并击晕
            // TODO: 实现全地图伤害和击晕效果
            OnPurificationStrikeUsed?.Invoke();
        }

        /// <summary>
        /// 解锁治疗无人机
        /// </summary>
        public bool UnlockHealingDrone(int cost)
        {
            // TODO: 检查资源
            hasHealingDrone = true;
            SpawnHealingDrone();
            return true;
        }

        private void SpawnHealingDrone()
        {
            if (healingDronePrefab != null && activeHealingDrone == null)
            {
                activeHealingDrone = Instantiate(healingDronePrefab, transform.position, Quaternion.identity);
            }
        }

        public event System.Action<float, float> OnHealthChanged;
        public event System.Action OnCoreDestroyed;
        public event System.Action OnPurificationStrikeUsed;
    }
}

