using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public GameObject startButton;
	public Text fishMax;
	public int fishNum;

	private static UIManager instance;
	public static UIManager Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		fishNum = 10;
		fishMax.text = "Size:" + 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
