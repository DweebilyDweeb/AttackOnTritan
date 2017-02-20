using UnityEngine;
using System.Collections;

public class TeslaScript : TurretScript {

    private GameObject target;
    private Vector3 direction;

    private GameObject lightning;
    private GameObject lightning2;

    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        minAttackDamage = 5;
        maxAttackDamage = 8;
        attackSpeed = 0.25f;
        proximity = 4f;
        direction = new Vector3(0, 0, 0);
        lightning = transform.GetChild(0).gameObject;
        lightning2 = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target)
        {
            if (!lightning.activeInHierarchy)
                lightning.SetActive(true);
            if (!lightning2.activeInHierarchy)
                lightning2.SetActive(true);

            direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction.normalized);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, direction.magnitude * 0.28f);
        }
        else
        {
            if (lightning.activeInHierarchy)
                lightning.SetActive(false);
            if (lightning2.activeInHierarchy)
                lightning2.SetActive(false);
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

        //Debug.DrawRay(transform.position, direction, new Color(1, 0, 1), 10);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, proximity))
        {
           
        }
    }

    public void LevelUp()
    {
        LevelUpgrades(1, 2, 0.05f, 0.5f);
    }
}
