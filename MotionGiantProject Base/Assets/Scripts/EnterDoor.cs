using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
	public int nextRoom;

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			Debug.Log("Room" + nextRoom.ToString());
			SceneManager.LoadScene("Room" + nextRoom.ToString());

		}
	}
}