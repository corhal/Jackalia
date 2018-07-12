using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipListElement : MonoBehaviour {

	public Image ShipImage;
	public Image TickImage;
	public GameObject UnlockButton;
	public GameObject ChooseButton;
	public Text UnlockButtonLabel;
	public Slider CardsSlider;
	public Text CardsLabel;
	public ShipData ShipData;

	public void UnlockShip () {
		UIOverlay.Instance.OpenShipWindow (ShipData);
		/*if (Player.Instance.Gold < ShipData.GoldToUnlock) {
			UIOverlay.Instance.OpenPopUp ("Not enough gold!");
			return;
		}
		Player.Instance.GiveGold (ShipData.GoldToUnlock);
		Player.Instance.GiveItems (new Dictionary<string, int> () { { ShipData.Name, ShipData.CardsToUnlock } });
		ShipData.IsUnlocked = true;
		UIOverlay.Instance.ShipCatalogWindow.Close ();
		UIOverlay.Instance.OpenShipCatalogWindow ();*/
	}

	public void ChooseShip () {
		Player.Instance.CurrentShipData = ShipData;
		Player.Instance.Artifacts = ShipData.Artifacts;
		UIOverlay.Instance.ShipCatalogWindow.Close ();
		UIOverlay.Instance.OpenShipCatalogWindow ();
	}

	public void OpenInfo () {
		UIOverlay.Instance.OpenShipWindow (ShipData);
	}
}
