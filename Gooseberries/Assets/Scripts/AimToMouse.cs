using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimToMouse : MonoBehaviour
{
    Vector3 mouse_pos = new Vector3();
    Vector3 object_pos = new Vector3();
    float angle;
    public Transform bowPivotPos;
    public Transform centerPos;

    private void Update()
    {
        AimTowardMouse();
    }

    //need to do some math to figure out the 180 angle to flip the sprite
    //or maybe i can hack it to just use the x value of the mouse relative to the sprite
   
    //put a deadzone somewhere on the sprite
    //like a circle collider then raycast, if hit then dont rotate bow
    public void AimTowardMouse()
    {
        //aim toward mouse
        mouse_pos = Input.mousePosition;
        //mouse_pos.z = -20;//not sure what this does but it works
        object_pos = Camera.main.WorldToScreenPoint(centerPos.position);
        //mouse_pos.x = mouse_pos.x - object_pos.x;
        //mouse_pos.y = mouse_pos.y - object_pos.y;
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
