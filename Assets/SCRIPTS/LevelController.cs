using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour
{
    public int dieLimit = -5;
    public Player player;
    public Transform player_spawn;

    void Start()
    {
      player.transform.position = player_spawn.position;
      player.Init();
      player.die.AddListener(Spaw);
    }

    void Update()
    {
   
     if (player.transform.position.y < dieLimit)
        {
            Spaw();
        }
    }

    public void EndLevel(){
      Debug.Log("end level");
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void Spaw(){
      player.pc.enabled = false;
      player.transform.position = player_spawn.position;
      player.pc.enabled = true;
    }
}
