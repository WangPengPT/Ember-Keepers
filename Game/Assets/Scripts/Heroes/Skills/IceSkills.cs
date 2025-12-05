using UnityEngine;
using EmberKeepers.Combat;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes.Skills
{
    /// <summary>
    /// 霜冻穿刺 - 冰晶狙击 (Cryo)
    /// 射出一发冰晶箭，对直线上的所有敌人造成伤害并减速
    /// </summary>
    public class Skill_FrostPierce : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 15f;
        [SerializeField] private float damage = 35f;
        [SerializeField] private float slowAmount = 0.5f;
        [SerializeField] private float slowDuration = 3f;

        protected override void ExecuteSkill()
        {
            // 找到远离基地的直线怪群
            Vector3 direction = owner.transform.forward;
            Vector3 startPos = owner.transform.position;

            // 射线检测
            RaycastHit[] hits = Physics.RaycastAll(startPos, direction, range);
            
            foreach (var hit in hits)
            {
                MonsterBase monster = hit.collider.GetComponent<MonsterBase>();
                if (monster != null && !monster.IsDead)
                {
                    monster.TakeDamage(damage, ElementType.Ice);
                    // TODO: 应用减速效果
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：冷却就绪，目标为远离基地的直线怪群
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, range);
            return enemies != null && enemies.Count >= 2;
        }
    }

    /// <summary>
    /// 绝对零度 - 极地卫士 (Boreas)
    /// 释放范围冰环，对周围敌人造成伤害并短暂冻结
    /// </summary>
    public class Skill_AbsoluteZero : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float radius = 5f;
        [SerializeField] private float damage = 30f;
        [SerializeField] private float freezeDuration = 2f;

        protected override void ExecuteSkill()
        {
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, radius);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(damage, ElementType.Ice);
                    // TODO: 应用冻结效果
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：周围怪物数量>5时
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, radius);
            return enemies != null && enemies.Count > 5;
        }
    }

    /// <summary>
    /// 冰封结界 - 寒霜祭司 (Glacia)
    /// 在目标防线生成一个持续减速的冰霜区域
    /// </summary>
    public class Skill_IceBarrier : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 10f;
        [SerializeField] private float areaRadius = 4f;
        [SerializeField] private float slowAmount = 0.7f;
        [SerializeField] private float duration = 8f;

        protected override void ExecuteSkill()
        {
            // 找到接近基地核心的怪物位置
            Vector3 targetPosition = FindMonsterNearBase();
            
            // 在目标位置创建冰霜区域
            // TODO: 创建持续减速区域效果
            
            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private Vector3 FindMonsterNearBase()
        {
            // 找到最接近基地的怪物
            GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
            if (core != null)
            {
                var enemies = CombatSystem.Instance?.FindEnemiesInRange(core.transform.position, 15f);
                if (enemies != null && enemies.Count > 0)
                {
                    return enemies[0].transform.position;
                }
            }
            return owner.transform.position + owner.transform.forward * 5f;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：怪物接近基地核心时
            GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
            if (core != null)
            {
                var enemies = CombatSystem.Instance?.FindEnemiesInRange(core.transform.position, 10f);
                return enemies != null && enemies.Count > 0;
            }
            return false;
        }
    }
}

