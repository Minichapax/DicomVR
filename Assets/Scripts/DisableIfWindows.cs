using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisableIfWindows : MonoBehaviour
{
    // Start is called before the first frame update
    public new Camera camera;
    public GameObject[] elementsToEnable;
    public GameObject[] elementsToDisable;
    public bool disableInEditorAnyways;
    void Start()
    {
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetSubsystems(displaySubsystems);
        if(displaySubsystems.Count==0) disableVR();

    #if UNITY_EDITOR
        if(disableInEditorAnyways)
            disableVR();  
    #endif
    }

    void disableVR(){
        //Cambia la camara
        Camera.main.enabled = false;
        camera.targetTexture = null;
        camera.enabled = true;
        //Desactivo los elementos que no quiera de PC
        foreach(GameObject e in elementsToDisable)
            e.SetActive(false);
        foreach(GameObject e in elementsToEnable)
            e.SetActive(true);
        //Desactivo todo VR
        gameObject.SetActive(false);

    }


}
