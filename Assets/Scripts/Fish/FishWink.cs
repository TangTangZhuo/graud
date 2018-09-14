using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWink : MonoBehaviour {
	
	public RuntimeAnimatorController[] windMid;
	public RuntimeAnimatorController[] windLarge;
	public RuntimeAnimatorController[] windSmall;

	public static FishWink Instance{
		get{return instance;}
	}
	private static FishWink instance;

	void Awake(){
		instance = this;
	}
}
