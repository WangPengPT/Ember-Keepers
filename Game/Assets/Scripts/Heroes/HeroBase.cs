using UnityEngine;
using EmberKeepers.Data;
using EmberKeepers.Equipment;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 英雄基类，所有英雄的基础
    /// </summary>
    public class HeroBase : MonoBehaviour
    {
        [Header("Hero Identity")]
        [SerializeField] protected string heroId;
        [SerializeField] protected string heroName;
        [SerializeField] protected ElementType elementType;
        [SerializeField] protected HeroClass heroClass;

        [Header("Attributes")]
        [SerializeField] protected int strength = 0;
        [SerializeField] protected int agility = 0;
        [SerializeField] protected int intelligence = 0;
        [SerializeField] protected int elementMastery = 0;

        [Header("Combat Stats")]
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth = 100f;
        [SerializeField] protected float baseAttackDamage = 10f;
        [SerializeField] protected float attackSpeed = 1f;
        [SerializeField] protected float moveSpeed = 5f;
        [SerializeField] protected float physicalDefense = 0f;
        [SerializeField] protected float elementResistance = 0f;

        [Header("Energy System")]
        [SerializeField] protected float maxEnergy = 100f;
        [SerializeField] protected float currentEnergy = 0f;
        [SerializeField] protected float energyRegenRate = 5f;

        [Header("Equipment")]
        [SerializeField] protected EquipmentSlot[] equipmentSlots = new EquipmentSlot[6];

        [Header("Skill")]
        [SerializeField] protected Skills.HeroSkillBase activeSkill;

        [Header("Status")]
        [SerializeField] protected bool isDeployed = false;
        [SerializeField] protected bool isDead = false;
        [SerializeField] public float deathTimer = 0f; // 用于复活系统

        public string HeroId => heroId;
        public string HeroName => heroName;
        public ElementType ElementType => elementType;
        public HeroClass HeroClass => heroClass;
        public bool IsDead => isDead;
        public bool IsDeployed => isDeployed;

        public int Strength => strength;
        public int Agility => agility;
        public int Intelligence => intelligence;
        public int ElementMastery => elementMastery;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float CurrentEnergy => currentEnergy;
        public float MaxEnergy => maxEnergy;
        public float AttackDamage => baseAttackDamage;
        public float AttackSpeed => attackSpeed;

        protected virtual void Awake()
        {
            InitializeEquipmentSlots();
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            currentEnergy = 0f;
        }

        protected virtual void Update()
        {
            if (isDead)
            {
                UpdateDeathTimer();
                return;
            }

            if (isDeployed)
            {
                UpdateEnergy();
                UpdateCombat();
            }
        }

        /// <summary>
        /// 初始化英雄
        /// </summary>
        public virtual void Initialize(string id)
        {
            heroId = id;
            // TODO: 从数据表加载英雄配置
            LoadHeroData(id);
            InitializeSkill();
        }

        protected virtual void InitializeSkill()
        {
            // 子类实现，创建对应的技能组件
        }

        public Skills.HeroSkillBase ActiveSkill => activeSkill;

        protected virtual void LoadHeroData(string id)
        {
            // TODO: 从Excel数据表加载
        }

        private void InitializeEquipmentSlots()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i] == null)
                {
                    equipmentSlots[i] = new EquipmentSlot();
                }
            }
        }

        /// <summary>
        /// 设置部署状态
        /// </summary>
        public void SetDeployed(bool deployed)
        {
            isDeployed = deployed;
        }

        /// <summary>
        /// 更新能量
        /// </summary>
        protected virtual void UpdateEnergy()
        {
            if (currentEnergy < maxEnergy)
            {
                currentEnergy += energyRegenRate * Time.deltaTime;
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            }
        }

        /// <summary>
        /// 消耗能量
        /// </summary>
        public virtual void UseEnergy(float amount)
        {
            currentEnergy = Mathf.Max(0f, currentEnergy - amount);
        }

        /// <summary>
        /// 更新战斗逻辑（子类实现）
        /// </summary>
        protected virtual void UpdateCombat()
        {
            // 子类实现自动攻击、移动、技能释放
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public virtual void TakeDamage(float damage, ElementType element = ElementType.None)
        {
            if (isDead) return;

            float finalDamage = CalculateDamage(damage, element);
            currentHealth -= finalDamage;

            if (currentHealth <= 0)
            {
                Die();
            }

            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        /// <summary>
        /// 计算最终伤害
        /// </summary>
        protected virtual float CalculateDamage(float baseDamage, ElementType element)
        {
            float defense = physicalDefense;
            
            if (element != ElementType.None)
            {
                defense = elementResistance;
            }

            float finalDamage = Mathf.Max(1f, baseDamage - defense);
            return finalDamage;
        }

        /// <summary>
        /// 治疗
        /// </summary>
        public virtual void Heal(float amount)
        {
            if (isDead) return;

            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        /// <summary>
        /// 死亡
        /// </summary>
        protected virtual void Die()
        {
            isDead = true;
            currentHealth = 0f;
            deathTimer = 0f;
            OnHeroDied?.Invoke(this);
        }

        /// <summary>
        /// 更新死亡计时器
        /// </summary>
        protected virtual void UpdateDeathTimer()
        {
            deathTimer += Time.deltaTime;
            // TODO: 实现缓慢等待复活逻辑（1-2波次后自动复活）
        }

        /// <summary>
        /// 复活英雄
        /// </summary>
        public virtual void Revive(float healthPercent = 1f)
        {
            isDead = false;
            currentHealth = maxHealth * healthPercent;
            deathTimer = 0f;
            OnHeroRevived?.Invoke(this);
        }

        /// <summary>
        /// 添加属性点
        /// </summary>
        public virtual void AddAttribute(AttributeType type, int amount)
        {
            switch (type)
            {
                case AttributeType.Strength:
                    strength += amount;
                    UpdateStatsFromAttributes();
                    break;
                case AttributeType.Agility:
                    agility += amount;
                    UpdateStatsFromAttributes();
                    break;
                case AttributeType.Intelligence:
                    intelligence += amount;
                    UpdateStatsFromAttributes();
                    break;
                case AttributeType.ElementMastery:
                    elementMastery += amount;
                    UpdateStatsFromAttributes();
                    break;
            }
        }

        /// <summary>
        /// 根据属性更新战斗数值
        /// </summary>
        protected virtual void UpdateStatsFromAttributes()
        {
            // 力量：主要影响生命值和物理防御
            maxHealth = 100f + strength * 10f;
            physicalDefense = strength * 0.5f;

            // 敏捷：主要影响攻击速度和移动速度
            attackSpeed = 1f + agility * 0.05f;
            moveSpeed = 5f + agility * 0.2f;

            // 智力：主要影响技能强度和能量恢复
            energyRegenRate = 5f + intelligence * 0.3f;

            // 元素专精：影响元素穿透和元素伤害
            // TODO: 实现元素穿透计算
        }

        /// <summary>
        /// 装备物品
        /// </summary>
        public virtual bool EquipItem(EquipmentBase equipment, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= equipmentSlots.Length)
                return false;

            if (equipmentSlots[slotIndex].equipment != null)
            {
                // 卸下旧装备
                UnequipItem(slotIndex);
            }

            equipmentSlots[slotIndex].equipment = equipment;
            equipmentSlots[slotIndex].isEquipped = true;
            ApplyEquipmentStats(equipment);
            
            OnEquipmentChanged?.Invoke(slotIndex, equipment);
            return true;
        }

        /// <summary>
        /// 卸下装备
        /// </summary>
        public virtual void UnequipItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= equipmentSlots.Length)
                return;

            if (equipmentSlots[slotIndex].equipment != null)
            {
                RemoveEquipmentStats(equipmentSlots[slotIndex].equipment);
                equipmentSlots[slotIndex].equipment = null;
                equipmentSlots[slotIndex].isEquipped = false;
                
                OnEquipmentChanged?.Invoke(slotIndex, null);
            }
        }

        /// <summary>
        /// 应用装备属性
        /// </summary>
        protected virtual void ApplyEquipmentStats(EquipmentBase equipment)
        {
            // TODO: 实现装备属性应用
        }

        /// <summary>
        /// 移除装备属性
        /// </summary>
        protected virtual void RemoveEquipmentStats(EquipmentBase equipment)
        {
            // TODO: 实现装备属性移除
        }

        public event System.Action<float, float> OnHealthChanged;
        public event System.Action<HeroBase> OnHeroDied;
        public event System.Action<HeroBase> OnHeroRevived;
        public event System.Action<int, EquipmentBase> OnEquipmentChanged;
    }

    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence,
        ElementMastery
    }

    [System.Serializable]
    public class EquipmentSlot
    {
        public EquipmentBase equipment;
        public bool isEquipped;
    }
}

