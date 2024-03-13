using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionInScene : MonoBehaviour
{
    public GameObject player; // Player instance
    public GameObject currentRoom; // Current room instance
    public GameObject nextRoom; // Next room instance
    public Vector2 nextRoomSpawnPoint; // Spawn point in the next room

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") // Check if the collided object is the player
        {
            currentRoom.SetActive(false); // Set current room to inactive
            nextRoom.SetActive(true); // Set next room to active

            // Change player position to the spawn point of the next room
            player.transform.position = new Vector3(nextRoomSpawnPoint.x, nextRoomSpawnPoint.y, -2);
        }
    }
}
