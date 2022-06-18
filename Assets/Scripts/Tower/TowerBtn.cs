using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private Tower towerObject;
    public Tower TowerObject
    {
        get
        {
            return towerObject;
        }
    }
    [SerializeField]
    private Sprite spriteDarg;
    public Sprite SpriteDarg
    {
        get
        {
            return spriteDarg;
        }
    }
    [SerializeField]
    private int towerPrice;
    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
