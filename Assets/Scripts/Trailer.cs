using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trailer : MonoBehaviour
{
	public Image glass;

	public List<Text> texts = new List<Text>();

	private void Start()
	{
		StartCoroutine(RunTrailer());
	}

	public void Skip()
	{
		SceneManager.LoadScene("Fight");
	}

	private IEnumerator RunTrailer()
	{
		for (int count = 255; count >= 0; count--)
		{
			glass.color = new Color(glass.color.r, glass.color.g, glass.color.b, count / 255f);
			yield return new WaitForSeconds(0.01f);
		}
		
		for (int count = 0; count < texts.Count; count++)
		{
			for (int count1 = 0; count1 <= 255; count1++)
			{
				texts[count].color = new Color(texts[count].color.r, texts[count].color.g, texts[count].color.b, count1 / 255f);
				yield return new WaitForSeconds(0.01f);
			}
			yield return new WaitForSeconds(2);
			for (int count1 = 255; count1 >= 0; count1--)
			{
				texts[count].color = new Color(texts[count].color.r, texts[count].color.g, texts[count].color.b, count1 / 255f);
				yield return new WaitForSeconds(0.01f);
			}
			yield return new WaitForSeconds(0.5f);
		}

		yield return new WaitForSeconds(1);

		SceneManager.LoadScene("Fight");
	}
}
