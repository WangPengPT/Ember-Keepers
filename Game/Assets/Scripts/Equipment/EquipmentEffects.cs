using UnityEngine;
using EmberKeepers.Heroes;
using EmberKeepers.Data;

namespace EmberKeepers.Equipment
{
    /// <summary>
    /// 装备词条效果基类
    /// </summary>
    public abstract class EquipmentEffectBase : MonoBehaviour
    {
        protected EquipmentBase equipment;
        protected HeroBase owner;

        public virtual void Initialize(EquipmentBase eq, HeroBase hero)
        {
            equipment = eq;
            owner = hero;
        }

        /// <summary>
        /// 应用效果
        /// </summary>
        public abstract void ApplyEffect();

        /// <summary>
        /// 移除效果
        /// </summary>
        public abstract void RemoveEffect();
    }

    /// <summary>
    /// 溅射效果 - 普攻有25%几率对目标周围小范围造成伤害
    /// </summary>
    public class Effect_Splash : EquipmentEffectBase
    {
        private float splashChance = 0.25f;
        private float splashRadius = 2f;
        private float splashDamagePercent = 0.5f;

        public override void ApplyEffect()
        {
            // TODO: 监听攻击事件，应用溅射效果
        }

        public override void RemoveEffect()
        {
            // TODO: 移除监听
        }
    }

    /// <summary>
    /// 星火共鸣 - 释放技能时，额外回复5%能量
    /// </summary>
    public class Effect_StarfireResonance : EquipmentEffectBase
    {
        private float energyRestorePercent = 0.05f;

        public override void ApplyEffect()
        {
            // TODO: 监听技能释放事件
        }

        public override void RemoveEffect()
        {
            // TODO: 移除监听
        }
    }

    /// <summary>
    /// 仇恨引导 - 每次受到伤害，增加英雄5%仇恨值，可叠加
    /// </summary>
    public class Effect_HateGuidance : EquipmentEffectBase
    {
        private float hateIncreasePercent = 0.05f;

        public override void ApplyEffect()
        {
            // TODO: 监听受伤事件
        }

        public override void RemoveEffect()
        {
            // TODO: 移除监听
        }
    }

    /// <summary>
    /// 致残 - 暴击时，有50%几率使目标移动速度降低30%
    /// </summary>
    public class Effect_Cripple : EquipmentEffectBase
    {
        private float procChance = 0.5f;
        private float slowAmount = 0.3f;
        private float slowDuration = 2f;

        public override void ApplyEffect()
        {
            // TODO: 监听暴击事件
        }

        public override void RemoveEffect()
        {
            // TODO: 移除监听
        }
    }

    /// <summary>
    /// 元素转换 - 普攻10%几率对目标施加相反元素的易伤效果
    /// </summary>
    public class Effect_ElementConversion : EquipmentEffectBase
    {
        private float procChance = 0.1f;
        private float vulnerabilityPercent = 0.2f;
        private float vulnerabilityDuration = 3f;

        public override void ApplyEffect()
        {
            // TODO: 监听攻击事件
        }

        public override void RemoveEffect()
        {
            // TODO: 移除监听
        }
    }
}

