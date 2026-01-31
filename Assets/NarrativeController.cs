using UnityEngine;
using TMPro;
using System.Collections;
public class NarrativeController : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;
    bool isTyping;
    float typingSpeed = 0.04f;

    string[] lines =
    {
        "Hello.",
        "This is a narrative system.",
        "The text appears here."
    };

int index = 0;

IEnumerator TypeLine(string line)
{
    isTyping = true;
    text.text = "";

    foreach (char c in line)
    {
        text.text += c;
        yield return new WaitForSeconds(typingSpeed);
    }

    isTyping = false;
}
void Start()
{
    panel.SetActive(true);
    StartCoroutine(TypeLine(lines[index]));

}



void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (isTyping)
        {
            StopAllCoroutines();
            text.text = lines[index];
            isTyping = false;
            return;
        }

        index++;

        if (index >= lines.Length)
        {
            panel.SetActive(false);
            return;
        }

        StartCoroutine(TypeLine(lines[index]));
    }
}


}

