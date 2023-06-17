using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.Oculus;

public class DisableIfWindows : MonoBehaviour
{
    // Start is called before the first frame update
    public new Camera camera;
    public GameObject[] elements;
    public bool disableInEditorAnyways;
    void Start()
    {
    #if UNITY_EDITOR
        if(disableInEditorAnyways)
            disableVR();
    #else
        if (Utils.GetSystemHeadsetType() == SystemHeadset.None)  //Si no hay headset conectado
            disableVR();
        
    #endif
    }

    void disableVR(){
        //Cambia la camara
        Camera.main.enabled = false;
        camera.targetTexture = null;
        camera.enabled = true;
        //Desactivo los elementos que no quiera de PC
        foreach(GameObject e in elements)
            e.SetActive(false);
        //Desactivo todo VR
        gameObject.SetActive(false);
    }

}
