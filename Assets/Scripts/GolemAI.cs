using UnityEngine;
using UnityEngine.AI;

public class GolemAI : MonoBehaviour
{
    public Transform player;
    public float followRadius = 10f;
    public float roamRadius = 5f;
    public float roamInterval = 3f;

    private NavMeshAgent agent;
    private float roamTimer;
    private Renderer golemRenderer;
    private Color? currentColor = null;

    private float colorInteractionCooldown = 2f;
    private float lastColorInteractionTime = -Mathf.Infinity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        golemRenderer = GetComponent<Renderer>();
        roamTimer = roamInterval;
    }

    void Update()
    {
        roamTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followRadius)
        {
            agent.SetDestination(player.position);
            return;
        }

        if (roamTimer >= roamInterval)
        {
            Vector3 roamPos = GetRandomPositionNearPlayer();
            agent.SetDestination(roamPos);
            roamTimer = 0;
        }

        // Right-click detection with cooldown
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time >= lastColorInteractionTime + colorInteractionCooldown)
            {
                Debug.Log("Right-click detected");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                    if (hit.collider.CompareTag("Golem"))
                    {
                        Debug.Log("Right-clicked on the Golem!");
                        AbsorbColor(hit.collider.gameObject);
                        lastColorInteractionTime = Time.time;
                    }
                    else
                    {
                        Debug.Log("Right-clicked on a different object.");
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit anything.");
                }
            }
            else
            {
                Debug.Log("Interaction on cooldown.");
            }
        }
    }

    void AbsorbColor(GameObject clickedObject)
    {
        Renderer clickedRenderer = clickedObject.GetComponent<Renderer>();
        if (clickedRenderer != null && currentColor == null)
        {
            Color clickedColor = clickedRenderer.material.color;
            if (clickedColor != GameManager.Instance.grayMaterial.color)
            {
                golemRenderer.material.color = clickedColor;
                currentColor = clickedColor;
                Debug.Log("Absorbed color from " + clickedObject.name + ": " + clickedColor);
            }
        }
    }

    Vector3 GetRandomPositionNearPlayer()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += player.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time < lastColorInteractionTime + colorInteractionCooldown)
        {
            Debug.Log("OnTriggerEnter is on cooldown.");
            return;
        }

        lastColorInteractionTime = Time.time;

        Debug.Log("Trigger Entered: " + other.gameObject.name);

        Transform parent = other.transform.parent;
        if (parent == null)
        {
            Debug.Log("No parent found for collided object.");
            return;
        }

        Debug.Log("Hit parent tag: " + parent.tag);

        if (parent.CompareTag("Untagged"))
        {
            Debug.Log("Parent is untagged, skipping.");
            return;
        }

        Renderer[] childRenderers = parent.GetComponentsInChildren<Renderer>();
        if (childRenderers.Length == 0)
        {
            Debug.Log("Parent has no children with Renderers.");
            return;
        }

        Color golemColor = golemRenderer.material.color;
        Color absorbedColor = Color.clear;
        bool didSwap = false;

        foreach (Renderer childRenderer in childRenderers)
        {
            Color childColor = childRenderer.material.color;

            if (childColor == GameManager.Instance.grayMaterial.color)
                continue;

            if (golemColor == GameManager.Instance.grayMaterial.color)
            {
                Debug.Log("Golem is empty, absorbing color from child.");
                absorbedColor = childColor;
                golemRenderer.material.color = absorbedColor;
                currentColor = absorbedColor;
            }
            else
            {
                if (currentColor == null)
                {
                    Debug.Log("Swapping colors between Golem and child.");
                    absorbedColor = childColor;
                    Color tempColor = golemColor;
                    golemRenderer.material.color = absorbedColor;
                    currentColor = absorbedColor;
                    didSwap = true;
                }
            }

            break;
        }

        if (absorbedColor == Color.clear) return;

        if (didSwap)
        {
            foreach (Renderer childRenderer in childRenderers)
            {
                if (childRenderer.material.color == absorbedColor)
                {
                    childRenderer.material.color = golemColor;
                }
            }
        }
        else
        {
            foreach (Renderer childRenderer in childRenderers)
            {
                if (childRenderer.material.color == absorbedColor)
                {
                    childRenderer.material.color = GameManager.Instance.grayMaterial.color;
                }
            }
        }
    }
}
