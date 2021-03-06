﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour {

	public bool InMoveMode;
	public Selectable Selection;

	public Text GoldLabel;
	public Text EnergyLabel;
	public Text GemsLabel;
	public Text TimeLabel;

	public Text LevelLabel;
	public Text ExpLabel;
	public Slider ExpSlider;

	public Text ChestCountLabel;

	Player player;
	public static UIOverlay Instance;

	public List<GameObject> HideInAdventureObjects;
	public List<GameObject> HideOffAdventureObjects;

	public ContextButtonsOverlay MyButtonsOverlay;
	public PopUp MyPopUp;
	public ImagesPopUp MyImagesPopUp;
	public AdventureSelectionWindow AdventureSelectionWindow;
	public AdventureWindow AdventureWindow;
	public EnergyPopup EnergyPopup;
	public AltFarmWindow AltFarmWindow;
	public ArtifactsBuyPopUp ArtifactsBuyPopUp;
	public ShipCatalogWindow ShipCatalogWindow;
	public RegionPopUp RegionPopUp;
	public ShipWindow ShipWindow;
	public BattleWindow BattleWindow;

	public GameObject MapNode;
	public List<Button> UseArtifactButtons;
	public List<Slider> ArtifactCooldownSliders;

	public GameObject SpecialIndicatorObject;

	void Awake () {
		if (Instance == null) {			
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);  
		}
	}

	public void CheatGold () {
		Player.Instance.TakeGold (1000);
	}

	public void RefreshArtifactsCooldown () {
		for (int i = 0; i < Player.Instance.Artifacts.Count; i++) {
			UseArtifactButtons [i].gameObject.GetComponentInChildren<Text> ().text = "x" + Player.Instance.Inventory [Player.Instance.Artifacts [i].Name];
		}
	}

	public void UseArtifact (int index) {
		Player.Instance.UseArtifact (index);
		RefreshArtifactsCooldown ();
		// UseArtifactButtons [index].enabled = false;
		if (Player.Instance.ActiveArtifact.Name == "Reveal") {
			RevealArtifact ();
		}
	}

	public void RevealArtifact () {
		foreach (var neighborTile in PlayerShip.Instance.CurrentTile.FullNeighbors) {
			neighborTile.StopParticles ();
		}
	}

	public GameObject FlyingRewardPrefab;

	public float JourneyTime;
	float startTime;

	Vector3 startPosition;
	Vector3 targetPosition;
	bool flyReward;
	GameObject flyingReward;

	public void FlyReward (Sprite sprite, Transform initialPosition, GameObject destinationNode) {
		flyingReward = Instantiate (FlyingRewardPrefab) as GameObject;
		flyingReward.transform.position = initialPosition.position;
		flyingReward.GetComponentInChildren<SpriteRenderer> ().sprite = sprite;

		flyingReward.GetComponentInChildren<SpriteRenderer> ().transform.localScale *= 0.15f; // kostyll

		startTime = Time.time;
		startPosition = flyingReward.transform.position;
		targetPosition = destinationNode.transform.position;
		flyReward = true;
	}

	void Start () {
		player = Player.Instance;
		if (player.OnAdventure) {
			foreach (var hideObject in HideInAdventureObjects) {
				hideObject.SetActive (false);
			}
			foreach (var hideObject in HideOffAdventureObjects) {
				hideObject.SetActive (true);
			}
			if (Player.Instance.CurrentAdventure.TreasureHunt) {
				MapNode.SetActive (true);
			} else {
				MapNode.SetActive (false);
			}	
		} else {
			MapNode.SetActive (false);
			foreach (var hideObject in HideInAdventureObjects) {
				hideObject.SetActive (true);
			}
			foreach (var hideObject in HideOffAdventureObjects) {
				hideObject.SetActive (false);
			}
		}
		for (int i = 0; i < Player.Instance.Artifacts.Count; i++) {		
			UseArtifactButtons [i].GetComponent<Image> ().sprite = Player.Instance.Artifacts [i].Icon;
			UseArtifactButtons [i].gameObject.GetComponentInChildren<Text> ().text = "" + Player.Instance.Inventory [Player.Instance.Artifacts [i].Name];
		}
		RefreshArtifactsCooldown ();
		SpecialIndicatorObject.GetComponentInChildren<Slider> ().maxValue = Player.Instance.CurrentShipData.SpecialCooldown;
		SpecialIndicatorObject.GetComponentsInChildren<Image> () [1].sprite = Player.Instance.CurrentShipData.SpecialSprite; // есть фаталити, а есть костылити. это - костылити.
	}

	void Update () { // OMG
		GoldLabel.text = "" + player.Gold;
		GemsLabel.text = "" + player.Inventory ["Gems"];
		EnergyLabel.text = "" + player.Energy + "/" + player.MaxEnergy;
		ExpLabel.text = "" + player.Inventory ["Exp"] + "/" + player.ExpForLevel [player.Level];
		LevelLabel.text = "" + player.Level;
		ExpSlider.maxValue = player.ExpForLevel [player.Level];
		ExpSlider.value = player.Inventory ["Exp"];
		ChestCountLabel.text = Player.Instance.ChestsFound + "/" + Player.Instance.CurrentAdventure.InitialChestsCount;
		if (player.OnAdventure) {			
			int time = (int)player.AdventureTimer;
			int hours = time / 3600;
			int minutes = (time - hours * 3600) / 60;
			int seconds = time - hours * 3600 - minutes * 60;
			TimeLabel.text = hours + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
			if (player.CurrentAdventure.TreasureHunt) {
				MapNode.GetComponentInChildren<Text> ().text = player.Inventory ["Map"] + "/" + player.CurrentAdventure.MapsForTreasure;
			}

			SpecialIndicatorObject.GetComponentInChildren<Slider> ().value = Player.Instance.CurrentShipData.SpecialCurrentCooldown;
			SpecialIndicatorObject.GetComponentInChildren<Text> ().text = Player.Instance.CurrentShipData.SpecialCurrentCooldown + "";
			if (Player.Instance.CurrentShipData.SpecialCurrentCooldown <= 0) {
				SpecialIndicatorObject.GetComponentInChildren<Text> ().text = "";
			}
		}

		if (flyReward) {
			float dTime = (Time.time - startTime) / JourneyTime;			
			flyingReward.transform.position = Vector3.Lerp(startPosition, targetPosition, dTime);
			if (Vector3.Distance (flyingReward.transform.position, targetPosition) < 0.01f) {
				flyReward = false;
				Destroy (flyingReward);				
			}
		}
	}

	public GameObject PreviousAdventureButtonObject;
	public GameObject NextAdventureButtonObject;
	public Image AdventureImage;
	public List<Sprite> AdventureSprites;

	public void ChangeAdventure (bool next) {
		Player.Instance.ChangeAdventure (next);
		if (Player.Instance.Adventures.IndexOf(Player.Instance.CurrentAdventure) == 0) {
			PreviousAdventureButtonObject.SetActive (false);
			if (Player.Instance.Adventures.Count > 1) {
				NextAdventureButtonObject.SetActive (true);
			}
		} else if (Player.Instance.Adventures.IndexOf(Player.Instance.CurrentAdventure) == Player.Instance.Adventures.Count - 1) {
			NextAdventureButtonObject.SetActive (false);
			if (Player.Instance.Adventures.Count > 1) {
				PreviousAdventureButtonObject.SetActive (true);
			}
		} else {
			if (Player.Instance.Adventures.Count > 1) {
				NextAdventureButtonObject.SetActive (true);
				PreviousAdventureButtonObject.SetActive (true);
			}
		}
		AdventureImage.sprite = AdventureSprites [Player.Instance.Adventures.IndexOf (Player.Instance.CurrentAdventure)];
	}

	public void OpenAdventureSelectionWindow () {
		AdventureSelectionWindow.Open ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenBattleWindow (BattleShip playerChar, BattleShip enemyChar) {
		BattleWindow.Open (playerChar, enemyChar);
	}

	public void OpenAltFarmWindow (AltFarm altFarm) {
		AltFarmWindow.Open (altFarm);
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void CloseAdventureSelectionWindow () {
		if (AdventureSelectionWindow != null) {
			AdventureSelectionWindow.Close ();
		}
	}

	public void OpenAdventureWindow (string regionName) {
		AdventureWindow.Open (regionName);
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenShipCatalogWindow () {
		ShipCatalogWindow.Open ();
		AdventureWindow.Close ();
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenShipWindow (ShipData ship) {
		ShipWindow.Open (ship);
		AdventureWindow.Close ();
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenRegionPopUp (Region region) {
		RegionPopUp.Open (region);
		ShipCatalogWindow.Close ();
		AdventureWindow.Close ();
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenEnergyPopup () {
		EnergyPopup.Open ();
		AdventureWindow.Close ();
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenArtifactsBuyPopUp (int index) {
		ArtifactsBuyPopUp.Open (index);
		EnergyPopup.Close ();
		AdventureWindow.Close ();
		AdventureSelectionWindow.Close ();
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
	}

	public void OpenSelectableInfo (Selectable selectable) {
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
		CloseAdventureSelectionWindow ();
	}

	public void OpenShipWindow (CreatureData shipData) {
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
		CloseAdventureSelectionWindow ();
	}

	public void OpenPopUp (string message) {
		MyPopUp.Open (message);
	}

	public void OpenImagesPopUp (string message, Dictionary<string, int> itemNames) {
		MyImagesPopUp.Open (message, itemNames);
	}

	public void OpenContextButtons (Selectable selectable) {
		if (InMoveMode) {
			return;
		}
		CloseContextButtons (true);
		Selection = selectable;
		Selection.Animate ();
		MyButtonsOverlay.Open (selectable);
		MyPopUp.Close ();
		CloseAdventureSelectionWindow ();
	}

	public void CloseContextButtons (bool deselect) {
		MyButtonsOverlay.Overlay.SetActive (false); // kostyll
		if (deselect && Selection != null) {
			Selection.Deanimate ();
		}
	}

	public void CloseAllWindows () {		
		MyButtonsOverlay.Close ();
		MyPopUp.Close ();
		CloseAdventureSelectionWindow ();
	}
}
