using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{

    //Chão
    public Tilemap floors;
    public TileBase floor;

    //Parede
    public Tilemap walls;
    public TileBase wall;

    //Porta
    public Tilemap doors;
    public TileBase door;

    //Grid -- FIELD DE TESTE
    int gridSizeX = 50;
    int gridSizeY = 50;

    public static bool redo = false; // Gerar um novo mapa caso retorne "true"
    public bool nonStaticRedo = false;

    int limitRacer = 0; // Contador de vezes que a função GenerateRooms foi chamada
    int limitMax = 300; // Limitar as vezes que a função GenerateRoom é chamada

    //Obstáculo
    public GameObject obstacle;
    GameObject tempObstacle;
    List<GameObject> obstacles = new List<GameObject> { }; // Lista de todos os obstacles criados

    Vector2Int lastRoomPos;

    List<Vector2Int> AllRoomsPos = new List<Vector2Int> { }; // Lista das posições de todas as rooms criadas

    int roomSize = 16;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRooms(roomSize, new Vector2Int(0, 0), firstCall: true); // Gerar um conjunto de rooms randômicas no início do jogo
        lastRoomPos = FindLastRoom(); // A posição da room mais distante em relação à room inicial
        doors.SetTile(new Vector3Int(lastRoomPos.x,lastRoomPos.y,0),door); // Depois do GenerateRooms terminar todas as chamadas,uma porta é setada na última room criada
        GenerateObstacles(); // Gerar obstáculos espalhados pelo mapa ( Não spawnam nem na última room e nem na primeira )
    }


    // Update is called once per frame
    void Update()
    {
        if(nonStaticRedo == true)
        {
            redo = true;
            nonStaticRedo = false;
        }

        if(redo == true)
        {
            walls.ClearAllTiles();
            floors.ClearAllTiles();
            doors.ClearAllTiles();
            limitRacer = 0;
            AllRoomsPos = new List<Vector2Int> { };
            GenerateRooms(roomSize,new Vector2Int(0,0),firstCall: true);
            lastRoomPos = FindLastRoom();
            doors.SetTile(new Vector3Int(lastRoomPos.x,lastRoomPos.y, 0), door);
            DestroyObstacles();
            GenerateObstacles();
            redo = false;
        }
    }

    Vector2Int FindLastRoom() // Achar a room mais distante em relação à room inicial
    {
        Vector2Int loopPos;
        loopPos = AllRoomsPos[0];
        
        foreach(Vector2Int roomPos in AllRoomsPos)
        {
            if (Mathf.Abs(roomPos.x) + Mathf.Abs(roomPos.y) > Mathf.Abs(loopPos.x) + Mathf.Abs(loopPos.y)) loopPos = roomPos; 
        }

        return loopPos;
    }

    void GenerateObstacles()
    {
        foreach(Vector2Int roomPos in AllRoomsPos)
        {
            if(roomPos != new Vector2Int(0,0) && roomPos != lastRoomPos)
            {
              int repeat = new System.Random().Next(3, 6); // Determinar quantos obstáculos vão nascer por room. Range 3-5
              int repeated = 0;
              List<Vector2> tempObstaclePos = new List<Vector2> { }; // Lista temporária das posições dos obstáculos. Reseta a cada loop do "foreach"

                while (repeated < repeat)
                {
                    int randomPosX = new System.Random().Next(roomPos.x - (roomSize / 2) + 2, roomPos.x + (roomSize / 2) - 2 + 1);  // +2 e -2 são para delimitar a área das rooms que o obstáculo pode spawnar
                    int randomPosY = new System.Random().Next(roomPos.y - (roomSize / 2) + 2, roomPos.y + (roomSize / 2) - 2 + 1);
                    bool canContinue = true;
                    
                    foreach(Vector2 obstaclePos in tempObstaclePos) // Checar se o obstáculo não está sendo spawnado em uma mesma posição de outro obstáculo
                    {
                        if (Mathf.Abs(randomPosX) + Mathf.Abs(randomPosY) == Mathf.Abs(obstaclePos.x) + Mathf.Abs(obstaclePos.y)) canContinue = false;
                    }

                    if (canContinue == true)
                    {
                        tempObstaclePos.Add(new Vector2(randomPosX, randomPosY));
                        Debug.Log($"rp:{repeated},rx:{randomPosX},ry:{randomPosY}");

                        tempObstacle = Instantiate(obstacle, new Vector3(randomPosX, randomPosY, 0), Quaternion.identity);
                        obstacles.Add(tempObstacle);

                        repeated++;
                    }
                }
            }
        }
    }

    void DestroyObstacles()
    {
        foreach(GameObject _obstacle in obstacles)
        {
            Destroy(_obstacle);
        }
    }

    void GenerateGrid() // Gerar um grid
    {

        // Gerar grid no runtime por meio da variação linear dos valores X e Y

        for (int x = -(gridSizeX/2) ; x <= gridSizeX/2; x++)
        {
            for (int y = -(gridSizeY/2) ; y <= gridSizeY/2; y++)
            {
                if (y == -(gridSizeY / 2) || y == gridSizeY / 2 || x == -(gridSizeX/2) || x == gridSizeX/2) walls.SetTile(new Vector3Int(x,y,0),wall);
                else floors.SetTile(new Vector3Int(x, y, 0), floor);
            }
        }

        // Gerar tiles randomicamente pelo grid
        // -1 ou + 1 indica que as bordas do grid não estão inclusas no loop
   
        for (int x = -(gridSizeX / 2) + 1; x <= (gridSizeX / 2) - 1; x++)
        {
            for (int y = -(gridSizeY / 2) + 1; y <= (gridSizeY / 2) - 1; y++)
            {

                int size = new System.Random().Next(4, 7); // Range 10-20
                int placeSquare = new System.Random().Next(1, 4);
                Debug.Log(size);
                Debug.Log(placeSquare);

                if (x + size <= gridSizeX / 2 && y + size <= gridSizeY / 2)
                {
                     CreateSquare(x, y, size, 1);
                     y += size - 1;
                }

             }
        }
 

    }

    void CreateSquare(int x, int y, int size, int door)
    {
        bool createSquare = true;

        foreach (Vector2Int rPos in AllRoomsPos)
        {
            if (rPos == new Vector2Int(x, y)) createSquare = false;
        }

        if (createSquare == true)
        {
            Debug.Log(door);
            for (int _x = x - (size / 2); _x <= x + (size / 2); _x++)
            {
                for (int _y = y - (size / 2); _y <= y + (size / 2); _y++)
                {
                    switch (door)
                    {
                        
                        // Porta no extremo norte
                        case 1:
                            if (_x == x - (size / 2) || _x == x + (size / 2) || _y == y - (size / 2) || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x,_y,0),null);
                            }
                            break;


                        // Porta no extremo sul
                        case 2:
                            if (_x == x - (size / 2) || _x == x + (size / 2) || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo leste
                        case 3:
                            if (_x == x - (size / 2) || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo oeste
                        case 4:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) || _y == y - (size / 2) || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta em todos os extremos
                        case 5:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo norte e leste
                        case 6:
                            if (_x == x - (size / 2) || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo norte e sul
                        case 7:
                            if (_x == x - (size / 2) || _x == x + (size / 2) || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo norte e oeste
                        case 8:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) || _y == y - (size / 2) || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;

                        // Porta no extremo leste e oeste
                        case 9:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo leste e sul
                        case 10:
                            if (_x == x - (size / 2) || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo oeste e sul
                        case 11:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo norte,leste e oeste
                        case 12:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo sul,leste e oeste
                        case 13:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2)) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo leste,norte e sul
                        case 14:
                            if (_x == x - (size / 2) || _x == x + (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;


                        // Porta no extremo oeste,norte e sul
                        case 15:
                            if (_x == x - (size / 2) && _y != y && _y != y - 1 && _y != y + 1 || _x == x + (size / 2) || _y == y - (size / 2) && _x != x && _x != x - 1 && _x != x + 1 || _y == y + (size / 2) && _x != x && _x != x - 1 && _x != x + 1) walls.SetTile(new Vector3Int(_x, _y, 0), wall);
                            else
                            {
                                floors.SetTile(new Vector3Int(_x, _y, 0), floor);
                                walls.SetTile(new Vector3Int(_x, _y, 0), null);
                            }
                            break;
                    }
                }
            }
            AllRoomsPos.Add(new Vector2Int(x, y));
        }
    }


    // Gerar vários quadrados(Rooms) em um padrão específico(ramificações)
    // 1 - N ; 2 - S ; 3 - L ; 4 - O ; 5 - T ; 6 - NL ; 7 - NS ; 
    // 8 - NO ; 9 - LO ; 10 - SL ; 11 - SO ; 12 - NLO ; 13 - SLO ; 14 - NSL ; 15 - NSO

    int[] northDoors = { 1, 6, 8, 12, 14, 15, 7 };
    int[] southDoors = { 2, 10, 11, 13, 14, 15, 7 };
    int[] eastDoors = { 3, 6, 10, 12, 13, 14, 9 };
    int[] westDoors = { 4, 8, 11, 12, 13, 15, 9 };

    int randomSDoor, randomNDoor, randomWDoor, randomEDoor = 0;

    void GenerateRooms(int squareSize, Vector2Int lastSquarePos, int lastSquare = 0, bool firstCall = false)
    {
        limitRacer++;
       
            if (firstCall == true)
            {
                int chooseSquare = new System.Random().Next(1, 16); // Escolhe um quadrado com portas "X" randomicamente. Range (1-15)

                CreateSquare(lastSquarePos.x, lastSquarePos.y, squareSize, chooseSquare); // Criar a room 
                GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y), chooseSquare, false); // Chamar a função de novo para iniciar o loop de criação das rooms
            }

            // Quadrados encerradores de ramificações são quadrados que evitam que o jogador saia do mapa.Eles só possuem uma porta e esta está ligada
            // A uma outra porta,fechando,assim,a ramificação 

            // Se o limitador estiver dentro do limite,a posição das portas nos quadrados é gerada randomicamente,podendo ser quadrados encerradores de ramificações
            // Ou quadrados que geram mais quadrados
            if (limitRacer < limitMax)
            {
                randomSDoor = southDoors[new System.Random().Next(0, southDoors.Length)];
                randomNDoor = northDoors[new System.Random().Next(0, northDoors.Length)];
                randomWDoor = westDoors[new System.Random().Next(0, westDoors.Length)];
                randomEDoor = eastDoors[new System.Random().Next(0, eastDoors.Length)];
            }

            // Caso o limitador alcance o seu limite,todas as chamadas do método "GenerateRooms" são forçadas a gerar quadrados encerradores de ramificações
            else if(limitRacer == limitMax)
            {
                randomSDoor = 2;
                randomNDoor = 1;
                randomWDoor = 4;
                randomEDoor = 3;
            }

            switch (lastSquare)
            {
                //Sul
                case 1:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    break;

                //Norte
                case 2:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    break;

                //Oeste
                case 3:
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Leste
                case 4:
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    break;

                //Todos
                case 5:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    break;

                //Sul-Oeste
                case 6:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Sul
                case 7:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    break;

                //Sul-Leste
                case 8:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    break;

                //Leste-Oeste
                case 9:
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Oeste
                case 10:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Leste
                case 11:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    break;

                //Sul-Leste-Oeste
                case 12:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Leste-Oeste
                case 13:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Sul-Oeste
                case 14:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x + squareSize, lastSquarePos.y, squareSize, randomWDoor);
                    if (randomWDoor != 4) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x + squareSize, lastSquarePos.y), randomWDoor);
                    break;

                //Norte-Sul-Leste
                case 15:
                    CreateSquare(lastSquarePos.x, lastSquarePos.y - squareSize, squareSize, randomNDoor);
                    if (randomNDoor != 1) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y - squareSize), randomNDoor);
                    CreateSquare(lastSquarePos.x, lastSquarePos.y + squareSize, squareSize, randomSDoor);
                    if (randomSDoor != 2) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x, lastSquarePos.y + squareSize), randomSDoor);
                    CreateSquare(lastSquarePos.x - squareSize, lastSquarePos.y, squareSize, randomEDoor);
                    if (randomEDoor != 3) GenerateRooms(squareSize, new Vector2Int(lastSquarePos.x - squareSize, lastSquarePos.y), randomEDoor);
                    break;



            }

        }
    
}
