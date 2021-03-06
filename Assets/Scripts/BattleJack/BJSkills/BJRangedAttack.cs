﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJRangedAttack : BJSkill {
	
	public GameObject Projectile;
	public GameObject ProjectilePrefab;
	bool shouldMoveProjectile;
	float speed = 10.0f;
	float initialZ = -7.0f;


	public override void UseSkill (BJCreatureObject user, BJCreatureObject mainTarget) {	
		base.UseSkill (user, mainTarget);	
		CurrentUser = user;
		CurrentMainTarget = mainTarget;
		Projectile = Instantiate (ProjectilePrefab) as GameObject;
		shouldMoveProjectile = true;
		Projectile.transform.position = new Vector3 (CurrentUser.transform.position.x, CurrentUser.transform.position.y, initialZ);
	}

	public override void AssignSkillIndexes () {
		ValidTargetIndexes = new List<int> { 0, 1, 2, 3, 4 };
	}

	void Update () {
		if (shouldMoveProjectile) {
			float step = speed * Time.deltaTime;
			Projectile.transform.position = Vector2.MoveTowards (Projectile.transform.position, CurrentMainTarget.transform.position, step);
			Projectile.transform.position = new Vector3 (Projectile.transform.position.x, Projectile.transform.position.y, initialZ);
			if (Vector2.Distance (Projectile.transform.position, CurrentMainTarget.transform.position) < 0.001f) {				
				shouldMoveProjectile = false;
				Destroy (Projectile);
				CurrentUser.DealDamage ((int)CurrentUser.Creature.BaseDamage, 1.0f, CurrentMainTarget);
				for (int i = 0; i < Effects.Count; i++) {
					if (Random.Range(0.0f, 0.99f) < EffectChances [i]) {
						Effects [i].Applier = CurrentUser;
						CurrentMainTarget.ApplyEffect (Effects [i], true);
					}
				}
				StartCoroutine(FinishSkill (0.1f));
			}
		}
	}
}
