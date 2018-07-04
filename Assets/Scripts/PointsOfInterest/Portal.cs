using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : PointOfInterest {
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<PlayerShip> () != null) {
			if (Player.Instance.PlayerShipRewardChests.Count > 0) {
				Player.Instance.NoticeChests ();
			}
			UIOverlay.Instance.OpenAdventureSelectionWindow ();
		}
	}
}
