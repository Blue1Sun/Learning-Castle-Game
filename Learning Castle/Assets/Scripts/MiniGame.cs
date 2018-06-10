using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MiniGame {

	static public void GameEnd (GameObject window, int numOfRounds, int score, float badResult){		
		string resultMessage;

		window.SetActive (true);

		if (score <= badResult)
			resultMessage = "Вам стоит попробовать еще раз. \r\n\r\nВы набрали " + score + " очков.";
		else if (score > badResult && score < numOfRounds)
			resultMessage = "Отличная работа! \r\n\r\nВы набрали " + score + " очков.";
		else 
			resultMessage = "Потрясающий результат, Вы не сделали ни одной ошибки! \r\n\r\nВы набрали " + score + " очков.";

		PlayerData playerData = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();

		if(score > playerData.MinigameRecord [Menu.castle - 1]){
			#region SOCKET STUFF
			if(WebSockets.isSocket){
				UserRecord userRecord = new UserRecord(playerData.Id, Menu.castle, score, numOfRounds); 

				WebSocket socket = new WebSocket("ws://127.0.0.1:16000");
				socket.Connect();

				string jsonmessage = JsonUtility.ToJson (userRecord);
				socket.Send (jsonmessage);

				socket.Close();
			}
			#endregion

			playerData.MinigameRecord [Menu.castle - 1] = score;
		}

		GameObject.Find("ResultMessage").GetComponent<Text>().text = resultMessage;
	}

	static public void GameEnd (GameObject window, int numOfRounds, int score, float badResult, string mistakes){	
	
		GameEnd (window, numOfRounds, score, badResult);

		if (score < numOfRounds) 
			GameObject.Find("Mistakes").GetComponent<Text>().text = mistakes;
	}

}
