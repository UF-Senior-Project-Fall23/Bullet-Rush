
public interface Boss
{

    public string[] Attacks { get; }

    void PhaseChange();
    void BossLogic(float deltaTime);

}