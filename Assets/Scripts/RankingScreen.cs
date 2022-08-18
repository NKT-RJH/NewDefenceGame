using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScreen : MonoBehaviour
{
	public GameObject rankBar;
	public Transform content;

	private void Start()
	{
		Ranking ranking = FindObjectOfType<Ranking>();

		foreach (Rank rank in ranking.ranks)
		{
			Transform temporary = Instantiate(rankBar, content).transform;

			temporary.Find("Number").GetComponent<Text>().text = string.Format("{0}µî", ranking.ranks.IndexOf(rank) + 1);
			temporary.Find("Name").GetComponent<Text>().text = rank.name;
			temporary.Find("Score").GetComponent<Text>().text = string.Format("{0}Á¡", rank.score);
		}
	}
}
