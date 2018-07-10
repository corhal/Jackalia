using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShip : MonoBehaviour {

	Player player;
	public static PlayerShip Instance;
	MoveOnClick mover;

	public SpriteRenderer ShipSprite;
	public int EnergyPerDistance;

	public Collider2D lastSeenCollider;

	public int RewardChestsCapacity;
	// public List<RewardChest> RewardChests;

	public SelectableTile CurrentTile;

	public List<GameObject> Arrows;

	SelectableTile origin;

	public GameObject FlyingTextPrefab;

	public Slider CargoSlider;

	public delegate void PlayerTurn (PlayerShip sender);
	public event PlayerTurn OnPlayerTurn;

	public BattleShip BattleShip;
	public EnemyShip CaughtEnemyShip;

	public Vector3 InitialParticlesPosition;
	public ParticleSystem ShootParticles;

	void Awake () {
		if (Instance == null) {			
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);  
		}
		player = Player.Instance;
		mover = GetComponent<MoveOnClick> ();
		mover.OnFinishedMoving += Mover_OnFinishedMoving;
		CargoSlider.maxValue = RewardChestsCapacity;
		// RewardChests = new List<RewardChest> ();
		BattleShip.OnDamageTaken += BattleShip_OnDamageTaken;
		BattleShip.OnAttackedTarget += BattleShip_OnAttackedTarget;
		InitialParticlesPosition = ShootParticles.gameObject.transform.position;
	}

	void BattleShip_OnAttackedTarget (BattleShip sender) {
		ShootParticles.gameObject.transform.position = new Vector3 (InitialParticlesPosition.x + Random.Range (-0.5f, 0.5f), InitialParticlesPosition.y + Random.Range (-0.5f, 0.5f), InitialParticlesPosition.z);
		ShootParticles.Play ();
	}

	void BattleShip_OnDamageTaken (BattleShip sender, int amount) {
		ShowFlyingText ("-" + amount + " HP", Color.red);
	}

	void Mover_OnFinishedMoving (MoveOnClick sender) {
		Invoke ("CheckEnemyShip", 0.1f);
		if (lastSeenCollider == null) {
			return;
		} 
	}

	void CheckEnemyShip () {
		if (CaughtEnemyShip == null) {
			return;
		}
		if (CaughtEnemyShip != null) {
			//SelectableTile portalTile = Board.Instance.FindTileWithPOIKind (POIkind.Portal);
			//CaughtPlayerShip.MoveToTile (portalTile, false, true);
			//CaughtPlayerShip.ShowFlyingText (("-" + EnergyDamage), Color.red);
			//Player.Instance.Energy -= EnergyDamage;
			InitialParticlesPosition = ShootParticles.gameObject.transform.position;
			CaughtEnemyShip.InitialParticlesPosition = CaughtEnemyShip.ShootParticles.gameObject.transform.position;
			BattleShip.Target = CaughtEnemyShip.BattleShip;
			CaughtEnemyShip.BattleShip.Target = BattleShip;
		}
	}

	void Start () {
		mover.EnergyPerDistance = EnergyPerDistance;

		Collider2D[] otherColliders = Physics2D.OverlapCircleAll (transform.position, 0.1f);
		foreach (var otherCollider in otherColliders) {
			if (otherCollider.gameObject.GetComponent<SelectableTile> () != null) {
				CurrentTile = otherCollider.gameObject.GetComponent<SelectableTile> ();
			}
		}

		if (Player.Instance.CurrentShipData.Special == "Diagonal") {
			foreach (var arrow in Arrows) {
				arrow.transform.RotateAround (transform.position, Vector3.forward, 45.0f);
			}
		}

		HideArrows ();
		ShowArrows ();
		CargoSlider.value = Player.Instance.RewardChests.Count;

		ShipSprite.sprite = Player.Instance.CurrentShipData.Sprite;

		if (Player.Instance.CurrentShipData.Special == "Cargo") {
			RewardChestsCapacity++;
			CargoSlider.maxValue = RewardChestsCapacity;
		}
	}

	void Update () {
		BattleShip.Tick (Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D other) { // will work even when passing through
		if (other.GetComponent<SelectableTile> () == null && other.GetComponentInParent<SelectableTile> () == null) {
			lastSeenCollider = other;
		}
		if (other.GetComponent<EnemyShip> () != null) {
			CaughtEnemyShip = other.GetComponent<EnemyShip> ();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (lastSeenCollider == other) {
			lastSeenCollider = null;
		}
		if (other.GetComponent<EnemyShip> () != null) {
			BattleShip.Target = null;
			CaughtEnemyShip.BattleShip.Target = null;
			CaughtEnemyShip = null;
		}
	}

	public void MoveToTile (SelectableTile tile, bool spendEnergy, bool teleport) {
		float arrowsDelay = teleport ? 0.0f : 1.5f;
		if (!spendEnergy) {
			if (teleport) {
				mover.Stop ();
				transform.position = tile.transform.position;
			} else {
				mover.MoveToPoint (tile.transform.position);
			}
			CurrentTile = tile;
			HideArrows ();
			Invoke ("ShowArrows", arrowsDelay);
			return;
		}
		if (player.Energy >= EnergyPerDistance * 1) {
			player.Energy -= EnergyPerDistance * 1;
			origin = CurrentTile; // currently only happens on non-free movement
			if (teleport) {
				transform.position = tile.transform.position;
			} else {
				mover.MoveToPoint (tile.transform.position);
			}
			CurrentTile = tile;
			HideArrows ();
			Invoke ("ShowArrows", arrowsDelay);
		} else {
			UIOverlay.Instance.OpenEnergyPopup ();
		}
	}

	public void FallBack (bool spendEnergy) {
		MoveToTile (origin, spendEnergy, true);
	}

	public void HideArrows () {
		foreach (var arrow in Arrows) {
			arrow.SetActive (false);
		}
	}

	public void ShowArrows () {		
		for (int i = 0; i < CurrentTile.Neighbors.Count; i++) {
			if (CurrentTile.Neighbors [i].AbsBoardCoords.x > CurrentTile.AbsBoardCoords.x) {
				Arrows [0].SetActive (true);
			}
			if (CurrentTile.Neighbors [i].AbsBoardCoords.y < CurrentTile.AbsBoardCoords.y) {
				Arrows [1].SetActive (true);
			}		
			if (CurrentTile.Neighbors [i].AbsBoardCoords.x < CurrentTile.AbsBoardCoords.x) {
				Arrows [2].SetActive (true);
			}
			if (CurrentTile.Neighbors [i].AbsBoardCoords.y > CurrentTile.AbsBoardCoords.y) {
				Arrows [3].SetActive (true);
			}
		}	
	}

	public void ShowFlyingText (string message, Color color) {
		GameObject flyingTextObject = Instantiate (FlyingTextPrefab) as GameObject;
		flyingTextObject.transform.SetParent (GetComponentInChildren<Canvas> ().transform);
		flyingTextObject.transform.localScale = Vector3.one * 1.5f;
		flyingTextObject.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
		BJFlyingText flyingText = flyingTextObject.GetComponent<BJFlyingText> ();
		flyingText.Label.color = color;
		flyingText.Label.text = message;
	}

	void OnDestroy () {
		mover.OnFinishedMoving -= Mover_OnFinishedMoving;
	}

	public void TakeChestReward (RewardChest rewardChest) {
		Player.Instance.PlayerShipRewardChests.Add (rewardChest);
		CargoSlider.value = Player.Instance.PlayerShipRewardChests.Count;
		//Player.Instance.RewardChests.Add (rewardChest);
		// CargoSlider.value = Player.Instance.RewardChests.Count;
		//Player.Instance.OpenChest (rewardChest);
		//Player.Instance.RewardChests.Add (rewardChest);
	}

	public void MakePlayerTurn () {
		UIOverlay.Instance.RefreshArtifactsCooldown ();
		Player.Instance.CurrentShipData.TickSpecialCooldown ();
		if (OnPlayerTurn != null) {
			OnPlayerTurn (this);
		}
	}
}
