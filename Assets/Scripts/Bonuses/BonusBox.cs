using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBox : ColorObject {

	public const float MAX_COLOR_DIF = 0.2f;

	public List<Bonus> possibleBonuses;

	public static BonusBox CreateBonusBox(BonusBox prefab, Vector3 position, Color color)
	{
		BonusBox createdBox = Instantiate(prefab, position, Quaternion.identity);
		createdBox.color = color;
		return createdBox;
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		OnTrigger(other);
	}

	void OnTriggerStay2D (Collider2D other) 
	{
		OnTrigger(other);
	}

	void OnTrigger (Collider2D other) 
	{
		ColorBeing being = other.GetComponentInParent<ColorBeing>();
		if (being != null && ColorUtils.GetHueDif(being.color, this.color) < MAX_COLOR_DIF )
		{
			ApplyBonus(being);
			Destroy(this.gameObject);
		}
	}

	void ApplyBonus(ColorBeing being)
	{
		if (being.isLocalPlayer || isServer && !(being is ControllableColorBeing))
		{
			Bonus chosenBonus = possibleBonuses[UnityEngine.Random.Range(0, possibleBonuses.Count)];
			chosenBonus.ApplyToBeing(being);
		}
	}
}
