using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
	public GameObject help;
	public GameObject credit;
	public GameObject rank;

	public GameObject Buttons;
	public GameObject text;

	private void Update()
	{
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
		{
			text.SetActive(false);
			Buttons.SetActive(true);
		}
	}

	public void GameStart()
	{
		SceneManager.LoadScene("Trailer");
	}

	public void Help()
	{
		help.SetActive(true);
	}

	public void TurnOffHelp()
	{
		help.SetActive(false);
	}

	public void Credit()
	{
		credit.SetActive(true);
	}

	public void Rank()
	{
		rank.SetActive(true);
	}

	public void TurnOffRank()
	{
		rank.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
