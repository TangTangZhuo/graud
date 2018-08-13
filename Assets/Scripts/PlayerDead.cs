using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour {
	Material material;
	// Use this for initialization
	void Start () {
		material = GetComponent<SpriteRenderer> ().material;
		Color color = new Color (0, 0, 0);
		material.SetColor ("_icolor", color);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GameOver(){
		StartCoroutine(Dissolve());

	}

	IEnumerator Dissolve(){
		float value = 0.251f;
		for (float time = 0; time < 1; time += Time.deltaTime) {
			value = Mathf.Lerp (value, 0.8f, time);
			material.SetFloat ("_value", value);
			if (Mathf.Abs (value - 0.8f) < 0.0001f) {
				Time.timeScale = 0;
				continue;
			}
			yield return 0;
		}
	}
}
