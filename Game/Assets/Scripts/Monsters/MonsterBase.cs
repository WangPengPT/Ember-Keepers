using UnityEngine;
using EmberKeepers.Data;

namespace EmberKeepers.Monsters
{
    /// <summary>
    /// 怪物基类
    /// </summary>
    public class MonsterBase : MonoBehaviour
    {
        [Header("Monster Identity")]
        [SerializeField] protected string monsterId;
        [SerializeField] protected string monsterName;
        [SerializeField] protected MonsterFamily family;
        [SerializeField] protected int level = 1;

        [Header("Combat Stats")]
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth = 100f;
        [SerializeField] protected float attackDamage = 10f;
        [SerializeField] protected float attackSpeed = 1f;
        [SerializeField] protected float moveSpeed = 3f;

        [Header("Element Resistance")]
        [SerializeField] protected float fireResistance = 0f;
        [SerializeField] protected float iceResistance = 0f;
        [SerializeField] protected float thunderResistance = 0f;
        [SerializeField] protected float earthResistance = 0f;
        [SerializeField] protected float physicalResistance = 0f;

        [Header("Special Abilities")]
        [SerializeField] protected bool isElite = false;
        [SerializeField] protected bool isBoss = false;

        [Header("Pathfinding")]
        [SerializeField] protected Transform target; // 基地核心
        [SerializeField] protected float detectionRange = 10f;

        public string MonsterId => monsterId;
        public bool IsDead => currentHealth <= 0;
        public bool IsElite => isElite;
        public bool IsBoss => isBoss;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            FindTarget();
        }

        protected virtual void Update()
        {
            if (IsDead) return;

            MoveTowardsTarget();
            CheckForAttack();
        }

        /// <summary>
        /// 初始化怪物
        /// </summary>
        public virtual void Initialize(string id, int monsterLevel, Transform targetTransform)
        {
            monsterId = id;
            level = monsterLevel;
            target = targetTransform;
            
            // TODO: 从数据表加载怪物配置
            LoadMonsterData(id, monsterLevel);
        }

        protected virtual void LoadMonsterData(string id, int level)
        {
            // TODO: 从Excel数据表加载
            // 根据等级调整属性
            ScaleStatsByLevel(level);
        }

        protected virtual void ScaleStatsByLevel(int level)
        {
            float multiplier = 1f + (level - 1) * 0.3f;
            maxHealth *= multiplier;
            attackDamage *= multiplier;
            moveSpeed *= (1f + (level - 1) * 0.1f);
        }

        protected virtual void FindTarget()
        {
            if (target == null)
            {
                // 查找基地核心
                GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
                if (core != null)
                    target = core.transform;
            }
        }

        protected virtual void MoveTowardsTarget()
        {
            if (target == null) return;

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(target);
        }

        protected virtual void CheckForAttack()
        {
            if (target == null) return;

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= 2f) // 攻击范围
            {
                Attack();
            }
        }

        protected virtual void Attack()
        {
            // TODO: 对基地核心造成伤害
            if (target != null)
            {
                // BaseCore core = target.GetComponent<BaseCore>();
                // if (core != null)
                //     core.TakeDamage(attackDamage);
            }
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public virtual void TakeDamage(float damage, ElementType element = ElementType.None)
        {
            if (IsDead) return;

            float finalDamage = CalculateDamage(damage, element);
            currentHealth -= finalDamage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 计算最终伤害（考虑元素抗性）
        /// </summary>
        protected virtual float CalculateDamage(float baseDamage, ElementType element)
        {
            float resistance = 0f;

            switch (element)
            {
                case ElementType.Fire:
                    resistance = fireResistance;
                    break;
                case ElementType.Ice:
                    resistance = iceResistance;
                    break;
                case ElementType.Thunder:
                    resistance = thunderResistance;
                    break;
                case ElementType.Earth:
                    resistance = earthResistance;
                    break;
                case ElementType.None:
                    resistance = physicalResistance;
                    break;
            }

            float finalDamage = baseDamage * (1f - resistance);
            return Mathf.Max(1f, finalDamage);
        }

        protected virtual void Die()
        {
            // 掉落金币和装备
            DropLoot();
            OnMonsterDied?.Invoke(this);
            Destroy(gameObject, 0.1f);
        }

        protected virtual void DropLoot()
        {
            // TODO: 实现掉落逻辑
            // ResourceManager.Instance.AddGold(Random.Range(5, 15));
        }

        public event System.Action<MonsterBase> OnMonsterDied;
    }

    public enum MonsterFamily
    {
        Whisper,      // 低语者
        Corroder,     // 爬行者
        Bonewall,     // 护盾者
        Aberration,   // 畸变体
        Cluster,      // 聚合体
        ChaosMage     // 施法者
    }
}

