using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    int currentPanel = 0;
    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentPanel < panels.Length)
        {
            panels[currentPanel].SetActive(false);
            currentPanel++;
            if (currentPanel >= panels.Length)
            {
                Time.timeScale = 1f;
                return;
            }
            panels[currentPanel].SetActive(true);
        }
    }
}
