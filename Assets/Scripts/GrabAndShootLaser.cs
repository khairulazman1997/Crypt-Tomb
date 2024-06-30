using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

public class GrabAndShootLaser : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public LayerMask grabbableMask;
    private GameObject grabbedObject;
    public GameObject tipOfStaff;

    private int PowerLevel;
    public TMP_Text ui;

    // Start is called before the first frame update
    void Start()
    {
        PowerLevel = 0;
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.GetState(handType))
        {

            RaycastHit hit;

            if (Physics.Raycast(tipOfStaff.transform.position, transform.forward, out hit, 100) && grabbedObject == null)
            {
                hitPoint = hit.point;
                ShowLaser(hit);

                if (hit.collider.gameObject.layer >= 7)
                {
                    GrabObject(hit);
                }
            }
        }
        else
        {
            laser.SetActive(false);
        }

        if (grabAction.GetStateUp(handType))
        {
            laser.SetActive(false);
            if (grabbedObject != null && grabbedObject.layer == 7)
            {
                ShootObject();
            }

            if (grabbedObject != null && grabbedObject.layer == 8)
            {
                ReadBook();
            }
        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(tipOfStaff.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
                                                laserTransform.localScale.y,
                                                hit.distance);
    }

    private void GrabObject(RaycastHit hit)
    {
        grabbedObject = hit.transform.gameObject;
        grabbedObject.transform.SetParent(transform);
        grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        grabbedObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //grabbedObject.transform.localPosition = new Vector3(0, 0, 0);

        if (grabbedObject.layer == 9)
        {
            grabbedObject.GetComponent<Animator>().enabled = false;
            grabbedObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        grabbedObject.transform.position = tipOfStaff.transform.position + (transform.forward * 0.7f);
        laser.SetActive(false);

        if (grabbedObject.layer == 7)
        {
            grabbedObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void ShootObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<AudioSource>().Play();
        grabbedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 400 * PowerLevel);
        grabbedObject = null;
    }

    private void ReadBook()
    {
        grabbedObject.GetComponent<AudioSource>().Play();
        PowerLevel++;
        ui.text = "PowerLevel" + PowerLevel;
        Destroy(grabbedObject);
    }
}
