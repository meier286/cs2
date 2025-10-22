
enum State
{
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
    private List<int> moves = new List<int>();

    public Player(string name)
    {
        setName(name);
    }

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

    public void AddMove(int move)
    {
        moves.Add(move);
    }

    public List<int> GetMoves()
    {
        return moves;
    }

    public void Move(int steps)
    {
        if (state != State.Playing) return;

        int newLocation = (getLocation() + steps) % Game.size;
        
        if (newLocation < 0)
        {
            newLocation += Game.size;
        }
        
        setLocation(newLocation);
        
        if (steps < 0)
        {
            steps = -steps;
        }
        setDistanceTraveled(getDistanceTraveled() + steps);
    }
}

class Game
{
    public static int size;
    public Player cat;
    public Player mouse;
    private List<string> outputLines = new List<string>();
    private bool gameEnded = false;

    public Game(int size)
    {
        Game.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        
        outputLines.Add("Cat and Mouse");
        outputLines.Add("");
        outputLines.Add("Cat\tMouse\tDistance");
        outputLines.Add("---------------------");
    }

    public void ProcessInputFile(string filename)
    {
        try
        {
            string[] lines = File.ReadAllLines(filename);
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                string[] parts = line.Split(' ');
                string command = parts[0];
                
                switch (command)
                {
                    case "M":
                        if (parts.Length > 1 && int.TryParse(parts[1], out int mouseValue))
                        {
                            if (mouse.GetState() == State.NotInTheGame)
                            {
                                mouse.setLocation(mouseValue);
                                mouse.setState(State.Playing);
                            }
                            else
                            {
                                mouse.AddMove(mouseValue);
                            }
                        }
                        break;
                        
                    case "C":
                        if (parts.Length > 1 && int.TryParse(parts[1], out int catValue))
                        {
                            if (cat.GetState() == State.NotInTheGame)
                            {
                                cat.setLocation(catValue);
                                cat.setState(State.Playing);
                            }
                            else
                            {
                                cat.AddMove(catValue);
                            }
                        }
                        break;
                        
                    case "P":
                        PrintState();
                        break;
                }
                
                if (!gameEnded && mouse.GetMoves().Count > 0 && cat.GetMoves().Count > 0)
                {
                    ProcessMoves();
                }
                
                if (gameEnded) break;
            }
            
            if (!gameEnded)
            {
                ProcessRemainingMoves();
            }
            
            GenerateFinalOutput();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    private void ProcessMoves()
    {
        var mouseMoves = mouse.GetMoves();
        var catMoves = cat.GetMoves();
        
        int maxTurns = Math.Max(mouseMoves.Count, catMoves.Count);
        
        for (int i = 0; i < maxTurns && !gameEnded; i++)
        {
            if (i < mouseMoves.Count)
            {
                mouse.Move(mouseMoves[i]);
            }
            
            if (i < catMoves.Count)
            {
                cat.Move(catMoves[i]);
            }
            
            if (mouse.getLocation() == cat.getLocation() && 
                mouse.GetState() == State.Playing && 
                cat.GetState() == State.Playing)
            {
                mouse.setState(State.Looser);
                cat.setState(State.Winner);
                gameEnded = true;
                break;
            }
        }
        
        mouseMoves.Clear();
        catMoves.Clear();
    }

    private void ProcessRemainingMoves()
    {
        var mouseMoves = mouse.GetMoves();
        var catMoves = cat.GetMoves();
        
        if (mouseMoves.Count > 0 || catMoves.Count > 0)
        {
            ProcessMoves();
        }
    }

    private void PrintState()
    {
        string catPos = (cat.GetState() == State.Playing) ? cat.getLocation().ToString() : "??";
        string mousePos = (mouse.GetState() == State.Playing) ? mouse.getLocation().ToString() : "??";
        
        int distance = 0;
        if (mouse.GetState() == State.Playing && cat.GetState() == State.Playing)
        {
            distance = Math.Abs(mouse.getLocation() - cat.getLocation());
        }
        else
        {
            distance = -1; 
        }
        
        string distanceStr = (distance >= 0) ? distance.ToString() : "??";
        
        outputLines.Add($"{catPos}\t{mousePos}\t{distanceStr}");
    }

    private void GenerateFinalOutput()
    {
        outputLines.Add("---------------------");
        outputLines.Add("");
        outputLines.Add("");
        outputLines.Add($"Cat distance: {cat.getDistanceTraveled()}");
        outputLines.Add($"Mouse distance: {mouse.getDistanceTraveled()}");
        outputLines.Add("");
        
        if (gameEnded)
        {
            outputLines.Add($"Мышь поймана в клетке: {mouse.getLocation()}");
        }
        else
        {
            outputLines.Add("Мышь ускользнула от кота");
        }
        
        try
        {
            File.WriteAllLines("PursuitLog.txt", outputLines);
            Console.WriteLine("Результаты записаны в PursuitLog.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing output file: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь для файла ChaseData.txt:");
        string inputFile = Console.ReadLine();
        
        if (string.IsNullOrEmpty(inputFile))
        {
            inputFile = "ChaseData.txt";
        }
        
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Файл с командами не найден!");
            return;
        }
        
        try
        {
            string[] lines = File.ReadAllLines(inputFile);
            if (lines.Length == 0)
            {
                Console.WriteLine("Файл с командами пуст!");
                return;
            }
            
            if (int.TryParse(lines[0], out int gameSize))
            {
                Game game = new Game(gameSize);
                game.ProcessInputFile(inputFile);
            }
            else
            {
                Console.WriteLine("Неверный размер поля!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        Console.WriteLine("Нажмите на любую кнопку для выхода...");
        Console.ReadKey();
    }
}