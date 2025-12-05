using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Monsters
{
    /// <summary>
    /// 烈焰指挥官 - 火系精英怪
    /// 周期性强化周围小怪的攻击力
    /// </summary>
    public class Monster_FlameCommander : MonsterBase
    {
        [Header("Elite Ability")]
        [SerializeField] private float buffInterval = 5f;
        [SerializeField] private float buffRadius = 8f;
        [SerializeField] private float attackBuffPercent = 0.3f;

        private float lastBuffTime = 0f;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "烈焰指挥官";
            family = MonsterFamily.Whisper; // 占位
            isElite = true;
            isBoss = false;

            maxHealth = 300f;
            attackDamage = 20f;
            moveSpeed = 2.5f;
            fireResistance = 0.5f;

            ScaleStatsByLevel(level);
        }

        protected override void Update()
        {
            base.Update();
            
            if (Time.time - lastBuffTime >= buffInterval)
            {
                BuffNearbyMonsters();
                lastBuffTime = Time.time;
            }
        }

        private void BuffNearbyMonsters()
        {
            // TODO: 强化周围小怪的攻击力
            Debug.Log($"{monsterName} 强化了周围怪物的攻击力！");
        }
    }

    /// <summary>
    /// 冰霜巨像 - 冰系精英怪
    /// 每次普攻都造成范围减速
    /// </summary>
    public class Monster_FrostGolem : MonsterBase
    {
        [Header("Elite Ability")]
        [SerializeField] private float slowRadius = 4f;
        [SerializeField] private float slowAmount = 0.5f;
        [SerializeField] private float slowDuration = 2f;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "冰霜巨像";
            isElite = true;
            isBoss = false;

            maxHealth = 400f;
            attackDamage = 25f;
            moveSpeed = 1.5f;
            iceResistance = 0.6f;

            ScaleStatsByLevel(level);
        }

        protected override void Attack()
        {
            base.Attack();
            // 范围减速
            ApplySlowArea();
        }

        private void ApplySlowArea()
        {
            // TODO: 对周围敌人应用减速效果
            Debug.Log($"{monsterName} 释放了范围减速！");
        }
    }

    /// <summary>
    /// 电能裂变体 - 雷系精英怪
    /// 阵亡后自爆，并对周围英雄造成大量雷系伤害
    /// </summary>
    public class Monster_ElectricFission : MonsterBase
    {
        [Header("Elite Ability")]
        [SerializeField] private float explosionRadius = 6f;
        [SerializeField] private float explosionDamage = 100f;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "电能裂变体";
            isElite = true;
            isBoss = false;

            maxHealth = 250f;
            attackDamage = 15f;
            moveSpeed = 3.5f;
            thunderResistance = 0.5f;

            ScaleStatsByLevel(level);
        }

        protected override void Die()
        {
            // 自爆
            Explode();
            base.Die();
        }

        private void Explode()
        {
            // TODO: 对周围英雄造成雷系伤害
            Debug.Log($"{monsterName} 自爆了！");
        }
    }
}

