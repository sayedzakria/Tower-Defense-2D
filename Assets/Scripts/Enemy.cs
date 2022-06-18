using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int target = 0;
    public Transform exitPoint;
    public Transform[] wayPoints;
    public float navigationUpdate;

    private Transform enemy;
    private float navigationTime = 0;
    [SerializeField]
    private int healthPoints;
    private bool isDead = false;
    private Collider2D enemyCollider;
    private Animator anim;

    [SerializeField]
    private int rewardAmount;
    public bool IsDead
    {
        get { return isDead; }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        GameManager.Instance.RegisterEnemy(this);
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints != null&&!isDead)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
            target += 1;
        else if (collision.tag == "Finish")
        {
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.RoundEscap += 1;
            GameManager.Instance.UnRegisterEnemy(this);
            GameManager.Instance.isWaveOver();
            // Destroy(gameObject);

        }
        else if (collision.tag == "ProjectTile")
        {
            ProjectTile projectTile = collision.gameObject.GetComponent<ProjectTile>();
            enemyHit(projectTile.AttackStrength);
            Destroy(collision.gameObject);
        }
    }
    private void enemyHit(int enemyhit)
    {
        if ((healthPoints - enemyhit) > 0)
        {
            healthPoints -= enemyhit;
            GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Hit);
            anim.Play("Hurt");
        }
        else
        {
            anim.SetTrigger("didDie");
            die();
        }

    }
    public void die()
    {
        
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Death);
        GameManager.Instance.AddMony(rewardAmount);
        GameManager.Instance.isWaveOver();
    }
}
