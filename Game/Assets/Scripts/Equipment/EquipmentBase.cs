using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Equipment
{
    /// <summary>
    /// 装备基类
    /// </summary>
    public class EquipmentBase : ScriptableObject
    {
        [Header("Equipment Identity")]
        [SerializeField] protected string equipmentId;
        [SerializeField] protected string equipmentName;
        [SerializeField] protected Rarity rarity;
        [SerializeField] protected EquipmentType equipmentType;
        [SerializeField] protected HeroClass requiredClass;

        [Header("Base Stats")]
        [SerializeField] protected float attackDamage = 0f;
        [SerializeField] protected float skillPower = 0f;
        [SerializeField] protected float attackSpeed = 0f;
        [SerializeField] protected float physicalDefense = 0f;
        [SerializeField] protected float elementResistance = 0f;
        [SerializeField] protected float maxHealth = 0f;
        [SerializeField] protected float energyRegen = 0f;
        [SerializeField] protected float critChance = 0f;
        [SerializeField] protected float elementPenetration = 0f;

        [Header("Special Effects")]
        [SerializeField] protected string[] specialEffectIds;

        public string EquipmentId => equipmentId;
        public string EquipmentName => equipmentName;
        public Rarity Rarity => rarity;
        public EquipmentType EquipmentType => equipmentType;
        public HeroClass RequiredClass => requiredClass;

        /// <summary>
        /// 应用装备效果
        /// </summary>
        public virtual void ApplyEffect(Heroes.HeroBase hero)
        {
            // 子类实现特殊效果
        }

        /// <summary>
        /// 移除装备效果
        /// </summary>
        public virtual void RemoveEffect(Heroes.HeroBase hero)
        {
            // 子类实现
        }
    }
}

