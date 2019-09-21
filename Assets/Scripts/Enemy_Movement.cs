using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public interface ICustomEnemyMovementTarget : IEventSystemHandler
{
    void RandomWalk();
    void GeneratePath(Vector3 goal);
}

public class Enemy_Movement : MonoBehaviour, ICustomEnemyMovementTarget
{
    float currentLerpTime = 0.35f;
    public float lerpTime = 0.35f;
    Vector3 originalPosition, newPosition;
    public Vector3 pubGoal;
    public LayerMask obstacles, walls;
    GameObject attackedObject;
    public GameObject map;
    public List<Vector3> path;
    EntityScript entity;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<EntityScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLerpTime != lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            if(currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
                if (attackedObject != null)
                {
                    ExecuteEvents.Execute<ICustomEntityTarget>(attackedObject, null, (x, y) => x.Damage(entity.atk, entity.objectName, entity.myPose.ToString()));
                }
                ExecuteEvents.Execute<ICustomTurnTarget>(map, null, (x, y) => x.enemyTurnsEnded());
            }
            float t = currentLerpTime / lerpTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(originalPosition, newPosition, t);
        }
    }

    public void RandomWalk()
    {
        int randDir = Random.Range(0, 4);
        Vector3 newDir = new Vector3(0, 0, 0);
        originalPosition = transform.position;
        RaycastHit hit;
        Ray aimRay;
        switch (randDir)
        {
            case (0):
                newDir = new Vector3(0, 0, 1);
                break;
            case (1):
                newDir = new Vector3(-1, 0, 0);
                break;
            case (2):
                newDir = new Vector3(0, 0, -1);
                break;
            case (3):
                newDir = new Vector3(1, 0, 0);
                break;
            default:
                break;

        }
        aimRay = new Ray(transform.position, newDir);
        if (!Physics.Raycast(aimRay, out hit, 1, obstacles))
        {
            newPosition = originalPosition + newDir;
            currentLerpTime = 0f;
            attackedObject = null;
        }
        else if (hit.transform.gameObject.tag == "Player")
        {
            //Attack
            currentLerpTime = 0f;
            attackedObject = hit.transform.gameObject;
        }
    }

    public void GeneratePath(Vector3 goal)
    {
        pubGoal = goal;
        Vector3 start = transform.position, current = new Vector3();
        Queue<Vector3> frontier = new Queue<Vector3>();
        frontier.Enqueue(start);
        Dictionary<Vector3, Vector3> came_from = new Dictionary<Vector3, Vector3>();
        came_from.Add(start, new Vector3(0, -1, 0));
        while(frontier.Count > 0)
        {
            current = frontier.Dequeue();
            if(current == goal)
            {
                break;
            }
            foreach(Vector3 next in Neighbors(current))
            {   
                if (!came_from.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    came_from.Add(next, current);
                }
            }
        }
        //current = goal;
        path = new List<Vector3>();
        while(current != start)
        {
            path.Add(current);
            current = came_from[current];
        }
        path.Reverse();
        RaycastHit hit;
        Ray aimRay = new Ray(transform.position, path[0]-transform.position);
        
        if (!Physics.Raycast(aimRay, out hit, 1f, obstacles))
        {
            originalPosition = transform.position;
            newPosition = path[0];
            currentLerpTime = 0f;

            attackedObject = null;
        }
        else if (hit.transform.gameObject.tag == "Player")
        {
            //Attack
            originalPosition = transform.position;
            newPosition = transform.position;
            currentLerpTime = 0f;
            attackedObject = hit.transform.gameObject;
        }
    }

    List<Vector3> Neighbors(Vector3 current)
    {
        List<Vector3> neighbors = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            Vector3 newDir = new Vector3(0, 0, 0);
            switch (i)
            {
                case (0):
                    newDir = new Vector3(1, 0, 0);
                    break;
                case (1):
                    newDir = new Vector3(0, 0, -1);
                    break;
                case (2):
                    newDir = new Vector3(-1, 0, 0);
                    break;
                case (3):
                    newDir = new Vector3(0, 0, 1);
                    break;
            }
            RaycastHit hit;
            Ray aimRay = new Ray(current, newDir);
            if (!Physics.Raycast(aimRay, out hit, 1, walls))
            {
                neighbors.Add(current + newDir);
            }
        }
        return neighbors;
    }

    Vector3 heuristic(Vector3 node, Vector3 goal)
    {
        float dx = Mathf.Abs(node.x - goal.x);
        float dz = Mathf.Abs(node.z - goal.z);
        return new Vector3(dx, 0, dz);
    }
}
