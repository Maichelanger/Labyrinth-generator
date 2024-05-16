using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; // 0 = top, 1 = right, 2 = bottom, 3 = left
    }

    [Tooltip("Size of the board")]
    [SerializeField] private Vector2Int size;
    [Tooltip("Initial position of the maze")]
    [SerializeField] private int initialPos = 0;
    [Tooltip("The prefabs that will be used to generate the maze. They should have a RoomBehaviour to work.")]
    [SerializeField] private GameObject[] roomPrefabs;
    [Tooltip("Size of the room's prefab. To find out, check the floor tiles size.")]
    [SerializeField] private Vector2 roomSize;
    [Tooltip("Steps in the generation. DOES NOT represent total rooms.")]
    [SerializeField] private int steps;

    private List<Cell> board = new List<Cell>();

    private void Start()
    {
        MazeGenerator();
    }

    private void MazeGenerator()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = initialPos;
        Stack<int> stack = new Stack<int>();
        int num = 0;

        while (num < steps)
        {
            num++;
            board[num].visited = true;

            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (stack.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = stack.Pop();
                }
            }
            else
            {
                stack.Push(currentCell);
                int randomNeighbour = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];

                if (randomNeighbour < currentCell)
                {
                    if (randomNeighbour + 1 == currentCell) // Left
                    {
                        board[currentCell].status[3] = true;
                        board[randomNeighbour].status[1] = true;
                    }
                    else // Down
                    {
                        board[currentCell].status[2] = true;
                        board[randomNeighbour].status[0] = true;
                    }
                }
                else
                {
                    if (randomNeighbour - 1 == currentCell) // Right
                    {
                        board[currentCell].status[1] = true;
                        board[randomNeighbour].status[3] = true;
                    }
                    else // Top
                    {
                        board[currentCell].status[0] = true;
                        board[randomNeighbour].status[2] = true;
                    }
                }
                currentCell = randomNeighbour;
            }
        }

        InstantiateDungeonRooms();
    }

    private void InstantiateDungeonRooms()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int randomPrefabIndex = UnityEngine.Random.Range(0, roomPrefabs.Length); // Select a random room prefab
                GameObject room = Instantiate(roomPrefabs[randomPrefabIndex], new Vector3(i * roomSize.x, 0, j * roomSize.y), Quaternion.identity, transform);
                room.GetComponent<RoomBehaviour>().SetBlockers(board[i + j * size.x].status);
            }
        }
        //StartCoroutine(InstantiatePlayer());
    }

    private List<int> CheckNeighbours(int currentCell)
    {
        List<int> neighbours = new List<int>();

        if (currentCell - size.x >= 0 && !board[currentCell - size.x].visited) // top
        {
            neighbours.Add(currentCell - size.x);
        }

        if ((currentCell + 1) % size.x != 0 && !board[currentCell + 1].visited) // right
        {
            neighbours.Add(currentCell + 1);
        }

        if (currentCell + size.x < board.Count && !board[currentCell + size.x].visited) // bottom
        {
            neighbours.Add(currentCell + size.x);
        }

        if (currentCell % size.x != 0 && !board[currentCell - 1].visited) // left
        {
            neighbours.Add(currentCell - 1);
        }

        return neighbours;
    }

    /**
     * 
     * Unfinished

    IEnumerator InstantiatePlayer()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject firstRoom = transform.GetChild(0).gameObject;
        Transform roomCenter = firstRoom.transform.Find("Room Center");
        Instantiate(playerPrefab, roomCenter.position, Quaternion.identity);
    }
    **/
}
