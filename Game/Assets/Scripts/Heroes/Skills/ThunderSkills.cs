using UnityEngine;
using EmberKeepers.Combat;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes.Skills
{
    /// <summary>
    /// 连锁闪电 - 电弧法师 (Volta)
    /// 释放一道高能闪电，在多个敌人间弹跳造成伤害
    /// </summary>
    public class Skill_ChainLightning : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 12f;
        [SerializeField] private float damage = 25f;
        [SerializeField] private int maxBounces = 5;
        [SerializeField] private float bounceRange = 5f;

        protected override void ExecuteSkill()
        {
            // 找到初始目标（中等距离的怪物群）
            MonsterBase firstTarget = FindMediumRangeTarget();
            if (firstTarget == null) return;

            // 连锁闪电
            MonsterBase currentTarget = firstTarget;
            System.Collections.Generic.HashSet<MonsterBase> hitTargets = new System.Collections.Generic.HashSet<MonsterBase>();
            
            for (int i = 0; i < maxBounces; i++)
            {
                if (currentTarget == null || hitTargets.Contains(currentTarget))
                    break;

                currentTarget.TakeDamage(damage, ElementType.Thunder);
                hitTargets.Add(currentTarget);

                // 寻找下一个目标
                currentTarget = FindNextBounceTarget(currentTarget.transform.position, hitTargets);
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private MonsterBase FindMediumRangeTarget()
        {
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, range);
            if (enemies != null && enemies.Count > 0)
            {
                // 找到中等距离的怪物
                foreach (var enemy in enemies)
                {
                    float distance = Vector3.Distance(owner.transform.position, enemy.transform.position);
                    if (distance > 3f && distance < range * 0.7f)
                        return enemy;
                }
                return enemies[0];
            }
            return null;
        }

        private MonsterBase FindNextBounceTarget(Vector3 fromPosition, System.Collections.Generic.HashSet<MonsterBase> hitTargets)
        {
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(fromPosition, bounceRange);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    if (!hitTargets.Contains(enemy))
                        return enemy;
                }
            }
            return null;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：冷却就绪，目标为中等距离的怪物群
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, range);
            return enemies != null && enemies.Count >= 2;
        }
    }

    /// <summary>
    /// 电荷超载 - 雷霆战甲 (Fulgur)
    /// 激活电荷，大幅提高自身防御力，并周期性对周围敌人造成电击伤害
    /// </summary>
    public class Skill_ChargeOverload : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float duration = 8f;
        [SerializeField] private float defenseBonus = 20f;
        [SerializeField] private float shockInterval = 1f;
        [SerializeField] private float shockRadius = 4f;
        [SerializeField] private float shockDamage = 15f;

        private float buffEndTime = 0f;
        private float lastShockTime = 0f;
        private bool isActive = false;

        protected override void Update()
        {
            base.Update();
            
            if (isActive)
            {
                if (Time.time >= buffEndTime)
                {
                    EndBuff();
                }
                else if (Time.time - lastShockTime >= shockInterval)
                {
                    ShockEnemies();
                    lastShockTime = Time.time;
                }
            }
        }

        protected override void ExecuteSkill()
        {
            // 提高防御力
            owner.physicalDefense += defenseBonus;
            isActive = true;
            buffEndTime = Time.time + duration;
            lastShockTime = Time.time;

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private void ShockEnemies()
        {
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, shockRadius);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(shockDamage, ElementType.Thunder);
                }
            }
        }

        private void EndBuff()
        {
            owner.physicalDefense -= defenseBonus;
            isActive = false;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：自身生命低于70%时
            return owner.CurrentHealth / owner.MaxHealth < 0.7f;
        }
    }

    /// <summary>
    /// 能量涌动 - 脉冲技师 (Surge)
    /// 治疗周围友方英雄，并立即为他们回复少量能量
    /// </summary>
    public class Skill_EnergySurge : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float radius = 8f;
        [SerializeField] private float healAmount = 30f;
        [SerializeField] private float energyRestore = 20f;

        protected override void ExecuteSkill()
        {
            // 找到周围友方英雄
            var heroes = FindObjectsOfType<HeroBase>();
            foreach (var hero in heroes)
            {
                if (hero == owner || hero.IsDead) continue;

                float distance = Vector3.Distance(owner.transform.position, hero.transform.position);
                if (distance <= radius)
                {
                    hero.Heal(healAmount);
                    hero.UseEnergy(-energyRestore); // 负值表示恢复
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：友方英雄生命低于80%且能量不足时
            var heroes = FindObjectsOfType<HeroBase>();
            foreach (var hero in heroes)
            {
                if (hero == owner || hero.IsDead) continue;

                float distance = Vector3.Distance(owner.transform.position, hero.transform.position);
                if (distance <= radius)
                {
                    bool lowHealth = hero.CurrentHealth / hero.MaxHealth < 0.8f;
                    bool lowEnergy = hero.CurrentEnergy < hero.MaxEnergy * 0.5f;
                    if (lowHealth && lowEnergy)
                        return true;
                }
            }
            return false;
        }
    }
}

