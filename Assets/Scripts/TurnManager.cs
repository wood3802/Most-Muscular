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
    public int enemyTurns, enemyTurnsTaken = 0;
    public LayerMask enemyLayerMask;
    Collider[] enemiesInRange;
    int i = 0;
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

        Invoke("EnemyTurn", 0.2f);
        
    }

    void EnemyTurn()
    {
        enemiesInRange = Physics.OverlapSphere(player.transform.position, 100, enemyLayerMask);
        enemyTurns = enemiesInRange.Length;
        enemyTurnsTaken = 0;
        i = 0;
        ExecuteEvents.Execute<ICustomEnemyMovementTarget>(enemiesInRange[i].gameObject, null, (x, y) => x.GeneratePath(player.transform.position));
    }

    public void enemyTurnsEnded()
    {
        enemyTurnsTaken++;
        i++;
        if (i < enemiesInRange.Length)
            ExecuteEvents.Execute<ICustomEnemyMovementTarget>(enemiesInRange[i].gameObject, null, (x, y) => x.GeneratePath(player.transform.position));
        else
            movementScript.turn = true;
    }


}
