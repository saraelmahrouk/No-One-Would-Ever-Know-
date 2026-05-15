using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyChaser : MonoBehaviour
{
    // ── Inspector fields ──────────────────────
    public Transform player;
    public Animator  animator;

    public float chaseSpeed   = 3.5f;
    public float roamSpeed    = 1.5f;
    public float viewDistance = 20f;
    [Range(0, 360)]
    public float fieldOfView  = 120f;
    public float eyeHeight    = 1.5f;
    public float roamRadius   = 15f;
    public float roamWaitMin  = 2f;
    public float roamWaitMax  = 5f;
    public LayerMask visionMask = ~0;

    // Animation state names
    public string idleAnim  = "Idle";
    public string walkAnim  = "Walk";
    public string attackAnim = "Attack";

    // ── Death sequence fields ─────────────────
    public Camera mainCamera;
    public Camera deathCamera;
    public MonoBehaviour playerController; // drag your player movement script here

    // ── Private ───────────────────────────────
    NavMeshAgent agent;
    bool caught = false;
    string currentAnim = "";
    Vector3 spawnPoint;

    enum State { Roaming, Chasing }
    State currentState = State.Roaming;

    float roamTimer = 0f;
    bool  waitingAtSpot = false;

    private bool triggered = false;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = roamSpeed;
        spawnPoint = transform.position;
        PlayAnim(idleAnim);
        PickNewRoamPoint();

        // Make sure death camera is off at start
        if (deathCamera != null)
            deathCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (caught) return;

        if (CanSeePlayer())
        {
            if (!triggered)
            {
                GameManagerRoom2.instance.TriggerEntitySeen();
                triggered = true;
            }
            else if (currentState == State.Roaming)
            {
                GameManagerRoom2.instance.PlaySound();
            }

            currentState = State.Chasing;
            agent.speed = chaseSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            PlayAnim(walkAnim);
        }
        else
        {
            currentState = State.Roaming;
            agent.speed = roamSpeed;
            HandleRoaming();
        }
    }

    void HandleRoaming()
    {
        if (waitingAtSpot)
        {
            agent.isStopped = true;
            PlayAnim(idleAnim);
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0f)
            {
                waitingAtSpot = false;
                PickNewRoamPoint();
            }
        }
        else
        {
            agent.isStopped = false;
            PlayAnim(walkAnim);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                waitingAtSpot = true;
                roamTimer = Random.Range(roamWaitMin, roamWaitMax);
            }
        }
    }

    void PickNewRoamPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * roamRadius;
            randomPoint.y = spawnPoint.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    void PlayAnim(string name)
    {
        if (currentAnim == name) return;
        currentAnim = name;
        animator.Play(name);
    }

    bool CanSeePlayer()
    {
        Vector3 origin    = transform.position + Vector3.up * eyeHeight;
        Vector3 target    = player.position;
        Vector3 direction = (target - origin).normalized;
        float   distance  = Vector3.Distance(origin, target);

        if (distance > viewDistance) return false;
        
        Debug.DrawRay(origin, direction * distance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, visionMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.root == transform.root) return false;

            if (hit.collider.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (caught || !other.CompareTag("Player")) return;
        caught = true;
        agent.isStopped = true;
        StartCoroutine(DoCatch());
    }

    System.Collections.IEnumerator DoCatch()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (deathCamera != null)
            deathCamera.gameObject.SetActive(true);

        animator.Play(attackAnim);

        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnim) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        // small delay if you want a “fade out” feel
        yield return new WaitForSeconds(0.5f);

        // restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}