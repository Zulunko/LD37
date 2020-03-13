using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChatter : MonoBehaviour {
    private AudioSource[] sources;

    void Start() {
        sources = GetComponents<AudioSource>();
        StartCoroutine(Chatter());
    }

	IEnumerator Chatter() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10f, 30f));
            sources[Random.Range(0, sources.Length)].Play();
        }
    }
}
