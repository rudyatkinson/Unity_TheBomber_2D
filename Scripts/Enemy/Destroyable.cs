using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an abstract class which includes that deactivate and VFX instantiations
/// <para>Must used with the destroyable objects</para>
/// </summary>
public abstract class Destroyable : MonoBehaviour
{
    protected abstract void Deactivate();

    /// <summary>
    /// Used to instantiate a fireVFX
    /// </summary>
    protected void InstantiateFire(
        GameObject fireVFX, 
        int fireCount, 
        Collider2D collider2d, 
        Transform transform)
    {
        for (int i = 0; i < fireCount; i++)
        {
            // Get bound size and bound center point to use later.
            var center = collider2d.bounds.center;
            var sizeX = collider2d.bounds.size.x;
            var sizeY = collider2d.bounds.size.y;

            // Find min and max point of x and min point of y coordinates. 
            var minX = center.x - (sizeX / 2);
            var maxX = center.x + (sizeX / 2);
            var minY = center.y - (sizeY / 2);

            // Get a random point via min and max points to instantiate vfx at random point.
            var rndX = Random.Range(minX, maxX);
            var firePos = new Vector2(rndX, minY);

            // Set a rotation to use later to instantiate vfx with true rotation.
            var rot = Quaternion.Euler(0, 0, 0);

            // Instantiating the fireVFX
            Instantiate(fireVFX,
                firePos,
                rot,
                transform);
        }
    }

    /// <summary>
    /// Used to instantiate an explosionVFX
    /// </summary>
    protected IEnumerator InstantiateExplosive(
        GameObject explosionVFX, 
        float explosionCd, 
        float explosionCdOffset, 
        Collider2D collider2d, 
        Transform transform)
    {
        while (true)
        {
            // Get bound size and bound center point to use later.
            var center = collider2d.bounds.center;
            var sizeX = collider2d.bounds.size.x;
            var sizeY = collider2d.bounds.size.y;

            // Find min and max point of x and y coordinates. 
            var minX = center.x - (sizeX / 2);
            var maxX = center.x + (sizeX / 2);
            var minY = center.y - (sizeY / 2);
            var maxY = center.y + (sizeY / 2);

            // Get a random point via min and max points to instantiate vfx at random point.
            var rndX = Random.Range(minX, maxX);
            var rndY = Random.Range(minY, maxY);
            var explosionPos = new Vector2(rndX, rndY);

            // Set a rotation to use later to instantiate vfx with true rotation.
            var rot = Quaternion.Euler(0, 0, 0);

            // Instantiating the explosionVFX
            Instantiate(explosionVFX,
                explosionPos,
                rot,
                transform);

            // Setting cooldown for wait to next explosion.
            var minSec = Random.Range(explosionCd - explosionCdOffset,
                explosionCd + explosionCdOffset);
            yield return new WaitForSeconds(minSec);
        }
    }
}
