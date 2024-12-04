using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class ZombieSpawnController : MonoBehaviour
{
    public int initializeZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;
    public int waveAdder = 5;

    public int currentWave = 0;
    public float waveCoolDown = 10f;

    public bool inCoolDown;
    public float coolDownCounter;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI currentWaveUI;
    public TextMeshProUGUI enemiesLeftUI;
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;

    public List<Transform> spawnPoints;

    private void Start()
    {
        currentZombiesPerWave = initializeZombiesPerWave;
        string waveOverText = LanguagesDB.Instance.GetText("WaveOverText");
        waveOverUI.text = waveOverText;
        
        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;
        
        currentWaveUI.text = $"{LanguagesDB.Instance.GetText("Wave")} {currentWave.ToString()}";
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f));
            
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnPosition = spawnPoints[randomIndex].position + spawnOffset;
            
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
        enemiesLeftUI.text = LanguagesDB.Instance.GetText("EnemiesLeft") + currentZombiesAlive.Count;
    }
    private IEnumerator WaveCoolDown()
    {
        inCoolDown = true;
        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCoolDown);
        inCoolDown = false;
        waveOverUI.gameObject.SetActive(false);
        currentZombiesPerWave += waveAdder;
        StartNextWave();
    }

    private void KillAllEnemies()
    {
        foreach (Enemy zombie in currentZombiesAlive)
        {
            zombie.Die();    
        }
    }

    public void DoubleAndGiveItToTheNextWave()
    {
        KillAllEnemies();
        initializeZombiesPerWave *= 2;
    }

}
