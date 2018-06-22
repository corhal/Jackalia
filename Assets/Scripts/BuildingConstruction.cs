using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingConstruction : Selectable {

	public GameObject BuildingResultPrefab;
	public GameObject ReadyMarkerObject;

	bool isBeingBuilt;

	public int BuildCost;
	public int BuildTime;

	public Text TimeLabel;
	public Slider TimeSlider;

	float t = 0.0f;

	protected override void Awake () {
		base.Awake ();
		Action buildAction = new Action("Build", BuildCost, player.DataBase.ActionIconsByNames["Info"], Build);
		actions.Add (buildAction);
	}

	protected override void Start () {
		base.Start ();
		float time = Player.Instance.GlobalTimer - Player.Instance.AdventureStartedTime;

		if (Player.Instance.BuildingIsBeingBuilt) {
			t += time;
			isBeingBuilt = true;
			TimeSlider.gameObject.SetActive (true);
			TimeLabel.gameObject.SetActive (true);
			TimeSlider.maxValue = BuildTime;
		}

	}

	public void Build () {
		if (player.Gold >= BuildCost) {
			player.GiveGold (BuildCost);
			isBeingBuilt = true;
			TimeSlider.gameObject.SetActive (true);
			TimeLabel.gameObject.SetActive (true);
			TimeSlider.maxValue = BuildTime;
			Player.Instance.BuildingIsBeingBuilt = true;
		} else {
			uiManager.OpenPopUp ("Not enough gold!");
		}
	}

	public void FinishBuilding () {
		GameObject resultBuilding = Instantiate (BuildingResultPrefab) as GameObject;
		resultBuilding.transform.position = transform.position;
		Player.Instance.BuildingsToSavePrefabs.Add (BuildingResultPrefab);
		Player.Instance.CoordsToSave.Add (transform.position);
		Player.Instance.BuildingNamesToDestroy.Add (Name);
		Destroy (this.gameObject);
	}

	protected override void Update () {
		base.Update ();
		if (isBeingBuilt) {
			t += Time.deltaTime;
			TimeSlider.value = t;
			TimeLabel.text = Utility.SecondsToTime (BuildTime - (int)t);
			if (t >= BuildTime) {
				t = 0.0f;
				ReadyMarkerObject.SetActive (true);
				isBeingBuilt = false;
				Player.Instance.BuildingIsBeingBuilt = false;
			}
		}
	}
}
