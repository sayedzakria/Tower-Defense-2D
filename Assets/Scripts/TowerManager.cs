using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : SingleTone<TowerManager>
{
    public TowerBtn towerbuttonPressed;
    private SpriteRenderer spriteRenderer;
    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //start butting tower
            //get mouse position
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //going from zero position to mouse position
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit2D.collider.tag == "BuildSite")
            {
                //change tag to prevent build above it
                buildTile = hit2D.collider;
                buildTile.tag = "BuildSiteFull";
                RegisterBuildSide(buildTile);
                PlaceTower(hit2D);
            }
            
        }
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }
    public void RegisterBuildSide(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }
    public void RenamTagsBuildsides()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildSite";
        }
        BuildList.Clear();
    }
    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }
    public void DestroyAllTowers()
    {
        foreach (Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject()&&towerbuttonPressed!=null)
        {
            Tower newTower = Instantiate(towerbuttonPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            buyTower(towerbuttonPressed.TowerPrice);
            GameManager.Instance.Audio_Source.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            DisableSpriteRender();
        }
    }
    public void buyTower(int price)
    {
        GameManager.Instance.SubtractMony(price);
    }
    public void SelectdBttonPressed(TowerBtn towerSelected)
    {
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMony)
        {
            towerbuttonPressed = towerSelected;
            EnableSpriteRender(towerbuttonPressed.SpriteDarg);
        }
    }
    public void FollowMouse()
    {
        transform.position=   Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void EnableSpriteRender(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }
    public void DisableSpriteRender()
    {
        spriteRenderer.enabled = false;
        
    }
}
