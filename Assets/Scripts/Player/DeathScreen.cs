using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public UnityEngine.UI.Image fadeImage;
    public float fadeDuration = 7.0f;
    
    public TextMeshProUGUI mainMenuButtonText;
 
    public void StartFade()
    {
        fadeImage.gameObject.SetActive(true);
        mainMenuButtonText.text = LanguagesDB.Instance.GetText("ToMainMenu");
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(FadeOut());
    }
 
    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); 
 
        while (timer < fadeDuration)
        {
            
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
 
       
        fadeImage.color = endColor;
    }
}
