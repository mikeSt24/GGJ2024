using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blank_ui_controller : MonoBehaviour
{
    public playerBlanksBehavior pbb;
    public int this_n = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this_n > pbb.current_blanks)
        {
            Destroy(gameObject);
        }
    }
}
