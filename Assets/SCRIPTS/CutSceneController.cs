using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public NarrativeController nc;
    void Start()
    {
      nc.onInitialTextFinished.AddListener(EndCutscene);
        
    }

    void Update()
    {
        
    }

    void EndCutscene(){
      Debug.Log("end");
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
