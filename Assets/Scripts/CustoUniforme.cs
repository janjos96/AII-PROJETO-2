using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustoUniforme : SearchAlgorithm {

	private Queue<SearchState> openQueue;


	protected override void Begin () {
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		SearchState start = new SearchState (startNode, 0);
		openQueue = new Queue<SearchState> ();
		openQueue.Enqueue(start);
		
	}
	
	protected override void Step () {
		
		if (openQueue.Count > 0)
		{
			SearchState currentState = openQueue.Dequeue();
			VisitNode (currentState);
			if (currentState.node == targetNode) {
				solution = currentState;
				finished = true;
				running = false;
				foundPath = true;
			} else {
				List<SearchState> OrdList = new List<SearchState> (); //cria uma lista temporaria

				foreach (Node suc in GetNodeSucessors(currentState.node)) {
					SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);
					OrdList.Add (new_node); //adiciona o no à lista temporaria
				}

				OrdList.Sort (Ordena); //aplica a função Ordena à lista temporária

				foreach (SearchState s in OrdList) { //Adiciona à fila os nós ordenados da lista temporaria
					openQueue.Enqueue (s);
				}

				// for energy
				if ((ulong) openQueue.Count > maxListSize) {
					maxListSize = (ulong) openQueue.Count;
				}
			}
		}
		else
		{
			finished = true;
			running = false;
			foundPath = true;
		}

	}
	private int Ordena(SearchState n1, SearchState n2) { //pega em dois nós lado a lado
		return n1.g.CompareTo (n2.g); //compara o custo (g) de dois nós lado a lado --> PROBLEMA: NAO ORDENA TUDO, SO OS ULTIMOS 2
	}
}
