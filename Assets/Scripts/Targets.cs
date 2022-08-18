using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{
	public List<Vector3> pathOne = new List<Vector3>();
	public List<Vector3> pathTwo = new List<Vector3>();

	private void Awake()
	{
		Transform one = transform.Find("Turn1").transform;

		for (int count = 0; count < one.childCount; count++)
		{
			pathOne.Add(one.GetChild(count).position);
		}

		Transform two = transform.Find("Turn2").transform;

		for (int count = 0; count < two.childCount; count++)
		{
			pathTwo.Add(two.GetChild(count).position);
		}
	}
}
