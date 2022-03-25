using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class AlertBoxScript : MonoBehaviour
{
   
    public AudioClip alertClip;
    public AudioSource audioSource;
    public Image alertImage;
    public GameObject item;
    public GameObject itemNotebook;
    public Transform player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(alertImage.gameObject.activeSelf == true)
        {
            Vector3 objectPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
            float playerObjectAngle = Vector3.SignedAngle(this.player.forward, objectPos - playerPos, this.player.transform.up); //Vector3.Angle(playerPos.forward, this.transform.position - playerPos.position);
            //Debug.Log("Player is this far: " + playerObjectAngle);
            if(Mathf.Abs(playerObjectAngle) < 40)
            {
                playerObjectAngle *= 20;

                alertImage.rectTransform.position = new Vector2(Screen.width / 2 + playerObjectAngle, Screen.height / 2);
            }
            
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        alertImage.gameObject.SetActive(true);
        audioSource.PlayOneShot(alertClip);
        /*Item.collectable = item;
        Item.collectableNotebook = itemNotebook;
        Debug.Log("Item: " + item.name);*/
    }

    private void OnTriggerExit(Collider other)
    {
        alertImage.gameObject.SetActive(false);
    }
}
