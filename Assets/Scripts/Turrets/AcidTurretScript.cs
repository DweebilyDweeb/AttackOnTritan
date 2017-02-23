using UnityEngine;
using System.Collections;

public class AcidTurretScript : TurretScript {

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
        proximity = 10f;
        direction = new Vector3(0, 0, 0);
        rotateSpd = 3f;
        attackStyle = ATTSTYLE.FURTHEST_FIRST;
    }
	
	// Update is called once per frame
	protected override void Update () {
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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, proximity))
        {
            Debug.Log(hit.transform.gameObject.name);
        }
    }

    public void LevelUp()
    {
        if (towerLevel == 1)
        {
            GameObject turretbase = Resources.Load("Turrets/Acid/Base 1") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Acid/Turret 1") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
            GameObject barrel = Resources.Load("Turrets/Acid/Barrel 1") as GameObject;
            transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (towerLevel == 2)
        {
            GameObject turretbase = Resources.Load("Turrets/Acid/Base 2") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretbarrel = Resources.Load("Turrets/Acid/Turret 2") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
            GameObject barrel = Resources.Load("Turrets/Acid/Barrel 2") as GameObject;
            transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretbarrel.GetComponent<MeshFilter>().sharedMesh;
        }
        LevelUpgrades(1, 2, 0.05f, 0.5f);
        towerLevel += 1;
    }
}
