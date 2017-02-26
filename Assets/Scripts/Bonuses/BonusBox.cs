using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBox : MonoBehaviour {

	public List<Bonus> possibleBonuses;

	public static BonusBox SpawnBonusBox()
	{
		return null;
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		ColorBeing being = other.GetComponentInParent<ColorBeing>();
		if (being != null)
		{
			ApplyBonus(being);
			Destroy(this.gameObject);
		}
	}

	void ApplyBonus(ColorBeing being)
	{
		Bonus chosenBonus = possibleBonuses[UnityEngine.Random.Range(0, possibleBonuses.Count)];
		chosenBonus.ApplyToBeing(being);
	}
}
