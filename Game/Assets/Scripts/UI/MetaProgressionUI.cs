using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.MetaProgression;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 元进度UI - 显示局外永久升级界面
    /// </summary>
    public class MetaProgressionUI : MonoBehaviour
    {
        [Header("Resource Display")]
        [SerializeField] private TextMeshProUGUI essenceText;

        [Header("Hero Bloodline")]
        [SerializeField] private TextMeshProUGUI bloodlineLevel;
        [SerializeField] private TextMeshProUGUI bloodlineEffect;
        [SerializeField] private Button bloodlineUpgradeButton;
        [SerializeField] private TextMeshProUGUI bloodlineCost;

        [Header("Initial Gold")]
        [SerializeField] private TextMeshProUGUI initialGoldLevel;
        [SerializeField] private TextMeshProUGUI initialGoldEffect;
        [SerializeField] private Button initialGoldUpgradeButton;
        [SerializeField] private TextMeshProUGUI initialGoldCost;

        [Header("Shop Optimization")]
        [SerializeField] private TextMeshProUGUI shopOptLevel;
        [SerializeField] private TextMeshProUGUI shopOptEffect;
        [SerializeField] private Button shopOptUpgradeButton;
        [SerializeField] private TextMeshProUGUI shopOptCost;

        [Header("Essence Efficiency")]
        [SerializeField] private TextMeshProUGUI essenceEffLevel;
        [SerializeField] private TextMeshProUGUI essenceEffEffect;
        [SerializeField] private Button essenceEffUpgradeButton;
        [SerializeField] private TextMeshProUGUI essenceEffCost;

        private MetaProgressionManager metaManager;

        private void Start()
        {
            metaManager = MetaProgressionManager.Instance;
            SetupButtons();
            UpdateAllInfo();
        }

        private void OnEnable()
        {
            UpdateAllInfo();
        }

        private void SetupButtons()
        {
            if (bloodlineUpgradeButton)
                bloodlineUpgradeButton.onClick.AddListener(UpgradeBloodline);
            
            if (initialGoldUpgradeButton)
                initialGoldUpgradeButton.onClick.AddListener(UpgradeInitialGold);
            
            if (shopOptUpgradeButton)
                shopOptUpgradeButton.onClick.AddListener(UpgradeShopOpt);
            
            if (essenceEffUpgradeButton)
                essenceEffUpgradeButton.onClick.AddListener(UpgradeEssenceEff);
        }

        private void UpdateAllInfo()
        {
            if (metaManager == null) return;

            UpdateEssenceDisplay();
            UpdateBloodlineInfo();
            UpdateInitialGoldInfo();
            UpdateShopOptInfo();
            UpdateEssenceEffInfo();
        }

        private void UpdateEssenceDisplay()
        {
            if (essenceText && metaManager != null)
            {
                essenceText.text = $"星火精粹: {metaManager.StarfireEssence}";
            }
        }

        private void UpdateBloodlineInfo()
        {
            if (metaManager == null) return;

            int level = metaManager.progressionData.heroBloodlineUpgradeLevel;
            float bonus = metaManager.GetHeroHealthBonus();
            int cost = CalculateCost(level);

            if (bloodlineLevel) bloodlineLevel.text = $"等级 {level}";
            if (bloodlineEffect) bloodlineEffect.text = $"所有英雄初始生命值 +{bonus}";
            if (bloodlineCost) bloodlineCost.text = $"消耗: {cost} 精粹";
            if (bloodlineUpgradeButton) bloodlineUpgradeButton.interactable = metaManager.StarfireEssence >= cost;
        }

        private void UpdateInitialGoldInfo()
        {
            if (metaManager == null) return;

            int level = metaManager.progressionData.initialGoldUpgradeLevel;
            int bonus = metaManager.GetInitialGoldBonus();
            int cost = CalculateCost(level);

            if (initialGoldLevel) initialGoldLevel.text = $"等级 {level}";
            if (initialGoldEffect) initialGoldEffect.text = $"初始金币 +{bonus}";
            if (initialGoldCost) initialGoldCost.text = $"消耗: {cost} 精粹";
            if (initialGoldUpgradeButton) initialGoldUpgradeButton.interactable = metaManager.StarfireEssence >= cost;
        }

        private void UpdateShopOptInfo()
        {
            if (metaManager == null) return;

            int level = metaManager.progressionData.shopOptimizationUpgradeLevel;
            float bonus = metaManager.GetShopRareEquipmentChanceBonus() * 100f;
            int cost = CalculateCost(level);

            if (shopOptLevel) shopOptLevel.text = $"等级 {level}";
            if (shopOptEffect) shopOptEffect.text = $"稀有装备概率 +{bonus:F0}%";
            if (shopOptCost) shopOptCost.text = $"消耗: {cost} 精粹";
            if (shopOptUpgradeButton) shopOptUpgradeButton.interactable = metaManager.StarfireEssence >= cost;
        }

        private void UpdateEssenceEffInfo()
        {
            if (metaManager == null) return;

            int level = metaManager.progressionData.essenceEfficiencyUpgradeLevel;
            float bonus = (metaManager.GetEssenceEfficiencyBonus() - 1f) * 100f;
            int cost = CalculateCost(level);

            if (essenceEffLevel) essenceEffLevel.text = $"等级 {level}";
            if (essenceEffEffect) essenceEffEffect.text = $"精粹获取效率 +{bonus:F0}%";
            if (essenceEffCost) essenceEffCost.text = $"消耗: {cost} 精粹";
            if (essenceEffUpgradeButton) essenceEffUpgradeButton.interactable = metaManager.StarfireEssence >= cost;
        }

        private int CalculateCost(int currentLevel)
        {
            // 基础消耗50，每级递增25
            return 50 + currentLevel * 25;
        }

        private void UpgradeBloodline()
        {
            if (metaManager == null) return;

            int cost = CalculateCost(metaManager.progressionData.heroBloodlineUpgradeLevel);
            if (metaManager.UpgradeHeroBloodline(cost))
            {
                UpdateAllInfo();
                Debug.Log("英雄血脉强化升级成功！");
            }
        }

        private void UpgradeInitialGold()
        {
            if (metaManager == null) return;

            int cost = CalculateCost(metaManager.progressionData.initialGoldUpgradeLevel);
            if (metaManager.UpgradeInitialGold(cost))
            {
                UpdateAllInfo();
                Debug.Log("初始金币储备升级成功！");
            }
        }

        private void UpgradeShopOpt()
        {
            if (metaManager == null) return;

            int cost = CalculateCost(metaManager.progressionData.shopOptimizationUpgradeLevel);
            if (metaManager.UpgradeShopOptimization(cost))
            {
                UpdateAllInfo();
                Debug.Log("商店优化升级成功！");
            }
        }

        private void UpgradeEssenceEff()
        {
            if (metaManager == null) return;

            int cost = CalculateCost(metaManager.progressionData.essenceEfficiencyUpgradeLevel);
            if (metaManager.UpgradeEssenceEfficiency(cost))
            {
                UpdateAllInfo();
                Debug.Log("精粹获取效率升级成功！");
            }
        }
    }
}

