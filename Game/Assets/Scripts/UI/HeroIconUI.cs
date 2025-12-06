using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Heroes;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 英雄图标UI - 显示在底部面板的已部署英雄列表中
    /// </summary>
    public class HeroIconUI : MonoBehaviour
    {
        [SerializeField] private Image heroIcon;
        [SerializeField] private Image heroIconBackground;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider energyBar;
        [SerializeField] private TextMeshProUGUI heroNameText;
        [SerializeField] private Image deathOverlay;
        [SerializeField] private Button iconButton;

        private HeroBase hero;

        public HeroBase Hero => hero;

        public void Initialize(HeroBase heroBase)
        {
            hero = heroBase;
            
            if (heroNameText) heroNameText.text = hero.HeroName;
            
            // 订阅事件
            if (hero != null)
            {
                hero.OnHealthChanged += UpdateHealth;
                hero.OnHeroDied += OnHeroDied;
                hero.OnHeroRevived += OnHeroRevived;
            }

            UpdateUI();
        }

        private void Update()
        {
            if (hero != null && !hero.IsDead)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (hero == null) return;

            // 生命值
            if (healthBar)
            {
                float healthPercent = hero.CurrentHealth / hero.MaxHealth;
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

            // 能量
            if (energyBar)
            {
                energyBar.value = hero.CurrentEnergy / hero.MaxEnergy;
                
                // 能量条使用蓝色
                var fillImage = energyBar.fillRect?.GetComponent<Image>();
                if (fillImage != null)
                {
                    fillImage.color = new Color(0.3f, 0.6f, 1f); // 蓝色
                }
            }
        }

        private void UpdateHealth(float current, float max)
        {
            if (healthBar)
            {
                healthBar.value = current / max;
            }
        }

        private void OnHeroDied(HeroBase deadHero)
        {
            if (deathOverlay)
            {
                deathOverlay.gameObject.SetActive(true);
            }
        }

        private void OnHeroRevived(HeroBase revivedHero)
        {
            if (deathOverlay)
            {
                deathOverlay.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (hero != null)
            {
                hero.OnHealthChanged -= UpdateHealth;
                hero.OnHeroDied -= OnHeroDied;
                hero.OnHeroRevived -= OnHeroRevived;
            }
        }
    }
}

