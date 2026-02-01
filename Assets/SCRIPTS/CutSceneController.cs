using UnityEngine;

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
    }
}
