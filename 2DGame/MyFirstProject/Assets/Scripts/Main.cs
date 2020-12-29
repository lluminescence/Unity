using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    float timer = 0f;
    public Text timeText;
    public TimeWork timeWork;
    public float countDown;
    public GameObject inventoryPan;
    public SoundEffector soundEffector;
    public AudioSource musicSource, soundSource;

    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 144;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 9;

        if ((int)timeWork == 2)
            timer = countDown;
    }

    public void Update()
    {
        coinText.text = player.GetCoins().ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHearts() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timeWork == 1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)timeWork == 2)
        {
            timer -= Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
            if (timer <= 0)
                Lose();
        }
        else
        {
            timeText.gameObject.SetActive(false); 
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        pauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        pauseScreen.SetActive(false );
    }

    public void Win()
    {
        soundEffector.PlayWinSound();

        Time.timeScale = 0f;
        player.enabled = false;
        winScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);

        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }

    public void Lose()
    {
        soundEffector.PlayLoseSound();

        Time.timeScale = 0f;
        player.enabled = false;
        loseScreen.SetActive(true);

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }

    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public enum TimeWork
{
    None,
    Stopwatch,
    Timer
}