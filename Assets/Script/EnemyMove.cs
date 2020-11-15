using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyMove : MonoBehaviour {

	private Transform player;
	private NavMeshAgent nav;
	private Animator anim;
	private EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		nav = GetComponent<NavMeshAgent> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		player = GameManager.Instance.Player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!GameManager.Instance.GameOver && enemyHealth.IsAlive)
			nav.SetDestination (player.position);
		else if ((!GameManager.Instance.GameOver || GameManager.Instance.GameOver) && !enemyHealth.IsAlive) {
			nav.enabled = false;
		} else {
			nav.enabled = false;
			anim.Play ("Idle");
		}
	}
}
