using System.Collections;

/// A simple interface delineating attributes a boss should have.
public interface Boss
{
    /// Returns the names of the attacks the boss has access to.
    public string[] Attacks { get; }
    
    /// Should be used to signify a different stage/phase of the fight or to dispatch different attacks.
    void PhaseChange();
    
    /// Used by the boss controller, runs the startup code for the boss.
    IEnumerator StartPhase();

    void ForceDeath();

}