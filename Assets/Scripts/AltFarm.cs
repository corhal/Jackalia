using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltFarm : Selectable {

	public GameObject ReadyMarkerObject;

	bool isGrowing;
	public int CurrentIndex;

	public List<int> FoodsToGrow;
	public List<int> FoodCosts;
	public List<int> FoodTimes;

	public Text TimeLabel;
	public Slider TimeSlider;

	float t = 0.0f;

	protected override void Awake () {
		base.Awake ();
		Action openWindowAction = new Action("Open window", 0, player.DataBase.ActionIconsByNames["Info"], OpenWindow);
		actions.Add (openWindowAction);
	}

	protected override void Start () {
		base.Start ();
		float time = Player.Instance.GlobalTimer - Player.Instance.AdventureStartedTime;

		if (Player.Instance.AltFarmIsGrowing) {
			t += time;
			isGrowing = true;
		}

	}

	public void OpenWindow () {
		uiManager.OpenAltFarmWindow ();
	}

	public void CollectFood () {
		int initialEnergy = player.Energy;
		player.Energy += FoodsToGrow [CurrentIndex]; // = Mathf.Min (player.Energy + FoodsToGrow [CurrentIndex], player.MaxEnergy);
		ReadyMarkerObject.SetActive (false);
		TimeSlider.gameObject.SetActive (false);
		TimeLabel.gameObject.SetActive (false);
	}

	public void StartGrowing (int index) {
		if (player.Gold >= FoodCosts [index]) {
			player.GiveGold (FoodCosts [index]);
			CurrentIndex = index;
			isGrowing = true;
			TimeSlider.gameObject.SetActive (true);
			TimeLabel.gameObject.SetActive (true);
			TimeSlider.maxValue = FoodTimes [CurrentIndex];
			Player.Instance.AltFarmIsGrowing = true;
		} else {
			uiManager.OpenPopUp ("Not enough gold!");
		}
	}

	protected override void Update () {
		base.Update ();
		if (isGrowing) {
			t += Time.deltaTime;
			TimeSlider.value = t;
			TimeLabel.text = Utility.SecondsToTime (FoodTimes [CurrentIndex] - (int)t);
			if (t >= FoodTimes [CurrentIndex]) {
				t = 0.0f;
				ReadyMarkerObject.SetActive (true);
				isGrowing = false;
				Player.Instance.AltFarmIsGrowing = false;
			}
		}
	}
}
