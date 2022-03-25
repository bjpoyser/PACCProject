using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject notebook;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameIsPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Pause();
            }
        }
    }

    void Resume()
    {
       // Item.collectable.SetActive(false);
        notebook.SetActive(false);
       // Item.collectableNotebook.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        //Item.collectable.SetActive(true);
        notebook.SetActive(true);
       // Item.collectableNotebook.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
