using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject tar;

    [SerializeField] private float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo(tar);
    }

    private void MoveTo(GameObject tar)
    {
        var tarPosition = tar.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, tarPosition, speed * Time.deltaTime);
    }
}
