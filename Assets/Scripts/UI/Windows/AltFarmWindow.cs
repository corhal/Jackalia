using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltFarmWindow : MonoBehaviour {

	public GameObject Window;
	public AltFarm AltFarm;

	public List<Text> GrowFoodAmountLabels;
	public List<Text> GrowFoodCostLabels;
	public List<Text> GrowFoodTimeLabels;

	public void Open () {
		Window.SetActive (true);

		for (int i = 0; i < AltFarm.FoodsToGrow.Count; i++) {
			GrowFoodAmountLabels [i].text = AltFarm.FoodsToGrow [i] + "";
			GrowFoodCostLabels [i].text = AltFarm.FoodCosts [i] + "";
			GrowFoodTimeLabels [i].text = Utility.SecondsToTime (AltFarm.FoodTimes [i]) + "";
		}
	}

	public void ClickFarm (int index) {
		AltFarm.StartGrowing (index);
		Close ();
	}

	public void Close () {
		Window.SetActive (false);
	}
}
