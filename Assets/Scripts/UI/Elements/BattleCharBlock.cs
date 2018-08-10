using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharBlock : MonoBehaviour {

	public BattleShip Character;

	public Text CharLabel;
	public Image CharImage;
	public List<Image> Targets;
	public Slider HPbar;
	public Text HPlabel;
	public List<Image> AttackIcons;
	public List<Image> DefenseIcons;

	public Sprite TargetSprite;

	public Vector3 InitialPosition;

	public GameObject FlyingTextPrefab;

	void Start () {
		InitialPosition = CharImage.transform.position;
	}

	public void SetCharacter (BattleShip character) {
		Character = character;
		CharLabel.text = character.ShipData.Name;
		CharImage.sprite = character.ShipData.Sprite;

		/*if (Character.Alignment == "player") {
			foreach (var target in Targets) {
				target.gameObject.SetActive (false);
			}
		}*/

		HPbar.maxValue = Character.MaxHP;
		UpdateInfo ();

		for (int i = 0; i < character.AttacksCount; i++) {
			AttackIcons [i].gameObject.SetActive (true);
		}

		for (int i = 0; i < character.BlocksCount; i++) {
			DefenseIcons [i].gameObject.SetActive (true);
		}

		character.OnDamageTaken += Character_OnDamageTaken;
	}

	void Character_OnDamageTaken (BattleShip sender, BodyPart bodyPart, bool block, int amount) {
		string messageString = "";
		if (block) {
			messageString = "Block! ";
		}
		ShowFlyingText (messageString + "-" + amount, Targets [(int)bodyPart].transform.position, Color.red);
	}

	public void ShowFlyingText (string message, Vector3 origin, Color color) {
		GameObject flyingTextObject = Instantiate (FlyingTextPrefab) as GameObject;
		flyingTextObject.transform.SetParent (transform);
		flyingTextObject.transform.localScale = Vector3.one * 1.5f;
		flyingTextObject.transform.position = new Vector3 (origin.x, origin.y + 0.5f, origin.z);
		BJFlyingText flyingText = flyingTextObject.GetComponent<BJFlyingText> ();
		flyingText.Label.color = color;
		flyingText.Label.text = message;
	}

	public void UpdateInfo () {		
		HPbar.value = Character.HP;
		HPlabel.text = Character.HP + "/" + Character.MaxHP;
	}

	public void ToggleTargets (bool visible) {
		foreach (var target in Targets) {
			target.gameObject.SetActive (visible);
		}
	}

	public void ToggleAttackIcons (bool myTurn) {
		float alpha = myTurn ? 1.0f : 0.5f; 
		foreach (var attackIcon in AttackIcons) {
			attackIcon.color = new Color (1.0f, 1.0f, 1.0f, alpha);
		}
	}

	public void ToggleDefenseIcons (bool myTurn) {
		float alpha = myTurn ? 1.0f : 0.5f; 
		foreach (var defenseIcon in DefenseIcons) {
			defenseIcon.color = new Color (1.0f, 1.0f, 1.0f, alpha);
		}
	}

	public void ResetTargets () {
		foreach (var target in Targets) {
			target.sprite = TargetSprite;
		}
	}

	bool shouldMove;
	Vector3 secondaryPosition;
	Vector3 targetPosition;

	public void MoveToPoint (Vector3 target) {
		shouldMove = true;
		startTime = Time.time;
		secondaryPosition = CharImage.transform.position;
		targetPosition = target;
		journeyLength = Vector3.Distance(secondaryPosition, targetPosition );
	}

	public float MoveSpeed = 10.0F;
	private float startTime;
	private float journeyLength;

	public Color InitialColor;
	bool animate;

	bool showHit;
	public float TintTime;

	float t;

	public delegate void CharBlockEventHandler (BattleCharBlock battleCharBlock);
	public event CharBlockEventHandler OnMovementFinished;

	void Update () {
		/*if (animate) {
			CreatureImage.color = Color.Lerp(InitialColor, Color.black, Mathf.PingPong(Time.time, 1));
		}

		if (showHit) {						
			CreatureImage.color = Color.Lerp(InitialColor, Color.red, t);
			if (t < 1){
				t += Time.deltaTime/TintTime;
			} else {
				showHit = false;
				t = 0;
				CreatureImage.color = InitialColor;
			}
		}*/

		if (shouldMove) {
			float distCovered = (Time.time - startTime) * MoveSpeed;
			float fracJourney = distCovered / journeyLength;
			CharImage.transform.position = Vector3.Lerp(secondaryPosition, targetPosition, fracJourney);
			if (Vector3.Distance(CharImage.transform.position, targetPosition) < 0.01f) {
				shouldMove = false;
				if (OnMovementFinished != null) {
					OnMovementFinished (this);
				}
			}
		}
	}
}
