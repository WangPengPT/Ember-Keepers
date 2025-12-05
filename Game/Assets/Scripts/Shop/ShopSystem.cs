using UnityEngine;
using System.Collections.Generic;
using EmberKeepers.Core;
using EmberKeepers.Data;
using EmberKeepers.Heroes;

namespace EmberKeepers.Shop
{
    /// <summary>
    /// 商店系统，处理波次间隙的购买逻辑
    /// </summary>
    public class ShopSystem : MonoBehaviour
    {
        [Header("Shop Settings")]
        [SerializeField] private int heroPurchaseCost = 200;
        [SerializeField] private int attributeBookCost = 50;
        [SerializeField] private int equipmentRefreshCost = 10;

        [Header("Available Items")]
        [SerializeField] private List<ShopHero> availableHeroes = new List<ShopHero>();
        [SerializeField] private List<ShopAttributeBook> availableAttributeBooks = new List<ShopAttributeBook>();
        [SerializeField] private List<ShopEquipment> availableEquipment = new List<ShopEquipment>();

        private ResourceManager resourceManager;

        private void Awake()
        {
            resourceManager = Core.GameManager.Instance?.GetComponent<ResourceManager>();
        }

        /// <summary>
        /// 刷新商店（波次间隙时调用）
        /// </summary>
        public void RefreshShop()
        {
            RefreshHeroes();
            RefreshAttributeBooks();
            RefreshEquipment();
        }

        private void RefreshHeroes()
        {
            availableHeroes.Clear();
            // TODO: 从数据表随机生成可购买的英雄
            // 根据元进度的商店优化，提高稀有英雄出现概率
        }

        private void RefreshAttributeBooks()
        {
            availableAttributeBooks.Clear();
            // 生成4种属性书
            availableAttributeBooks.Add(new ShopAttributeBook { type = AttributeType.Strength, cost = attributeBookCost });
            availableAttributeBooks.Add(new ShopAttributeBook { type = AttributeType.Agility, cost = attributeBookCost });
            availableAttributeBooks.Add(new ShopAttributeBook { type = AttributeType.Intelligence, cost = attributeBookCost });
            availableAttributeBooks.Add(new ShopAttributeBook { type = AttributeType.ElementMastery, cost = attributeBookCost });
        }

        private void RefreshEquipment()
        {
            availableEquipment.Clear();
            // TODO: 从装备池随机生成装备
            // 根据元进度的商店优化，提高稀有装备出现概率
        }

        /// <summary>
        /// 购买英雄
        /// </summary>
        public bool PurchaseHero(ShopHero shopHero)
        {
            if (resourceManager == null || !resourceManager.SpendGold(shopHero.cost))
                return false;

            // TODO: 创建英雄并添加到英雄池
            OnHeroPurchased?.Invoke(shopHero);
            return true;
        }

        /// <summary>
        /// 购买属性书
        /// </summary>
        public bool PurchaseAttributeBook(ShopAttributeBook book)
        {
            if (resourceManager == null || !resourceManager.SpendGold(book.cost))
                return false;

            // TODO: 添加到背包或直接使用
            OnAttributeBookPurchased?.Invoke(book);
            return true;
        }

        /// <summary>
        /// 购买装备
        /// </summary>
        public bool PurchaseEquipment(ShopEquipment equipment)
        {
            if (resourceManager == null || !resourceManager.SpendGold(equipment.cost))
                return false;

            OnEquipmentPurchased?.Invoke(equipment);
            return true;
        }

        public event System.Action<ShopHero> OnHeroPurchased;
        public event System.Action<ShopAttributeBook> OnAttributeBookPurchased;
        public event System.Action<ShopEquipment> OnEquipmentPurchased;
    }

    [System.Serializable]
    public class ShopHero
    {
        public string heroId;
        public int cost;
    }

    [System.Serializable]
    public class ShopAttributeBook
    {
        public AttributeType type;
        public int cost;
    }

    [System.Serializable]
    public class ShopEquipment
    {
        public string equipmentId;
        public int cost;
    }
}

