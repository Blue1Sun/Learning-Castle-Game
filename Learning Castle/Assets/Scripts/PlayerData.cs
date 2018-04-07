using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour {

	[SerializeField]
	private int id;
	[SerializeField]
	private int status;
	[SerializeField]
	private string gender;
	[SerializeField]
	private bool[] isCompleted;
	[SerializeField]
	private bool[] completedTests;
	[SerializeField]
	private int[] minigameRecord;

	public int Id {
		get	{ return id;	}
		set	{ id = value; }
	}

	public int Status {
		get	{ return status;	}
		set	{ status = value; }
	}

	public string Gender {
		get	{ return gender;	}
		set	{ gender = value; }
	}

	public bool[] IsCompleted {
		get	{ return isCompleted;	}
		set	{ isCompleted = value; }
	}

	public bool[] CompletedTests {
		get	{ return completedTests;	}
		set	{ completedTests = value; }
	}

	public int[] MinigameRecord {
		get	{ return minigameRecord;	}
		set	{ minigameRecord = value; }
	}

	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	public string toString(){
		string userInformation = "";

		userInformation += this.id+ " " + this.status+ " " + this.gender;

		foreach (bool unit in isCompleted)
			userInformation += unit + " ";
		userInformation += ";";

		foreach (bool unit in completedTests)
			userInformation += unit + " ";
		userInformation += ";";

		foreach (int unit in minigameRecord)
			userInformation += unit + " ";
		userInformation += ";";

		return userInformation;
	}
}
