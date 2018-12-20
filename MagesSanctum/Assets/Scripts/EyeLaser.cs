using UnityEngine;

public class EyeLaser : MonoBehaviour
{
    public static EyeLaser Instance { get; private set; }

    public Camera source;

    public HexTile SelectedTile { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SelectedTile = null;

        if (GameManager.Instance.Phase != GamePhase.BUILD)
            return;

        RaycastHit hitInfo;

        if (Physics.Raycast(source.ViewportPointToRay(new Vector2(0.5F, 0.5F)), out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Tile") || hitInfo.collider.CompareTag("Tower"))
            {
                SelectedTile = hitInfo.collider.GetComponentInParent<HexTile>();
                SelectedTile?.Ping();
            }
        }
    }
}
