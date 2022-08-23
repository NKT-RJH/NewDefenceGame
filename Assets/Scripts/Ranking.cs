using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ranking : MonoBehaviour
{
	public List<Rank> ranks = new List<Rank>();

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void FirstLoad()
	{
		Application.targetFrameRate = 60;

		string save = PlayerPrefs.GetString("SaveFile");

		string[] rankings = save.Split('/');
		
		if (rankings.Length <= 1) return;

		Ranking me = FindObjectOfType<Ranking>();

		foreach (string temporary in rankings)
		{
			string[] splited = temporary.Split(',');
			if (splited.Length <= 1) continue;
			me.ranks.Add(new Rank(splited[0], int.Parse(splited[1])));
		}
	}

	private void Awake()
	{
		var obj = FindObjectsOfType<Ranking>();

		if (obj.Length == 1)
		{
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Sort_And_Save()
	{
		ranks = ranks.OrderBy(x => x.score).Reverse().ToList();

		string save = string.Empty;
		foreach (Rank rank in ranks)
		{
			save = string.Format("{0}/{1}", save, rank.ToString());
		}
		PlayerPrefs.SetString("SaveFile", save);
		print(save);
	}
}

public class Rank
{
	public string name = "¿Õ∏Ì";
	public int score = 0;
	
	public Rank(int score)
	{
		this.score = score;
	}

	public Rank(string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	public override string ToString()
	{
		return string.Format("{0},{1}", name, score);
	}
}