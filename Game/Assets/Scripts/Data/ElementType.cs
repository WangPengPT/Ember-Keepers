namespace EmberKeepers.Data
{
    /// <summary>
    /// 元素类型枚举
    /// </summary>
    public enum ElementType
    {
        None = 0,
        Fire = 1,      // 火
        Ice = 2,       // 冰
        Thunder = 3,   // 雷
        Earth = 4      // 土
    }

    /// <summary>
    /// 职业类型枚举
    /// </summary>
    public enum HeroClass
    {
        Guardian = 0,  // 守护者
        DPS = 1,       // 火力手
        Support = 2    // 策术师
    }

    /// <summary>
    /// 装备稀有度
    /// </summary>
    public enum Rarity
    {
        Common = 0,    // 白色
        Uncommon = 1,  // 绿色
        Rare = 2,      // 蓝色
        Epic = 3,      // 紫色
        Legendary = 4  // 橙色
    }

    /// <summary>
    /// 装备类型
    /// </summary>
    public enum EquipmentType
    {
        Weapon = 0,    // 武器
        Armor = 1,     // 护甲
        Trinket = 2    // 饰品
    }
}

