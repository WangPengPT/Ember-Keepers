using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Heroes;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 英雄详情UI - 显示英雄属性、装备、技能等信息
    /// </summary>
    public class HeroDetailUI : MonoBehaviour
    {
        [Header("Hero Info")]
        [SerializeField] private TextMeshProUGUI heroNameText;
        [SerializeField] private TextMeshProUGUI heroClassText;
        [SerializeField] private TextMeshProUGUI elementTypeText;

        [Header("Stats")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider energyBar;
        [SerializeField] private TextMeshProUGUI energyText;

        [Header("Attributes")]
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI agilityText;
        [SerializeField] private TextMeshProUGUI intelligenceText;
        [SerializeField] private TextMeshProUGUI elementMasteryText;

        [Header("Combat Stats")]
        [SerializeField] private TextMeshProUGUI attackDamageText;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI physicalDefenseText;
        [SerializeField] private TextMeshProUGUI elementResistanceText;

        [Header("Equipment Slots")]
        [SerializeField] private Image[] equipmentSlotImages;

        [Header("Skill")]
        [SerializeField] private Image skillIcon;
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private Slider skillCooldownBar;

        private HeroBase currentHero;

        public void DisplayHero(HeroBase hero)
        {
            currentHero = hero;
            UpdateHeroInfo();
        }

        private void Update()
        {
            if (currentHero != null && gameObject.activeInHierarchy)
            {
                UpdateDynamicInfo();
            }
        }

        private void UpdateHeroInfo()
        {
            if (currentHero == null) return;

            // 基础信息
            if (heroNameText) heroNameText.text = currentHero.HeroName;
            if (heroClassText) heroClassText.text = currentHero.HeroClass.ToString();
            if (elementTypeText) elementTypeText.text = currentHero.ElementType.ToString();

            // 属性
            if (strengthText) strengthText.text = $"力量: {currentHero.Strength}";
            if (agilityText) agilityText.text = $"敏捷: {currentHero.Agility}";
            if (intelligenceText) intelligenceText.text = $"智力: {currentHero.Intelligence}";
            if (elementMasteryText) elementMasteryText.text = $"元素专精: {currentHero.ElementMastery}";

            // 战斗属性
            if (attackDamageText) attackDamageText.text = $"攻击力: {currentHero.AttackDamage:F1}";
            if (attackSpeedText) attackSpeedText.text = $"攻速: {currentHero.AttackSpeed:F2}";
            if (physicalDefenseText) physicalDefenseText.text = $"物防: {currentHero.physicalDefense:F1}";
            if (elementResistanceText) elementResistanceText.text = $"元抗: {currentHero.elementResistance:F1}";

            // 技能
            if (currentHero.ActiveSkill != null)
            {
                if (skillNameText) skillNameText.text = currentHero.ActiveSkill.SkillName;
            }

            UpdateDynamicInfo();
        }

        private void UpdateDynamicInfo()
        {
            if (currentHero == null) return;

            // 生命值
            if (healthBar)
            {
                healthBar.value = currentHero.CurrentHealth / currentHero.MaxHealth;
            }
            if (healthText)
            {
                healthText.text = $"{currentHero.CurrentHealth:F0} / {currentHero.MaxHealth:F0}";
            }

            // 能量
            if (energyBar)
            {
                energyBar.value = currentHero.CurrentEnergy / currentHero.MaxEnergy;
            }
            if (energyText)
            {
                energyText.text = $"{currentHero.CurrentEnergy:F0} / {currentHero.MaxEnergy:F0}";
            }

            // 技能冷却
            if (skillCooldownBar && currentHero.ActiveSkill != null)
            {
                // TODO: 获取技能冷却进度
                skillCooldownBar.value = currentHero.ActiveSkill.IsReady ? 1f : 0.5f;
            }
        }
    }
}

