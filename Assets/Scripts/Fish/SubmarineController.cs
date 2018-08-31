using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

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
	public Slider progressSlider;
	public GameObject settleView;
	public float force;

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
		InitProgressSlider ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerRig.gravityScale != gravityScale) {
			playerRig.gravityScale = gravityScale;
		}
		if (ProgressManager.Instance.isRunning) {			
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					if (Input.GetTouch (0).deltaPosition.x > 0) {
						transform.rotation = Quaternion.Euler (0, 0, 0);
					}
					if (Input.GetTouch (0).deltaPosition.x < 0) {
						transform.rotation = Quaternion.Euler (0, 180, 0);
					}
					transform.Translate (new Vector3 (Input.GetTouch (0).deltaPosition.x * moveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
			} else {

				if (Input.GetKey (KeyCode.A)) {
					transform.rotation = Quaternion.Euler (0, 180, 0);
					transform.Translate (new Vector3 (-moveSpeed * Time.deltaTime, 0, 0), Space.World);			
				} else if (Input.GetKey (KeyCode.D)) {
					transform.rotation = Quaternion.Euler (0, 0, 0);
					transform.Translate (new Vector3 (moveSpeed * Time.deltaTime, 0, 0), Space.World);
				}
			}
			progressSlider.value = transform.position.y;
		}
		if (isSettle) {
			time += Time.deltaTime;
			if (time > 0.1f) {
				if (fishIndex < netParent.childCount - 1) {
					Transform fish = netParent.GetChild (fishIndex);
					Settlement (fish, 0.3f);
					if (PlayerPrefs.GetInt (fish.name.Split (new char[]{ '(' }) [0], 0)==0) {
						Illustration.Instance.illNew.SetActive (true);
					}
					PlayerPrefs.SetInt (fish.name.Split (new char[]{'('}) [0], 1);
					ScoreGenerate (fish);
				}
				else if(fishIndex == netParent.childCount - 1){
					Transform fish = netParent.GetChild (fishIndex);
					fish.DOScale (4, 0.3f).OnComplete(()=>{
						isSettle = false;
						MessageBox.Show("SALE REWARD","$"+goldSum);
						MessageBox.confim=()=>{
							int gold = PlayerPrefs.GetInt ("gold", 0) + goldSum;
							PlayerPrefs.SetInt ("gold", gold);
							Upgrading.Instance.CheckGold();
							UpgradingOffline.Instance.CheckGold();
							ProgressManager.Instance.GameWin ();
							//settleView.SetActive(true);
							//settleView.transform.GetComponentInChildren<Text>().text = "$" + goldSum;
						};
							
					});
					fish.GetComponent<SpriteRenderer> ().DOFade (0f, 0.3f);
					fish.DOMoveY (netParent.GetChild (fishIndex).position.y+0.3f, 0.3f, false);
					if (PlayerPrefs.GetInt (fish.name.Split (new char[]{ '(' }) [0], 0)==0) {
						Illustration.Instance.illNew.SetActive (true);
					}
					PlayerPrefs.SetInt (fish.name.Split (new char[]{'('}) [0], 1);
					ScoreGenerate (fish);
				}					
				fishIndex++;
				time = 0;
			}
		}

		//playerRig.position = new Vector3 (Mathf.Clamp (playerRig.position.x,boundL.position.x,boundR.position.x), playerRig.position.y, 0);
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
		if (collider.tag == "BoundaryL") {
			moveSpeed = 0;
			playerRig.AddForce (Vector2.right * force);
			Invoke ("ReMoveSpeed", 0.5f);
		}
		if (collider.tag == "BoundaryR") {
			moveSpeed = 0;
			playerRig.AddForce (Vector2.left * force);
			Invoke ("ReMoveSpeed", 0.5f);
		}
	}
		
	void ReMoveSpeed(){
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			moveSpeed = 1;
		else
			moveSpeed = 10;
	}
//	public void OnPier(){
//		if (ProgressManager.Instance.isOver) {
//			gravityScale = 0;
//			playerRig.velocity = Vector3.zero;
//			ProgressManager.Instance.isReady = true;
//			ProgressManager.Instance.isOver = false;
//			isSettle = true;
//		}
//	}

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
		fishDic.Add ("fish2(Clone)", 125);
		fishDic.Add ("fish3(Clone)", 150);
		fishDic.Add ("fish4(Clone)", 175);
		fishDic.Add ("fish5(Clone)", 200);
		fishDic.Add ("fish6(Clone)", 225);
		fishDic.Add ("fish7(Clone)", 250);
		fishDic.Add ("fish8(Clone)", 275);

		fishDic.Add ("unusual1(Clone)", fishDic["fish1(Clone)"]*2);
		fishDic.Add ("unusual2(Clone)", fishDic["fish2(Clone)"]*2);
		fishDic.Add ("unusual3(Clone)", fishDic["fish3(Clone)"]*2);
		fishDic.Add ("unusual4(Clone)", fishDic["fish4(Clone)"]*2);
		fishDic.Add ("unusual5(Clone)", fishDic["fish5(Clone)"]*2);
		fishDic.Add ("unusual6(Clone)", fishDic["fish6(Clone)"]*2);
		fishDic.Add ("unusual7(Clone)", fishDic["fish7(Clone)"]*2);
		fishDic.Add ("unusual8(Clone)", fishDic["fish8(Clone)"]*2);
	}

	public void InitProgressSlider(){
		progressSlider.minValue = UIManager.Instance.diveDepth;
		progressSlider.maxValue = 0;
	}
}
