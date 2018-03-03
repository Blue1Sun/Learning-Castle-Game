using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {

	public Gender gender;
	public bool[] isCompleted;

	public enum Gender {Woman, Man};

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	void login() {
		SceneManager.LoadScene ("Map");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
