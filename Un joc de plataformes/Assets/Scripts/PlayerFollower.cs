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
        transform.position = new Vector3(player.transform.position.x, getPositionY(), player.transform.position.z) - Vector3.forward * 10; //Vector3.Lerp(player.transform.position, player.transform.position - offset, Time.deltaTime);
    }

    //limitate position 'y' of the camera(background limits)
    float getPositionY()
    {
        float positionY = player.transform.position.y;
        //9.58
        if (player.transform.position.y > 9.58f)
        {
            positionY = 9.58f;
        }

        if (player.transform.position.y < -5.41f)
        {
            positionY = -5.41f;
        }

        return positionY;
    }
}
