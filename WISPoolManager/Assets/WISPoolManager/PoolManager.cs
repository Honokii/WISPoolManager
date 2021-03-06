using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WISPoolManager {
    public class PoolManager : MonoBehaviour {
        public static PoolManager Instance;
        
        private readonly Dictionary<string, Queue<Poolable>> _availablePools = new Dictionary<string, Queue<Poolable>>();
        private readonly Dictionary<string, Queue<Poolable>> _activePools = new Dictionary<string, Queue<Poolable>>();
        
        [SerializeField] private bool setAsSingletonInstance = true;
        
        [SerializeField] private bool shouldInitializeOnStart = true;
        
        [SerializeField] private List<Pool> pools = new List<Pool>();

        #region Unity Methods

        private void Start() {
            if (setAsSingletonInstance && Instance != this)
                Instance = this;

            if (shouldInitializeOnStart) 
                InitializePools();
        }

        private void OnDestroy() {
            if (setAsSingletonInstance && Instance == this)
                Instance = null;
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
                    pooledObject.poolTag = poolTag;
                    queue.Enqueue(pooledObject);
                }
                
                _availablePools.Add(poolTag.value, queue);
            }
        }

        public Poolable GetPoolable(string poolTag) {
            if (!_availablePools.ContainsKey(poolTag))
                return null;

            var availableQueue = _availablePools[poolTag];
            
            if (availableQueue.Count > 0) {
                var obj = availableQueue.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            var poolInfo = pools.Find(x => x.poolTag.value == poolTag);
            if (!poolInfo.shouldExpand) return null;
            
            var pooledObject = Instantiate(poolInfo.poolObject, transform);
            return pooledObject;
        }

        public Poolable GetPoolable(string poolTag, Transform parent) {
            var poolable = GetPoolable(poolTag);
            if (poolable == null)
                return null;

            poolable.transform.parent = parent;
            return poolable;
        }

        public T GetPoolable<T>(string poolTag, Transform parent) {
            var poolable = GetPoolable(poolTag);
            poolable.transform.parent = parent;
            var result = poolable.GetComponent<T>();
            return result;
        }

        public void DisablePoolable(Poolable poolable) {
            if (!_availablePools.ContainsKey(poolable.poolTag.value)) {
                Destroy(poolable.gameObject);
                return;
            }

            var queue = _availablePools[poolable.poolTag.value];
            queue.Enqueue(poolable);
            poolable.gameObject.SetActive(false);
        }
    }
}
