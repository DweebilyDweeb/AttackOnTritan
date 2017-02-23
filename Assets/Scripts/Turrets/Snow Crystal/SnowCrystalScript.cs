using UnityEngine;
using System.Collections;

public class SnowCrystalScript : TurretScript
{
    public GameObject Snow;

    private GameObject target;
    private GameObject curr;
    private GameObject particles;


    private Vector3 direction;

    private float Sloweffect;
    private float Snowradius;

    // Use this for initialization
    protected override void Start()
    {
        //onGrid; 
        base.Start();
        attackSpeed = 0.5f;
        proximity = 10.0f;
        direction = new Vector3(0, 0, 0);
        Sloweffect = 0.9f;
        Snowradius = 6.0f;
        timer = attackSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (curr)
        {

            direction = curr.transform.position - transform.position;
            particles.transform.position = curr.transform.position;
            if (direction.magnitude >= proximity)
            {
                Destroy(curr.GetComponent<SnowStormScript>());
                Destroy(particles);
                curr = null;
                target = null;
                timer = attackSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LevelUp();
        }
    }

    protected override Collider[] EnemiesInAttackRadius()
    {
        if (base.EnemiesInAttackRadius() == null)
        {
            curr = null;
            target = null;
        }

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
                        if (!enemy.GetComponent<SnowStormScript>() || enemy.GetComponent<SnowStormScript>().Slow() < Sloweffect)
                        {
                            if ((transform.position - enemy.transform.position).magnitude < nearestDistance)
                            {
                                nearestDistance = (enemy.transform.position - transform.position).magnitude;
                                target = enemy.transform.gameObject;
                            }
                        }
                       
                    }
                    if (!curr && target)
                    {
                        Slow(target);
                        curr = target;
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
        if (towerLevel == 1)
        {
            GameObject turretbase = Resources.Load("Turrets/Snow/Base 1") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretcrystal = Resources.Load("Turrets/Snow/Crystal 1") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretcrystal.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (towerLevel == 2)
        {
            GameObject turretbase = Resources.Load("Turrets/Snow/Base 2") as GameObject;
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = turretbase.GetComponent<MeshFilter>().sharedMesh;
            GameObject turretcrystal = Resources.Load("Turrets/Snow/Crystal 2") as GameObject;
            transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = turretcrystal.GetComponent<MeshFilter>().sharedMesh;
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        LevelUpgrades(0.1f, 6.0f);
        towerLevel += 1;
    }

    public void Slow(GameObject target)
    {
        if (!target.GetComponent<SnowStormScript>())
        {
            target.AddComponent<SnowStormScript>();
            target.GetComponent<SnowStormScript>().Snow(Snowradius, Sloweffect, true, target);

            particles = Instantiate(Snow);
            var x = Snow.GetComponent<ParticleSystem>().shape;
            x.radius = Snowradius;
        }
    }

    private void LevelUpgrades(float slow, float range)
    {
        Sloweffect += slow;
        proximity += range;
    }
}
