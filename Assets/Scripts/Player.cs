using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {
	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;

	public float restartLevelDelay = 1f;

	public Animator animator;
	public int food;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();

		food = GameManager.instance.playerFoodPoints;

		base.Start ();
	}

	protected void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	
	// Update is called once per frame
	void Update () {
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) {
			vertical = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			GameManager.instance.playersTurn = false;
//			AttemptMove<Wall> (horizontal, vertical);
		}
	}

	private void OnTriggerEnter2D( Collider2D other){
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			other.gameObject.SetActive (false);
		}
		GameManager.instance.playerFoodPoints = food;

	}

//	protected override void OnCantMove <T> (T component){
//		Debug.Log ("can't move");
//		Wall hitWall = component as Wall;
//		hitWall.DamageWall (wallDamage);
//		animator.SetTrigger ("playerChop"); //Here, playerChop turns to true, and PlayerChop animation is played
//
//	}

	private void Restart()
	{
//		SceneManager.LoadScene (0);
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoseFood(int loss)
	{
		animator.SetTrigger ("playerHit"); //Here, playerChop turns to true, and PlayerChop animation is played
		food -= loss;
		GameManager.instance.playerFoodPoints = food;
		CheckIfGameOver();

	}

//	protected override bool AttemptMove<T> (int xDir, int yDir){
//		food--;
//		GameManager.instance.playerFoodPoints = food;
//
//		base.AttemptMove<T> (xDir, yDir);
//		RaycastHit2D hit;
//		CheckIfGameOver ();
//		GameManager.instance.playersTurn = false;
//		return true;
//	}

	public void CheckIfGameOver(){
		if (food <= 0) {
			GameManager.instance.GameOver (); 
		}
	}
		


	
}
