using UnityEngine;
using System.Collections;

public class Camera_for_person : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0F;

    [SerializeField]
    private Transform target;

    private void Awake()
    {
        if (target==null) target = FindObjectOfType<Person>().transform; 
    }

    private void Update()
    {
        if (target == null) target = FindObjectOfType<Person>().transform;
        Vector3 position = target.position;     position.z = -10.0F;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
	
}
