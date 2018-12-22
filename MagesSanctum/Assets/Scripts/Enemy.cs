using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Enemy speed in m/s")]
    public float speed = 2F;
    public float finalSpeedMult = 2F;
    public float rotateTime = .5F;

    [HideInInspector]
    public Vector3[] path;
    [HideInInspector]
    public Vector3? finalTarget;

    private int pathEntry = -1;

    private bool navDone = false;

    private void Start()
    {
        if (path == null || path.Length == 0)
        {
            Debug.LogError("No path given");
            return;
        }

        Next();
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
        Destroy(gameObject);
    }
}
