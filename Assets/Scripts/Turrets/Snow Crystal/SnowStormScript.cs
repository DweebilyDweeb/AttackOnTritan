using UnityEngine;
using System.Collections;

public class SnowStormScript : MonoBehaviour {

    public float SnowRadius;
    public float SnowSlow;
    public bool Source;
    public float originalSpeed;
    public GameObject Main;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Source)
        {
            AreaofEffect(RadiusCheck());
        }
        else if ((Main.transform.position - this.transform.position).magnitude > SnowRadius || !Main.GetComponent<SnowStormScript>())
        {
            Destroy(this.GetComponent<SnowStormScript>());
        }
	}

    public void Snow(float rad, float slow, bool source, GameObject main)
    {
        Source = source;
        SnowRadius = rad;
        SnowSlow = slow;
        Main = main;
        originalSpeed = this.GetComponent<AIMovement>().GetMaxMovementSpeed();
        this.GetComponent<AIMovement>().SetMaxMovementSpeed(originalSpeed * SnowSlow);
    }

    public bool isSource()
    {
        return Source;
    }

    public float Slow()
    {
        return SnowSlow;
    }

    private void AreaofEffect(Collider[] range)
    {
        foreach (Collider enemy in range)
        {
            if (!enemy.GetComponent<SnowStormScript>())
            {
                enemy.gameObject.AddComponent<SnowStormScript>();
                enemy.gameObject.GetComponent<SnowStormScript>().Snow(SnowRadius,SnowSlow, false, this.gameObject);
            }

        }
    }

    private Collider[] RadiusCheck()
    {
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, SnowRadius, LayerMask.GetMask("Monster"));

        if (enemiesColliders.Length <= 0)
            return null;
        return enemiesColliders;
    }

    private void destroying()
    {

    }

    private void OnDestroy()
    {
        this.GetComponent<AIMovement>().SetMaxMovementSpeed(originalSpeed);
        if (Source)
        {
            foreach (Collider enemy in RadiusCheck())
            {
                if (enemy.GetComponent<SnowStormScript>())
                {
                    Destroy(enemy.gameObject.GetComponent<SnowStormScript>());
                }

            }
        }    
    }

}
