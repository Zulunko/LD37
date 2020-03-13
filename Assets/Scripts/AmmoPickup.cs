using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {
    private List<Referrer> referrers = new List<Referrer>();

	void OnTriggerEnter(Collider c) {
        if (c.gameObject.name == "Player") {
            Camera.main.GetComponent<CamControl>().AddAmmo(3);
            foreach (Referrer r in referrers) {
                r.RemoveReference(gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void RegisterReferrer(Referrer r) {
        referrers.Add(r);
    }
}
