using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aprofundamento_Progressivo : SearchAlgorithm {

	private Stack<SearchState> openStack;
	public int nivel;//guarda o depth do current state
	public int maxdepth;

	//1 Começa a fazer a pilha com o estado inicial
	//2 Entra no ciclo e caso a pilha nao esteja vazia retira o elemento a cabeca da pilha
	//Analisa e verifica se e solucao
	//Se for solucao devolve o elemento 
	//Se nao for solucao determina os sucessores e adiciona os a cabeca da pilha
	//a diferenca entre profundidade e profundidade limitada e que no 2.3.2
	// so expandimos se o node for de um nivel inferior ao maximo

	protected override void Begin () {							//1 Cria a Pilha
		startNode = GridMap.instance.NodeFromWorldPoint (startPos);
		targetNode = GridMap.instance.NodeFromWorldPoint (targetPos);

		//cria a pilha e insere o node inicial
		SearchState start = new SearchState (startNode, 0);
		openStack = new Stack<SearchState> ();
		openStack.Push(start);
		maxdepth = 0;

	}
	//está sempre a repetir - sempre a ser chamado
	protected override void Step () {							//2 Repete
		//se a pilha não esta vazia
		if (openStack.Count > 0)
		{
			//tira o node
			SearchState currentState = openStack.Pop();		//2.2 Retira o elemento a cabeca da pilha
			VisitNode (currentState);
			//verifica se o node e solucao
			if (currentState.node == targetNode) {//2.3
				//se e solucao termina
				solution = currentState;						//2.3.1
				finished = true;
				running = false;
				foundPath = true;
			} else {//2.3.2 Insere elementos a cabeca da pilha 
				//efeito de procura em profundidade
				//se nao e solucao vai buscar os sucessores do no
				nivel= currentState.depth;
				if (nivel < maxdepth) {//se o depth do no for menor que o maximo expande
					foreach (Node suc in GetNodeSucessors(currentState.node)) {
						//cria um novo estado e um novo no
						//volta acima
						SearchState new_node = new SearchState (suc, suc.gCost + currentState.g, currentState);//define o no para onde vais a seguir
						openStack.Push (new_node);
					}
				} 
				// for energy
				if ((ulong) openStack.Count > maxListSize) {
					maxListSize = (ulong) openStack.Count;
				}
			}
	}
		else //se a pilha estiver vazia termina
		{	//2.1 Testa se uma pilha nao tem elementos
			finished = false;
			running = true ;
			foundPath = false;
			maxdepth++;//aumenta o maximo
			SearchState start = new SearchState (startNode, 0);
			openStack.Push(start);


		}

	}
}

/*somehow temos de ir buscar o nivel do no e comparar com o nivel maximo
 * se for menor expande e se for maior nao*/


