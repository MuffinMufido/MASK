using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public Button start_button;
    public Button options_button;
    public Button exit_button;
    public GameObject panel;
    
    void Start()
    {
      start_button.onClick.AddListener(GameStart);
      options_button.onClick.AddListener(ToggleOptions);
      exit_button.onClick.AddListener(ExitGame);
    }

    void GameStart(){
      Debug.Log("start");
    }

    void ToggleOptions(){
      EventSystem.current.SetSelectedGameObject(null);
      Debug.Log("Options");
      panel.SetActive(!panel.activeInHierarchy);
    }

    void ExitGame(){
      Application.Quit();

      // This works specifically for the Unity Editor
      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      #endif
    }
}
