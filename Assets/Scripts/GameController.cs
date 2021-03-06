using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
//	public Text restartText;
	public Text gameOverText;
	public GameObject restartButton;

	private bool gameOver;
	private bool restart;
	private int score;

	void Start(){
		gameOver = false;
		restart = false;
//		restartText.text = "";
		restartButton.SetActive (false);
		gameOverText.text = "";
		score = 0;
		UpdateScore();
		StartCoroutine (SpawnWaves ());
	}

//	void Update (){
//		if (restart) {
//			if (Input.GetKeyDown (KeyCode.R)) {
//				Application.LoadLevel (Application.loadedLevel);
//			}
//		}
//	}

	IEnumerator SpawnWaves (){
		yield return new WaitForSeconds (startWait);
		while (true){
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver) {
				restartButton.SetActive (true);
//				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}

	void UpdateScore (){
		scoreText.text = "Score: " + score;
	}

	public void AddScore (int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

	public void GameOver(){
		gameOverText.text = "Game Over!";
		gameOver = true;
	}

	public void RestartGame () {
		Application.LoadLevel (Application.loadedLevel);
	}
}
