using UnityEngine;
using System.Collections;

public class attackTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("НЫЫЫЫЫЫАААААА!");
            //Destroy(other.gameObject);
        }
    }
}
