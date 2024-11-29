using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHP = 100;
    public int HP = 100;

    public GameObject bloodScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        UpdateHP();
    } 

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDead);
            KillPlayer();
        }
        else
        {
            StartCoroutine(BloodyScreenEffect());
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
            UpdateHP();
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
        gameOverUI.GetComponent<TextMeshProUGUI>().text = LanguagesDB.Instance.GetText("GameOver");
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    public void Heal(int hpAmount)
    {
        if (HP < maxHP)
        {   
            int healAmount = System.Math.Min(hpAmount, maxHP - HP);
            HP += healAmount;
            UpdateHP();
        }
    }
    public void UpdateHP()
    {
        playerHealthUI.text = $"{LanguagesDB.Instance.GetText("Health")} {HP}";
    }
    
    public void MultiplyMaxHP(int multiplier)
    {
        maxHP *= multiplier;
    }
    public void DivideMaxHP(int divisor)
    {
        maxHP /= divisor;
    }
}
