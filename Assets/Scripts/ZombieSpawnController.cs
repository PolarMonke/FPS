using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ZombieSpawnController : MonoBehaviour
{
    public int initializeZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;
    public int waveMultiplier = 2;

    public int currentWave = 0;
    public float waveCoolDown = 10f;

    public bool inCoolDown;
    public float coolDownCounter;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI currentWaveUI;
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;

    private void Start()
    {
        currentZombiesPerWave = initializeZombiesPerWave;
        string waveOverText = LanguagesDB.Instance.GetText("WaveOverText");
        waveOverUI.text = waveOverText;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        currentWaveUI.text = $"{LanguagesDB.Instance.GetText("Wave")} {currentWave.ToString()}";
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Zombie>();
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();

        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();
        if (currentZombiesAlive.Count == 0 && !inCoolDown)
        {
            StartCoroutine(WaveCoolDown());
        }
        if (inCoolDown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCoolDown;
        }   
        coolDownCounterUI.text = coolDownCounter.ToString("F0");
    }
    private IEnumerator WaveCoolDown()
    {
        inCoolDown = true;
        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCoolDown);
        inCoolDown = false;
        waveOverUI.gameObject.SetActive(false);
        currentZombiesPerWave *= waveMultiplier;
        StartNextWave();
    }

}
