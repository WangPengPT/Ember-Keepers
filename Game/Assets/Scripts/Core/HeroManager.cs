using UnityEngine;
using System.Collections.Generic;
using EmberKeepers.Heroes;

namespace EmberKeepers.Core
{
    /// <summary>
    /// 英雄管理器，管理所有英雄的创建、部署、状态
    /// </summary>
    public class HeroManager : MonoBehaviour
    {
        [Header("Hero Settings")]
        [SerializeField] private Transform heroContainer;
        [SerializeField] private List<HeroBase> allHeroes = new List<HeroBase>();
        [SerializeField] private List<HeroBase> deployedHeroes = new List<HeroBase>();

        [Header("Hero Prefabs")]
        [SerializeField] private List<GameObject> heroPrefabs = new List<GameObject>();

        public List<HeroBase> AllHeroes => allHeroes;
        public List<HeroBase> DeployedHeroes => deployedHeroes;

        private void Awake()
        {
            if (heroContainer == null)
            {
                GameObject container = new GameObject("HeroContainer");
                heroContainer = container.transform;
            }
        }

        /// <summary>
        /// 创建新英雄
        /// </summary>
        public HeroBase CreateHero(string heroId, Vector3 position)
        {
            // TODO: 根据heroId从数据表加载英雄配置
            GameObject heroPrefab = GetHeroPrefab(heroId);
            if (heroPrefab == null)
            {
                Debug.LogError($"Hero prefab not found: {heroId}");
                return null;
            }

            GameObject heroObj = Instantiate(heroPrefab, position, Quaternion.identity, heroContainer);
            HeroBase hero = heroObj.GetComponent<HeroBase>();
            
            if (hero != null)
            {
                allHeroes.Add(hero);
                hero.Initialize(heroId);
            }

            return hero;
        }

        /// <summary>
        /// 部署英雄到指定位置
        /// </summary>
        public bool DeployHero(HeroBase hero, Vector3 deployPosition)
        {
            if (hero == null)
                return false;

            if (deployedHeroes.Contains(hero))
            {
                // 重新部署
                hero.transform.position = deployPosition;
            }
            else
            {
                // 新部署
                deployedHeroes.Add(hero);
                hero.transform.position = deployPosition;
                hero.SetDeployed(true);
            }

            OnHeroDeployed?.Invoke(hero);
            return true;
        }

        /// <summary>
        /// 移除英雄部署
        /// </summary>
        public void UndeployHero(HeroBase hero)
        {
            if (hero == null)
                return;

            deployedHeroes.Remove(hero);
            hero.SetDeployed(false);
            OnHeroUndeployed?.Invoke(hero);
        }

        /// <summary>
        /// 获取英雄预制体
        /// </summary>
        private GameObject GetHeroPrefab(string heroId)
        {
            // TODO: 根据heroId查找对应的预制体
            if (heroPrefabs.Count > 0)
                return heroPrefabs[0];
            
            return null;
        }

        /// <summary>
        /// 获取所有存活的英雄
        /// </summary>
        public List<HeroBase> GetAliveHeroes()
        {
            List<HeroBase> alive = new List<HeroBase>();
            foreach (var hero in deployedHeroes)
            {
                if (hero != null && !hero.IsDead)
                {
                    alive.Add(hero);
                }
            }
            return alive;
        }

        public event System.Action<HeroBase> OnHeroDeployed;
        public event System.Action<HeroBase> OnHeroUndeployed;
    }
}

