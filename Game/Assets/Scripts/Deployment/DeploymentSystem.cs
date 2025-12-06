using UnityEngine;
using UnityEngine.EventSystems;
using EmberKeepers.Heroes;
using EmberKeepers.Core;

namespace EmberKeepers.Deployment
{
    /// <summary>
    /// 部署系统，处理英雄拖拽部署到防御区域
    /// </summary>
    public class DeploymentSystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Deployment Settings")]
        [SerializeField] private LayerMask deploymentLayer;
        [SerializeField] private float deploymentRadius = 10f;
        [SerializeField] private GameObject deploymentAreaPrefab;

        private HeroBase draggedHero;
        private Vector3 originalPosition;
        private bool isDragging = false;

        private HeroManager heroManager;
        private GameManager gameManager;

        private void Awake()
        {
            heroManager = FindFirstObjectByType<HeroManager>();
            gameManager = FindFirstObjectByType<GameManager>();
        }

        /// <summary>
        /// 开始拖拽英雄
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (gameManager != null && !gameManager.IsPaused)
                return; // 只能在策略阶段部署

            if (Camera.main == null)
            {
                Debug.LogWarning("DeploymentSystem: 主摄像机未找到，无法开始拖拽");
                return;
            }

            // 检测是否点击了英雄
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                HeroBase hero = hit.collider.GetComponent<HeroBase>();
                if (hero != null)
                {
                    draggedHero = hero;
                    originalPosition = hero.transform.position;
                    isDragging = true;
                }
            }
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || draggedHero == null)
                return;

            if (Camera.main == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, deploymentLayer))
            {
                // 检查是否在部署区域内
                if (IsValidDeploymentPosition(hit.point))
                {
                    draggedHero.transform.position = hit.point;
                }
            }
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging || draggedHero == null)
                return;

            if (Camera.main == null)
            {
                // 没有摄像机，返回原位置
                draggedHero.transform.position = originalPosition;
                draggedHero = null;
                isDragging = false;
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, deploymentLayer))
            {
                if (IsValidDeploymentPosition(hit.point))
                {
                    // 部署英雄
                    if (heroManager != null)
                    {
                        heroManager.DeployHero(draggedHero, hit.point);
                    }
                }
                else
                {
                    // 无效位置，返回原位置
                    draggedHero.transform.position = originalPosition;
                }
            }
            else
            {
                // 没有有效目标，返回原位置
                draggedHero.transform.position = originalPosition;
            }

            draggedHero = null;
            isDragging = false;
        }

        /// <summary>
        /// 检查是否为有效的部署位置
        /// </summary>
        private bool IsValidDeploymentPosition(Vector3 position)
        {
            // 检查是否在基地核心周围的有效范围内
            GameObject core = GameObject.FindGameObjectWithTag("BaseCore");
            if (core != null)
            {
                float distance = Vector3.Distance(position, core.transform.position);
                if (distance <= deploymentRadius && distance >= 2f) // 不能太近也不能太远
                {
                    // 检查是否与其他英雄重叠
                    Collider[] overlaps = Physics.OverlapSphere(position, 1f);
                    foreach (var col in overlaps)
                    {
                        if (col.GetComponent<HeroBase>() != null && col.GetComponent<HeroBase>() != draggedHero)
                            return false;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建部署区域可视化
        /// </summary>
        public void CreateDeploymentArea(Vector3 center, float radius)
        {
            if (deploymentAreaPrefab != null)
            {
                GameObject area = Instantiate(deploymentAreaPrefab, center, Quaternion.identity);
                area.transform.localScale = Vector3.one * radius * 2f;
            }
        }
    }
}

