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
        [SerializeField] private Image heroPortrait;
        [SerializeField] private TextMeshProUGUI heroNameText;
        [SerializeField] private TextMeshProUGUI heroClassText;
        [SerializeField] private TextMeshProUGUI elementTypeText;
        [SerializeField] private Image elementTypeIcon;

        [Header("Stats")]
        [SerializeField] private GameObject statsGroup;
        [SerializeField] private Slider healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image healthIcon;
        [SerializeField] private Slider energyBar;
        [SerializeField] private TextMeshProUGUI energyText;
        [SerializeField] private Image energyIcon;

        [Header("Attributes")]
        [SerializeField] private GameObject attributesGroup;
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private Image strengthIcon;
        [SerializeField] private TextMeshProUGUI agilityText;
        [SerializeField] private Image agilityIcon;
        [SerializeField] private TextMeshProUGUI intelligenceText;
        [SerializeField] private Image intelligenceIcon;
        [SerializeField] private TextMeshProUGUI elementMasteryText;
        [SerializeField] private Image elementMasteryIcon;

        [Header("Combat Stats")]
        [SerializeField] private GameObject combatStatsGroup;
        [SerializeField] private TextMeshProUGUI attackDamageText;
        [SerializeField] private Image attackDamageIcon;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private Image attackSpeedIcon;
        [SerializeField] private TextMeshProUGUI physicalDefenseText;
        [SerializeField] private Image physicalDefenseIcon;
        [SerializeField] private TextMeshProUGUI elementResistanceText;
        [SerializeField] private Image elementResistanceIcon;

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
            if (heroClassText) heroClassText.text = $"职业: {currentHero.HeroClass}";
            if (elementTypeText) elementTypeText.text = $"元素: {currentHero.ElementType}";

            // 属性 - 使用更简洁的格式
            if (strengthText) strengthText.text = $"{currentHero.Strength}";
            if (agilityText) agilityText.text = $"{currentHero.Agility}";
            if (intelligenceText) intelligenceText.text = $"{currentHero.Intelligence}";
            if (elementMasteryText) elementMasteryText.text = $"{currentHero.ElementMastery}";

            // 战斗属性 - 使用更简洁的格式
            if (attackDamageText) attackDamageText.text = $"{currentHero.AttackDamage:F1}";
            if (attackSpeedText) attackSpeedText.text = $"{currentHero.AttackSpeed:F2}";
            if (physicalDefenseText) physicalDefenseText.text = $"{currentHero.physicalDefense:F1}";
            if (elementResistanceText) elementResistanceText.text = $"{currentHero.elementResistance:F1}";

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
                float healthPercent = currentHero.CurrentHealth / currentHero.MaxHealth;
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
                healthText.text = $"{currentHero.CurrentHealth:F0} / {currentHero.MaxHealth:F0}";
            }

            // 能量
            if (energyBar)
            {
                energyBar.value = currentHero.CurrentEnergy / currentHero.MaxEnergy;
                
                // 能量条使用蓝色
                var fillImage = energyBar.fillRect?.GetComponent<Image>();
                if (fillImage != null)
                {
                    fillImage.color = new Color(0.3f, 0.6f, 1f); // 蓝色
                }
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

