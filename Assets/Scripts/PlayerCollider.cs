using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {
	Rigidbody2D playerRig;
	public float force;
	// Use this for initialization
	void Start () {
		playerRig = transform.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, Vector2.down, 0.5f);
		foreach (RaycastHit2D hit in hits) {
			if (hit.transform.tag == "Step" && playerRig.velocity.y < -7) {
				playerRig.AddForce (Vector2.up * force);
				DestroyWithTag (hit.transform, "Step");
			}
		}

	}
		
	void DestroyWithTag(Transform trans,string tag){
		if (trans.tag == tag) {
			Transform obj = trans;
			while (obj.parent.tag == tag) {
				obj = obj.parent;
			}
			Destroy (obj.gameObject);
		}
	}
}
