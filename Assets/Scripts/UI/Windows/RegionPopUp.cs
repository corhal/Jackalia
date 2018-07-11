using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionPopUp : MonoBehaviour {

	public GameObject Window;
	public Text Header;

	public Text LowLevelLabel;

	public Button UnlockButton;
	public Text UnlockButtonLabel;

	public Button OkButton;

	public Text UnlockHeader;
	public Slider UnlockSlider;
	public Text UnlockLabel;

	public Region CurrentRegion;

	public void Open (Region region) {
		Window.SetActive (true);

		CurrentRegion = region;
		Header.text = region.Name;

		if (Player.Instance.Level < region.Clouds [0].LevelRequirement) {
			LowLevelLabel.gameObject.SetActive (true);
			UnlockButton.gameObject.SetActive (false);
			UnlockButtonLabel.gameObject.SetActive (false);
			UnlockSlider.gameObject.SetActive (false);
			UnlockLabel.gameObject.SetActive (false);
			UnlockHeader.gameObject.SetActive (false);
			OkButton.gameObject.SetActive (true);
			LowLevelLabel.text = "You need level " + region.Clouds [0].LevelRequirement + "!";

		} else {
			LowLevelLabel.gameObject.SetActive (false);
			UnlockButton.gameObject.SetActive (true);
			UnlockButtonLabel.gameObject.SetActive (true);
			UnlockSlider.gameObject.SetActive (true);
			UnlockLabel.gameObject.SetActive (true);
			UnlockHeader.gameObject.SetActive (true);
			OkButton.gameObject.SetActive (false);

			UnlockButtonLabel.text = "Unlock        " + region.Clouds [0].Cost;
			UnlockSlider.maxValue = region.InitialCloudsAmount;
			UnlockSlider.value = region.InitialCloudsAmount - region.Clouds.Count;
			UnlockLabel.text = (region.InitialCloudsAmount - region.Clouds.Count) + "/" + region.InitialCloudsAmount;
		}
	}

	public void TryUnlock () {
		if (CurrentRegion.Clouds[0].Cost > Player.Instance.Gold) {
			UIOverlay.Instance.OpenPopUp ("Not enough gold!");
		} else {
			Player.Instance.GiveGold (CurrentRegion.Clouds [0].Cost);
			CurrentRegion.DestroyCloud (CurrentRegion.Clouds [0]);
			Close ();
		}
	}

	public void Close () {
		Window.SetActive (false);
	}
}
