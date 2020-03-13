using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupArrow : MonoBehaviour {
    PickupSpawner pickup;
    MeshRenderer arrowRenderer;

	// Use this for initialization
	void Start () {
        pickup = GameObject.Find("PickupSpawner").GetComponent<PickupSpawner>();
        arrowRenderer = GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// We're just pointing the player to the nearest pickup.
        if (pickup.currentPickups.Count != 0) {
            GameObject closestPickup = null;
            float distance = 1000;
			// Finding closest pickup.
            foreach (GameObject p in pickup.currentPickups) {
                float currDistance = Vector3.Magnitude(p.transform.position - transform.position);
                if (currDistance < distance) {
                    distance = currDistance;
                    closestPickup = p;
                }
            }
			// Orient to "look at" closest pickup.
            transform.LookAt(closestPickup.transform.position);
            arrowRenderer.enabled = true;
        } else {
            arrowRenderer.enabled = false;
        }
	}
}
