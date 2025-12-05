using UnityEngine;
using EmberKeepers.Data;
using EmberKeepers.Heroes.Skills;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 冰晶狙击 / Cryo - 冰系火力手
    /// </summary>
    public class Hero_Cryo : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_FrostPierce>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "冰晶狙击";
            elementType = ElementType.Ice;
            heroClass = HeroClass.DPS;
            
            // 属性倾向：敏捷
            agility = 5;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 极地卫士 / Boreas - 冰系守护者
    /// </summary>
    public class Hero_Boreas : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_AbsoluteZero>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "极地卫士";
            elementType = ElementType.Ice;
            heroClass = HeroClass.Guardian;
            
            // 属性倾向：力量 & 智力
            strength = 3;
            intelligence = 2;
            UpdateStatsFromAttributes();
        }
    }

    /// <summary>
    /// 寒霜祭司 / Glacia - 冰系策术师
    /// </summary>
    public class Hero_Glacia : HeroBase
    {
        protected override void InitializeSkill()
        {
            base.InitializeSkill();
            activeSkill = gameObject.AddComponent<Skills.Skill_IceBarrier>();
            activeSkill.Initialize(this);
        }

        public override void Initialize(string id)
        {
            base.Initialize(id);
            heroName = "寒霜祭司";
            elementType = ElementType.Ice;
            heroClass = HeroClass.Support;
            
            // 属性倾向：智力
            intelligence = 5;
            UpdateStatsFromAttributes();
        }
    }
}

