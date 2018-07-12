using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : PointOfInterest {

	public int AdditionalRequiredEnergy;

	void Start () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<PlayerShip>() != null) {			
			Interact ();
		}
	}

	public override void Interact () {
		if (!(POIData.OneTime && POIData.Interacted)) {
			base.Interact ();

			if (Player.Instance.CurrentShipData.Special == "Rewind" && Player.Instance.CurrentShipData.IsSpecialReady) {
				Player.Instance.CurrentShipData.UseSpecial ();
				PlayerShip.Instance.MoveToTile (Tile, false, false);
				// PlayerShip.Instance.FallBack (false);
				PlayerShip.Instance.ShowFlyingText ("Maneuver!", Color.green);
				return;
			}

			PlayerShip.Instance.ShowFlyingText (("-" + AdditionalRequiredEnergy), Color.red);
			Player.Instance.Energy = Player.Instance.Energy - AdditionalRequiredEnergy;

			if (Player.Instance.CurrentShipData.Special == "Crush" && Player.Instance.CurrentShipData.IsSpecialReady) {
				Player.Instance.CurrentShipData.UseSpecial ();
				Destroy (gameObject);
				//PlayerShip.Instance.MoveToTile (Tile, false, false);
				// PlayerShip.Instance.FallBack (false);
				PlayerShip.Instance.ShowFlyingText ("Crush!", Color.green);
				//return;
			}
		}
	}
}
