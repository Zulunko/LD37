using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float health = 100;
    private float maxHealth = 100;
    private static float REGEN = 0.5f;
    public GUISkin hpMissing;
    public GUISkin hp;
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
		// The player has health regen.
        health += REGEN * Time.deltaTime;
        if (health > maxHealth)
            health = maxHealth;
    }

    void OnGUI() {
		// Show player healthbar.
        if (!GameManager.gameLost) {
            GUI.skin = hpMissing;
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 25, 300, 25), "");
            GUI.skin = hp;
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height - 25, health / maxHealth * 300, 25), "");
        }
    }

	public void Damage(float damage) {
		// Add damage to health. Damage should always be negative unless it's healing.
        health += damage;
		// You ded.
        if (health <= 0 && !GameManager.gameLost) {
            gameManager.LoseGame();
        }
    }
}
