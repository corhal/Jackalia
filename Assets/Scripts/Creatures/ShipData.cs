using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipData {
	public string Name;
	public Sprite Sprite;

	public int CardsToUnlock;
	public int GoldToUnlock;

	public bool IsUnlocked;

	public string Special;
	public int SpecialCooldown;
	public int SpecialCurrentCooldown;
	public Sprite SpecialSprite;

	public bool IsSpecialReady { get { return SpecialCurrentCooldown <= 0; } }

	public void TickSpecialCooldown () {
		if (SpecialCurrentCooldown > 0) {
			SpecialCurrentCooldown--;
		}
	}

	public void UseSpecial () {
		SpecialCurrentCooldown = SpecialCooldown;
	}
}
