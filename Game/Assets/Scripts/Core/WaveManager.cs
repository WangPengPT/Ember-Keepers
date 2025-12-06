using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmberKeepers.Data;
using EmberKeepers.Wave;

namespace EmberKeepers.Core
{
    /// <summary>
    /// 波次管理器，控制怪物波次的生成和进度
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField] private int currentWave = 0;
        [SerializeField] private int maxMainWaves = 20;
        [SerializeField] private int endlessLoopWaves = 10;
        [SerializeField] private bool isEndlessMode = false;

        [Header("Wave Data")]
        private List<WaveDataEntry> mainWaveData = new List<WaveDataEntry>();
        private List<WaveDataEntry> endlessWaveData = new List<WaveDataEntry>();

        private bool isWaveInProgress = false;
        private float waveStartTime;
        private MonsterSpawnManager spawnManager;

        public int CurrentWave => currentWave;
        public bool IsEndlessMode => isEndlessMode;
        public bool IsWaveInProgress => isWaveInProgress;

        private void Awake()
        {
            LoadWaveData();
            spawnManager = FindFirstObjectByType<MonsterSpawnManager>();
            
            if (spawnManager != null)
            {
                spawnManager.OnWaveCleared += OnWaveCompleted;
            }
        }

        private void LoadWaveData()
        {
            // 使用配置生成器生成波次数据
            mainWaveData = WaveConfigGenerator.GenerateMainWaves();
            endlessWaveData = WaveConfigGenerator.GenerateEndlessWaves();
            
            Debug.Log($"加载了 {mainWaveData.Count} 波主线关卡和 {endlessWaveData.Count} 波无尽循环");
        }

        public void ResetWaves()
        {
            currentWave = 0;
            isEndlessMode = false;
            isWaveInProgress = false;
        }

        public void StartCurrentWave()
        {
            if (isWaveInProgress)
                return;

            currentWave++;
            
            if (currentWave > maxMainWaves)
            {
                isEndlessMode = true;
                // 进入无尽模式
            }

            isWaveInProgress = true;
            waveStartTime = Time.time;

            WaveDataEntry waveData = GetCurrentWaveData();
            if (waveData != null)
            {
                StartCoroutine(SpawnWave(waveData));
            }
            else
            {
                Debug.LogError($"WaveManager: 无法开始第 {currentWave} 波，数据为空！");
                isWaveInProgress = false;
                return;
            }

            OnWaveStarted?.Invoke(currentWave);
        }

        private IEnumerator SpawnWave(WaveDataEntry waveData)
        {
            if (spawnManager != null)
            {
                yield return spawnManager.SpawnWave(waveData);
            }
            else
            {
                Debug.LogError("MonsterSpawnManager not found!");
                yield return new WaitForSeconds(1f);
            }
        }

        public void OnWaveCompleted()
        {
            isWaveInProgress = false;
            OnWaveCompletedEvent?.Invoke(currentWave);
            
            // 进入策略阶段
            GameManager.Instance?.EnterStrategyPhase();
        }

        private WaveDataEntry GetCurrentWaveData()
        {
            if (isEndlessMode)
            {
                if (endlessWaveData == null || endlessWaveData.Count == 0)
                {
                    Debug.LogError("WaveManager: 无尽模式数据未加载！");
                    return null;
                }
                int loopIndex = ((currentWave - maxMainWaves - 1) % endlessLoopWaves);
                if (loopIndex >= 0 && loopIndex < endlessWaveData.Count)
                    return endlessWaveData[loopIndex];
            }
            else
            {
                if (mainWaveData == null || mainWaveData.Count == 0)
                {
                    Debug.LogError("WaveManager: 主线波次数据未加载！");
                    return null;
                }
                if (currentWave > 0 && currentWave <= mainWaveData.Count)
                    return mainWaveData[currentWave - 1];
            }

            Debug.LogWarning($"WaveManager: 无法获取第 {currentWave} 波的数据");
            return null;
        }

        public event System.Action<int> OnWaveStarted;
        public event System.Action<int> OnWaveCompletedEvent;
    }
}

