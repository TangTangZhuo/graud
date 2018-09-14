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
	int settleCount;
	float settleTime;
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
		settleCount = 0;
		settleTime = 0;
		time = 0;
		isSettle = false;
		fishIndex = 0;
		playerRig = GetComponent<Rigidbody2D> ();
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			moveSpeed = 0.5f;
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
			if (time > settleTime) {
				if (fishIndex < settleCount) {
					Transform fish = netParent.GetChild (fishIndex);
					Settlement (fish, 0.3f);
					if (PlayerPrefs.GetInt (fish.name.Split (new char[]{ '(' }) [0], 0)==0) {
						PlayerPrefs.SetInt ("illNew", 1);
					}
					PlayerPrefs.SetInt (fish.name.Split (new char[]{'('}) [0], 1);
					ScoreGenerate (fish);

				}
				else if(fishIndex == settleCount){
					Transform fish = netParent.GetChild (fishIndex);
					fish.DOScale (1, 0.3f).OnComplete(()=>{
						fish.GetComponent<SpriteRenderer> ().DOFade (0f, 0.3f);
						if(fish.childCount>0){
							Destroy(fish.GetChild(0).gameObject);
						}
						isSettle = false;
						MessageBox.Show("SALE REWARD","$"+goldSum);
						MessageBox.confim =()=>{
							int gold = PlayerPrefs.GetInt ("gold", 0) + goldSum;
							PlayerPrefs.SetInt ("gold", gold);
							Upgrading.Instance.CheckGold();
							UpgradingOffline.Instance.CheckGold();
							ProgressManager.Instance.GameWin ();
						};
						MessageBox.doubleR =()=>{
							int gold = PlayerPrefs.GetInt ("gold", 0) + goldSum*2;
							PlayerPrefs.SetInt ("gold", gold);
							Upgrading.Instance.CheckGold();
							UpgradingOffline.Instance.CheckGold();
							ProgressManager.Instance.GameWin ();
						};							
							
					});						

					if (PlayerPrefs.GetInt (fish.name.Split (new char[]{ '(' }) [0], 0)==0) {
						PlayerPrefs.SetInt ("illNew", 1);
					}
					PlayerPrefs.SetInt (fish.name.Split (new char[]{'('}) [0], 1);
					ScoreGenerate (fish);

				}	
				MultiHaptic.HapticLight ();
				fishIndex++;
				time = 0;
			}
		}
	}

	float GetSettleTime(int count){
		if (count < 10) {
			return 0.1f;
		} else if (count >= 10 && count < 20) {
			return 0.08f;
		} else if (count >= 20 && count < 30) {
			return 0.06f;
		} else if (count >= 30 && count < 40) {
			return 0.05f;
		} else if (count >= 40 && count < 50) {
			return 0.04f;
		} else {
			return 0.02f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Pier") {
			if (ProgressManager.Instance.isOver) {
				gravityScale = 0;
				playerRig.velocity = Vector3.zero;
				ProgressManager.Instance.isReady = true;
				ProgressManager.Instance.isOver = false;
				settleCount = netParent.childCount - 1;

				if (PlayerPrefs.GetInt ("Level", 1) == 1) {
					if (settleCount > PlayerPrefs.GetInt ("maxFish-1", 0)) {
						PlayerPrefs.SetInt ("maxFish-1", settleCount);
					}
					PlayerPrefs.SetInt ("bestFish-1", PlayerPrefs.GetInt ("bestFish-1", 0) + settleCount);
				}else if (PlayerPrefs.GetInt ("Level", 1) == 2) {
					if (settleCount > PlayerPrefs.GetInt ("maxFish-2", 0)) {
						PlayerPrefs.SetInt ("maxFish-2", settleCount);
					}
					PlayerPrefs.SetInt ("bestFish-2", PlayerPrefs.GetInt ("bestFish-2", 0) + settleCount);
				}else if (PlayerPrefs.GetInt ("Level", 1) == 3) {
					if (settleCount > PlayerPrefs.GetInt ("maxFish-3", 0)) {
						PlayerPrefs.SetInt ("maxFish-3", settleCount);
					}
					PlayerPrefs.SetInt ("bestFish-3", PlayerPrefs.GetInt ("bestFish-3", 0) + settleCount);
				}

				settleTime = GetSettleTime (settleCount);
				isSettle = true;
				progressSlider.gameObject.SetActive (false);
			}
		}

		if (collider.tag == "BoundaryL") {
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					if (Input.GetTouch (0).deltaPosition.x < 0) {
						moveSpeed = 0;
					} else {
						moveSpeed = 0.5f;
					}
				} 
			}
			else {
				if (Input.GetKey (KeyCode.A)) {
					moveSpeed = 0;
				} else {
					moveSpeed = 10;
				}
			}
		}
		if (collider.tag == "BoundaryR") {
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					if (Input.GetTouch (0).deltaPosition.x > 0) {
						moveSpeed = 0;
					} else {
						moveSpeed = 0.5f;
					}
				} 
			} else {
				if (Input.GetKey (KeyCode.D)) {
					moveSpeed = 0;
				} else {
					moveSpeed = 10;
				}
			}
		}
		
	}

	void OnTriggerStay2D(Collider2D collider){
		if (collider.tag == "BoundaryL") {
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					if (Input.GetTouch (0).deltaPosition.x < 0) {
						moveSpeed = 0;
					} else {
						moveSpeed = 0.5f;
					}
				} 
			}
			else {
				if (Input.GetKey (KeyCode.A)) {
					moveSpeed = 0;
				} else {
					moveSpeed = 10;
				}
			}
		}
		if (collider.tag == "BoundaryR") {
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					if (Input.GetTouch (0).deltaPosition.x > 0) {
						moveSpeed = 0;
					} else {
						moveSpeed = 0.5f;
					}
				} 
			} else {
				if (Input.GetKey (KeyCode.D)) {
					moveSpeed = 0;
				} else {
					moveSpeed = 10;
				}
			}
		}
		transform.position = new Vector2 (Mathf.Clamp (transform.position.x, boundL.position.x, boundR.position.x), transform.position.y);
	}

	void Settlement(Transform fish,float time){
		fish.DOScale (1, time).OnComplete(()=>{
			fish.GetComponent<SpriteRenderer> ().DOFade (0f, time);
			if(fish.childCount>0){
				Destroy(fish.GetChild(0).gameObject);
			}
		});
	}

	void ScoreGenerate(Transform fish){
		Text text = Text.Instantiate (score, netParent.position, score.transform.rotation, scoreParent);
		text.text = (fishDic [fish.name]/2).ToString();
		goldSum += (fishDic [fish.name]/2);
		text.transform.position = Camera.main.WorldToScreenPoint (fish.position);
		text.DOFade (1f, 0.3f);
		text.transform.DOMoveY (text.transform.position.y+250f, 0.3f, false);
		text.transform.DOScale (1.5f, 0.3f).OnComplete(()=>{Destroy(text.gameObject);});
	}

	void InitFishDic(){
		fishDic.Add ("fish1(Clone)", 50);
		fishDic.Add ("fish2(Clone)", 75);
		fishDic.Add ("fish3(Clone)", 125);
		fishDic.Add ("fish4(Clone)", 200);
		fishDic.Add ("fish5(Clone)", 300);
		fishDic.Add ("fish6(Clone)", 450);
		fishDic.Add ("fish7(Clone)", 700);
		fishDic.Add ("fish8(Clone)", 1000);
		fishDic.Add ("fish9(Clone)", 1600);
		fishDic.Add ("fish10(Clone)", 2500);
		fishDic.Add ("fish11(Clone)", 1000);
		fishDic.Add ("fish12(Clone)", 2000);
		fishDic.Add ("fish13(Clone)", 3000);
		fishDic.Add ("fish14(Clone)", 4000);
		fishDic.Add ("fish15(Clone)", 6000);
		fishDic.Add ("fish16(Clone)", 9000);
		fishDic.Add ("fish17(Clone)", 13500);
		fishDic.Add ("fish18(Clone)", 20250);
		fishDic.Add ("fish19(Clone)", 30375);
		fishDic.Add ("fish20(Clone)", 45560);
		fishDic.Add ("fish21(Clone)", 20340);
		fishDic.Add ("fish22(Clone)", 30510);
		fishDic.Add ("fish23(Clone)", 45765);
		fishDic.Add ("fish24(Clone)", 65100);
		fishDic.Add ("fish25(Clone)", 93200);
		fishDic.Add ("fish26(Clone)", 125820);
		fishDic.Add ("fish27(Clone)", 169850);
		fishDic.Add ("fish28(Clone)", 229306);
		fishDic.Add ("fish29(Clone)", 309564);
		fishDic.Add ("fish30(Clone)", 417911);

		AddUnusual (30);
	}

	void AddUnusual(int number){
		for (int i = 1; i <= number; i++) {
			fishDic.Add ("unusual"+i+"(Clone)", fishDic["fish"+i+"(Clone)"]*2);
		}
	}

	public void InitProgressSlider(){
		progressSlider.minValue = UIManager.Instance.diveDepth;
		progressSlider.maxValue = 0;
	}
		
	public void SynDepth(){
		progressSlider.transform.Find ("depth").GetComponent<Text> ().text = UIManager.Instance.diveDepth+"M";
	}
}
