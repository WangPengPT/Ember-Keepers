using UnityEngine;
using UnityEngine.UI;
using EmberKeepers.Core;
using EmberKeepers.Localization;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 主菜单UI
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private Button startGameButton;
        [SerializeField] private TextMeshProUGUI startGameButtonText;
        [SerializeField] private Button metaProgressionButton;
        [SerializeField] private TextMeshProUGUI metaProgressionButtonText;
        [SerializeField] private Button heroArchiveButton;
        [SerializeField] private TextMeshProUGUI heroArchiveButtonText;
        [SerializeField] private Button settingsButton;
        [SerializeField] private TextMeshProUGUI settingsButtonText;
        [SerializeField] private Button exitButton;
        [SerializeField] private TextMeshProUGUI exitButtonText;

        [Header("Menu Title")]
        [SerializeField] private TextMeshProUGUI gameTitleText;

        [Header("Panels")]
        [SerializeField] private GameObject metaProgressionPanel;
        [SerializeField] private GameObject heroArchivePanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Language Selection")]
        [SerializeField] private LanguageSelectionDialog languageSelectionDialog;

        private void Start()
        {
            SetupButtons();
            HideAllPanels();
            CheckFirstLaunch();
        }

        /// <summary>
        /// 检查是否是首次启动，如果是则显示语言选择对话框
        /// </summary>
        private void CheckFirstLaunch()
        {
            if (LocalizationManager.Instance != null && LocalizationManager.Instance.IsFirstLaunch())
            {
                if (languageSelectionDialog != null)
                {
                    languageSelectionDialog.ShowDialog();
                }
                else
                {
                    Debug.LogWarning("MainMenuUI: LanguageSelectionDialog未分配，无法显示首次启动语言选择");
                }
            }
        }

        private void SetupButtons()
        {
            if (startGameButton)
            {
                startGameButton.onClick.AddListener(StartNewGame);
                AddButtonHoverEffect(startGameButton);
            }
            
            if (metaProgressionButton)
            {
                metaProgressionButton.onClick.AddListener(ShowMetaProgression);
                AddButtonHoverEffect(metaProgressionButton);
            }
            
            if (heroArchiveButton)
            {
                heroArchiveButton.onClick.AddListener(ShowHeroArchive);
                AddButtonHoverEffect(heroArchiveButton);
            }
            
            if (settingsButton)
            {
                settingsButton.onClick.AddListener(ShowSettings);
                AddButtonHoverEffect(settingsButton);
            }
            
            if (exitButton)
            {
                exitButton.onClick.AddListener(ExitGame);
                AddButtonHoverEffect(exitButton);
            }
        }

        /// <summary>
        /// 为按钮添加悬停效果
        /// </summary>
        private void AddButtonHoverEffect(Button button)
        {
            if (button == null) return;

            var colors = button.colors;
            colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f);
            colors.selectedColor = new Color(0.95f, 0.95f, 1f);
            button.colors = colors;
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

