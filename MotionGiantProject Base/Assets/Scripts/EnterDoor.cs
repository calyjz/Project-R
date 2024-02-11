using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
	public char Door;

	

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			//North = 0, East = 1, South = 2, West = 3
			int doorDirection = 0;

			//TODO: Figure out a way to automatically get the door the player went through
			switch (Door)
            {
				case 'N':
					doorDirection = 0;
					break;
				case 'E':
					doorDirection = 1;
					break;
				case 'S':
					doorDirection = 2;
					break;
				case 'W':
					doorDirection = 3;
					break;
				default:
					Debug.Log("Wrong Direction entered");
					break;
			}	

			GameController.Instance.loadNextRoom(doorDirection);

		}
	}
}