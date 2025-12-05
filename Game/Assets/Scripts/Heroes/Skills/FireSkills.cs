using UnityEngine;
using EmberKeepers.Combat;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes.Skills
{
    /// <summary>
    /// 燃魂弹幕 - 烬火射手 (Ignis)
    /// 短暂提高攻速，使普攻附带持续灼烧效果
    /// </summary>
    public class Skill_BurningSoulBarrage : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float attackSpeedBonus = 0.5f;
        [SerializeField] private float burnDamagePerSecond = 5f;
        [SerializeField] private float burnDuration = 3f;

        private float buffEndTime = 0f;
        private bool isActive = false;

        protected override void Update()
        {
            base.Update();
            
            if (isActive && Time.time >= buffEndTime)
            {
                EndBuff();
            }
        }

        protected override void ExecuteSkill()
        {
            // 提高攻速
            owner.attackSpeed += attackSpeedBonus;
            isActive = true;
            buffEndTime = Time.time + duration;

            // TODO: 添加视觉效果
            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private void EndBuff()
        {
            owner.attackSpeed -= attackSpeedBonus;
            isActive = false;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：冷却就绪时，目标为血量最高的非Boss怪
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, 10f);
            if (enemies != null && enemies.Count > 0)
            {
                // 找到血量最高的非Boss怪
                foreach (var enemy in enemies)
                {
                    if (!enemy.IsBoss)
                        return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 熔岩护甲 - 熔岩壁垒 (Cinder)
    /// 释放火焰冲击，并获得一层吸收伤害的熔岩护盾
    /// </summary>
    public class Skill_MoltenArmor : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float shieldAmount = 50f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionDamage = 30f;

        protected override void ExecuteSkill()
        {
            // 释放火焰冲击
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, explosionRadius);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(explosionDamage, ElementType.Fire);
                }
            }

            // 添加护盾（TODO: 实现护盾系统）
            // owner.AddShield(shieldAmount);

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：自身生命低于50%时
            return owner.CurrentHealth / owner.MaxHealth < 0.5f;
        }
    }

    /// <summary>
    /// 净化烈焰 - 炽热咏者 (Sol)
    /// 在指定区域召唤火柱，造成范围伤害并清除区域内怪物的增益效果
    /// </summary>
    public class Skill_PurifyingFlame : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 8f;
        [SerializeField] private float damage = 40f;
        [SerializeField] private float duration = 3f;

        protected override void ExecuteSkill()
        {
            // 找到目标区域（怪物密集处）
            Vector3 targetPosition = FindBestTargetPosition();
            
            // 在目标位置创建火柱效果
            // TODO: 创建火柱特效和持续伤害区域
            
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(targetPosition, range);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(damage, ElementType.Fire);
                    // TODO: 清除增益效果
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private Vector3 FindBestTargetPosition()
        {
            // 找到怪物最密集的位置
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, 15f);
            if (enemies != null && enemies.Count > 0)
            {
                Vector3 center = Vector3.zero;
                foreach (var enemy in enemies)
                {
                    center += enemy.transform.position;
                }
                return center / enemies.Count;
            }
            return owner.transform.position + owner.transform.forward * 5f;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：怪物密集且有增益时
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, 15f);
            return enemies != null && enemies.Count >= 3;
        }
    }
}

