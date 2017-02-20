using UnityEngine;
using System.Collections;


public class TurretScript : MonoBehaviour
{

    public enum ATTSTYLE
    {
        NEAREST_FIRST = 0,
        FURTHEST_FIRST,
        TOTAL_STYLES
    }

    protected int towerLevel; //{ get; set; }
    protected int minAttackDamage; //{ get; set; }
    protected int maxAttackDamage; //{ get; set; }
    protected float timer; //{ get; set; }
    protected float attackSpeed; //{ get; set; }
    protected float proximity; //{ get; set; }
    protected ATTSTYLE attackStyle; //{ get; set; }
    protected Quaternion originalRotation;

    public int Level
    {
        get
        {
            return towerLevel;
        }
        set
        {
            towerLevel = value;
        }
    }

    public int MinAtt
    {
        get
        {
            return minAttackDamage;
        }
        set
        {
            minAttackDamage = value;
        }
    }

    public int MaxAtt
    {
        get
        {
            return maxAttackDamage;
        }
        set
        {
            maxAttackDamage = value;
        }
    }

    public float Timer
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
        }
    }

    public float AttSpd
    {
        get
        {
            return attackSpeed;
        }
        set
        {
            attackSpeed = value;
        }
    }

    public float Proximity
    {
        get
        {
            return proximity;
        }
        set
        {
            proximity = value;
        }
    }

    public ATTSTYLE Style
    {
        get
        {
            return attackStyle;
        }
        set
        {
            attackStyle = value;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        towerLevel = 1;
        timer = 0;
        attackStyle = ATTSTYLE.NEAREST_FIRST;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (EnemiesInAttackRadius() == null)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                Attacking(EnemiesInAttackRadius());
                timer = 0;
            }
        }
    }

    // Checks if there are any colliders within the radius/proximity of where the Tower is
    protected virtual Collider[] EnemiesInAttackRadius()
    {
        // Return an array with all colliders touching or inside the sphere
        Collider[] enemiesColliders = Physics.OverlapSphere(transform.position, proximity, LayerMask.GetMask("Monster"));

        // If there are no enemy colliders
        if(enemiesColliders.Length <= 0)
            return null;

        return enemiesColliders;
    }

    // Different attacks based on the type specified
    protected virtual void Attacking(Collider[] enemies)
    {
        
    }

    protected virtual void LevelUpgrades(int min, int max, float attspd, float prox)
    {
        minAttackDamage += min;
        maxAttackDamage += max;
        if (attackSpeed - attspd > 0)
            attackSpeed -= attspd;
        proximity += prox;
    }
}
