using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipCatalogWindow : MonoBehaviour {

	public GameObject Window;

	public GameObject ShipElementsContainer;
	public List<ShipListElement> ShipListElements;

	public GameObject ShipListElementPrefab;


	public void Open () {
		Window.SetActive (true);
		foreach (var shipListElement in ShipListElements) {
			Destroy (shipListElement.gameObject);
		}
		ShipListElements.Clear ();

		for (int i = 0; i < Player.Instance.ShipDatas.Count; i++) {
			GameObject shipListElementObject = Instantiate (ShipListElementPrefab) as GameObject;
			ShipListElement shipListElement = shipListElementObject.GetComponent<ShipListElement> ();
			shipListElement.ShipImage.sprite = Player.Instance.ShipDatas [i].Sprite;
			shipListElement.CardsLabel.text = Player.Instance.Inventory[Player.Instance.ShipDatas [i].Name] + "/" + Player.Instance.ShipDatas [i].CardsToUnlock;
			shipListElement.CardsSlider.maxValue = Player.Instance.ShipDatas [i].CardsToUnlock;
			shipListElement.CardsSlider.value = Player.Instance.Inventory [Player.Instance.ShipDatas [i].Name];
			shipListElement.UnlockButtonLabel.text = "Unlock       " + Player.Instance.ShipDatas [i].GoldToUnlock;
			shipListElement.ShipData = Player.Instance.ShipDatas [i];
			if (Player.Instance.Inventory[Player.Instance.ShipDatas [i].Name] >= Player.Instance.ShipDatas [i].CardsToUnlock) {
				shipListElement.CardsSlider.gameObject.SetActive (false);
				shipListElement.UnlockButton.gameObject.SetActive (true);
			}
			if (Player.Instance.ShipDatas [i].IsUnlocked) {
				shipListElement.CardsSlider.gameObject.SetActive (false);
				shipListElement.UnlockButton.gameObject.SetActive (false);
				shipListElement.ChooseButton.gameObject.SetActive (true);
			}
			if (Player.Instance.CurrentShipData == Player.Instance.ShipDatas [i]) {
				shipListElement.ChooseButton.gameObject.SetActive (false);
				shipListElement.TickImage.gameObject.SetActive (true);
			}

			shipListElementObject.transform.SetParent (ShipElementsContainer.transform);
			shipListElementObject.transform.localScale = Vector3.one;
			ShipListElements.Add (shipListElement);
		}
	}

	public void Close () {
		Window.SetActive (false);
	}
}
