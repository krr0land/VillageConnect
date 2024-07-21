using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WorldMap map;

    [SerializeField] private TextMeshProUGUI RoadCount;
    [SerializeField] private TextMeshProUGUI VillageCount;
    [SerializeField] private TextMeshProUGUI VillageTotal;
    [SerializeField] private Image MusicButton;

    [SerializeField] private Sprite MusicOn;
    [SerializeField] private Sprite MusicOff;

    [SerializeField] private AudioSource musicPlayer;

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

    public void ToggleMusic()
    {
        musicPlayer.mute = !musicPlayer.mute;
        MusicButton.sprite = musicPlayer.mute ? MusicOff : MusicOn;
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
