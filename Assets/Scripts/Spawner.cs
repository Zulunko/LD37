using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject enemyPrefab;
    private float prevTime = 0;
    private float delay = 0;
    private int remainingSpawns = 0;
    private static float SPAWNFAST = 0.5f;
    private static float SPAWNSLOW = 2f;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (remainingSpawns > 0) {
            float time = Time.fixedTime;
            if (time - prevTime > delay) {
                prevTime = time;
                GameObject.Instantiate(enemyPrefab, transform.position, transform.rotation);
                delay = Random.Range(SPAWNFAST, SPAWNSLOW);
                remainingSpawns--;
            }
        }
	}

    public void Spawn(int numToSpawn) {
        remainingSpawns += numToSpawn;
    }
}
