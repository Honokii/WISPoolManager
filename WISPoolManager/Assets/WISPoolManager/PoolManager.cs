using System.Collections.Generic;
using UnityEngine;

namespace WISPoolManager {
    public class PoolManager : MonoBehaviour {
        private static PoolManager _instance;
        
        private Dictionary<string, Queue<Poolable>> availablePools = new Dictionary<string, Queue<Poolable>>();
        
        [SerializeField] private bool shouldInitializeOnStart = true;
        [SerializeField] private List<Pool> pools = new List<Pool>();

        #region Unity Methods

        private void Start() {
            if (_instance != null)
                return;
            
            _instance = this;
            if (shouldInitializeOnStart) 
                InitializePools();
        }

        private void OnDestroy() {
            if (_instance == this)
                _instance = null;
        }

        #endregion

        public void InitializePools() {
            foreach (var pool in pools) {
                var poolTag = pool.poolTag;
                var poolObject = pool.poolObject;
                var queue = new Queue<Poolable>();
                
                for (var i = 0; i < pool.initialCount; i++) {
                    var pooledObject = Instantiate(poolObject, this.transform);
                    pooledObject.gameObject.SetActive(false);
                    queue.Enqueue(pooledObject);
                }
                
                availablePools.Add(poolTag, queue);
            }
        }

        public static Poolable GetPoolable(string poolTag) {
            var manager = _instance;
            if (manager == null) return null;
            
            if (!manager.availablePools.ContainsKey(poolTag))
                return null;

            var availableQueue = manager.availablePools[poolTag];
            
            if (availableQueue.Count > 0) {
                var obj = availableQueue.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            var poolInfo = manager.pools.Find(x => x.poolTag == poolTag);
            if (!poolInfo.shouldExpand) return null;
            
            var pooledObject = Instantiate(poolInfo.poolObject, manager.transform);
            return pooledObject;
        }

        public static void DisablePoolable(Poolable poolable) {
            var manager = _instance;
            if (manager == null) return;

            if (!manager.availablePools.ContainsKey(poolable.poolTag)) {
                Destroy(poolable.gameObject);
                return;
            }

            var queue = manager.availablePools[poolable.poolTag];
            queue.Enqueue(poolable);
            poolable.gameObject.SetActive(false);
        }
    }
}
