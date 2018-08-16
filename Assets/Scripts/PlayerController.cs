using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	Rigidbody2D playerRig;
	public float moveSpeed;
	bool isStart;
	public bool isMove;
	private float addSpeed;

	private static PlayerController instance;
	public static PlayerController Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		playerRig = GetComponent<Rigidbody2D> ();
		isStart = false;
		isMove = true;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			moveSpeed = 30f;
		} else {
			moveSpeed = 250;
		}
		addSpeed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//print (playerRig.velocity.y);
		if (playerRig.velocity.y < -6) {
			addSpeed = moveSpeed * playerRig.velocity.y / -6f - moveSpeed;
		} else {
			addSpeed = 0;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			if (isMove) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					playerRig.AddForce (Vector2.right * Input.GetTouch (0).deltaPosition.x * (moveSpeed+addSpeed));
					//transform.Translate (0, -touch.deltaPosition.x * moveSpeed, 0);
					if (!isStart) {
						DropGenerate.Instance.isGenerate = true;
						isStart = true;
					}
				} else {
					playerRig.velocity = new Vector2 (playerRig.velocity.x/1.1f, playerRig.velocity.y);
				}
			}
		} else {
			if (isMove) {
				if (Input.GetKey (KeyCode.A)) {
					//playerRig.velocity = Vector2.left * moveSpeed;
					//transform.Translate (0, moveSpeed*Time.deltaTime, 0);
					//playerRig.MovePosition (playerRig.position + Vector2.left* moveSpeed * Time.fixedDeltaTime);
					playerRig.AddForce (Vector2.left * (moveSpeed+addSpeed));
					if (!isStart) {
						DropGenerate.Instance.isGenerate = true;
						isStart = true;
					}
				} else if (Input.GetKey (KeyCode.D)) {
					//playerRig.velocity = Vector2.right * moveSpeed;
					//transform.Translate (0, -moveSpeed * Time.deltaTime, 0);
					//playerRig.MovePosition (playerRig.position + Vector2.right* moveSpeed * Time.fixedDeltaTime);
					playerRig.AddForce (Vector2.right * (moveSpeed+addSpeed));
					if (!isStart) {
						DropGenerate.Instance.isGenerate = true;
						isStart = true;
					}
				} else {
					playerRig.velocity = new Vector2 (playerRig.velocity.x/1.1f, playerRig.velocity.y);
				}
			}
		}
	}

	void LateUpdate(){

	}
}
