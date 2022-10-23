using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public float WaitingTime = 4;                 //  Wait time of every action
    public float RotationTime = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float WalkingSpeed = 6;                     //  Walking speed, speed in the nav mesh agent
    public float RunningSpeed = 9;                      //  Running speed of enemy

    public float EnemyRadius = 15;                   //  Radius of the enemy view
    public float EnemyViewAngle = 90;                    //  Angle of the enemy view
    public LayerMask player;                    //  To detect the player with the raycast
    public LayerMask obstacle;                  //  To detect the obstacules with the raycast
    public float MeshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something


    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int TheCurrentWaypoint;                     //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    float DelayTime;                               //  Variable of the wait time that makes the delay
    float Rotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool playerInRange;                           //  If the player is in range of vision, state of chasing
    bool PlayerIsNear;                              //  If the player is near, state of hearing
    bool Patrolling;                                //  If the enemy is patrol, state of patrolling
    bool PlayerCaught;                            //  if the enemy has caught the player

    void Start()
    {
        PlayerPosition = Vector3.zero;
        Patrolling = true;
        PlayerCaught = false;
        playerInRange = false;
        PlayerIsNear = false;
        DelayTime = WaitingTime;                 //  Set the wait time variable that will change
        Rotate = RotationTime;

        TheCurrentWaypoint = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = WalkingSpeed;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[TheCurrentWaypoint].position);    //  Set the destination to the first waypoint
    }

    private void Update()
    {
        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!Patrolling)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        //  The enemy is chasing the player
        PlayerIsNear = false;                       //  Set false that the player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!PlayerCaught)
        {
            Move(RunningSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player last location
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (DelayTime <= 0 && !PlayerCaught && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                Patrolling = true;
                PlayerIsNear = false;
                Move(WalkingSpeed);
                Rotate = RotationTime;
                DelayTime = WaitingTime;
                navMeshAgent.SetDestination(waypoints[TheCurrentWaypoint].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                DelayTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (PlayerIsNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position to investagate
            if (Rotate <= 0)
            {
                Move(WalkingSpeed);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                Rotate -= Time.deltaTime;
            }
        }
        else
        {
            PlayerIsNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[TheCurrentWaypoint].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (DelayTime <= 0)
                {
                    NextPoint();          // update current waypoint position to another position
                    Move(WalkingSpeed);
                    DelayTime = WaitingTime;
                }
                else
                {
                    Stop();
                    DelayTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {

    }

    public void NextPoint()
    {
        TheCurrentWaypoint = (TheCurrentWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[TheCurrentWaypoint].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        PlayerCaught = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (DelayTime <= 0)
            {
                PlayerIsNear = false;
                Move(WalkingSpeed);
                navMeshAgent.SetDestination(waypoints[TheCurrentWaypoint].position);
                DelayTime = WaitingTime;
                Rotate = RotationTime;
            }
            else
            {
                Stop();
                DelayTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, EnemyRadius, player);   //  Make an overlap sphere around the enemy to detect the player in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < EnemyViewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enemy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacle))
                {
                    playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    Patrolling = false;                 //  Change the state to chasing the player
                }
                else
                {
                                                 //If the player is behind a obstacle the player position will not be registered
                   playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > EnemyRadius)
            {            
                  //If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                  
                playerInRange = false;                //  Changes the state of a chasing enemy
            }
            if (playerInRange)
            {
                
                  //If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                  
                PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision then go to it
            }
        }
    }
}
