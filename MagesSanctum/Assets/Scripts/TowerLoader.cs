using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerLoader : MonoBehaviour
{
    public const string TOWERS_FOLDER = "Prefabs/Towers";

    public Sprite missingno;
    public GameObject towerDisplayTemplate;

    private void Awake()
    {
        Debug.Assert(towerDisplayTemplate, "Tower Display Template not provided");
        if (!towerDisplayTemplate)
            return;

        TowerBase[] towers = Resources.LoadAll<GameObject>(TOWERS_FOLDER).Select(obj => obj.GetComponent<TowerBase>()).Where(b => b).ToArray();


        foreach (TowerBase t in towers)
        {
            GameObject display = Instantiate(towerDisplayTemplate, transform);

            {
                Image img = display.transform.Find("Image")?.GetComponent<Image>();

                if (img)
                    img.sprite = t.icon ? t.icon : missingno;
            }

            {
                TextMeshProUGUI text = display.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();

                if (text)
                    text.text = string.IsNullOrWhiteSpace(t.friendlyName) ? t.name : t.friendlyName;
            }
            {
                TextMeshProUGUI text = display.transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();

                if (text)
                    text.text = string.Format(text.text, t.towerCost);
            }
            {
                TextMeshProUGUI text = display.transform.Find("FireRate")?.GetComponent<TextMeshProUGUI>();

                if (text)
                    text.text = string.Format(text.text, t.fireRate.ToString("N2"));
            }
            {
                TextMeshProUGUI text = display.transform.Find("ShotDamage")?.GetComponent<TextMeshProUGUI>();

                if (text)
                    text.text = string.Format(text.text, t.shotDamage);
            }
            {
                TextMeshProUGUI text = display.transform.Find("ShotSpeed")?.GetComponent<TextMeshProUGUI>();

                if (text)
                    text.text = string.Format(text.text, t.shotSpeed.ToString("N2"));
            }


        }
    }
}
