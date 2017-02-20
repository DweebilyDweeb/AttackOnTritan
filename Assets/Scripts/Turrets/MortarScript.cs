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
        velocity = Mathf.Sqrt(proximity * 9.8f);
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
                            nearestDistance = (enemy.transform.position - transform.position).magnitude;
                            target = enemy.transform.gameObject;
                        }
                    }
                    initialiseprojectile(nearestDistance);
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
        LevelUpgrades(1, 2, 0.05f, 0.5f);
    }

    private void initialiseprojectile(float distance)
    {

        GameObject projectile = Instantiate(Bulletprefab);
        projectile.transform.position = transform.position;
        Vector3 xyzdirection;
        //sin^-1(distance * gravity)/velocity^2
        angle =  Mathf.Asin((distance * 9.8f) / (velocity * velocity)) / 2 ;
        velxz = Mathf.Cos(angle);
        vely = Mathf.Sin(angle);

        // XYZ proximity set
        xyzdirection.x = direction.normalized.x * velxz * velocity;
        xyzdirection.y = vely * velocity;
        xyzdirection.z = direction.normalized.z * velxz * velocity;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = xyzdirection;
        Destroy(projectile, 2.0f);
        Debug.Log(Mathf.Rad2Deg * angle);
    }
}
