using UnityEngine;

public interface Boss
{

    public string[] Attacks { get; }

    void PhaseChange();
    void BossLogic(float deltaTime, GameObject b, Vector3 playerPos);

}