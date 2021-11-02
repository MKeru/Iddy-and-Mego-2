using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector2 playerInitPosition;

    private void Start()
    {
        playerInitPosition = FindObjectOfType<Cat>().transform.position;
    }
    public void Restart()
    {
        //Method 1: restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // reset player position
        // Save player initial posiitoon when game starts
        // FindObjectOfType<Cat>().transform.position = playerInitPosition;
    }
}
