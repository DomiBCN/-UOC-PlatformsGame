using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

    public Transform player;

    Vector3 offset;

    private void Start()
    {
        //offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - Vector3.forward * 10; //Vector3.Lerp(player.transform.position, player.transform.position - offset, Time.deltaTime);
    }
}
