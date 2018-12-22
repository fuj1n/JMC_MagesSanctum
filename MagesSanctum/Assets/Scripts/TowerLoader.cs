using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerLoader : MonoBehaviour
{
    public const string TOWERS_FOLDER = "Prefabs/Towers";
    public const string MISSINGNO_PATH = "Sprites/missingno";
    private static Sprite missingno;

    public GameObject towerDisplayTemplate;

    private void Awake()
    {
        Debug.Assert(towerDisplayTemplate, "Tower Display Template not provided");
        if (!towerDisplayTemplate)
            return;

        TowerBase[] towers = Resources.LoadAll<TowerBase>(TOWERS_FOLDER).OrderBy(x => x.towerCost).ToArray();


        foreach (TowerBase t in towers)
        {
            GameObject display = Instantiate(towerDisplayTemplate, transform);

            {
                RadioSelect rad = display.GetComponent<RadioSelect>();

                if (rad)
                    rad.additionalData = t;
            }

            UpdateTowerDisplay(t, display);
        }
    }

    public static string[] GetFormats(GameObject display)
    {
        List<string> formats = new List<string>();

        {
            TextMeshProUGUI text = display.transform.Find("Cost")?.GetComponent<TextMeshProUGUI>();

            if (text)
                formats.Add(text.text);
        }
        {
            TextMeshProUGUI text = display.transform.Find("FireRate")?.GetComponent<TextMeshProUGUI>();

            if (text)
                formats.Add(text.text);
        }
        {
            TextMeshProUGUI text = display.transform.Find("ShotDamage")?.GetComponent<TextMeshProUGUI>();

            if (text)
                formats.Add(text.text);
        }
        {
            TextMeshProUGUI text = display.transform.Find("ShotSpeed")?.GetComponent<TextMeshProUGUI>();

            if (text)
                formats.Add(text.text);
        }

        return formats.ToArray();
    }

    public static void UpdateTowerDisplay(TowerBase t, GameObject display, string[] formats = null)
    {
        if (formats == null)
            formats = GetFormats(display);

        if (!missingno)
            missingno = Resources.Load<Sprite>(MISSINGNO_PATH);

        int fmt = 0;

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
                text.text = string.Format(formats[fmt++], t.towerCost);
        }
        {
            TextMeshProUGUI text = display.transform.Find("FireRate")?.GetComponent<TextMeshProUGUI>();

            if (text)
                text.text = string.Format(formats[fmt++], t.fireRate.ToString("N2"));
        }
        {
            TextMeshProUGUI text = display.transform.Find("ShotDamage")?.GetComponent<TextMeshProUGUI>();

            if (text)
                text.text = string.Format(formats[fmt++], t.shotDamage);
        }
        {
            TextMeshProUGUI text = display.transform.Find("ShotSpeed")?.GetComponent<TextMeshProUGUI>();

            if (text)
                text.text = string.Format(formats[fmt++], t.shotSpeed.ToString("N2"));
        }
    }
}
