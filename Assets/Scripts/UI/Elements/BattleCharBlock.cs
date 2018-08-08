using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharBlock : MonoBehaviour {

	public BattleShip Character;

	public Text CharLabel;
	public Image CharImage;
	public List<Image> Targets;
	public Slider HPbar;
	public Text HPlabel;
	public List<Image> AttackIcons;
	public List<Image> DefenseIcons;

	public void SetCharacter (BattleShip character) {
		Character = character;
		CharLabel.text = character.ShipData.Name;
		CharImage.sprite = character.ShipData.Sprite;

		if (Character.Alignment == "player") {
			foreach (var target in Targets) {
				target.gameObject.SetActive (false);
			}
		}

		HPbar.maxValue = Character.MaxHP;
		UpdateInfo ();

		for (int i = 0; i < character.AttacksCount; i++) {
			AttackIcons [i].gameObject.SetActive (true);
		}

		for (int i = 0; i < character.BlocksCount; i++) {
			DefenseIcons [i].gameObject.SetActive (true);
		}
	}

	public void UpdateInfo () {		
		HPbar.value = Character.HP;
		HPlabel.text = Character.HP + "/" + Character.MaxHP;
	}

	public void ToggleTargets (bool visible) {
		foreach (var target in Targets) {
			target.gameObject.SetActive (visible);
		}
	}
}
