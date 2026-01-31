using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Player player;
    public Transform player_spawn;

    void Start()
    {
      player.transform.position = player_spawn.position;
      player.Init();
    }

    void Update()
    {
      if(player.transform.position.y < -5){
        player.pc.enabled = false;
        player.transform.position = player_spawn.position;
        player.pc.enabled = true;
      }
    }

    public void EndLevel(){
      Debug.Log("end level");

    }
}
