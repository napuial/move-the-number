string controlSequence = "12345678 "; 
int boardSize = 9; 
int placeSize = 2; 
int visibleMaxNumber = 8; 
int emptyPlaceIndex = 8; 
char[,] board = fillBoard(createBoard(boardSize, placeSize), visibleMaxNumber); 

Console.WriteLine("\nSet this correct pattern to win:\n\n> 123\n> 456\n> 78  \n\nThis is your playboard:\n");
while(!gameControl(board, controlSequence)) {
    setAvailability(board, emptyPlaceIndex);
    showBoard(board);
    Console.WriteLine("\nType a number to make your move:");
    char number = Convert.ToChar(Console.ReadLine());
    emptyPlaceIndex = setEmptyPlace(board, emptyPlaceIndex, number);
    resetAvailability(board);
    Console.WriteLine();
}
Console.WriteLine("\nCongratulations!\n123\n456\n789\nYou won!");

char[,] createBoard(int outsideLength, int innerLength) { 
    return new char[outsideLength, innerLength];
}

string generateRandomFiller(int amountOfChars) {
    string result = "";
    Random random = new Random();
    int r;
    int counter = amountOfChars;
    while(counter > 0) {
        r = random.Next(1, amountOfChars + 1);
        if(!result.Contains("" + r)) {
            result += r;
            counter--;
        }
    }
    return result + ' '; 
}

char[,] fillBoard(char[,] board, int visibleMaxNumber) { 
    string filler = generateRandomFiller(visibleMaxNumber);
    for(int i = 0; i < board.GetLength(0); i++) {
        board[i, 0] = '0';
        board[i, 1] = filler[i];
    }
    return board;
}

char[,] setAvailability(char[,] board, int emptyPlaceIndex) {
    //top
    try {
        board[emptyPlaceIndex - 3, 0] = '1'; 
    } catch (Exception ignored) { }
    //bottom
    try {
        board[emptyPlaceIndex + 3, 0] = '1'; 
    } catch (Exception ignored) { }
    //left
    try {
        if(emptyPlaceIndex % 3 != 0) {
            board[emptyPlaceIndex - 1, 0] = '1'; 
        }
    } catch(Exception ignored) { }
    //right
    try  {
        if((emptyPlaceIndex + 1) % 3 != 0) {
            board[emptyPlaceIndex + 1, 0] = '1'; 
        }
    } catch(Exception ignored) { }
    return board;
}

char[,] resetAvailability(char[,] board) {
    for(int i = 0; i < board.GetLength(0); i++) {
        board[i, 0] = '0';
    }
    return board;
}

int setEmptyPlace(char[,] board, int emptyPlaceIndex, char c) {
    for(int i = 0; i < board.GetLength(0); i++) {
        if(board[i, 1] == c && board[i, 0] != '1') {
            break;
        }
        if(board[i, 1] == c) {
            board[i, 1] = ' ';
            board[emptyPlaceIndex, 1] = c;
            return i;
        }
    }
    return emptyPlaceIndex;
}

bool gameControl(char[,] board, string controlSequence) { 
    string sequence = "";
    for(int i = 0; i < board.GetLength(0); i++) {
        sequence += board[i, 1];
    }
    return sequence.Equals(controlSequence);
}

void showBoard(char[,] board) {
    for(int i = 0; i < board.GetLength(0); i++) {
        Console.Write(board[i, 1]);
        if((i + 1) % 3 == 0) {
            Console.WriteLine();
        }
    }
}

//runTests();

void runTests() {
    Console.WriteLine("Test for board expected length. Success: " + testCreateBoard(boardSize, placeSize)); 
    Console.WriteLine("Test for filler generated with unique chars and expected length. Success: " + 
                        testGenerateRandomFiller(visibleMaxNumber)); 
    Console.WriteLine("Test for filling the board. Success: " + testFillBoard(board)); 
    Console.WriteLine("Test for setting the availability of making a move. Success: " + testSetAvailability(board)); 
    Console.WriteLine("Test for setting the correct emptyPlaceIndex. Success: " + testSetEmptyPlace()); 
    Console.WriteLine("Test for the end of the game. Success: " + testGameControl(controlSequence));
}

//test methods

bool testCreateBoard(int outsideLength, int innerLength) {
    int expectedBoardLength = outsideLength * innerLength;
    char[,] board = createBoard(outsideLength, innerLength);
    return board.Length == expectedBoardLength && board.GetLength(0) == outsideLength;
}

bool testGenerateRandomFiller(int amountOfChars) {
    string geneartedRandomFiller = generateRandomFiller(amountOfChars); 
    string exceptionedChars = ""; 
    for(int i = 1; i < amountOfChars; i++) {
        exceptionedChars += i;
    }
    exceptionedChars += ' ';
    foreach(char c in exceptionedChars) {
        if(!geneartedRandomFiller.Contains(c)) {
            return false;
        }
    }
    return geneartedRandomFiller.Length == amountOfChars + 1;
}

bool testFillBoard(char[,] board) { 
    int amountOfChars = 0;
    foreach(char c in board) {
        amountOfChars++;
    }
    return amountOfChars == board.Length; 
}

bool testSetAvailability(char[,] board) {
    //case 1: top-left
    int case1 = 0;
    char[,] arrayForCase1 = setAvailability(board, case1);
    char[,] suppotArrayForCase1 = {{'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'0'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase1[i, 0] != suppotArrayForCase1[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 2: top-middle
    int case2 = 1;
    char[,] arrayForCase2 = setAvailability(board, case2);
    char[,] suppotArrayForCase2 = {{'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase2[i, 0] != suppotArrayForCase2[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 3: top-right
    int case3 = 2;
    char[,] arrayForCase3 = setAvailability(board, case3);
    char[,] suppotArrayForCase3 = {{'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase3[i, 0] != suppotArrayForCase3[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 4: middle-left
    int case4 = 3;
    char[,] arrayForCase4 = setAvailability(board, case4);
    char[,] suppotArrayForCase4 = {{'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase4[i, 0] != suppotArrayForCase4[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 5: middle-middle
    int case5 = 4;
    char[,] arrayForCase5 = setAvailability(board, case5);
    char[,] suppotArrayForCase5 = {{'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase5[i, 0] != suppotArrayForCase5[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 6: middle-right
    int case6 = 5;
    char[,] arrayForCase6 = setAvailability(board, case6);
    char[,] suppotArrayForCase6 = {{'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase6[i, 0] != suppotArrayForCase6[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 7: bottom-left
    int case7 = 6;
    char[,] arrayForCase7 = setAvailability(board, case7);
    char[,] suppotArrayForCase7 = {{'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase7[i, 0] != suppotArrayForCase7[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 8: bottom-middle
    int case8 = 7;
    char[,] arrayForCase8 = setAvailability(board, case8);
    char[,] suppotArrayForCase8 = {{'0'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}, {'1'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase8[i, 0] != suppotArrayForCase8[i, 0]) {
            return false;
        }
    }
    board = resetAvailability(board);
    //case 9: bottom-right
    int case9 = 8;
    char[,] arrayForCase9 = setAvailability(board, case9);
    char[,] suppotArrayForCase9 = {{'0'}, {'0'}, {'0'}, {'0'}, {'0'}, {'1'}, {'0'}, {'1'}, {'0'}};
    for(int i = 0; i < board.GetLength(0);  i++) {
        if(arrayForCase9[i, 0] != suppotArrayForCase9[i, 0]) {
            return false;
        }
    }
    return true;
}

bool testSetEmptyPlace() {
    int emptyPlaceIndex = 2;
    char[,] randomBoard = {{'0', '3'}, {'1', '5'}, {'0', ' '}, 
                           {'0', '4'}, {'0', '6'}, {'1', '2'},
                           {'0', '8'}, {'0', '1'}, {'0', '7'}};
    return setEmptyPlace(randomBoard, emptyPlaceIndex, '5') == 1 && setEmptyPlace(randomBoard, emptyPlaceIndex, '2') == 5 
            && setEmptyPlace(randomBoard, emptyPlaceIndex, '3') == 2;
}

bool testGameControl(string controlSequence) {
    char[,] expectedBoard = {{'0', '1'}, {'0', '2'}, {'0', '3'}, 
                             {'0', '4'}, {'0', '5'}, {'0', '6'},
                             {'0', '7'}, {'0', '8'}, {'0', ' '}};
    char[,] randomBoard = {{'0', '1'}, {'0', '2'}, {'0', '3'}, 
                           {'0', '4'}, {'0', '5'}, {'0', '6'},
                           {'0', '7'}, {'0', ' '}, {'0', '8'}};

    return gameControl(expectedBoard, controlSequence) && !gameControl(randomBoard, controlSequence);
    
}

//support methods

void showBoardAvailability(char[,] board) { 
    for(int i = 0; i < board.GetLength(0); i++) { 
        Console.Write(board[i, 0]);
        if((i + 1) % 3 == 0) {
            Console.WriteLine();
        }
    }
}
