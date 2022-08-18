using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End : MonoBehaviour
{
	public Text scoreText;
	public Text nameText;

	private void Start()
	{
		Ranking ranking = FindObjectOfType<Ranking>();
		scoreText.text = string.Format("{0}Á¡", ranking.ranks[ranking.ranks.Count - 1].score);
	}

	public void SaveName()
	{
		if (string.IsNullOrEmpty(nameText.text)) return;

		Ranking ranking = FindObjectOfType<Ranking>();

		ranking.ranks[ranking.ranks.Count - 1].name = nameText.text;

		SceneManager.LoadScene("Title");
	}
}