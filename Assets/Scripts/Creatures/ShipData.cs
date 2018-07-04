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
}
