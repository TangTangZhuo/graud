using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour {
	public Slider progressSlider;
	public Slider sliderBad;
	public bool isReady;
	public bool isRunning;
	public bool isOver;

	private static ProgressManager instance;
	public static ProgressManager Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		isReady = true;
		isRunning = false;
		isOver = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void GameWin(){
		//CameraController.Instance.isMove = false;
		//PlayerController.Instance.isMove = false;
		//Invoke ("LoadScene", 0f);
		SceneManager.LoadScene ("Main");
	}

	public void GameOver(){
		isOver = true;
		isRunning = false;
	}

	void LoadScene(){
		SceneManager.LoadScene ("Main");
	}

	public void onStartButton(){
		isRunning = true;
		isReady = false;
		UIManager.Instance.startButton.SetActive (false);
		UIManager.Instance.upgrading.SetActive (false);
		SubmarineController.Instance.gravityScale = 2;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			SubmarineController.Instance.moveSpeed = 1;
		else
			SubmarineController.Instance.moveSpeed = 10;
		FishGenerate.Instance.GenerateFish ();
	}


}
