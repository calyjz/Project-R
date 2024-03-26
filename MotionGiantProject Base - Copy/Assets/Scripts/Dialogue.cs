using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string line;
    public float textSpeed;
    public float delayBeforeNextLine; // Add this line
    public DialogueScriptableObject allLines;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        line = allLines.getDialogueByScene(SceneManager.GetActiveScene().name);
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed); // Use WaitForSecondsRealtime so the typing isn't paused
        }
        yield return new WaitForSecondsRealtime(delayBeforeNextLine); // Add this line
        gameObject.SetActive(false);
    }
}
