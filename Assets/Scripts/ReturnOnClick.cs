using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnOnClick : MonoBehaviour
{
    public float delay = 3f;
    private bool inputEnabled = false;

    void Start()
    {
        Invoke(nameof(EnableInput), delay);
    }

    void EnableInput()
    {
        inputEnabled = true;
    }

    void Update()
    {
        if (!inputEnabled) return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
