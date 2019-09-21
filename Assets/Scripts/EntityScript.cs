using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICustomEntityTarget : IEventSystemHandler
{
    void Damage(float enemy_atk, string enemyName, string pose);
}

public class EntityScript : MonoBehaviour, ICustomEntityTarget
{
    public int hp = 5;
    public float atk = 0;
    public float def = 0;
    public enum Poses
    {
        LatSpread,
        DoubleBicep,
        SideChest,
        SideTris,
        AbdominalAndThigh,
        MostMuscular
    };
    public Poses myPose;
    public GameObject textObject;
    public string objectName;
    // Start is called before the first frame update
    void Start()
    {
        myPose = Poses.LatSpread;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float enemy_atk, string enemyName, string pose)
    {
        Poses enemyPose;
        Enum.TryParse<Poses>(pose, out enemyPose);
        float attempt = UnityEngine.Random.Range(0f, 1f);
        if (attempt < (enemy_atk / (enemy_atk + def)))
        {
            hp--;
            ExecuteEvents.Execute<ICustomMessageTarget>(textObject, null, (x, y) => x.newMessage(enemyName + " attacked for 1 point of damage!"));
        }
        else
        {
            //attack missed
            Debug.Log("Miss!");
            ExecuteEvents.Execute<ICustomMessageTarget>(textObject, null, (x, y) => x.newMessage(enemyName + " missed!"));
        }
        if(hp == 0)
        {
            if(gameObject.tag != "Player")
                gameObject.SetActive(false);
            else
            {
                //Game over
            }
        }
    }
}
