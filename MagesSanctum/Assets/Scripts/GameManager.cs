﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool requireCursor = false;

    public GamePhase Phase
    {
        get
        {
            return phase;
        }
        set
        {
            phase = value;
            EventBus.Post(new EventGamePhaseChanged(value));
        }
    }
    private GamePhase phase;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Phase = GamePhase.BUILD;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Phase = GamePhase.COMBAT; // TEMP

        Cursor.lockState = requireCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}

public enum GamePhase
{
    BUILD,
    COMBAT
}