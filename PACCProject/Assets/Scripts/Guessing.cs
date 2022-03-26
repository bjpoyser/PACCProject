using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Guessing : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _notebook;
    [SerializeField] private List<Toggle> _toggles = new List<Toggle>();

    public void ShowWinScreen()
    {
        SceneManager.LoadScene("Win");
    }

    public void ShowGuess()
    {
        for (int i = 0; i < _toggles.Count; i++)
        {
            _toggles[i].gameObject.SetActive(true);
        }
    }

    public void ShowLosePopUp()
    {
        _notebook.SetActive(false);
        _losePanel.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
