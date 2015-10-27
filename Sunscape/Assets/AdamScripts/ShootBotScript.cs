using UnityEngine;
using System.Collections;

public class ShootBotScript : MonoBehaviour
{

    public GameObject ObjectToShoot;
    public float MaxCooldown = 1;
    public float ObjectLifetime = 10;
    public float ObjectSpeed = 5;

    float currentCooldown;
    bool active;
    // Use this for initialization
    void Start()
    {
        active = true;
        currentCooldown = MaxCooldown;

    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                GameObject newObject = Instantiate(ObjectToShoot);
                newObject.transform.position = transform.position + transform.forward;
                newObject.GetComponent<Rigidbody>().velocity = transform.forward * ObjectSpeed;
                Destroy(newObject, ObjectLifetime);
                currentCooldown = MaxCooldown;

            }
        }

    }
}
