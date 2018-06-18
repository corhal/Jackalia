using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureWindow : MonoBehaviour {

	public GameObject Window;
	public int FoodCost;

	public Text FoodCostLabel;

	public void Open () {		
		Window.SetActive (true);
		CountTiles ();
	}

	public void Close () {
		Window.SetActive (false);
	}

	public void CountTiles () {
		Adventure adventure = Player.Instance.CurrentAdventure;
		int tilesAmount = (adventure.PosWidth - adventure.NegWidth + 1) * (adventure.PosHeight - adventure.NegHeight + 1);
		FoodCost = tilesAmount;
		FoodCostLabel.text = "Start " + FoodCost;
	}

	public void StartAdventure () {
		if (FoodCost <= Player.Instance.Energy) {
			Player.Instance.Energy -= FoodCost;
			Close ();
			Player.Instance.NewBoard = true;
			Player.Instance.LoadAdventure ();
		} else {
			Debug.Log ("not enough food");
		}
	}
}
