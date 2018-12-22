using UnityEngine;

public class HexBuilder : MonoBehaviour
{
    public GameObject template;
    public Vector2Int hexelCount;

    public bool drawFilledPreview = false;

    public HexTile[,] world;

    private void Start()
    {
        world = new HexTile[hexelCount.x, hexelCount.y];

        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;

        if (!mesh)
            return;

        Vector3 size = Vector3.Scale(mesh.bounds.size, template.transform.localScale);

        for (int x = 0; x < hexelCount.x; x++)
        {
            int cx = x - Mathf.FloorToInt(hexelCount.x / 2F);

            for (int y = 0; y < hexelCount.y; y++)
            {
                int cy = y - Mathf.FloorToInt(hexelCount.y / 2F);

                GameObject go = Instantiate(template, transform);
                go.transform.localPosition = new Vector3(cx * size.x + (Mathf.Abs(cy % 2) * size.x / 2), 0F, cy * size.z * .75F);

                world[x, y] = go.GetComponentInChildren<HexTile>();
                if (world[x, y])
                {
                    world[x, y].parent = this;
                    world[x, y].coords = new Vector2Int(x, y);
                }
            }
        }

        EventBus.Post(new EventWorldChanged(this));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;

        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;

        Vector3 scale = Vector3.Scale(transform.lossyScale, template.transform.localScale);
        Vector3 size = Vector3.Scale(mesh.bounds.size, scale);

        Material[] mat = template?.GetComponent<Renderer>()?.sharedMaterials;

        if (drawFilledPreview)
        {

            if (!mesh)
                return;
            if (mat == null || mat.Length == 0)
                return;

            for (int x = Mathf.FloorToInt(-hexelCount.x / 2F); x < Mathf.FloorToInt(hexelCount.x / 2F); x++)
            {
                for (int y = Mathf.FloorToInt(-hexelCount.y / 2F); y < Mathf.FloorToInt(hexelCount.y / 2F); y++)
                {
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        mat[i < mat.Length ? i : 0].SetPass(0);

                        Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(transform.rotation * (transform.position + new Vector3(x * size.x + (Mathf.Abs(y % 2) * size.x / 2), 0F, y * size.z * .75F)), transform.rotation, scale), i);
                    }
                }
            }
        }
        else
        {
            Gizmos.color = Color.magenta;

            for (int x = Mathf.FloorToInt(-hexelCount.x / 2F); x < Mathf.FloorToInt(hexelCount.x / 2F); x++)
            {
                for (int y = Mathf.FloorToInt(-hexelCount.y / 2F); y < Mathf.FloorToInt(hexelCount.y / 2F); y++)
                {
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        if (mat != null && i < mat.Length)
                            Gizmos.color = mat[i].color;
                        Gizmos.DrawWireMesh(mesh, i, transform.rotation * (transform.position + new Vector3(x * size.x + (Mathf.Abs(y % 2) * size.x / 2), 0F, y * size.z * .75F)), transform.rotation, scale);
                    }
                }
            }
        }
    }
#endif
}