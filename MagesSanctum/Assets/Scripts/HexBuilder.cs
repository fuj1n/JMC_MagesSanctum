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
                go.transform.localPosition = new Vector3(x * size.x + (y % 2 * size.x / 2), 0F, y * size.z * .75F);
            }
        }
    }

    private void OnDrawGizmos()
    {
        //if (UnityEditor.EditorApplication.isPlaying)
        //    return;

        Mesh mesh = template?.GetComponent<MeshFilter>()?.sharedMesh;
        Material[] mat = template?.GetComponent<Renderer>()?.sharedMaterials;

        if (!mesh)
            return;
        if (mat == null || mat.Length == 0)
            return;

        Vector3 size = mesh.bounds.size;

        for (int x = 0; x < hexelCount.x; x++)
        {
            for (int y = 0; y < hexelCount.y; y++)
            {
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    mat[i < mat.Length ? i : 0].SetPass(0);

                    Graphics.DrawMeshNow(mesh, transform.rotation * (transform.position + new Vector3(x * size.x + (y % 2 * size.x / 2), 0F, y * size.z * .75F)), transform.rotation, i);
                }
            }
        }
    }

}
