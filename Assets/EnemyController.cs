using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    NavSystem navigation;
    const int JumpTriggerLayer = 7;
    // Start is called before the first frame update
    void Start()
    {
        navigation = GameObject.FindGameObjectWithTag("NavSystem").GetComponent<NavSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get Nav Target

        //Horizontal Movement

        //Check height differrence
            //if grounded and contacting jump trigger => jump
    }
}
