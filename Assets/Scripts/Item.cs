using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	public Image[] itemGlass = new Image[4];
	public Text[] countText = new Text[4];

	public GameManager gameManager;
	public EnemySpawner enemySpawner;
	public Text buyCountText;
	public Text buyCostText;
	public GameObject goldWarning;
	public GameObject goldBonus;
	public AudioSource audioSource;
	public AudioClip healSound;
	public AudioClip wholeAttackSound;
	public AudioClip goldBonusSound;
	public AudioClip stopSound;

	private float[] itemCount = new float[4];
	private float[] coolDown = new float[4];
	private float[] maxCoolDown = new float[4];

	public int buyCount = 10;
	public int buyCost = 10;

	private bool incresing = false;

	private void Update()
	{
		for (int count = 0; count < coolDown.Length; count++)
		{
			if (coolDown[count] > 0)
			{
				itemGlass[count].gameObject.SetActive(true);
				coolDown[count] -= Time.deltaTime;
				for (int count1 = 0; count1 < coolDown.Length; count1++)
				{
					itemGlass[count1].fillAmount = coolDown[count1] / maxCoolDown[count1];
				}
			}
			else
			{
				itemGlass[count].gameObject.SetActive(false);
			}
		}

		buyCountText.text = string.Format("{0}회 남음", buyCount);
		buyCostText.text = string.Format("{0}원", buyCost);

		for (int count = 0; count < itemCount.Length; count++)
		{
			countText[count].text = string.Format("{0}개", itemCount[count]);
		}
	}

	public void RandomItemBuy()
	{
		if (buyCount <= 0) return;
		if (gameManager.gold < buyCost)
		{
			goldWarning.SetActive(true);
			return;
		}

		bool[] areMax = new bool[4];

		for (int count = 0; count < itemCount.Length; count ++)
		{
			if (itemCount[count] == 3)
			{
				areMax[count] = true;
			}
		}

		int[] chances = new int[4] { 15, 25, 30, 30 };
		int number = 0;
		int random = Random.Range(1, 101);
		
		for (int count = 0; count < itemCount.Length; count++)
		{
			number += chances[count];
			if (areMax[count]) continue;
			if (random <= number)
			{
				itemCount[count]++;
				
				buyCount--;
				gameManager.gold -= buyCost;
				buyCost += 5;

				break;
			}
		}

		//if (random <= 15)
		//{
		//	itemCount[0]++;
		//}
		//else if (random <= 40)
		//{
		//	itemCount[1]++;
		//}
		//else if (random <= 70)
		//{
		//	itemCount[2]++;
		//}
		//else if (random <= 100)
		//{
		//	itemCount[3]++;
		//}

	}

	public void RandomItem()
	{
		bool[] areMax = new bool[4];

		for (int count = 0; count < itemCount.Length; count++)
		{
			if (itemCount[count] == 3)
			{
				areMax[count] = true;
			}
		}

		int[] chances = new int[4] { 15, 25, 30, 30 };
		int number = 0;
		int random = Random.Range(1, 101);

		for (int count = 0; count < itemCount.Length; count++)
		{
			number += chances[count];
			if (areMax[count]) continue;
			if (random <= number)
			{
				itemCount[count]++;
				break;
			}
		}

		//int random = Random.Range(1, 101);
		//if (random <= 15)
		//{
		//	itemCount[0]++;
		//}
		//else if (random <= 40)
		//{
		//	itemCount[1]++;
		//}
		//else if (random <= 70)
		//{
		//	itemCount[2]++;
		//}
		//else if (random <= 100)
		//{
		//	itemCount[3]++;
		//}
	}

	private void SetCoolDown(int number, float length)
	{
		for (int count = 0; count < maxCoolDown.Length; count++)
		{
			maxCoolDown[count] = count != number ? 1f : length;
			coolDown[count] = count != number ? 1f : length;
		}
	}

	public void Heal()
	{
		if (itemCount[0] <= 0) return;
		if (coolDown[0] > 0) return;
		if (gameManager.hp >= 10) return;

		itemCount[0]--;

		gameManager.hp += gameManager.hp < 8 ? 3 : 10 - gameManager.hp;

		SetCoolDown(0, 5);

		audioSource.PlayOneShot(healSound);
	}

	public void WholeDamage()
	{
		if (itemCount[1] <= 0) return;
		if (coolDown[1] > 0) return;

		itemCount[1]--;

		foreach (Transform temporary in enemySpawner.enemys)
		{
			temporary.GetChild(0).GetComponent<Enemy>().hp /= 2;
		}

		SetCoolDown(1, 7);

		audioSource.PlayOneShot(wholeAttackSound);
	}

	public void StartGoldIncrease()
	{
		if (itemCount[2] <= 0) return;
		if (coolDown[2] > 0) return;
		if (incresing) return;

		itemCount[2]--;

		StartCoroutine(GoldIncrease());

		SetCoolDown(2, 6);

		audioSource.PlayOneShot(goldBonusSound);
	}

	public IEnumerator GoldIncrease()
	{
		goldBonus.SetActive(true);
		incresing = true;
		gameManager.goldPower *= 2;
		yield return new WaitForSeconds(180);
		incresing = false;
		gameManager.goldPower /= 2;
		goldBonus.SetActive(false);
	}

	public void StartStop()
	{
		if (itemCount[3] <= 0) return;
		if (coolDown[3] > 0) return;

		itemCount[3]--;

		StartCoroutine(Stop());

		SetCoolDown(3, 12);

		audioSource.PlayOneShot(stopSound);
	}

	public IEnumerator Stop()
	{
		Queue<float> speeds = new Queue<float>();
		foreach (Transform temporary in enemySpawner.enemys)
		{
			Enemy enemy = temporary.GetChild(0).GetComponent<Enemy>();
			speeds.Enqueue(enemy.speed);
			enemy.speed = 0;
			enemy.itemSlow = true;
		}

		yield return new WaitForSeconds(10);

		foreach (Transform temporary in enemySpawner.enemys)
		{
			Enemy enemy = temporary.GetChild(0).GetComponent<Enemy>();
			enemy.speed = speeds.Dequeue();
			enemy.itemSlow = false;
		}
	}
}
