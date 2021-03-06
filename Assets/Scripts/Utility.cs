﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Utility {

	public static T RandomEnumValue<T> () {
		var v = System.Enum.GetValues (typeof(T));
		return (T)v.GetValue (new System.Random ().Next (v.Length));
	}

	public static float SnapNumberToFactor(float number, float factor) {
		int multiple =  Mathf.RoundToInt(number/factor);

		return multiple * factor;
	}

	public static string SecondsToTime (int time) {
		int hours = time / 3600;
		int minutes = (time - hours * 3600) / 60;
		int seconds = time - hours * 3600 - minutes * 60;
		return (hours + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2"));
	}

	public static GameObject CastRayToMouse(int layerMask) {		
		Vector3 mousePoint;
		Vector2 mousePoint2D;

		mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePoint2D = new Vector2 (mousePoint.x, mousePoint.y);
		RaycastHit2D hit = Physics2D.Raycast (mousePoint2D, Vector2.zero, 100.0f, layerMask);

		GameObject hitObject = null;

		if (hit.collider != null) {
			hitObject = hit.collider.gameObject;
		}

		return hitObject;
	}

	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = Random.Range (i, count);
			var tmp = ts [i];
			ts [i] = ts [r];
			ts [r] = tmp;
		}
	}

	public static bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		for (int i = 0; i < results.Count; i++) {
			if (results[i].gameObject.GetComponent<Text> () != null) {
				results.RemoveAt (i); // Довольно сомнительное решение, которое я сейчас обосновываю тем, что текстовые лейблы, как правило, некликабельны
			}
		}
		return results.Count > 0;
	}

	// sum=(a+b)(b-a+1)/2

	public static float MathExpectation(float intervalStart, float intervalEnd) {
		float sum = (intervalStart + intervalEnd) * (intervalEnd - intervalStart + 1) / 2;
		return sum / (intervalEnd - intervalStart);
	}

	/*public static IOrderedEnumerable<KeyValuePair<Client, int>> SortDictionary(Dictionary<Client, int> dictionary, bool descending) {
		// Order by values.
		// ... Use LINQ to specify sorting by value.
		IOrderedEnumerable<KeyValuePair<Client, int>> items = null;
		if (!descending) {
			items = from pair in dictionary
				orderby pair.Value ascending
				select pair;
		} else {
			items = from pair in dictionary
				orderby pair.Value descending
				select pair;
		}	

		return items;		
	}*/
}
