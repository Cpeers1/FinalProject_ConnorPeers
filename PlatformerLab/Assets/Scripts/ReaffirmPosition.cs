using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReaffirmPosition
{
    public static void testForInfringement()
    {
        List<Collider2D> cols = new List<Collider2D>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            foreach(GameObject go in SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                cols.AddRange(go.GetComponentsInChildren<Collider2D>(true));
            }

        }
        Debug.Log(cols.Count);
        cols.TrimExcess();

        ////Make sure we are not checking with the infringer
        //for (int i = 0; i < cols.Count; i ++)
        //{
        //    if (cols[i] == possibleInfringer)
        //        cols.RemoveAt(i);
        //}

        //lets try to sketch out a custom collider.


        foreach (Collider2D col2d1 in cols)
        {
            foreach (Collider2D col2d2 in cols)
            {
                //if((col2d2.name == "ObjectColForOtherObjects" && col2d1.name.Contains("QuestionMarkBox")))
                //{
                //    Debug.Log(col2d1);
                //    //Debug.Log(col2d1.bounds.center);
                //    //Debug.Log(col2d1.bounds);
                //    Debug.Log(col2d2);
                //    //Debug.Log(col2d2.bounds);
                //    //Debug.Log(col2d2.bounds.center);
                //    //Debug.Break();
                //}

                if
                    (
                        (col2d1.isActiveAndEnabled && !col2d1.isTrigger) &&
                        (col2d2.isActiveAndEnabled && !col2d2.isTrigger)
                        && !Physics2D.GetIgnoreCollision(col2d1, col2d2)
                        && !Physics2D.GetIgnoreLayerCollision(col2d1.gameObject.layer, col2d2.gameObject.layer)
                        && col2d1 != col2d2
                        && (col2d1.GetType() != typeof(EdgeCollider2D) && col2d2.GetType() != typeof(EdgeCollider2D))
                    )
                {


                    Rect oneRect = new Rect(col2d1.bounds.min, new Vector2(col2d1.bounds.size.x, col2d1.bounds.size.y));
                    Rect twoRect = new Rect(col2d2.bounds.min, new Vector2(col2d2.bounds.size.x, col2d2.bounds.size.y));

                    if (col2d1.bounds.Intersects(col2d2.bounds) && (col2d2.GetComponent<Rigidbody2D>() != null || col2d2.gameObject.GetComponentInParent<Rigidbody2D>() != null))
                    {
                        Debug.Log("Yeah");
                        Vector3 NormalizedDirection = (twoRect.position - oneRect.position).normalized;
                        //Debug.Log(NormalizedDirection);
                        int e = 0;
                        foreach (Collider2D col2d3 in cols)
                        {

                            if (col2d3 != col2d1 && col2d3 != col2d2 && col2d3.enabled && !col2d3.isTrigger && !Physics2D.GetIgnoreLayerCollision(col2d2.gameObject.layer, col2d3.gameObject.layer) && col2d3.GetType() != typeof(EdgeCollider2D))
                            {

                                while (col2d2.bounds.Intersects(col2d3.bounds))
                                {
                                    e++;
                                    Debug.Log(e);
                                    //KEEP MOVING. Were still in something.
                                    col2d2.GetComponentInParent<Rigidbody2D>().gameObject.transform.position += e * (NormalizedDirection + col2d2.bounds.extents);
                                    Debug.Log("noep");
                                }

                            }
                        }
                            //LETS TRY USING RAYS.
                        Ray testRay = new Ray(col2d1.bounds.center, NormalizedDirection);
                        Debug.DrawRay(testRay.origin, testRay.direction, Color.blue, 1f);

                        
                    }
                        
                    //else
                    //{
                    //    if (col2d1.bounds.Intersects(col2d2.bounds) && col2d1.GetComponent<Rigidbody2D>() != null)
                    //    {
                    //        Vector3 NormalizedDirection = (oneRect.position - twoRect.center).normalized;
                    //        //Debug.Log(NormalizedDirection);
                    //        int e = 0;
                    //        foreach (Collider2D col2d3 in cols)
                    //        {
                    //            if (col2d3 != col2d1 && col2d3 != col2d2 && col2d3.enabled && !col2d3.isTrigger && !Physics2D.GetIgnoreLayerCollision(col2d1.gameObject.layer, col2d3.gameObject.layer) && col2d3.GetType() != typeof(EdgeCollider2D))
                    //            {

                    //                if (col2d1.bounds.Intersects(col2d3.bounds))
                    //                {
                    //                    e++;
                    //                    Debug.Log(e);
                    //                    //KEEP MOVING. Were still in something.
                    //                    col2d2.transform.position += e * (NormalizedDirection + col2d2.bounds.extents);
                    //                    Debug.Log("noep");
                    //                }

                    //            }
                    //        }
                    //    }
                    //}

                }
            }
        }
    }
}
