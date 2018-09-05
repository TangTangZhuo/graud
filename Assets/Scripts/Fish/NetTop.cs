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
			if (!isOver) {
				if (collider.tag == "unusual") {
					collider.GetComponent<GhostSprites> ().alphaFluctuationDivisor = 0;
				}
				collider.transform.position = new Vector3(transform.position.x+Random.Range(-0.4f,0.4f), transform.position.y+Random.Range(-0.4f,0.4f),0.5f);
				collider.transform.SetParent (transform);
			} 				
		}
	}

		
}
