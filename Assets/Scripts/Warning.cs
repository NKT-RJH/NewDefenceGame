using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip sound;

	private void OnEnable()
	{
		audioSource.PlayOneShot(sound);
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);

		gameObject.SetActive(false);
	}
}
