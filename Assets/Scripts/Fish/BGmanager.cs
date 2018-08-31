using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGmanager : MonoBehaviour {
	Color[] bgColors;
	int index;

	public GameObject waterF;
	Transform bg;

	private static BGmanager instance;
	public static BGmanager Instance{
		get{return instance;}
	}

	void Awake(){
		instance = this;	
	}

	// Use this for initialization
	void Start () {
		index = 0;
		bg = GameObject.Find ("bg2").transform;
		bgColors = new Color[] {new Color (152 / 255f, 242 / 255f, 255 / 255f), new Color (22 / 255f, 194 / 255f, 219 / 255f),
			new Color (0 / 255f, 95 / 255f, 209 / 255f), new Color (27 / 255f, 50 / 255f, 148 / 255f), new Color (35 / 255f, 51 / 255f, 142 / 255f),
			new Color (63 / 255f, 32 / 255f, 123 / 255f), new Color (71 / 255f, 16 / 255f, 117 / 255f), new Color (85 / 255f, 0 / 255f, 93 / 255f),
			new Color (99 / 255f, 1 / 255f, 45 / 255f), new Color (111 / 255f, 2 / 255f, 2 / 255f)
		};
		Camera.main.backgroundColor = bgColors [index];
	}


	// Update is called once per frame
	void Update () {
		
		//背景颜色渐变
		index = (int)(SubmarineController.Instance.transform.position.y / -17);
		if (index > bgColors.Length - 1) {
			index = bgColors.Length - 1;
		}
		Camera.main.backgroundColor = Color.Lerp (Camera.main.backgroundColor, bgColors [index], 0.5f*Time.deltaTime);


	}

	public void GenetareWaterF(){
		Transform startPos = FishGenerate.Instance.startPosition;
		int Count = UIManager.Instance.diveDepth / -20;
		float screenMid = startPos.position.x;
		float baseGeneratePosy = -21 ;
		for (int i = 0; i < Count; i++) {			
			float offset = Random.Range (-5, 6);
			GameObject.Instantiate (waterF, new Vector3 (screenMid, baseGeneratePosy + offset ,0),waterF.transform.rotation,bg);
			baseGeneratePosy -= 20;
		}
	}

}
