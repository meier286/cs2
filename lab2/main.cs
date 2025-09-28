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
                    cat.SetSatate(State.Ready);
                    //прочитать ходы кота из файла
                }

                if (!mouse.state == State.Ready)
                {
                    Console.Writeline("Игрок " + mouse.name + ", подтвердите готовность к игре, написав что-либо в консоль.");
                    mouse.SetState(State.Ready);
                    //прочитать ходы мыши из файла
                }

                if (mouse.GetState() == State.Ready && cat.GetState() == State.Ready)
                {
                    Game.State = GameState.Start;
                    cat.SetState(state.Playing);
                    mouse.Setstate(state.Playing);
                    isGameNotStarted = 0;
                }
            }

            //движок

            if (cat.GetState() == State.Looser || mouse.GetState == State.Looser)
            {
                Game.SetState(GameState.End);
            }
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