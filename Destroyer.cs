using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float lifeLine = 5f;

    // Update is called once per frame
    void Update()
    {
        if (lifeLine > 0)
        {
            lifeLine -= Time.deltaTime;
            if (lifeLine <= 0)
            {
                Destruction();
            }
        }

        if (this.transform.position.y <= -5)
        {
            Destruction();
        }
    }

        void OnCollisionEnter(Collision coll)
        {
            if(coll.gameObject.name == "destroyer")
            { Destruction(); }
        }
	

    public void Destruction ()
    {
        Destroy(this.gameObject);
    }
}
