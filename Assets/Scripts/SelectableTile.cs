using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableTile : Selectable {

	public GameObject ParticleSystem;
	public GameObject ColliderObject;
	public Vector2Int BoardCoords;
	public Vector2Int AbsBoardCoords;
	public string BoardCoordsAsString;

	public POIkind PointOfInterest;

	public List<SelectableTile> Neighbors;
	public List<SelectableTile> FullNeighbors;
	public List<SelectableTile> DiagonalNeighbors;

	protected override void Awake () {
		base.Awake ();
		Neighbors = new List<SelectableTile> ();
		FullNeighbors = new List<SelectableTile> ();
	}

	public void StopParticles () {
		ParticleSystem.SetActive (false);
		//ColliderObject.SetActive (false);
		Player.Instance.Tiles [BoardCoordsAsString] = false;
	}

	public override void MoveShipHere (bool teleport) {		
		if (Player.Instance.Energy >= GameManager.Instance.PlayerShip.EnergyPerDistance * 1) {
			StopParticles ();
		}
		base.MoveShipHere (teleport);
	}
}
