using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EmberKeepers.Core;
using EmberKeepers.MetaProgression;

namespace EmberKeepers.UI
{
    /// <summary>
    /// 游戏结束UI - 显示结算和奖励
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("Result Display")]
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private TextMeshProUGUI waveReachedText;

        [Header("Rewards")]
        [SerializeField] private TextMeshProUGUI essenceRewardText;
        [SerializeField] private TextMeshProUGUI totalEssenceText;

        [Header("Buttons")]
        [SerializeField] private Button returnToMenuButton;
        [SerializeField] private Button retryButton;

        private WaveManager waveManager;
        private MetaProgressionManager metaManager;

        private void Start()
        {
            waveManager = FindFirstObjectByType<WaveManager>();
            metaManager = MetaProgressionManager.Instance;
            
            SetupButtons();
            CalculateRewards();
        }

        private void SetupButtons()
        {
            if (returnToMenuButton)
                returnToMenuButton.onClick.AddListener(ReturnToMenu);
            
            if (retryButton)
                retryButton.onClick.AddListener(Retry);
        }

        private void CalculateRewards()
        {
            if (waveManager == null || metaManager == null) return;

            int waveReached = waveManager.CurrentWave;
            
            // 根据波次计算星火精粹奖励
            int essenceReward = CalculateEssenceReward(waveReached);
            
            // 应用精粹获取效率加成
            essenceReward = Mathf.RoundToInt(essenceReward * metaManager.GetEssenceEfficiencyBonus());
            
            // 添加精粹
            metaManager.AddStarfireEssence(essenceReward);

            // 更新UI
            if (resultText)
            {
                if (waveReached >= 20)
                {
                    resultText.text = "胜利！";
                    resultText.color = Color.green;
                }
                else
                {
                    resultText.text = "失败";
                    resultText.color = Color.red;
                }
            }

            if (waveReachedText)
                waveReachedText.text = $"到达波次: {waveReached}";
            
            if (essenceRewardText)
                essenceRewardText.text = $"+{essenceReward} 星火精粹";
            
            if (totalEssenceText)
                totalEssenceText.text = $"总计: {metaManager.StarfireEssence}";
        }

        private int CalculateEssenceReward(int waveReached)
        {
            // 基础奖励：每波10精粹
            int baseReward = waveReached * 10;
            
            // Boss额外奖励
            if (waveReached >= 10) baseReward += 50;
            if (waveReached >= 15) baseReward += 100;
            if (waveReached >= 20) baseReward += 200;
            
            return baseReward;
        }

        private void ReturnToMenu()
        {
            // TODO: 加载主菜单场景
            Debug.Log("返回主菜单");
        }

        private void Retry()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartNewGame();
            }
        }
    }
}

