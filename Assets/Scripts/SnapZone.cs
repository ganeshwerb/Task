using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform snapPosition;
    private void OnTriggerEnter(Collider other)
    {
        if((layerMask.value == (1 << other.gameObject.layer)))
        {
            other.transform.GetComponent<Grabbable>().enabled = false;
            other.transform.parent = transform;
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;
            other.transform.localScale = Vector3.one;
            other.transform.GetComponent<Grabbable>().enabled = true;
            BandPressLevelManager.LockPulley();
        }
    }
}
