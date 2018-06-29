using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactsBuyPopUp : MonoBehaviour {

	public GameObject Window;
	public Text MessageText;
	public Text BuyButtonLabel;
	public Image ArtifactIcon;

	public int ArtifactsToBuy;
	public int GemsCost;

	public int CurrentArtifactIndex;

	public void Open (int artifactIndex) {
		CurrentArtifactIndex = artifactIndex;
		Window.SetActive (true);
		ArtifactIcon.sprite = Player.Instance.Artifacts [CurrentArtifactIndex].Icon;
		MessageText.text = "No " + Player.Instance.Artifacts [CurrentArtifactIndex].Name + " artifacts left!\nBuy " + ArtifactsToBuy + " more?";
		BuyButtonLabel.text = "Yes " + GemsCost + "   ";
	}

	public void Close () {
		Window.SetActive (false);
	}

	public void BuyArtifacts () {
		if (Player.Instance.Gems >= GemsCost) {
			Close ();
			Player.Instance.Inventory [Player.Instance.Artifacts [CurrentArtifactIndex].Name] += ArtifactsToBuy;
		}
		Player.Instance.GiveGems (GemsCost);
		UIOverlay.Instance.RefreshArtifactsCooldown ();
	}
}
