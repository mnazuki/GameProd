using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int numberOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public int score;
    public int scoreMax;

    public bool isGameOver = false;
    private UIConveyor uiConveyor;
    private void Update()
    {
        if (!isGameOver)
        {
            if (health == 0) 
            {
                isGameOver = true;
                return;
            }
        }
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }


            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

}
