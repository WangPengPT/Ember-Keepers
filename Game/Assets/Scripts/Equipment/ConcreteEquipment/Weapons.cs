using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Equipment
{
    /// <summary>
    /// 基础长弓 - 白色武器（火力手）
    /// </summary>
    [CreateAssetMenu(fileName = "BasicLongbow", menuName = "EmberKeepers/Equipment/Weapon/BasicLongbow")]
    public class Weapon_BasicLongbow : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_basic_longbow";
            equipmentName = "基础长弓";
            rarity = Rarity.Common;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.DPS;
            attackDamage = 10f;
        }
    }

    /// <summary>
    /// 训练法典 - 白色武器（策术师）
    /// </summary>
    [CreateAssetMenu(fileName = "TrainingCodex", menuName = "EmberKeepers/Equipment/Weapon/TrainingCodex")]
    public class Weapon_TrainingCodex : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_training_codex";
            equipmentName = "训练法典";
            rarity = Rarity.Common;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.Support;
            skillPower = 10f;
        }
    }

    /// <summary>
    /// 磨损战斧 - 白色武器（守护者）
    /// </summary>
    [CreateAssetMenu(fileName = "WornBattleAxe", menuName = "EmberKeepers/Equipment/Weapon/WornBattleAxe")]
    public class Weapon_WornBattleAxe : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_worn_battle_axe";
            equipmentName = "磨损战斧";
            rarity = Rarity.Common;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.Guardian;
            physicalDefense = 10f;
        }
    }

    /// <summary>
    /// 寒霜法杖 - 绿色武器（策术师-冰）
    /// </summary>
    [CreateAssetMenu(fileName = "FrostStaff", menuName = "EmberKeepers/Equipment/Weapon/FrostStaff")]
    public class Weapon_FrostStaff : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_frost_staff";
            equipmentName = "寒霜法杖";
            rarity = Rarity.Uncommon;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.Support;
            skillPower = 15f;
            // 特殊效果：技能命中时，有30%几率施加1秒冻结
        }
    }

    /// <summary>
    /// 熔岩手炮 - 绿色武器（火力手-火）
    /// </summary>
    [CreateAssetMenu(fileName = "LavaCannon", menuName = "EmberKeepers/Equipment/Weapon/LavaCannon")]
    public class Weapon_LavaCannon : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_lava_cannon";
            equipmentName = "熔岩手炮";
            rarity = Rarity.Uncommon;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.DPS;
            attackDamage = 20f;
            // 特殊效果：普攻暴击时，在目标脚下留下灼烧区域
        }
    }

    /// <summary>
    /// 星火核心 - 橙色传奇武器（全职业）
    /// </summary>
    [CreateAssetMenu(fileName = "StarfireCore", menuName = "EmberKeepers/Equipment/Weapon/StarfireCore")]
    public class Weapon_StarfireCore : EquipmentBase
    {
        private void OnEnable()
        {
            equipmentId = "weapon_starfire_core";
            equipmentName = "星火核心";
            rarity = Rarity.Legendary;
            equipmentType = EquipmentType.Weapon;
            requiredClass = HeroClass.DPS; // None表示全职业，但枚举不支持，用DPS占位
            attackDamage = 30f;
            skillPower = 30f;
            // 特殊效果：激活英雄技能时，对周围所有友军回复少量生命值
        }
    }
}

