
using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace MorpionClientGUI
{
    public class GameForm : Form
    {
        private Button[,] buttons = new Button[3, 3];
        private Label infoLabel;
        private Button quitButton;
        private StreamWriter writer;
        private StreamReader reader;
        private TcpClient socket;
        private string mySymbol = "?";

        public GameForm()
        {
            this.Text = "Morpion Client";
            this.Size = new Size(320, 420);
            InitializeGrid();

            try
            {
                socket = new TcpClient("127.0.0.1", 12345);
                NetworkStream stream = socket.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
                Close();
            }
        }

        private void InitializeGrid()
        {
            int size = 80;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    var btn = new Button();
                    btn.Location = new Point(j * size + 10, i * size + 10);
                    btn.Size = new Size(size, size);
                    btn.Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    btn.Tag = new Point(i, j);
                    btn.Click += OnCellClick;
                    buttons[i, j] = btn;
                    this.Controls.Add(btn);
                }

            infoLabel = new Label()
            {
                Location = new Point(10, 260),
                Size = new Size(280, 30),
                Text = "Connexion...",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(infoLabel);

            quitButton = new Button()
            {
                Text = "Quitter",
                Location = new Point(10, 300),
                Size = new Size(280, 30)
            };
            quitButton.Click += (s, e) => this.Close();
            this.Controls.Add(quitButton);
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Point p = (Point)btn.Tag;
            writer.WriteLine($"MOVE {p.X} {p.Y}");
        }

        private void ReceiveMessages()
        {
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("WELCOME"))
                    {
                        mySymbol = line.Split(' ')[1];
                        UpdateInfo($"Vous êtes '{mySymbol}'. En attente du tour...");
                    }
                    else if (line.StartsWith("UPDATE"))
                    {
                        var parts = line.Split(' ');
                        int x = int.Parse(parts[1]);
                        int y = int.Parse(parts[2]);
                        string symbol = parts[3];
                        this.Invoke(new Action(() =>
                        {
                            buttons[x, y].Text = symbol;
                            buttons[x, y].Enabled = false;
                        }));
                    }
                    else if (line.StartsWith("TURN"))
                    {
                        string current = line.Split(' ')[1];
                        UpdateInfo(current == mySymbol ? "À vous de jouer." : "Tour de l'adversaire.");
                        EnableButtons(current == mySymbol);
                    }
                    else if (line.StartsWith("END"))
                    {
                        string result = line.Split(' ')[1];
                        string msg = result == "DRAW" ? "Match nul !" :
                                     result == mySymbol ? "Vous avez gagné !" : "Vous avez perdu.";
                        MessageBox.Show(msg, "Fin de partie");
                        this.Invoke(new Action(() => this.Close()));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Connexion perdue.");
                this.Invoke(new Action(() => this.Close()));
            }
        }

        private void UpdateInfo(string text)
        {
            this.Invoke(new Action(() => infoLabel.Text = text));
        }

        private void EnableButtons(bool enable)
        {
            this.Invoke(new Action(() =>
            {
                foreach (var btn in buttons)
                {
                    if (btn.Text == "")
                        btn.Enabled = enable;
                }
            }));
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GameForm());
        }
    }
}
