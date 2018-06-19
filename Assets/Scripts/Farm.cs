using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : Selectable {

	public Slider FoodSlider;
	public GameObject ReadyMarkerObject;

	public int MaxFood { get { return Player.Instance.MaxFoodInFarm; } set { Player.Instance.MaxFoodInFarm = value; } }
	public int Food { get { return Player.Instance.FoodInFarm; } set { Player.Instance.FoodInFarm = value; } }
	public float SecondsPerFood;
	float t = 0.0f;

	protected override void Awake () {
		base.Awake ();
		Action collectAction = new Action("Collect", 0, player.DataBase.ActionIconsByNames["Info"], CollectFood);
		actions.Add (collectAction);
		FoodSlider.maxValue = MaxFood;
	}

	protected override void Start () {
		base.Start ();
		float time = Player.Instance.GlobalTimer - Player.Instance.AdventureStartedTime;
		Food += (int)(time / (float)SecondsPerFood);
		if (Food >= MaxFood / 3 && !ReadyMarkerObject.activeSelf) {
			ReadyMarkerObject.SetActive (true);
		}
	}

	public void CollectFood () {
		int initialEnergy = player.Energy;
		player.Energy = Mathf.Min (player.Energy + Food, player.MaxEnergy);
		Food -= player.Energy - initialEnergy;
		if (Food < MaxFood / 3) {
			ReadyMarkerObject.SetActive (false);
		}
	}

	protected override void Update () {
		base.Update ();
		if (Food < MaxFood) {
			t += Time.deltaTime;
			if (t >= SecondsPerFood) {
				t = 0.0f;
				Food += 1;
			}
			if (Food >= MaxFood / 3 && !ReadyMarkerObject.activeSelf) {
				ReadyMarkerObject.SetActive (true);
			}
		}
		FoodSlider.value = Food;
	}
}
