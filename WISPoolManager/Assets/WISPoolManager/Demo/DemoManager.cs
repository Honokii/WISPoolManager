using UnityEngine;
using WIS.Core;
using WISPoolManager;

public class DemoManager : MonoBehaviour {
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private StringObject cubeTag;
    private float pulse = 1f;
    private float currentPulse;

    private void FixedUpdate() {
        if (currentPulse >= pulse) {
            var cube = _poolManager.GetPoolable(cubeTag.value, transform);
            cube.transform.position = Vector3.zero;
            currentPulse = 0;
        }

        currentPulse += Time.deltaTime;
    }
}
