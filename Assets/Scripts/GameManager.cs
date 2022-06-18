using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next,play,gameover,win
}
public class GameManager : SingleTone<GameManager>
{
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text totalMonyLBL;
    [SerializeField]
    private Text currentWaveLBL;
    [SerializeField]
    private Text totalEscapLBL;
    [SerializeField]
    private Button playBTN;
    public GameObject spawnPoint;
    public GameObject[] enemies;
   
    public int totalEnemies;
    public int enemeisPerSpawn;
    const float spawnDeley=1f;

    private int waveNumber = 0;
    private int totalMony = 10;
    private int totalEscaped = 0;
    private AudioSource audioSource;
    public AudioSource Audio_Source
    {
        
        get { return audioSource; }
    }
    public int TotalEscaped {
        set { totalEscaped = value; }
        get { return totalEscaped; }
    }
    private int roundEscap = 0;
    public int RoundEscap
    {
        set { roundEscap = value; }
        get { return roundEscap; }
    }
    private int totalKilled = 0;
    public int TotalKilled
    {
        set { totalKilled = value; }
        get { return totalKilled; }
    }
    private int whichEnemiesToSpwn = 0;
    private gameStatus currentGameStatus = gameStatus.play;
    public List<Enemy> EnemiesList = new List<Enemy>();
    [SerializeField]
    private Text playBtn;

    public int TotalMony { get { return totalMony; }
        set { totalMony = value;
            totalMonyLBL.text = totalMony.ToString(); }
    }


    // Start is called before the first frame update
    void Start()
    {
        playBTN.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();
    }
    private void Update()
    {
        handleEscap();
    }
    IEnumerator Spawn()
    {
        if (enemeisPerSpawn > 0 && EnemiesList.Count < totalEnemies)
        {
            for (int i = 0; i < enemeisPerSpawn; i++)
            {
                if (EnemiesList.Count < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[Random.Range(0,enemies.Length)]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                   
                }

            }
            yield return new WaitForSeconds(spawnDeley);
            StartCoroutine(Spawn());
        }
    }
    public void RegisterEnemy(Enemy enemy)
    {
        EnemiesList.Add(enemy);
    }
    public void UnRegisterEnemy(Enemy enemy)
    {
        EnemiesList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyAllEnemies()
    {
        foreach (Enemy enemy in EnemiesList)
        {
            Destroy(enemy.gameObject);
        }
        EnemiesList.Clear();
    }
   public void AddMony(int amount)
    {
        TotalMony += amount;
    }
    public void SubtractMony(int amount)
    {
        TotalMony -= amount;
    }
    public void isWaveOver()
    {
        totalEscapLBL.text = "Escaped" + TotalEscaped + "/10";
        if ((RoundEscap + TotalKilled) == totalEnemies)
        {
            setCurrentGameState(); 
            showMenu();
        }
    }
    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentGameStatus = gameStatus.gameover;
        }else if (waveNumber == 0 && (TotalKilled + RoundEscap) == 0)
        {
            currentGameStatus = gameStatus.play;
        }else if (waveNumber >= totalWaves)
        {
            currentGameStatus = gameStatus.win;
        }
        else
        {
            currentGameStatus = gameStatus.next;
        }
    }
    public void showMenu()
    {
        switch (currentGameStatus)
        {
            case gameStatus.next:
                playBtn.text = "Next Wave";
                break;
            case gameStatus.play:
                playBtn.text = "Play";
                break;
            case gameStatus.gameover:
                playBtn.text = "Play Again!";
                GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Gameover);
                break;
            case gameStatus.win:
                playBtn.text = "Play";
                break;
            default:
                break;
        }
        playBTN.gameObject.SetActive(true);
    }
    public void plyBtnPressed()
    {
        switch (currentGameStatus)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            case gameStatus.play:
                break;
            case gameStatus.gameover:
                break;
            case gameStatus.win:
                break;
            default:
                totalEnemies = 3;
                totalEscaped = 0;
                TotalMony = 10;
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenamTagsBuildsides();
                totalMonyLBL.text = TotalMony.ToString();
                totalEscapLBL.text = "Escaped" + totalEscaped + "/10";
                
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscap = 0;
        currentWaveLBL.text = "Wave" + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBTN.gameObject.SetActive(false);
    }
    private void handleEscap()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableSpriteRender();
            TowerManager.Instance.towerbuttonPressed = null;
        }
    }
}
