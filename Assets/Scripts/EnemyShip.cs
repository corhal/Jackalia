using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour {

	Player player;
	MoveOnClick mover;

	public int EnergyPerDistance;

	public PlayerShip CaughtPlayerShip;

	public SelectableTile CurrentTile;

	public int EnergyDamage;

	SelectableTile origin;

	public GameObject FlyingTextPrefab;

	public BattleShip BattleShip;
	public ParticleSystem ShootParticles;
	public Vector3 InitialParticlesPosition;

	void Awake () {
		player = Player.Instance;
		mover = GetComponent<MoveOnClick> ();
		mover.OnFinishedMoving += Mover_OnFinishedMoving;
		PlayerShip.Instance.OnPlayerTurn += PlayerShip_Instance_OnPlayerTurn;
		BattleShip.OnDamageTaken += BattleShip_OnDamageTaken;
		BattleShip.OnAttackedTarget += BattleShip_OnAttackedTarget;
	}

	void BattleShip_OnDamageTaken (BattleShip sender, int amount) {
		ShowFlyingText ("-" + amount + " HP", Color.red);
		if (sender.HP <= 0) {
			Destroy (gameObject);
		}
	}

	void BattleShip_OnAttackedTarget (BattleShip sender) {
		ShootParticles.gameObject.transform.position = new Vector3 (InitialParticlesPosition.x + Random.Range (-0.5f, 0.5f), InitialParticlesPosition.y + Random.Range (-0.5f, 0.5f), InitialParticlesPosition.z);
		ShootParticles.Play ();
	}

	void PlayerShip_Instance_OnPlayerTurn (PlayerShip sender) {
		MoveRandomly ();
	}

	void Mover_OnFinishedMoving (MoveOnClick sender) {
		Invoke ("CheckPlayerShip", 0.1f);
	}

	void CheckPlayerShip () {
		if (CaughtPlayerShip == null) {
			return;
		}
		if (CaughtPlayerShip != null) {
			//SelectableTile portalTile = Board.Instance.FindTileWithPOIKind (POIkind.Portal);
			//CaughtPlayerShip.MoveToTile (portalTile, false, true);
			//CaughtPlayerShip.ShowFlyingText (("-" + EnergyDamage), Color.red);
			//Player.Instance.Energy -= EnergyDamage;
			InitialParticlesPosition = ShootParticles.gameObject.transform.position;
			CaughtPlayerShip.InitialParticlesPosition = CaughtPlayerShip.ShootParticles.gameObject.transform.position;
			BattleShip.Target = CaughtPlayerShip.BattleShip;
			CaughtPlayerShip.BattleShip.Target = BattleShip;
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
		InitialParticlesPosition = ShootParticles.gameObject.transform.position;
	}

	void Update () {
		BattleShip.Tick (Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D other) { // will work even when passing through
		if (other.GetComponent<PlayerShip> () != null) {
			CaughtPlayerShip = other.GetComponent<PlayerShip> ();
		}

	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<PlayerShip> () != null && other.GetComponent<PlayerShip> () == CaughtPlayerShip) {
			BattleShip.Target = null;
			CaughtPlayerShip.BattleShip.Target = null;
			CaughtPlayerShip = null;
		}
	}

	public void MoveToTile (SelectableTile tile, bool spendEnergy, bool teleport) {
		float arrowsDelay = teleport ? 0.0f : 1.5f;
		if (!spendEnergy) {
			if (teleport) {
				transform.position = tile.transform.position;
			} else {
				mover.MoveToPoint (tile.transform.position);
			}
			CurrentTile = tile;
			return;
		}
		origin = CurrentTile; // currently only happens on non-free movement
		if (teleport) {
			transform.position = tile.transform.position;
		} else {
			mover.MoveToPoint (tile.transform.position);
		}
		CurrentTile = tile;
	}

	public void FallBack (bool spendEnergy) {
		MoveToTile (origin, spendEnergy, true);
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
		PlayerShip.Instance.OnPlayerTurn -= PlayerShip_Instance_OnPlayerTurn;
	}

	public void MoveRandomly () {
		float coinToss = Random.Range (0.0f, 1.0f);
		if (coinToss >= 0.5f) {
			int index = Random.Range (0, CurrentTile.Neighbors.Count);
			MoveToTile (CurrentTile.Neighbors [index], false, false);
		}
	}
}
