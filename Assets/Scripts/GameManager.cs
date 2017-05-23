using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	public int debugEnable = 0;
	public int debugDisable = 0;

	private Text levelText;
	private GameObject levelImage;
	private Text foodText;
	private int level = 1;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;


	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	private void OnLevelWasLoaded(int index){
		level++;
		InitGame();
	}
		

//	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
//		Debug.Log (level);
//		level++;
//		Debug.Log (level);
//		InitGame ();
//	}
//
//	void OnEnable(){
//		SceneManager.sceneLoaded += OnLevelFinishedLoading;
//		debugEnable++;
//		Debug.Log ("OnEnable: " + debugEnable);
//
//	}
//	void OnDisable(){
//		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
//		debugDisable++;
//		Debug.Log ("OnDisable: " + debugDisable);
//	}



	void InitGame(){

		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		foodText = GameObject.Find ("FoodText").GetComponent<Text> ();
		foodText.text = "Food: " + playerFoodPoints;
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		enemies.Clear ();
		Debug.Log ("Init Game: " + level);
		boardScript.SetupScene (level);
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
		doingSetup=false;
	}

	public void GameOver(){
		levelText.text = "After " + level + " days, you've starved to death.";
		levelText.fontSize = 14;
		levelImage.SetActive (true);
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		foodText.text = "Food: " + playerFoodPoints;
		if (enemiesMoving || doingSetup)
			return;
		StartCoroutine (MoveEnemies ());
		
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies [i].moveTime);

		}
		playersTurn = true;
		enemiesMoving = false;
			 
	}

}
