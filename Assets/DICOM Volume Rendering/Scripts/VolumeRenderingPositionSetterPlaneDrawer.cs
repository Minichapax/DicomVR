using UnityEngine;

namespace VolumeRendering
{

public class VolumeRenderingPositionSetterPlaneDrawer : MonoBehaviour
{
    public enum Plane
    {
        X,
        Y,
        Z
    }

    public Plane plane = Plane.X;
    public float scale = 5f;

    private Vector3 initialPosition;
    void Start(){
        initialPosition = transform.localPosition ;
    }
    void Update(){
    }

    public void SetPosition(float value){
        if(plane == Plane.X){
            transform.localPosition  = new Vector3(value, initialPosition.y, initialPosition.z);
        }
        if(plane == Plane.Y){
            transform.localPosition  = new Vector3(initialPosition.x, value, initialPosition.z);
        }
        if(plane == Plane.Z){
            transform.localPosition  = new Vector3(initialPosition.x, initialPosition.y, value);
        }
    }

}

}
