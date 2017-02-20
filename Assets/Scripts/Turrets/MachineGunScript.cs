using UnityEngine;
using System.Collections;

public class MachineGunTurretScript : TurretScript
{

    private GameObject target;
    private Vector3 direction;
    private float rotateSpd;
    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        minAttackDamage = 10;
        maxAttackDamage = 15;
        attackSpeed = 0.5f;
        proximity = 4f;
        rotateSpd = 3f;
        direction = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target)
        {
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
                    Vector3 tempDir = new Vector3(0, 0, 0);
                    float nearestDistance = proximity;
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude < nearestDistance)
                        {
                            tempDir = enemy.transform.position - transform.position;
                            nearestDistance = (enemy.transform.position - transform.position).magnitude;
                            target = enemy.transform.gameObject;
                        }
                    }
                    break;
                }

            case ATTSTYLE.FURTHEST_FIRST:
                {
                    Vector3 tempDir = new Vector3(0, 0, 0);
                    foreach (Collider enemy in enemies)
                    {
                        if ((transform.position - enemy.transform.position).magnitude >= proximity)
                        {
                            tempDir = enemy.transform.position - transform.position;
                            target = enemy.transform.gameObject;
                        }
                    }
                    break;
                }
        }

        Debug.DrawRay(transform.position, direction, new Color(1, 0, 1), 10);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, proximity))
        {
            //Debug.Log(hit.transform.gameObject.name);
        }
    }

    public void LevelUp()
    {
        LevelUpgrades(1, 2, 0.05f, 0.5f);
    }
}

