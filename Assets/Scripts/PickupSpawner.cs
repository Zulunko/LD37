using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour, Referrer {
    public GameObject[] spawnLocs;
    public GameObject pickupPrefab;
    public List<GameObject> currentPickups = new List<GameObject>();

    private float lastSpawnTime = 0;
    private float spawnInterval = 15;

    void Update() {
        // Remove initial ammo pickup
        if (lastSpawnTime == 0)
            lastSpawnTime = Time.time;
		// Spawn ammo every interval
        if (Time.time - lastSpawnTime > spawnInterval) {
            SpawnAmmo();
            lastSpawnTime = Time.time;
        }
    }

    public void SpawnAmmo() {
		// Randomly choose an ammo location to spawn from and spawn it.
        int loc = Mathf.FloorToInt(Random.Range(0, spawnLocs.Length - 0.01f));
        GameObject newPickup = (GameObject)Instantiate(pickupPrefab, spawnLocs[loc].transform.position, Quaternion.Euler(90, Random.Range(0, 360), 0));
        newPickup.GetComponent<AmmoPickup>().RegisterReferrer(this);
        currentPickups.Add(newPickup);
    }

    public void RemoveReference(GameObject pickup) {
        currentPickups.Remove(pickup);
    }
}
