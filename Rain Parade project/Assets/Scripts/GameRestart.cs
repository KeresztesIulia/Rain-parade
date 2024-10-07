using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] string thisSceneName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) SceneManager.LoadScene(thisSceneName);
    }
}
