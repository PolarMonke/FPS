using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public GameObject bloodScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"Health: {HP}";
    } 

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            print("player dead");
            KillPlayer();
        }
        else
        {
            print($"player hit\ndealt {damageAmount}");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health{HP}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand") && !isDead)
        {
            TakeDamage(other.GetComponent<ZombieHand>().damage);
        }    
    }

    private void KillPlayer()
    {
        isDead = true;
        playerHealthUI.gameObject.SetActive(false);
        GetComponent<DeathScreen>().StartFade();
        GetComponentInChildren<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<Animator>().enabled = true;
        StartCoroutine(ShowGameOverUI());

    }
    
    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodScreen.activeInHierarchy)
        {
            bloodScreen.SetActive(true);
        }

        var image = bloodScreen.GetComponentInChildren<UnityEngine.UI.Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 2f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null; ; 
        }

        if (bloodScreen.activeInHierarchy)
        {
            bloodScreen.SetActive(false);
        }
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }
}
