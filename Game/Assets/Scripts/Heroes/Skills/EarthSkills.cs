using UnityEngine;
using EmberKeepers.Combat;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes.Skills
{
    /// <summary>
    /// 震荡射击 - 碎岩者 (Rocker)
    /// 射出一发石弹，造成范围伤害并有几率击退敌人
    /// </summary>
    public class Skill_SeismicShot : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 10f;
        [SerializeField] private float damage = 40f;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockbackChance = 0.7f;

        protected override void ExecuteSkill()
        {
            // 找到接近基地的怪物
            MonsterBase target = FindTargetNearBase();
            if (target == null) return;

            Vector3 targetPos = target.transform.position;

            // 范围伤害
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(targetPos, explosionRadius);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(damage, ElementType.Earth);
                    
                    // 击退
                    if (Random.value < knockbackChance)
                    {
                        Vector3 direction = (enemy.transform.position - targetPos).normalized;
                        // TODO: 应用击退效果
                    }
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private MonsterBase FindTargetNearBase()
        {
            GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
            if (core != null)
            {
                var enemies = CombatSystem.Instance?.FindEnemiesInRange(core.transform.position, 15f);
                if (enemies != null && enemies.Count > 0)
                {
                    // 找到最接近基地的怪物
                    MonsterBase closest = null;
                    float closestDistance = float.MaxValue;
                    foreach (var enemy in enemies)
                    {
                        float distance = Vector3.Distance(core.transform.position, enemy.transform.position);
                        if (distance < closestDistance)
                        {
                            closest = enemy;
                            closestDistance = distance;
                        }
                    }
                    return closest;
                }
            }
            return null;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：冷却就绪，目标为接近基地的怪物
            return FindTargetNearBase() != null;
        }
    }

    /// <summary>
    /// 石化皮肤 - 大地之盾 (Terra)
    /// 获得大量护甲，并强制吸引周围敌人的仇恨
    /// </summary>
    public class Skill_PetrifiedSkin : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float duration = 10f;
        [SerializeField] private float armorBonus = 30f;
        [SerializeField] private float tauntRadius = 6f;

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
            // 提高护甲
            owner.physicalDefense += armorBonus;
            isActive = true;
            buffEndTime = Time.time + duration;

            // 强制吸引仇恨
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, tauntRadius);
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    // TODO: 实现仇恨系统，强制怪物攻击此英雄
                }
            }

            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private void EndBuff()
        {
            owner.physicalDefense -= armorBonus;
            isActive = false;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：自身生命低于60%且周围有怪物时
            bool lowHealth = owner.CurrentHealth / owner.MaxHealth < 0.6f;
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, tauntRadius);
            bool hasEnemies = enemies != null && enemies.Count > 0;
            
            return lowHealth && hasEnemies;
        }
    }

    /// <summary>
    /// 泥沼陷阱 - 泥沼巫师 (Mire)
    /// 在地面制造一片泥沼，对敌人造成轻微持续伤害并大幅降低其移动速度
    /// </summary>
    public class Skill_MireTrap : HeroSkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private float range = 12f;
        [SerializeField] private float areaRadius = 4f;
        [SerializeField] private float duration = 10f;
        [SerializeField] private float damagePerSecond = 5f;
        [SerializeField] private float slowAmount = 0.8f;

        protected override void ExecuteSkill()
        {
            // 找到怪物密集且远离英雄的位置
            Vector3 targetPosition = FindBestTrapPosition();
            
            // 创建泥沼区域
            // TODO: 创建持续伤害和减速区域效果
            
            Debug.Log($"{owner.HeroName} 使用了 {skillName}！");
        }

        private Vector3 FindBestTrapPosition()
        {
            // 找到怪物密集且远离英雄的位置
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, range * 2f);
            if (enemies != null && enemies.Count > 0)
            {
                Vector3 center = Vector3.zero;
                int count = 0;
                foreach (var enemy in enemies)
                {
                    float distance = Vector3.Distance(owner.transform.position, enemy.transform.position);
                    if (distance > 5f) // 远离英雄
                    {
                        center += enemy.transform.position;
                        count++;
                    }
                }
                if (count > 0)
                    return center / count;
            }
            return owner.transform.position + owner.transform.forward * 8f;
        }

        public override bool ShouldUseSkill()
        {
            if (!IsReady) return false;
            
            // AI逻辑：怪物密集且远离英雄时
            var enemies = CombatSystem.Instance?.FindEnemiesInRange(owner.transform.position, range * 2f);
            if (enemies != null && enemies.Count >= 3)
            {
                // 检查是否有远离英雄的怪物
                foreach (var enemy in enemies)
                {
                    float distance = Vector3.Distance(owner.transform.position, enemy.transform.position);
                    if (distance > 5f)
                        return true;
                }
            }
            return false;
        }
    }
}

