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
    private State state = state.NotInTheGame;
    private int distanceTraveled = 0;

    void setName(string name)
    {
        this.name = name;
    }

    string getName()
    {
        return name;
    }

    void setLocation(int location)
    {
        this.location = location;
    }

    int getLocation()
    {
        return location;
    }

    void setState(State state)
    {
        this.state = state;
    }

    State GetState()
    {
        return state;
    }

    void setDistanceTraveled(int distanceTraveled)
    {
        this.distanceTraveled = distanceTraveled;
    }

    int getDistanceTravaled()
    {
        return distanceTraveled;
    }

    public Player(string name)
    {
        setName(name);
        Console.Writeline("Игрок был создан.");
    }

    public void Move(int steps)
    {
        if (getLocation() + steps > Game.size)
        {
            setLocation(-(Game.size - (getLocation + steps)));
        }
        else
        {
            setLocation(getLocation() + steps);   
        }
        if (steps < 0)
        {
            steps = -steps;
        }
        setDistanceTraveled(getDistanceTravaled() + steps);
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
    public int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    public Game(int size)
    {
        this.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.WaitingForPlayers;
    }

    public void Run()
    {
        while (state != GameState.End)
        {
            bool isGameNotStarted = 1;
            while (isGameNotStarted)
            {
                if (!cat.GetState() == State.Ready)
                {
                    Console.Writeline("Игрок " + cat.name + ", подтвердите готовность к игре, написав что-либо в консоль.");
                    Console.Readline();
                    cat.SetSatate(State.Ready);
                    //прочитать ходы кота из файла в массив
                }

                if (!mouse.state == State.Ready)
                {
                    Console.Writeline("Игрок " + mouse.name + ", подтвердите готовность к игре, написав что-либо в консоль.");
                    Console.Readline();
                    mouse.SetState(State.Ready);
                    //прочитать ходы мыши из файла в массив
                }

                if (mouse.GetState() == State.Ready && cat.GetState() == State.Ready)
                {
                    Game.State = GameState.Start;
                    cat.SetState(state.Playing);
                    mouse.Setstate(state.Playing);
                    isGameNotStarted = 0;
                }
            }

            int catTurns;
            int mouseTurns;
            Console.Writeline("Введите количество ходов для игрока кот: ");
            catTurns = Console.Readline();
            int[] catTurnsStorage = new int[catTurns];
            for (int i = 0; i < catTurns; i++)
            {
                Console.Writeline("Введите шаг игрока кот для хода + " + (i + 1));
                catTurnsStorage[i] = Console.Readline();
            }
            
            Console.Writeline("Введите количество ходов для игрока мышь: ");
            mouseTurns = Console.Readline();
            int[] mouseTurnsStorage = new int[mouseTurns];
            for (int i = 0; i < catTurns; i++)
            {
                Console.Writeline("Введите шаг игрока мышь для хода + " + (i + 1));
                mouseTurnsStorage[i] = Console.Readline();
            }

            int globalSteps = catTurns;
            if (globalSteps < mouseTurns)
            {
                globalSteps = mouseTurns;
            }

            for (int i = 0; i < globalSteps; i++)
            {
                if (!globalSteps > mouseTurns)
                {
                    mouse.Move(mouseTurnsStorage[i]);
                }

                if (!globalSteps > catTurns)
                {
                    cat.Move(catTurnsStorage[i]);
                }

                int currentDistance;
                if (mouse.getLocation() > cat.getLOcation())
                {
                    currentDistance = mouse.getLocation() - cat.getLocation();
                }

                else if (mouse.getLocation() < cat.getLOcation())
                {
                    currentDistance = cat.getLocation() - mouse.getLocation();
                }

                else {
                    mouse.setState(State.Looser);
                    cat.setState(State.Winner);
                }
                

                if (mouse.GetState == State.Looser)
                {
                    Console.Writeline("Игра окончена. Кот поймал мышь!");
                    Game.SetState(GameState.End);
                }

                Console.Writeline("Расстояние между котом и мышью на ходу +" + (i + 1) + " равно " + currentDistance);
            }

            Console.Writeline("Игра окончена. Кот не смог поймать мышь!");
            cat.setState(State.Looser);
            Game.SetState(GameState.End);
            
        }
    }
}

class Program
{
    static void main(string[] args)
    {
        Game.Run();
    }
}