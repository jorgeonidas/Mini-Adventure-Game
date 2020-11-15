using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAtack : MonoBehaviour {

	[SerializeField] private float range = 3f;
	[SerializeField] private float timeBetweenAtacks = 1f;
	[SerializeField] Transform fireLocation;

	private Animator anim;
	private GameObject player;
	private bool playerInRange;
	private EnemyHealth enemyHealt;
	private GameObject arrow;

	// Use this for initialization
	void Start () {
		arrow = GameManager.Instance.Arrow;
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
		anim.SetBool ("PlayerInRange", playerInRange);
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

	private void rotateTowards(Transform player){
		//calculo vector direccion ( objetivo - origen)
		Vector3 direction = (player.position - transform.position).normalized;
		//calculo la rotacion a la cual va a mirar
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		//con lerp se interpola entre una rotacion origen a una rotacion destino a una cierta velocidad por tiempo
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 10f);
	}

	public void FireArrow(){
		//instancio la flecha
		//la posiciono en el punto donde esta la ballesta
		//le asigno la rotacion del enemigo
		//aplico velocity en su componente z (transform.forward)
		GameObject newArrow = Instantiate (arrow) as GameObject;
		newArrow.transform.position = fireLocation.position;
		newArrow.transform.rotation = transform.rotation;
		newArrow.GetComponent<Rigidbody> ().velocity = transform.forward * 25f;
	}
}
