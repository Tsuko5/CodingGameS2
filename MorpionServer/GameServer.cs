
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class GameState
{
    public string[,] board = new string[3, 3];
    public string currentPlayer = "X";
    public bool gameEnded = false;

    public bool MakeMove(int x, int y, string player)
    {
        if (!gameEnded && board[x, y] == null && currentPlayer == player)
        {
            board[x, y] = player;
            currentPlayer = player == "X" ? "O" : "X";
            return true;
        }
        return false;
    }

    public string CheckWinner()
    {
        string[] players = { "X", "O" };
        foreach (var p in players)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((board[i, 0] == p && board[i, 1] == p && board[i, 2] == p) ||
                    (board[0, i] == p && board[1, i] == p && board[2, i] == p))
                    return p;
            }
            if ((board[0, 0] == p && board[1, 1] == p && board[2, 2] == p) ||
                (board[0, 2] == p && board[1, 1] == p && board[2, 0] == p))
                return p;
        }

        bool full = true;
        foreach (var cell in board)
            if (cell == null) full = false;

        return full ? "DRAW" : null;
    }
}

class ClientHandler
{
    public TcpClient Client;
    public string Symbol;
    public StreamWriter Writer;
    public StreamReader Reader;

    public ClientHandler(TcpClient client, string symbol)
    {
        Client = client;
        Symbol = symbol;
        NetworkStream stream = client.GetStream();
        Writer = new StreamWriter(stream) { AutoFlush = true };
        Reader = new StreamReader(stream);
    }
}

class GameServer
{
    static TcpListener listener = new TcpListener(IPAddress.Any, 12345);
    static List<ClientHandler> clients = new List<ClientHandler>();
    static GameState gameState = new GameState();

    static void Main()
    {
        listener.Start();
        Console.WriteLine("Serveur démarré sur le port 12345.");

        while (clients.Count < 2)
        {
            var client = listener.AcceptTcpClient();
            string symbol = clients.Count == 0 ? "X" : "O";
            var handler = new ClientHandler(client, symbol);
            clients.Add(handler);
            handler.Writer.WriteLine($"WELCOME {symbol}");
            new Thread(() => HandleClient(handler)).Start();
        }
    }

    static void HandleClient(ClientHandler handler)
    {
        try
        {
            string line;
            while ((line = handler.Reader.ReadLine()) != null)
            {
                if (line.StartsWith("MOVE"))
                {
                    string[] parts = line.Split(' ');
                    int x = int.Parse(parts[1]);
                    int y = int.Parse(parts[2]);

                    if (gameState.MakeMove(x, y, handler.Symbol))
                    {
                        foreach (var c in clients)
                        {
                            c.Writer.WriteLine($"UPDATE {x} {y} {handler.Symbol}");
                            c.Writer.WriteLine($"TURN {gameState.currentPlayer}");
                        }

                        string result = gameState.CheckWinner();
                        if (result != null)
                        {
                            gameState.gameEnded = true;
                            foreach (var c in clients)
                                c.Writer.WriteLine($"END {result}");
                        }
                    }
                    else
                    {
                        handler.Writer.WriteLine("INVALID");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur client : {ex.Message}");
        }
    }
}
