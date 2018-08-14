using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepGenerate : MonoBehaviour {
	public Transform[] steps;
	public Transform preStep;
	int currentLevel;
	float leftScreen;

	int[] stepsNumber;

	// Use this for initialization
	void Start () {
		leftScreen = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0)).x;
		int abc = GetStepNumber (currentLevel) [0];
		int de = GetStepNumber (currentLevel) [1];
		stepsNumber = new int[]{abc, abc, abc, de, de};
		GenerateStep (1);
	}
	
	// Update is called once per frame
	void Update () {
		if (stepsNumber [0] != 0 && stepsNumber [1] != 0 && stepsNumber [2] != 0 && stepsNumber [3] != 0 && stepsNumber [4] != 0) {
			
		}
	}

	//返回各类障碍数量，A=B=C,D=E
	int[] GetStepNumber(int level){
		int stepSum = level / 5 + 25;
		if (level < 10 && level > 0) {
			return new int[]{ (int)(0.3f * stepSum), (int)(0.05f * stepSum) };
		} else if (level < 20) {
			return new int[]{ (int)(0.27f * stepSum), (int)(0.1f * stepSum) };
		}else if (level < 30) {
			return new int[]{ (int)(0.23f * stepSum), (int)(0.15f * stepSum) };
		}else if (level < 40) {
			return new int[]{ (int)(0.2f * stepSum), (int)(0.2f * stepSum) };
		}else if (level < 50) {
			return new int[]{ (int)(0.17f * stepSum), (int)(0.25f * stepSum) };
		}else  {
			return new int[]{ (int)(0.13f * stepSum), (int)(0.3f * stepSum) };
		}
	} 

	void GenerateStep(int num){
		Transform step = GameObject.Instantiate (steps [num],transform);
		step.position = new Vector3 (leftScreen+Random.value*3.6f, preStep.position.y - 3.4f);

	}
}
