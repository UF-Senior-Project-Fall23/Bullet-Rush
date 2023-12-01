using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface puppetAttack
{
    public bool spotlight { get; set; }
    public void reset();
    public IEnumerator BladeFlourish(int followPattern);
    public IEnumerator Spotlight();
    public IEnumerator Rush();
    public IEnumerator SpinDance(bool rush);
}