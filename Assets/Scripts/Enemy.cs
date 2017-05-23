using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	private int armor;
	private int health;
	private float speed;

	private int xDirection, yDirection;

	protected override void Start () {

		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent<Animator> ();

		target = GameObject.FindGameObjectWithTag ("Player").transform;

		xDirection = 1;
		yDirection = 0;

		base.Start ();
	}

	protected override void AttemptMove<T> ()
	{
		base.AttemptMove<T> ();
		return;
	}

//	public void MoveEnemy(){
//		int xDir = 0;
//		int yDir = 0;
//		if (Mathf.Abs (target.position.x - transform.position.x) <= float.Epsilon) {
//			yDir = target.position.y > transform.position.y ? 1 : -1;
//		} else {
//			xDir = target.position.x > transform.position.x ? 1 : -1;
//		}
//		AttemptMove<Player> (xDir, yDir);
//	}
	public void MoveEnemy(){
		AttemptMove<Wall> ();
	}

	private void TurnRight(){
		/*
			 x = 1, y = 0 (going right)
			 x = -1, y = 0 (going left)
			 x = 0, y = 1 (going up)
			 x = 0, y = -1 (going down)
			 
		*/
		int tempY = yDirection;

		if (yDirection != 0) {
			yDirection = 0;
		} else {
			yDirection = xDirection;
		}

		if (xDirection != 0) {
			xDirection = 0;
		} else {
			xDirection = tempY;
		}


	}
}
