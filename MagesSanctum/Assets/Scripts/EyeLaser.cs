using UnityEngine;

public class EyeLaser : MonoBehaviour
{
    public static EyeLaser Instance { get; private set; }

    public Camera source;
    public GameObject hoveredTowerCanvas;
    public Vector3 hoveredTowerOffset;

    public HexTile SelectedTile { get; private set; }

    private string[] displayFormats;

    private void Awake()
    {
        Instance = this;

        if (hoveredTowerCanvas)
            displayFormats = TowerLoader.GetFormats(hoveredTowerCanvas.transform.GetChild(0).gameObject);
    }

    private void Update()
    {
        SelectedTile = null;

        RaycastHit hitInfo;

        if (Physics.Raycast(source.ViewportPointToRay(new Vector2(0.5F, 0.5F)), out hitInfo) && GameManager.Instance.Phase == GamePhase.BUILD)
        {
            if (hitInfo.collider.CompareTag("Tile") || hitInfo.collider.CompareTag("Tower"))
            {
                SelectedTile = hitInfo.collider.GetComponentInParent<HexTile>();
                SelectedTile?.Ping();
            }
        }

        if (!hoveredTowerCanvas)
            return;

        if (hitInfo.collider && hitInfo.collider.CompareTag("Tower"))
        {
            TowerBase t = hitInfo.collider.GetComponent<TowerBase>();

            if (!t && hoveredTowerCanvas.activeInHierarchy)
                hoveredTowerCanvas.SetActive(false);

            if (t)
            {
                if (!hoveredTowerCanvas.activeInHierarchy)
                    hoveredTowerCanvas.SetActive(true);

                TowerLoader.UpdateTowerDisplay(t, hoveredTowerCanvas.transform.GetChild(0).gameObject, displayFormats);
                hoveredTowerCanvas.transform.position = hitInfo.point + hoveredTowerOffset;
                hoveredTowerCanvas.transform.forward = -hitInfo.normal;
            }
        }
        else if (hoveredTowerCanvas.activeInHierarchy)
        {
            hoveredTowerCanvas.SetActive(false);
        }
    }
}
