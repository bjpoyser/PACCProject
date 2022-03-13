using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickerlight : MonoBehaviour
{
    Light light;

    float minSpeed = 0.01f;
    float maxSpeed = 0.09f;
    float minIntensity = 1.8f;
    float maxIntensity = 2f;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(run());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator run()
    {
        while (true)
        {
            light.enabled = true;
            light.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(minSpeed, maxSpeed));
            light.enabled = true;
            yield return new WaitForSeconds(Random.Range(minSpeed, maxSpeed));
        }
    }
}
