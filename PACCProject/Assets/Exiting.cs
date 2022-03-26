using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exiting : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject HoldOn;

    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            if(GameManager.Instance.MaxSelections <= GameManager.Instance.currentFound) uiObject.SetActive(true);
            else HoldOn.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
