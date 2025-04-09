using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    //just to push
    public GameObject GameHandler;
    private GameHandler gh;
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        gh = GameHandler.GetComponent<GameHandler>();
        UpdateUI();
    }

    public void AddValorCoins(int amount)
    {
        gh.coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currencyText)
        {
            currencyText.text = $"Valor Coins: {gh.coinCount}";

        }
    }
}
