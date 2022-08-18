using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(StartCredit());
	}

	private IEnumerator StartCredit()
	{
		float goalY = 1620;

		while (GetComponent<RectTransform>().position.y < goalY)
		{
			if (Input.anyKeyDown || Input.GetMouseButtonDown(0)) goto SKIP;
			transform.Translate(Vector3.up *50* Time.deltaTime);
			yield return null;
		}

		yield return new WaitForSeconds(1);
	SKIP:
		GetComponent<RectTransform>().position = new Vector3(960, -540, 0);

		gameObject.SetActive(false);
	}
}
