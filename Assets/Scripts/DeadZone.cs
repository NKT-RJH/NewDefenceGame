using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
	public GameManager gameManager;
	public EnemySpawner enemySpawner;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemySpawner.enemys.Remove(other.transform);
			Destroy(other.gameObject);
			gameManager.hp--;
		}
	}
}