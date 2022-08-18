using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public int number;
	public int level = 1;

	private int damage = 0;
	private float delay = 0;
	private float range = 0;
	private float slow = 0;
	private int target = 1;

	private float countTime = 0f;

	public GameObject particle;
	private GameObject boomParticle;
	public AudioClip sound;

	private EnemySpawner enemySpawner;
	private AudioSource audioSource;

	private Transform mainTransform;
	private Transform watch = null;

	private void Start()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		audioSource = FindObjectOfType<AudioSource>();
		mainTransform = transform.parent;
		boomParticle = transform.Find("Boom").gameObject;

		switch (number)
		{
			case 1:
				damage = 3;
				delay = 2;
				break;
			case 2:
				damage = 2;
				delay = 2;
				range = 1.5f;
				break;
			case 3:
				damage = 3;
				delay = 1;
				range = 2.5f;
				slow = 0.2f;
				break;
			case 4:
				damage = 2;
				delay = 2;
				target = 3;
				break;
		}
	}

	private void Update()
	{
		if (number <= 2)
		{
			if (watch)
			{
				transform.LookAt(watch);
				if (Vector3.Distance(mainTransform.position, watch.position) > 6f)
				{
					watch = null;
				}
			}
		}

		countTime += Time.deltaTime;

		if (countTime < delay) return;
		
		countTime = 0f;

		FindTarget();
	}

	private void FindTarget()
	{
		Queue<Enemy> targets = new Queue<Enemy>(target);
		foreach (Transform target in enemySpawner.enemys)
		{
			if (Vector3.Distance(mainTransform.position, target.position) <= 6f)
			{
				targets.Enqueue(target.GetChild(0).GetComponent<Enemy>());
			}
		}
		if (targets.Count <= 0) return;

		watch = targets.Peek().transform;

		for(int count = 0; count < target; count++)
		{
			if (range > 0)
			{
				StartCoroutine(BoomAttack(targets.Dequeue()));
			}
			else
			{
				StartCoroutine(Attack(targets.Dequeue()));
			}

			if (targets.Count <= 0) break;
		}
	}

	private IEnumerator Attack(Enemy target)
	{
		// ¿Ã∆Â∆Æ ª˝º∫
		GameObject game = Instantiate(particle, target.transform);
		boomParticle.SetActive(true);
		target.hp -= damage;
		audioSource.PlayOneShot(sound);
		yield return new WaitForSeconds(0.3f);
		// ¿Ã∆Â∆Æ ªË¡¶
		boomParticle.SetActive(false);
		Destroy(game);
	}

	private IEnumerator BoomAttack(Enemy target)
	{
		// ¿Ã∆Â∆Æ ª˝º∫
		GameObject game = Instantiate(particle);
		game.transform.position += target.transform.position;
		boomParticle.SetActive(true);
		List<Enemy> targets = new List<Enemy>();

		foreach(Transform temporary in enemySpawner.enemys)
		{
			if (Vector3.Distance(target.transform.position, temporary.position) <= range)
			{
				targets.Add(temporary.GetChild(0).GetComponent<Enemy>());
			}
		}

		foreach(Enemy temporary in targets)
		{
			if (slow > 0)
			{
				if (!temporary.slow)
				{
					temporary.speed *= 1 - slow;
					temporary.slow = true;
				}
			}

			temporary.hp -= damage;
		}

		audioSource.PlayOneShot(sound);
		yield return new WaitForSeconds(0.3f);

		boomParticle.SetActive(false);
		Destroy(game);
		// ¿Ã∆Â∆Æ ªË¡¶

		if (slow <= 0) yield break;

		yield return new WaitForSeconds(4.7f);

		foreach (Enemy temporary in targets)
		{
			if (temporary.slow)
			{
				temporary.speed *= 1 + slow;
				temporary.slow = false;
			}
		}
	}

	public void LevelUp()
	{
		level++;
		switch (number)
		{
			case 1:
				damage += 2;
				delay -= 0.5f;
				break;
			case 2:
				damage += level == 3 ? 1 : 2;
				delay -= level == 3 ? 1 : 0;
				range += 0.5f;
				break;
			case 3:
				slow += level == 3 ? 0.2f : 0.1f;
				break;
			case 4:
				damage += 1;
				target += level == 3 ? 4 : 3;
				break;
		}
	}

	public MeshFilter GetBodyMeshFilter()
	{
		return transform.GetChild(0).GetComponent<MeshFilter>();
	}
}
