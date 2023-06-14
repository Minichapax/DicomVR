using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.Oculus;

public class CanvasVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
        gameObject.SetActive(true);
        #else
        if (Utils.GetSystemHeadsetType() == 0)
        {
            gameObject.SetActive(false);
        }
        #endif
    }

}
