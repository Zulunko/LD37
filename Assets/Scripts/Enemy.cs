using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Enemy AI
public class Enemy : MonoBehaviour {

    public GUISkin Hp;
    public GUISkin HpMissing;

    private NavMeshAgent agent;
    private Transform player;
    private Player playerScript;
    new private Renderer renderer;
    private float health = 100;
    private float totalHealth = 100;
    private List<Referrer> refers = new List<Referrer>();
    new private ParticleSystem particleSystem;
    private AudioSource damageSound;
    private AudioSource deathSound;

    private static float MINSPEED = 2f;
    private static float MAXSPEED = 6;
    private static float MINHEALTH = 35;
    private static float MAXHEALTH = 140;
    private static float ATTACKRANGE = 1.5f;
    private static float DAMAGEPERSEC = 25;

    public static float ENEMYSCALE = 1;
    public static float SPEEDSCALE = 1;

    private bool destroyed = false;
    private int slowedCount = 0;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        playerScript = player.gameObject.GetComponent<Player>();
        renderer = GetComponent<Renderer>();
        particleSystem = GetComponent<ParticleSystem>();
        AudioSource[] sounds = GetComponents<AudioSource>();

        damageSound = sounds[0];
        deathSound = sounds[1];

        // More health = less speed and vice versa (balancing!)
        float index = Random.Range(0f, 1f);
        agent.speed = SPEEDSCALE * (MINSPEED + (index * (MAXSPEED - MINSPEED)));
        totalHealth = health = ENEMYSCALE * (MINHEALTH + ((1 - index) * (MAXHEALTH - MINHEALTH)));

        GameManager.ENEMIES++;
	}
	
	// Update is called once per frame
	void Update () {
		// The AI always simply seeks toward the player.
        agent.destination = player.position;
		// If the AI is in range to attack, attack the player!
        if (Vector3.Magnitude(player.position - transform.position) < ATTACKRANGE) {
            if (!particleSystem.isPlaying) {
                particleSystem.Play();
                damageSound.Play();
            }
			// Enemies do continuous damage over time to the player as they stay in range.
			// This ensures enemies don't "burst" the player down while also being somewhat forgiving if the player touches one enemy.
            playerScript.Damage(-Time.deltaTime * DAMAGEPERSEC);
        } else {
            particleSystem.Stop();
            damageSound.Stop();
        }
	}

    public void Damage(float diff) {
		// Handle taking damage and dying.
        health += diff;
        if (health <= 0 && !destroyed) {
            deathSound.Play();
            StartCoroutine(DelayedDestroy());
        }
    }

    void OnGUI() {
		// Show healthbars above enemies.
        if (Vector3.Dot(Camera.main.transform.forward, transform.position - Camera.main.transform.position) > 0
            && !Physics.Raycast(transform.position, Camera.main.transform.position - transform.position, Vector3.Magnitude(transform.position - Camera.main.transform.position))) {

            Vector3 targetPos = Camera.main.WorldToScreenPoint(transform.position + 1.5f * Vector3.up);
            GUI.skin = HpMissing;
            GUI.Box(new Rect(targetPos.x - 50, Screen.height - targetPos.y, 100, 10), "");
            GUI.skin = Hp;
            GUI.Box(new Rect(targetPos.x - 50, Screen.height - targetPos.y, health / totalHealth * 100, 10), "");
        }
    }

    public void RegisterReference(Referrer r) {
        refers.Add(r);
    }

    public void UnregisterReference(Referrer r) {
        refers.Remove(r);
    }

    private void ReleaseAllReferences() {
        foreach (Referrer r in refers) {
            r.RemoveReference(gameObject);
        }
    }

	// We want the enemy to be slowed to half when under the effect of at least one slow turret.
	// This counting mechanism ensures the enemy won't be double-slowed but also will remain slowed until out of all slow effects.
    public void Slow() {
        if (slowedCount == 0) {
            agent.speed /= 2;
        }
        slowedCount++;
    }

	// The opposite of the above. Realistically dividing and multiplying speed by 2 could eventually cause the speed to deviate, but that'd take a long time.
    public void RemoveSlow() {
        slowedCount--;
        if (slowedCount == 0) {
            agent.speed *= 2;
        }
    }

    private IEnumerator DelayedDestroy() {
		// Destroy must happen on the next frame during the coroutine processing to avoid conflicts with currently active mechanics.
        destroyed = true;
        GameManager.ENEMIES--;
        yield return null;
        ReleaseAllReferences();
        Destroy(gameObject);
    }
}
