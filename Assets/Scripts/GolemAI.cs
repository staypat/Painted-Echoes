using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class GolemAI : MonoBehaviour
{
    public Transform player;
    public float followRadius = 10f;
    public float roamRadius = 5f;
    public float roamInterval = 5f;

    private NavMeshAgent agent;
    private float roamTimer;
    private Renderer golemRenderer;
    private Material currentColor;

    private float colorInteractionCooldown = 2f;
    public float lastColorInteractionTime = -Mathf.Infinity;
    public Animator animator;
    private bool isRolling = false;

    private bool isClearing = false;
    public InputActionReference absorbAction;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        golemRenderer = GetComponent<Renderer>();
        golemRenderer.enabled = false; // ← Hide the mesh
        roamTimer = roamInterval;
        currentColor = GameManager.Instance.grayMaterial;
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
        // if (Input.GetMouseButtonDown(1))
        // {

        //     Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //     RaycastHit hit;

        //     if (Physics.Raycast(ray, out hit, 4.5f))
        //     {
        //         Transform root = hit.collider.transform.root;

        //         if (root.CompareTag("Golem"))
        //         {
        //             Debug.Log("Right-clicked on the Golem or its child!");
        //             GolemColor(root.gameObject); // always pass the root Golem
        //             lastColorInteractionTime = Time.time;
        //         }
        //         else
        //         {
        //             Debug.Log("Right-clicked on a different object.");
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("Raycast did not hit anything.");
        //     }
        // }

        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.3f;

        animator.SetBool("roll", isMoving);

        if (!isMoving)
        {
            Transform childTransform = transform.GetChild(0);

            Vector3 currentEuler = childTransform.eulerAngles;

            float targetX = -90f;
            float targetY = currentEuler.y;
            float targetZ = currentEuler.z;

            Quaternion targetRotation = Quaternion.Euler(targetX, targetY, targetZ);

            float rotationSpeed = 50f;
            childTransform.rotation = Quaternion.RotateTowards(
                childTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void AbsorbGolem(InputAction.CallbackContext context)
    {

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 4.5f))
        {
            Transform root = hit.collider.transform.root;

            if (root.CompareTag("Golem"))
            {
                Debug.Log("Right-clicked on the Golem or its child!");
                GolemColor(root.gameObject); // always pass the root Golem
                lastColorInteractionTime = Time.time;
                ClearGolemColor();
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

    public void GolemColor(GameObject clickedObject)
    {
        Debug.Log("GolemColor called on: " + currentColor);
        if (currentColor != null && currentColor != GameManager.Instance.grayMaterial)
        {
            Debug.Log("Golem has color, clearing to gray. Raycast temporarily disabled.");
            isClearing = true;
            ClearGolemColor();
            lastColorInteractionTime = Time.time;

            StartCoroutine(DelayedAbsorb(clickedObject));
            return;
        }

        TryAbsorbColor(clickedObject);
    }



    void TryAbsorbColor(GameObject sourceObject)
    {
        Renderer clickedRenderer = sourceObject.GetComponent<Renderer>();
        if (clickedRenderer != null)
        {
            Color clickedColor = clickedRenderer.material.color;
            Color gray = GameManager.Instance.grayMaterial.color;

            if (clickedColor != gray)
            {
                ApplyColorToGolem(clickedRenderer.material);
                Debug.Log("Absorbed color from " + sourceObject.name + ": " + clickedColor);
                lastColorInteractionTime = Time.time;
            }
            else
            {
                Debug.Log("Clicked color is gray — nothing to absorb.");
            }
        }
        else
        {
            Debug.Log("No renderer found on clicked object.");
        }
    }

    public void ApplyColorToGolem(Material newMat)
    {
        Debug.Log("Color: " + newMat);
        currentColor = newMat;

        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            renderer.material = newMat;
            renderer.material.color = newMat.color;
        }
    }

    void ClearGolemColor()
    {
        currentColor = GameManager.Instance.grayMaterial;

        Color gray = GameManager.Instance.grayMaterial.color;

        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            renderer.material.color = gray;
        }

        Debug.Log("Golem color cleared to gray.");
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
        if (isClearing)
        {
            Debug.Log("OnTriggerEnter skipped during clear.");
            return;
        }


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
                ApplyColorToGolem(childRenderer.material);
            }
            else
            {
                if (currentColor == GameManager.Instance.grayMaterial)
                {
                    Debug.Log("Swapping colors between Golem and child.");
                    absorbedColor = childColor;
                    ApplyColorToGolem(childRenderer.material);
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

    System.Collections.IEnumerator DelayedAbsorb(GameObject sourceObject)
    {
        Debug.Log("Starting delayed absorb for: " + sourceObject.name);
        yield return null;
        TryAbsorbColor(sourceObject);
        isClearing = false; // Flag OFF after absorb
    }

    private void OnEnable()
    {
        absorbAction.action.performed += AbsorbGolem;
    }

    private void OnDisable()
    {
        absorbAction.action.performed -= AbsorbGolem;
    }
}