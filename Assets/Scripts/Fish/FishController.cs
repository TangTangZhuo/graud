using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {
	bool isFished;
	Transform Net;
	int fishNum;
	// Use this for initialization
	void Start () {
		isFished = false;
		fishNum = UIManager.Instance.fishNum;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFished) {			
			transform.position = Net.position;
		}

	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Net") {			
			fishNum -= 1;
			UIManager.Instance.fishMax.text = "Size:" + fishNum;
			if (fishNum == 0) {

				ProgressManager.Instance.GameOver ();
			}
			Net = collider.transform;
			isFished = true;
		}
	}

}
