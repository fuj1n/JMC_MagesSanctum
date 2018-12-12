using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool requireCursor = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Cursor.lockState = requireCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
