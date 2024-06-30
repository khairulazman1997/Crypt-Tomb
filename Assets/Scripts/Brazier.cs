using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier : MonoBehaviour
{
    public static int noCandleLit = 0;
    public GameObject fire;
    public GameObject candleFire;
    public GameObject cupFire;
    public GameObject container;

    public void OnTriggerEnter(Collider other)
    {
        Transform otherFire = other.gameObject.transform.Find("Fire");
        if (otherFire != null)
        {
            GetComponent<AudioSource>().Play();
            fire.SetActive(true);
            candleFire.SetActive(true);
            candleFire.GetComponent<AudioSource>().Play();
            noCandleLit++;
            Debug.Log(noCandleLit);
        }

        if (noCandleLit == 4)
        {
            cupFire.SetActive(true);
            container.SetActive(true);
            container.GetComponent<AudioSource>().Play();
        }
    }

}
