using UnityEngine;
using System.Collections;

namespace EmberKeepers.Core
{
    /// <summary>
    /// 游戏主管理器，控制游戏状态和流程
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.MainMenu;
        
        [Header("References")]
        private WaveManager waveManager;
        private ResourceManager resourceManager;
        private HeroManager heroManager;

        public GameState CurrentState => currentState;
        public bool IsPaused => currentState == GameState.StrategyPhase;
        public bool IsCombat => currentState == GameState.CombatPhase;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeManagers();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeManagers()
        {
            if (waveManager == null)
                waveManager = FindFirstObjectByType<WaveManager>();
            
            if (resourceManager == null)
                resourceManager = FindFirstObjectByType<ResourceManager>();
            
            if (heroManager == null)
                heroManager = FindFirstObjectByType<HeroManager>();

            // 检查关键管理器是否存在
            if (waveManager == null)
                Debug.LogWarning("GameManager: WaveManager未找到，某些功能可能无法正常工作");
            
            if (resourceManager == null)
                Debug.LogWarning("GameManager: ResourceManager未找到，某些功能可能无法正常工作");
            
            if (heroManager == null)
                Debug.LogWarning("GameManager: HeroManager未找到，某些功能可能无法正常工作");
        }

        private void Start()
        {
            if (currentState == GameState.MainMenu)
            {
                // 主菜单逻辑
            }
        }

        /// <summary>
        /// 开始新的游戏局
        /// </summary>
        public void StartNewGame()
        {
            // 确保管理器已初始化
            if (resourceManager == null)
            {
                resourceManager = FindFirstObjectByType<ResourceManager>();
            }
            
            if (waveManager == null)
            {
                waveManager = FindFirstObjectByType<WaveManager>();
            }

            if (resourceManager == null)
            {
                Debug.LogError("GameManager: 无法开始新游戏，ResourceManager未找到！");
                return;
            }

            if (waveManager == null)
            {
                Debug.LogError("GameManager: 无法开始新游戏，WaveManager未找到！");
                return;
            }

            currentState = GameState.StrategyPhase;
            resourceManager.InitializeResources();
            waveManager.ResetWaves();
            OnGameStateChanged?.Invoke(currentState);
        }

        /// <summary>
        /// 开始战斗阶段
        /// </summary>
        public void StartCombatPhase()
        {
            if (currentState != GameState.StrategyPhase)
                return;

            if (waveManager == null)
            {
                waveManager = FindFirstObjectByType<WaveManager>();
                if (waveManager == null)
                {
                    Debug.LogError("GameManager: 无法开始战斗阶段，WaveManager未找到！");
                    return;
                }
            }

            currentState = GameState.CombatPhase;
            waveManager.StartCurrentWave();
            OnGameStateChanged?.Invoke(currentState);
        }

        /// <summary>
        /// 进入策略阶段（波次间隙）
        /// </summary>
        public void EnterStrategyPhase()
        {
            if (currentState != GameState.CombatPhase)
                return;

            if (waveManager == null)
            {
                waveManager = FindFirstObjectByType<WaveManager>();
                if (waveManager == null)
                {
                    Debug.LogWarning("GameManager: WaveManager未找到，跳过波次完成处理");
                }
            }

            currentState = GameState.StrategyPhase;
            if (waveManager != null)
            {
                waveManager.OnWaveCompleted();
            }
            OnGameStateChanged?.Invoke(currentState);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            currentState = GameState.GameOver;
            OnGameStateChanged?.Invoke(currentState);
            // 计算奖励，返回主菜单等
        }

        public event System.Action<GameState> OnGameStateChanged;
    }

    public enum GameState
    {
        MainMenu,
        StrategyPhase,  // 波次间隙，暂停状态
        CombatPhase,   // 战斗阶段
        GameOver
    }
}

