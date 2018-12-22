using DG.Tweening;
using UnityEngine;

public class PowerCoreShrink : MonoBehaviour
{
    public Transform anchor;

    private float cachedHealth;

    private float scaleScalar;

    private void Start()
    {
        cachedHealth = GameManager.Instance.coreHealth;

        if (!anchor)
            anchor = transform;

        scaleScalar = anchor.localScale.x;
    }

    private void Update()
    {
        float health = GameManager.Instance.coreHealth;
        if (!Mathf.Approximately(cachedHealth, health))
        {
            cachedHealth = health;
            anchor.DOScale(Mathf.Max(.05F, cachedHealth) * scaleScalar, .25F).SetEase(Ease.InOutElastic);
        }
    }
}
