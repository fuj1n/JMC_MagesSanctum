using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("Base Tower Properties")]
    public int towerCost;
    public float fireRate;
    public int shotDamage;
    public float shotSpeed;
    public bool blocksNavigation;

    public bool canFire = true;

    public Transform fireAnchor;
    public GameObject bulletTemplate;

    [Header("Base Tower Display")]
    public Sprite icon;
    public string friendlyName;

    protected float timer;

    protected virtual void Start()
    {
        if (canFire)
        {
            Debug.Assert(fireAnchor, "Fire anchor not provided for tower: " + name);
            Debug.Assert(bulletTemplate, "Bullet template not provided for tower: " + name);
        }
    }

    protected virtual void Update()
    {
        if (!canFire)
            return;

        timer += Time.deltaTime;

        if (timer > fireRate / 60F)
        {
            timer = 0F;

            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        if (!fireAnchor || !bulletTemplate)
            return;


    }
}
