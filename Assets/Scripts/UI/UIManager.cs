using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WorldMap map;

    [SerializeField] private TextMeshProUGUI RoadCount;
    [SerializeField] private TextMeshProUGUI VillageCount;
    [SerializeField] private TextMeshProUGUI VillageTotal;

    private void Awake()
    {
        map.RefreshTexts += RefreshTexts;
    }

    private void RefreshTexts(object sender, EventArgs e)
    {
        RoadCount.text = map.Roads.ToString();
        VillageCount.text = map.ConnectedVillages.ToString();
        VillageTotal.text = map.TotalVillages.ToString();
    }
}
