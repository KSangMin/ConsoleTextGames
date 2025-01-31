int[] ttt = new int[10];
bool phase = true;

int[,] cse =
{ 
    { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 },
    { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 },
    { 1, 5, 9 }, { 3, 5, 7 } 
};

int n = 9;

Console.WriteLine("플레이어 1: X, 플레이어 2: O\n");

while (n-- != 0)
{
    if (Check(phase ? 1 : 2)) continue;

    Console.Clear();
    PrintTicTactoe();

    if (CheckAnswer())
    {
        int cur = phase ? 1 : 2;
        Console.WriteLine($"플레이어 {cur} 승리!");
        break;
    }
}

void PrintTicTactoe()
{
    for(int i = 1; i <= 9; i++)
    {
        string output = i.ToString();
        switch(ttt[i])
        {
            case 1:
                output = "X";
                break;
            case 2:
                output = "O";
                break;
            default:
                break;
        }
        Console.Write(output + " ");
        if(i % 3 == 0) Console.WriteLine();
    }
    Console.WriteLine();
}

bool Check(int turn)
{
    phase = !phase;
    string mark = (turn == 1) ? "X" : "O";
    Console.WriteLine($"플레이어 {turn}의 차례({mark})");
    Console.Write("칸을 선택하세요: ");
    int i;
    bool rightInput = int.TryParse(Console.ReadLine(), out i);

    if (!rightInput || i < 1 || i > 9)
    {
        n++;
        Console.WriteLine("\n잘못된 입력입니다. 올바른 숫자를 입력해 주세요.(1~9)\n");
        return true;
    }
    if (ttt[i] != 0)
    {
        n++;
        Console.WriteLine("\n이미 선택된 칸입니다. 다시 선택하세요.\n");
        return true;
    }
    ttt[i] = turn;
    return false;
}

bool CheckAnswer()
{
    for(int i = 0; i < 8; i++)
    {
        if (ttt[cse[i, 0]] != 0 && ttt[cse[i, 0]] == ttt[cse[i, 1]] && ttt[cse[i, 1]] == ttt[cse[i, 2]])
        {
            return true;
        }
    }

    return false;
}