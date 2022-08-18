using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
	public int number;
	public int hp;
	public int gold;
	public float speed;
	
	public bool boost = false;
	public bool slow = false;
	public bool itemSlow = false;
	private int targetNumber = 0;
	private float maxhp;

	public Image hpBar;
	public GameObject emptyhpBar;
	public GameObject slowEffect;
	private GameManager gameManager;
	private EnemySpawner enemySpawner;
	private Item item;
	private List<Vector3> paths = new List<Vector3>();

	private Transform mainTransform;

	private void Start()
	{
		switch (number)
		{
			case 1:
				hp = 5;
				gold = 3;
				speed = 1;
				break;
			case 2:
				hp = 15;
				gold = 10;
				speed = 1.2f;
				break;
			case 3:
				hp = 10;
				gold = 15;
				speed = 1;
				item = FindObjectOfType<Item>();
				break;
			case 4:
				hp = 15;
				gold = 20;
				speed = 0.8f;
				break;
		}
		enemySpawner = FindObjectOfType<EnemySpawner>();
		gameManager = FindObjectOfType<GameManager>();
		mainTransform = transform.parent;

		maxhp = hp;
		
		Targets targets = FindObjectOfType<Targets>();
		int random = UnityEngine.Random.Range(1, 101);
		if (random <= 29 + enemySpawner.stage)
		{
			paths = targets.pathTwo;
		}
		else if (random <= 100)
		{
			paths = targets.pathOne;
		}

		StartCoroutine(SlowEffect());
	}

	private void Update()
	{
		if (gameManager.pause) return;

		mainTransform.position = Vector3.MoveTowards(mainTransform.position, paths[targetNumber], speed * Time.deltaTime);
		transform.LookAt(paths[targetNumber]);

		if (Vector3.Distance(mainTransform.position, paths[targetNumber]) <= 0.1f)
		{
			targetNumber++;
		}

		if (number == 2)
		{
			Boost();
		}

		if (hp != maxhp)
		{
			hpBar.gameObject.SetActive(true);
			emptyhpBar.SetActive(true);

			hpBar.fillAmount = hp / maxhp;
		}

		if (hp <= 0)
		{
			switch (number)
			{
				case 3:
					item.RandomItem();
					break;
				case 4:
					Boom();
					break;
			}
			gameManager.gold += gold * gameManager.goldPower;
			enemySpawner.enemys.Remove(mainTransform);
			Destroy(mainTransform.gameObject);
		}
	}

	private IEnumerator SlowEffect()
	{
		while (true)
		{
			slowEffect.SetActive(slow || itemSlow);
			yield return null;
		}
	}

	private void Boost()
	{
		for (int count = 0; count < enemySpawner.enemys.Count; count++)
		{
			try
			{
				Enemy enemy = enemySpawner.enemys[count].GetChild(0).GetComponent<Enemy>();
				if (Vector3.Distance(mainTransform.position, enemySpawner.enemys[count].position) <= 1.5f)
				{
					if (enemy.boost) continue;
					enemy.speed *= 1.3f;
					enemy.boost = true;
				}
				else
				{
					if (!enemy.boost) continue;
					enemy.speed /= 1.3f;
					enemy.boost = false;
				}
			}
			catch (NullReferenceException) { }
		}
	}

	private void Boom()
	{
		for (int count = 0; count < enemySpawner.enemys.Count; count++)
		{
			try
			{
				if (Vector3.Distance(mainTransform.position, enemySpawner.enemys[count].position) <= 1.5f)
				{
					enemySpawner.enemys[count].GetChild(0).GetComponent<Enemy>().hp -= 3;
				}
			}
			catch (NullReferenceException) { }
		}
	}
}
