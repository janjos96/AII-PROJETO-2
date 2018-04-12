using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PesquisaSofregaSearch : SearchAlgorithm
{

    private PriorityQueue openQueue = new PriorityQueue();


    protected override void Begin()
    {
        startNode = GridMap.instance.NodeFromWorldPoint(startPos);
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos);

        SearchState start = new SearchState(startNode, 0);
        openQueue = new PriorityQueue();
        openQueue.Add(start, 0);

    }

    protected override void Step()
    {

        if (openQueue.Count > 0)
        {
            SearchState currentState = openQueue.PopFirst();
            VisitNode(currentState);
            if (currentState.node == targetNode)
            {
                solution = currentState;
                finished = true;
                running = false;
                foundPath = true;
            }
            else
            {
                foreach (Node suc in GetNodeSucessors(currentState.node))
                {
                    SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, GetHeuristic(suc), currentState);
                    openQueue.Add(new_node, (int)new_node.h);
                }
                // for energy
                if ((ulong)openQueue.Count > maxListSize)
                {
                    maxListSize = (ulong)openQueue.Count;
                }
            }
        }
        else
        {
            finished = true;
            running = false;
        }

    }

    protected virtual int GetHeuristic(Node suc) 
    {
        int distancia = Mathf.Abs(targetNode.gridX - suc.gridX) + Mathf.Abs(targetNode.gridY - suc.gridY);
        return distancia; 
    }
}
