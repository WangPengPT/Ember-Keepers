using UnityEngine;
using EmberKeepers.Monsters;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 怪物数据表（从Excel导入）
    /// </summary>
    [CreateAssetMenu(fileName = "MonsterData", menuName = "EmberKeepers/Data/MonsterData")]
    public class MonsterData : ScriptableObject
    {
        [Header("Monster Identity")]
        public string monsterId;
        public string monsterName;
        public MonsterFamily family;
        public bool isElite = false;
        public bool isBoss = false;

        [Header("Base Stats (Level 1)")]
        public float baseHealth = 100f;
        public float baseAttackDamage = 10f;
        public float baseAttackSpeed = 1f;
        public float baseMoveSpeed = 3f;

        [Header("Element Resistance")]
        public float fireResistance = 0f;
        public float iceResistance = 0f;
        public float thunderResistance = 0f;
        public float earthResistance = 0f;
        public float physicalResistance = 0f;

        [Header("Special Abilities")]
        public string[] abilityIds;
        public string description;

        [Header("Loot")]
        public int minGoldDrop = 5;
        public int maxGoldDrop = 15;
        public float equipmentDropChance = 0.1f;
    }
}

