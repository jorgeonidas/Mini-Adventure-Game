using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private LayerMask layerMask;
	private CharacterController characterController;
	private Vector3 currentLookTarget = Vector3.zero;
	private Animator anim;
	private BoxCollider[] swordColliders;
	private GameObject fireTrail;
	private ParticleSystem fireTrailParticles;

	// Use this for initialization
	void Start () {
		//buscar objeto por tag
		fireTrail = GameObject.FindGameObjectWithTag ("Fire") as GameObject;
		fireTrail.SetActive (false);
		characterController = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
		swordColliders = GetComponentsInChildren<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.GameOver) {
			Vector3 moveDirection = new Vector3 (Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
			characterController.SimpleMove (moveDirection * moveSpeed);
			//animaciones
			if (moveDirection == Vector3.zero) {
				anim.SetBool ("IsWalking", false);
			} else {
				anim.SetBool ("IsWalking", true);
			}

			if(Input.GetMouseButtonDown(0)){
				anim.Play ("DoubleChop");
			}
			if (Input.GetMouseButtonDown (1)) {
				anim.Play ("SpinAtack");
			}
		}
	}

	void FixedUpdate(){

		if (!GameManager.Instance.GameOver) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//dibujar el rayo
			Debug.DrawRay (ray.origin, ray.direction *500, Color.blue);
			//si el rayo hizo hit al layerMask entonces asignaremos ese punto a la posicion objetivo
			if (Physics.Raycast (ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore/*si hay otros colliders ignoralos*/)) {
				if(hit.point != currentLookTarget){
					currentLookTarget = hit.point;
				}

				Vector3 targetPosition = new Vector3 (currentLookTarget.x, transform.position.y, currentLookTarget.z);
				//calculamos y asignamos la nueva rotacion interpolando del angulo origen al angulo final
				Quaternion targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime *10f);
			}
		}

	}

	//Animation Events Model->"animacion"->Edit->Events
	public void BeginAttack(){
		foreach (var weapon in swordColliders) {
			weapon.enabled = true;
		}
	}

	public void EndAttack(){
		foreach (var weapon in swordColliders) {
			weapon.enabled = false;
		}
	}

	public void SpeedPowerUp(){
		StartCoroutine (fireTrailRoutine ());
	}

	IEnumerator fireTrailRoutine(){
		fireTrail.SetActive (true);
		moveSpeed = 10f;

		yield return new WaitForSeconds (5f);
		moveSpeed = 5f;
		//apagar la emision para que las partigulas se apagen progresivamente
		fireTrailParticles = fireTrail.GetComponent<ParticleSystem> ();
		var em = fireTrailParticles.emission;
		em.enabled = false;

		//despues de 3 segundos desactivamos el sistema de particulas
		yield return new WaitForSeconds (3f);
		em.enabled = true;
		fireTrail.SetActive (false);
	}
}
