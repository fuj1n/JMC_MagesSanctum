using UnityEngine;

public class SetHDR : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color color;

    private void Awake()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Mesh m = GetComponent<MeshFilter>()?.sharedMesh;

        if (m)
            Gizmos.DrawMesh(m, transform.position, transform.rotation, transform.lossyScale);
    }
}
