using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid
{
    class Program
    {
        static RenderWindow window;

        static Texture ballTexture;
        static Texture stickTexture;
        static Texture blockTexture;
        static Texture block2Texture;

        static Sprite stick;
        static Sprite[] blocks;
        static Sprite[] blocks2;

        static Ball ball;

        static int attemptsNumber;
        static bool attemptsDecreased = false;
        static bool isGame = true;

        static int blocksNum = 100;
        static int lvlNumber = 1;

        public static void SetStartPosition()
        {
            int index = 0;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    blocks[index] = new Sprite(blockTexture);
                    blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75, y * (blocks[index].TextureRect.Height + 15) + 100);
                    
                    if(index % 2 == 0)
                    {
                        blocks2[index] = new Sprite(block2Texture);
                        blocks2[index].Position = new Vector2f(x * (blocks2[index].TextureRect.Width + 15) + 75, y * (blocks2[index].TextureRect.Height + 15) + 100);
                    }
                    index++;
                }
            }
            stick.Position = new Vector2f(385, 500);
            ball.sprite.Position = new Vector2f(380, 475);
        }

        public static void PressOnStart()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
            {
                ball.Start(5, new Vector2f(0, -1));
            }
        }

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "Arkanoid");
            window.SetFramerateLimit(60);
            window.Closed += Window_Closed;

            ballTexture = new Texture("Ball.png");
            stickTexture = new Texture("Stick.png");
            blockTexture = new Texture("Block.png");
            block2Texture = new Texture("Block2.png");

            ball = new Ball(ballTexture);
            stick = new Sprite(stickTexture);
            blocks = new Sprite[blocksNum];
            blocks2 = new Sprite[blocksNum];
            for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);
            for (int i = 0; i < blocks2.Length; i++) blocks2[i] = new Sprite(block2Texture);

            SetStartPosition();

            attemptsNumber = 3;

            while (window.IsOpen == true && isGame == true)
            {
                window.Clear();

                window.DispatchEvents();

                PressOnStart();

                ball.Move(new Vector2i(0, 0), new Vector2i(800, 600));

                ball.CheckCollision(stick, "Stick");

                for (int i = 0; i < blocks.Length; i++)
                {
                    if (ball.CheckCollision(blocks[i], "Block") == true)
                    {
                        blocks[i].Position = new Vector2f(1000, 1000);
                        break;
                    }

                    if (ball.CheckCollision(blocks2[i], "Block2") == true)
                    {
                        blocks2[i].Position = new Vector2f(1000, 1000);
                        break;
                    }
                }

                stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.TextureRect.Width * 0.5f, stick.Position.Y);

                window.Draw(ball.sprite);
                window.Draw(stick);
                if (lvlNumber == 1)
                {
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        if (i % 2 != 0)
                        {
                            window.Draw(blocks[i]);
                        }
                    }
                    for (int i = 0; i < blocks2.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            window.Draw(blocks2[i]);
                        }
                    }
                }

                window.Display();

                if (ball.sprite.Position.Y == 600 && !attemptsDecreased)
                {
                    attemptsNumber--;
                    attemptsDecreased = true;
                    ball.sprite.Position = new Vector2f(380, 475);
                    ball.speed = 0;
                }
                else if (ball.sprite.Position.Y < 600)
                {
                    attemptsDecreased = false;
                }

                if (attemptsNumber == 0)
                {
                    isGame = false;
                }

                if (!isGame)
                {
                    while (true)
                    {
                        isGame = true;
                        attemptsDecreased = true;
                        attemptsNumber = 3;
                        SetStartPosition();
                        lvlNumber = 1;

                        break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(60, 10);
                Console.Write("Попытки: {0}", attemptsNumber);
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}