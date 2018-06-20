using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour {

	Player player;
	MoveOnClick mover;

	public int EnergyPerDistance;

	public Collider2D lastSeenCollider;

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
		if (lastSeenCollider == null) {
			return;
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
		if (other.GetComponent<SelectableTile> () == null && other.GetComponentInParent<SelectableTile> () == null) {
			lastSeenCollider = other;
		}
		if (other.GetComponent<PlayerShip> () != null) {
			SelectableTile portalTile = Board.Instance.FindTileWithPOIKind (POIkind.Portal);
			PlayerShip.Instance.MoveToTile (portalTile, false, true);
			PlayerShip.Instance.ShowFlyingText (("-" + EnergyDamage), Color.red);
			Player.Instance.Energy -= EnergyDamage;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (lastSeenCollider == other) {
			lastSeenCollider = null;
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
