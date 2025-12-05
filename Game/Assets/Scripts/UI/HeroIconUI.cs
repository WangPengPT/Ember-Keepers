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
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider energyBar;
        [SerializeField] private TextMeshProUGUI heroNameText;
        [SerializeField] private Image deathOverlay;

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
                healthBar.value = hero.CurrentHealth / hero.MaxHealth;
            }

            // 能量
            if (energyBar)
            {
                energyBar.value = hero.CurrentEnergy / hero.MaxEnergy;
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

