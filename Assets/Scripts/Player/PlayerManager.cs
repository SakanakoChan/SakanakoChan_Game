using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cheat_Get500Currency();
        }
    }

    private void Cheat_Get500Currency()
    {
        currency += 500;
    }

    public bool BuyIfAvailable(int _price)
    {
        if (currency < _price)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        currency -= _price;
        return true;
    }
}
