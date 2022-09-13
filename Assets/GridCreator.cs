using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public Node nodeObj;

    public Dictionary<Vector2, Node> nodes;
    public Vector2 roomSize;

    private void Awake()
    {
        nodes = new Dictionary<Vector2, Node>();
        
    }

    private void Start()
    {
        CreateGrids();
    }

    public void InitalizeNodes()
    {
        foreach(var itr in nodes.Values)
        {
            itr.InitializeNode();
        }
    }

    void CreateNode(Vector2 pos)
    {
        Node tmp = Instantiate(nodeObj);
        tmp.transform.name = pos.ToString();
        tmp.transform.position = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
        tmp.gridCreator = this;
        if (pos.x == 1 && pos.y == 1)
            tmp.walkable = false;
        if (pos.x == 0 && pos.y == 0)
            tmp.g = tmp.f = 0;
        nodes.Add(pos, tmp);
    }

    void CreateGrids()
    {
        for (int x = -(int)(roomSize.x / 2); x < roomSize.x / 2; x++)
        {
            for (int y = -(int)(roomSize.y / 2); y < roomSize.y / 2; y++)
            {
                CreateNode(new Vector2(x, y));
            }
        }
    }

    

}
