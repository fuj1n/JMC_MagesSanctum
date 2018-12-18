using UnityEngine;

public class EmissiveOscillator : MonoBehaviour
{
    public float frequency = 1F;
    public float amplitude = 2F;
    public float offset = .5F;

    [ColorUsage(false)]
    public Color min;
    [ColorUsage(false)]
    public Color max;

    public Renderer render;

    private float time;

    private void Update()
    {
        float val = offset + Mathf.Sin(time) * amplitude;

        render.material.SetColor("_EmissionColor", Color.Lerp(min, max, val));

        time += Time.deltaTime * frequency;

        while (time > 2 * Mathf.PI)
            time -= 2 * Mathf.PI;
    }
}
