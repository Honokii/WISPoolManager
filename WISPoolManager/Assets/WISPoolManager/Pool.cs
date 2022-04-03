using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WISPoolManager {
    
    [Serializable]
    public class Pool {
        public string poolTag;
        public Poolable poolObject;
        public int initialCount;
        public bool shouldExpand;
    }
}