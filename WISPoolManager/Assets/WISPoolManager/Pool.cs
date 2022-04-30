using UnityEngine;

namespace WISPoolManager {
    
    [CreateAssetMenu(menuName = "Pool Manager/Pool", fileName = "NewPool", order = 0)]
    public class Pool : ScriptableObject {
        
        public string poolTag;
        
        public Poolable poolObject;
        
        public int initialCount;
        
        public bool shouldExpand;
    }
}