using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public const string MISSINGNO_PATH = "Sprites/missingno";

    public static Sprite MissingNo
    {
        get
        {
            if (!missingno)
                missingno = Resources.Load<Sprite>(MISSINGNO_PATH);

            return missingno;
        }
    }

    private static Sprite missingno;

    public PlayerManager player;

    public TowerDisplay currentTowerDisplay;

    [Header("Status Bar")]
    public TextMeshProUGUI toolText;
    public TextMeshProUGUI coinText;

    private string coinTextFormat;

    private void Awake()
    {
        if (coinText)
            coinTextFormat = coinText.text;
    }

    private void Update()
    {
        if (!player)
            return;

        if (toolText)
            toolText.text = player.GetToolName();
        if (coinText)
            coinText.text = string.Format(coinTextFormat, player.coins);

        if (!currentTowerDisplay)
            return;

        if (GameManager.Instance.Phase != GamePhase.BUILD || player.destroyTool)
        {
            if (currentTowerDisplay.gameObject.activeInHierarchy)
                currentTowerDisplay.gameObject.SetActive(false);

            return;
        }

        RadioSelect select = RadioSelect.Controller.GetSelection("BuildMenu.SelectedTower");
        if (!select || !(select.additionalData is TowerBase))
        {
            if (currentTowerDisplay.gameObject.activeInHierarchy)
                currentTowerDisplay.gameObject.SetActive(false);

            return;
        }

        if (!currentTowerDisplay.gameObject.activeInHierarchy)
            currentTowerDisplay.gameObject.SetActive(true);

        currentTowerDisplay.Load((TowerBase)select.additionalData);
    }
}
