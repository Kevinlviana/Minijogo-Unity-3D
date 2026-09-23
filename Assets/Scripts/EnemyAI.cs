using UnityEngine;
using UnityEngine.AI;

namespace CristalRush
{
    public class EnemyAI : MonoBehaviour
    {
        public Transform[] waypoints;

        public float patrolSpeed = 2.5f;
        public float chaseSpeed = 4.5f;
        public float chaseRange = 8f;
        public float catchRange = 1f;
        public float waitAtWaypoint = 1f;

        NavMeshAgent agent;
        Transform player;

        int waypointIndex;
        float waitTimer;
        bool chasing;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (!agent.isOnNavMesh && NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                agent.Warp(hit.position);

            GameManager gm = GameManager.Instance;
            if (gm != null && gm.player != null) player = gm.player.transform;

            if (waypoints != null && waypoints.Length > 0) agent.SetDestination(waypoints[0].position);
        }

        void Update()
        {
            if (player == null || !agent.isOnNavMesh) return;

            GameManager gm = GameManager.Instance;
            if (gm != null && gm.State != GameState.Playing) return;

            float dist = Vector3.Distance(transform.position, player.position);

            if (dist <= chaseRange)
            {
                chasing = true;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
            }
            else if (chasing && dist > chaseRange * 1.4f)
            {
                chasing = false;
            }

            if (!chasing) Patrol();

            if (dist <= catchRange && gm != null) gm.PlayerCaught();
        }

        void Patrol()
        {
            if (waypoints == null || waypoints.Length == 0) return;
            agent.speed = patrolSpeed;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitAtWaypoint)
                {
                    waitTimer = 0f;
                    waypointIndex = (waypointIndex + 1) % waypoints.Length;
                    agent.SetDestination(waypoints[waypointIndex].position);
                }
            }
        }
    }
}
