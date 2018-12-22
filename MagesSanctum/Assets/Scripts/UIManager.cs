using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerManager player;

    public GameObject currentTowerDisplay;

    [Header("Status Bar")]
    public TextMeshProUGUI toolText;
    public TextMeshProUGUI coinText;

    private string coinTextFormat;
    private string[] towerDisplayFormats;

    private void Awake()
    {
        if (coinText)
            coinTextFormat = coinText.text;
        towerDisplayFormats = TowerLoader.GetFormats(currentTowerDisplay);
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
            if (currentTowerDisplay.activeInHierarchy)
                currentTowerDisplay.SetActive(false);

            return;
        }

        RadioSelect select = RadioSelect.Controller.GetSelection("BuildMenu.SelectedTower");
        if (!select || !(select.additionalData is TowerBase))
        {
            if (currentTowerDisplay.activeInHierarchy)
                currentTowerDisplay.SetActive(false);

            return;
        }

        if (!currentTowerDisplay.activeInHierarchy)
            currentTowerDisplay.SetActive(true);

        TowerLoader.UpdateTowerDisplay((TowerBase)select.additionalData, currentTowerDisplay, towerDisplayFormats);
    }
}
