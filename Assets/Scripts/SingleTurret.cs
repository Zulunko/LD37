using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTurret : MonoBehaviour, Referrer {

    public List<GameObject> affectedEnemies = new List<GameObject>();
    private float damageInterval = 0.25f;
    private float damage = 20f;
    private float lastDamageTime = 0;
    private GameObject currEnemy = null;
    new private AudioSource audio;
    LineRenderer line;

    // Use this for initialization
    void Start() {
        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
		// We lock to one enemy until that enemy exits our range or dies.
        if (currEnemy != null) {
            if (Physics.Raycast(transform.position, currEnemy.transform.position - transform.position, Vector3.Magnitude(currEnemy.transform.position - transform.position))) {
                currEnemy = null;
                line.SetPosition(1, transform.position);
            }
        }
		// Pick one of the enemies within range and attack them.
        if (currEnemy == null && affectedEnemies.Count > 0) {
            foreach (GameObject e in affectedEnemies) {
                if (!Physics.Raycast(transform.position, e.transform.position - transform.position, Vector3.Magnitude(e.transform.position - transform.position))) {
                    currEnemy = e;
                    break;
                }
            }
        }
		// While attacking an enemy, deal damage every damageInterval.
        if (currEnemy != null) {
            line.SetPosition(1, currEnemy.transform.position);
            if (Time.time - lastDamageTime >= damageInterval) {
                audio.Play();
                currEnemy.GetComponent<Enemy>().Damage(-damage);
                lastDamageTime = Time.time;
            }
        }
    }

	// Register enemies who are inside of the radius.
    void OnTriggerEnter(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            affectedEnemies.Add(c.gameObject);
            e.RegisterReference(this);
        }
    }

	// Unregister enemies who leave the radius.
    void OnTriggerExit(Collider c) {
        Enemy e = c.GetComponent<Enemy>();
        if (e != null) {
            e.UnregisterReference(this);
            RemoveReference(c.gameObject);
        }
    }

    public void RemoveReference(GameObject enemy) {
        affectedEnemies.Remove(enemy);
        if (enemy == currEnemy) {
            line.SetPosition(1, transform.position);
            currEnemy = null;
        }
    }
}
