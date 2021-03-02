using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using Sudoku_Generator;

namespace Sudoku_Generator_GUI
{
    class Program
    {

        private static List<Sprite> interactables = new List<Sprite>();

        private static Sprite board;

        private static List<Text> text = new List<Text>();
        private static List<Text> nums = new List<Text>();

        private static Font defaultFont = new Font("res/HATTEN.TTF");

        private static bool clicked = false;
        private static bool symmetry = true;
        static void Main(string[] args)
        {
            VideoMode videoMode = new VideoMode(600 , 600);
            RenderWindow window = new RenderWindow(videoMode, "Sudoku Generator");
            window.SetFramerateLimit(30);

            Texture buttonTexture = new Texture("res/button.png");
            Sprite button = new Sprite(buttonTexture);

            Texture boardTexture = new Texture("res/SudokuBoard.png");
            board = new Sprite(boardTexture);

            Texture check = new Texture("res/checked.png");
            Texture uncheck = new Texture("res/unchecked.png");

            Sprite symCheck = new Sprite(check);

            interactables.Add(button);
            interactables.Add(symCheck);
            
            
            Text generateText = new Text("Generate", defaultFont);
            Text symmetryText = new Text("Symmetry?", defaultFont);
            symmetryText.FillColor = Color.Black;

            text.Add(generateText);
            text.Add(symmetryText);
            

            //Setup event handlers
            window.Closed += OnClose;
            window.MouseButtonPressed += OnClick;
            window.KeyPressed += OnKeyPress;

            View view = new View(new FloatRect(0, 0, 520, 520));
            view.Viewport = new FloatRect(0, 0, 1, 1);

            button.Position = new Vector2f((window.Size.X / 2) - (button.GetGlobalBounds().Width / 2), 
                (window.Size.Y / 2) - (button.GetGlobalBounds().Height / 2));

            generateText.Position = new Vector2f((window.Size.X / 2) - (generateText.GetGlobalBounds().Width / 2),
                (window.Size.Y / 2) - (generateText.GetGlobalBounds().Height / 2) - 10);

            board.Position = new Vector2f((view.Size.X / 2) - (board.GetGlobalBounds().Width / 2),
                (view.Size.Y / 2) - (board.GetGlobalBounds().Height / 2));

            symmetryText.Position = new Vector2f((window.Size.X / 2) - (symmetryText.GetGlobalBounds().Width / 2),
                (window.Size.Y) -  150);

            symCheck.Position = symmetryText.Position + new Vector2f(-symCheck.GetGlobalBounds().Width - 10, 0);





            while (window.IsOpen)
            {
                window.DispatchEvents();

                if(interactables.Count > 1)
                {
                    interactables[1].Texture = (symmetry) ? check : uncheck;
                }
                
                //DRAW CYCLE
                window.Clear(new Color(200, 200, 200));

                if (!clicked)
                {
                    foreach(Sprite s in interactables)
                    {
                        window.Draw(s);
                    }

                    foreach(Text t in text)
                    {
                        window.Draw(t);
                    }
                }
                else
                {
                    window.SetView(view);

                    window.Draw(board);

                    foreach(Text t in nums)
                    {
                        window.Draw(t);
                    }

                    window.SetView(window.DefaultView);
                }
                window.Display();
            }
        }

        static void OnKeyPress(Object sender, KeyEventArgs e)
        {
            float amount = 0.05f;

            switch (e.Code)
            {
                case Keyboard.Key.Down:
                    amount = -amount;

                    goto case Keyboard.Key.Up;

                case Keyboard.Key.Up:
                    foreach(Text t in nums)
                    {
                        t.Scale += new Vector2f(amount, amount);
                    }
                    break;

            }
        }


        static void OnClose(Object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }


        static void OnClick(Object sender, MouseButtonEventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;

            Vector2f corrected = w.MapPixelToCoords(new Vector2i(e.X, e.Y));

            if(interactables.Count > 1)
            {
                if (interactables[0].GetGlobalBounds().Contains(corrected.X, corrected.Y))
                {

                    interactables.Clear();
                    text.Clear();
                    clicked = true;

                    int majorO = 9;
                    int minorO = 2;
                    int size = 51;
                    Vector2f offset = new Vector2f(19, 5);

                    int[,] grid = Generator.Generate(symmetry);

                    for (int x = 0; x < 9; x++)
                    {
                        for (int y = 0; y < 9; y++)
                        {
                            if(grid[y,x] == 0)
                            {
                                continue;
                            }

                            Text n = new Text(grid[y,x].ToString(), defaultFont);
                            n.Scale = new Vector2f(1.25f, 1.25f);
                            n.FillColor = new Color(63, 63, 63);
                            n.Position = new Vector2f(((x / 3 + 1) * majorO) + (((x / 3) * 2 + (x % 3)) * minorO) + (x * size),
                                ((y / 3 + 1) * majorO) + (((y / 3) * 2 + (y % 3)) * minorO) + (y * size)) + board.Position + offset;

                            nums.Add(n);
                        }
                    }

                    
                }else if(interactables[1].GetGlobalBounds().Contains(corrected.X, corrected.Y))
                {
                    symmetry = !symmetry;
                }

            }


        }

    }
}
