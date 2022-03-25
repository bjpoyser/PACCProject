using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcesscontroller : MonoBehaviour
{

    [SerializeField] private PostProcessVolume _postprocess;
    ColorGrading colorGradingLayer = null;
    public bool pPOn;
    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        _postprocess.profile.TryGetSettings(out colorGradingLayer);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            colorGradingLayer.enabled.value = !colorGradingLayer.enabled.value;
        }
    }

}

