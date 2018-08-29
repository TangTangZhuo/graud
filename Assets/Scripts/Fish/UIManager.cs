using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public GameObject startButton;
	public GameObject upgrading;
	public int diveDepth;
	public Text goldT;
	public int offlineGold;

	private static UIManager instance;
	public static UIManager Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
