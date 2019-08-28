using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    float currentLerpTime = 0.35f;
    public float lerpTime = 0.35f;
    Vector3 originalPosition, newPosition;
    Quaternion originalRotation, newRotation;
    bool moving = false, rotating = false;
    public bool turn = true;
    public LayerMask wallLayer, enemyLayer;
    public GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        newPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && turn)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                RaycastHit hit;
                Ray aimRay = new Ray(transform.position, transform.forward);
                if (!Physics.Raycast(aimRay, out hit, 1, wallLayer))
                {
                    currentLerpTime = 0f;
                    originalPosition = transform.position;
                    newPosition = transform.position + new Vector3(Mathf.Round(transform.forward.x), Mathf.Round(transform.forward.y), Mathf.Round(transform.forward.z));
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RaycastHit hit;
                Ray aimRay = new Ray(transform.position, -transform.forward);
                if (!Physics.Raycast(aimRay, out hit, 1, wallLayer))
                {
                    currentLerpTime = 0f;
                    originalPosition = transform.position;
                    newPosition = transform.position - new Vector3(Mathf.Round(transform.forward.x), Mathf.Round(transform.forward.y), Mathf.Round(transform.forward.z));
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RaycastHit hit;
                Ray aimRay = new Ray(transform.position, -transform.right);
                if (!Physics.Raycast(aimRay, out hit, 1, wallLayer))
                {
                    currentLerpTime = 0f;
                    originalPosition = transform.position;
                    newPosition = transform.position - new Vector3(Mathf.Round(transform.right.x), Mathf.Round(transform.right.y), Mathf.Round(transform.right.z));
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                Ray aimRay = new Ray(transform.position, transform.right);
                if (!Physics.Raycast(aimRay, out hit, 1, wallLayer))
                {
                    currentLerpTime = 0f;
                    originalPosition = transform.position;
                    newPosition = transform.position + new Vector3(Mathf.Round(transform.right.x), Mathf.Round(transform.right.y), Mathf.Round(transform.right.z));
                    moving = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                currentLerpTime = 0f;
                newRotation = Quaternion.LookRotation((transform.position - transform.right) - transform.position);
                originalRotation = transform.rotation;
                moving = true;
                rotating = true;
                Collider[] objects = Physics.OverlapSphere(transform.position, 100, enemyLayer);
                for(int i = 0; i < objects.Length; i++)
                {
                    ExecuteEvents.Execute<ICustomBillboardTarget>(objects[i].gameObject, null, (x, y) => x.Turn(transform.right));
                }

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentLerpTime = 0f;
                newRotation = Quaternion.LookRotation((transform.position + transform.right) - transform.position);
                originalRotation = transform.rotation;
                moving = true;
                rotating = true;
                Collider[] objects = Physics.OverlapSphere(transform.position, 100, enemyLayer);
                for (int i = 0; i < objects.Length; i++)
                {
                    ExecuteEvents.Execute<ICustomBillboardTarget>(objects[i].gameObject, null, (x, y) => x.Turn(-transform.right));
                }
            }

        }
        if (currentLerpTime != lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
                if (!rotating)
                {
                    turn = false;
                    ExecuteEvents.Execute<ICustomTurnTarget>(map, null, (x, y) => x.playerTurnEnded());
                }
            }
            float t = currentLerpTime / lerpTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            transform.position = Vector3.Lerp(originalPosition, newPosition, t);
            if(rotating)
            {
                transform.rotation = Quaternion.Slerp(originalRotation, newRotation, t);
            }
        }
        else
        {
            rotating = false;
            moving = false;
            originalPosition = transform.position;
            newPosition = transform.position;
        }
    }
}
