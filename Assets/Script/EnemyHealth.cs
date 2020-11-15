using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

	[SerializeField] private int startingHealth = 20;
	[SerializeField] private float timeSinceLastHit = 0.5f;
	[SerializeField] private float dissapearSpeed = 2f;

	private AudioSource audio;
	private float timer = 0f;
	private Animator anim;
	private NavMeshAgent nav;
	private bool isAlive;
	private Rigidbody rigidbody;
	private CapsuleCollider capsuleCollider;
	private bool dissaperarEnemy = false;
	private int currentHealth;
	private ParticleSystem blood;

	public bool IsAlive{
		get{
			return isAlive;
		}
	}
		
	// Use this for initialization
	void Start () {
		//registrar a la lista de enemigos en el GameManager una vez instanciado
		GameManager.Instance.RegiterEnemy (this);
		rigidbody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		isAlive = true;
		currentHealth = startingHealth;
		blood = GetComponentInChildren<ParticleSystem> ();
		//enemyAtack = GetComponent<EnemyAtack> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		//hundir el enemigo y en la funcion removeEnemy se destruye el gameObject
		if (dissaperarEnemy) {
			transform.Translate (-Vector3.up*dissapearSpeed*Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other){
		if (timer >= timeSinceLastHit && !GameManager.Instance.GameOver) {
			if (other.tag == "PlayerWeapon") {
				takeHit ();
				blood.Play ();
				timer = 0f;
			}
		}
	}

	public void takeHit(){		
		currentHealth -= 10;
		//Debug.Log (currentHealth);
		if (currentHealth > 0) {
			anim.Play ("Hurt");
			audio.PlayOneShot (audio.clip);
		}else if(currentHealth <= 0) {
			//Debug.Log ("Kill " + gameObject.name);
			audio.PlayOneShot (audio.clip);
			isAlive = false;
			killEnemy ();
		}
	}

	public void killEnemy(){
		//desabilito los colliders de las armas para que no se activen los Triggers por accidente
		//desabilito el resto de la fisica
		capsuleCollider.enabled = false;
		nav.enabled = false;
		//ejecuto la animacion de morir
		anim.SetTrigger ("EnemyDie");
		//rigidibody pasa a ser cinematico para "hundir" el gameObject debajo del escenario
		rigidbody.isKinematic = true;
		StartCoroutine(removeEnemy());
	}

	IEnumerator removeEnemy(){
		GameManager.Instance.KilledEnemy (this);
		//esperamos segundos desopues que el enemigo muere
		yield return new WaitForSeconds (4f);
		//empezamos a hundir el enemigo
		dissaperarEnemy = true;
		//esperamos otros segundos mas
		yield return new WaitForSeconds (2f);
		//destruimos al enemigo (gameObject)
		Destroy (gameObject);
	}
}
