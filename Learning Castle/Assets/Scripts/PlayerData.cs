using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {

	public Gender gender;
	public bool[] isCompleted;
	public bool[] completedTests;

	public enum Gender {Woman, Man};

	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	void login() {
		SceneManager.LoadScene ("Map");
	}
}
