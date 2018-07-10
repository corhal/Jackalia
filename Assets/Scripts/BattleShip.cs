using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleShip {

	public int HP;
	public int Attack;
	public float AttackSpeed;

	public BattleShip Target;

	float timer;

	public delegate void DamageTaken (BattleShip sender, int amount);
	public event DamageTaken OnDamageTaken; 

	public delegate void AttackedTarget (BattleShip sender);
	public event AttackedTarget OnAttackedTarget; 

	public void Tick (float timeDelta) {
		timer += timeDelta;
		if (Target != null && HP > 0 && Target.HP > 0 && timer >= 1.0f / AttackSpeed) {
			timer = 0;
			AttackTarget ();
		}
	}

	public void AttackTarget () {
		Target.TakeDamage (Attack);
		if (OnAttackedTarget != null) {
			OnAttackedTarget (this);
		}
	}

	public void TakeDamage (int amount) {
		HP -= amount;
		if (OnDamageTaken != null) {
			OnDamageTaken (this, amount);
		}
	}
}
