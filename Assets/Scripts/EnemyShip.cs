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

	void Awake () {
		player = Player.Instance;
		mover = GetComponent<MoveOnClick> ();
		mover.OnFinishedMoving += Mover_OnFinishedMoving;
		PlayerShip.Instance.OnPlayerTurn += PlayerShip_Instance_OnPlayerTurn;
	}

	void PlayerShip_Instance_OnPlayerTurn (PlayerShip sender) {
		MoveRandomly ();
	}

	void Mover_OnFinishedMoving (MoveOnClick sender) {
		if (CaughtPlayerShip == null) {
			return;
		}
		if (CaughtPlayerShip != null) {
			SelectableTile portalTile = Board.Instance.FindTileWithPOIKind (POIkind.Portal);
			CaughtPlayerShip.MoveToTile (portalTile, false, true);
			CaughtPlayerShip.ShowFlyingText (("-" + EnergyDamage), Color.red);
			Player.Instance.Energy -= EnergyDamage;
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
	}

	void OnTriggerEnter2D (Collider2D other) { // will work even when passing through
		if (other.GetComponent<PlayerShip> () != null) {
			CaughtPlayerShip = other.GetComponent<PlayerShip> ();
		}

	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<PlayerShip> () != null && other.GetComponent<PlayerShip> () == CaughtPlayerShip) {
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
		int index = Random.Range (0, CurrentTile.Neighbors.Count);
		MoveToTile (CurrentTile.Neighbors [index], false, false);
	}
}
