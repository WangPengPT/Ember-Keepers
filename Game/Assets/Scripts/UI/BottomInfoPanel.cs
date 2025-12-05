using UnityEngine;
using UnityEngine.UI;
using EmberKeepers.Heroes;
using EmberKeepers.Core;
using System.Collections.Generic;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 底部信息面板 - RTS风格，根据点击对象切换显示内容
    /// </summary>
    public class BottomInfoPanel : MonoBehaviour
    {
        [Header("Panel Sections")]
        [SerializeField] private GameObject defaultPanel;
        [SerializeField] private GameObject heroDetailPanel;
        [SerializeField] private GameObject baseCorePanel;

        [Header("Default Panel - Deployed Heroes")]
        [SerializeField] private Transform heroIconContainer;
        [SerializeField] private GameObject heroIconPrefab;

        [Header("Hero Detail Panel")]
        [SerializeField] private HeroDetailUI heroDetailUI;

        [Header("Base Core Panel")]
        [SerializeField] private BaseCoreUI baseCoreUI;

        private HeroManager heroManager;
        private List<HeroIconUI> heroIcons = new List<HeroIconUI>();

        private void Start()
        {
            heroManager = FindFirstObjectByType<HeroManager>();
            
            if (heroManager != null)
            {
                heroManager.OnHeroDeployed += OnHeroDeployed;
                heroManager.OnHeroUndeployed += OnHeroUndeployed;
            }

            ShowDefaultPanel();
        }

        private void Update()
        {
            // 检测点击
            if (Input.GetMouseButtonDown(0))
            {
                DetectClick();
            }
        }

        private void DetectClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 点击英雄
                HeroBase hero = hit.collider.GetComponent<HeroBase>();
                if (hero != null)
                {
                    ShowHeroDetailPanel(hero);
                    return;
                }

                // 点击基地核心
                BaseCore core = hit.collider.GetComponent<BaseCore>();
                if (core != null)
                {
                    ShowBaseCorePanel(core);
                    return;
                }
            }

            // 点击空地
            ShowDefaultPanel();
        }

        /// <summary>
        /// 显示默认面板（已部署英雄列表）
        /// </summary>
        public void ShowDefaultPanel()
        {
            if (defaultPanel) defaultPanel.SetActive(true);
            if (heroDetailPanel) heroDetailPanel.SetActive(false);
            if (baseCorePanel) baseCorePanel.SetActive(false);
        }

        /// <summary>
        /// 显示英雄详情面板
        /// </summary>
        public void ShowHeroDetailPanel(HeroBase hero)
        {
            if (defaultPanel) defaultPanel.SetActive(false);
            if (heroDetailPanel) heroDetailPanel.SetActive(true);
            if (baseCorePanel) baseCorePanel.SetActive(false);

            if (heroDetailUI != null)
            {
                heroDetailUI.DisplayHero(hero);
            }
        }

        /// <summary>
        /// 显示基地核心面板
        /// </summary>
        public void ShowBaseCorePanel(BaseCore core)
        {
            if (defaultPanel) defaultPanel.SetActive(false);
            if (heroDetailPanel) heroDetailPanel.SetActive(false);
            if (baseCorePanel) baseCorePanel.SetActive(true);

            if (baseCoreUI != null)
            {
                baseCoreUI.DisplayBaseCore(core);
            }
        }

        private void OnHeroDeployed(HeroBase hero)
        {
            // 添加英雄图标到列表
            if (heroIconPrefab != null && heroIconContainer != null)
            {
                GameObject iconObj = Instantiate(heroIconPrefab, heroIconContainer);
                HeroIconUI iconUI = iconObj.GetComponent<HeroIconUI>();
                if (iconUI != null)
                {
                    iconUI.Initialize(hero);
                    heroIcons.Add(iconUI);
                }
            }
        }

        private void OnHeroUndeployed(HeroBase hero)
        {
            // 移除英雄图标
            HeroIconUI iconToRemove = heroIcons.Find(icon => icon.Hero == hero);
            if (iconToRemove != null)
            {
                heroIcons.Remove(iconToRemove);
                Destroy(iconToRemove.gameObject);
            }
        }
    }
}

