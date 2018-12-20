﻿using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Material buildOutline;
    public Material destroyOutline;

    private long tickPinged = 0;
    private long tick = 3;

    private TowerBase tower;

    private PlayerManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        tick++;

        if (tick - tickPinged > 2)
            return;

        if (GameManager.Instance?.Phase != GamePhase.BUILD)
            return;
        if (!player.destroyTool && this.tower)
            return;

        RadioSelect select = RadioSelect.Controller.GetSelection("BuildMenu.SelectedTower");

        GameObject tower = null;

        if (player.destroyTool && this.tower)
            tower = this.tower.gameObject;
        else if (!player.destroyTool && select && select.additionalData is TowerBase)
            tower = (select.additionalData as TowerBase).gameObject;

        if (!tower)
            return;

        Material mat = player.destroyTool ? destroyOutline : buildOutline;

        Debug.Assert(mat, "No material specified on " + name);

        foreach (MeshRenderer render in tower.GetComponentsInChildren<MeshRenderer>())
        {
            if (!render.enabled)
                continue;
            MeshFilter filter = render.GetComponent<MeshFilter>();

            if (filter)
            {
                Vector3 position = render.transform.position;
                Quaternion rotation = render.transform.rotation;
                Vector3 scale = render.transform.lossyScale;

                if (!player.destroyTool)
                {
                    scale.Scale(transform.lossyScale);
                    position.Scale(scale);

                    position = transform.position + position;
                    rotation = transform.rotation * rotation;
                }

                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale + Vector3.one * 0.01F);
                Graphics.DrawMesh(filter.sharedMesh, matrix, mat, gameObject.layer);
            }
        }
    }

    public void Ping()
    {
        tickPinged = tick;
    }

    public bool BuildTower(TowerBase obj)
    {
        if (this.tower || !obj)
            return false;

        TowerBase tower = Instantiate(obj, transform);
        this.tower = tower;

        return true;
    }

    public int DestroyTower()
    {
        if (!tower)
            return 0;

        int towerCost = tower.towerCost;

        Destroy(tower.gameObject);
        return towerCost;
    }

    public bool BlocksNavigation()
    {
        return tower?.blocksNavigation ?? false;
    }
}
