using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public static LaserManager instance;
    private float laserTimeTracker = 0;
    private int laserWaveTracker = 0;
    private class LaserWaves {
        public int Amount;
        public float WaveTime;
        public float SpawnTime;
        public LaserWaves (int amount, float waveTime, float spawnTime) {
            Amount = amount;
            WaveTime = waveTime;
            SpawnTime = spawnTime;
        }
    }
    private Tuple<int, LaserWaves> laserWave;
    void Awake() {
        if (instance != null && instance != this) { 
            Destroy(this); 
        } 
        else { 
            instance = this;
        }
        Player.instance.OnReset += InitializeLaserWaves;
    }

    private void InitializeLaserWaves () {
        laserTimeTracker = 0;
        laserWaveTracker = 0;
        laserWave = new Tuple<int, LaserWaves>(0, new LaserWaves(1, 15, 3));
    }

    void Update () {
        laserTimeTracker += Time.deltaTime;
        if (laserTimeTracker >= laserWave.Item2.WaveTime) {
            //reset tracker
            laserTimeTracker = 0;
            laserWaveTracker ++;
            //do wave logic

            //set next wave
            //these values should be set depending on how many waves have passed
            int nextAmount;
            int nextWaveTime;
            int nextSpawnTime;

            //then set laserWave to a new Tuple containing these.
        }
    }
}
