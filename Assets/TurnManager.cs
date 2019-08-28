using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICustomTurnTarget: IEventSystemHandler
{
    void playerTurnEnded();
    void enemyTurnsEnded();
}

public class TurnManager : MonoBehaviour, ICustomTurnTarget
{
    public GameObject player;
    Movement movementScript;
    // Start is called before the first frame update
    void Start()
    {
        movementScript = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerTurnEnded()
    {
        //Execute event to enemies in range of player
        Collider[] enemiesInRange = Physics.OverlapSphere(player.transform.position, 100);
        for(int i = 0; i < enemiesInRange.Length; i++)
        {

        }
    }

    public void enemyTurnsEnded()
    {
        movementScript.turn = true;
        
    }
}
