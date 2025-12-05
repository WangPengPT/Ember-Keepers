using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Core;
using EmberKeepers.Base;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 顶部信息栏 - 显示基地核心状态、波次信息、资源
    /// </summary>
    public class TopInfoPanel : MonoBehaviour
    {
        [Header("Base Core Info")]
        [SerializeField] private Slider coreHealthBar;
        [SerializeField] private TextMeshProUGUI coreHealthText;

        [Header("Wave Info")]
        [SerializeField] private TextMeshProUGUI waveNumberText;
        [SerializeField] private TextMeshProUGUI waveStatusText;

        [Header("Resources")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI stardustShardsText;
        [SerializeField] private TextMeshProUGUI starfireEssenceText;

        private ResourceManager resourceManager;
        private WaveManager waveManager;
        private BaseCore baseCore;

        private void Start()
        {
            InitializeReferences();
            SubscribeToEvents();
            UpdateAllInfo();
        }

        private void InitializeReferences()
        {
            resourceManager = FindFirstObjectByType<ResourceManager>();
            waveManager = FindFirstObjectByType<WaveManager>();
            
            GameObject coreObj = GameObject.FindGameObjectWithTag("BaseCore");
            if (coreObj != null)
                baseCore = coreObj.GetComponent<BaseCore>();
        }

        private void SubscribeToEvents()
        {
            if (resourceManager != null)
            {
                resourceManager.OnGoldChanged += UpdateGold;
                resourceManager.OnStardustShardsChanged += UpdateStardustShards;
                resourceManager.OnStarfireEssenceChanged += UpdateStarfireEssence;
            }

            if (waveManager != null)
            {
                waveManager.OnWaveStarted += UpdateWaveInfo;
                waveManager.OnWaveCompletedEvent += OnWaveCompleted;
            }

            if (baseCore != null)
            {
                baseCore.OnHealthChanged += UpdateCoreHealth;
            }
        }

        private void UpdateAllInfo()
        {
            if (resourceManager != null)
            {
                UpdateGold(resourceManager.Gold);
                UpdateStardustShards(resourceManager.StardustShards);
                UpdateStarfireEssence(resourceManager.StarfireEssence);
            }

            if (waveManager != null)
            {
                UpdateWaveInfo(waveManager.CurrentWave);
            }

            if (baseCore != null)
            {
                UpdateCoreHealth(baseCore.CurrentHealth, baseCore.MaxHealth);
            }
        }

        private void UpdateCoreHealth(float current, float max)
        {
            if (coreHealthBar != null)
            {
                coreHealthBar.value = current / max;
            }

            if (coreHealthText != null)
            {
                coreHealthText.text = $"{current:F0} / {max:F0}";
            }
        }

        private void UpdateWaveInfo(int waveNumber)
        {
            if (waveNumberText != null)
            {
                if (waveManager != null && waveManager.IsEndlessMode)
                {
                    waveNumberText.text = $"无尽模式 - 第 {waveNumber} 波";
                }
                else
                {
                    waveNumberText.text = $"第 {waveNumber} 波";
                }
            }

            if (waveStatusText != null)
            {
                waveStatusText.text = "战斗中";
                waveStatusText.color = Color.red;
            }
        }

        private void OnWaveCompleted(int waveNumber)
        {
            if (waveStatusText != null)
            {
                waveStatusText.text = "准备阶段";
                waveStatusText.color = Color.green;
            }
        }

        private void UpdateGold(int amount)
        {
            if (goldText != null)
            {
                goldText.text = $"金币: {amount}";
            }
        }

        private void UpdateStardustShards(int amount)
        {
            if (stardustShardsText != null)
            {
                stardustShardsText.text = $"星火碎片: {amount}";
            }
        }

        private void UpdateStarfireEssence(int amount)
        {
            if (starfireEssenceText != null)
            {
                starfireEssenceText.text = $"星火精粹: {amount}";
            }
        }
    }
}

