using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public void setRotationY(float value){
        Vector3 rotacionActual = transform.eulerAngles;
        rotacionActual.y = value-180;
        transform.eulerAngles = rotacionActual;
    }
    public void setRotationX(float value){
        Vector3 rotacionActual = transform.eulerAngles;
        rotacionActual.x = value-90;
        transform.eulerAngles = rotacionActual;
    }
}
