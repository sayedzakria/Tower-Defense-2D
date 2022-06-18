using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenAttacks;
    [SerializeField]
    private float attackRadus;
    [SerializeField]
    private ProjectTile projectTile;
    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool isAttacking = false;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRang = new List<Enemy>();
        foreach (Enemy enemy in GameManager.Instance.EnemiesList)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= attackRadus)
            {
                enemiesInRang.Add(enemy);
            }
        }
        {

        }
        return enemiesInRang;
    }
    private Enemy GetNearstEnemy()
    {
        Enemy nearstenemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearstenemy = enemy;
            }
        }
        return nearstenemy;
    }
    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            
                Enemy nearesEnemy = GetNearstEnemy();
                if (nearesEnemy != null && Vector2.Distance(transform.localPosition, nearesEnemy.transform.localPosition) < attackRadus)
                {
                    targetEnemy = nearesEnemy;
                }
           
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadus)
            {
                targetEnemy = null;
            }
        }
        
    }
    //is good for moving objects
    private void FixedUpdate()
    {
        if (isAttacking)
            Attack();
    }
    public void Attack()
    {
        isAttacking = false;
        ProjectTile newprojectTile = Instantiate(projectTile) as ProjectTile;
        newprojectTile.transform.localPosition = transform.localPosition;
        if (newprojectTile.ProjectTileType == proType.arrow)
        {
            GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if (newprojectTile.ProjectTileType == proType.rock)
        {
            GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Rock);
        }
        else if (newprojectTile.ProjectTileType == proType.fireball)
        {
            GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.Fireball);
        }
        if (targetEnemy == null)
        {
            Destroy(newprojectTile);
        }
        else
        {
            StartCoroutine(MoveProjecttile(newprojectTile));
        }
    }
    IEnumerator MoveProjecttile(ProjectTile projectTile)
    {
        while (GetTargetDistance(targetEnemy) > 0.02f && projectTile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectTile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            //5f is the speed *Time.deltaTime to adjust on different computers speed
            projectTile.transform.localPosition = Vector2.MoveTowards(projectTile.transform.localPosition, targetEnemy.transform.localPosition,5f*Time.deltaTime);
            yield return null;
        }
        if (projectTile != null && targetEnemy == null)
        {
            Destroy(projectTile);
        }
    }
    private float GetTargetDistance(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy = GetNearstEnemy();
            if (enemy == null)
                return 0f;
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    }
}
