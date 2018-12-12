using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10F;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;

        rb.velocity = transform.rotation * new Vector3(horizontal, 0F, vertical).normalized * speed + Vector3.up * rb.velocity.y;
    }

    private bool BuildUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Open build menu and freeze player
            return true;
        }

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
