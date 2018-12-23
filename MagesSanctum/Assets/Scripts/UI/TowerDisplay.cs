using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDisplay : MonoBehaviour
{
    public Image imageField;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI costField;
    public TextMeshProUGUI fireRateField;
    public TextMeshProUGUI shotSpeedField;
    public TextMeshProUGUI rangeField;
    public TextMeshProUGUI shotDamageField;

    private string costFormat;
    private string fireRateFormat;
    private string shotSpeedFormat;
    private string rangeFormat;
    private string shotDamageFormat;

    private bool init;

    public string Name
    {
        set
        {
            if (nameField)
                nameField.text = value;
        }
    }

    public Sprite Image
    {
        set
        {
            Sprite img = value;

            if (!img)
                img = UIManager.MissingNo;

            if (imageField)
                imageField.sprite = img;
        }
    }

    public int Cost
    {
        set
        {
            Awake();
            if (costField)
                costField.text = string.Format(costFormat, value);
        }
    }

    public float FireRate
    {
        set
        {
            Awake();
            if (fireRateField)
                fireRateField.text = string.Format(fireRateFormat, value.ToString("N2"));
        }
    }

    public float ShotSpeed
    {
        set
        {
            Awake();
            if (shotSpeedField)
                shotSpeedField.text = string.Format(shotSpeedFormat, value.ToString("N2"));
        }
    }

    public float Range
    {
        set
        {
            Awake();
            if (rangeField)
                rangeField.text = string.Format(rangeFormat, value.ToString("N2"));
        }
    }

    public float ShotDamage
    {
        set
        {
            Awake();
            if (shotDamageField)
                shotDamageField.text = string.Format(shotDamageFormat, value.ToString("N2"));
        }
    }

    private void Awake()
    {
        if (init)
            return;

        init = true;

        if (costField)
            costFormat = costField.text;
        if (fireRateField)
            fireRateFormat = fireRateField.text;
        if (shotSpeedField)
            shotSpeedFormat = shotSpeedField.text;
        if (rangeField)
            rangeFormat = rangeField.text;
        if (shotDamageField)
            shotDamageFormat = shotDamageField.text;
    }

    public void Load(TowerBase tower)
    {
        if (!tower)
            return;

        Name = string.IsNullOrWhiteSpace(tower.friendlyName) ? tower.name : tower.friendlyName;
        Image = tower.icon;
        Cost = tower.towerCost;
        FireRate = tower.fireRate;
        ShotSpeed = tower.shotSpeed;
        Range = tower.range;
        ShotDamage = tower.shotDamage;

    }
}
