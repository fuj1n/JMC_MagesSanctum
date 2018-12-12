using UnityEngine;

public class EyeLaser : MonoBehaviour
{
    public Camera source;

    private void Update()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(source.ViewportPointToRay(new Vector2(0.5F, 0.5F)), out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Tile"))
            {
                hitInfo.collider.GetComponent<HexTile>()?.Ping();
            }
        }
    }
}
