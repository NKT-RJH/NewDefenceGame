using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKey : MonoBehaviour
{
	private Dictionary<KeyCode, Action> actionsByKey = new Dictionary<KeyCode, Action>();

	public GameManager gameManager;
	public EnemySpawner enemySpawner;

	private bool stopFlag = false;

	private List<KeyValuePair<int, float>> enemys = new List<KeyValuePair<int, float>>();

	private List<Stage> backUpStages = new List<Stage>();

	private void Start()
	{
		backUpStages = enemySpawner.stages;

		// 치트키는 F1 에서부터 F7 까지! 일단은 테스트 함수로..,
		actionsByKey.Add(KeyCode.F1, StopEnemy);
		actionsByKey.Add(KeyCode.F2, GoldIncrease);
		actionsByKey.Add(KeyCode.F3, KillAll);
		actionsByKey.Add(KeyCode.F4, KillAll_Gold);
		actionsByKey.Add(KeyCode.F5, GotoTitle);
		actionsByKey.Add(KeyCode.F6, GotoFirstStage);
		actionsByKey.Add(KeyCode.F7, GotoSecondStage);
	}

	private void Update()
	{
		if (gameManager.gameEnd || gameManager.gameOver || gameManager.pause) return;

		if (!Input.anyKeyDown) return;
		
		foreach (KeyValuePair<KeyCode, Action> keyValue in actionsByKey)
		{
			if (Input.GetKeyDown(keyValue.Key))
			{
				keyValue.Value.Invoke();
			}
		}
	}

	private void StopEnemy()
	{
		if (!stopFlag)
		{
			foreach (Transform path in enemySpawner.enemys)
			{
				Enemy enemy = path.GetChild(0).GetComponent<Enemy>();
				enemys.Add(new KeyValuePair<int, float>(enemy.code, enemy.speed));
				enemy.speed = 0;
			}
			stopFlag = true;
			enemySpawner.rest = true;
		}
		else
		{
			for (int count = 0; count < enemys.Count; count++)
			{
				for (int count1 = 0; count1 < enemySpawner.enemys.Count; count1++)
				{
					try
					{
						Enemy enemy = enemySpawner.enemys[count1].GetChild(0).GetComponent<Enemy>();
						if (enemy.code.Equals(enemys[count].Key))
						{
							enemy.speed = enemys[count].Value;
							break;
						}
					}
					catch (NullReferenceException) { }
				}
			}
			enemys = new List<KeyValuePair<int, float>>();
			stopFlag = false;
			enemySpawner.rest = false;
		}
	}

	private void GoldIncrease()
	{
		gameManager.gold += 100;
	}

	private void KillAll()
	{
		int max = enemySpawner.enemys.Count;
		for (int count = 0; count < max; count++)
		{
			Transform temporary = enemySpawner.enemys[0];
			enemySpawner.enemys.Remove(temporary);
			Destroy(temporary.gameObject);
		}
	}

	private void KillAll_Gold()
	{
		foreach (Transform path in enemySpawner.enemys)
		{
			path.GetChild(0).GetComponent<Enemy>().hp = 0;
		}
	}

	private void GotoTitle()
	{
		SceneManager.LoadScene("Title");
	}

	private void GotoFirstStage()
	{
		KillAll();
		enemySpawner.stages = backUpStages;
		enemySpawner.code = 0;
		enemySpawner.stage = 0;
		enemySpawner.faze = 0;
		enemySpawner.restTime = enemySpawner.stages[enemySpawner.stage].restTime;
		enemySpawner.leastTime = enemySpawner.stages[enemySpawner.stage].fazes[enemySpawner.faze].fazeTime;
		enemySpawner.spawnTime = 0;

		enemySpawner.stageText.text = string.Format("Stage {0}", enemySpawner.stage + 1);
	}

	private void GotoSecondStage()
	{
		KillAll();
		enemySpawner.stages = backUpStages;
		enemySpawner.stage = 1;
		enemySpawner.faze = 0;
		enemySpawner.restTime = enemySpawner.stages[enemySpawner.stage].restTime;
		enemySpawner.leastTime = enemySpawner.stages[enemySpawner.stage].fazes[enemySpawner.faze].fazeTime;
		enemySpawner.spawnTime = 0;
		enemySpawner.code = 0;

		enemySpawner.stageText.text = string.Format("Stage {0}", enemySpawner.stage + 1);
	}
}
