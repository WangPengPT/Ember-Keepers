using UnityEngine;
using EmberKeepers.Data;
using EmberKeepers.Heroes.Skills;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 碎岩者 / Rocker - 土系火力手
    /// </summary>
    public class Hero_Rocker : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_SeismicShot>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "碎岩者";
            elementType = ElementType.Earth;
            heroClass = HeroClass.DPS;
            
            // 属性倾向：敏捷 & 力量
            agility = 3;
            strength = 2;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 大地之盾 / Terra - 土系守护者
    /// </summary>
    public class Hero_Terra : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_PetrifiedSkin>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "大地之盾";
            elementType = ElementType.Earth;
            heroClass = HeroClass.Guardian;
            
            // 属性倾向：力量
            strength = 5;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 泥沼巫师 / Mire - 土系策术师
    /// </summary>
    public class Hero_Mire : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_MireTrap>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "泥沼巫师";
            elementType = ElementType.Earth;
            heroClass = HeroClass.Support;
            
            // 属性倾向：智力
            intelligence = 5;
            UpdateStatsFromAttributes();
        }
    }
}

