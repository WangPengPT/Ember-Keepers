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

            WaveData waveData = GetCurrentWaveData();
            if (waveData != null)
            {
                StartCoroutine(SpawnWave(waveData));
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
                int loopIndex = ((currentWave - maxMainWaves - 1) % endlessLoopWaves);
                return endlessWaveData[loopIndex];
            }
            else
            {
                if (currentWave > 0 && currentWave <= mainWaveData.Count)
                    return mainWaveData[currentWave - 1];
            }

            return null;
        }

        public event System.Action<int> OnWaveStarted;
        public event System.Action<int> OnWaveCompletedEvent;
    }
}

