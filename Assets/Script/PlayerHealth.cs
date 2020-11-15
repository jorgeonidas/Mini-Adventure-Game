using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {
	[SerializeField] private int startinHealth = 100;
	[SerializeField] float timeSinceLastHit = 1f;
	[SerializeField] Slider healthSlider;

	private float timer = 0f;
	private CharacterController characterController;
	private Animator anim;
	private int currenHealth;
	private AudioSource audio;
	private ParticleSystem blood; 
	// Use this for initialization

	public int CurrentHealth{
		get{
			return currenHealth;
		}
		set{
			if (value < 0) {
				currenHealth = 0;
			} else {
				currenHealth = value;
			}
		}
	}

	void Awake(){
		Assert.IsNotNull (healthSlider);
	}

	void Start () {
		anim = GetComponent<Animator> ();
		characterController = GetComponent<CharacterController> ();
		currenHealth = startinHealth;
		audio = GetComponent<AudioSource> ();
		blood = GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other){
		if(timer >= timeSinceLastHit && !GameManager.Instance.GameOver ){
			if(other.tag == "Weapon"){
				takeHit ();
				timer = 0;
			}
		}
	}

	public void takeHit(){
		if (currenHealth > 0) {
			GameManager.Instance.playerHit (currenHealth);
			anim.Play ("Hurt");
			currenHealth -= 10;
			healthSlider.value = currenHealth;
			audio.PlayOneShot (audio.clip);
			blood.Play();

		}
		if (currenHealth <= 0) {
			killPlayer ();
		}
	}

	public void killPlayer(){
		GameManager.Instance.playerHit (currenHealth);
		anim.SetTrigger ("HeroDie");
		characterController.enabled = false;
		audio.PlayOneShot (audio.clip);
		blood.Play();
	}

	//powerUps
	public void powerUpHealth(){
		if (CurrentHealth <= 70) {
			CurrentHealth += 30;
		} else if(currenHealth < startinHealth) {
			currenHealth = startinHealth;
		}
		healthSlider.value = currenHealth;
	}
}
