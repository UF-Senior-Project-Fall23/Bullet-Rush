using System.Collections;
using UnityEngine;

public interface Boss
{
    public string[] Attacks { get; }

    void PhaseChange();

    IEnumerator StartPhase();

}