using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private int _maxSelections = 3;
    [SerializeField] private int _currentSelections = 0;
    [SerializeField] private List<ClueSelector> _clueSelctorList = new List<ClueSelector>();
    [SerializeField] private Button _guess;
    [SerializeField] private ClueSelector _firstRightClue;
    [SerializeField] private ClueSelector _secondRightClue;
    [SerializeField] private ClueSelector _thirdRightClue;

    public static GameManager Instance { get { return instance; } }

    public int MaxSelections { get => _maxSelections; set => _maxSelections = value; }
    public int CurrentSelections { get => _currentSelections; set => _currentSelections = value; }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;

        _guess.onClick.AddListener(CheckFinalSelections);
        _guess.interactable = false;
    }

    private void Start()
    {
        UpdateSelections();
    }

    public void UpdateSelections()
    {
        bool status = _currentSelections < _maxSelections;
        for (int i = 0; i < _clueSelctorList.Count; i++)
        {
            _clueSelctorList[i].SetInteractionStatus(status);
        }

        _guess.interactable = !status;
    }

    public void CheckFinalSelections()
    {
        int correctAnswers = 0;
        for (int i = 0; i < _clueSelctorList.Count; ++i)
        {
            if (_clueSelctorList[i].Selected)
            {
                if(_clueSelctorList[i] == _firstRightClue || _clueSelctorList[i] == _secondRightClue || _clueSelctorList[i] == _thirdRightClue)
                {
                    correctAnswers++;
                }
            }
        }

        if (correctAnswers == 3) Debug.Log("You Win");
    }
}
