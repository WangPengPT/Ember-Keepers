using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 装备数据表（从Excel导入）
    /// </summary>
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "EmberKeepers/Data/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        [Header("Equipment Identity")]
        public string equipmentId;
        public string equipmentName;
        public Rarity rarity;
        public EquipmentType equipmentType;
        public HeroClass requiredClass; // None表示全职业

        [Header("Base Stats")]
        public float attackDamage = 0f;
        public float skillPower = 0f;
        public float attackSpeed = 0f;
        public float physicalDefense = 0f;
        public float elementResistance = 0f;
        public float maxHealth = 0f;
        public float energyRegen = 0f;
        public float critChance = 0f;
        public float elementPenetration = 0f;
        public float moveSpeed = 0f;

        [Header("Special Effects")]
        public string[] effectIds; // 词条效果ID列表
        public string description;
    }
}

