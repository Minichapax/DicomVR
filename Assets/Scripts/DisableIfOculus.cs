using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfOculus : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] elementsToEnable;
    public GameObject[] elementsToDisable;
    public bool disableInEditorAnyways;

    void Start()
    {
        #if UNITY_EDITOR
        if(disableInEditorAnyways)
            disablePC();
        #elif UNITY_ANDROID 
            disablePC();
        #endif
    }
    void disablePC(){
        //Desactivo los elementos que no quiera de PC
        foreach(GameObject e in elementsToDisable)
            e.SetActive(false);
        foreach(GameObject e in elementsToEnable)
            e.SetActive(true);
        //Desactivo todo VR
        gameObject.SetActive(false);
    }
}
