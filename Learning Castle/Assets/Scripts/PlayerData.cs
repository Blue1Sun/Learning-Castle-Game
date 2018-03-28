using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {
	
	public int id;
	public int status;
	public string gender;
	public bool[] isCompleted;
	public bool[] completedTests;
	public int[] minigameRecord;

	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	void login() {
		SceneManager.LoadScene ("Map");
	}

	public string toString(){
		string userInformation = "";

		userInformation += this.id+ " " + this.status+ " " + this.gender;

		foreach (bool unit in isCompleted)
			userInformation += unit + " ";
		//userInformation += "\r\n";

		foreach (bool unit in completedTests)
			userInformation += unit + " ";
		//userInformation += "\r\n";

		foreach (int unit in minigameRecord)
			userInformation += unit + " ";
		//userInformation += "\r\n";

		return userInformation;
	}
}
