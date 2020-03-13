using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    static float SPEED = 5;
    static float SPRINTTIME = 3;
    static float SPRINTMULT = 2;
    static float SPRINTCD = 10;

    private bool grounded = false;
    private float lastSprint = -1000;
    new private Rigidbody rigidbody;
    private AudioSource grunt;
    private AudioSource pant;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        AudioSource[] sources = GetComponents<AudioSource>();
        grunt = sources[0];
        pant = sources[1];
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
        Rotate();
    }

    void Move() {
		// Sprint, which has a cooldown.
        if (Input.GetKey(KeyCode.LeftShift) && (Time.time - lastSprint > SPRINTCD)) {
            StartCoroutine(Sprint());
            lastSprint = Time.time;
        }
		// Calculate movement speed/direction.
        Vector3 newVel = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) {
            newVel.z++;
        }
        if (Input.GetKey(KeyCode.S)) {
            newVel.z--;
        }
        if (Input.GetKey(KeyCode.A)) {
            newVel.x--;
        }
        if (Input.GetKey(KeyCode.D)) {
            newVel.x++;
        }
        if (newVel.magnitude != 0) {
            newVel = newVel.normalized * SPEED;
        }
		// Apply movement to the rigidbody by directly manipulating velocity.
        rigidbody.velocity = transform.rotation * new Vector3(newVel.x, rigidbody.velocity.y, newVel.z);
	}

    void Rotate() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * Settings.msens, 0));
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "Floor") {
            grounded = true;
        }
    }

    void OnCollisionExit(Collision c) {
        if (c.gameObject.name == "Floor") {
            grounded = false;
        }
    }

	// Show sprint state and cooldown.
    void OnGUI() {
        string msg = "";
        if (Time.time - lastSprint < SPRINTTIME) {
            msg = "SPRINTING!";
        } else if (Time.time - lastSprint < SPRINTCD) {
            msg = "Sprint CD: " + Mathf.FloorToInt(SPRINTCD - (Time.time - lastSprint)).ToString();
        } else {
            msg = "Sprint ready!";
        }
        GUI.Box(new Rect(0, Screen.height - 25, 100, 25), msg);
    }

	// Coroutine to handle sprinting for 3 seconds.
    IEnumerator Sprint() {
        grunt.Play();
        SPEED *= SPRINTMULT;
        yield return new WaitForSeconds(3);
        pant.Play();
        SPEED /= SPRINTMULT;
    }
}
