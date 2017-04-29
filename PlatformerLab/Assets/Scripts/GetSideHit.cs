using UnityEngine;
using System.Collections;

public class GetSideHit : MonoBehaviour
{

    /************************************************************
    ** Make sure to add rigidbodies to your objects.
    ** Place this script on your object not object being hit
    ** this will only work on a Cube being hit 
    ** it does not consider the direction of the Cube being hit
    ** remember to name your C# script "GetSideHit"
    ************************************************************/

    public enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    static public HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit)
    {

        HitDirection hitDirection = HitDirection.None;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);

        if (Physics.Raycast(MyRay, out MyRayHit))
        {

            if (MyRayHit.collider != null)
            {

                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up) { hitDirection = HitDirection.Top; }
                if (MyNormal == -MyRayHit.transform.up) { hitDirection = HitDirection.Bottom; }
                if (MyNormal == MyRayHit.transform.forward) { hitDirection = HitDirection.Forward; }
                if (MyNormal == -MyRayHit.transform.forward) { hitDirection = HitDirection.Back; }
                if (MyNormal == MyRayHit.transform.right) { hitDirection = HitDirection.Right; }
                if (MyNormal == -MyRayHit.transform.right) { hitDirection = HitDirection.Left; }
            }
        }
        return hitDirection;
    }

    static public HitDirection HitDirection2Test(Collision2D col)
    {
        Vector3 hit = col.contacts[0].normal;
        Debug.Log(hit);
        float angle = Vector3.Angle(hit, Vector3.up);
        HitDirection hitDirection = HitDirection.None;

        if (Mathf.Approximately(angle, 0))
        {
            //Down
            hitDirection = HitDirection.Bottom;
        }
        if (Mathf.Approximately(angle, 180))
        {
            //Up
            hitDirection = HitDirection.Top;
        }
        if (Mathf.Approximately(angle, 90))
        {
            // Sides
            Vector3 cross = Vector3.Cross(Vector3.forward, hit);
            if (cross.y > 0)
            { // left side of the player
                hitDirection = HitDirection.Left;
            }
            else
            { // right side of the player
                hitDirection = HitDirection.Right;
            }
        }
        return hitDirection;
    }

    //static public HitDirection HitDirection2Test(Collider2D col)
    //{
    //    Vector3 hit = 
    //    Debug.Log(hit);
    //    float angle = Vector3.Angle(hit, Vector3.up);
    //    HitDirection hitDirection = HitDirection.None;

    //    if (Mathf.Approximately(angle, 0))
    //    {
    //        //Down
    //        hitDirection = HitDirection.Bottom;
    //    }
    //    if (Mathf.Approximately(angle, 180))
    //    {
    //        //Up
    //        hitDirection = HitDirection.Top;
    //    }
    //    if (Mathf.Approximately(angle, 90))
    //    {
    //        // Sides
    //        Vector3 cross = Vector3.Cross(Vector3.forward, hit);
    //        if (cross.y > 0)
    //        { // left side of the player
    //            hitDirection = HitDirection.Left;
    //        }
    //        else
    //        { // right side of the player
    //            hitDirection = HitDirection.Right;
    //        }
    //    }
    //    return hitDirection;
    //}

    //public static Rect[] Love2DTileMergingTranslated(Bounds[] rectangles)
    //{
      
    ////Here's how the rectangles would be used for physics.
    ////-- Use contents of rectangles to create physics bodies
    ////--phys_world is the world, wall_rects is the list of...
    ////--wall rectangles

    ////for _, r in ipairs(rectangles) do
    ////                local start_x = r.start_x * TILE_SIZE
    ////    local start_y = r.start_y * TILE_SIZE
    ////    local width = (r.end_x - r.start_x + 1) * TILE_SIZE
    ////    local height = (r.end_y - r.start_y + 1) * TILE_SIZE

    ////    local x = start_x + (width / 2)
    ////    local y = start_y + (height / 2)

    ////    local body = love.physics.newBody(phys_world, x, y, 0, 0)
    ////    local shape = love.physics.newRectangleShape(body, 0, 0,
    ////      width, height)

    ////    shape: setFriction(0)

    ////    table.insert(wall_rects, { body = body, shape = shape})
    ////end
    //}
}