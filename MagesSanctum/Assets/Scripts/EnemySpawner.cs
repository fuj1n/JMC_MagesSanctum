using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public HexBuilder builder;
    public Transform endTarget;

    public Vector3 enemySpawnOffset;

    public int startY;

    public bool isActive;

    [Header("Line Renderer")]
    public Gradient activeColor;
    public Gradient inactiveColor;
    public Vector3 lineRenderOffset;

    private List<Vector2Int> path;

    private LineRenderer lineRender;

    private void Awake()
    {
        EventBus.Register(this);
        lineRender = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (lineRender)
            lineRender.colorGradient = isActive ? activeColor : inactiveColor;
    }

    private void UpdateLineRender()
    {
        if (!lineRender)
            return;

        Vector3[] points = GetPath();

        lineRender.positionCount = points.Length + 1;
        lineRender.SetPositions(points.Prepend(transform.position + enemySpawnOffset).Select(x => x + lineRenderOffset).ToArray());
        if (endTarget)
        {
            lineRender.positionCount++;

            lineRender.SetPosition(lineRender.positionCount - 1, endTarget.position.Set(Utils.Axis.Y, transform.position.y + enemySpawnOffset.y) + lineRenderOffset);
        }
    }

    [SubscribeEvent]
    public void EnemyClock(EventEnemy.SpawnClock e)
    {
        if (!isActive)
            return;

        if (!e.template)
            return;

        Enemy en = Instantiate(e.template, transform.position + enemySpawnOffset, e.template.transform.rotation);
        if (en)
        {
            en.path = GetPath();

            if (endTarget)
            {
                en.finalTarget = endTarget.position.Set(Utils.Axis.Y, transform.position.y + enemySpawnOffset.y);
            }
        }
    }

    [SubscribeEvent]
    public void WorldChange(EventWorldChanged e)
    {
        if (!builder)
        {
            Debug.LogWarning("Hex Builder not provided to spawner: " + name + ", using first responder: " + e.parent.name);
            builder = e.parent;
        }

        if (e.parent == builder)
        {
            path = CalculateAI(builder, startY);
            UpdateLineRender();
        }
    }

    public Vector3[] GetPath()
    {
        return path.Select(val =>
        {
            Vector3 pos = builder.world[val.x, val.y].transform.position + enemySpawnOffset;
            pos.y = transform.position.y + enemySpawnOffset.y;
            return pos;
        }).ToArray();
    }

    private void OnValidate()
    {
        if (lineRender)
            lineRender.colorGradient = activeColor;
    }

    #region Enemy AI
    public static List<Vector2Int> CalculateAI(HexBuilder builder, int startY, Vector2Int? newTile = null)
    {
        Debug.Assert(builder?.world != null, "Hex Builder World not yet initialized");

        bool[,] world = new bool[builder.world.GetLength(0), builder.world.GetLength(1)];
        bool[,] isPossible = new bool[world.GetLength(0), world.GetLength(1)];

        for (int x = 0; x < world.GetLength(0); x++)
            for (int y = 0; y < world.GetLength(1); y++)
                world[x, y] = !builder.world[x, y] || builder.world[x, y].BlocksNavigation();

        if (newTile != null && newTile.Value.x < world.GetLength(0) && newTile.Value.y < world.GetLength(1))
            world[newTile.Value.x, newTile.Value.y] = true;

        for (int x = 0; x < world.GetLength(0); x++)
        {
            for (int y = 0; y < world.GetLength(1); y++)
            {
                if (x == 0)
                {
                    isPossible[x, y] = !world[x, y];
                }
                else
                {
                    isPossible[x, y] = !world[x, y] && isPossible[x - 1, y];
                }
            }
        }

        for (int pass = 0; pass < isPossible.GetLength(1); pass++)
        {
            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    if (x == 0 || isPossible[x, y])
                        continue;
                    else
                    {
                        isPossible[x, y] = !world[x, y] && (isPossible[x - 1, y] || (y > 0 && isPossible[x, y - 1]) || (y < isPossible.GetLength(1) - 1 && isPossible[x, y + 1]));
                    }
                }
            }
        }

        bool isPossibleAtAll = false;
        for (int y = 0; y < world.GetLength(1); y++)
        {
            if (isPossible[isPossible.GetLength(0) - 1, y])
            {
                isPossibleAtAll = true;
                break;
            }
        }

        List<Vector2Int> aiPoints = new List<Vector2Int>();

        int[,] numbers = new int[isPossible.GetLength(0) + 1, isPossible.GetLength(1)];

        if (isPossibleAtAll)
        {
            for (int gx = 0; gx < numbers.GetLength(0); gx++)
                for (int y = 0; y < numbers.GetLength(1); y++)
                    numbers[gx, y] = 25565;

            Queue<Vector2Int> pointQ = new Queue<Vector2Int>();
            for (int y = startY; y >= 0; y--)
            {
                numbers[0, y] = startY - y;
                pointQ.Enqueue(new Vector2Int(0, y));
            }
            for (int y = startY + 1; y < numbers.GetLength(1); y++)
            {
                numbers[0, y] = y - startY;
                pointQ.Enqueue(new Vector2Int(0, y));
            }

            while (pointQ.Count > 0)
                ProcessPoint(pointQ, isPossible, numbers, pointQ.Dequeue());

            int shortestY = 0;
            for (int y = 1; y < numbers.GetLength(1); y++)
            {
                if (numbers[numbers.GetLength(0) - 1, y] < numbers[numbers.GetLength(0) - 1, shortestY])
                {
                    shortestY = y;
                }
            }

            int py = shortestY;
            int x = numbers.GetLength(0) - 1;
            List<Vector2Int> path = new List<Vector2Int>();

            while (numbers[x, py] != 0)
            {
                int lowest = numbers[x, py];
                int lx = x, ly = py;

                foreach (Cardinal side in Cardinal.SIDES)
                {
                    if (side.chk(numbers, x, py))
                    {
                        if (numbers[x + side.x, py + side.y] < lowest)
                        {
                            lowest = numbers[x + side.x, py + side.y];
                            lx = x + side.x;
                            ly = py + side.y;
                            break;
                        }
                    }
                }

                path.Add(new Vector2Int(x - 1, py));
                if (path.Count > numbers.Length)
                    break;
                x = lx;
                py = ly;
            }

            path.Reverse();
            aiPoints.AddRange(path);
            aiPoints.Add(new Vector2Int(aiPoints[aiPoints.Count - 1].x + 1, aiPoints[aiPoints.Count - 1].y));

            return aiPoints.Where(val => val.x >= 0 && val.y >= 0 && val.x < world.GetLength(0) && val.y < world.GetLength(1)).ToList();
        }

        return null;
    }

    private static void ProcessPoint(Queue<Vector2Int> queue, bool[,] possibility, int[,] grid, Vector2Int p)
    {
        int num = grid[p.x, p.y];

        foreach (Cardinal side in Cardinal.SIDES)
        {
            if (side.chk(grid, p.x, p.y) && grid[p.x + side.x, p.y + side.y] > num + 1)
            {
                if (p.x + side.x >= 1 && possibility[p.x + side.x - 1, p.y + side.y])
                {
                    grid[p.x + side.x, p.y + side.y] = num + 1;
                    queue.Enqueue(new Vector2Int(p.x + side.x, p.y + side.y));
                }
            }
        }
    }

    public class Cardinal
    {
        public static readonly Cardinal UP = new Cardinal((a, x, y) => y > 0, 0, -1);
        public static readonly Cardinal DOWN = new Cardinal((a, x, y) => y < a.GetLength(1) - 1, 0, 1);
        public static readonly Cardinal LEFT = new Cardinal((a, x, y) => x > 0, -1, 0);
        public static readonly Cardinal RIGHT = new Cardinal((a, x, y) => x < a.GetLength(0) - 1, 1, 0);

        public static readonly Cardinal[] SIDES = { UP, DOWN, LEFT, RIGHT };

        public delegate bool BoundCheck(int[,] a, int x, int y);

        public readonly BoundCheck chk;
        public readonly int x, y;
        private Cardinal(BoundCheck chk, int x, int y)
        {
            this.chk = chk;

            this.x = x;
            this.y = y;
        }
    }
    #endregion
}
