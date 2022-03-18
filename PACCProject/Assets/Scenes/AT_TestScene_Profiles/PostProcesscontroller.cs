using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcesscontroller : MonoBehaviour
{
   
    [SerializeField] private PostProcessVolume _postprocess;
    ColorGrading colorGradingLayer = null;
    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        _postprocess.profile.TryGetSettings(out colorGradingLayer);

    }
    // Update is called once per frame
    void Update()
    {


            if (Input.GetKey("v") == true && toggle == false)
            {
                colorGradingLayer.enabled.value = true;
            
                toggle = true;
            
                
            }
            else if (Input.GetKey("b") == true && toggle == true)
            {
                colorGradingLayer.enabled.value = false;
           
                toggle = false;
            
        };
       

   
    }
    
}
