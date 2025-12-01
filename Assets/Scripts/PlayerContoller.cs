using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    private int currentLives;
    [SerializeField] private int maxLives;

    private void Awake()
    {
        currentLives = maxLives;
    }

    /// <summary>
    /// When the player receive damage
    /// </summary>
    /// <param name="quantity"></param>
    public void DamagePlayer(int quantity)
    {
        currentLives -= quantity;

        //HUD Show Damage Flash
        HUDController.Instance.ShowDamageFlash(); 

        if (currentLives <= 0)
        {
            //TODO HUDController Active Panel
            Debug.Log("GAME OVER!!!");
        }
    }

}
