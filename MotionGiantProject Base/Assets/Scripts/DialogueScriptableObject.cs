using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1)]
public class DialogueScriptableObject : ScriptableObject
{
    string[] room1Dialogue = new string[] { "Another room? When does it end", "I'm going in circles", "Have I been here before?" };
    string[] room2Dialogue = new string[] { "The second room, what a journey", "I'm lost, LOST!", "I remember so little" };

    public int room1DialogueIndex; // Don't initialize here
    public int room2DialogueIndex; // Don't initialize here

    void Awake() // This method is called when the script instance is being loaded
    {
        room1DialogueIndex = Random.Range(0, room1Dialogue.Length); // Initialize here
        room2DialogueIndex = Random.Range(0, room2Dialogue.Length); // Initialize here
    }

    public int getDialogueIndexByScene(string sceneName)
    {
        if (sceneName == "Room1")
        {
            return room1DialogueIndex;
        }
        else if (sceneName == "Room2")
        {
            return room2DialogueIndex;
        }
        return 0;

    }

    public int IncreaseDialogueIndex(string[] dialogue, int dialogueIndex)
    {
        if (dialogueIndex >= dialogue.Length - 1)
        {
            return 0;
        }
        else
        {
            return dialogueIndex + 1;
        }
    }

    public string getDialogueByScene(string sceneName)
    {
        string dialogue = "I haven't been here before";
        if (sceneName == "Room1")
        {
            room1DialogueIndex = IncreaseDialogueIndex(room1Dialogue, room1DialogueIndex);
            return room1Dialogue[room1DialogueIndex];
        }
        else if (sceneName == "Room2")
        {
            room2DialogueIndex = IncreaseDialogueIndex(room2Dialogue, room2DialogueIndex);
            return room2Dialogue[room2DialogueIndex];
        }
        return dialogue;
    }
}

