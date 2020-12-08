using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int hp = 0, gg = 0;
    public Sprite[] counts;
    public Sprite is_hp, no_hp, is_gg, no_gg, is_key, no_key;
    public Image hp_img, gg_img, key_img;
    public Player player;

    private void Start()
    {
        if(PlayerPrefs.GetInt("hp") > 0)
        {
            hp = PlayerPrefs.GetInt("hp");
            hp_img.sprite = is_hp;
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[hp];
        }

        if (PlayerPrefs.GetInt("gg") > 0)
        {
            gg = PlayerPrefs.GetInt("gg");
            gg_img.sprite = is_gg;
            gg_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[gg];
        }
    }

    public void AddHp()
    {
        hp++;
        hp_img.sprite = is_hp;
        hp_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[hp];
    }

    public void AddGg()
    {
        gg++;
        gg_img.sprite = is_gg;
        gg_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[gg];
    }

    public void AddKey()
    {
        key_img.sprite = is_key;
    }

    public void UseHp()
    {
        if (hp > 0)
        {
            hp--;
            player.RecountHp(1);
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[hp];
            if (hp == 0)
            {
                hp_img.sprite = no_hp;
            }
        }
    }

    public void UseGg()
    {
        if (gg > 0)
        {
            gg--;
            player.GreenGem();
            gg_img.transform.GetChild(0).GetComponent<Image>().sprite = counts[gg];
            if (gg == 0)
            {
                gg_img.sprite = no_gg;
            }
        }
    }

    public void RecountItems()
    {
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("gg", gg);
    }
}
