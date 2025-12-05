using UnityEngine;
using System.Collections.Generic;
using EmberKeepers.Heroes;
using EmberKeepers.Monsters;
using EmberKeepers.Data;

namespace EmberKeepers.Combat
{
    /// <summary>
    /// 战斗系统，处理伤害计算、元素克制等
    /// </summary>
    public class CombatSystem : MonoBehaviour
    {
        public static CombatSystem Instance { get; private set; }

        [Header("Combat Settings")]
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private float detectionRange = 10f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 计算元素克制关系
        /// </summary>
        public static float GetElementMultiplier(ElementType attackerElement, ElementType defenderResistance)
        {
            // 火克冰，雷克土
            if (attackerElement == ElementType.Fire && defenderResistance == ElementType.Ice)
                return 1.5f; // 克制伤害加成
            
            if (attackerElement == ElementType.Thunder && defenderResistance == ElementType.Earth)
                return 1.5f;

            // 被克制
            if (attackerElement == ElementType.Ice && defenderResistance == ElementType.Fire)
                return 0.75f;
            
            if (attackerElement == ElementType.Earth && defenderResistance == ElementType.Thunder)
                return 0.75f;

            return 1f; // 无克制关系
        }

        /// <summary>
        /// 英雄攻击怪物
        /// </summary>
        public void HeroAttackMonster(HeroBase hero, MonsterBase monster)
        {
            if (hero == null || monster == null || hero.IsDead)
                return;

            float distance = Vector3.Distance(hero.transform.position, monster.transform.position);
            if (distance > attackRange)
                return;

            float baseDamage = hero.AttackDamage;
            ElementType element = hero.ElementType;

            // 计算最终伤害
            float finalDamage = CalculateDamage(baseDamage, element, monster);
            monster.TakeDamage(finalDamage, element);
        }

        /// <summary>
        /// 计算伤害（考虑元素抗性和克制）
        /// </summary>
        private float CalculateDamage(float baseDamage, ElementType element, MonsterBase monster)
        {
            float resistance = GetElementResistance(element, monster);
            float multiplier = GetElementMultiplier(element, GetMonsterPrimaryResistance(monster));
            
            float finalDamage = baseDamage * (1f - resistance) * multiplier;
            return Mathf.Max(1f, finalDamage);
        }

        /// <summary>
        /// 获取怪物的元素抗性
        /// </summary>
        private float GetElementResistance(ElementType element, MonsterBase monster)
        {
            // TODO: 从怪物数据获取抗性
            return 0f;
        }

        /// <summary>
        /// 获取怪物的主要抗性类型
        /// </summary>
        private ElementType GetMonsterPrimaryResistance(MonsterBase monster)
        {
            // TODO: 从怪物数据获取
            return ElementType.None;
        }

        /// <summary>
        /// 查找最近的敌人
        /// </summary>
        public MonsterBase FindNearestEnemy(Vector3 position, float range)
        {
            MonsterBase nearest = null;
            float nearestDistance = float.MaxValue;

            // TODO: 使用对象池或管理器获取所有怪物
            MonsterBase[] monsters = FindObjectsOfType<MonsterBase>();
            
            foreach (var monster in monsters)
            {
                if (monster.IsDead) continue;

                float distance = Vector3.Distance(position, monster.transform.position);
                if (distance <= range && distance < nearestDistance)
                {
                    nearest = monster;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        /// <summary>
        /// 查找范围内的所有敌人
        /// </summary>
        public List<MonsterBase> FindEnemiesInRange(Vector3 position, float range)
        {
            List<MonsterBase> enemies = new List<MonsterBase>();
            
            MonsterBase[] monsters = FindObjectsOfType<MonsterBase>();
            foreach (var monster in monsters)
            {
                if (monster.IsDead) continue;

                float distance = Vector3.Distance(position, monster.transform.position);
                if (distance <= range)
                {
                    enemies.Add(monster);
                }
            }

            return enemies;
        }
    }
}

