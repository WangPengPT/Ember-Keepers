using UnityEngine;
using EmberKeepers.Core;

namespace EmberKeepers.UI
{
    /// <summary>
    /// UI管理器，控制所有UI面板的显示和隐藏
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Panels")]
        [SerializeField] private GameObject topInfoPanel;
        [SerializeField] private GameObject bottomInfoPanel;
        [SerializeField] private GameObject combatPanel;
        [SerializeField] private GameObject strategyPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject heroDetailPanel;
        [SerializeField] private GameObject baseCorePanel;

        private GameManager gameManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += OnGameStateChanged;
            }

            // 初始化UI状态
            ShowCombatUI();
        }

        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.CombatPhase:
                    ShowCombatUI();
                    break;
                case GameState.StrategyPhase:
                    ShowStrategyUI();
                    break;
                case GameState.MainMenu:
                    ShowMainMenuUI();
                    break;
                case GameState.GameOver:
                    ShowGameOverUI();
                    break;
            }
        }

        /// <summary>
        /// 显示战斗阶段UI
        /// </summary>
        public void ShowCombatUI()
        {
            if (topInfoPanel) topInfoPanel.SetActive(true);
            if (bottomInfoPanel) bottomInfoPanel.SetActive(true);
            if (combatPanel) combatPanel.SetActive(true);
            if (strategyPanel) strategyPanel.SetActive(false);
            if (shopPanel) shopPanel.SetActive(false);
        }

        /// <summary>
        /// 显示策略阶段UI
        /// </summary>
        public void ShowStrategyUI()
        {
            if (topInfoPanel) topInfoPanel.SetActive(true);
            if (bottomInfoPanel) bottomInfoPanel.SetActive(true);
            if (combatPanel) combatPanel.SetActive(false);
            if (strategyPanel) strategyPanel.SetActive(true);
            if (shopPanel) shopPanel.SetActive(true);
        }

        /// <summary>
        /// 显示主菜单UI
        /// </summary>
        public void ShowMainMenuUI()
        {
            if (topInfoPanel) topInfoPanel.SetActive(false);
            if (bottomInfoPanel) bottomInfoPanel.SetActive(false);
            // TODO: 显示主菜单
        }

        /// <summary>
        /// 显示游戏结束UI
        /// </summary>
        public void ShowGameOverUI()
        {
            // TODO: 显示游戏结束界面
        }

        /// <summary>
        /// 显示英雄详情面板
        /// </summary>
        public void ShowHeroDetailPanel(bool show)
        {
            if (heroDetailPanel) heroDetailPanel.SetActive(show);
        }

        /// <summary>
        /// 显示基地核心面板
        /// </summary>
        public void ShowBaseCorePanel(bool show)
        {
            if (baseCorePanel) baseCorePanel.SetActive(show);
        }
    }
}

