using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 英雄数据表（从Excel导入）
    /// </summary>
    [CreateAssetMenu(fileName = "HeroData", menuName = "EmberKeepers/Data/HeroData")]
    public class HeroData : ScriptableObject
    {
        [Header("Hero Identity")]
        public string heroId;
        public string heroNameCN;
        public string heroNameEN;
        public ElementType elementType;
        public HeroClass heroClass;

        [Header("Base Attributes")]
        public int baseStrength = 0;
        public int baseAgility = 0;
        public int baseIntelligence = 0;
        public int baseElementMastery = 0;

        [Header("Base Stats")]
        public float baseHealth = 100f;
        public float baseAttackDamage = 10f;
        public float baseAttackSpeed = 1f;
        public float baseMoveSpeed = 5f;
        public float basePhysicalDefense = 0f;
        public float baseElementResistance = 0f;

        [Header("Energy System")]
        public float baseMaxEnergy = 100f;
        public float baseEnergyRegenRate = 5f;

        [Header("Skill")]
        public string skillId;
        public string skillName;
        public float skillCooldown = 10f;
        public float skillEnergyCost = 50f;
        public string skillDescription;

        [Header("AI Logic")]
        public string aiLogicType; // "LowHealth", "CooldownReady", "MonsterNearBase", etc.
    }
}

