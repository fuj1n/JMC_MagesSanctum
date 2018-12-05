using UnityEngine;

public class HexBuilder : MonoBehaviour
{
    public GameObject template;
    public Vector2Int hexelCount;

    private void Start()
    {
        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;

        if (!mesh)
            return;

        Vector3 size = mesh.bounds.size;
        for (int x = 0; x < hexelCount.x; x++)
        {
            for (int y = 0; y < hexelCount.y; y++)
            {
                GameObject go = Instantiate(template, transform);
                go.transform.position = transform.position + new Vector3(x * size.x + (y % 2 * size.x / 2), 0F, y * size.z * .75F);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;

        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;

        if (!mesh)
            return;

        Vector3 size = mesh.bounds.size;

        for (int x = 0; x < hexelCount.x; x++)
        {
            for (int y = 0; y < hexelCount.y; y++)
            {
                Gizmos.DrawMesh(mesh, transform.position + new Vector3(x * size.x + (y % 2 * size.x / 2), 0F, y * size.z * .75F));
            }
        }
    }

}
