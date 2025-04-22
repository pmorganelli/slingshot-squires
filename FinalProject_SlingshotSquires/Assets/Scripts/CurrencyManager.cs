using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    //just to push
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddValorCoins(int amount)
    {
        GameHandler.coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currencyText)
        {
            currencyText.text = $"{GameHandler.coinCount}";

        }
    }
}
