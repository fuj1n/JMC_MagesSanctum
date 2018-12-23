using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    [HideInInspector]
    public Enemy target;

    private void Awake()
    {
        Invoke("Die", 5F);
    }

    private void Update()
    {
        if (!target)
        {
            Die();
            return;
        }

        transform.LookAt(target.transform);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target.transform)
        {
            target.Damage(damage);

            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
