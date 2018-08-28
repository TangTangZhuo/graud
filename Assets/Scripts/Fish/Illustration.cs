using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Illustration : MonoBehaviour {
	public Transform illustration;
	public Transform fishItem;
	public Transform scrollContent;

	int screenHeight;
	bool isShow;
	int fishNumber;

	public Sprite[] fishImage;
	public string[] fishName;
	public Sprite[] unusualImage;
	public Sprite[] unLockImage;

	private static Illustration instance;
	public static Illustration Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		isShow = false;
		screenHeight = Screen.height;
		fishNumber = FishGenerate.Instance.fish.Length;

		IllUpdate ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void IllUpdate(){
		for (int i =0; i < fishNumber; i++) {
			
			Transform item = Transform.Instantiate (fishItem, scrollContent);
			if (PlayerPrefs.GetInt ("fish" + (i + 1), 0) == 1) {
				item.Find ("name").GetComponent<Text> ().text = fishName [i];
				item.Find ("image").GetComponent<Image> ().sprite = fishImage [i];
			} else {
				item.Find ("name").GetComponent<Text> ().text = "???";
				item.Find ("image").GetComponent<Image> ().sprite = unLockImage [i];

			}
			if (PlayerPrefs.GetInt ("unusual" + (i + 1), 0) == 1) {
				item.Find ("unusual").GetComponent<Image> ().sprite = unusualImage [i];
			} else {
				item.Find ("unusual").GetComponent<Image> ().enabled = false;
			}
		}
	}

	public void OnIllBtnClick(){
		if (!isShow) {
			illustration.DOMoveY (illustration.position.y - screenHeight, 0.5f, false);
			isShow = true;
		}
	}

	public void OnBackBtnClick(){
		if (isShow) {
			illustration.DOMoveY (illustration.position.y + screenHeight, 0.5f, false);
			isShow = false;
		}
	}
		
}
