using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Monsters
{
    /// <summary>
    /// 影界低语者 - 基础小怪，数量庞大，血量低
    /// </summary>
    public class Monster_Whisper : MonsterBase
    {
        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "影界低语者";
            family = MonsterFamily.Whisper;
            isElite = false;
            isBoss = false;

            // 基础属性（等级1）
            maxHealth = 50f;
            attackDamage = 5f;
            attackSpeed = 1f;
            moveSpeed = 3f;

            // 元素抗性：无抗性（Lvl 1-3），后期获得微弱物理抗性
            if (level >= 4)
            {
                physicalResistance = 0.1f;
            }

            ScaleStatsByLevel(level);
        }
    }
}

