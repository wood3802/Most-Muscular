using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICustomBillboardTarget: IEventSystemHandler
{
    void Turn(Vector3 dir);
}

public class Billboard : MonoBehaviour, ICustomBillboardTarget
{
    bool turning;
    Quaternion newRotation, originalRotation;
    float currentLerpTime = 0f;
    public float lerpTime = 0.25f;

    void OnEnable()
    {
        GameObject player = GameObject.Find("Cube");
        transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y + 180, player.transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, Vector3.up);
        if(turning)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
                turning = false;
            }
            float t = currentLerpTime / lerpTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.rotation = Quaternion.Slerp(originalRotation, newRotation, t);
        }
    }

    public void Turn(Vector3 dir)
    {
        newRotation = Quaternion.LookRotation((transform.position + dir) - transform.position);
        originalRotation = transform.rotation;
        currentLerpTime = 0;
        turning = true;
    }
}
