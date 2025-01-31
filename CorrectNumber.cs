int answer = new Random().Next(1, 101);
int input = 0, n = 0;

Console.WriteLine("숫자 맞추기 게임을 시작합니다. 1에서 100까지의 숫자 중 하나를 맞춰보세요.: ");
while (answer != input)
{
    n++;
    Console.Write("숫자를 입력하세요: ");
    input = int.Parse(Console.ReadLine());
    if (input < answer)
    {
        Console.WriteLine("너무 작습니다!");
    }
    else if (input > answer)
    {
        Console.WriteLine("너무 큽니다!");
    }
    else
    {
        Console.WriteLine($"축하합니다! {n}번 만에 숫자를 맞추었습니다.");
    }
}
