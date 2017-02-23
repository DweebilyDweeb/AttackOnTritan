using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour {

    [SerializeField]
    private List<GameObject> tritanCrystals;
    // Malcolm's Stuff
    Animator anim;
    public GameObject check;
    private bool destroyed;
    float lifetime;
    // Use this for initialization
    void Start () {
        //anim = gameObject.GetComponent<Animator>();
        anim = gameObject.GetComponent(typeof(Animator)) as Animator;
        destroyed = false;
        lifetime = 5;
        check = GameObject.Find("Main Menu Camera");

	}
	
	// Update is called once per frame
	void Update () {
        if(destroyed == true)
        {
            if (lifetime > 0)
            {
                lifetime -= Time.deltaTime;
            }
            else
            {
                Reset();
            }
            return;
        }
	    foreach(GameObject crystal in tritanCrystals)
        {
            if (crystal.GetComponent<Health>().IsDead())
            {
                anim.SetTrigger("Lose");
                destroyed = true;
            }

        }   
	}
    public void Reset()
    {
        destroyed = false;
        check.GetComponent<Menu>().GoToMainMenu();
    }
}
