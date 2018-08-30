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
	bool isWin;
	float distance;
	// Use this for initialization
	void Start () {
		isWin = false;
		playerRig = player.GetComponent<Rigidbody2D> ();
		distance = Vector3.Distance (transform.position, player.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (ProgressManager.Instance.isRunning) {
			lookat (player);
			if (transform.position.x - player.position.x > 0.1) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					xspeed *= Input.GetTouch (0).deltaPosition.x;
				}
				transform.Translate (xspeed * Time.deltaTime, 0, 0, Space.World);
			} else if (transform.position.x - player.position.x < -0.1) {
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
					xspeed *= Input.GetTouch (0).deltaPosition.x;
				}
				transform.Translate (-xspeed * Time.deltaTime, 0, 0, Space.World);
			}
			transform.position = new Vector3 (transform.position.x, player.position.y + distance, 0);
		} else if(ProgressManager.Instance.isOver) {
			SubmarineController.Instance.moveSpeed = 0;
			if (Mathf.Abs (transform.localEulerAngles.z) > 5) {
				playerRig.velocity = Vector3.zero;
				SubmarineController.Instance.gravityScale = 0;
				transform.RotateAround (player.position, player.forward, roSpeed);
			}
			else if(Mathf.Abs( transform.localEulerAngles.z) <5) {
				playerRig.velocity = new Vector3 (0, playerRig.velocity.y, 0);
				SubmarineController.Instance.gravityScale = -4;
				transform.position = new Vector3 (transform.position.x, player.position.y - distance, 0);
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
