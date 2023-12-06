using System.Collections;
using UnityEngine;

// Handles the code for the gloves in Cordelia's Puppeteer's Grasp attack.
public class Hands : MonoBehaviour
{
    public static Hands instance;

    private Collider2D myCollider;
    public GameObject handUpPreFab;
    public GameObject handDownPreFab;
    public GameObject voidPreFab;
    public GameObject voidMaskPreFab;

    GameObject handUp;
    GameObject handDown = null;
    GameObject voidArt;
    GameObject voidMask;

    // Instantiates basic animation data.
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myCollider.enabled = false;
        Vector3 handDownPos = new Vector3(transform.position.x, transform.position.y - 2f, 0);
        Vector3 voidMaskPos = new Vector3(transform.position.x, transform.position.y + .55f, 0);

        handUp = Instantiate(handUpPreFab, handDownPos, Quaternion.identity);
        handDown = Instantiate(handDownPreFab);


        voidArt = Instantiate(voidPreFab, transform.position, Quaternion.identity);

        voidMask = Instantiate(voidMaskPreFab, voidMaskPos, Quaternion.identity);

        StartCoroutine(HandRaise());
    }


    // Makes the hand rise up from the void.
    public IEnumerator HandRaise()
    {
        float length = 0f;
        float endTime = 1f;

        yield return new WaitForSeconds(.5f);
        while (length < endTime)
        {
            float speed = 6f;
            if(length > .8f)
            {
                Vector3 voidMaskPos = new Vector3(transform.position.x, transform.position.y + .2f, 0);
                voidMask.transform.position = voidMaskPos;
            }
            handUp.transform.position = Vector3.MoveTowards(handUp.transform.position, transform.position, speed * Time.deltaTime);

            length += Time.deltaTime;
            yield return null;
        }
        myCollider.enabled = true;
        yield return null;

    }

    // Makes the hand strike down and attack where the player was, then destroys itself.
    public IEnumerator Hit()
    {
        float length = 0f;
        float endTime = 1.5f;

        Destroy(voidMask);
        Destroy(handUp);
        handDown = Instantiate(handDownPreFab, transform.position, Quaternion.identity);
        length = 0f;
        Vector3 playerPos = PlayerController.instance.transform.position;
        yield return new WaitForSeconds(.4f);

        //Vector3 playerPos = PlayerController.instance.transform.position;
        Vector3 direction = playerPos - handDown.transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90);
        float dist = 3f;

        Vector3 normalizedDirection = direction.normalized;
        normalizedDirection.y = normalizedDirection.y /1.5f;
        Vector3 newPosition = handDown.transform.position - normalizedDirection * -dist;

        handDown.transform.position = newPosition;
        handDown.transform.rotation = Quaternion.Euler(0, 0, angle);

        yield return new WaitForSeconds(.65f);

        handDown.transform.position = transform.position;
        handDown.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(.2f);
        Destroy(handDown);
        Destroy(voidArt);
        Destroy(gameObject);
        yield return null;

    }

    // Detects when the player is nearby and starts an attack.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myCollider.enabled = false;
            StartCoroutine(Hit());
        }
    }

    // Cleans up animation data
    public void OnDestroy()
    {
        if(handDown != null)
        {
            Destroy(handDown);
        }
        if (handUp != null)
        {
            Destroy(handUp);
        }
        if(voidArt != null)
        {
            Destroy(voidArt);
        }
        if(voidMask != null)
        {
            Destroy(voidMask);
        }

        Destroy(gameObject);
    }
}
