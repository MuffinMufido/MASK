using UnityEngine;

public class EndPortal : MonoBehaviour
{
  LevelController parent;

  void Awake(){
    parent = GetComponentInParent<LevelController>();
  }

  void OnTriggerEnter(Collider other){
    if (other.gameObject.tag == "Player"){
      parent.EndLevel();
    };
  }
}
