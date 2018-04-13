using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SASearch : SearchAlgorithm
{

    private List<SearchState> openList;
    private int random;
    private int deltaE;

    public int T = 10000;
    public int seed = 10;


    //1 Começa a fazer uma lista com o estado inicial
    //2 Entra no ciclo e caso a lista não esteja vazia retira o unico elemento da lista
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //O algoritmo aleatório tem apenas um nó guardado em cada momento por isso se o nó não for solução
    //o algoritmo apaga a lista e adiciona um, apenas um, nó aleatório da vizinhança à lista
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar

    protected override void Begin() //1 Inicia a função
    {                           
        startNode = GridMap.instance.NodeFromWorldPoint(startPos); //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a lista e insere o node inicial
        SearchState start = new SearchState(startNode, 0); //nó inicial
        openList = new List<SearchState>();
        openList.Add(start); //adiciona o nó à lista

    }

    //está sempre a repetir - sempre a ser chamado
    protected override void Step()                        //2 Repete
    {
        T = T - seed;

        SearchState currentState = openList[0];
        VisitNode(currentState);

        if(T == 0){
            //se é solução termina
            solution = currentState;                    //2.3.1
            finished = true;
            running = false;
            foundPath = true;

        }

        //openList.Clear(); //limpa a lista removendo o nó que não era solução

        List<Node> neighbours = new List<Node>(); //cria um lista que contem os nós da vizinhança
        neighbours = GetNodeSucessors(currentState.node); //devolve a lista com os nós


        random = Random.Range(0, neighbours.Count); //como a vizinhança pode ir de zero a 4 nós, o random é gerado
                                                    //de acordo com o cumprimento da lista de nós vizinhos

        SearchState new_node = new SearchState(neighbours[random], neighbours[random].gCost + currentState.g, GetHeuristic(neighbours[random]), currentState); //define o nó que vai adicionar

        if((int)new_node.h < (int)currentState.h)
        {
            openList.Clear();
            openList.Add(new_node);

            // for energy
            if ((ulong)openList.Count > maxListSize)
            {
                maxListSize = (ulong)openList.Count;
            }
        } else
        {
            deltaE = (int)new_node.h - (int)currentState.h;
            Mathf.Exp(deltaE/T);
            Debug.Log(Mathf.Exp(deltaE / T));

            // for energy
            if ((ulong) openList.Count > maxListSize)
            {
                maxListSize = (ulong) openList.Count;
            }
        }

    }

    //Heuristica a ser usada para calurar o h(n) do nó a expandir
    //Neste caso a heuristica é a distancia vertical+horizontal do nó a verificar até ao destino
    protected virtual int GetHeuristic(Node suc)
    {
        int distancia = Mathf.Abs(targetNode.gridX - suc.gridX) + Mathf.Abs(targetNode.gridY - suc.gridY);
        return distancia;
    }

}