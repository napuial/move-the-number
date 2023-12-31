﻿string controlSequence = "12345678 "; 
int boardSize = 9; 
int placeSize = 2; 
int visibleMaxNumber = 8; 
int emptyPlaceIndex = 8; 
char[,] board = FillBoard(CreateBoard(boardSize, placeSize), visibleMaxNumber);

Console.WriteLine(@"> > > > > > > > > > > > > > > > >
> Set this correct pattern to win:
> 123
> 456
> 78

> Numbers can be moved by putting them on the empty place. 
> You can choose one number from numbers around the empty place and type it. 
> Then the number will change its place.
> > > > > > > > > > > > > > > > >

This is your playboard:");

while (!GameControl(board, controlSequence)) 
{
    SetAvailability(board, emptyPlaceIndex);
    ShowBoard(board);
    Console.WriteLine("\nType a number to make your move:");
    try 
    {
        char number = Convert.ToChar(Console.ReadLine());
        if (ValidUserInput(board, controlSequence, number)) 
        {
            emptyPlaceIndex = SetEmptyPlace(board, emptyPlaceIndex, number);
            ResetAvailability(board);
        }
        Console.WriteLine();
    } 
    catch (Exception e) 
    {
        Console.WriteLine("\nType a number from 1-8 range which can be moved!");
        Console.WriteLine();
    }
}
Console.WriteLine(@"> > > > > > > > > > > > > > > > >
>Congratulations! 
>123
>456
>78

You won!
> > > > > > > > > > > > > > > > >");

char[,] CreateBoard(int outsideLength, int innerLength) 
{ 
    return new char[outsideLength, innerLength];
}

string GenerateRandomFiller(int amountOfChars) 
{
    string result = "";
    Random random = new Random();
    int r;
    int counter = amountOfChars;
    while (counter > 0) 
    {
        r = random.Next(1, amountOfChars + 1);
        if (!result.Contains("" + r)) 
        {
            result += r;
            counter--;
        }
    }
    return result + ' '; 
}

char[,] FillBoard(char[,] board, int visibleMaxNumber) 
{ 
    string filler = GenerateRandomFiller(visibleMaxNumber);
    for (int i = 0; i < board.GetLength(0); i++) 
    {
        board[i, 0] = '0';
        board[i, 1] = filler[i];
    }
    return board;
}

char[,] SetAvailability(char[,] board, int emptyPlaceIndex) 
{
    //top
    try 
    {
        board[emptyPlaceIndex - 3, 0] = '1'; 
    } 
    catch (Exception ignored) { }
    //bottom
    try 
    {
        board[emptyPlaceIndex + 3, 0] = '1'; 
    } 
    catch (Exception ignored) { }
    //left
    try 
    {
        if (emptyPlaceIndex % 3 != 0) 
        {
            board[emptyPlaceIndex - 1, 0] = '1'; 
        }
    } 
    catch(Exception ignored) { }
    //right
    try
    {
        if ((emptyPlaceIndex + 1) % 3 != 0) 
        {
            board[emptyPlaceIndex + 1, 0] = '1'; 
        }
    } 
    catch(Exception ignored) { }
    return board;
}

char[,] ResetAvailability(char[,] board) 
{
    for (int i = 0; i < board.GetLength(0); i++) 
    {
        board[i, 0] = '0';
    }
    return board;
}

int SetEmptyPlace(char[,] board, int emptyPlaceIndex, char c) 
{
    for (int i = 0; i < board.GetLength(0); i++) 
    {
        if (board[i, 1] == c && board[i, 0] != '1') 
        {
            break;
        }
        if (board[i, 1] == c) 
        {
            board[i, 1] = ' ';
            board[emptyPlaceIndex, 1] = c;
            return i;
        }
    }
    return emptyPlaceIndex;
}

bool GameControl(char[,] board, string controlSequence) 
{ 
    string sequence = "";
    for (int i = 0; i < board.GetLength(0); i++) 
    {
        sequence += board[i, 1];
    }
    return sequence.Equals(controlSequence);
}

void ShowBoard(char[,] board) 
{
    for (int i = 0; i < board.GetLength(0); i++) 
    {
        Console.Write(board[i, 1]);
        if ((i + 1) % 3 == 0) 
        {
            Console.WriteLine();
        }
    }
}

bool ValidUserInput(char[,] board, string controlSequence, char c) 
{
    string validInputs = "";
    bool inputStatus = true;
    if (!controlSequence[..^1].Contains(c)) 
    {
        inputStatus = false;
    } 
    else 
    {
        for (int i = 0; i < board.GetLength(0); i++) 
        {
            if (board[i, 1] == c) 
            {
                if (board[i, 0] == '0') 
                {
                    inputStatus = false;
                    break;
                } 
                else 
                {
                    return true;
                }
            }   
        }
    }
    for (int j = 0; j < board.GetLength(0); j++) 
    {
        if (board[j, 0] == '1') 
        {
            validInputs += " " + board[j, 1] + ",";
        }
    }
    validInputs = validInputs[..^1];
    if (!inputStatus) 
    {
        Console.WriteLine($"\n'{c}' is not valid input.\nValid inputs for this turn are:{validInputs}.");
    }
    return inputStatus;
}

//RunTests();

void RunTests() 
{
    Console.WriteLine("Test for board expected length. Success: " + TestCreateBoard(boardSize, placeSize)); 
    Console.WriteLine("Test for filler generated with unique chars and expected length. Success: " + 
                        TestGenerateRandomFiller(visibleMaxNumber)); 
    Console.WriteLine("Test for filling the board. Success: " + TestFillBoard(board)); 
    Console.WriteLine("Test for setting the availability of making a move. Success: " + TestSetAvailability(board)); 
    Console.WriteLine("Test for setting the correct emptyPlaceIndex. Success: " + TestSetEmptyPlace()); 
    Console.WriteLine("Test for the end of the game. Success: " + TestGameControl(controlSequence));
}

//test methods

bool TestCreateBoard(int outsideLength, int innerLength) 
{
    int expectedBoardLength = outsideLength * innerLength;
    char[,] board = CreateBoard(outsideLength, innerLength);
    return board.Length == expectedBoardLength && board.GetLength(0) == outsideLength;
}

bool TestGenerateRandomFiller(int amountOfChars) 
{
    string geneartedRandomFiller = GenerateRandomFiller(amountOfChars); 
    string exceptionedChars = ""; 
    for (int i = 1; i < amountOfChars; i++) 
    {
        exceptionedChars += i;
    }
    exceptionedChars += ' ';
    foreach (char c in exceptionedChars) 
    {
        if (!geneartedRandomFiller.Contains(c)) 
        {
            return false;
        }
    }
    return geneartedRandomFiller.Length == amountOfChars + 1;
}

bool TestFillBoard(char[,] board) 
{ 
    int amountOfChars = 0;
    foreach (char c in board) 
    {
        amountOfChars++;
    }
    return amountOfChars == board.Length; 
}

bool TestSetAvailability(char[,] board) 
{
    //case 1: top-left
    int case1 = 0;
    char[,] arrayForCase1 = SetAvailability(board, case1);
    char[,] suppotArrayForCase1 = {{'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'0'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase1[i, 0] != suppotArrayForCase1[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 2: top-middle
    int case2 = 1;
    char[,] arrayForCase2 = SetAvailability(board, case2);
    char[,] suppotArrayForCase2 = {{'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase2[i, 0] != suppotArrayForCase2[i, 0]) {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 3: top-right
    int case3 = 2;
    char[,] arrayForCase3 = SetAvailability(board, case3);
    char[,] suppotArrayForCase3 = {{'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase3[i, 0] != suppotArrayForCase3[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 4: middle-left
    int case4 = 3;
    char[,] arrayForCase4 = SetAvailability(board, case4);
    char[,] suppotArrayForCase4 = {{'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase4[i, 0] != suppotArrayForCase4[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 5: middle-middle
    int case5 = 4;
    char[,] arrayForCase5 = SetAvailability(board, case5);
    char[,] suppotArrayForCase5 = {{'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase5[i, 0] != suppotArrayForCase5[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 6: middle-right
    int case6 = 5;
    char[,] arrayForCase6 = SetAvailability(board, case6);
    char[,] suppotArrayForCase6 = {{'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase6[i, 0] != suppotArrayForCase6[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 7: bottom-left
    int case7 = 6;
    char[,] arrayForCase7 = SetAvailability(board, case7);
    char[,] suppotArrayForCase7 = {{'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase7[i, 0] != suppotArrayForCase7[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 8: bottom-middle
    int case8 = 7;
    char[,] arrayForCase8 = SetAvailability(board, case8);
    char[,] suppotArrayForCase8 = {{'0'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase8[i, 0] != suppotArrayForCase8[i, 0]) 
        {
            return false;
        }
    }
    board = ResetAvailability(board);
    //case 9: bottom-right
    int case9 = 8;
    char[,] arrayForCase9 = SetAvailability(board, case9);
    char[,] suppotArrayForCase9 = {{'0'}, {'0'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}};
    for (int i = 0; i < board.GetLength(0);  i++) 
    {
        if (arrayForCase9[i, 0] != suppotArrayForCase9[i, 0]) 
        {
            return false;
        }
    }
    return true;
}

bool TestSetEmptyPlace() 
{
    int emptyPlaceIndex = 2;
    char[,] randomBoard = {{'0', '3'}, {'1', '5'}, {'0', ' '}, 
                           {'0', '4'}, {'0', '6'}, {'1', '2'},
                           {'0', '8'}, {'0', '1'}, {'0', '7'}};
    return SetEmptyPlace(randomBoard, emptyPlaceIndex, '5') == 1 && SetEmptyPlace(randomBoard, emptyPlaceIndex, '2') == 5 
            && SetEmptyPlace(randomBoard, emptyPlaceIndex, '3') == 2;
}

bool TestGameControl(string controlSequence) 
{
    char[,] expectedBoard = {{'0', '1'}, {'0', '2'}, {'0', '3'}, 
                             {'0', '4'}, {'0', '5'}, {'0', '6'},
                             {'0', '7'}, {'0', '8'}, {'0', ' '}};
    char[,] randomBoard = {{'0', '1'}, {'0', '2'}, {'0', '3'}, 
                           {'0', '4'}, {'0', '5'}, {'0', '6'},
                           {'0', '7'}, {'0', ' '}, {'0', '8'}};

    return GameControl(expectedBoard, controlSequence) && !GameControl(randomBoard, controlSequence);
}

//support methods

void ShowBoardAvailability(char[,] board) 
{ 
    for (int i = 0; i < board.GetLength(0); i++) 
    { 
        Console.Write(board[i, 0]);
        if ((i + 1) % 3 == 0) 
        {
            Console.WriteLine();
        }
    }
}
