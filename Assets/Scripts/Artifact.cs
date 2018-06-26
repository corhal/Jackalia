using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour {

	public string Name;

	public bool IsActivated;

	public int Cooldown;
	public int CurrentCooldown;

	public void Use () {
		CurrentCooldown = Cooldown;
		IsActivated = !IsActivated;
	}
}
