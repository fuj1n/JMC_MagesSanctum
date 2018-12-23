using DG.Tweening;
using System.Linq;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("Base Tower Properties")]
    public int towerCost;
    public float fireRate;
    public int shotDamage;
    public float shotSpeed;
    public float range;
    public bool blocksNavigation;

    public bool canFire = true;

    public Transform fireAnchor;
    public Transform gunPivot;
    public Bullet bulletTemplate;

    [Header("Base Tower Display")]
    public Sprite icon;
    public string friendlyName;

    protected float timer;
    protected Enemy target;

    private Tween lookAtTween;

    protected virtual void Start()
    {
        if (canFire)
        {
            Debug.Assert(fireAnchor, "Fire anchor not provided for tower: " + name);
            Debug.Assert(bulletTemplate, "Bullet template not provided for tower: " + name);
        }

        if (!gunPivot)
            gunPivot = transform;
    }

    protected virtual void Update()
    {
        if (!canFire)
            return;

        timer += Time.deltaTime;

        if (!target || Vector3.Distance(target.transform.position, transform.position) > range * transform.lossyScale.x)
        {
            target = SelectTarget();

            if (target)
                lookAtTween = gunPivot.DOLookAt(target.transform.position, .1F);

            return;
        }

        if (lookAtTween == null || !lookAtTween.IsPlaying())
            gunPivot.LookAt(target.transform);


        if (timer > 1F / fireRate)
        {
            timer = 0F;

            Shoot();
        }
    }

    protected virtual Enemy SelectTarget()
    {
        return Physics.SphereCastAll(transform.position, range * transform.lossyScale.x, Vector3.down, range * transform.lossyScale.x)
               .Where(x => x.collider.CompareTag("Enemy"))
               .Select(x => x.collider.GetComponent<Enemy>())
               .Where(x => x)
               .OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
               .FirstOrDefault();
    }

    protected virtual void Shoot()
    {
        if (!fireAnchor || !bulletTemplate || !target)
            return;

        Bullet bu = Instantiate(bulletTemplate, fireAnchor.position, Quaternion.identity);
        bu.target = target;
        bu.speed = shotSpeed;
        bu.damage = shotDamage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range * transform.lossyScale.x);
    }
}
