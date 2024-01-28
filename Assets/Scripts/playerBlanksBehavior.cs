using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBlanksBehavior : MonoBehaviour
{
    public int max_blanks;
    public int current_blanks;
    public Animator explosion_animator;
    // Start is called before the first frame update
    void Start()
    {
        current_blanks = max_blanks;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            if(current_blanks > 0)
            {
                explosion_animator.SetBool("exploding", true);
                DestroyAllBullets();
                current_blanks--;
            }
        }
    }

    void DestroyAllBullets()
    {
        var objectList = Object.FindObjectsByType<BasicProjectileBehaviour>(FindObjectsSortMode.None);

        for(int i = 0; i < objectList.Length; i++) 
        {
            float distance = Vector3.Magnitude(objectList[i].transform.position - transform.position);
            if (distance < 10)
            {
                Destroy(objectList[i].gameObject);
            }
        }
    }
}
