using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    //just to push
    public int valorCoins = 0;
    public TextMeshProUGUI currencyText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddValorCoins(int amount)
    {
        valorCoins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        currencyText.text = $"Valor Coins: {valorCoins}";
    }
}
