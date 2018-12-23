using UnityEngine;

public class EyeLaser : MonoBehaviour
{
    public static EyeLaser Instance { get; private set; }

    public Camera source;
    public TowerDisplay hoveredTowerDisplay;
    public EnemyDisplay hoveredEnemyDisplay;
    public Vector3 hoverDisplayOffset;

    public HexTile SelectedTile { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SelectedTile = null;

        if (Physics.Raycast(source.ViewportPointToRay(new Vector2(0.5F, 0.5F)), out RaycastHit hitInfo) && GameManager.Instance.Phase == GamePhase.BUILD)
        {
            if (hitInfo.collider.CompareTag("Tile") || hitInfo.collider.CompareTag("Tower"))
            {
                SelectedTile = hitInfo.collider.GetComponentInParent<HexTile>();
                SelectedTile?.Ping();
            }
        }

        if (hoveredTowerDisplay)
        {
            if (hitInfo.collider && hitInfo.collider.CompareTag("Tower"))
            {
                TowerBase t = hitInfo.collider.GetComponent<TowerBase>();

                if (!t && hoveredTowerDisplay.transform.parent.gameObject.activeInHierarchy)
                    hoveredTowerDisplay.transform.parent.gameObject.SetActive(false);

                if (t)
                {
                    if (!hoveredTowerDisplay.transform.parent.gameObject.activeInHierarchy)
                        hoveredTowerDisplay.transform.parent.gameObject.SetActive(true);

                    hoveredTowerDisplay.Load(t);
                    hoveredTowerDisplay.transform.parent.position = hitInfo.point + hoverDisplayOffset;
                    hoveredTowerDisplay.transform.parent.forward = -hitInfo.normal;
                }
            }
            else if (hoveredTowerDisplay.transform.parent.gameObject.activeInHierarchy)
            {
                hoveredTowerDisplay.transform.parent.gameObject.SetActive(false);
            }
        }

        if (hoveredEnemyDisplay)
        {
            if (hitInfo.collider && hitInfo.collider.CompareTag("Enemy"))
            {
                Enemy e = hitInfo.collider.GetComponent<Enemy>();

                if (!e && hoveredEnemyDisplay.transform.parent.gameObject.activeInHierarchy)
                    hoveredEnemyDisplay.transform.parent.gameObject.SetActive(false);

                if (e)
                {
                    if (!hoveredEnemyDisplay.transform.parent.gameObject.activeInHierarchy)
                        hoveredEnemyDisplay.transform.parent.gameObject.SetActive(true);

                    hoveredEnemyDisplay.Load(e);
                    hoveredEnemyDisplay.transform.parent.position = hitInfo.point + hoverDisplayOffset;
                    hoveredEnemyDisplay.transform.parent.forward = -hitInfo.normal;
                }
            }
            else if (hoveredEnemyDisplay.transform.parent.gameObject.activeInHierarchy)
            {
                hoveredEnemyDisplay.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
