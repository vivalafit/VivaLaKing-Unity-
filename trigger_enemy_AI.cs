using UnityEngine;
using System.Collections;

public class trigger_enemy_AI : MonoBehaviour
{
    [SerializeField]
    private float dist;
    //
    public Transform target;
    public GameObject enemyscript;
    public Transform other;
  
 
   
    void Start()
    {

     
    }
    void Update()
    {
        Example();
    }
 

    void Example()
    {
        if (other)
        {
            dist = Vector3.Distance(other.position, transform.position);
            Debug.Log("Distance to other: " + dist);
            if (dist > 6)
            {
                enemyscript.GetComponent<EnemyAI>().enabled = false;
            }
            else if (dist < 6)
            {
                enemyscript.GetComponent<EnemyAI>().enabled = true;
            }
        }
      
    }
}