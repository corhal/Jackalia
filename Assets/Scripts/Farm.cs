using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Selectable {

	protected override void Awake () {
		base.Awake ();
		Action collectAction = new Action("Collect", 0, player.DataBase.ActionIconsByNames["Info"], CollectFood);
		actions.Add (collectAction);
	}

	public void CollectFood () {
		Debug.Log ("collecting food lol");
	}
}
