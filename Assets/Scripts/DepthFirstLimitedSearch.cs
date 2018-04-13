using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstLimitedSearch : SearchAlgorithm
{

    private Stack<SearchState> openStack;
    public int maxDepth; //guarda a depth máxima no qual o algoritmo pode analizar, definido pelo utilizador

    //1 Começa a fazer a pilha com o estado inicial
    //2 Entra no ciclo e caso a pilha não esteja vazia retira o elemento à cabeça da pilha (LIFO)
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //Se nao for solução determina os sucessores e adiciona-os à cabeça da pilha
    //A diferença entre profundidade e profundidade limitada é que no 2.3.2
    //só expandimos se o node for de um nível inferior ao maximo
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar

    protected override void Begin()                             //1 Inicia a função
    {
        startNode = GridMap.instance.NodeFromWorldPoint(startPos); //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a pilha e insere o node inicial
        SearchState start = new SearchState(startNode, 0);  //nó inicial
        openStack = new Stack<SearchState>();
        openStack.Push(start); //adiciona o nó à pilha

    }

    //está sempre a repetir - sempre a ser chamado
    protected override void Step()                              //2 Repete
    {
        //se a pilha não esta vazia
        if (openStack.Count > 0)
        {
            //tira o node na cabeça
            SearchState currentState = openStack.Pop();         //2.2 Retira o elemento à cabeça da pilha (LIFO)

            //Verifica se o nível do nó é inferior
            //ao nível maximo onde o algoritmo pode procurar
            if (currentState.depth <= maxDepth) {               //2.3
                VisitNode(currentState);
                //verifica se o node é solução
                if (currentState.node == targetNode)
                {
                    //se é solucao termina
                    solution = currentState;                    //2.3.1
                    finished = true;
                    running = false;
                    foundPath = true;
                }
                else                                            //2.3.2 Insere elementos à cabeca da pilha 
                {
                    //efeito de procura em profundidade
                    //se não é solução vai buscar os sucessores do nó
                    foreach (Node suc in GetNodeSucessors(currentState.node))  //Devolve os nós sucessores pela ordem N-E-S-W
                    {
                        //cria um novo estado e um novo nó
                        //volta acima
                        SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState);  //define o nó que vai adicionar
                        openStack.Push(new_node); //adiciona o nó
                    }
                    // for energy
                    if ((ulong)openStack.Count > maxListSize)
                    {
                        maxListSize = (ulong)openStack.Count;
                    }
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