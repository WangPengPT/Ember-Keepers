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
        [SerializeField] private GameObject coreHealthGroup;
        [SerializeField] private Slider coreHealthBar;
        [SerializeField] private TextMeshProUGUI coreHealthText;
        [SerializeField] private Image coreHealthIcon;

        [Header("Wave Info")]
        [SerializeField] private GameObject waveInfoGroup;
        [SerializeField] private TextMeshProUGUI waveNumberText;
        [SerializeField] private TextMeshProUGUI waveStatusText;
        [SerializeField] private Image waveStatusIcon;

        [Header("Resources")]
        [SerializeField] private GameObject resourcesGroup;
        [SerializeField] private GameObject goldGroup;
        [SerializeField] private Image goldIcon;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private GameObject stardustGroup;
        [SerializeField] private Image stardustIcon;
        [SerializeField] private TextMeshProUGUI stardustShardsText;
        [SerializeField] private GameObject starfireGroup;
        [SerializeField] private Image starfireIcon;
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
                
                // 根据生命值百分比改变颜色
                var fillImage = coreHealthBar.fillRect?.GetComponent<Image>();
                if (fillImage != null)
                {
                    float percent = current / max;
                    if (percent > 0.6f)
                        fillImage.color = Color.green;
                    else if (percent > 0.3f)
                        fillImage.color = Color.yellow;
                    else
                        fillImage.color = Color.red;
                }
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
                waveStatusText.color = new Color(1f, 0.3f, 0.3f); // 更柔和的红色
            }

            if (waveStatusIcon != null)
            {
                waveStatusIcon.color = new Color(1f, 0.3f, 0.3f);
            }
        }

        private void OnWaveCompleted(int waveNumber)
        {
            if (waveStatusText != null)
            {
                waveStatusText.text = "准备阶段";
                waveStatusText.color = new Color(0.3f, 1f, 0.3f); // 更柔和的绿色
            }

            if (waveStatusIcon != null)
            {
                waveStatusIcon.color = new Color(0.3f, 1f, 0.3f);
            }
        }

        private void UpdateGold(int amount)
        {
            if (goldText != null)
            {
                goldText.text = FormatNumber(amount);
            }
        }

        private void UpdateStardustShards(int amount)
        {
            if (stardustShardsText != null)
            {
                stardustShardsText.text = FormatNumber(amount);
            }
        }

        private void UpdateStarfireEssence(int amount)
        {
            if (starfireEssenceText != null)
            {
                starfireEssenceText.text = FormatNumber(amount);
            }
        }

        /// <summary>
        /// 格式化数字显示（添加千位分隔符）
        /// </summary>
        private string FormatNumber(int number)
        {
            return number.ToString("N0");
        }
    }
}

