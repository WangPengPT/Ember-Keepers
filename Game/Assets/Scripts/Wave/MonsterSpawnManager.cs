using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmberKeepers.Monsters;
using EmberKeepers.Data;
using EmberKeepers.Core;

namespace EmberKeepers.Wave
{
    /// <summary>
    /// 怪物生成管理器
    /// </summary>
    public class MonsterSpawnManager : MonoBehaviour
    {
        public static MonsterSpawnManager Instance { get; private set; }

        [Header("Spawn Settings")]
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform baseCore;
        [SerializeField] private float spawnRadius = 20f;

        [Header("Monster Prefabs")]
        [SerializeField] private GameObject[] monsterPrefabs;

        private List<MonsterBase> activeMonsters = new List<MonsterBase>();
        private WaveManager waveManager;

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

        private void Start()
        {
            waveManager = FindFirstObjectByType<WaveManager>();
            
            if (baseCore == null)
            {
                GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
                if (core != null)
                    baseCore = core.transform;
            }

            InitializeSpawnPoints();
        }

        private void InitializeSpawnPoints()
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                // 在基地周围创建生成点
                spawnPoints = new Transform[8];
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * 45f * Mathf.Deg2Rad;
                    Vector3 position = baseCore.position + new Vector3(
                        Mathf.Cos(angle) * spawnRadius,
                        0f,
                        Mathf.Sin(angle) * spawnRadius
                    );

                    GameObject spawnPoint = new GameObject($"SpawnPoint_{i}");
                    spawnPoint.transform.position = position;
                    spawnPoints[i] = spawnPoint.transform;
                }
            }
        }

        /// <summary>
        /// 生成波次怪物
        /// </summary>
        public IEnumerator SpawnWave(WaveDataEntry waveData)
        {
            Debug.Log($"开始生成第 {waveData.waveNumber} 波怪物");

            // 生成Boss
            if (waveData.hasBoss)
            {
                SpawnBoss(waveData.bossId);
            }

            // 生成普通怪物和精英怪
            foreach (var spawnData in waveData.monsterSpawns)
            {
                StartCoroutine(SpawnMonsterGroup(spawnData));
                yield return new WaitForSeconds(spawnData.spawnDelay);
            }
        }

        /// <summary>
        /// 生成怪物组
        /// </summary>
        private IEnumerator SpawnMonsterGroup(MonsterSpawnData spawnData)
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                SpawnMonster(spawnData.monsterId, spawnData.level);
                
                if (i < spawnData.count - 1)
                {
                    yield return new WaitForSeconds(spawnData.spawnInterval);
                }
            }
        }

        /// <summary>
        /// 生成单个怪物
        /// </summary>
        public MonsterBase SpawnMonster(string monsterId, int level)
        {
            // 选择生成点
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // 根据monsterId获取预制体
            GameObject prefab = GetMonsterPrefab(monsterId);
            if (prefab == null)
            {
                Debug.LogError($"找不到怪物预制体: {monsterId}");
                return null;
            }

            // 实例化怪物
            GameObject monsterObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            MonsterBase monster = monsterObj.GetComponent<MonsterBase>();

            if (monster != null)
            {
                monster.Initialize(monsterId, level, baseCore);
                monster.OnMonsterDied += OnMonsterDied;
                activeMonsters.Add(monster);
            }

            return monster;
        }

        /// <summary>
        /// 生成Boss
        /// </summary>
        private void SpawnBoss(string bossId)
        {
            // Boss在正前方生成
            Vector3 bossPosition = baseCore.position + Vector3.forward * spawnRadius;
            
            GameObject prefab = GetMonsterPrefab(bossId);
            if (prefab == null) return;

            GameObject bossObj = Instantiate(prefab, bossPosition, Quaternion.identity);
            MonsterBase boss = bossObj.GetComponent<MonsterBase>();

            if (boss != null)
            {
                boss.Initialize(bossId, 1, baseCore);
                boss.OnMonsterDied += OnMonsterDied;
                activeMonsters.Add(boss);
            }
        }

        /// <summary>
        /// 获取怪物预制体
        /// </summary>
        private GameObject GetMonsterPrefab(string monsterId)
        {
            // TODO: 根据monsterId从资源管理器加载
            // 暂时返回默认预制体
            if (monsterPrefabs != null && monsterPrefabs.Length > 0)
            {
                return monsterPrefabs[0];
            }
            return null;
        }

        /// <summary>
        /// 怪物死亡回调
        /// </summary>
        private void OnMonsterDied(MonsterBase monster)
        {
            activeMonsters.Remove(monster);

            // 检查波次是否完成
            if (activeMonsters.Count == 0)
            {
                OnWaveCleared?.Invoke();
            }
        }

        /// <summary>
        /// 清除所有怪物
        /// </summary>
        public void ClearAllMonsters()
        {
            foreach (var monster in activeMonsters)
            {
                if (monster != null)
                    Destroy(monster.gameObject);
            }
            activeMonsters.Clear();
        }

        public int ActiveMonsterCount => activeMonsters.Count;
        public event System.Action OnWaveCleared;
    }
}

