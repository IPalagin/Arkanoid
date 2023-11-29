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
        static bool attemptsDecreased;
        static bool isGame;

        static int blocksNum;
        static int blocksCount;
        static int lvlNumber = 1;

        static int[] blocksHP;
        static int[] blocks2HP;

        public static void SetStartPosition()
        {
            int index = 0;

            blocksHP = new int[blocksNum];
            blocks2HP = new int[blocksNum];

            for (int i = 0; i < blocksNum; i++)
            {
                if (blocks[i] != null)
                {
                    blocksHP[i] = 1;
                }

                if (blocks2[i] != null)
                {
                    blocks2HP[i] = 2;
                }
            }

            if (lvlNumber == 1)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                       blocks[index] = new Sprite(blockTexture);
                       blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75, y * (blocks[index].TextureRect.Height + 15) + 100);
                       index++;
                    }
                }
            }

            if(lvlNumber == 2)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (x > 0 && x < 9 && y > 0 && y < 9)
                        {
                            blocks[index] = new Sprite(blockTexture);
                            blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75, y * (blocks[index].TextureRect.Height + 15) + 100);
                        }
                        else
                        {
                            blocks2[index] = new Sprite(block2Texture);
                            blocks2[index].Position = new Vector2f(x * (blocks2[index].TextureRect.Width + 15) + 75, y * (blocks2[index].TextureRect.Height + 15) + 100);
                        }
                        index++;
                    }
                }
            }

            if(lvlNumber == 3)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if ((x + y) % 2 == 0)
                        {
                            blocks[index] = new Sprite(blockTexture);
                            blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75, y * (blocks[index].TextureRect.Height + 15) + 100);
                        }
                        else
                        {
                            blocks2[index] = new Sprite(block2Texture);
                            blocks2[index].Position = new Vector2f(x * (blocks2[index].TextureRect.Width + 15) + 75, y * (blocks2[index].TextureRect.Height + 15) + 100);
                        }
                        index++;
                    }
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

            blocksNum = 100;
            attemptsDecreased = false;
            isGame = true;
            attemptsNumber = 3;
            blocksCount = blocksNum;

            ball = new Ball(ballTexture);
            stick = new Sprite(stickTexture);
            blocks = new Sprite[blocksNum];
            blocks2 = new Sprite[blocksNum];

            for (int i = 0; i < blocksNum; i++)
            {
                blocks[i] = new Sprite(blockTexture);
                blocks2[i] = new Sprite(block2Texture);
            }

            SetStartPosition();

            while (window.IsOpen == true && isGame == true)
            {
                window.Clear();

                window.DispatchEvents();

                PressOnStart();

                ball.Move(new Vector2i(0, 0), new Vector2i(800, 600));

                ball.CheckCollision(stick, "Stick");

                for (int i = 0; i < blocksNum; i++)
                {
                    if (blocks[i] != null)
                    {
                        if (ball.CheckCollision(blocks[i], "Block") == true)
                        {
                            blocksHP[i]--;
                            if (blocksHP[i] == 0)
                            {
                                blocks[i] = null;
                            }
                            break;
                        }
                    }

                    if (blocks2[i] != null)
                    {
                        if (ball.CheckCollision(blocks2[i], "Block2") == true)
                        {
                            blocks2HP[i]--;
                            if (blocks2HP[i] == 0)
                            {
                                blocks2[i] = null;
                            }
                            break;
                        }
                    }
                }

                stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.TextureRect.Width * 0.5f, stick.Position.Y);

                window.Draw(ball.sprite);
                window.Draw(stick);

                for (int i = 0; i < blocksNum; i++)
                {
                    if (blocks[i] != null)
                    {
                        window.Draw(blocks[i]);
                    }

                    if (blocks2[i] != null)
                    {
                        window.Draw(blocks2[i]);
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

                if (blocksCount <= 0)
                {
                    lvlNumber++;
                    ball.speed += 5;
                    SetStartPosition();
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