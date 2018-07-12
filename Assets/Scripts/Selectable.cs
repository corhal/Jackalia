using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

	public bool IsAvailable = true;

	protected List<Action> actions;
	public List<Action> Actions {  get { return actions; } }
	public List<string> StatNames;

	public string Name;
	public int Level;
	public int MaxLevel;
	public Allegiance Allegiance;

	public string Process;
	public bool InProcess;
	public float InitialProcessSeconds;

	protected UIOverlay uiManager;
	protected GameManager gameManager;
	protected Player player;

	bool animate;

	SpriteRenderer mySprite;
	Color initialColor;


	protected virtual void Awake () {
		actions = new List<Action> ();
		gameManager = GameManager.Instance;
		uiManager = UIOverlay.Instance;
		player = Player.Instance;
		Action infoAction = new Action ("Info", 0, player.DataBase.ActionIconsByNames ["Info"], ShowInfo);
		//Action moveAction = new Action("Move", 0, player.DataBase.ActionIconsByNames["Info"], MoveShipHere);
		actions.Add (infoAction);
		//actions.Add (moveAction);
	}

	public void RemoveActionByName (string actionName) {
		foreach (var action in actions) {
			if (action.Name == actionName) {
				actions.Remove (action);
			}
		}
	}

	public virtual void ShowInfo () {
		uiManager.OpenSelectableInfo (this);
	}

	public virtual void MoveShipHere (bool teleport) {
		PlayerShip.Instance.MoveToTile (this as SelectableTile, true, teleport);
		uiManager.CloseContextButtons (true);
	}

	public virtual float GetProcessSeconds () {
		return 0.0f;
	}

	protected virtual void Start () {
		mySprite = GetComponentInChildren<SpriteRenderer> ();
		initialColor = mySprite.color;
	}

	protected virtual void Update () {
		if (animate) {
			mySprite.color = Color.Lerp(initialColor, Color.black, Mathf.PingPong(Time.time, 1));
		}
	}

	public virtual int GetStatByString (string statName) {
		return 0;
	}

	public virtual int GetUpgradedStatByString (string statName) {
		return 0;
	}

	public virtual void Deanimate () {
		animate = false;
		mySprite.color = initialColor;
	}

	public virtual void Animate () {
		animate = true;
	}

	void OnMouseUp () {
		Invoke ("RealClick", 0.1f);
		if (!Player.Instance.OnAdventure && !Utility.IsPointerOverUIObject ()) {
			uiManager.OpenContextButtons (this);
		}
	}

	void RealClick () {
		if (Player.Instance.OnAdventure && !GameManager.Instance.CameraDragged && IsAvailable && !Utility.IsPointerOverUIObject () && Allegiance != Allegiance.Enemy) {
			if (Player.Instance.ActiveArtifact.Name == "Jump" && Player.Instance.ActiveArtifact.IsActivated) {
				foreach (var neighborTile in GameManager.Instance.PlayerShip.CurrentTile.Neighbors) {
					if (neighborTile.Neighbors.Contains(this as SelectableTile) && !GameManager.Instance.PlayerShip.CurrentTile.FullNeighbors.Contains(this as SelectableTile)) {
						Animate ();
						PlayerShip.Instance.MakePlayerTurn ();
						MoveShipHere (true);
						Deanimate ();	
						Player.Instance.ActiveArtifact.Use ();
						return;
					}
				}
			}


				
			if (Player.Instance.ActiveArtifact.Name == "Bash" && Player.Instance.ActiveArtifact.IsActivated) {
				foreach (var enemyShip in GameManager.Instance.EnemyShips) {
					if (enemyShip.CurrentTile == (this as SelectableTile)) {
						enemyShip.MoveRandomly (1.0f);
						PlayerShip.Instance.ShowFlyingText ("Bash!", Color.green);
					}
				}
				// (this as SelectableTile).StopParticles ();
				Player.Instance.ActiveArtifact.Use ();
				return;
			}


			if (Player.Instance.ActiveArtifact.Name == "Scry" && Player.Instance.ActiveArtifact.IsActivated) {
				(this as SelectableTile).StopParticles ();
				Player.Instance.ActiveArtifact.Use ();
				return;
			}

			if (Player.Instance.CurrentShipData.Special != "Diagonal" && !GameManager.Instance.PlayerShip.CurrentTile.Neighbors.Contains ((this as SelectableTile))) {
				return;
			}

			if (Player.Instance.CurrentShipData.Special == "Diagonal" && !GameManager.Instance.PlayerShip.CurrentTile.DiagonalNeighbors.Contains ((this as SelectableTile))) {
				return;
			}

			Animate ();
			PlayerShip.Instance.MakePlayerTurn ();
			MoveShipHere (false);
			Deanimate ();		
		}
	}
}
