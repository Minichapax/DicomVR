using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public void setRotation(float value){
        Vector3 rotacionActual = transform.eulerAngles;
        rotacionActual.y = value-180;
        transform.eulerAngles = rotacionActual;
    }
}
