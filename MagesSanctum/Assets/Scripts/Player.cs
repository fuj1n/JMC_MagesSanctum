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
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;

        rb.velocity = transform.rotation * new Vector3(horizontal, 0F, vertical).normalized * speed + Vector3.up * rb.velocity.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
