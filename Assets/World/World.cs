using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material skyboxLight;
    public Material skyboxNight;

    private bool contrast = false;


    public void contrastMode(bool value){
        contrast = value;
        if(contrast){
            RenderSettings.skybox = skyboxNight;
            gameObject.SetActive(false);
        }else{
            RenderSettings.skybox = skyboxLight;
            gameObject.SetActive(true);
        }
    }
}
