using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int health;
    public int numberOfHearts;
    public Image[] hearts;

    public bool isGameOver = false;

    private void Update()
    {
        if (!isGameOver)
        {
            Debug.Log("Before Clamp: " + health);
            health = Mathf.Clamp(health, 0, numberOfHearts);
            Debug.Log("After Clamp: " + health);

            if (health == 0) 
            {
                isGameOver = true;
                Debug.Log("Game Over");
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
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }


}
