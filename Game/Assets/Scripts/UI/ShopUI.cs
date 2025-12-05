using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Shop;
using EmberKeepers.Core;
using System.Collections.Generic;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 商店UI - 显示可购买的英雄、属性书、装备
    /// </summary>
    public class ShopUI : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private Button heroTabButton;
        [SerializeField] private Button attributeTabButton;
        [SerializeField] private Button equipmentTabButton;

        [Header("Content Panels")]
        [SerializeField] private GameObject heroPanel;
        [SerializeField] private GameObject attributePanel;
        [SerializeField] private GameObject equipmentPanel;

        [Header("Hero Shop")]
        [SerializeField] private Transform heroItemContainer;
        [SerializeField] private GameObject heroItemPrefab;

        [Header("Attribute Shop")]
        [SerializeField] private Button strButton;
        [SerializeField] private Button agiButton;
        [SerializeField] private Button intButton;
        [SerializeField] private Button elementButton;
        [SerializeField] private TextMeshProUGUI attributeCostText;

        [Header("Equipment Shop")]
        [SerializeField] private Transform equipmentItemContainer;
        [SerializeField] private GameObject equipmentItemPrefab;
        [SerializeField] private Button refreshEquipmentButton;

        [Header("Selected Hero")]
        [SerializeField] private Transform selectedHeroDropdown;

        private ShopSystem shopSystem;
        private ResourceManager resourceManager;
        private HeroManager heroManager;
        private List<GameObject> currentShopItems = new List<GameObject>();

        private void Start()
        {
            shopSystem = FindFirstObjectByType<ShopSystem>();
            resourceManager = FindFirstObjectByType<ResourceManager>();
            heroManager = FindFirstObjectByType<HeroManager>();

            SetupButtons();
            ShowHeroTab();
        }

        private void SetupButtons()
        {
            if (heroTabButton)
                heroTabButton.onClick.AddListener(ShowHeroTab);
            
            if (attributeTabButton)
                attributeTabButton.onClick.AddListener(ShowAttributeTab);
            
            if (equipmentTabButton)
                equipmentTabButton.onClick.AddListener(ShowEquipmentTab);

            if (refreshEquipmentButton)
                refreshEquipmentButton.onClick.AddListener(RefreshEquipmentShop);

            SetupAttributeButtons();
        }

        private void SetupAttributeButtons()
        {
            if (strButton)
                strButton.onClick.AddListener(() => PurchaseAttributeBook(Heroes.AttributeType.Strength));
            
            if (agiButton)
                agiButton.onClick.AddListener(() => PurchaseAttributeBook(Heroes.AttributeType.Agility));
            
            if (intButton)
                intButton.onClick.AddListener(() => PurchaseAttributeBook(Heroes.AttributeType.Intelligence));
            
            if (elementButton)
                elementButton.onClick.AddListener(() => PurchaseAttributeBook(Heroes.AttributeType.ElementMastery));

            if (attributeCostText)
                attributeCostText.text = "消耗: 50 金币";
        }

        public void ShowHeroTab()
        {
            if (heroPanel) heroPanel.SetActive(true);
            if (attributePanel) attributePanel.SetActive(false);
            if (equipmentPanel) equipmentPanel.SetActive(false);

            RefreshHeroShop();
        }

        public void ShowAttributeTab()
        {
            if (heroPanel) heroPanel.SetActive(false);
            if (attributePanel) attributePanel.SetActive(true);
            if (equipmentPanel) equipmentPanel.SetActive(false);
        }

        public void ShowEquipmentTab()
        {
            if (heroPanel) heroPanel.SetActive(false);
            if (attributePanel) attributePanel.SetActive(false);
            if (equipmentPanel) equipmentPanel.SetActive(true);

            RefreshEquipmentShop();
        }

        private void RefreshHeroShop()
        {
            ClearShopItems();

            if (shopSystem == null) return;

            // TODO: 从ShopSystem获取可购买的英雄列表
            // 暂时显示示例
        }

        private void RefreshEquipmentShop()
        {
            ClearShopItems();

            if (shopSystem == null) return;

            // TODO: 从ShopSystem获取可购买的装备列表
            // 暂时显示示例
        }

        private void ClearShopItems()
        {
            foreach (var item in currentShopItems)
            {
                if (item != null)
                    Destroy(item);
            }
            currentShopItems.Clear();
        }

        private void PurchaseAttributeBook(Heroes.AttributeType type)
        {
            if (shopSystem == null || resourceManager == null) return;

            // 获取当前选中的英雄
            var selectedHero = GetSelectedHero();
            if (selectedHero == null)
            {
                Debug.LogWarning("请先选择一个英雄！");
                return;
            }

            // 尝试购买属性书
            ShopAttributeBook book = new ShopAttributeBook
            {
                type = type,
                cost = 50
            };

            if (shopSystem.PurchaseAttributeBook(book))
            {
                // 应用属性加成
                selectedHero.AddAttribute(type, 1);
                Debug.Log($"{selectedHero.HeroName} 获得了 +1 {type}！");
            }
            else
            {
                Debug.LogWarning("金币不足！");
            }
        }

        private Heroes.HeroBase GetSelectedHero()
        {
            // TODO: 从下拉菜单获取选中的英雄
            // 暂时返回第一个部署的英雄
            if (heroManager != null && heroManager.DeployedHeroes.Count > 0)
            {
                return heroManager.DeployedHeroes[0];
            }
            return null;
        }
    }
}

