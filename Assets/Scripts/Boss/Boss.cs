using UnityEngine;

public interface Boss
{
    public string[] Attacks { get; }

    void PhaseChange();

    void BossLogic(GameObject b, Vector3 playerPos);

}