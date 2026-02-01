using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
public class NarrativeController : MonoBehaviour
{
  public TMP_Text text;
  public AudioSource audioSource;
  public AudioClip typingClip;
  bool isTyping;
  float typingSpeed = 0.05f;


  public string[] inital_list;
 
  public UnityEvent onInitialTextFinished;

  int index = 0;

  void Start(){
    StartCoroutine(PrintInitialList());
  }

  IEnumerator PrintInitialList()
  {
      yield return PrintListRoutine(inital_list);
      onInitialTextFinished?.Invoke();
  }

  IEnumerator TypeLine(string line)
  {
      isTyping = true;
      text.text = "";

      foreach (char c in line)
      {
          text.text += c;
          if (c != ' ' && typingClip != null)
          {
              audioSource.pitch = Random.Range(0.9f, 1.1f);
              audioSource.PlayOneShot(typingClip);
          }
          yield return new WaitForSeconds(typingSpeed);
      }

      isTyping = false;
  }

  void Print(string s){
       StartCoroutine(TypeLine(s));
  }

  public void PrintList(string[] lines)
  {
      StartCoroutine(PrintListRoutine(lines));
  }

  IEnumerator PrintListRoutine(string[] lines)
  {
      for (int i = 0; i < lines.Length; i++)
      {
          Print(lines[i]);
          yield return new WaitUntil(() => !isTyping);
          yield return new WaitForSeconds(1f);
      }
      text.text = "";
  }

}

