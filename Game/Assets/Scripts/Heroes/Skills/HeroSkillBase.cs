using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes.Skills
{
    /// <summary>
    /// 英雄技能基类
    /// </summary>
    public abstract class HeroSkillBase : MonoBehaviour
    {
        [Header("Skill Info")]
        [SerializeField] protected string skillId;
        [SerializeField] protected string skillName;
        [SerializeField] protected float cooldown = 10f;
        [SerializeField] protected float energyCost = 50f;
        [SerializeField] protected float currentCooldown = 0f;

        [Header("References")]
        protected HeroBase owner;

        public string SkillId => skillId;
        public string SkillName => skillName;
        public float Cooldown => cooldown;
        public float EnergyCost => energyCost;
        public bool IsReady => currentCooldown <= 0f && owner.CurrentEnergy >= energyCost;

        public virtual void Initialize(HeroBase hero)
        {
            owner = hero;
        }

        protected virtual void Update()
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseSkill()
        {
            if (!IsReady)
                return false;

            currentCooldown = cooldown;
            owner.UseEnergy(energyCost);
            ExecuteSkill();
            return true;
        }

        /// <summary>
        /// 执行技能效果（子类实现）
        /// </summary>
        protected abstract void ExecuteSkill();

        /// <summary>
        /// 检查是否应该使用技能（AI逻辑）
        /// </summary>
        public virtual bool ShouldUseSkill()
        {
            return IsReady;
        }
    }
}

