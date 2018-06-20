using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPopup : MonoBehaviour {

	public GameObject Window;
	public Text MessageText;
	public Text BuyButtonLabel;

	public int EnergyToBuy;
	public int GemsCost;

	public void Open () {
		Window.SetActive (true);
		MessageText.text = "You ran out of energy!\nBuy " + EnergyToBuy + " more?";
		BuyButtonLabel.text = "Yes " + GemsCost + "   ";
	}

	public void Close () {
		Window.SetActive (false);
	}

	public void RefuseEnergy () {
		Player.Instance.LoadVillage ();
	}

	public void BuyEnergy () {
		if (Player.Instance.Gems >= GemsCost) {
			Close ();
			Player.Instance.Energy += EnergyToBuy;
		}
		Player.Instance.GiveGems (GemsCost);
	}
}
