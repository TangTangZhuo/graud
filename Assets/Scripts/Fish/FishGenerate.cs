﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGenerate : MonoBehaviour {
	private int diveDepth;
	public Transform startPosition;
	public Transform[] fish;

	int typeDistance;
	int fishIndex;
	int[] euler;

	private static FishGenerate instance;
	public static FishGenerate Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		fishIndex = 0;
		typeDistance = 10;
		euler = new int[]{ 180, -180 };
		diveDepth = UIManager.Instance.diveDepth;
		//GenerateFish ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateFish(){
		float screenMid = startPosition.position.x;
		int counter = 0;
		for (int i = (int)startPosition.position.y-2; i > diveDepth; i+=0) {
			float baseGeneratePosy = i - typeDistance / 2 ;
			for (int num = 0; num < 5; num++) {
				print (fishIndex);
				Transform.Instantiate (fish[fishIndex], new Vector3 (screenMid + Random.Range (-2, 3), baseGeneratePosy + Random.Range (-5, 6)), 
					Quaternion.Euler (0, euler [Random.Range (0, 2)], 0), transform);
			}
			i -= typeDistance;
			counter++;
			if (counter == 2) {
				fishIndex++;
			}
		}
	}
}
