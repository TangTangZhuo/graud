using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NetController : MonoBehaviour {
	public Transform player;
	public float xspeed;
	public float yspeed;
	public float roSpeed;

	Rigidbody2D playerRig;
	bool isOver;
	float distance;
	float rotate;
	// Use this for initialization
	void Start () {
		rotate = 0;
		isOver = false;
		playerRig = player.GetComponent<Rigidbody2D> ();
		distance = Vector3.Distance (transform.position, player.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (ProgressManager.Instance.isRunning) {
			lookat (player);
			if (Mathf.Abs( transform.position.x - player.position.x) > 0.1) {
				transform.DOMoveX (player.position.x, 0.3f, false);
			}
			transform.position = new Vector3 (transform.position.x, player.position.y + distance, 0);
		} else if(ProgressManager.Instance.isOver) {
			SubmarineController.Instance.moveSpeed = 0;
//			transform.DOPause();
//			SubmarineController.Instance.gravityScale = 0;
//			if (playerRig.velocity != Vector2.zero) {
//				transform.DOMove (new Vector3 (transform.position.x, player.position.y - distance, 0), 1f, false).OnComplete(()=>{
//					transform.position = new Vector3 (transform.position.x, player.position.y + distance, 0);
//					player.DOMove(GameObject.FindWithTag("Pier").transform.position,2f,false).OnComplete(()=>{
//						SubmarineController.Instance.OnPier();
//					});
//				});
//			}
			if (!isOver) {
//				transform.DOMoveX (player.position.x, 0.3f, false).OnComplete (() => {
//					isOver = true;
//				});
//				SubmarineController.Instance.gravityScale = 0;
				transform.DOPause();
				isOver = true;
			}
			if (isOver) {
				
//				if (Mathf.Abs (transform.localEulerAngles.z) > 10) {
//					playerRig.velocity = Vector3.zero;
//					SubmarineController.Instance.gravityScale = 0;
//					//transform.position = new Vector3 (player.position.x, transform.position.y, transform.position.z);
//					transform.RotateAround (player.position, player.forward, roSpeed);
//					//transform.DORotate (new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 1f, RotateMode.Fast);
//				} else if (Mathf.Abs (transform.localEulerAngles.z) <= 10) {
//					transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
//					//	playerRig.velocity = new Vector3 (0, playerRig.velocity.y, 0);
//					SubmarineController.Instance.gravityScale = player.position.y / 5;
//					transform.position = new Vector3 (player.position.x, player.position.y - distance,transform.position.z );
//				}

//				print (Mathf.Abs( transform.localEulerAngles.x));
//				transform.RotateAround (player.position, -transform.right, roSpeed);
//
//				playerRig.velocity = Vector3.zero;
//				SubmarineController.Instance.gravityScale = 0;
//
				if (rotate <= 180) {
					playerRig.velocity = Vector3.zero;
					SubmarineController.Instance.gravityScale = 0;
					//transform.position = new Vector3 (player.position.x, transform.position.y, transform.position.z);
					transform.RotateAround (player.position, -player.right, roSpeed);
					rotate += roSpeed;
					//transform.DORotate (new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 1f, RotateMode.Fast);
				} else  {
					transform.localEulerAngles = new Vector3 (165, 0,  -180);
					//	playerRig.velocity = new Vector3 (0, playerRig.velocity.y, 0);
					SubmarineController.Instance.gravityScale = player.position.y / 5;
					transform.position = new Vector3 (player.position.x, player.position.y - distance,transform.position.z );
				}
			}
		}else if (ProgressManager.Instance.isReady) {
//			UIManager.Instance.startButton.SetActive (true);
//			if (Mathf.Abs (transform.localEulerAngles.z) > 175&&Mathf.Abs (transform.localEulerAngles.z) < 185) {
//				playerRig.velocity = Vector3.zero;
//				playerRig.gravityScale = 0;
//				transform.RotateAround (player.position, player.forward, roSpeed);
//			}
//			ProgressManager.Instance.GameWin();
		}
	}
		

	void lookat(Transform trans){
		// 世界坐标系转到本地坐标系
		Vector3 relative = transform.InverseTransformPoint(trans.position);
		// 归一化（并不是必须的）
		relative.Normalize();

		// 反正切求弧并转成角度
		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
		angle = Mathf.Abs(angle);

		// 左右2侧的垂直向量
		Vector3 leftVertical = new Vector3(-1, 0, 0);
		Vector3 rightVertical = new Vector3(1, 0, 0);
		// 跟目标向量做点积
		float leftDot = Vector3.Dot(relative, leftVertical);
		float rightDot = Vector3.Dot(relative, rightVertical);

		// >= 0 说明夹角 <= 90
		if (leftDot >= 0)
		{
			transform.Rotate(0, 0, angle);
		}
		else if (rightDot >= 0)
		{
			transform.Rotate(0, 0, -angle);
		}
		else
		{
			Debug.LogError("dot有问题");
		}


	}
}
