using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NetTop : MonoBehaviour {
	int fishNum;
	public Transform fish;
	// Use this for initialization
	void Start () {
		fishNum = UIManager.Instance.fishNum;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Fish") {									
			if (fishNum > 0) {
				fishNum -= 1;
				UIManager.Instance.fishMax.text = "Size:" + fishNum;
				collider.transform.position = transform.position;

				collider.transform.SetParent (transform);
				if (fishNum == 0) {
					ProgressManager.Instance.GameOver ();
				}
			}
		}
	}
		
}
