﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WISPoolManager;

public class DemoManager : MonoBehaviour {
    [SerializeField] private PoolManager _poolManager;

    private float pulse = 1f;
    private float currentPulse;
    
    private void FixedUpdate() {
        if (currentPulse >= pulse) {
            var cube = PoolManager.GetPoolable("cube");
            currentPulse = 0;
        }

        currentPulse += Time.deltaTime;
    }
}