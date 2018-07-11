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

	public Button UpgradeButton;

	public void Open (ShipData ship) {
		Window.SetActive (true);

		Header.text = ship.Name;

		SpecialImage.sprite = ship.SpecialSprite;
		InfoLabel.text = ship.SpecialInfo;

		ShipImage.sprite = ship.Sprite;
		CardsSlider.maxValue = ship.CardsToUnlock;
		CardsSlider.value = Player.Instance.Inventory [ship.Name];
		CardsLabel.text = Player.Instance.Inventory [ship.Name] + "/" + ship.CardsToUnlock;
	}

	public void Close () {
		Window.SetActive (false);
	}
}
