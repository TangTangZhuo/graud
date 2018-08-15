using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour {
	private static ProgressManager instance;
	public static ProgressManager Instance{
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

	public void GameWin(){
		CameraController.Instance.isMove = false;
		PlayerController.Instance.isMove = false;
		Invoke ("LoadScene", 1.5f);
	}

	void LoadScene(){
		SceneManager.LoadScene ("Main");
	}
}
