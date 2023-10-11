using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface Boss
{

    public string[] Attacks { get; }

    void PhaseChange();
    void BossLogic();

}
