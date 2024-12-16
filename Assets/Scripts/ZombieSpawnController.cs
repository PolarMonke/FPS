using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class ZombieSpawnController : MonoBehaviour
{
    public int initializeEnemiesPerWave = 5;
    public int currentEnemiesPerWave;

    public float spawnDelay = 0.5f;
    public int waveAdder = 5;

    public bool bossIsSpawned;

    public int currentWave = 0;
    public float waveCoolDown = 10f;

    public bool inCoolDown;
    public float coolDownCounter;

    public List<Enemy> currentEnemiesAlive;

    public GameObject zombiePrefab;
    public GameObject mZombiePrefab;
    public GameObject skibidiPrefab;

    public TextMeshProUGUI currentWaveUI;
    public TextMeshProUGUI enemiesLeftUI;
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;
    //public TextMeshProUGUI bossHPUI;

    public List<Transform> spawnPoints;

    private void Start()
    {
        currentEnemiesPerWave = initializeEnemiesPerWave;
        string waveOverText = LanguagesDB.Instance.GetText("WaveOverText");
        waveOverUI.text = waveOverText;
        
        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentEnemiesAlive.Clear();
        currentWave++;

        GlobalReferences.Instance.waveNumber = currentWave;
        
        currentWaveUI.text = $"{LanguagesDB.Instance.GetText("Wave")} {currentWave.ToString()}";
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentEnemiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f));
            
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnPosition = spawnPoints[randomIndex].position + spawnOffset;
            
            
            if (!bossIsSpawned && currentWave % 3 == 0)
            {
                var skibidi = Instantiate(skibidiPrefab, spawnPosition, Quaternion.identity);
                Enemy enemyScript = skibidi.GetComponent<SkibidiBoss>();
                currentEnemiesAlive.Add(enemyScript);
                bossIsSpawned = true;
            }
            else if (Random.value < 0.3)
            {
                var mZombie = Instantiate(mZombiePrefab, spawnPosition, Quaternion.identity);
                Enemy enemyScript = mZombie.GetComponent<MZombie>();
                currentEnemiesAlive.Add(enemyScript);
            }
            else
            {
                var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
                Enemy enemyScript = zombie.GetComponent<Zombie>();
                currentEnemiesAlive.Add(enemyScript);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    private void Update()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();

        foreach (Enemy enemy in currentEnemiesAlive)
        {
            if (enemy.isDead)
            {
                if (enemy.enemyType == Enemy.EnemyTypes.SkibidiBoss)
                {
                    bossIsSpawned = false;
                }
                enemiesToRemove.Add(enemy);
            }
        }
        foreach (Enemy enemy in enemiesToRemove)
        {
            currentEnemiesAlive.Remove(enemy);
        }
        enemiesToRemove.Clear();
        print(currentEnemiesAlive.Count);
        if (currentEnemiesAlive.Count == 0 && !inCoolDown)
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
        enemiesLeftUI.text = LanguagesDB.Instance.GetText("EnemiesLeft") + currentEnemiesAlive.Count;
    }
    private IEnumerator WaveCoolDown()
    {
        inCoolDown = true;
        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCoolDown);
        inCoolDown = false;
        waveOverUI.gameObject.SetActive(false);
        currentEnemiesPerWave += waveAdder;
        StartNextWave();
    }

    private void KillAllEnemies()
    {
        foreach (Enemy enemy in currentEnemiesAlive)
        {
            enemy.Die();
            //yield return new WaitForSeconds(0.05f);    
        }
        
    }

    public void DoubleAndGiveItToTheNextWave()
    {
        KillAllEnemies();
        initializeEnemiesPerWave *= 2;
    }

    public void StartFromWave(int waveNumber)
    {
        if (waveNumber <= 0)
        {
            return;
        }
        currentWave = waveNumber - 1;
        currentEnemiesPerWave = initializeEnemiesPerWave + (currentWave * waveAdder);
        StartNextWave();
    }

}
