using UnityEngine;
using EmberKeepers.Data;
using EmberKeepers.Heroes.Skills;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 烬火射手 / Ignis - 火系火力手
    /// </summary>
    public class Hero_Ignis : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_BurningSoulBarrage>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "烬火射手";
            elementType = ElementType.Fire;
            heroClass = HeroClass.DPS;
            
            // 属性倾向：敏捷 & 智力
            agility = 3;
            intelligence = 2;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 熔岩壁垒 / Cinder - 火系守护者
    /// </summary>
    public class Hero_Cinder : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_MoltenArmor>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "熔岩壁垒";
            elementType = ElementType.Fire;
            heroClass = HeroClass.Guardian;
            
            // 属性倾向：力量
            strength = 5;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 炽热咏者 / Sol - 火系策术师
    /// </summary>
    public class Hero_Sol : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_PurifyingFlame>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "炽热咏者";
            elementType = ElementType.Fire;
            heroClass = HeroClass.Support;
            
            // 属性倾向：智力
            intelligence = 5;
            UpdateStatsFromAttributes();
        }
    }
}

