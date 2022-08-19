using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public int hp = 10;
	public int gold = 50;
	public int goldPower = 1;
	public bool gameOver = false;
	public bool gameEnd = false;
	public bool pause = false;

	public Transform hpObj;
	public Text goldText;
	public GameObject glass;
	public GameObject pauseGlass;
	public GameObject gameOverText;
	public GameObject gameEndText;
	public AudioSource audioSource;
	public AudioClip gameEndSound;

    private void Update()
    {
		if (gameEnd || gameOver) return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pause = !pause;

			glass.SetActive(pause);
			pauseGlass.SetActive(pause);
		}

		if (pause) return;

		goldText.text = string.Format("{0}¿ø", gold);

		SetHP();

		if (hp <= 0)
		{
			GameOver();
		}
    }

	private void SetHP()
	{
		for (int count = 0; count < hpObj.childCount; count++)
		{
			hpObj.GetChild(count).gameObject.SetActive(false);
		}
		for (int count = 0; count < hp; count++)
		{
			hpObj.GetChild(count).gameObject.SetActive(true);
		}
	}

	private void SaveScore()
	{
		Ranking ranking = FindObjectOfType<Ranking>();

		int score = 0;

		score += hp * 100;

		score += gold * 50;

		score += FindObjectOfType<EnemySpawner>().stage * 100;

		Rank rank = new Rank(score);

		ranking.ranks.Add(rank);
		
	}

	public void GameEnd()
	{
		Time.timeScale = 0;
		glass.SetActive(true);
		gameEndText.SetActive(true);

		StartCoroutine(SendEnd());
	}

	private IEnumerator SendEnd()
	{
		audioSource.volume = 0.005f;
		audioSource.PlayOneShot(gameEndSound);
		yield return new WaitForSeconds(2);

		SaveScore();
		FindObjectOfType<Ranking>().Sort_And_Save();

		SceneManager.LoadScene("End");
	}

	private void GameOver()
	{
		gameOver = true;
		
		glass.SetActive(true);
		gameOverText.SetActive(true);

		StartCoroutine(SendEnd());
	}
}
