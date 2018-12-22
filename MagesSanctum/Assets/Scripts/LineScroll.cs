using UnityEngine;

public class LineScroll : MonoBehaviour
{
    public float speed;

    private float time;
    private LineRenderer render;

    private void Awake()
    {
        render = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (speed > 0)
            while (time * speed > 1)
                time -= 1F / speed;
        else if (speed < 0)
            while (time * speed < -1)
                time += 1F / speed;

        render.material.SetTextureOffset("_MainTex", Vector2.right * time * speed);
        time += Time.deltaTime;
    }
}
