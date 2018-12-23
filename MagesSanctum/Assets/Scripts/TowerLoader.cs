using System.Linq;
using UnityEngine;

public class TowerLoader : MonoBehaviour
{
    public const string TOWERS_FOLDER = "Prefabs/Towers";


    public TowerDisplay towerDisplayTemplate;

    private void Awake()
    {
        Debug.Assert(towerDisplayTemplate, "Tower Display Template not provided");
        if (!towerDisplayTemplate)
            return;

        TowerBase[] towers = Resources.LoadAll<TowerBase>(TOWERS_FOLDER).OrderBy(x => x.towerCost).ToArray();


        foreach (TowerBase t in towers)
        {
            TowerDisplay display = Instantiate(towerDisplayTemplate, transform);

            {
                RadioSelect rad = display.GetComponent<RadioSelect>();

                if (rad)
                    rad.additionalData = t;
            }

            display.Load(t);
        }
    }
}
