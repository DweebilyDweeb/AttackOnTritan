using UnityEngine;
using System.Collections;

public class MortarScript : TurretScript
{

    private GameObject target;
    private float angle = 45;
    private float velxz;
    private float vely;
    private Vector3 direction;
    private float rotateSpd;
    private float velocity;
    private float explosion;
    private float height;

    public GameObject Bulletprefab;


    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        minAttackDamage = 100;
        maxAttackDamage = 150;
        attackSpeed = 0.5f;
        proximity = 10f;
        rotateSpd = 3f;
        attackStyle = ATTSTYLE.NEAREST_FIRST;
        velocity = Mathf.Sqrt((proximity+0.1f) * 9.8f);
        explosion = 5.0f;
        height = 0.0f;
        towerLevel = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target)
        {
            // Gets Vector3 direction from traget
            direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), Time.deltaTime * rotateSpd);

}
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * rotateSpd);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LevelUp();
        }

    }

    protected override Collider[] EnemiesInAttackRadius()
    {
        if (base.EnemiesInAttackRadius() == null)
            target = null;
        return base.EnemiesInAttackRadius();
    }

    protected override void Attacking(Collider[] enemies)
    {
        switch (attackStyle)
        {
            case ATTSTYLE.NEAREST_FIRST:
                {
                    float nearestDistance = proximity;
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude < nearestDistance)
                        {
                            nearestDistance = new Vector3(enemy.transform.position.x - transform.position.x, 0 , enemy.transform.position.z - transform.position.z).magnitude;
                            target = enemy.transform.gameObject;
                            height = enemy.transform.position.y - transform.position.y;
                        }
                    }
                    initialiseprojectile(nearestDistance, height);
                    break;
                }

            case ATTSTYLE.FURTHEST_FIRST:
                {
                    float longestDistance = 0f;
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude >= longestDistance)
                        {
                            longestDistance = (enemy.transform.position - transform.position).magnitude;
                            target = enemy.transform.gameObject;
                        }
                    }
                    break;
                }
        }

        Debug.DrawRay(transform.position, direction, new Color(1, 0, 1), 10);
        
    }

    public void LevelUp()
    {
      
        if (towerLevel == 1)
        {
            GameObject turretbase = Resources.Load("Turrets/Mortar/Base 1") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Mortar/Turret 1") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (towerLevel == 2)
        {
            GameObject turretbase = Resources.Load("Turrets/Mortar/Base 2") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Mortar/Turret 2") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
        }
        LevelUpgrades(1, 2, 0.05f, 0.5f);
        towerLevel += 1;
        velocity = Mathf.Sqrt((proximity + 0.1f) * 9.8f);

    }

    private void initialiseprojectile(float distance, float height)
    {

        GameObject projectile = Instantiate(Bulletprefab);
        projectile.GetComponent<MortarProjectileScript>().setDamage(minAttackDamage, maxAttackDamage);
        projectile.GetComponent<MortarProjectileScript>().setRadius(explosion);
        projectile.transform.position = transform.position;
        Vector3 xyzdirection;
        //sin^-1(distance * gravity)/velocity^2

        //angle =  Mathf.Asin((distance * 9.8f) / (velocity * velocity)) / 2 ;
        angle = Mathf.Atan((velocity * velocity + Mathf.Sqrt( (velocity * velocity * velocity * velocity) - 9.8f * ((9.8f * distance * distance) + (2 * height * velocity * velocity)))) / (9.8f * distance));

        velxz = Mathf.Cos(angle);
        vely = Mathf.Sin(angle);

        // XYZ proximity set
        xyzdirection.x = direction.normalized.x * velxz * velocity;
        xyzdirection.y = vely * velocity;
        xyzdirection.z = direction.normalized.z * velxz * velocity;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = xyzdirection;
    }
}
