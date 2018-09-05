using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour {
	float time;

	bool stopShake;
	// Use this for initialization
	void Start () {
		time = 2;
		stopShake = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ProgressManager.Instance.isReady) {
			time += Time.deltaTime;
			if (time > 2) {
				transform.DOShakePosition (10, 0.2f, 1, 90, false, false).timeScale=0.3f;
				time = 0;
			}

		}
		if (ProgressManager.Instance.isRunning) {
			if (!stopShake) {
				transform.DOKill (false);
				stopShake = true;
			}
		}
	}	
}
