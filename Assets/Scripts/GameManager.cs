using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The game manager tracks the game-wide state of the game.
public class GameManager : MonoBehaviour {

    public GameObject[] spawnerBoxes;
    private Spawner[] spawners;
    private int currentWave = 0;
    private static int TOTALWAVES = 10;
    private static float WAVENUMSCALE = 1.3f;
    private static float WAVEHPSCALE = 1.25f;
    private static float WAVEDELAYSCALE = 1.05f;
    private static float WAVESPEEDSCALE = 1.05f;
    private float waveDelay = 25;
    private string waveMessage = "";
    private int numEnemies = 4;
    public static int ENEMIES = 0;
    public static bool gameLost = false;

	// Use this for initialization
	void Start () {
        spawners = new Spawner[spawnerBoxes.Length];
		for (int i = 0; i < spawnerBoxes.Length; i++) {
            spawners[i] = spawnerBoxes[i].GetComponent<Spawner>();
        }
        StartCoroutine(WaveCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI() {
		// Shows a message in the center of the screen to communicate game state.
        if (waveMessage != "" && !gameLost) {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 50), waveMessage);
        }
        GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), "Enemies: " + ENEMIES.ToString() + "\nWave: " + currentWave.ToString());
        if (gameLost) {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU LOSE!!!");
        }
    }

	// A coroutine that handles the UI that must stay on-screen for certain amounts of time.
    IEnumerator WaveCoroutine() {
        for (int i = 0; i < TOTALWAVES; i++) {
			// Countdown to next wave.
            for (int j = 10; j > 0; j--) {
                waveMessage = "Next wave in: " + j.ToString();
                yield return new WaitForSeconds(1);
            }
            IncrementWave();
			// Start spawning from all spawners.
            foreach (Spawner s in spawners) {
                s.Spawn(numEnemies);
            }
			// Display a wave start message for 3 seconds.
            waveMessage = "Wave " + currentWave + "/" + TOTALWAVES + " Start!";
            yield return new WaitForSeconds(3);
            waveMessage = "";
			// Wait until the next wave.
            yield return new WaitForSeconds(waveDelay);
        }
        while (ENEMIES > 0) {
            yield return null;
        }
		// If all enemies are dead, the player won. If the player dies, this all gets wiped by the game reset.
        waveMessage = "YOU WIN!!!!";
        Camera.main.GetComponent<CamControl>().Success();
        StartCoroutine(ProceedToMenu());
    }

	// Reset the game 5 seconds after it's over.
    IEnumerator ProceedToMenu() {
        yield return new WaitForSeconds(5);
        gameLost = false;
        ENEMIES = 0;
        Enemy.ENEMYSCALE = 1;
        Enemy.SPEEDSCALE = 1;
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

	// For each wave, increase the scaling of all enemies to ensure the player faces increasing challenge.
    void IncrementWave() {
        currentWave++;
        Enemy.ENEMYSCALE *= WAVEHPSCALE;
        Enemy.SPEEDSCALE *= WAVESPEEDSCALE;
        waveDelay *= WAVEDELAYSCALE;
        numEnemies = (int) (numEnemies * WAVENUMSCALE);
    }

    public void LoseGame() {
        gameLost = true;
        Camera.main.GetComponent<CamControl>().Failure();
        StartCoroutine(ProceedToMenu());
    }
}
