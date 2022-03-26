using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSelector : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    private bool _selected;

    public bool Selected { get => _selected; set => _selected = value; }

    private void Awake()
    {
        _toggle.onValueChanged.AddListener((value) => SelectCurrent(value));
    }

    public void SelectCurrent(bool status)
    {
        _selected = status;

        if (_selected)
            GameManager.Instance.CurrentSelections++;
        else
            GameManager.Instance.CurrentSelections--;
        
        GameManager.Instance.UpdateSelections();
    }

    public void SetInteractionStatus(bool pStatus)
    {
        if (_toggle != null && !_toggle.isOn)
        {
            _toggle.interactable = pStatus;
        } 
    }
}
