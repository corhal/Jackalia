using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharBlock : MonoBehaviour {

	public Text CharLabel;
	public Image CharImage;
	public List<Image> Targets;
	public Slider HPbar;
	public Text HPlabel;
	public List<Image> AttackIcons;
	public List<Image> DefenseIcons;

	public void SetCharacter (BattleShip character) {
		CharLabel.text = character.ShipData.Name;
		CharImage.sprite = character.ShipData.Sprite;

		foreach (var target in Targets) {
			target.gameObject.SetActive (false);
		}

		HPbar.maxValue = character.MaxHP;
		HPbar.value = character.MaxHP;
		HPlabel.text = character.HP + "/" + character.MaxHP;

		for (int i = 0; i < character.AttacksCount; i++) {
			AttackIcons [i].gameObject.SetActive (true);
		}

		for (int i = 0; i < character.BlocksCount; i++) {
			DefenseIcons [i].gameObject.SetActive (true);
		}
	}
}
