using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter if you're in 3D
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the door — loading Level1...");
            SceneManager.LoadScene("Level1");
        }
    }
}
