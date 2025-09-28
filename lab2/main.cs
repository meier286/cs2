enum State {
    Winner,
    Looser,
    Playing,
    NotInTheGame
}

class Player
{
    public string name;
    public int location = -1;
    public State state = state.NotInTheGame;
    public int distanceTraveled = 0;

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
        setLocation(getLocation() + steps);
        if (steps < 0)
        {
            steps = -steps;
        }
        setDistanceTraveled(getDistanceTravaled() + steps);
    }
}

enum GameState
{
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
        state = GameState.Start;
    }

    public void Run()
    {
        while (state != GameState.End)
        {
            
        }
    }
}

class Program
{
    static void main(string[] args)
    {

    }
}