using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody2D playerRig;
	public float moveSpeed=1;
	bool isStart;
	public bool isMove;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (isMove) {
			if (Input.GetKey (KeyCode.A)) {
				//playerRig.velocity = Vector2.left * moveSpeed;
				//transform.Translate (0, moveSpeed * Time.deltaTime, 0);
				//playerRig.MovePosition (new Vector2 (transform.position.x - moveSpeed * Time.deltaTime, transform.position.y));
				playerRig.AddForce (Vector2.left * moveSpeed);
				if (!isStart) {
					DropGenerate.Instance.isGenerate = true;
					isStart = true;
				}
			}

			if (Input.GetKey (KeyCode.D)) {
				//playerRig.velocity = Vector2.right * moveSpeed;
				//transform.Translate (0, -moveSpeed * Time.deltaTime, 0);
				//playerRig.MovePosition (new Vector2 (transform.position.x + moveSpeed * Time.deltaTime, transform.position.y));
				playerRig.AddForce (Vector2.right * moveSpeed);
				if (!isStart) {
					DropGenerate.Instance.isGenerate = true;
					isStart = true;
				}
			}
		}
	}

	void LateUpdate(){

	}
}
