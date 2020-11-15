using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance = null;
	[SerializeField] GameObject player;
	[SerializeField] GameObject[] spawnPoints;
	[SerializeField] GameObject[] powerUpSpawns;
	[SerializeField] GameObject tanker;
	[SerializeField] GameObject ranger;
	[SerializeField] GameObject soldier;
	[SerializeField] GameObject arrow;
	[SerializeField] GameObject healthPowerUp;
	[SerializeField] GameObject speedPowerUp;
	[SerializeField] Text levelText;
	[SerializeField] Text nextWaveText;
	[SerializeField] Text endGameText;
	[SerializeField] int maxPowerUps = 4;
	[SerializeField] int finalLevel = 20;


	private bool gameOver = false;
	private int currentLevel; 
	private float generatedSpawnTime = 1;
	private float currentSpawnTime = 0;
	private float powerUpSpawnTime = 60;
	private float currentPowerUpSpawnTime = 0;
	private GameObject newEnemy;
	private GameObject newPowerUp;
	private int powerups = 0;

	private bool isRestTime;
	private float restTime;

	private List<EnemyHealth> enemies = new List<EnemyHealth> ();
	private List<EnemyHealth> killedEnemies = new List<EnemyHealth> ();

	public void RegiterEnemy(EnemyHealth enemy){
		enemies.Add (enemy);
	}

	public void KilledEnemy(EnemyHealth enemy){
		killedEnemies.Add (enemy);
	}

	public void registerPowerUp(){
		powerups++;
	}

	public bool GameOver{
		get{return gameOver;}
	}

	public GameObject Player{
		get{return player;}
	}

	public GameObject Arrow{
		get{return arrow;}
	}


	void Awake(){
		//para asegurar solo una instancia del GameManager a la vez
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
		//DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		endGameText.GetComponent<Text> ().enabled = false;
		nextWaveText.enabled = false;
		restTime = 5f;
		isRestTime = false;
		currentLevel = 1;
		StartCoroutine (spawn ());
		StartCoroutine (spawnPowerUp ());
	}
	
	// Update is called once per frame
	void Update () {
		currentSpawnTime += Time.deltaTime;
		currentPowerUpSpawnTime += Time.deltaTime;
		//probando cuenta regresiva
		if (isRestTime) {			
			restTime -= Time.deltaTime;
			//Debug.Log (Mathf.RoundToInt(restTime));
			nextWaveText.enabled = true;
			nextWaveText.text = "Nex Wave In " + Mathf.RoundToInt(restTime);
		} else {
			nextWaveText.enabled = false;
			restTime = 5f;
		}
	}

	public void playerHit(int currentHP){
		if (currentHP > 0) {
			gameOver = false;
		} else {
			gameOver = true;
			StartCoroutine (endGame ("Defeat!"));
		}
	}

	IEnumerator spawn(){
		//checkear si el spawntime es mayor al currentTime
			//si hay menos enemigos en la pantalla que el numero de nivel actual, seleccionar un spawnpoint aleatoriamente
			//y spawnear un enemigo aleatoriamente
		//si hemos matado la misma cantidad de enemigos que el numero del el nivel actual, limpiar las listas enemies y killedEnemies
		//incrementar el nivel actual y comenzar de nuevo
		if (currentSpawnTime > generatedSpawnTime) {
			currentSpawnTime = 0;
			if (enemies.Count < currentLevel) {
				isRestTime = false;
				GameObject spawnLocation = spawnPoints [Random.Range (0, spawnPoints.Length-1)];
				//instanciar un enemigo aleatoriamente
				int randomEnemy = Random.Range(0,3);
				if (randomEnemy == 0) {
					newEnemy = Instantiate (soldier) as GameObject;
				} else if (randomEnemy == 1) {
					newEnemy = Instantiate (ranger) as GameObject;
				} else if (randomEnemy == 2) {
					newEnemy = Instantiate (tanker) as GameObject;
				}
				newEnemy.transform.position = spawnLocation.transform.position;
			}
			//si ya superamos el nivel actual pero no el final
			if (killedEnemies.Count == currentLevel && currentLevel != finalLevel) {
				enemies.Clear ();
				killedEnemies.Clear ();
				//tiempo de descanso entre niveles
				isRestTime = true;
				yield return new WaitForSeconds (5f);
				currentLevel += 1;
				levelText.text = "Level: " + currentLevel;
			}
			//si ya superamos todas las ordas
			if (killedEnemies.Count == finalLevel) {
				StartCoroutine (endGame ("Victory!"));
			}
		}
		yield return null;
		StartCoroutine (spawn());
	}

	IEnumerator spawnPowerUp(){
		if (currentPowerUpSpawnTime > powerUpSpawnTime) {
			currentPowerUpSpawnTime = 0;
			if (powerups < maxPowerUps) {
				int randIndex = Random.Range (0, powerUpSpawns.Length-1);
				GameObject powerSpawLocation = powerUpSpawns [randIndex];
				int randPowerUp = Random.Range (0, 2);
				if (randPowerUp == 0) {
					newPowerUp = Instantiate (healthPowerUp) as GameObject;
				} else if (randPowerUp == 1) {
					newPowerUp = Instantiate (speedPowerUp) as GameObject;
				}

				newPowerUp.transform.position = powerSpawLocation.transform.position;
			}
		}
		yield return null;
		StartCoroutine (spawnPowerUp ());
	}

	IEnumerator endGame(string outcome){
		endGameText.text = outcome;
		endGameText.GetComponent<Text> ().enabled = true;

		yield return new WaitForSeconds (3f);

		SceneManager.LoadScene ("GameMenu");
	}
}
