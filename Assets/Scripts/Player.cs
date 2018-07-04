using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public List<Adventure> Adventures;

	public Adventure CurrentAdventure;

	public int MaxEnergy;
	public int Energy;

	public int MaxEnergySave;
	public int EnergySave;

	public int FoodInFarm; // top 10 dirty haxx
	public int MaxFoodInFarm;

	public int ChestsFound;

	public List<Mission> Missions;
	public Mission CurrentMission;
	// public List<CreatureData> ShipDatas;
	public bool FirstLoad = true;
	public static Player Instance;

	public DataBase DataBase;
	public BJDataBase BJDataBase;
	public Dictionary<string, int> Inventory;

	public int Gems { get { return Inventory ["Gems"]; } }
	public int Gold { get { return Inventory ["Gold"]; } }
	public int Exp { get { return Inventory ["Exp"]; } }
	public int Level = 1;
	public List<int> ExpForLevel = new List<int> {0, 100, 500, 1000, 2000};

	public List<CreatureData> CurrentTeam;

	public Vector3 PlayerShipCoordinates;

	public Dictionary<string, bool> Tiles;
	public Dictionary<string, POIData> POIDataByTiles;
	public List<POIData> POIDatas;

	public bool NewBoard;
	public bool OnAdventure;
	public float AdventureTimer;

	public List<RewardChest> PlayerShipRewardChests;
	public List<RewardChest> RewardChests;
	public RewardChest CurrentlyOpeningChest;

	public bool ReceivedReward;

	public float AdventureStartedTime;
	public float GlobalTimer;

	public List<GameObject> BuildingsToSavePrefabs;
	public List<string> BuildingNamesToDestroy;
	public List<Vector3> CoordsToSave;

	public int FarmIndex;
	public List<int> AltFarmGrowingIndexes;
	public List<float> AltFarmStartedGrowingTimes;
	public List<bool> AltFarmsAreGrowing;

	public bool BuildingIsBeingBuilt;

	public List<Artifact> Artifacts;
	public Artifact ActiveArtifact;

	public ShipData CurrentShipData;
	public List<ShipData> ShipDatas;

	void Awake () {
		if (Instance == null) {			
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);  
		}
		DontDestroyOnLoad(gameObject);
		// CurrentTeam = new List<CreatureData> ();
		Inventory = new Dictionary<string, int> ();
		Tiles = new Dictionary<string, bool> ();
		POIDataByTiles = new Dictionary<string, POIData> ();
		POIDatas = new List<POIData> ();
		RewardChests = new List<RewardChest> ();
		PlayerShipRewardChests = new List<RewardChest> ();
	}

	void Start () {
		CurrentAdventure = Adventures [0];
		foreach (var shipData in ShipDatas) {
			Inventory.Add (shipData.Name, 0);
			DataBase.ItemIconsByNames.Add (shipData.Name, shipData.Sprite);
		}
		CurrentShipData = ShipDatas [0];
	}

	public void UseArtifact (int index) {
		if (Inventory [Artifacts [index].Name] > 0) {
			Inventory [Artifacts [index].Name]--;
			ActiveArtifact = Artifacts [index];
			ActiveArtifact.Use ();
		} else {
			UIOverlay.Instance.OpenArtifactsBuyPopUp (index);
		}
	}

	float timer = 0.0f;
	int previousTimer = 0;

	void Update () {
		GlobalTimer += Time.deltaTime;
		if (OnAdventure) {
			AdventureTimer -= Time.deltaTime;
			if (AdventureTimer <= 0) {				
				LoadVillage ();
				UIOverlay.Instance.OpenPopUp ("Adventure time is over!");
			}
		}
		timer += Time.deltaTime;
		if (CurrentlyOpeningChest != null && CurrentlyOpeningChest.ChestState != ChestState.Open) {
			if (timer > previousTimer + 1) {
				previousTimer++;
				CurrentlyOpeningChest.TickOpen (1);
			}
		}
	}

	public void SavePlayerShip (PlayerShip playerShip) {
		PlayerShipCoordinates = playerShip.gameObject.transform.position;
	}

	public void CreateShipDatas () {
		// CurrentTeam = new List<CreatureData> (5);

	}

	public void LoadBattle (int sceneIndex) {
		// PlayerShipRewardChests = new List<RewardChest> (GameManager.Instance.PlayerShip.RewardChests);
		SceneManager.LoadScene (sceneIndex);
	}

	public void LoadVillage () {
		Invoke ("ReloadChests", 0.1f);
		FarmIndex = 0;
		// RewardChests.Clear ();
		PlayerShipRewardChests.Clear ();
		for (int i = CurrentTeam.Count - 1; i >= 0; i--) {
			if (CurrentTeam [i].IsDead) {
				CurrentTeam.Remove (CurrentTeam [i]);
			}
		}
		Player.Instance.Inventory ["Map"] = 0;
		OnAdventure = false;
		Energy = EnergySave; 
		MaxEnergy = MaxEnergySave;
		SceneManager.LoadScene (0);
		// Invoke ("ReceiveAdventureReward", 0.5f);
		CurrentAdventure = Adventures [0];
		Invoke ("ReceiveAdventureReward", 0.1f);
	}

	public void NoticeChests () {
		foreach (var shipRewardChest in PlayerShipRewardChests) {
			ChestsFound++;
			RewardChests.Add (shipRewardChest);
		}
		Debug.Log (RewardChests.Count + "");
		PlayerShipRewardChests.Clear ();
		PlayerShip.Instance.CargoSlider.value = PlayerShipRewardChests.Count;
	}

	public void ReceiveAdventureReward () {
		Dictionary<string, int> totalReward = new Dictionary<string, int> ();
		Debug.Log (RewardChests.Count + "");
		foreach (var rewardChest in RewardChests) {
			// ChestsFound++;
			foreach (var rewardItem in rewardChest.RewardItems) {
				if (!totalReward.ContainsKey(rewardItem.Key)) {
					totalReward.Add (rewardItem.Key, rewardItem.Value);
				} else {
					totalReward [rewardItem.Key] += rewardItem.Value;
				}
			}
		}

		Player.Instance.TakeItems (totalReward);
		UIOverlay.Instance.OpenImagesPopUp ("Your reward:", totalReward);

		RewardChests.Clear ();
	}

	public void BeginChestOpen (RewardChest rewardChest) {
		timer = 0.0f;
		previousTimer = 0;
		rewardChest.StartOpen ();
		CurrentlyOpeningChest = rewardChest;
	}

	public void ReceiveChestReward (RewardChest rewardChest) {
		Player.Instance.TakeItems (rewardChest.RewardItems);
		UIOverlay.Instance.OpenImagesPopUp ("Your reward:", rewardChest.RewardItems);
		Player.Instance.RewardChests.Remove (rewardChest);
	}

	public void ReceiveReward (Dictionary<string, int> rewardItems) {
		Player.Instance.TakeItems (rewardItems);
		UIOverlay.Instance.OpenImagesPopUp ("Your reward:", rewardItems);
	}

	public void LoadAdventure () {
		if (!OnAdventure) {
			OnAdventure = true;
			AdventureTimer = CurrentAdventure.TimeLimit;
			Missions.Clear ();
			Tiles.Clear ();
			POIDataByTiles.Clear ();
			POIDatas.Clear ();
			AdventureStartedTime = GlobalTimer;
			Energy -= CurrentAdventure.EnergyCost;
			EnergySave = Energy;
			MaxEnergySave = MaxEnergy;
			MaxEnergy = CurrentAdventure.EnergyCost;
			Energy = MaxEnergy;
			ChestsFound = 0;
		} else {
			ReceivedReward = false;
		}
		SceneManager.LoadScene (2);
		Invoke ("ReloadChests", 0.1f);
	}

	public void ChangeAdventure (bool next) {
		if (next) {
			if (Adventures.IndexOf (CurrentAdventure) + 1 < Adventures.Count) {
				CurrentAdventure = Adventures [Adventures.IndexOf (CurrentAdventure) + 1];
			} else {
				CurrentAdventure = Adventures [0];
			}
		} else {
			if (Adventures.IndexOf (CurrentAdventure) - 1 >= 0) {
				CurrentAdventure = Adventures [Adventures.IndexOf (CurrentAdventure) - 1];
			} else {
				CurrentAdventure = Adventures [Adventures.Count - 1];
			}
		}
	}

	public void TakeGold (int amount) {
		Inventory ["Gold"] += amount;
		// Gold += amount;
	}

	public void GiveGold (int amount) {
		if (Gold >= amount) {
			Inventory ["Gold"] -= amount;
		} else {
			UIOverlay.Instance.OpenPopUp ("Not enough gold");
		}
	}

	public void TakeGems (int amount) {
		Inventory ["Gems"] += amount;
		// Gold += amount;
	}

	public void GiveGems (int amount) {
		if (Gems >= amount) {
			Inventory ["Gems"] -= amount;
		} else {
			UIOverlay.Instance.OpenPopUp ("Not enough gems");
		}
	}

	public void TakeExp (int amount) {
		Inventory ["Exp"] += amount;
		if (Inventory ["Exp"] >= ExpForLevel[Level]) {
			LevelUp ();
		}
	}

	public void LevelUp () {
		Inventory ["Exp"] -= ExpForLevel [Level];
		Level++;
	}

	public void GiveItems (Dictionary<string, int> amountsByItemNames) {
		// probably should check here
		foreach (var amountByItemName in amountsByItemNames) {
			if (!Inventory.ContainsKey(amountByItemName.Key)) {
				Inventory.Add (amountByItemName.Key, 0);
			}
			Inventory [amountByItemName.Key] -= amountByItemName.Value;
		}
	}

	public void TakeItems (Dictionary<string, int> amountsByItemNames) {
		foreach (var amountByItemName in amountsByItemNames) {
			if (!Inventory.ContainsKey(amountByItemName.Key)) {
				Inventory.Add (amountByItemName.Key, 0);
			}
			if (amountByItemName.Key == "Exp") {
				TakeExp (amountByItemName.Value);
			} else {
				Inventory [amountByItemName.Key] += amountByItemName.Value;
			}
		}
	}

	public bool CheckCost (Dictionary<string, int> amountsByItemNames) {
		foreach (var amountByItemName in amountsByItemNames) {
			if (!Inventory.ContainsKey(amountByItemName.Key)) {
				Inventory.Add (amountByItemName.Key, 0);
			}
			if (Inventory[amountByItemName.Key] < amountByItemName.Value) {
				return false;
			}
		}
		return true;
	}
}
