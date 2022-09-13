using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder_JPS : MonoBehaviour
{
    public int btn_x, btn_y;

    public enum Way
    {
        no, left, right, up, down, diagonal_up_right, diagonal_up_left, diagonal_down_right, diagonal_down_left
    };

    public GridCreator gridCreator;


    public List<Node> OpenSet;
    public List<Node> CloseSet;
    public List<Node> totalPath;

    public Node start;
    public Node end;
    public Node cur;

    public static int GetH(Node p, Node q) => (int)(Mathf.Abs(p.transform.position.x - q.transform.position.x) + Mathf.Abs(p.transform.position.y - q.transform.position.y)) * 10;

    int GetG(Node src, Node dst)
    {
        int dist_x;
        int dist_y;
        dist_x = Mathf.Abs((int)(src.transform.position.x - dst.transform.position.x));
        dist_y = Mathf.Abs((int)(src.transform.position.y - dst.transform.position.y));
        return dist_x == 0 || dist_y == 0 ? 10 * (dist_x + dist_y) : 14 * dist_x; // if Diagonal, 14 * dist_x == 14 * dist_y
    }

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


    private void Start()
    {
        FindPath(gridCreator.nodes[new Vector2(0, 0)], gridCreator.nodes[new Vector2(3, 4)]);
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

    bool IsCorner(Node node)
    {
        // Right - Right Top Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, 1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, 1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, 1)].walkable)
            {
                return true;
            }
        }

        // Right - Right Down Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, -1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, -1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, -1)].walkable)
            {
                return true;
            }
        }

        // Left - Left Up Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, 1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, 1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, 1)].walkable)
            {
                return true;
            }
        }

        // Left - Left Down Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, -1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, -1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, -1)].walkable)
            {
                return true;
            }
        }

        // Up - Up Left Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(-1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, 1)].walkable)
            {
                return true;
            }
        }

        // Up - Up Right Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, 1)].walkable)
            {
                return true;
            }
        }

        // Down - Down Left Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(-1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, -1)].walkable)
            {
                return true;
            }
        }

        // Down - Down Right Corner
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, -1)].walkable)
            {
                return true;
            }
        }

        return false;
    }

    bool IsCornerDiagonal(Node node)
    {
        // Left Top Corner 1
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(-1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, 1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }
        // Left Down Corner - 6
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(-1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, -1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }

        // Right Top Corner - 3
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, 1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }
        // Right Down Corner - 8
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 0))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(1, 0)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, -1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }

        //Right Down Top Corner - 5
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, 1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, 1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(1, 1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }

        // 4 
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, -1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, -1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, -1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, -1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }

        // 7
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(0, 1))
            && gridCreator.nodes.ContainsKey(node.GetPosition() + new Vector2(-1, 1)))
        {
            if (!gridCreator.nodes[node.GetPosition() + new Vector2(0, 1)].walkable
                && gridCreator.nodes[node.GetPosition() + new Vector2(-1, 1)].walkable)
            {
                Debug.Log("Corner is " + node.name);
                return true;
            }
        }

        return false;
    }

    void AddOpenSet(Node node, Way way)
    {
        if (CloseSet.Contains(node))
        {
            return;
        } 
        if (OpenSet.Contains(node))
        {
            if (GetG(cur, node) + cur.g < node.g)
            {
                node.g = GetG(cur, node) + cur.g;
                node.h = GetH(node, end);
                node.f = node.g + node.h;
                node.SetParent(cur);
            }
        }
        else
        {
            Debug.Log(node.name + " Added");
            OpenSet.Add(node);
            node.way = way;
            node.SetParent(cur);
            node.g = GetG(cur, node) + cur.g;
            node.h = GetH(node, end);
            node.f = node.g + node.h;
        }
    }

    Node SearchWay(Node node, Way way)
    {
        Vector2 vec = new Vector2();
        switch (way)
        {
            case Way.right:
                vec = Vector2.right;
                break;
            case Way.left:
                vec = Vector2.left;
                break;
            case Way.up:
                vec = Vector2.up;
                break;
            case Way.down:
                vec = Vector2.down;
                break;
            default: return null;
        }
        if (!node.walkable)
            return null;

        if (node == end)
        {
            Debug.Log("end");
            return node;
        }

        if (IsCorner(node))
        {
            Debug.Log("Corner");
            return node;
        }

        if (gridCreator.nodes.ContainsKey(node.GetPosition() + vec))
            return SearchWay(gridCreator.nodes[node.GetPosition() + vec], way);
        else
            return null;
    }

    Node SearchDiagonal(Node node, Way vertical, Way horizontal)
    {
        //Debug.Log(vertical.ToString() + " " + horizontal.ToString() + " " + "Diagonal search " + node.name);
        
        Vector2 vec1 = new Vector2();
        Vector2 vec2 = new Vector2();
        switch (vertical)
        {
            case Way.up:
                vec1 = Vector2.up;
                break;
            case Way.down:
                vec1 = Vector2.down;
                break;
            default:
                Debug.Log("??");
                return null;
        }
        switch (horizontal)
        {
            case Way.right:
                vec2 = Vector2.right;
                break;
            case Way.left:
                vec2 = Vector2.left;
                break;
            default:
                Debug.Log("??");
                return null;
        }
        if (!node.walkable)
            return null;

        if (node == end)
        {
            return node;
        }

        if (IsCornerDiagonal(node))
        {
            return node;
        }

        Node cacheVertical = null;
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + (vertical == Way.up ? Vector2.up : Vector2.down)))
            cacheVertical = SearchWay(gridCreator.nodes[node.GetPosition() + (vertical == Way.up ? Vector2.up : Vector2.down)], vertical);

        Node cacheHorizontal = null;
        if (gridCreator.nodes.ContainsKey(node.GetPosition() + (horizontal == Way.right ? Vector2.right : Vector2.left)))
            cacheHorizontal = SearchWay(gridCreator.nodes[node.GetPosition() + (horizontal == Way.right ? Vector2.right : Vector2.left)], horizontal);

        //if (cacheVertical != null)
        //    Debug.Log("cacheVertical = " + cacheVertical + "node = " + node.name);
        //if (cacheHorizontal !=null)
        //    Debug.Log("cacheHorizontal = " + cacheHorizontal + "node = " + node.name);
        if (cacheVertical != null || cacheHorizontal != null)
        {
            //Debug.Log(cacheVertical == null ? cacheHorizontal.name : cacheVertical.name + " " + node.name);
            //return cacheVertical == null ? cacheHorizontal : cacheVertical;
            return node;
        }

        if (gridCreator.nodes.ContainsKey(node.GetPosition() + vec1 + vec2))
            return SearchDiagonal(gridCreator.nodes[node.GetPosition() + vec1 + vec2], vertical, horizontal);
        else
            return null;
    }

    void FindPath(Node src, Node dst)
    {
        OpenSet.Clear();
        CloseSet.Clear();
        totalPath.Clear();
        gridCreator.InitalizeNodes();

        start = src;
        end = dst;
        Node cache;
        this.cur = src;
        CloseSet.Add(src);

        // 여기서부터 아래처럼 src에서 시작하는게 아니라 src 이웃에서 시작하도록 바꾸기.
        // Straight Way
        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.right))
        {
            cache = SearchWay(FindNextWayNode(src, Way.right), Way.right);
            if (cache != null)
            {
                Debug.Log("Right");
                AddOpenSet(cache, Way.right);
                Debug.Log(OpenSet.Count);
            }
        }

        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.left))
        {
            cache = SearchWay(FindNextWayNode(src, Way.left), Way.left);
            if (cache != null)
            {
                Debug.Log("Left");
                AddOpenSet(cache, Way.left);
                Debug.Log(OpenSet.Count);
            }
        }

        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.up))
        {
            cache = SearchWay(FindNextWayNode(src, Way.up), Way.up);
            if (cache != null)
            {
                Debug.Log("Up");
                AddOpenSet(cache, Way.up);
                Debug.Log(OpenSet.Count);
            }
        }

        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.down))
        {
            cache = SearchWay(FindNextWayNode(src, Way.down), Way.down);
            if (cache != null)
            {
                Debug.Log("Down");
                AddOpenSet(cache, Way.down);
                Debug.Log(OpenSet.Count);
            }
        }

        // Diagonal Way
        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.up + Vector2.right))
        {
            cache = SearchDiagonal(FindNextWayNode(src, Way.diagonal_up_right), Way.up, Way.right);
            if (cache != null)
            {
                Debug.Log("Right up");
                AddOpenSet(cache, Way.diagonal_up_right);
                Debug.Log(OpenSet.Count);
            }
        }

        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.up + Vector2.left))
        {
            cache = SearchDiagonal(FindNextWayNode(src, Way.diagonal_up_left), Way.up, Way.left);
            if (cache != null)
            {
                Debug.Log("Left up");
                AddOpenSet(cache, Way.diagonal_up_left);
                Debug.Log(OpenSet.Count);
            }
        }


        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.down + Vector2.right))
        {
            cache = SearchDiagonal(FindNextWayNode(src, Way.diagonal_down_right), Way.down, Way.right);
            if (cache != null)
            {
                Debug.Log("Right down");
                AddOpenSet(cache, Way.diagonal_down_right);
                Debug.Log(OpenSet.Count);
            }
        }

        if (gridCreator.nodes.ContainsKey(src.GetPosition() + Vector2.down + Vector2.left))
        {
            cache = SearchDiagonal(FindNextWayNode(src, Way.diagonal_down_left), Way.down, Way.left);
            if (cache != null)
            {
                Debug.Log("Left down");
                AddOpenSet(cache, Way.diagonal_down_left);
                Debug.Log(OpenSet.Count);
            }
        }

        while (OpenSet.Count > 0)
        {
            cur = GetLowestNodeInOpenSet();
            OpenSet.Remove(cur);
            CloseSet.Add(cur);

            if (cur == end)
                break;
            if (cur.way == Way.down || cur.way == Way.left || cur.way == Way.right || cur.way == Way.up)
            {
                // 원래 방향 탐색
                if (FindNextWayNode(cur, cur.way) != null)
                {
                    cache = SearchWay(FindNextWayNode(cur, cur.way), cur.way);
                    if (cache != null)
                        AddOpenSet(cache, cur.way);
                }

                // 코너인 경우 코너방향 대각선 탐색
                if (cur.way == Way.right && IsCorner(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_right), Way.up, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_right);
                    }

                    if (FindNextWayNode(cur, Way.diagonal_down_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_right), Way.down, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_right);
                    }
                    
                }
                if (cur.way == Way.left && IsCorner(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_left), Way.up, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_left);
                    }

                    if (FindNextWayNode(cur, Way.diagonal_down_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_left), Way.down, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_left);
                    }
                    
                }
                if (cur.way == Way.up && IsCorner(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_left), Way.up, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_left);
                    }
                    
                    if (FindNextWayNode(cur, Way.diagonal_up_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_right), Way.up, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_right);
                    }
                    
                }
                if (cur.way == Way.down && IsCorner(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_down_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_left), Way.down, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_left);
                    }

                    if (FindNextWayNode(cur, Way.diagonal_down_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_right), Way.down, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_right);
                    }
                    
                }
            }
            /////////////////////////////////////////////////////
            else
            {
                //원래 방향 탐색
                if (FindNextWayNode(cur, cur.way) != null)
                {
                    cache = SearchDiagonal(FindNextWayNode(cur, cur.way), 
                        (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_up_right) ? Way.up : Way.down,
                    cur.way == Way.diagonal_down_right || cur.way == Way.diagonal_up_right ? Way.right : Way.left);
                    if (cache != null)
                        AddOpenSet(cache, cur.way);
                }

                //대각선이기에 수직 수평 위치도 탐색
                if (FindNextWayNode(cur, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_up_right) ? Way.up : Way.down) != null)
                {
                    cache = SearchWay(FindNextWayNode(cur, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_up_right) ? Way.up : Way.down),
                        (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_up_right) ? Way.up : Way.down);
                    if (cache != null)
                        AddOpenSet(cache, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_up_right) ? Way.up : Way.down);
                }

                if (FindNextWayNode(cur, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_down_left) ? Way.left : Way.right) != null)
                {
                    cache = SearchWay(FindNextWayNode(cur, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_down_left) ? Way.left : Way.right),
                        (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_down_left) ? Way.left : Way.right);
                    if (cache != null)
                        AddOpenSet(cache, (cur.way == Way.diagonal_up_left || cur.way == Way.diagonal_down_left) ? Way.left : Way.right);
                }

                // 코너인 경우
                if (cur.way == Way.diagonal_up_right && IsCornerDiagonal(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_left), Way.up, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_left);
                    }


                    if (FindNextWayNode(cur, Way.diagonal_down_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_right), Way.down, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_right);
                    }

                }
                if (cur.way == Way.diagonal_up_left && IsCornerDiagonal(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_right), Way.up, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_right);
                    }


                    if (FindNextWayNode(cur, Way.diagonal_down_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_left), Way.down, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_left);
                    }

                }
                if (cur.way == Way.diagonal_down_right && IsCornerDiagonal(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_up_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_right), Way.up, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_right);
                    }


                    if (FindNextWayNode(cur, Way.diagonal_down_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_left), Way.down, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_left);
                    }

                }
                if (cur.way == Way.diagonal_down_left && IsCornerDiagonal(cur))
                {
                    if (FindNextWayNode(cur, Way.diagonal_down_right) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_down_right), Way.down, Way.right);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_down_right);
                    }


                    if (FindNextWayNode(cur, Way.diagonal_up_left) != null)
                    {
                        cache = SearchDiagonal(FindNextWayNode(cur, Way.diagonal_up_left), Way.up, Way.left);
                        if (cache != null)
                            AddOpenSet(cache, Way.diagonal_up_left);
                    }

                }
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
    }


    Node FindNextWayNode(Node node, Way way)
    {
        Vector2 vec = new Vector2();
        switch(way)
        {
            case Way.right: vec = Vector2.right; break;
            case Way.left: vec = Vector2.left; break;
            case Way.down: vec = Vector2.down; break;
            case Way.up: vec = Vector2.up; break;
            case Way.diagonal_down_left: vec = Vector2.down + Vector2.left; break;
            case Way.diagonal_down_right: vec = Vector2.down + Vector2.right; break;
            case Way.diagonal_up_left: vec = Vector2.up + Vector2.left; break;
            case Way.diagonal_up_right: vec = Vector2.up + Vector2.right; break;
        }
        if (!gridCreator.nodes.ContainsKey(node.GetPosition() + vec))
            return null;
        return gridCreator.nodes[node.GetPosition() + vec];
    }
}
