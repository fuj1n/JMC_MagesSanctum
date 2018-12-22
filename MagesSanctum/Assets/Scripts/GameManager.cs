using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string ENEMY_PATH = "Prefabs/Enemies";
    public static GameManager Instance { get; private set; }

    [Header("Spawning")]
    public int initialEnemyCount = 5;
    public float timeBetweenEnemies = 1.5F;
    [Space]
    [Range(0F, 5F)]
    public float enemyCountIncreasePercent = .5F;
    public float timeBetweenEnemiesDecreaseRate = .2F;

    [Header("Stats")]
    public float maxCoreHealth;

    [HideInInspector]
    public float coreHealth = 1F;

    [System.NonSerialized]
    public bool requireCursor = false;

    public GamePhase Phase
    {
        get
        {
            return _phase;
        }
        set
        {
            _phase = value;
            EventBus.Post(new EventGamePhaseChanged(value));
        }
    }
    private GamePhase _phase;
    public int Wave { get; private set; }

    private int enemiesToSpawn;
    private float enemyTimer;

    private Enemy[] enemyTypes;
    private EnemySpawner[] spawners;

    private int enemyCount;

    private void Awake()
    {
        coreHealth = 1F;

        EventBus.Register(this);

        Instance = this;

        spawners = FindObjectsOfType<EnemySpawner>();
        enemyTypes = Resources.LoadAll<Enemy>(ENEMY_PATH);

        Debug.Assert(enemyTypes != null && enemyTypes.Length > 0, "No enemies found, spawn routine will error");
    }

    private void Start()
    {
        EnterBuildPhase();
    }

    private void Update()
    {
        Cursor.lockState = requireCursor ? CursorLockMode.None : CursorLockMode.Locked;

        if (Phase == GamePhase.COMBAT && enemiesToSpawn > 0)
        {
            enemyTimer -= Time.deltaTime;

            if (enemyTimer <= 0F)
            {
                EventBus.Post(new EventEnemy.SpawnClock(enemyTypes[Random.Range(0, enemyTypes.Length)]));
                enemyTimer = Mathf.Max(.5F, timeBetweenEnemies - timeBetweenEnemiesDecreaseRate * Wave);
                enemiesToSpawn--;
            }
        }
    }

    public void EnterBuildPhase()
    {
        foreach (EnemySpawner spawner in spawners)
            spawner.isActive = false;

        int activeCount = Random.Range(1, spawners.Length);

        for (int i = 0; i < activeCount; i++)
            spawners[Random.Range(0, spawners.Length)].isActive = true;

        Phase = GamePhase.BUILD;
    }

    public void EnterCombatPhase()
    {
        enemiesToSpawn = Mathf.RoundToInt(initialEnemyCount + initialEnemyCount * enemyCountIncreasePercent * Wave);

        Wave++;
        Phase = GamePhase.COMBAT;
    }

    public void EnemyRemoved()
    {
        enemyCount--;

        if (enemyCount <= 0 && enemiesToSpawn <= 0)
        {
            enemyCount = 0;

            FindObjectsOfType<Enemy>().ToList().ForEach(e => Destroy(e.gameObject));
            EnterBuildPhase();
        }
    }

    [SubscribeEvent]
    public void EnemySpawned(EventEnemy.Spawned e)
    {
        enemyCount++;
    }

    [SubscribeEvent]
    public void EnemyDied(EventEnemy.Died e) => EnemyRemoved();

    [SubscribeEvent]
    public void EnemyPassed(EventEnemy.Passed e)
    {
        coreHealth -= e.damage / maxCoreHealth;

        if (coreHealth <= 0F)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // TODO proper loss

            return;
        }

        EnemyRemoved();
    }
}

public enum GamePhase
{
    BUILD,
    COMBAT
}