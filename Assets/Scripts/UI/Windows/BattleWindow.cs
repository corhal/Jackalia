using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour {

	public GameObject Window;

	public Sprite AttackSprite;
	public Sprite DefendSprite;

	public BattleCharBlock PlayerBlock;
	public BattleCharBlock EnemyBlock;

	public BattleCharBlock AttackingCharBlock;
	public BattleCharBlock DefendingCharBlock;

	public BattleShip AttackingChar;
	public BattleShip DefendingChar;

	public BattleShip PlayerChar;
	public BattleShip EnemyChar;

	public List<BodyPart> BodyPartsToAttack;

	int currentAttacksCounter;
	int currentBlocksCounter;

	public bool isPlayerTurn = true;

	public void Open (BattleShip playerChar, BattleShip enemyChar) {
		Window.SetActive (true);
		PlayerBlock.SetCharacter (playerChar);
		PlayerBlock.ToggleTargets (false);
		EnemyBlock.SetCharacter (enemyChar);

		PlayerChar = playerChar;
		AttackingChar = PlayerChar;
		AttackingCharBlock = PlayerBlock;

		EnemyChar = enemyChar;
		DefendingChar = EnemyChar;
		DefendingCharBlock = EnemyBlock;

		PlayerBlock.ToggleAttackIcons (isPlayerTurn);
		PlayerBlock.ToggleDefenseIcons (!isPlayerTurn);

		EnemyBlock.ToggleAttackIcons (!isPlayerTurn);
		EnemyBlock.ToggleDefenseIcons (isPlayerTurn);

		EnemyDefend ();
	}

	public void Close () {
		Window.SetActive (false);
	}

	public void AddAttack (int index) {
		currentAttacksCounter++;

		switch (index) {
		case 0:
			BodyPartsToAttack.Add (BodyPart.Head);
			break;
		case 1:
			BodyPartsToAttack.Add (BodyPart.Torso);
			break;
		case 2:
			BodyPartsToAttack.Add (BodyPart.Legs);
			break;
		default:
			break;
		}

		DefendingCharBlock.Targets [index].sprite = AttackSprite;
		AttackingCharBlock.AttackIcons [index].color = new Color (1.0f, 1.0f, 1.0f, 0.5f);

		if (AttackingChar.AttacksCount == currentAttacksCounter) {
			EndTurn ();
		}
	}

	public void AddBlock (int index) {
		currentBlocksCounter++;

		switch (index) {
		case 0:
			DefendingChar.BlockedBodyParts.Add (BodyPart.Head);
			break;
		case 1:
			DefendingChar.BlockedBodyParts.Add (BodyPart.Torso);
			break;
		case 2:
			DefendingChar.BlockedBodyParts.Add (BodyPart.Legs);
			break;
		default:
			break;
		}

		if (DefendingChar != EnemyChar) {
			DefendingCharBlock.Targets [index].sprite = DefendSprite;
			DefendingCharBlock.DefenseIcons [index].color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
		}

		if (currentBlocksCounter == DefendingChar.BlocksCount && !isPlayerTurn) {
			EnemyAttack ();
		}
	}

	public void EnemyDefend () {
		for (int i = 0; i < EnemyChar.BlocksCount; i++) {
			AddBlock(Random.Range(0, 3));
		}
	}

	public void EnemyAttack () {
		for (int i = 0; i < EnemyChar.AttacksCount; i++) {
			AddAttack(Random.Range(0, 3));
		}
		// EndTurn ();
	}

	public void EndTurn () {
		currentAttacksCounter = 0;
		currentBlocksCounter = 0;

		foreach (var bodyPartToAttack in BodyPartsToAttack) {
			AttackingChar.AttackTarget (DefendingChar, bodyPartToAttack);
		}

		PlayerBlock.UpdateInfo ();
		EnemyBlock.UpdateInfo ();

		PlayerChar.BlockedBodyParts.Clear ();
		EnemyChar.BlockedBodyParts.Clear ();
		BodyPartsToAttack.Clear ();

		if (DefendingChar.HP <= 0) {
			Close ();
			return;
		}

		isPlayerTurn = !isPlayerTurn;

		BattleShip tempChar = AttackingChar;
		AttackingChar = DefendingChar;
		DefendingChar = tempChar;

		BattleCharBlock tempBlock = AttackingCharBlock;
		AttackingCharBlock = DefendingCharBlock;
		DefendingCharBlock = tempBlock;

		PlayerBlock.ToggleTargets (!isPlayerTurn);
		EnemyBlock.ToggleTargets (isPlayerTurn);

		PlayerBlock.ToggleAttackIcons (isPlayerTurn);
		PlayerBlock.ToggleDefenseIcons (!isPlayerTurn);

		EnemyBlock.ToggleAttackIcons (!isPlayerTurn);
		EnemyBlock.ToggleDefenseIcons (isPlayerTurn);


		if (isPlayerTurn) {
			EnemyDefend ();
		}

		PlayerBlock.ResetTargets ();
		EnemyBlock.ResetTargets ();
	}

	public void ShowFlyingText (string message, Color color) {
		/*GameObject flyingTextObject = Instantiate (FlyingTextPrefab) as GameObject;
		flyingTextObject.transform.SetParent (GetComponentInChildren<Canvas> ().transform);
		flyingTextObject.transform.localScale = Vector3.one * 1.5f;
		flyingTextObject.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
		BJFlyingText flyingText = flyingTextObject.GetComponent<BJFlyingText> ();
		flyingText.Label.color = color;
		flyingText.Label.text = message;*/
	}
}
