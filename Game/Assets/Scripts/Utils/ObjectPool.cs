using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.Utils
{
    /// <summary>
    /// 对象池系统 - 用于优化怪物、特效等频繁创建销毁的对象
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 注册预制体到对象池
        /// </summary>
        public void RegisterPrefab(string poolName, GameObject prefab, int initialSize = 10)
        {
            if (!prefabDictionary.ContainsKey(poolName))
            {
                prefabDictionary[poolName] = prefab;
                poolDictionary[poolName] = new Queue<GameObject>();

                // 预先创建对象
                for (int i = 0; i < initialSize; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    poolDictionary[poolName].Enqueue(obj);
                }
            }
        }

        /// <summary>
        /// 从对象池获取对象
        /// </summary>
        public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(poolName))
            {
                Debug.LogWarning($"Pool {poolName} not registered!");
                return null;
            }

            GameObject obj;
            if (poolDictionary[poolName].Count > 0)
            {
                obj = poolDictionary[poolName].Dequeue();
            }
            else
            {
                // 池中没有可用对象，创建新的
                obj = Instantiate(prefabDictionary[poolName]);
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }

        /// <summary>
        /// 回收对象到池中
        /// </summary>
        public void Recycle(string poolName, GameObject obj)
        {
            if (!poolDictionary.ContainsKey(poolName))
            {
                Debug.LogWarning($"Pool {poolName} not registered!");
                Destroy(obj);
                return;
            }

            obj.SetActive(false);
            poolDictionary[poolName].Enqueue(obj);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void ClearPool(string poolName)
        {
            if (poolDictionary.ContainsKey(poolName))
            {
                while (poolDictionary[poolName].Count > 0)
                {
                    GameObject obj = poolDictionary[poolName].Dequeue();
                    Destroy(obj);
                }
            }
        }

        /// <summary>
        /// 清空所有对象池
        /// </summary>
        public void ClearAllPools()
        {
            foreach (var pool in poolDictionary.Values)
            {
                while (pool.Count > 0)
                {
                    GameObject obj = pool.Dequeue();
                    Destroy(obj);
                }
            }
            poolDictionary.Clear();
            prefabDictionary.Clear();
        }
    }
}

