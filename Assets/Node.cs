using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public PathFinder_JPS.Way way;

    public GridCreator gridCreator;

    public bool walkable;

    public bool total_path;

    public List<Node> neighbor;
    public List<GameObject> colList;

    public int g, h, f;

    public Node parent;

    public void SetParent(Node p)
    {
        parent = p;
    }
    public Node GetParent() { return parent; }

    private void OnDrawGizmos()
    {
        
        if (walkable)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector2.one);
        }
        else
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position, Vector2.one);
        }
        if (total_path)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(transform.position, Vector2.one);
        }
    }

    private void Awake()
    {
        colList = new List<GameObject>();
        InitializeNode();
    }

    public void InitializeNode()
    {
        neighbor = new List<Node>();
        colList = new List<GameObject>();

        g = h = f = 0;
        g = int.MaxValue;
        f = int.MaxValue;

        total_path = false;

        parent = null;
        
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Collision") && !colList.Contains(collision.gameObject))
        {
            colList.Add(collision.gameObject);

            walkable = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Collision") && !colList.Contains(collision.gameObject))
        {
            colList.Add(collision.gameObject);

            walkable = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Collision") && colList.Contains(collision.gameObject))
        {
            colList.Remove(collision.gameObject);

            if (colList.Count <= 0)
                walkable = true;
        }
    }
    public Vector2 GetPosition()
    {
        return (Vector2)transform.position - new Vector2(0.5f, 0.5f);
    }
}
