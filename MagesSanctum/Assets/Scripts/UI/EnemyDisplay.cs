using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI healthField;
    public Image healthFill;
    public TextMeshProUGUI damageField;
    public TextMeshProUGUI speedField;
    public TextMeshProUGUI rewardField;

    private string healthFormat;
    private string damageFormat;
    private string speedFormat;
    private string rewardFormat;

    private bool init;

    public string Name
    {
        set
        {
            nameField.text = value;
        }
    }

    public float Damage
    {
        set
        {
            Awake();
            if (damageField)
                damageField.text = string.Format(damageFormat, value.ToString("N2"));
        }
    }

    public float Speed
    {
        set
        {
            Awake();
            if (speedField)
                speedField.text = string.Format(speedFormat, value.ToString("N2"));
        }
    }

    public int Reward
    {
        set
        {
            Awake();
            if (rewardField)
                rewardField.text = string.Format(rewardFormat, value.ToString("N2"));
        }
    }

    public void SetHealth(float percent, float max)
    {
        Awake();

        percent = Mathf.Clamp01(percent);

        if (healthField)
            healthField.text = string.Format(healthFormat, Mathf.RoundToInt(percent * max), Mathf.RoundToInt(max));
        if (healthFill)
            healthFill.fillAmount = percent;
    }

    private void Awake()
    {
        if (init)
            return;

        init = true;

        if (healthField)
            healthFormat = healthField.text;
        if (damageField)
            damageFormat = damageField.text;
        if (speedField)
            speedFormat = speedField.text;
        if (rewardField)
            rewardFormat = rewardField.text;
    }

    public void Load(Enemy enemy)
    {
        if (!enemy)
            return;

        Name = string.IsNullOrWhiteSpace(enemy.friendlyName) ? enemy.name : enemy.friendlyName;
        Damage = enemy.damage;
        Speed = enemy.speed;
        Reward = enemy.coinReward;

        SetHealth(enemy.GetHealth(), enemy.maxHealth);
    }
}
