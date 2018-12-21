using Rewired;
using Rewired.Integration.Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 10F;

    public GameObject buildMenu;

    [Header("Hands")]
    public Transform effectAnchor;
    public Animator animator;

    [Header("Templates")]
    public GameObject buildModeEffect;
    public GameObject destroyModeEffect;

    [Header("Current Stats")]
    public int coins;

    [HideInInspector]
    public bool destroyTool;

    private Rigidbody rb;
    private Player player;

    private bool buildScreenUp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = ReInput.players.GetPlayer(0);
        EventBus.Register(this);

        // Forces the build menu to initialize as we rely on it
        buildMenu.SetActive(true);
    }

    private void Start()
    {
        buildMenu.SetActive(false);
    }

    private void Update()
    {
        bool skipInput = false;

        switch (GameManager.Instance.Phase)
        {
            case GamePhase.BUILD:
                skipInput = BuildUpdate();
                break;
            case GamePhase.COMBAT:
                skipInput = CombatUpdate();
                break;
        }

        RewiredCinemachineBridge.active = !skipInput;

        if (skipInput)
            return;

        Vector2 movement = player.GetAxis2D("Move X", "Move Z");
        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;
        rb.velocity = transform.rotation * new Vector3(movement.x, 0F, movement.y).normalized * speed + Vector3.up * rb.velocity.y;
    }

    private bool BuildUpdate()
    {
        if (player.GetButtonDown("Toggle Build Menu"))
        {
            buildScreenUp = !buildScreenUp;
            GameManager.Instance.requireCursor = buildScreenUp;

            buildMenu.SetActive(buildScreenUp);
            return true;
        }

        if (buildScreenUp)
            return true;

        if (player.GetButtonDown("Next Weapon/Tool") || player.GetButtonDown("Previous Weapon/Tool"))
        {
            destroyTool = !destroyTool;
            UpdateEffect();
            UpdateAnimator();
        }


        if (player.GetButtonDown("Build"))
        {
            if (destroyTool)
            {
                HexTile tile = EyeLaser.Instance.SelectedTile;

                if (tile)
                    coins += Mathf.RoundToInt(tile.DestroyTower() * .85F);
            }
            else
            {
                RadioSelect selection = RadioSelect.Controller.GetSelection("BuildMenu.SelectedTower");

                if (selection)
                {
                    Debug.Assert(selection.additionalData is TowerBase, "Selection data of " + selection + "is of wrong type: " + selection.additionalData?.GetType());

                    HexTile tile = EyeLaser.Instance.SelectedTile;

                    if (tile)
                    {
                        TowerBase tower = selection.additionalData as TowerBase;

                        if (coins >= tower.towerCost && tile.BuildTower(selection.additionalData as TowerBase))
                            coins -= tower.towerCost;
                    }
                }
            }
        }

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

        buildScreenUp = false;
        buildMenu.SetActive(false);
        GameManager.Instance.requireCursor = false;
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
            if (!destroyTool && buildModeEffect)
                Instantiate(buildModeEffect, effectAnchor);
            else if (destroyTool && destroyModeEffect)
                Instantiate(destroyModeEffect, effectAnchor);
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

    public string GetToolName()
    {
        if (GameManager.Instance.Phase == GamePhase.BUILD)
        {
            return destroyTool ? "Destroy Tool" : "Build Tool";
        }
        else
        {
            return "Uncharted Territory";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
