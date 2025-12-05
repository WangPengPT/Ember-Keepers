using UnityEngine;
using EmberKeepers.Combat;
using EmberKeepers.Data;

namespace EmberKeepers.Heroes
{
    /// <summary>
    /// 英雄AI组件，处理自动攻击、移动、技能释放
    /// </summary>
    [RequireComponent(typeof(HeroBase))]
    public class HeroAI : MonoBehaviour
    {
        private HeroBase hero;
        private MonsterBase currentTarget;
        private float lastAttackTime = 0f;
        private float skillCooldownTimer = 0f;

        [Header("AI Settings")]
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float moveSpeed = 5f;

        private void Awake()
        {
            hero = GetComponent<HeroBase>();
        }

        private void Update()
        {
            if (hero.IsDead || !hero.IsDeployed)
                return;

            UpdateSkillCooldown();
            FindTarget();
            MoveAndAttack();
            CheckSkillUsage();
        }

        private void UpdateSkillCooldown()
        {
            if (skillCooldownTimer > 0f)
            {
                skillCooldownTimer -= Time.deltaTime;
            }
        }

        private void FindTarget()
        {
            if (currentTarget != null && !currentTarget.IsDead)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (distance <= detectionRange)
                    return; // 目标仍然有效
            }

            // 寻找新目标
            currentTarget = CombatSystem.Instance?.FindNearestEnemy(transform.position, detectionRange);
        }

        private void MoveAndAttack()
        {
            if (currentTarget == null || currentTarget.IsDead)
                return;

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > attackRange)
            {
                // 移动到攻击范围
                Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(currentTarget.transform);
            }
            else
            {
                // 在攻击范围内，进行攻击
                if (Time.time - lastAttackTime >= 1f / hero.attackSpeed)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }

        private void Attack()
        {
            if (currentTarget == null || currentTarget.IsDead)
                return;

            CombatSystem.Instance?.HeroAttackMonster(hero, currentTarget);
        }

        private void CheckSkillUsage()
        {
            if (hero.ActiveSkill == null)
                return;

            // 使用技能系统的AI逻辑
            if (hero.ActiveSkill.ShouldUseSkill())
            {
                if (hero.ActiveSkill.UseSkill())
                {
                    OnSkillUsed?.Invoke();
                }
            }
        }

        public event System.Action OnSkillUsed;
    }
}

