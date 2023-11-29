using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface puppetAttack
{
    public int attackNum { get; set; }
    public bool spotlight { get; set; }
    public IEnumerator BladeFlourish(int followPattern);
    public IEnumerator Spotlight();
    public IEnumerator Rush();
}
