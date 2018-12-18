using Rewired;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 10F;

    [Header("Hands")]
    public Transform effectAnchor;
    public Animator animator;

    [Header("Templates")]
    public GameObject buildModeEffect;

    private Rigidbody rb;
    private Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = ReInput.players.GetPlayer(0);

        EventBus.Register(this);
    }

    private void Update()
    {
        switch (GameManager.Instance.Phase)
        {
            case GamePhase.BUILD:
                if (BuildUpdate())
                    return;
                break;
            case GamePhase.COMBAT:
                if (CombatUpdate())
                    return;
                break;
        }

        Vector2 movement = player.GetAxis2D("Move X", "Move Z");

        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;

        rb.velocity = transform.rotation * new Vector3(movement.x, 0F, movement.y).normalized * speed + Vector3.up * rb.velocity.y;
    }

    private bool BuildUpdate()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Open build menu and freeze player
        //    return true;
        //}

        return false;
    }

    private bool CombatUpdate()
    {
        return false;
    }

    [SubscribeEvent]
    public void OnPhaseChange(EventGamePhaseChanged e)
    {
        UpdateAnimator(e.phase);
        UpdateEffect(e.phase);
    }

    public void UpdateEffect()
    {
        UpdateEffect(GameManager.Instance?.Phase ?? GamePhase.BUILD);
    }

    public void UpdateEffect(GamePhase phase)
    {
        if (!effectAnchor)
            return;

        foreach (Transform t in effectAnchor)
            Destroy(t.gameObject);

        if (phase == GamePhase.BUILD)
        {
            if (buildModeEffect)
                Instantiate(buildModeEffect, effectAnchor);
        }
        else
        {

        }
    }

    public void UpdateAnimator()
    {
        UpdateAnimator(GameManager.Instance?.Phase ?? GamePhase.BUILD);
    }

    public void UpdateAnimator(GamePhase phase)
    {
        if (!animator)
            return;

        // Resets all the bools in the animator
        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.type == AnimatorControllerParameterType.Bool || p.type == AnimatorControllerParameterType.Trigger)
                animator.SetBool(p.nameHash, false);

        animator.SetBool("BuildMode", phase == GamePhase.BUILD);

        if (phase == GamePhase.COMBAT)
        {
            // TODO: set combat mode
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
