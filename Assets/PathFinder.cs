using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public int btn_x, btn_y;
    List<Node> OpenSet;
    List<Node> CloseSet;

    public GridCreator gridCreator;

    List<Node> totalPath;

    Node start;
    Node end;
    Node cur;

    public bool can_diagonal_move;

    public void OnClick()
    {
        FindPath(gridCreator.nodes[new Vector2(0, 0)], gridCreator.nodes[new Vector2(btn_x, btn_y)]);
    }

    private void Awake()
    {
        OpenSet = new List<Node>();
        CloseSet = new List<Node>();
        totalPath = new List<Node>();
    }


    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        FindPath(gridCreator.nodes[new Vector2(0, 0)], gridCreator.nodes[new Vector2(3, 4)]);
    }

    public static int GetH(Node p, Node q) => (int)(Mathf.Abs(p.transform.position.x - q.transform.position.x) + Mathf.Abs(p.transform.position.y - q.transform.position.y)) * 10;

    int GetG(Node src, Node dst)
    {
        int dist_x;
        int dist_y;
        dist_x = (int)(src.transform.position.x - dst.transform.position.x);
        dist_y = (int)(src.transform.position.y - dst.transform.position.y);
        return dist_x == 0 || dist_y == 0 ? 10 : 14;
    }

    Node GetLowestNodeInOpenSet()
    {
        if (OpenSet.Count <= 0)
            return null;
        Node result = OpenSet[0];
        foreach (var p in OpenSet)
        {
            if (p.f < result.f)
                result = p;
        }
        return result;
    }

    void SetNeighbors(Node tmp)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;
                if (!can_diagonal_move && !(dx == 0 || dy == 0))
                    continue;
                
                // 
                if (gridCreator.nodes.ContainsKey(new Vector2(tmp.transform.position.x + dx - 0.5f, tmp.transform.position.y + dy - 0.5f)))
                {
                    Node p = gridCreator.nodes[new Vector2(tmp.transform.position.x + dx - 0.5f, tmp.transform.position.y + dy - 0.5f)];

                    if (!p.walkable)
                        continue;

                    // if p is diagonal position
                    if (Mathf.Abs(p.transform.position.x - tmp.transform.position.x) == 1 && Mathf.Abs(p.transform.position.y - tmp.transform.position.y) == 1)
                    {
                        // if p is left top
                        if (p.transform.position.x - tmp.transform.position.x == 1 && p.transform.position.y - tmp.transform.position.y == 1)
                        {
                            // if p's down, left is !walkable
                            if (gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x - 1, p.transform.position.y)) && gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x, p.transform.position.y - 1)))
                            {
                                if (!gridCreator.nodes[new Vector2(p.transform.position.x - 1, p.transform.position.y)].walkable &&
                                    !gridCreator.nodes[new Vector2(p.transform.position.x, p.transform.position.y - 1)].walkable)
                                {
                                    continue;
                                }

                            }
                        }
                        // if p is right top
                        if (p.transform.position.x - tmp.transform.position.x == -1 && p.transform.position.y - tmp.transform.position.y == 1)
                        {
                            // if p's down, right is !walkable
                            if (gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x, tmp.transform.position.y - 1)) && gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x + 1, tmp.transform.position.y)))
                            {
                                if (!gridCreator.nodes[new Vector2(p.transform.position.x, tmp.transform.position.y - 1)].walkable &&
                                    !gridCreator.nodes[new Vector2(p.transform.position.x + 1, tmp.transform.position.y)].walkable)
                                {
                                    continue;
                                }

                            }
                        }
                        // if p is left down
                        if (p.transform.position.x - tmp.transform.position.x == -1 && p.transform.position.y - tmp.transform.position.y == -1)
                        {
                            // if p's up, right is !walkable
                            if (gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x, tmp.transform.position.y + 1)) && gridCreator.nodes.ContainsKey(new Vector2(p.transform.position.x + 1, tmp.transform.position.y)))
                            {
                                if (!gridCreator.nodes[new Vector2(p.transform.position.x, tmp.transform.position.y + 1)].walkable &&
                                    !gridCreator.nodes[new Vector2(p.transform.position.x + 1, tmp.transform.position.y)].walkable)
                                {
                                    continue;
                                }

                            }
                        }
                        // if p is right down
                        if (p.transform.position.x - tmp.transform.position.x == 1 && p.transform.position.y - tmp.transform.position.y == -1)
                        {
                            // if p's up, left is !walkable
                            if (gridCreator.nodes.ContainsKey(new Vector2(tmp.transform.position.x - 1, tmp.transform.position.y)) && gridCreator.nodes.ContainsKey(new Vector2(tmp.transform.position.x, tmp.transform.position.y + 1)))
                            {
                                if (!gridCreator.nodes[new Vector2(p.transform.position.x, tmp.transform.position.y + 1)].walkable &&
                                    !gridCreator.nodes[new Vector2(p.transform.position.x - 1, tmp.transform.position.y)].walkable)
                                {
                                    continue;
                                }

                            }
                        }
                    }

                    if (!OpenSet.Contains(p) && !CloseSet.Contains(p))
                    {
                        p.SetParent(tmp);
                        p.g = GetG(tmp, p) + tmp.g;
                        p.h = GetH(p, end);
                        p.f = p.g + p.h;
                    }
                    tmp.neighbor.Add(p);
                }
            }
        }
    }

    public void FindPath(Node src, Node dst)
    {
        if (OpenSet == null) OpenSet = new List<Node>();
        OpenSet.Clear();
        if (CloseSet == null) CloseSet = new List<Node>();
        CloseSet.Clear();
        if (totalPath == null) totalPath = new List<Node>();
        totalPath.Clear();

        gridCreator.InitalizeNodes();

        start = src;
        end = dst;
        start.g = 0;

        OpenSet.Add(src);

        while (OpenSet.Count > 0)
        {
            cur = GetLowestNodeInOpenSet();
            // 길 찾기 완료
            if (cur == end)
            {
                //End Function
                SetTotalPath();
                return;
            }

            OpenSet.Remove(cur);
            CloseSet.Add(cur);

            SetNeighbors(cur);
            foreach (var itr in cur.neighbor)
            {
                // If itr is Wall or Collide Obj Node, Dont Search
                if (!itr.walkable && itr != end)
                    continue;
                if (CloseSet.Contains(itr))
                    continue;
                if (!OpenSet.Contains(itr))
                {
                    OpenSet.Add(itr);
                }
                int tentative_g = cur.g + GetG(cur, itr);
                if (tentative_g >= itr.g)
                    continue;
                itr.SetParent(cur);
                itr.g = tentative_g;
                itr.h = GetH(itr, src);
                itr.f = itr.g + GetH(itr, dst);
            }
        }
        SetTotalPath();
    }

    void SetTotalPath()
    {
        Node itr = end;
        while (itr != null)
        {
            totalPath.Add(itr);
            itr.total_path = true;
            itr = itr.GetParent();
        }
        DebugPath();
    }

    void DebugPath()
    {
        for (int i = 0; i < totalPath.Count; i++)
        {
            Debug.Log(totalPath[i].transform.position - new Vector3(0.5f , 0.5f));
        }
    }
}
