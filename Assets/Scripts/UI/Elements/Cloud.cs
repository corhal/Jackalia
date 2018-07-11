using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour {

	public GameObject LevelTooltipObject;
	public GameObject CostTooltipObject;

	public int Cost;
	public int LevelRequirement;

	public Region Region;

	void Awake () {
		Region = GetComponentInParent<Region> ();
	}

	void Start () {
		LevelTooltipObject.GetComponentInChildren<Text> ().text = LevelRequirement + "";
		CostTooltipObject.GetComponentInChildren<Text> ().text = Cost + "";
	}

	public void Tap () {		
		/*if (Player.Instance.Level >= LevelRequirement) {
			if (!CostTooltipObject.activeSelf) {
				CostTooltipObject.SetActive (true);
			} else {
				TryReveal ();
			}
		} else if (!LevelTooltipObject.activeSelf) {
			LevelTooltipObject.SetActive (true);
		} else {
			// UIOverlay.Instance.OpenPopUp ("Level too low!");
			LevelTooltipObject.SetActive (false);
		}*/
		UIOverlay.Instance.OpenRegionPopUp (Region);
	}

	public void TryReveal () {
		if (Player.Instance.Gold >= Cost) {
			Player.Instance.GiveGold (Cost);
			Region.DestroyCloud (this);
		} else {
			UIOverlay.Instance.OpenPopUp ("Not enough gold!");
			CostTooltipObject.SetActive (false);
		}
	}
}
