using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipWindow : MonoBehaviour {

	public GameObject Window;

	public Text Header;

	public Image RarityImage;
	public Text RarityLabel;

	public Image SpecialImage;
	public Text InfoLabel;

	public Image ShipImage;
	public Slider CardsSlider;
	public Text CardsLabel;

	public GameObject StatsContainer;
	public GameObject StatsElementPrefab;

	public List<GameObject> StatElements;

	public Button UpgradeButton;

	public ShipData ShipData;

	public void Open (ShipData ship) {
		Window.SetActive (true);

		ShipData = ship;
		Header.text = ship.Name;

		SpecialImage.sprite = ship.SpecialSprite;
		InfoLabel.text = ship.SpecialInfo;

		ShipImage.sprite = ship.Sprite;
		CardsSlider.maxValue = ship.CardsToUnlock;
		CardsSlider.value = Player.Instance.Inventory [ship.Name];
		CardsLabel.text = Player.Instance.Inventory [ship.Name] + "/" + ship.CardsToUnlock;

		UpgradeButton.GetComponentInChildren<Text> ().text = "Unlock\n" + ship.GoldToUnlock;

		foreach (var statElementObject in StatElements) {
			Destroy (statElementObject);
		}
		StatElements.Clear ();

		foreach (var statName in ship.StatNames) {
			GameObject statElementObject = Instantiate (StatsElementPrefab) as GameObject;
			statElementObject.GetComponentInChildren<Text> ().text = statName + "\n" + ship.GetStatByString (statName);
			statElementObject.transform.SetParent (StatsContainer.transform);
			statElementObject.transform.localScale = Vector3.one;
			StatElements.Add (statElementObject);
		}
	}

	public void UpgradeShip () {
		if (Player.Instance.Gold < ShipData.GoldToUnlock) {
			UIOverlay.Instance.OpenPopUp ("Not enough gold!");
			return;
		}
		Player.Instance.GiveGold (ShipData.GoldToUnlock);
		Player.Instance.GiveItems (new Dictionary<string, int> () { { ShipData.Name, ShipData.CardsToUnlock } });
		ShipData.IsUnlocked = true;
		Close ();
		UIOverlay.Instance.ShipCatalogWindow.Close ();
		UIOverlay.Instance.OpenShipCatalogWindow ();
	}

	public void Close () {
		Window.SetActive (false);
	}
}
