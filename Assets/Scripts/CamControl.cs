using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

    public GameObject[] turretPrefabs;
    public GameObject[] dummyTurretPrefabs;
    private GameObject[] dummyTurrets;
    private GameObject player;
    private Vector3 mouseOverPoint;
    private GameObject turretDummy = null;
    private int ammo = 6;
    private static float PLACERANGE = 15;
    private static Vector3 GONE = new Vector3(0, -100, 0);
    private float currAngle = 15;
    private AudioSource music;
    private AudioSource reload;
    private AudioSource success;
    private AudioSource failure;

    private short currentTurret = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
		// The "dummyTurrets" are the transparent version of the turrets that display when the player is trying to place a turret.
        dummyTurrets = new GameObject[dummyTurretPrefabs.Length];
        for (int i = 0; i < dummyTurretPrefabs.Length; i++) {
            dummyTurrets[i] = (GameObject) Instantiate(dummyTurretPrefabs[i], GONE, Quaternion.identity);
        }
        AudioSource[] sources = GetComponents<AudioSource>();
        music = sources[0];
        reload = sources[1];
        success = sources[2];
        failure = sources[3];
    }
	
	void FixedUpdate () {
		// Here we're placing the camera but being sure to obey walls so the camera will not simply display the other side of a wall (it gets closer to the player to avoid the wall).
        Vector3 target = player.transform.position + player.transform.rotation * Quaternion.Euler(currAngle, 0, 0) * new Vector3(0, 3, -3);
        RaycastHit hitInfo;
        if (Physics.Raycast(player.transform.position, target - player.transform.position, out hitInfo, Vector3.Magnitude(target - player.transform.position))) {
            target = hitInfo.point + hitInfo.normal * 0.5f;
        }
        transform.position = target;
        transform.LookAt(player.transform.position + Vector3.up + player.transform.forward);
	}

    void Update() {
		// Check if the user selected a turret to place.
        SelectTurret();

		// Adjust camera angle based on mouse Y movement (mouse X movement turns the player which also turns the camera).
        currAngle -= Input.GetAxis("Mouse Y") * Settings.msens * 0.5f;

		// We're done if we don't have a dummy turret.
        if (turretDummy == null) {
            return;
        }

        if (ammo == 0) {
            RemoveDummy();
        } else if (ammo > 0) {
			// Repositioning the turret ghost to sit wherever the user is aiming.
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
                Vector3 spawnPosition = hitInfo.point + hitInfo.normal * 0.6f;
                mouseOverPoint = new Vector3(spawnPosition.x, 0, spawnPosition.z);
            }
            if (Vector3.Magnitude(player.transform.position - mouseOverPoint) > PLACERANGE) {
                turretDummy.transform.position = GONE;
            } else {
                turretDummy.transform.position = mouseOverPoint;
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
					// If the user clicks and the turret ghost is in a valid position, replace the turret ghost with an actually-instantiated turret.
                    if (!Physics.CheckSphere(mouseOverPoint, 0.5f, ~(1 << 8), QueryTriggerInteraction.Ignore)) {
                        Instantiate(turretPrefabs[currentTurret], mouseOverPoint, Quaternion.identity);
                        ammo--;
                        RemoveDummy();
                    }
                }
            }
        }
    }

    void OnGUI() {
        GUI.Box(new Rect(0, 0, 200, 100), "Ammo: " + ammo.ToString() + "\n1 - Single target turret\n2 - Area turret\n3 - Sludge field");
    }

    private void SelectTurret() {
		// Set the turretDummy to be the transparent turret of whatever we selected.
        if (Input.GetKey(KeyCode.Alpha1)) {
            currentTurret = 0;
            RemoveDummy();
            turretDummy = dummyTurrets[currentTurret];
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            currentTurret = 1;
            RemoveDummy();
            turretDummy = dummyTurrets[currentTurret];
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            currentTurret = 2;
            RemoveDummy();
            turretDummy = dummyTurrets[currentTurret];
        } else if (Input.GetKey(KeyCode.Mouse1)) {
            RemoveDummy();
        }
    }

    public void AddAmmo(int addAmmo) {
        ammo += addAmmo;
        reload.Play();
    }

    private void RemoveDummy() {
        if (turretDummy != null)
            turretDummy.transform.position = GONE;
        turretDummy = null;
    }

    public void Success() {
        success.Play();
    }

    public void Failure() {
        failure.Play();
    }
}
