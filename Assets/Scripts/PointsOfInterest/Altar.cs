using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : PointOfInterest {

	public int BonusEnergy;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<PlayerShip>() != null) {
			Interact ();
		}
	}

	public override void Interact () {
		if (!(POIData.OneTime && POIData.Interacted)) {
			base.Interact ();

			Player.Instance.Energy += BonusEnergy;
			UIOverlay.Instance.OpenPopUp ("This altar gives you " + BonusEnergy + " energy!");
		}
	}
}
