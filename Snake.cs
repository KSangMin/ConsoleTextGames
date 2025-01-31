using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace SnakeGame
{
    // 방향을 표현하는 열거형입니다.
    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    struct Pair
    {
        public int x;
        public int y;
        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("시작을 원할 경우 Enter.");
            Console.ReadLine();
            Console.Clear();

            int time = 0;
            int speed = 100;//게임 속도 조절 (이 값을 변경하면 게임의 속도가 바뀝니다)

            // 음식의 위치를 무작위로 생성하고, 그립니다.
            FoodCreator foodCreator = new FoodCreator(80, 20, '$');
            foodCreator.DrawWall();
            foodCreator.CreateFood();

            // 뱀의 초기 위치와 방향을 설정하고, 그립니다.
            Point p = new Point(4, 5, '*');
            Snake snake = new Snake(p, 4, Direction.RIGHT, foodCreator);
            snake.Draw();

            // 게임 루프: 이 루프는 게임이 끝날 때까지 계속 실행됩니다.
            while (true)
            {
                // 키 입력이 있는 경우에만 방향을 변경하고 이동합니다.
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            snake.Move(Direction.UP);
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            snake.Move(Direction.LEFT);
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            snake.Move(Direction.DOWN);
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            snake.Move(Direction.RIGHT);
                            break;
                        default:
                            snake.Move(snake.dir);
                            break;
                    }
                }
                else
                {
                    snake.Move(snake.dir);
                }

                // 뱀이 벽이나 자신의 몸에 부딪혔는지, 음식을 먹었는지 등을 확인하고 처리하는 로직을 작성하세요.
                // 이동, 음식 먹기, 충돌 처리 등의 로직을 완성하세요.
                if (snake.IsHitAnything()) break;

                Thread.Sleep(speed); // 게임 진행
                time += speed;

                Console.SetCursorPosition(0, foodCreator.maxHeight + 2);
                Console.Write($"현재 뱀의 길이: {snake.bodies.Count}, 뱀이 먹은 음식 수 : {snake.bodies.Count - 4}, 버틴 시간: {time / 1000f}초");
                // 뱀의 상태를 출력합니다 (예: 현재 길이, 먹은 음식의 수 등)
            }
            Console.SetCursorPosition(0, foodCreator.maxHeight + 5);
        }
    }

    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public char sym { get; set; }

        // Point 클래스 생성자
        public Point(int _x, int _y, char _sym)
        {
            x = _x;
            y = _y;
            sym = _sym;
        }

        // 점을 그리는 메서드
        public void Draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(sym);

            return;
        }

        // 점을 지우는 메서드
        public void Clear()
        {
            sym = ' ';
            Draw();

            return;
        }

        // 두 점이 같은지 비교하는 메서드
        public bool IsHit(Point p)
        {
            return p.x == x && p.y == y;
        }
    }

    public class Snake
    {
        public char headSym = '@';//머리를 나타내는 문자
        public List<Point> bodies = new List<Point>(); // 머리를 포함한 몸 Point 배열
        public Point tail;//꼬리(사과 먹을 때 사용)
        public Direction dir;
        public FoodCreator f;
        Dictionary<Direction, Pair> d = new Dictionary<Direction, Pair>();

        public Snake(Point p, int len, Direction direction, FoodCreator food)
        {
            d[Direction.LEFT] = new Pair(-1, 0);
            d[Direction.RIGHT] = new Pair(1, 0);
            d[Direction.UP] = new Pair(0, -1);
            d[Direction.DOWN] = new Pair(0, 1);

            dir = direction;
            f = food;

            //몸 배열 초기화
            bodies.Add(new Point(p.x, p.y, headSym));
            for (int i = 1; i < len; i++)
            {
                Point temp = new Point(p.x - i, p.y, p.sym);
                bodies.Add(temp);
            }
            tail = new Point(bodies.Last().x, bodies.Last().y, bodies.Last().sym);
        }

        public void Draw()
        {
            foreach (Point p in bodies)
            {
                p.Draw();
            }

            return;
        }

        public void AddHead(Point p)
        {//이동 시 사용
            bodies.Insert(0, p);
            Draw();

            return;
        }

        public void AddTail(Point p)
        {//사과 먹을 때 사용
            bodies.Add(p);
            Draw();

            return;
        }

        public bool CheckDirection(Direction direction)
        {//이동 방향과 정반대로 이동 하려는지 체크
            switch (direction)
            {
                case Direction.UP:
                    if(dir == Direction.DOWN) return true;
                    break;
                case Direction.DOWN:
                    if(dir == Direction.UP) return true;
                    break;
                case Direction.LEFT:
                    if(dir == Direction.RIGHT) return true;
                    break;
                case Direction.RIGHT:
                    if (dir == Direction.LEFT) return true;
                    break;
                default:
                    break;
            }

            return false;
        }

        public void Move(Direction direction)
        {//이동
            if (CheckDirection(direction)) return;//이동 방향과 정반대로 이동 불가능
            dir = direction;
            int dx = d[dir].x, dy = d[dir].y;

            Point head = bodies[0];//기존 머리
            Point newHead = new Point(head.x + dx, head.y + dy, head.sym);//한 칸 이동할 위치의 새로운 머리

            head.sym = bodies.Last().sym;
            AddHead(newHead);
            tail = new Point(bodies.Last().x, bodies.Last().y, bodies.Last().sym);
            bodies.Last().Clear();
            bodies.Remove(bodies.Last());
            Draw();

            return;
        }

        public bool IsHitAnything()
        {//충돌 판정
            Point head = bodies[0];

            //벽
            if (head.x < 1 || f.maxWidth < head.x
                || head.y < 1 || f.maxHeight < head.y)
            {
                Console.SetCursorPosition(0, f.maxHeight + 4);
                Console.Write("벽에 부딪혔습니다!");
                return true;
            }

            //자기 몸
            for (int i = 1; i < bodies.Count; i++)
            {
                if (head.IsHit(bodies[i]))
                {
                    Console.SetCursorPosition(0, f.maxHeight + 4);
                    Console.Write("자기 몸에 부딪혔습니다!");
                    return true;
                }
            }

            IsHitApple();//사과

            return false;
        }

        public void IsHitApple()
        {//사과 충돌
            Point head = bodies[0];

            foreach (Point apple in f.apples)
            {
                if (head.IsHit(apple))
                {//사과를 먹으면 미리 저장해두었던 꼬리를 몸에 추가시켜서 길이 연장
                    AddTail(tail);
                    f.apples.Remove(apple);
                    f.CreateFood();//새 사과 생성
                    return;
                }
            }

            return;
        }
    }

    public class FoodCreator
    {
        public int maxWidth;
        public int maxHeight;
        public char sym;
        public List<Point> apples = new List<Point>();

        public FoodCreator(int width, int height, char mark)
        {
            maxWidth = width;
            maxHeight = height;
            sym = mark;
        }

        public void DrawWall()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string('#', maxWidth + 2));

            for (int i = 1; i <= maxHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(maxWidth + 1, i);
                Console.Write("#");
            }

            Console.SetCursorPosition(0, maxHeight + 1);
            Console.Write(new string('#', maxWidth + 2));

            return;
        }

        public Point CreateFood()
        {
            Random rand = new Random();
            Point p = new Point(rand.Next(1, maxWidth + 1), rand.Next(1, maxHeight + 1), sym);
            apples.Add(p);
            p.Draw();

            return p;
        }
    }
}
