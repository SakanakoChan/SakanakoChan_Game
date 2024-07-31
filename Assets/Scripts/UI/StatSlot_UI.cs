using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlot_UI : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = $"Stat - {statName}";

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValue_UI();
    }

    public void UpdateStatValue_UI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStatByType(statType).GetValue().ToString();
        }
    }
}
