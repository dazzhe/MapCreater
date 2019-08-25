using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	private CharacterController controller;
	private float velocity = 0f;
	public GameObject came;
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;
	private EditMap em;

	// Use this for initialization
	void Start () {
		em = GetComponent<EditMap> ();
		controller = gameObject.GetComponent<CharacterController> ();
		m_CharacterTargetRot = transform.localRotation;
		m_CameraTargetRot = came.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (!em.modeUI) {
			if (Input.GetKeyDown (KeyCode.R)) {
				transform.position = Vector3.up * 20f;
			}
			Move ();
			LookRotation ();
		}
	}

	void Move(){
		Vector3 motion;
		motion.x = Input.GetAxis ("Horizontal");
		motion.z = Input.GetAxis ("Vertical");
		motion.y = 0;
		motion = 0.3f * motion.normalized;
		if (Input.GetKey (KeyCode.LeftShift)) {
			motion*= 2f;
		}
		if (Input.GetKey (KeyCode.Space)) {
			if (!Input.GetKey (KeyCode.F)) {
				controller.Move (Vector3.up * 0.2f);
				velocity = 0f;
			}
		} else {
			if (controller.isGrounded) {
				motion.y =-0.1f;
				velocity = 0f;
			} else {
				velocity -= 0.03f;
				motion.y = velocity;
			}
		}
		motion = transform.TransformVector (motion);
		controller.Move (motion);
	}

	void LookRotation(){
		float yRot = Input.GetAxis("Mouse X") * XSensitivity;
		float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
		
		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		transform.rotation = m_CharacterTargetRot;
		came.transform.localRotation = m_CameraTargetRot;
	}
}
