using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour {
	// Because slow is part of the enemy AI, we simply send enemies a message when they enter the field.
    void OnTriggerEnter(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            e.Slow();
        }
    }

    void OnTriggerExit(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            e.RemoveSlow();
        }
    }
}
