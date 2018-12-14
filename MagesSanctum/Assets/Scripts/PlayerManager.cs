using Rewired;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 10F;

    private Rigidbody rb;

    private Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = ReInput.players.GetPlayer(0);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
