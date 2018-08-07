using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour {

	public GameObject Window;

	public BattleCharBlock PlayerBlock;
	public BattleCharBlock EnemyBlock;

	public BattleShip PlayerChar;
	public BattleShip EnemyChar;

	public void Open (BattleShip playerChar, BattleShip enemyChar) {
		Window.SetActive (true);
		PlayerBlock.SetCharacter (playerChar);
		EnemyBlock.SetCharacter (enemyChar);
	}

	public void Close () {
		Window.SetActive (false);
	}
}
