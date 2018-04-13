using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSearch : SearchAlgorithm {

    private List<SearchState> openList;
    private int random;

    //1 Começa a fazer uma lista com o estado inicial
    //2 Entra no ciclo e caso a lista não esteja vazia retira o unico elemento da lista
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //O algoritmo aleatório tem apenas um nó guardado em cada momento por isso se o nó não for solução
    //o algoritmo apaga a lista e adiciona um, apenas um, nó aleatório da vizinhança à lista
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar

    protected override void Begin() {                           //1 Inicia a função
        startNode = GridMap.instance.NodeFromWorldPoint(startPos); //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a lista e insere o node inicial
        SearchState start = new SearchState(startNode, 0); //nó inicial
        openList = new List<SearchState>();
        openList.Add(start); //adiciona o nó à lista

    }

    //está sempre a repetir - sempre a ser chamado
    protected override void Step() {                        //2 Repete
        //se a lista não está vazia
        if (openList.Count > 0)
        {
            //tira o node no fim da lista menos o valor aleatório 0-3
            SearchState currentState = openList[0];       //2.2 Retira o elemento da lista
            VisitNode(currentState);
            //verifica se o node é solução
            if (currentState.node == targetNode)            //2.3
            {
                //se é solução termina
                solution = currentState;                    //2.3.1
                finished = true;
                running = false;
                foundPath = true;
            }
            else                                            //2.3.2 Insere elementos ao fim da lista
            {
                //se não é solução vai buscar os sucessores do nó

                openList.Clear(); //limpa a lista removendo o nó que não era solução

                List<Node> neighbours = new List<Node>(); //cria um lista que contem os nós da vizinhança
                neighbours = GetNodeSucessors(currentState.node); //devolve a lista com os nós

                random = Random.Range(0, neighbours.Count); //como a vizinhança pode ir de zero a 4 nós, o random é gerado
                                                            //de acordo com o cumprimento da lista de nós vizinhos

                SearchState new_node = new SearchState(neighbours[random], neighbours[random].gCost + currentState.g, currentState); //define o nó que vai adicionar
                openList.Add(new_node); //adiciona o  nó
                
                // for energy
                if ((ulong)openList.Count > maxListSize)
                {
                    maxListSize = (ulong)openList.Count;
                }
            }
        }
        else  //Se a pilha estiver vazia termina

        {
            finished = true;
            running = false;
            //foundPath = true;
        }

    }

}
