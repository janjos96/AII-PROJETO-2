using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecristalizaçãoSimulada : SearchAlgorithm
{

    private List<SearchState> openList;
    private int random;

    //1 Começa a fazer uma lista com o estado inicial
    //2 Entra no ciclo e caso a lista não esteja vazia retira o elemento no fim da lista
    //menos um fator aleatório de maneira a ir buscar entre os 4 últimos valores inseridos
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //Como é possivel numa lista ir buscar o último elemento, é gerado um valor aleatório de 0 a 3
    //para subtrair ao fim da lista e ir buscar aletóriamente um de 4 nós nesse mesmo fim
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar

    protected override void Begin()
    {                           //1 Inicia a função
        startNode = GridMap.instance.NodeFromWorldPoint(startPos); //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a lista e insere o node inicial
        SearchState start = new SearchState(startNode, 0); //nó inicial
        openList = new List<SearchState>();
        openList.Add(start); //adiciona o nó à lista

    }

    //está sempre a repetir - sempre a ser chamado
    protected override void Step()
    {                        //2 Repete
        //se a lista não está vazia
        if (openList.Count > 0)
        {
            //tira o node no fim da lista menos o valor aleatório 0-3
            SearchState currentState = openList[openList.Count - 1 - random];       //2.2 Retira o elemento da lista
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
                foreach (Node suc in GetNodeSucessors(currentState.node))  //Devolve os nós sucessores pela ordem N-E-S-W
                {
                    //cria um novo estado e um novo nó
                    //volta acima
                    SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, currentState); //define o nó que vai adicionar 
                    openList.Add(new_node); //adiciona o  nó
                }
                // for energy
                if ((ulong)openList.Count > maxListSize)
                {
                    maxListSize = (ulong)openList.Count;
                }
            }
            random = Random.Range(0, 3); //valor aleatório gerado no fim de cada iteração, de modo a escolher o seguintw
        }
        else  //Se a pilha estiver vazia termina

        {
            finished = true;
            running = false;
            //foundPath = true;
        }

    }

}
