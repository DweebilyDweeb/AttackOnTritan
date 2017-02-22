using UnityEngine;
using System.Collections;

public class MortarProjectileScript : MonoBehaviour {

    public GameObject radius;
    private float Explosion_radius;
    private float minDamage;
    private float maxDamage;

    MortarProjectileScript(float explosion, float minDamage, float maxDamage)
    {
    }

    // Use this for initialization
    void Start()
    {
    }

// Update is called once per frame
    void Update () {
        if (this.GetComponent<Rigidbody>().velocity.y < 0 && this.GetComponent<Rigidbody>().position.y <=0)
        {
            Collider[] inrange = Physics.OverlapSphere(transform.position, Explosion_radius, LayerMask.GetMask("Monster"));
            foreach (Collider enemy in inrange)
            {
                //Debug.Log(enemy.gameObject);
            }
            Destroy(this.gameObject);
            GameObject temp = Instantiate(radius);
            temp.transform.position = transform.position;
            temp.transform.localScale = new Vector3(Explosion_radius * 2, 0.1f, Explosion_radius * 2);
            Debug.Log(temp.transform.localScale);
            Destroy(temp, 0.1f);
        }

    }

    public void setRadius(float radius)
    {
        this.Explosion_radius = radius;
    }
    public void setDamage(float minD, float maxD)
    {
        this.minDamage = minD;
        this.maxDamage = maxD;
    }
}
