using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI
using TMPro; // Necesario para TextMeshPro

public class UIManager : MonoBehaviour
{
    public Sprite[] lifeSprites; // Array de imágenes para representar las vidas
    public Image livesImageDisplay; // Imágen que muestra las vidas
    public GameObject titleScreen;
    public int Score;
    public TMP_Text scoreText; // Referencia al texto del puntaje usando TextMeshProUGUI

    // Método para actualizar las vidas
    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0 && currentLives < lifeSprites.Length)
        {
            livesImageDisplay.sprite = lifeSprites[currentLives];
        }
        else
        {
            Debug.LogWarning("Current lives value is out of range.");
        }
    }

    // Método para actualizar el puntaje
    public void UpdateScore()
    {
        Score += 10;
        scoreText.text = "Score: " + Score.ToString(); // Actualiza el texto del puntaje
    }
    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
    }
    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
        scoreText.text = "Scrore";
    }
}
