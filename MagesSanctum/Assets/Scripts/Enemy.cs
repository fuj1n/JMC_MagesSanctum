using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string friendlyName;

    [Header("Speed")]
    [Tooltip("Enemy speed in m/s")]
    public float speed = 2F;
    public float finalSpeedMult = 2F;
    public float rotateTime = .5F;

    [Header("Stats")]
    public float damage;
    public float maxHealth;
    public int coinReward;

    [HideInInspector]
    public Vector3[] path;
    [HideInInspector]
    public Vector3? finalTarget;

    private int pathEntry = -1;

    private bool navDone = false;

    private float health = 1F;

    private void Awake()
    {
        EventBus.Post(new EventEnemy.Spawned());
    }

    private void Start()
    {
        if (path == null || path.Length == 0)
        {
            Debug.LogError("No path given");
            return;
        }

        Scale();
        Next();
    }

    public float GetHealth() => health;

    public void Damage(float damage)
    {
        health -= damage / maxHealth;

        if (health <= 0F)
        {
            health = 0F;
            Die();
        }
    }

    private void Next()
    {
        if (navDone)
            return;

        pathEntry++;

        Vector3? target;

        if (pathEntry < path.Length)
        {
            target = path[pathEntry];
            transform.DOMove(path[pathEntry], transform.position.GetMoveTime(path[pathEntry], speed)).OnComplete(() => Next()).SetEase(Ease.Linear);
        }
        else
        {
            navDone = true;

            target = finalTarget;

            if (finalTarget.HasValue)
                transform.DOMove(finalTarget.Value, transform.position.GetMoveTime(finalTarget.Value, speed * finalSpeedMult)).OnComplete(() => NavDone()).SetEase(Ease.Linear);
        }

        if (target.HasValue)
            transform.DOLookAt(target.Value, rotateTime, AxisConstraint.Y);
    }

    private void NavDone()
    {
        EventBus.Post(new EventEnemy.Passed(damage));
        Destroy(gameObject);
    }

    private void Die()
    {
        EventBus.Post(new EventEnemy.Died(coinReward));
        Destroy(gameObject);
    }

    public void Scale()
    {
        //TODO enemies get tougher the longer you play
    }
}
