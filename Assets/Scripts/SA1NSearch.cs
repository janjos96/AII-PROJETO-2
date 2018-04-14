using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA1NSearch : SearchAlgorithm
{

    private List<SearchState> openList;
    private int random;
    private float deltaE;
    public float T = 1000;
    public float seed = 1;
    public float rmax = 2;

    SearchState optNode;
    SearchState currentNode;


    //O algoritmo de Recristalização Simulada vai testando as soluções de maneira a encontrar a melhor possivel
    //Assim, cria uma lista com os nós e o seus sucessores e escolhe aleatóriamente um nó para analizar,
    //vai comparar o valor heuristico desse nó com o valor heuristo do nó atual e se for melhor vai expandir para esse nó,
    //caso contrário esse nó ainda pode ser escolhido caso o valor da função e^deltaE/T seja maior que a variavel "r" -> função probabilidade
    //Existe uma temperatura que vai descendo ao longo de cada iteração constantemente pela variável seed 
    //e quanto mais baixa for esta temperatura, maior será a chance de ser esculhido pelos valores da função probabilidade
    //
    //Como nós lemos no livro que o algoritmo tem uma complexidade espacial constante pois só mantem um nó em memória, tentamos
    //implementar com uma lista onde só existe 1 nó mas os resultados foram diferentes, então separamos em dois scripts SASearch e SA1NSearch (1 nó)

    protected override void Begin() //1 Inicia a função
    {
        startNode = GridMap.instance.NodeFromWorldPoint(startPos); //Verifica o nó inicial e a respectiva posição
        targetNode = GridMap.instance.NodeFromWorldPoint(targetPos); //Verifica o nó destino e a respectiva posição

        //Cria a lista e insere o node inicial
        SearchState start = new SearchState(startNode, 0, 0); //nó inicial
        openList = new List<SearchState>();
        openList.Add(start); //adiciona o nó à lista
        currentNode = start;
        optNode = start; //incialmente, pela falta de ramificação o inicial vai ser o melhor

    }

    //está sempre a repetir - sempre a ser chamado
    protected override void Step()                          //2 Repete
    {

        //Decrementa a variável seed à temperatura de modo a que temperatura desça
        T = polEscalona(T);                                 //2.1 PolEscalona

        //Quando a temperatura chega a zera significa que ele encontrou a melhor solução
        //(maximo global) e portanto devolve a solução
        if (T <= 0)
        {                                                   //2.2
            if (optNode.node == targetNode)
            {
                currentNode = optNode;                      //2.2.1 Devolve o nó
                solution = optNode;
                finished = true;
                running = false;
                foundPath = true;
                T = 1000;
            }
        }


        openList.Clear(); //limpa a lista removendo o nó que não era solução

        List<Node> neighbours = new List<Node>(); //cria um lista que contem os nós da vizinhança
        neighbours = GetNodeSucessors(currentNode.node); //devolve a lista com os nós

        random = Random.Range(0, neighbours.Count); //como a vizinhança pode ir de zero a 4 nós, o random é gerado
                                                    //de acordo com o cumprimento da lista de nós vizinhos

        // for energy
        if ((ulong)openList.Count > maxListSize)
        {
            maxListSize = (ulong)openList.Count;
        }
                                                            //2.3 Escolhe o próximo nó aleatóriamente
        SearchState nextNode = new SearchState(neighbours[random], neighbours[random].gCost +  currentNode.g, GetHeuristic(neighbours[random]),  currentNode); //define o nó que vai adicionar

        // for energy
        if ((ulong)openList.Count > maxListSize)
        {
            maxListSize = (ulong)openList.Count;
        }
                                                            //2.4 Compara Heuristica do nó corrente com o nó seguinte
        if (GetHeuristic(nextNode.node) <= GetHeuristic(currentNode.node))
        {
            currentNode = nextNode;                         //2.4.1 Caso seja melhor dá update para esse nó
            updateBestNode(currentNode);
        }
        else                                                //2.4 Compara Heuristica do nó corrente com o nó seguinte
        {
            //2.4.2 Caso não seja melhor ainda pode ser escolhido pelo valo
            //retribuido por e^deltaE/T, caso a variavel random tenha um valor menor
            float r = Random.Range(0, rmax);
            deltaE = GetHeuristic(currentNode.node) - GetHeuristic(nextNode.node);
            float prob = Mathf.Exp(deltaE / T);

            if (r < prob)
            {
                currentNode = nextNode;
                updateBestNode(currentNode);
            }
        }

        VisitNode(currentNode); //visita o nó

    }

    //Heuristica a ser usada para calurar o h(n) do nó a expandir
    //Neste caso a heuristica é a distancia vertical+horizontal do nó a verificar até ao destino
    protected virtual int GetHeuristic(Node suc)
    {
        int distancia = Mathf.Abs(targetNode.gridX - suc.gridX) + Mathf.Abs(targetNode.gridY - suc.gridY);
        return distancia;
    }

    //função que determina os valores sucessivos da temperatura decrementando a variável seed
    protected virtual float polEscalona(float t)
    {
        t -= seed;
        return t;
    }

    //função para alterar a variável melhor nó caso surja uma solução em que o valor heuristico é melhor
    //munda o Optimum node para o valor desse mesmo nó encontrado
    protected void updateBestNode(SearchState suc)
    {
        if (GetHeuristic(suc.node) < GetHeuristic(optNode.node))
        {
            optNode = suc;
        }
    }

}