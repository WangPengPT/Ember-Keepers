using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Monsters
{
    /// <summary>
    /// 荒芜之主 - Boss 1（第10波）
    /// 多线召唤：周期性激活2-3个裂隙，召唤大量Lvl 3爬行者
    /// </summary>
    public class Boss_DesolateLord : MonsterBase
    {
        [Header("Boss Abilities")]
        [SerializeField] private float summonInterval = 15f;
        [SerializeField] private int riftCount = 3;
        [SerializeField] private int crawlersPerRift = 5;

        private float lastSummonTime = 0f;
        private int phase = 1;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "荒芜之主";
            isElite = false;
            isBoss = true;

            maxHealth = 2000f;
            attackDamage = 40f;
            moveSpeed = 2f;
            physicalResistance = 0.3f;

            ScaleStatsByLevel(level);
        }

        protected override void Update()
        {
            base.Update();

            // 多阶段机制
            float healthPercent = currentHealth / maxHealth;
            if (healthPercent < 0.5f && phase == 1)
            {
                phase = 2;
                riftCount = 4; // 第二阶段增加裂隙数量
            }

            if (Time.time - lastSummonTime >= summonInterval)
            {
                SummonCrawlers();
                lastSummonTime = Time.time;
            }
        }

        private void SummonCrawlers()
        {
            // TODO: 在多个位置召唤爬行者
            Debug.Log($"{monsterName} 召唤了 {riftCount} 个裂隙，每个裂隙召唤 {crawlersPerRift} 只爬行者！");
        }
    }

    /// <summary>
    /// 永冻之灾 - Boss 2（第15波）
    /// 元素锁定：战斗中周期性切换弱点元素，强制玩家调动英雄
    /// </summary>
    public class Boss_EternalFrost : MonsterBase
    {
        [Header("Boss Abilities")]
        [SerializeField] private float elementSwitchInterval = 20f;
        [SerializeField] private ElementType currentWeakness = ElementType.Fire;

        private float lastSwitchTime = 0f;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "永冻之灾";
            isElite = false;
            isBoss = true;

            maxHealth = 3000f;
            attackDamage = 35f;
            moveSpeed = 1.8f;
            iceResistance = 0.8f; // 免疫冰伤

            ScaleStatsByLevel(level);
        }

        protected override void Update()
        {
            base.Update();

            if (Time.time - lastSwitchTime >= elementSwitchInterval)
            {
                SwitchWeakness();
                lastSwitchTime = Time.time;
            }
        }

        private void SwitchWeakness()
        {
            // 切换到下一个弱点元素
            switch (currentWeakness)
            {
                case ElementType.Fire:
                    currentWeakness = ElementType.Thunder;
                    fireResistance = 0.2f;
                    thunderResistance = -0.3f; // 易伤
                    break;
                case ElementType.Thunder:
                    currentWeakness = ElementType.Earth;
                    thunderResistance = 0.2f;
                    earthResistance = -0.3f;
                    break;
                case ElementType.Earth:
                    currentWeakness = ElementType.Fire;
                    earthResistance = 0.2f;
                    fireResistance = -0.3f;
                    break;
            }

            Debug.Log($"{monsterName} 切换弱点元素为：{currentWeakness}！");
        }
    }

    /// <summary>
    /// 裂隙之心 - Boss 3（第20波）
    /// 技能压制：拥有全局性技能，能暂时使所有英雄的技能冷却时间翻倍
    /// </summary>
    public class Boss_RiftHeart : MonsterBase
    {
        [Header("Boss Abilities")]
        [SerializeField] private float suppressionInterval = 25f;
        [SerializeField] private float suppressionDuration = 8f;

        private float lastSuppressionTime = 0f;
        private bool isSuppressing = false;

        public override void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            base.Initialize(id, monsterLevel, targetTransform);
            monsterName = "裂隙之心";
            isElite = false;
            isBoss = true;

            maxHealth = 5000f;
            attackDamage = 50f;
            moveSpeed = 1.5f;
            physicalResistance = 0.4f;
            fireResistance = 0.3f;
            iceResistance = 0.3f;
            thunderResistance = 0.3f;
            earthResistance = 0.3f;

            ScaleStatsByLevel(level);
        }

        protected override void Update()
        {
            base.Update();

            if (!isSuppressing && Time.time - lastSuppressionTime >= suppressionInterval)
            {
                SuppressHeroSkills();
                lastSuppressionTime = Time.time;
            }

            if (isSuppressing && Time.time - lastSuppressionTime >= suppressionDuration)
            {
                EndSuppression();
            }
        }

        private void SuppressHeroSkills()
        {
            isSuppressing = true;
            // TODO: 使所有英雄技能冷却时间翻倍
            Debug.Log($"{monsterName} 释放了技能压制！所有英雄技能冷却时间翻倍！");
        }

        private void EndSuppression()
        {
            isSuppressing = false;
            // TODO: 恢复正常冷却时间
            Debug.Log($"{monsterName} 的技能压制结束了！");
        }
    }
}

