enum State {
    Winner,
    Looser,
    Playing,
    Ready,
    NotInTheGame
}

class Player
{
    private string name;
    private int location = -1;
    private State state = State.NotInTheGame;
    private int distanceTraveled = 0;

    public void setName(string name)
    {
        this.name = name;
    }

    public string getName()
    {
        return name;
    }

    public void setLocation(int location)
    {
        this.location = location;
    }

    public int getLocation()
    {
        return location;
    }

    public void setState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }

    public void setDistanceTraveled(int distanceTraveled)
    {
        this.distanceTraveled = distanceTraveled;
    }

    public int getDistanceTraveled()
    {
        return distanceTraveled;
    }

    public Player(string name)
    {
        setName(name);
        Console.WriteLine("Игрок был создан.");
    }

    public void Move(int steps)
    {
        int newLocation = getLocation() + steps;
    
        if (newLocation > Game.size)
        {
            newLocation = newLocation - Game.size - 1;
        }
        else if (newLocation < 0)
        {
            newLocation = Game.size + newLocation + 1;
        }
    
        setLocation(newLocation);
    
        if (steps < 0)
        {
            steps = -steps;
        }
        setDistanceTraveled(getDistanceTraveled() + steps);
    }
}

enum GameState
{
    WaitingForPlayers,
    Start,
    End
}

class Game
{
    public static int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    public Game(int size)
    {
        Game.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.WaitingForPlayers;
    }

    public void Run()
    {
        while (state != GameState.End)
        {
            bool isGameNotStarted = true;
            while (isGameNotStarted)
            {
                if (cat.GetState() != State.Ready)
                {
                    Console.WriteLine("Игрок " + cat.getName() + ", подтвердите готовность к игре, написав что-либо в консоль.");
                    Console.ReadLine();
                    cat.setState(State.Ready);
                    //прочитать ходы кота из файла в массив
                }

                if (mouse.GetState() != State.Ready)
                {
                    Console.WriteLine("Игрок " + mouse.getName() + ", подтвердите готовность к игре, написав что-либо в консоль.");
                    Console.ReadLine();
                    mouse.setState(State.Ready);
                    //прочитать ходы мыши из файла в массив
                }

                if (mouse.GetState() == State.Ready && cat.GetState() == State.Ready)
                {
                    state = GameState.Start;
                    cat.setState(State.Playing);
                    mouse.setState(State.Playing);
                    isGameNotStarted = false;
                }
            }

            int catTurns;
            int mouseTurns;
            Console.WriteLine("Введите количество ходов для игрока кот: ");
            catTurns = Convert.ToInt32(Console.ReadLine());
            int[] catTurnsStorage = new int[catTurns];
            for (int i = 0; i < catTurns; i++)
            {
                Console.WriteLine("Введите шаг игрока кот для хода " + (i + 1) + ":");
                catTurnsStorage[i] = Convert.ToInt32(Console.ReadLine());
            }
            
            Console.WriteLine("Введите количество ходов для игрока мышь: ");
            mouseTurns = Convert.ToInt32(Console.ReadLine());
            int[] mouseTurnsStorage = new int[mouseTurns];
            for (int i = 0; i < mouseTurns; i++)
            {
                Console.WriteLine("Введите шаг игрока мышь для хода " + (i + 1));
                mouseTurnsStorage[i] = Convert.ToInt32(Console.ReadLine());
            }

            int globalSteps = catTurns;
            if (globalSteps < mouseTurns)
            {
                globalSteps = mouseTurns;
            }

            for (int i = 0; i < globalSteps; i++)
            {
                if (i < mouseTurns)
                {
                    mouse.Move(mouseTurnsStorage[i]);
                }

                if (i < catTurns)
                {
                    cat.Move(catTurnsStorage[i]);
                }

                int currentDistance;
                if (mouse.getLocation() > cat.getLocation())
                {
                    currentDistance = mouse.getLocation() - cat.getLocation();
                }
                else if (mouse.getLocation() < cat.getLocation())
                {
                    currentDistance = cat.getLocation() - mouse.getLocation();
                }
                else {
                    mouse.setState(State.Looser);
                    cat.setState(State.Winner);
                    currentDistance = 0;
                }
                
                if (mouse.GetState() == State.Looser)
                {
                    Console.WriteLine("Игра окончена. Кот поймал мышь!");
                    Console.WriteLine("Кот прошел расстояние в " + cat.getDistanceTraveled() + " клеток!");
                    Console.WriteLine("Мышь прошла расстояние в " + mouse.getDistanceTraveled() + " клеток!");
                    state = GameState.End;
                    break;
                }

                Console.WriteLine("Мышь: клетка" + mouse.getLocation());
                Console.WriteLine("Кот: клетка" + cat.getLocation());

                Console.WriteLine("Расстояние между котом и мышью на ходу " + (i + 1) + " равно " + currentDistance);
            }

            if (state != GameState.End)
            {
                Console.WriteLine("Игра окончена. Кот не смог поймать мышь!");
                Console.WriteLine("Кот прошел расстояние в " + cat.getDistanceTraveled() + " клеток!");
                Console.WriteLine("Мышь прошла расстояние в " + mouse.getDistanceTraveled() + " клеток!");
                cat.setState(State.Looser);
                mouse.setState(State.Winner);
                state = GameState.End;
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game(100);
        game.Run();
    }
}