using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : SearchAlgorithm {

	private Queue<SearchState> openQueue;

    //1 Começa a fazer a fila com o estado inicial
    //2 Entra no ciclo e caso a fila não esteja vazia retira o elemento no fim da fila (FIFO)
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //Se nao for solução determina os sucessores e adiciona-os ao fim da fila
    //A diferença entre largura e profundidade limitada é que no 2.3.2
    //adicionamos os nós sucessores no fim da fila e por ser FIFO o primeiro a entrar vai ser o primeiro a sair
    //e por isso percorre cada nível de cada vez
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar


    protected override void Begin () {                          //1 Inicia a função
        startNode = GridMap.instance.NodeFromWorldPoint (startPos);  //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint (targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a fila e insere o node inicial
        SearchState start = new SearchState (startNode, 0); //nó inicial
		openQueue = new Queue<SearchState> ();
		openQueue.Enqueue(start);
		
	}
	
    //está sempre a repetir - sempre a ser chamado
    protected override void Step () {                       //2 Repete
        //se a fila não esta vazia
		if (openQueue.Count > 0)
		{
            //tira o node no fim
            SearchState currentState = openQueue.Dequeue();         //2.2 Retira o elemento no fim da fila (FIFO)
			VisitNode (currentState);
            //verifica se o node é solução
            if (currentState.node == targetNode) { //2.3
                //se é solucao termina
                solution = currentState;                    //2.3.1
				finished = true;
				running = false;
				foundPath = true;
            } else {                                        //2.3.2 Insere elementos ao fim da fila 
                //efeito de procura em largura
                //se não é solução vai buscar os sucessores do nó
                foreach (Node suc in GetNodeSucessors(currentState.node)) { //Devolve os nós sucessores pela ordem N-E-S-W
                    //cria um novo estado e um novo nó
                    //volta acima
                    SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState); //define o nó que vai adicionar
                    openQueue.Enqueue (new_node); //adiciona o nó
				}
				// for energy
				if ((ulong) openQueue.Count > maxListSize) {
					maxListSize = (ulong) openQueue.Count;
				}
			}
		}
        else //Se a pilha estiver vazia termina
        {   //2.1 Testa se a pilha não tem elementos
			finished = true;
			running = false;
			//foundPath = true;
		}

	}
}
