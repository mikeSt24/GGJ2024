using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class player_parallax : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct parallax
    {
        public float parallax_ratio;
        public Vector3 initial_pos;
        public GameObject parallax_obj;

        public void InitializePos()
        {
            initial_pos = parallax_obj.GetComponent<Transform>().position;
        }

        public void MoveObject(Vector3 player_pos)
        {
            parallax_obj.transform.position = -player_pos * parallax_ratio;
        }
    };

    public List<parallax> prl_objs;
    void Start()
    {
        for(int i = 0; i < prl_objs.Count; ++i)
        {
            prl_objs[i].InitializePos();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(parallax prl in prl_objs)
        {
            prl.MoveObject(transform.position);
        }

    }
}
