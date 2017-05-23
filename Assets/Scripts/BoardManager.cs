using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {


	[Serializable]
	public class Count{
		public int minimum;
		public int maximum;
		public Count (int min, int max){
			minimum = min;
			maximum = max;

		}
	}



	public int row = 8;
	public int column = 15;

	public Vector3 startPosition = new Vector3 (0, 6, 0f);

	public Count wallCount = new Count (5, 9);

	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private bool enemiesSpawning;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();
		
	void InitialiseList(){
		gridPositions.Clear ();
		for (int x = 0; x < column-1; x++) {
			for (int y = 0; y < row - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		for (int x = -1; x < column + 1; x++) {
			for (int y = -1; y < row + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if ((x == -1 && y != row - 1) || y == -1 || ((x == 1 || x == column - 2) && y >= 1 && y <= row-2) || ((y == 1 || y == row-2) && x >= 1 && x <= column - 2) || x == column || (y == row && x != 0)) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				}
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (1, gridPositions.Count);
		Debug.Log (gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}


	IEnumerator SpawnEnemy(GameObject[] tileArray, int objectCount){
		print ("spawn enemy inside");
		enemiesSpawning = true;
		yield return new WaitForSeconds (1.5f);
		for (int i = 0; i < objectCount; i++) {
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			print (startPosition);
			Instantiate (tileChoice, startPosition, Quaternion.identity);
			yield return new WaitForSeconds (1.5f);
		}
		enemiesSpawning = false;
	}

	public void SetupScene(int level)
	{
		BoardSetup ();
		InitialiseList ();
		int enemyCount = 10;
		StartCoroutine (SpawnEnemy(enemyTiles, enemyCount));
		Instantiate (exit, new Vector3 (column - 1, row - 1, 0f), Quaternion.identity);

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
