using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    private bool started = false;
    private bool tutorial = false;

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

	void OnGUI() {
        if (!started && !tutorial) {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "START GAEM")) {
                started = true;
                SceneManager.LoadScene("main", LoadSceneMode.Single);
            }
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 25, 200, 25), "Mouse sensitivity: " + Settings.msens);
            Settings.msens = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 25), Settings.msens, 0, 10);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 75, 200, 50), "READ TUTORAIL")) {
                tutorial = true;
            }
        } else if (tutorial) {
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height),
                "HOW TO PLAY:\n" +
                "\n" +
                "Move with WASD\n" +
                "Select an item with 1, 2, or 3:\n" +
                "\t1 - Single-target high damage turret, decent range\n" +
                "\t2 - AoE low damage repeatable explosive, shorter range\n" +
                "\t3 - Sludge, slows all enemies. Does not stack.\n" +
                "Left click to place an item after selecting it.\n" +
                "\n" +
                "The weird cylinder floating near your character is telling you where ammo is.\n" +
                "Each item costs 1 ammo to place. Ammo pickups are blue capsules and give 3 ammo.\n" +
                "You can not attack, so if you're out of ammo and the enemy is catching up, you're dead.\n" +
                "Try not to run out of ammo. Enemies hurt you if they get close.\n" +
                "\n" +
                "Press shift to sprint briefly. Use it wisely.\n" +
                "\n" +
                "The game is fairly difficult, but good luck.");
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), "BACK")) {
                tutorial = false;
            }
        }
    }
}
