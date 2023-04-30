using System;
using UnityEngine;

public class Cloud : AreaOfInterest
{
    public AOIChannel cloudChannel;
    public float speed;
    
    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    public override AOIChannel GetOutputChannel()
    {
        return cloudChannel;
    }

    private void FixedUpdate()
    {
        var transformPosition = transform.position;
        transformPosition.x -= speed * Time.deltaTime;
        transform.position = transformPosition;
    }
}
