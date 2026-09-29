using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingMoney = 0;
    [SerializeField] private TMP_Text moneyText;

    private int currentMoney;

    private void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }

    public void SpendMoney(int amount)
    {
        currentMoney -= amount;

        if (currentMoney < 0)
        {
            currentMoney = 0;
        }

        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: R" + currentMoney;
        }
    }
}