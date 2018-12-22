using UnityEngine;

public class HexTile : MonoBehaviour
{
    [HideInInspector]
    public HexBuilder parent;
    [HideInInspector]
    public Vector2Int coords;

    [Header("Outlines")]
    public Material buildOutline;
    public Material destroyOutline;
    public Material cantBuildOutline;

    private long tickPinged = 0;
    private long tick = 3;

    private TowerBase tower;

    private PlayerManager player;
    private bool playerInside;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        tick++;

        if (tick - tickPinged > 2 || playerInside)
            return;

        if (GameManager.Instance?.Phase != GamePhase.BUILD)
            return;
        if (!player.destroyTool && this.tower)
            return;

        RadioSelect select = RadioSelect.Controller.GetSelection("BuildMenu.SelectedTower");

        GameObject tower = null;
        int cost = 0;

        if (player.destroyTool && this.tower)
            tower = this.tower.gameObject;
        else if (!player.destroyTool && select && select.additionalData is TowerBase)
        {
            tower = (select.additionalData as TowerBase).gameObject;
            cost = (select.additionalData as TowerBase).towerCost;
        }

        if (!tower)
            return;

        Material mat = player.destroyTool ? destroyOutline : buildOutline;
        if (cost > player.coins && cantBuildOutline)
            mat = cantBuildOutline;
        if (!player.destroyTool && EnemySpawner.CalculateAI(parent, coords.y, coords) == null)
            mat = cantBuildOutline;

        Debug.Assert(mat, "No material specified on " + name);

        foreach (MeshRenderer render in tower.GetComponentsInChildren<MeshRenderer>())
        {
            if (!render.enabled)
                continue;
            MeshFilter filter = render.GetComponent<MeshFilter>();

            if (filter)
            {
                Vector3 position = render.transform.position;
                Quaternion rotation = render.transform.rotation;
                Vector3 scale = render.transform.lossyScale;

                if (!player.destroyTool)
                {
                    scale.Scale(transform.lossyScale);
                    position.Scale(scale);

                    position = transform.position + position;
                    rotation = transform.rotation * rotation;
                }

                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale + Vector3.one * 0.01F);
                Graphics.DrawMesh(filter.sharedMesh, matrix, mat, gameObject.layer);
            }
        }
    }

    public void Ping()
    {
        tickPinged = tick;
    }

    public bool BuildTower(TowerBase obj)
    {
        if (tower || !obj || playerInside || EnemySpawner.CalculateAI(parent, coords.y, coords) == null)
            return false;

        tower = Instantiate(obj, transform);

        EventBus.Post(new EventWorldChanged(parent));
        return true;
    }

    public int DestroyTower()
    {
        if (!tower)
            return 0;

        int towerCost = tower.towerCost;

        BuildDestroyAnimation anim = tower.GetComponent<BuildDestroyAnimation>();
        if (anim)
        {
            if (!anim.DoDestroy())
                return 0;

            tower = null;
        }
        else
            Destroy(tower.gameObject);

        EventBus.Post(new EventWorldChanged(parent));
        return towerCost;
    }

    public bool BlocksNavigation()
    {
        return tower?.blocksNavigation ?? false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerInside = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerInside = false;
    }
}
