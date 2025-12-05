namespace EmberKeepers.Utils
{
    /// <summary>
    /// 游戏常量定义
    /// </summary>
    public static class GameConstants
    {
        // 波次相关
        public const int MAX_MAIN_WAVES = 20;
        public const int ENDLESS_LOOP_WAVES = 10;

        // 属性书价格
        public const int ATTRIBUTE_BOOK_COST = 50;

        // 复活相关
        public const int BASE_REVIVE_COST = 100;
        public const float REVIVE_COST_MULTIPLIER = 1.5f;
        public const float FREE_REVIVE_WAVE_DELAY = 2f;

        // 基地核心
        public const int BASE_CORE_INITIAL_HEALTH = 1000;
        public const int PURIFICATION_STRIKE_MAX_USES = 3;
        public const float PURIFICATION_STRIKE_COOLDOWN = 30f;

        // 元进度
        public const int META_BASE_COST = 50;
        public const int META_COST_INCREMENT = 25;

        // 装备槽位数量
        public const int EQUIPMENT_SLOTS = 6;

        // 玩家标签
        public const string TAG_BASE_CORE = "BaseCore";
        public const string TAG_HERO = "Hero";
        public const string TAG_MONSTER = "Monster";

        // 层级名称
        public const string LAYER_DEPLOYMENT = "Deployment";
        public const string LAYER_HERO = "Hero";
        public const string LAYER_MONSTER = "Monster";

        // 场景名称
        public const string SCENE_MAIN_MENU = "MainMenu";
        public const string SCENE_GAME = "Game";

        // 音效名称
        public const string SFX_HERO_ATTACK = "HeroAttack";
        public const string SFX_HERO_SKILL = "HeroSkill";
        public const string SFX_HERO_DEATH = "HeroDeath";
        public const string SFX_MONSTER_DEATH = "MonsterDeath";
        public const string SFX_PURCHASE = "Purchase";
        public const string SFX_WAVE_START = "WaveStart";
        public const string SFX_WAVE_COMPLETE = "WaveComplete";
        public const string SFX_VICTORY = "Victory";
        public const string SFX_DEFEAT = "Defeat";
    }
}

