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
        [SerializeField] private Slider healthBar;
        [SerializeField] private TextMeshProUGUI healthText;

        [Header("Upgrades")]
        [SerializeField] private Button upgradeHealthButton;
        [SerializeField] private Button upgradeEnergyButton;
        [SerializeField] private Button upgradeElementButton;

        [Header("Active Abilities")]
        [SerializeField] private Button purificationStrikeButton;
        [SerializeField] private TextMeshProUGUI purificationStrikeText;

        [Header("Defense Systems")]
        [SerializeField] private Toggle healingDroneToggle;

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
                healthBar.value = currentCore.HealthPercent;
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
                }
                else
                {
                    purificationStrikeText.text = "净化冲击 (冷却中)";
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

