using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    const string CHASE_TRIGGER = "Chasing";
    const string WALK_TRIGGER = "Patrolling";
    const string SHOOT_TRIGGER = "Shooting";

    public NavMeshAgent navMeshAgent;
    public float waitingTime = 4.5f;
    public float rotationTime = 2.5f;
    public float walkingSpeed = 3;
    public float RunningSpeed = 5.5f;

    public float enemyRadius = 15.0f;
    public float enemyViewAngle = 90.0f;
    public LayerMask player;
    public LayerMask obstacle;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public float firingrange = 10.0f;
    public float firingrate = 2.0f;

    public Transform[] waypoints;
    private int curWaypoint;
    private Animator animator;

    private Vector3 playerLastPosition = Vector3.zero;
    private Vector3 playerPosition;

    [SerializeField] private AIWeaponManager weaponManager;
    private int speedHash = Animator.StringToHash("Speed");

    private WeaponInverseKinematics weaponInverseKinematics;
    private GameObject playerObject;

    private float delayTime;
    private float rotate;
    private bool playerIsInRange;
    private bool playerIsNear;
    private bool patrolling;
    private bool playerCaught;

    void Start()
    {
        curWaypoint = 0;

        patrolling = true;
        playerIsInRange = false;
        playerIsNear = false;
        playerCaught = false;
        playerPosition = Vector3.zero;

        delayTime = waitingTime;
        rotate = rotationTime;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.SetDestination(waypoints[curWaypoint].position);

        weaponInverseKinematics = GetComponent<WeaponInverseKinematics>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // temporary way to keep the enemy speed but increase the animation speed so the enemy doesnt look like they are sliding
        float speedOffset = 1f;
        animator.SetFloat(speedHash, navMeshAgent.velocity.magnitude + speedOffset);

        EnviromentView();

        if (!patrolling) Chasing();
        else Patrolling();
    }

    private void Patrolling()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(WALK_TRIGGER);

        if (playerIsNear)
        {
            if (rotate <= 0)
            {
                Move(walkingSpeed);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                rotate -= Time.deltaTime;
            }
        }
        else
        {
            playerIsNear = false;
            playerLastPosition = Vector3.zero;

            navMeshAgent.SetDestination(waypoints[curWaypoint].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (delayTime <= 0)
                {
                    NextPoint();
                    Move(walkingSpeed);

                    delayTime = waitingTime;
                }
                else
                {
                    Stop();
                    delayTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Chasing()
    {
        playerIsNear = false;
        playerLastPosition = Vector3.zero;

        if (!playerCaught)
        {
            Move(RunningSpeed);
            navMeshAgent.SetDestination(playerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (delayTime <= 0 && !playerCaught && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                patrolling = true;
                playerIsNear = false;

                Move(walkingSpeed);
                rotate = rotationTime;
                delayTime = waitingTime;
                navMeshAgent.SetDestination(waypoints[curWaypoint].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) Stop();
                delayTime -= Time.deltaTime;
            }
        }
    }

    private void TryAttack()
    {
        AIWeaponManager.NecessaryUseConditions useConditions = weaponManager.GetWeaponNecessaryUseConditions();
        float dist = Vector3.Distance(transform.position, playerPosition);

        // Player is visible
        if (playerIsInRange)
        {
            // Player is in range to use the weapon -> stop the enemy and try an attack
            if (dist < useConditions.maxRange && dist > useConditions.minRange)
            {
                animator.SetTrigger(SHOOT_TRIGGER);
                weaponManager.DoAttack();
            }
            else
            {
                Move(RunningSpeed);
                navMeshAgent.SetDestination(playerPosition);
            }
        }
    }

    public void NextPoint()
    {
        curWaypoint++;
        curWaypoint %= waypoints.Length;

        navMeshAgent.SetDestination(waypoints[curWaypoint].position);
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);

        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (delayTime <= 0)
            {
                playerIsNear = false;

                Move(walkingSpeed);
                navMeshAgent.SetDestination(waypoints[curWaypoint].position);

                rotate = rotationTime;
                delayTime = waitingTime;
            }
            else
            {
                Stop();
                delayTime -= Time.deltaTime;
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.speed = speed;
        navMeshAgent.isStopped = false;
    }

    void Stop()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.isStopped = true;
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, enemyRadius, player);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < enemyViewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacle))
                {
                    playerIsInRange = true;
                    patrolling = false;
                    TryAttack();
                }
                else playerIsInRange = false;
            }
            if (Vector3.Distance(transform.position, player.position) > enemyRadius) playerIsInRange = false;
            if (playerIsInRange) playerPosition = player.transform.position;
        }
    }
}
