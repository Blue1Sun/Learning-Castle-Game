using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WebSockets : MonoBehaviour {

	[SerializeField]
	private bool isSocket = false;

	private WebSocket socket;    
    //User user = new User();

    /*public class User
    {
        public int status;
        public int id;

        public void Clear()
        {
           
        }
    }*/

    private class Message
    {
		private int code = 1;
		private string login;
		private string password;

		public string Login {
			get	{ return login;	}
			set	{ login = value; }
		}

		public string Password {
			get	{ return password;	}
			set	{ password = value; }
		}
    }

    private void Update()
    {
		if (Input.GetKeyDown (KeyCode.Return))
			GameObject.Find ("Enter").GetComponent<Button> ().onClick.Invoke();
    }

    public void auth()
    {
		if (isSocket) {
			socket = new WebSocket ("ws://127.0.0.1:16000");
			socket.Connect ();

			string login = GameObject.Find ("Login").GetComponent<InputField> ().text;
			string password = GameObject.Find ("Password").GetComponent<InputField> ().text;

			Message message = new Message ();
			message.Login = login;
			message.Password = password;

			string jsonmessage = JsonUtility.ToJson (message);
			socket.Send (jsonmessage);

			PlayerData user = GameObject.Find ("PlayerInfo").GetComponent<PlayerData> ();

			socket.OnMessage += (sender, e) => {
				Debug.Log (e.Data);

				JsonUtility.FromJsonOverwrite (e.Data, user);
			};

			System.Threading.Thread.Sleep (500);
			if (user.Status == 1) {
				Debug.Log ("OK");

				Debug.Log (user.toString ());
				Debug.Log (user.Gender);

				SceneManager.LoadScene ("Subjects");
			} else {
				Debug.Log ("NOT OK");
				socket.Close ();
				//Debug.Log(user.status);
				//Debug.Log(user.id);
			}
		} 
		else {
			SceneManager.LoadScene ("Subjects");
		}
    }

}
