using System.Threading;

namespace TextRPG
{
    internal class Program
    {
        public static Random random = new Random();

        static void Main(string[] args)
        {
            Console.Write("이름을 입력하세요: ");
            string name = Console.ReadLine();
            Console.Clear();
            Warrior warrior = new Warrior(name);
            Stage stage = new Stage(warrior);
            
            while (true)
            {
                Monster monster = (random.Next(0, 100) < 10) ? new Dragon() : new Goblin();

                warrior.PrintState(0);
                monster.PrintState(1);
                if (stage.Start(monster)) break;
            }
        }

        interface ICharacter
        {
            string Name { get; set; }
            int Health { get; set; }
            int Attack { get; set; }
            bool IsDead { get; set; }
            bool TakeDamage(Character c);
        }

        class Character : ICharacter
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Attack { get; set; }
            public bool IsDead { get; set; }

            public Character()
            {
                IsDead = false;
            }

            public bool TakeDamage(Character c)
            {
                Health -= c.Attack;

                Console.WriteLine($"\n{c.Name}의 공격! {Name}은 {c.Attack} 대미지를 입었다!");

                if (Health <= 0)
                {
                    Health = 0;
                    IsDead = true;
                    return true;
                }

                return false;
            }

            public void PrintState(int row)
            {
                Console.SetCursorPosition(0, row);
                Console.WriteLine(Name);
                Console.SetCursorPosition(Name.Length + 5, row);
                Console.WriteLine($"체력: {Health, 2}, 공격력: {Attack, 2}                ");
            }
        }

        class Warrior : Character
        {
            public Warrior(string name)
            {
                Name = name;
                Health = 25;
                Attack = 5;
            }
        }

        class Monster : Character
        {
            
        }

        class Goblin : Monster
        {
            public Goblin()
            {
                Name = "고블린";
                Health = random.Next(5, 12);
                Attack = random.Next(1, 4);
            }
        }

        class Dragon : Monster
        {
            public Dragon()
            {
                Name = "드래곤";
                Health = random.Next(50, 101);
                Attack = random.Next(13, 26);
            }
        }

        interface IItem
        {
            string Name { get; set; }
            void Use(Warrior warrior);
        }

        class HealthPotion : IItem
        {
            public string Name { get; set; }
            public int Amount { get; set; }

            public HealthPotion()
            {
                Name = "체력 증가 물약";
                Amount = 10;
            }

            public void Use(Warrior w)
            {
                w.Health += Amount;
                Console.WriteLine($"\n{Name}을 먹고 체력이 {Amount} 증가했다!");
            }
        }
        class StrengthPotion : IItem
        {
            public string Name { get; set; }
            public int Amount { get; set; }

            public StrengthPotion()
            {
                Name = "공격력 증가 물약";
                Amount = 2;
            }

            public void Use(Warrior w)
            {
                w.Attack += Amount;
                Console.WriteLine($"\n{Name}을 먹고 공격력이 {Amount} 증가했다!");
            }
        }

        class Stage
        {
            public Warrior warrior;
            public Monster monster;
            public IItem[] items = { new HealthPotion(), new StrengthPotion() };

            public Stage(Warrior w)
            {
                warrior = w;
            }

            public bool Start(Monster m)
            {
                monster = m;
                Console.WriteLine($"\n{monster.Name}을(를) 만났습니다! 전투 시작!");
                Thread.Sleep(1000);

                while (true)
                {
                    PrintAllStates();
                    if (monster.TakeDamage(warrior))//몬스터가 죽으면
                    {
                        PrintAllStates();
                        Console.WriteLine($"\n{monster.Name}을(를) 이겼습니다!                                                       ");
                        items[random.Next(0, 2)].Use(warrior);
                        Thread.Sleep(1000);
                        Console.Clear();
                        return false;
                    }
                    Thread.Sleep(1000);

                    PrintAllStates();
                    if (warrior.TakeDamage(monster))//플레이어가 죽으면
                    {
                        PrintAllStates();
                        Console.WriteLine("\n당신은 패배했습니다... 푹 쉬고 돌아와서 다시 싸워 보죠!");
                        return true;
                    }
                    Thread.Sleep(1000);
                }
            }

            public void PrintAllStates()
            {
                warrior.PrintState(0);
                monster.PrintState(1);
            }
        }
    }
}
