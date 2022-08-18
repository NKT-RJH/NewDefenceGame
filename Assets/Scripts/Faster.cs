using UnityEngine;
using UnityEngine.UI;

public class Faster : MonoBehaviour
{
	private int faster = 1;

	private Text text;

	private void Start()
	{
		text = transform.Find("Text").GetComponent<Text>();
	}

	private void Update()
	{
		text.text = string.Format("X{0}", faster);
	}

	public void Onclick()
	{
		faster += faster < 2 ? 1 : -1;
		Time.timeScale = faster;
	}
}
