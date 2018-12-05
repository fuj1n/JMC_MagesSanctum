using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public GameObject template;
    public int hexelCount;

    public float size = 1F;

    private void OnDrawGizmos()
    {
        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;

        if (!mesh)
            return;

        for (int x = 0; x < hexelCount; x++)
        {
            for (int y = 0; y < hexelCount; y++)
            {
                Gizmos.DrawMesh(mesh, transform.position + new Vector3(x * size, y * size));
            }
        }
    }
}
