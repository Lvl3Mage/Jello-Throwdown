using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    [SerializeField] NavSystem navigation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(navigation.NavToPlayer(transform.position));
        transform.position = (Vector2)transform.position + (navigation.NavToPlayer(transform.position) - (Vector2)transform.position).normalized *Time.deltaTime*5;
        // Debug.DrawLine(transform.position, navigation.NavToPlayer(), Color.green);
    }
}
