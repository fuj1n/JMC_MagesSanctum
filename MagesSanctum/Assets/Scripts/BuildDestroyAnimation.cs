using DG.Tweening;
using UnityEngine;

public class BuildDestroyAnimation : MonoBehaviour
{
    public float animationTime = 2F;

    private bool isDestroying = false;

    private void Awake()
    {
        Vector3 scale = transform.localScale;

        transform.localScale = Vector3.one * 0.01F;
        transform.DOScale(scale, animationTime);
    }

    public bool DoDestroy()
    {
        if (isDestroying)
            return false;

        isDestroying = true;
        transform.DOScale(Vector3.one * 0.01F, animationTime).OnComplete(() => Destroy(gameObject));
        return true;
    }

}
