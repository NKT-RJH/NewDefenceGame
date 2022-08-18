using System;
using UnityEngine;

public class TurretGround : MonoBehaviour
{
	public TurretSetting[] turretSettings = new TurretSetting[4];

	public Transform turretGround;
	public GameManager gameManager;
	public GameObject goldWarning;

	private Turret turret = null;

	private int[,] turretCosts = new int[4, 3]
	{
		{ 10,20,30 },
		{ 20,25,30 },
		{ 20,50,70 },
		{ 20,100,200 }
	};

	public void Onclick()
	{
		if (turret)
		{
			transform.Find("Option").gameObject.SetActive(true);
			transform.Find("Install").gameObject.SetActive(false);
		}
		else
		{
			transform.Find("Install").gameObject.SetActive(true);
			transform.Find("Option").gameObject.SetActive(false);
		}
		transform.Find("Exit").gameObject.SetActive(true);
	}

	public void Exit()
	{
		transform.Find("Option").gameObject.SetActive(false);
		transform.Find("Install").gameObject.SetActive(false);
		transform.Find("Exit").gameObject.SetActive(false);
	}

	public void Install(int number)
	{
		if (gameManager.gold < turretCosts[number, 0])
		{
			goldWarning.SetActive(true);
			return;
		}

		gameManager.gold -= turretCosts[number, 0];
		turret = Instantiate(turretSettings[number].turret, turretGround).GetComponent<Turret>();
		Exit();
	}

	public void Upgrade()
	{
		if (gameManager.gold < turretCosts[turret.number - 1, turret.level])
		{
			goldWarning.SetActive(true);
			return;
		}

		turret.level++;
		gameManager.gold -= turretCosts[turret.number - 1, turret.level - 1];
		turret.GetBodyMeshFilter().mesh = turretSettings[turret.number - 1].looks[turret.level - 1];

		if (turret.level == 3)
		{
			transform.Find("Option").Find("Upgrade").gameObject.SetActive(false);
		}
		Exit();
	}

	public void DestroyTurret()
	{
		gameManager.gold += turretCosts[turret.number - 1, turret.level - 1];
		Destroy(turret.gameObject);
		turret = null;
		transform.Find("Option").Find("Upgrade").gameObject.SetActive(true);
		Exit();
	}
}

[Serializable]
public class TurretSetting
{
	public GameObject turret;

	public Mesh[] looks = new Mesh[3];
}