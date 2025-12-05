using UnityEngine;
using EmberKeepers.Data;
using EmberKeepers.Heroes.Skills;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 电弧法师 / Volta - 雷系火力手
    /// </summary>
    public class Hero_Volta : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_ChainLightning>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "电弧法师";
            elementType = ElementType.Thunder;
            heroClass = HeroClass.DPS;
            
            // 属性倾向：智力 & 敏捷
            intelligence = 3;
            agility = 2;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 雷霆战甲 / Fulgur - 雷系守护者
    /// </summary>
    public class Hero_Fulgur : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_ChargeOverload>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "雷霆战甲";
            elementType = ElementType.Thunder;
            heroClass = HeroClass.Guardian;
            
            // 属性倾向：力量
            strength = 5;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 脉冲技师 / Surge - 雷系策术师
    /// </summary>
    public class Hero_Surge : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_EnergySurge>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "脉冲技师";
            elementType = ElementType.Thunder;
            heroClass = HeroClass.Support;
            
            // 属性倾向：智力
            intelligence = 5;
            UpdateStatsFromAttributes();
        }
    }
}

