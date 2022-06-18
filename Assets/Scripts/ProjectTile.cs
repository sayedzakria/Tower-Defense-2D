using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum proType
{
    rock,arrow,fireball
}
public class ProjectTile : MonoBehaviour
{
    [SerializeField]
    private int attackStrength;
    [SerializeField]
    private proType projectTileType;
    public int AttackStrength
    {
        get { return attackStrength; }
    }

    public proType ProjectTileType
    {
        get { return projectTileType; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
