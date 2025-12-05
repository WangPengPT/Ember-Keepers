using UnityEngine;
using UnityEngine.UI;
using EmberKeepers.Core;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 主菜单UI
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button metaProgressionButton;
        [SerializeField] private Button heroArchiveButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [Header("Panels")]
        [SerializeField] private GameObject metaProgressionPanel;
        [SerializeField] private GameObject heroArchivePanel;
        [SerializeField] private GameObject settingsPanel;

        private void Start()
        {
            SetupButtons();
            HideAllPanels();
        }

        private void SetupButtons()
        {
            if (startGameButton)
                startGameButton.onClick.AddListener(StartNewGame);
            
            if (metaProgressionButton)
                metaProgressionButton.onClick.AddListener(ShowMetaProgression);
            
            if (heroArchiveButton)
                heroArchiveButton.onClick.AddListener(ShowHeroArchive);
            
            if (settingsButton)
                settingsButton.onClick.AddListener(ShowSettings);
            
            if (exitButton)
                exitButton.onClick.AddListener(ExitGame);
        }

        private void StartNewGame()
        {
            HideAllPanels();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        }

        private void ShowMetaProgression()
        {
            HideAllPanels();
            if (metaProgressionPanel) metaProgressionPanel.SetActive(true);
        }

        private void ShowHeroArchive()
        {
            HideAllPanels();
            if (heroArchivePanel) heroArchivePanel.SetActive(true);
        }

        private void ShowSettings()
        {
            HideAllPanels();
            if (settingsPanel) settingsPanel.SetActive(true);
        }

        private void HideAllPanels()
        {
            if (metaProgressionPanel) metaProgressionPanel.SetActive(false);
            if (heroArchivePanel) heroArchivePanel.SetActive(false);
            if (settingsPanel) settingsPanel.SetActive(false);
        }

        private void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        /// <summary>
        /// 返回主菜单
        /// </summary>
        public void BackToMainMenu()
        {
            HideAllPanels();
        }
    }
}

