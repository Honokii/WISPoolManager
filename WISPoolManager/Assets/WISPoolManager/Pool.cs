using UnityEngine;
using WIS.Core;

namespace WISPoolManager {
    
    [CreateAssetMenu(menuName = "Pool Manager/Pool", fileName = "NewPool", order = 0)]
    public class Pool : ScriptableObject {
        
        public StringObject poolTag;
        
        public Poolable poolObject;
        
        public int initialCount;
        
        public bool shouldExpand;
    }
}