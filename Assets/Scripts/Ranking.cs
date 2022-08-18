using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ranking : MonoBehaviour
{
	public List<Rank> ranks = new List<Rank>();

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void Sort()
	{
		ranks = ranks.OrderBy(x => x.score).Reverse().ToList();
	}
}

public class Rank
{
	public string name = "�͸�";
	public int score = 0;

	public Rank(int score)
	{
		this.score = score;
	}
}