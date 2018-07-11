using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour {

	public string Name;
	public int InitialCloudsAmount;

	public List<Cloud> Clouds;

	public GameObject MissionMarkerObject;

	void Awake () {
		Clouds = new List<Cloud> (GetComponentsInChildren<Cloud> ());
		InitialCloudsAmount = Clouds.Count;
	}

	public void DestroyCloud (Cloud cloud) {
		if (Clouds.Contains (cloud)) {
			Clouds.Remove (cloud);
			Destroy (cloud.gameObject);
			Player.Instance.RegionCloudCounts [Name] = Clouds.Count;
		}
		if (Clouds.Count == 0) {
			Unlock ();
		}
	}

	public void Unlock () {
		foreach (var cloud in Clouds) {
			Destroy (cloud.gameObject);
		}
		Clouds.Clear ();
		MissionMarkerObject.SetActive (true);
	}
}
