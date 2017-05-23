using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;
	private int xDirection, yDirection;



	protected virtual void Start (){
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
		xDirection = 1;
		yDirection = 0;

	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;

	}


	protected IEnumerator SmoothMovement (Vector3 end){
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> ()
		where T : Component
	{
		print ("outside"+ xDirection + " " + yDirection);
		RaycastHit2D hit;
		bool canMove = Move (xDirection, yDirection ,out hit);
		print (canMove);

//		if (hit.transform == null) {
//			print (hit.transform);
//			return;
//		}

//		T hitComponent = hit.transform.GetComponent<T>();

//		print (hitComponent);


//		if (!canMove && hitComponent != null) {
		if (!canMove) {
			int[] directions = TurnRight (xDirection,yDirection);
			xDirection = directions [0];
			yDirection = directions [1];
			print (xDirection + " " + yDirection);
			Move (xDirection, yDirection ,out hit);
		}
		return;

	}

	protected int[] TurnRight(int xDir, int yDir){
		/*
			 x = 1, y = 0 (going right)
			 x = -1, y = 0 (going left)
			 x = 0, y = 1 (going up)
			 x = 0, y = -1 (going down)
			 
		*/
		int[] directions = new int[2];
		int tempY = yDir;

		if (yDir != 0) {
			yDir = 0;
		} else if (xDir == 1) {
			yDir = -1;
		} else {
			yDir = 1;
		}

		if (xDir != 0) {
			xDir = 0;
		} else if (tempY == -1) {
			xDir = -1;
		} else {
			xDir = 1;
		}

		directions [0] = xDir;
		directions [1] = yDir;
		return directions;

	}

		
}
