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
}
