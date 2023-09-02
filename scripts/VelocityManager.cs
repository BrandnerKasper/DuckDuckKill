using Godot;
using System;

public partial class VelocityManager : Node
{
    void addShotGunVelocity()
    {
        // This will get the parent and find a child node called "ShotGun"
        var shotGunNode = GetParent().GetNode("shotgun");

        // This will cast it to your ShotGun script type
        var shotGun = shotGunNode as ShotGun;

        if (shotGun != null) // Just to make sure we got it
        {
           // shotGun.Velocity += 10;
        }
    }
    
}
