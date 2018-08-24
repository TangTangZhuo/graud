using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SubmarineController : MonoBehaviour {
	public float moveSpeed;
	Rigidbody2D playerRig;
	[HideInInspector]
	public float gravityScale;
	public Transform netParent;
	public Transform boundL;
	public Transform boundR;

	float time;
	bool isSettle;
	int fishIndex;

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
				if (fishIndex < netParent.childCount - 1)
					Settlement (netParent.GetChild (fishIndex),0.3f);
				else if(fishIndex == netParent.childCount - 1){
					netParent.GetChild (fishIndex).DOScale (4, 0.3f).OnComplete(()=>{
						isSettle = false;
						ProgressManager.Instance.GameWin ();
					});
					netParent.GetChild (fishIndex).GetComponent<SpriteRenderer> ().DOFade (0f, 0.3f);
					netParent.GetChild (fishIndex).DOMoveY (netParent.GetChild (fishIndex).position.y+0.3f, 0.3f, false);
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

}
