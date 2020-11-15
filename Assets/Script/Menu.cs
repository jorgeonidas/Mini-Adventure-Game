using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	[SerializeField] private GameObject tanker;
	[SerializeField] private GameObject soldier;
	[SerializeField] private GameObject ranger;
	[SerializeField] private GameObject hero;

	private Animator tankerAnim;
	private Animator soldierAnim;
	private Animator rangerAnim;
	private Animator heroAnim;

	void Awake(){
		Assert.IsNotNull (hero);
		Assert.IsNotNull (tanker);
		Assert.IsNotNull (ranger);
		Assert.IsNotNull (soldier);
	}
	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		heroAnim = hero.GetComponent<Animator> ();
		tankerAnim = tanker.GetComponent<Animator> ();
		soldierAnim = soldier.GetComponent<Animator> ();
		rangerAnim = ranger.GetComponent<Animator> ();

		StartCoroutine (showcase());
	}
		
	IEnumerator showcase(){
		yield return new WaitForSeconds (1f);
		heroAnim.Play ("SpinAtack");
		yield return new WaitForSeconds (1f);	
		tankerAnim.Play ("Atack");
		yield return new WaitForSeconds (1f);
		soldierAnim.Play ("Atack");
		yield return new WaitForSeconds (1f);
		rangerAnim.Play ("Atack");
		yield return new WaitForSeconds (1f);
		StartCoroutine (showcase());
	}

	public void Battle(){
		SceneManager.LoadScene ("Level");
	}

	public void Quit(){
		Application.Quit ();
	}
}
