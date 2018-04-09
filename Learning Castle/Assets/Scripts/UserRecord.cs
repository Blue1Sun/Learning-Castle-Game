using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRecord {

	public int code = 4;
	public int student;
	public int topic;
	public int score;
	public int maxscore;

	public UserRecord (int student, int topic, int score, int maxscore){
		this.student = student;
		this.topic = topic;
		this.score = score;
		this.maxscore = maxscore;
	}
}
