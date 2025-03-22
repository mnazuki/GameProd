// placed on Game Manager Health at the hierarchy (add component)
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 3;

    [SerializeField] private Image[] hearts; // array for the heart images at the hierarchy
    [SerializeField] private Sprite fullHeart; // image sprite for full heart
    [SerializeField] private Sprite emptyHeart; // image sprite for empty heart
    [SerializeField] Animator[] animator; //[NEW] For Heart Animations

    void Update()
    {
        HealthChange();
        HealthCheck();
    }

    private void HealthChange(){
        // illusion of decreasing health but is switching the heart image only as the health value decreases from 3 to 0
        // change/switch the heart image based on the health value
        foreach (Image img in hearts){
            img.sprite = emptyHeart;
        }
        // display full heart image based on the health value
        for (int i = 0; i < health; i++){
            hearts[i].sprite = fullHeart;
        }
    }

    public bool  HealthCheck(){ //[NEW UPDATE] Switched from "void" to "bool" for easier checking in the WinLose Manager. Added True & False returns.
        if (health <= 0){
            // Debug.Log("Game Over");
            return true;  
        }
        return false;
    }

    // used in decreasing health when bacteria is clicked, references in DestructibleObject.cs
    public void DecreaseHealth()
    {
        if (health > 0)
        {
            health--;

            //[NEW] Play "shake" Animation each time heart is damaged
            if (animator[health] != null)
            {
                animator[health].Play("shake"); 
            }
        }
    }
}
