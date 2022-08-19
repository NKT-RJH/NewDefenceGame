using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
	public int stage = 0;
	public int faze = 0;
	public float leastTime = 0f;
	public float spawnTime = 0f;
	public float restTime = 0;

	public bool rest = false;

	// 이거 만들기!!
	//public int prepareTime

	public Text fazeText, fazeTimeText;
	public Text stageText, stageTimeText;
	public GameObject nextFaze;

	public GameManager gameManager;
	public Item item;

	private float allTime = 0f;

	public int code = 0;

	public List<Stage> stages = new List<Stage>();

	public GameObject[] enemyPrefabs = new GameObject[4];

	public List<Transform> enemys = new List<Transform>();

	private void Start()
	{
		restTime = stages[stage].restTime;
		leastTime = stages[stage].fazes[faze].fazeTime;
	}

	private IEnumerator Rest()
	{
		rest = true;
		while (restTime > 0)
		{
			restTime -= Time.deltaTime;
			fazeTimeText.text = string.Format("{0}분 {1}초", (int)restTime / 60, (int)restTime % 60);
			yield return null;
		}
		rest = false;
		item.buyCount = 10;
	}

	private void Update()
	{
		if (gameManager.gameEnd || gameManager.gameOver || gameManager.pause) return;

		allTime += Time.deltaTime;
		stageTimeText.text = string.Format("{0}분 {1}초", (int)allTime / 60, (int)allTime % 60);

		if (rest) return;

		if (restTime > 0)
		{
			StartCoroutine(Rest());
		}

		leastTime -= Time.deltaTime;
		spawnTime += Time.deltaTime;

		fazeText.text = string.Format("Wave {0}", faze + 1);
		stageText.text = string.Format("Stage {0}", stage + 1);

		fazeTimeText.text = string.Format("{0}분 {1}초", (int)leastTime / 60, (int)leastTime % 60);
		
		if (leastTime <= 0)
		{
			spawnTime = 0;
			faze++;
			if (faze < stages[stage].fazes.Count)
			{
				leastTime = stages[stage].fazes[faze].fazeTime;
				item.buyCount = 10;
			}
			else
			{
				faze--;
				stage++;
				if (stage >= stages.Count)
				{
					stage--;
					leastTime = 0;
					StartCoroutine(IsRealEnd());
					return;
				}
				restTime = stages[stage].restTime;
			}
		}


		int flag = 0;
		for (int count = 0; count < 4; count++)
		{
			flag += stages[stage].fazes[faze].enemyCount[count] <= 0 ? 1 : 0;
		}

		if (flag >= 4)
		{
			nextFaze.SetActive(true);
			return;
		}

		if (spawnTime >= stages[stage].fazes[faze].spawnTime)
		{
			spawnTime = 0;

			while (true)
			{
				int random = UnityEngine.Random.Range(0, 4);

				if (stages[stage].fazes[faze].enemyCount[random] <= 0) continue;

				if (random == 0)
				{
					StartCoroutine(SpawnFighters());
					break;
				}
				else
				{
					stages[stage].fazes[faze].enemyCount[random]--;
					GameObject game = Instantiate(enemyPrefabs[random]);
					game.transform.GetChild(0).GetComponent<Enemy>().code = code++;
					enemys.Add(game.transform);
					break;
				}
			}
		}
	}

	public void NextFaze()
	{
		leastTime = 0;
		nextFaze.SetActive(false);
	}

	private IEnumerator IsRealEnd()
	{
		while (enemys.Count > 0)
		{
			yield return null;
		}

		gameManager.gameEnd = true;
		gameManager.GameEnd();
	}

	private IEnumerator SpawnFighters()
	{
		for (int count = 0; count < 3; count++)
		{
			stages[stage].fazes[faze].enemyCount[0]--;
			GameObject game = Instantiate(enemyPrefabs[0]);
			game.transform.GetChild(0).GetComponent<Enemy>().code = code++;
			enemys.Add(game.transform);
			yield return new WaitForSeconds(0.3f);
		}
	}
}

[Serializable]
public class Faze
{
	public int[] enemyCount = new int[4];

	public float spawnTime;

	public float fazeTime;
}

[Serializable]
public class Stage
{
	public List<Faze> fazes = new List<Faze>();

	public float restTime;
}