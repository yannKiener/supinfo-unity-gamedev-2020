using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitToBounds : MonoBehaviour
{
    public bool debugPosition;
    public Vector2 lowerLimits;
    public Vector2 higherLimits;

    // Update is called once per frame
    void LateUpdate()
    {
        if (debugPosition)
        {
            Debug.Log(gameObject.name + "'s position : " + transform.position);
            Debug.Log(gameObject.name + "'s local position : " + transform.localPosition);
        }

        //Limit X position
        if (transform.localPosition.x > higherLimits.x)
        {
            transform.localPosition = new Vector3(higherLimits.x, transform.localPosition.y, transform.localPosition.z);
        } else
        {
            if (transform.localPosition.x < lowerLimits.x)
            {
                transform.localPosition = new Vector3(lowerLimits.x, transform.localPosition.y, transform.localPosition.z);
            }
        }

        //Limit Y position
        if (transform.localPosition.y > higherLimits.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, higherLimits.y, transform.localPosition.z);
        }
        else
        {
            if (transform.localPosition.y < lowerLimits.y)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, lowerLimits.y, transform.localPosition.z);
            }
        }

    }
}
