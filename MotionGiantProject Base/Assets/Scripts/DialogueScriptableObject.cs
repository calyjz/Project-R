using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1)]
public class DialogueScriptableObject : ScriptableObject
{

    string[] tutorialRoom1Dialogue = new string[] { "No! My research has escaped! This isn't good" };
    string[] tutorialRoom2Dialogue = new string[] { "Lasers!? Maybe my dash can get put to use..." };
    string[] tutorialRoom3Dialogue = new string[] { "What are those... pits? I can’t reach the enemies, but I might be able to counter." };

    string[] gameRoom1Dialogue = new string[] { "Are those... the Corvid interns?" };
    string[] gameRoom2Dialogue = new string[] { "Lasers, pits, escaped Miasma research subjects, and Miasma-affected Corvid interns. Fantastic." };
    string[] gameRoom3Dialogue = new string[] { "Another room, more time wasted. I’m lucky my higher Corvid status means I get a Miasma-resistant mask…" };
    string[] gameRoom4Dialogue = new string[] { "Ugh, why is the lantern on the other side of the room?" };
    string[] gameRoom5Dialogue = new string[] { "The pits and lasers must be for the insects. Convenient that I wasn’t told of its existence..." };
    string[] gameRoom6Dialogue = new string[] { "I should be halfway out? Hopefully? The lab is underground for its secrecy, I must make it to the tunnels..." };
    string[] gameRoom7Dialogue = new string[] { "These interns would have made wonderful researchers. Becoming a Corvid is usually fatal." };
    string[] gameRoom8Dialogue = new string[] { "Almost there. This lab is unsalvageable. My contributions, my study on the insects would have made progress in Project Raven! To building resistance to Miasma!" };
    string[] gameRoom9Dialogue = new string[] { "Of course the last room isn’t easy to get through." };

    public string getDialogueByScene(string sceneName)
    {
        string dialogue = "I haven't been here before";
        if (sceneName == "TutorialRoom1")
        {
            return tutorialRoom1Dialogue[0];
        }
        else if (sceneName == "TutorialRoom2")
        {
            return tutorialRoom2Dialogue[0];
        }
        else if (sceneName == "TutorialRoom3")
        {
            return tutorialRoom3Dialogue[0];
        }
        else if (sceneName == "GameRoom1")
        {
            return gameRoom1Dialogue[0];
        }
        else if (sceneName == "GameRoom2")
        {
            return gameRoom2Dialogue[0];
        }
        else if (sceneName == "GameRoom3")
        {
            return gameRoom3Dialogue[0];
        }
        else if (sceneName == "GameRoom4")
        {
            return gameRoom4Dialogue[0];
        }
        else if (sceneName == "GameRoom5")
        {
            return gameRoom5Dialogue[0];
        }
        else if (sceneName == "GameRoom6")
        {
            return gameRoom6Dialogue[0];
        }
        else if (sceneName == "GameRoom7")
        {
            return gameRoom7Dialogue[0];
        }
        else if (sceneName == "GameRoom8")
        {
            return gameRoom8Dialogue[0];
        }
        else if (sceneName == "GameRoom9")
        {
            return gameRoom9Dialogue[0];
        }

        return dialogue;
    }
}

