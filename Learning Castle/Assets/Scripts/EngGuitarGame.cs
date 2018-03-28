using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngGuitarGame : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject.Find ("LeftArrSpr").GetComponent<Animator> ().Play ("Flying");
		GameObject.Find ("RightArrSpr").GetComponent<Animator> ().Play ("Flying");
		GameObject.Find ("DownArrSpr").GetComponent<Animator> ().Play ("Flying");
		GameObject.Find ("UpArrSpr").GetComponent<Animator> ().Play ("Flying");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
