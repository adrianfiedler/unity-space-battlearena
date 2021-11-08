using UnityEngine;
using System.Collections;
using CnControls;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
	private AudioSource audioSource;
    public float speed;
    public Boundary boundary;
    public float tilt;

	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;
    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
	public float turnSpeed;
	public Joystick joystick;

	private Quaternion calibrationQuaternion;

	public ParticleSystem flareParticleSystem;
	public ParticleSystem coreParticleSystem;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();
		CalibrateAccellerometer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (areaButton.CanFire () && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, transform.rotation);
			audioSource.Play();
        }
	}

	//Used to calibrate the Input.acceleration input
	void CalibrateAccellerometer(){
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	//Get the 'calibrated' value from the Input
	Vector3 FixAccelleration(Vector3 acceleration){
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}

    void FixedUpdate()
    {
		Move ();
		Rotate ();
		rotateParticleSystems ();
    }

	private void Move ()
	{
		// float moveHorizontal = Input.GetAxis("Horizontal");
		// float moveVertical = Input.GetAxis("Vertical");

		// Vector2 direction = touchPad.GetDirection ();
		// Vector2 direction = new Vector2(joystick.JoystickInput.x,joystick.JoystickInput.y);
		Vector2 direction = new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));
		Vector3 movement = new Vector3 (direction.x, 0.0f, direction.y);

		rb.velocity = movement * speed ;

		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		); 
	}

	private void Rotate (){
		Vector2 direction = new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));
		Vector3 targetDir = new Vector3(direction.x, 0.0f, direction.y);
		float step = turnSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		rb.rotation = Quaternion.LookRotation(newDir);

		// rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	private void rotateParticleSystems(){
		coreParticleSystem.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
		flareParticleSystem.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
	}
}
