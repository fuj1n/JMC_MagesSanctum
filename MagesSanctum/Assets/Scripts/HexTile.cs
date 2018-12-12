using UnityEngine;

public class HexTile : MonoBehaviour
{
    public GameObject outline;
    private long tickPinged = 0;

    private long tick = 3;

    private bool cachedActive;

    private void Update()
    {
        tick++;

        bool active = tick - tickPinged <= 2;

        if (active != cachedActive)
        {
            cachedActive = active;
            outline.SetActive(active);
        }
    }

    public void Ping()
    {
        tickPinged = tick;
    }
}
