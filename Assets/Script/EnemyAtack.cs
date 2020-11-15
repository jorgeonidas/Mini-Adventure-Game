using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour {

	[SerializeField] private float range = 3f;
	[SerializeField] private float timeBetweenAtacks = 1f;

	private Animator anim;
	private GameObject player;
	private bool playerInRange;
	private BoxCollider[] weaponColliders;
	private EnemyHealth enemyHealt;

	// Use this for initialization
	void Start () {
		weaponColliders = GetComponentsInChildren<BoxCollider> ();
		player = GameManager.Instance.Player;
		anim = GetComponent<Animator> ();
		StartCoroutine (atack ());
		enemyHealt = GetComponent<EnemyHealth> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance (transform.position, player.transform.position) < range && enemyHealt.IsAlive) {
			playerInRange = true;
		} else {
			playerInRange = false;
		}
		if(enemyHealt.IsAlive)
			rotateTowards (player.transform);
	}

	IEnumerator atack(){
		if (playerInRange && !GameManager.Instance.GameOver) {
			anim.Play ("Atack");
			yield return new WaitForSeconds (timeBetweenAtacks);
		}

		yield return null;
		StartCoroutine (atack ());
	}
	//llamando a los eventos que asignamos en la animacion
	public void EnemyBeginAtack(){
		foreach (var weapon  in weaponColliders) {
			weapon.enabled = true;
		}
	}

	public void EnemyEndAtack(){
		foreach (var weapon  in weaponColliders) {
			weapon.enabled = false;
		}
	}

	private void rotateTowards(Transform player){
		//calculo vector direccion ( objetivo - origen)
		Vector3 direction = (player.position - transform.position).normalized;
		//calculo la rotacion a la cual va a mirar
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		//con lerp se interpola entre una rotacion origen a una rotacion destino a una cierta velocidad por tiempo
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 10f);
	}
}
