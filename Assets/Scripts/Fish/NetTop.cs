using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NetTop : MonoBehaviour {
	//public Transform fish;

	bool isOver;
	// Use this for initialization
	void Start () {
		isOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isOver) {
			if (transform.parent.position.y < UIManager.Instance.diveDepth) {
				ProgressManager.Instance.GameOver ();
				isOver = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Fish"||collider.tag == "unusual") {	
			if (!ProgressManager.Instance.isOvering && !ProgressManager.Instance.isReady) {
				if (collider.tag == "unusual") {
					MultiHaptic.HapticHeavy ();
					Destroy( collider.GetComponent<GhostSprites> ());
				}
				if (ProgressManager.Instance.isRunning) {
					collider.transform.position = new Vector3 (transform.position.x + Random.Range (-0.8f, 0.8f), transform.position.y + 0.2f + Random.Range (-0.5f, 0.4f), 0.5f);
					collider.transform.SetParent (transform);
				} else if (ProgressManager.Instance.isOver) {
					collider.transform.position = new Vector3 (transform.position.x + Random.Range (-0.8f, 0.8f), transform.position.y -0.4f + Random.Range (0.4f, 0.9f), 0.5f);
					collider.transform.SetParent (transform);
				}
			}
		}
	}

		
}
