using System.Collections.Generic;
using UnityEngine;

public class AAsterisco : SearchAlgorithm
{

    //Uso de priority queue pois é necessário existir uma ordem ascendente de f(n) na fila
    //e a priority queue faz essa organização depois de adicionar cada elemento
    private PriorityQueue openQueue = new PriorityQueue();

    //1 Começa a fazer a fila de prioridade com o estado inicial
    //2 Entra no ciclo e caso a fila de prioridade não esteja vazia retira o elemento à cabeça da fila
    //Analisa e verifica se é solução
    //Se for solução devolve o elemento 
    //Se não for solução determina os sucessores e adiciona-os à fila de prioridade de acordo com o seu custo heuristico h(n) + o custo de próprio nó g(n) 
    //ordenando por ordem crescente todos os f(n)=h(n)+g(n) associados a cada nó
    //A* é a mistura entre a pesquisa sôfrega e o custo uniforme, sendo adicionados os dois custos h(n) + g(n),
    //o custo huristico + o custo do nó a expandir respectivamente.
    //Se  hueristica for admissivel o h(n) nunca sobrestima o custo real dpo caminho que passa por n
    //A função recomeça com a posição do "zombie" para cada elemento a encontrar



    protected override void Begin()                              //1 Inicia a função
    {
        startNode = GridMap.instance.NodeFromWorldPoint(startPos);  //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a fila de prioridade e insere o node inicial
        SearchState start = new SearchState(startNode, 0);  //nó inicial
        openQueue = new PriorityQueue();
        openQueue.Add(start, 0); //adiciona o nó à fila

    }
    //está sempre a repetir - sempre a ser chamado
    protected override void Step()                                  //2 Repete
    {
        //se a fila não está vazia
        if (openQueue.Count > 0)
        {
            SearchState currentState = openQueue.PopFirst();        //2.2 Retira o elemento com h(n) mais baixo, está na primeira posição
            VisitNode(currentState);
            //verifica se o node é solução
            if (currentState.node == targetNode)                    //2.3
            {
                //se é solução termina
                solution = currentState;                            //2.3.1
                finished = true;
                running = false;
                foundPath = true;
            }
            else                                                    //2.3.2 Insere elementos à fila de prioridade
            {
                //se não é solução vai buscar os sucessores do nó
                foreach (Node suc in GetNodeSucessors(currentState.node))  //Devolve os nós sucessores pela ordem N-E-S-W
                {
                    //cria um novo estado e um novo nó
                    //volta acima
                    SearchState new_node = new SearchState(suc, suc.gCost + currentState.g, GetHeuristic(suc), currentState);  //define o nó que vai adicionar
                    openQueue.Add(new_node, (int)new_node.f); //adiciona o nó com o seu custo heuristico associado
                }
                // for energy
                if ((ulong)openQueue.Count > maxListSize)
                {
                    maxListSize = (ulong)openQueue.Count;
                }
            }
        }
        else   //Se a fila estiver vazia termina
        {
            finished = true;
            running = false;
            //foundPath = true;
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