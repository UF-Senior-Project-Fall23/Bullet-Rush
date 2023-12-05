using System.Collections;

public interface puppetAttack
{
    public bool spotlight { get; set; }
    public void reset();
    public IEnumerator BladeFlourish(int followPattern);
    public IEnumerator Spotlight();
    public IEnumerator Rush();
    public IEnumerator SpinDance(bool rush);
    public IEnumerator DetonatePuppets();
}