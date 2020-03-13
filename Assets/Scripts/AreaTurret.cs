using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTurret : MonoBehaviour, Referrer {

    List<GameObject> affectedEnemies = new List<GameObject>();
    private float damageInterval = 1f;
    private float damage = 20f;
    private float lastDamageTime = 0;
    ParticleSystem particles;
    new private AudioSource audio;

	// Use this for initialization
	void Start () {
        particles = transform.parent.FindChild("Particle System").GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		// Is it time to shoot?
        if (affectedEnemies.Count > 0 && Time.time - lastDamageTime >= damageInterval) {
			// Display shooty shooty particles!
            particles.Play();
            audio.Play();
			// Do the damage!
            foreach (GameObject e in affectedEnemies) {
                e.GetComponent<Enemy>().Damage(-damage);
            }
            lastDamageTime = Time.time;
        }
	}

	// Register enemies when they enter.
    void OnTriggerEnter(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            affectedEnemies.Add(c.gameObject);
            e.RegisterReference(this);
        }
    }

	// Unregister enemies when they leave.
    void OnTriggerExit(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            e.UnregisterReference(this);
            RemoveReference(c.gameObject);
        }
    }

    public void RemoveReference(GameObject enemy) {
        affectedEnemies.Remove(enemy);
    }
}
