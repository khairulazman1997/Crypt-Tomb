using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionKing : MonoBehaviour
{
    public GameObject gem;
    public GameObject blessing;
    public GameObject cameraRig;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
            this.gem.SetActive(true);
            blessing.SetActive(true);
            cameraRig.GetComponent<AudioSource>().enabled = false;
        }
    }
}
