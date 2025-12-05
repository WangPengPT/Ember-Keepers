using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.Data
{
    /// <summary>
    /// 波次数据表（从Excel导入）
    /// </summary>
    [CreateAssetMenu(fileName = "WaveDataAsset", menuName = "EmberKeepers/Data/WaveDataAsset")]
    public class WaveDataAsset : ScriptableObject
    {
        public List<WaveDataEntry> waveEntries = new List<WaveDataEntry>();
    }

    [System.Serializable]
    public class WaveDataEntry
    {
        public int waveNumber;
        public float difficulty;
        public bool hasBoss;
        public string bossId;
        public List<MonsterSpawnData> monsterSpawns = new List<MonsterSpawnData>();
    }

    [System.Serializable]
    public class MonsterSpawnData
    {
        public string monsterId;
        public int count;
        public int level;
        public float spawnDelay;
        public float spawnInterval;
    }
}

