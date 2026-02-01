using UnityEngine;

public class DamageBlcok : MonoBehaviour
{
  void OnTriggerEnter(Collider other){
    Debug.Log("bam");
    if (other.gameObject.tag == "Player"){
      other.gameObject.GetComponent<Player>().Die();
    }
  }
}
