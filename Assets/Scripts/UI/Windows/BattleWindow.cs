﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour {

	public GameObject Window;

	public BattleCharBlock PlayerBlock;
	public BattleCharBlock EnemyBlock;

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
		EnemyBlock.SetCharacter (enemyChar);

		PlayerChar = playerChar;
		AttackingChar = PlayerChar;

		EnemyChar = enemyChar;
		DefendingChar = EnemyChar;

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

		isPlayerTurn = !isPlayerTurn;

		BattleShip tempChar = AttackingChar;
		AttackingChar = DefendingChar;
		DefendingChar = tempChar;

		PlayerBlock.ToggleTargets (!isPlayerTurn);
		EnemyBlock.ToggleTargets (isPlayerTurn);


		if (isPlayerTurn) {
			EnemyDefend ();
		}
	}
}
