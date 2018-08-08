using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPart {
	Head, Torso, Legs
}

[System.Serializable]
public class BattleShip {

	public string Alignment;
	public ShipData ShipData;

	public int MaxHP;
	public int HP;
	public int Attack;
	public float AttackSpeed;

	public int ArmorPercent;

	public int AttacksCount;
	public int BlocksCount;

	public List<BodyPart> BlockedBodyParts;

	public BattleShip Target;

	float timer;

	public delegate void DamageTaken (BattleShip sender, int amount);
	public event DamageTaken OnDamageTaken; 

	public delegate void AttackedTarget (BattleShip sender);
	public event AttackedTarget OnAttackedTarget; 

	public void Tick (float timeDelta) {
		/*timer += timeDelta;
		if (Target != null && HP > 0 && Target.HP > 0 && timer >= 1.0f / AttackSpeed) {
			timer = 0;
			AttackTarget ();
		}*/
	}

	public void AttackTarget (BattleShip target, BodyPart bodyPart) {
		Target.TakeDamage (Attack, bodyPart);
		if (OnAttackedTarget != null) {
			OnAttackedTarget (this);
		}
	}

	public void TakeDamage (int amount, BodyPart bodyPart) {
		if (BlockedBodyParts.Contains(bodyPart)) {
			Debug.Log ("Blocked " + (int)(amount * (ArmorPercent / 100.0f)) + " damage!");
			amount -= (int)(amount * (ArmorPercent / 100.0f));
		}
		HP -= amount;
		if (OnDamageTaken != null) {
			OnDamageTaken (this, amount);
		}
	}
}
