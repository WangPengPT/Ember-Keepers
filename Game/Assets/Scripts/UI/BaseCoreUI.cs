using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Base;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 基地核心UI - 显示基地状态和升级选项
    /// </summary>
    public class BaseCoreUI : MonoBehaviour
    {
        [Header("Core Info")]
        [SerializeField] private GameObject coreInfoGroup;
        [SerializeField] private Image coreIcon;
        [SerializeField] private Slider healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image healthIcon;

        [Header("Upgrades")]
        [SerializeField] private GameObject upgradesGroup;
        [SerializeField] private Button upgradeHealthButton;
        [SerializeField] private TextMeshProUGUI upgradeHealthCostText;
        [SerializeField] private Button upgradeEnergyButton;
        [SerializeField] private TextMeshProUGUI upgradeEnergyCostText;
        [SerializeField] private Button upgradeElementButton;
        [SerializeField] private TextMeshProUGUI upgradeElementCostText;

        [Header("Active Abilities")]
        [SerializeField] private GameObject abilitiesGroup;
        [SerializeField] private Button purificationStrikeButton;
        [SerializeField] private TextMeshProUGUI purificationStrikeText;
        [SerializeField] private Image purificationStrikeIcon;
        [SerializeField] private Slider purificationStrikeCooldownBar;

        [Header("Defense Systems")]
        [SerializeField] private GameObject defenseGroup;
        [SerializeField] private Toggle healingDroneToggle;
        [SerializeField] private TextMeshProUGUI healingDroneStatusText;

        private BaseCore currentCore;

        public void DisplayBaseCore(BaseCore core)
        {
            currentCore = core;
            UpdateCoreInfo();
            SetupButtons();
        }

        private void Update()
        {
            if (currentCore != null && gameObject.activeInHierarchy)
            {
                UpdateDynamicInfo();
            }
        }

        private void UpdateCoreInfo()
        {
            if (currentCore == null) return;

            UpdateDynamicInfo();

            // 更新防御系统状态
            if (healingDroneToggle)
            {
                healingDroneToggle.isOn = currentCore.hasHealingDrone;
            }
        }

        private void UpdateDynamicInfo()
        {
            if (currentCore == null) return;

            // 生命值
            if (healthBar)
            {
                float healthPercent = currentCore.HealthPercent;
                healthBar.value = healthPercent;
                
                // 根据生命值百分比改变颜色
                var fillImage = healthBar.fillRect?.GetComponent<Image>();
                if (fillImage != null)
                {
                    if (healthPercent > 0.6f)
                        fillImage.color = Color.green;
                    else if (healthPercent > 0.3f)
                        fillImage.color = Color.yellow;
                    else
                        fillImage.color = Color.red;
                }
            }
            if (healthText)
            {
                healthText.text = $"{currentCore.CurrentHealth:F0} / {currentCore.MaxHealth:F0}";
            }

            // 净化冲击状态
            if (purificationStrikeButton)
            {
                purificationStrikeButton.interactable = currentCore.CanUsePurificationStrike;
            }
            if (purificationStrikeText)
            {
                if (currentCore.CanUsePurificationStrike)
                {
                    purificationStrikeText.text = "净化冲击 (可用)";
                    purificationStrikeText.color = Color.green;
                }
                else
                {
                    purificationStrikeText.text = "净化冲击 (冷却中)";
                    purificationStrikeText.color = Color.gray;
                }
            }

            // 治疗无人机状态
            if (healingDroneStatusText)
            {
                if (currentCore.hasHealingDrone)
                {
                    healingDroneStatusText.text = "已激活";
                    healingDroneStatusText.color = Color.green;
                }
                else
                {
                    healingDroneStatusText.text = "未激活";
                    healingDroneStatusText.color = Color.gray;
                }
            }
        }

        private void SetupButtons()
        {
            if (upgradeHealthButton)
            {
                upgradeHealthButton.onClick.RemoveAllListeners();
                upgradeHealthButton.onClick.AddListener(() => currentCore?.UpgradeHealth(100));
            }

            if (upgradeEnergyButton)
            {
                upgradeEnergyButton.onClick.RemoveAllListeners();
                upgradeEnergyButton.onClick.AddListener(() => currentCore?.UpgradeEnergyRegen(100));
            }

            if (upgradeElementButton)
            {
                upgradeElementButton.onClick.RemoveAllListeners();
                upgradeElementButton.onClick.AddListener(() => currentCore?.UpgradeElementAmplification(100));
            }

            if (purificationStrikeButton)
            {
                purificationStrikeButton.onClick.RemoveAllListeners();
                purificationStrikeButton.onClick.AddListener(() => currentCore?.UsePurificationStrike());
            }
        }
    }
}

