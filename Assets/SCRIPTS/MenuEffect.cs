using UnityEngine;

public class MenuEffect : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update(){
        transform.Rotate(new Vector3(0,5,0) * Time.deltaTime);
    }

}
