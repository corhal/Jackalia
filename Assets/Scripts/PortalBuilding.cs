using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBuilding : Selectable {


	protected override void Awake () {
		base.Awake ();
		Action openAdventureWindowAction = new Action("Adventures", 0, player.DataBase.ActionIconsByNames["Info"], OpenAdventureWindow);
		actions.Add (openAdventureWindowAction);
	}
		

	public void OpenAdventureWindow () {
		uiManager.OpenAdventureWindow ();
	}

}
