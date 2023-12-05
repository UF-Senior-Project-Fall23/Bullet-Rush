using UnityEngine;

public class DimLights : MonoBehaviour
{
    public static DimLights instance;
    public GameObject dimLight;

    private void Awake()
    {
        Debug.Log("AwakenedPart2");
        if (instance == null)
        {
            Debug.Log("Generated new instance.");
            instance = this;
        }
        else
        {
            Debug.Log("DESTROY new instance.");
            Destroy(gameObject);
        }
    }

    public void Appear()
    {
        if (dimLight.GetComponent<Renderer>() != null)
        {
            dimLight.GetComponent<Renderer>().enabled = true;
        }
    }

    public void TurnOff()
    {
        if (dimLight.GetComponent<Renderer>() != null)
        {
                dimLight.GetComponent<Renderer>().enabled = false;
        }
    }

}
