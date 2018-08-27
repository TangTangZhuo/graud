using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SubmarineController : MonoBehaviour {
	public float moveSpeed;
	Rigidbody2D playerRig;
	[HideInInspector]
	public float gravityScale;
	public Transform netParent;
	public Transform boundL;
	public Transform boundR;
	public Text score;
	public Transform scoreParent;

	float time;
	bool isSettle;
	int fishIndex;
	int goldSum;
	Dictionary<string,int> fishDic = new Dictionary<string, int>();

	private static SubmarineController instance;
	public static SubmarineController Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		time = 0;
		isSettle = false;
		fishIndex = 0;
		playerRig = GetComponent<Rigidbody2D> ();
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			moveSpeed = 1;
		else
			moveSpeed = 10;
		playerRig.gravityScale = 0;
		InitFishDic ();
		goldSum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerRig.gravityScale != gravityScale) {
			playerRig.gravityScale = gravityScale;
		}
		if (ProgressManager.Instance.isRunning) {			
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					transform.Translate (new Vector3 (Input.GetTouch (0).deltaPosition.x * moveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
			} else {

				if (Input.GetKey (KeyCode.A)) {
					transform.Translate (new Vector3 (-moveSpeed * Time.deltaTime, 0, 0), Space.World);			
				} else if (Input.GetKey (KeyCode.D)) {
					transform.Translate (new Vector3 (moveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
			}
		}
		if (isSettle) {
			time += Time.deltaTime;
			if (time > 0.1f) {
				if (fishIndex < netParent.childCount - 1) {
					Transform fish = netParent.GetChild (fishIndex);
					Settlement (fish, 0.3f);
					ScoreGenerate (fish);
				}
				else if(fishIndex == netParent.childCount - 1){
					Transform fish = netParent.GetChild (fishIndex);
					fish.DOScale (4, 0.3f).OnComplete(()=>{
						isSettle = false;
						int gold = PlayerPrefs.GetInt ("gold", 0) + goldSum;
						PlayerPrefs.SetInt ("gold", gold);
						Upgrading.Instance.CheckGold();

						print(goldSum);
						ProgressManager.Instance.GameWin ();
					});
					fish.GetComponent<SpriteRenderer> ().DOFade (0f, 0.3f);
					fish.DOMoveY (netParent.GetChild (fishIndex).position.y+0.3f, 0.3f, false);
					ScoreGenerate (fish);
				}					
				fishIndex++;
				time = 0;
			}
		}

		playerRig.position = new Vector3 (Mathf.Clamp (playerRig.position.x,boundL.position.x,boundR.position.x), playerRig.position.y, 0);
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Pier") {
			if (ProgressManager.Instance.isOver) {
				gravityScale = 0;
				playerRig.velocity = Vector3.zero;
				ProgressManager.Instance.isReady = true;
				ProgressManager.Instance.isOver = false;
				isSettle = true;
			}
		}
		if (collider.tag == "BoundaryL"||collider.tag == "BoundaryR") {
			
		}
	}

	void Settlement(Transform fish,float time){
		fish.DOScale (4, time);
		fish.DOMoveY (fish.position.y + 0.3f, time, false);
		fish.GetComponent<SpriteRenderer> ().DOFade (0f, time);
	}

	void ScoreGenerate(Transform fish){
		Text text = Text.Instantiate (score, netParent.position, score.transform.rotation, scoreParent);
		text.text = fishDic [fish.name].ToString();
		goldSum += fishDic [fish.name];
		text.transform.position = Camera.main.WorldToScreenPoint (fish.position);
		text.DOFade (1f, 0.3f);
		text.transform.DOMoveY (text.transform.position.y+250f, 0.3f, false);
		text.transform.DOScale (1.5f, 0.3f).OnComplete(()=>{Destroy(text.gameObject);});
	}

	void InitFishDic(){
		fishDic.Add ("fish1(Clone)", 100);
		fishDic.Add ("fish2(Clone)", 120);
		fishDic.Add ("fish3(Clone)", 150);
		fishDic.Add ("fish4(Clone)", 180);
		fishDic.Add ("fish5(Clone)", 210);
		fishDic.Add ("fish6(Clone)", 250);
	}
}
