using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour, puppetAttack
{
    public int a = 7;
    public int attack { get => a; set => a = value; }
}