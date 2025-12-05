using UnityEngine;
using EmberKeepers.Core;
using EmberKeepers.Data;
using EmberKeepers.MetaProgression;

namespace EmberKeepers.Utils
{
    /// <summary>
    /// 游戏初始化器 - 确保所有管理器正确初始化
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Manager Prefabs")]
        [SerializeField] private GameObject gameManagerPrefab;
        [SerializeField] private GameObject dataManagerPrefab;
        [SerializeField] private GameObject metaProgressionManagerPrefab;
        [SerializeField] private GameObject objectPoolPrefab;
        [SerializeField] private GameObject audioManagerPrefab;

        private void Awake()
        {
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            // 初始化DataManager
            if (DataManager.Instance == null && dataManagerPrefab != null)
            {
                Instantiate(dataManagerPrefab);
            }

            // 初始化MetaProgressionManager
            if (MetaProgressionManager.Instance == null && metaProgressionManagerPrefab != null)
            {
                Instantiate(metaProgressionManagerPrefab);
            }

            // 初始化GameManager
            if (GameManager.Instance == null && gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab);
            }

            // 初始化ObjectPool
            if (ObjectPool.Instance == null && objectPoolPrefab != null)
            {
                Instantiate(objectPoolPrefab);
            }

            // 初始化AudioManager
            if (AudioManager.Instance == null && audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }

            Debug.Log("Game managers initialized successfully!");
        }
    }
}

