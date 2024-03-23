// See https://aka.ms/new-console-template for more information

class Program
{
    static char[,] board = new char[3, 3];

    static void Mains(string[] args)
    {
        InitializeBoard();
        bool isGameOver = false;
        bool isPlayerTurn = true; // Assuming player starts the game.

        while (!isGameOver)
        {
            DisplayBoard();
            if (isPlayerTurn)
            {
                PlayerMove();
            }
            else
            {
                BotMove(); // Uncomment the one you want to use
                // BotRandomMove();
            }

            isPlayerTurn = !isPlayerTurn;
            isGameOver = CheckGameOver();
        }

        Console.WriteLine("Game Over!");
    }

    static void InitializeBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = ' ';
            }
        }
    }

    static void DisplayBoard()
    {
        Console.WriteLine("\n  0   1   2");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < 3; j++)
            {
                Console.Write(board[i, j]);
                if (j < 2) Console.Write(" | ");
                else Console.WriteLine();
            }

            if (i < 2) Console.WriteLine(" ---------");
        }

        Console.WriteLine();
    }

    // Placeholder for PlayerMove, BotMove, BotRandomMove, CheckGameOver
    static void PlayerMove()
    {
        int row, col;
        do
        {
            Console.WriteLine("Enter row and column numbers (0-2) to make your move:");
            row = Convert.ToInt32(Console.ReadLine());
            col = Convert.ToInt32(Console.ReadLine());
        } while (row < 0 || row > 2 || col < 0 || col > 2 || board[row, col] != ' ');

        board[row, col] = 'X'; // Assuming player is 'X'
    }

    static Random rand = new Random();

    static void BotRandomMove()
    {
        int row, col;
        do
        {
            row = rand.Next(3);
            col = rand.Next(3);
        } while (board[row, col] != ' ');

        board[row, col] = 'O'; // Assuming bot is 'O'
        Console.WriteLine($"Bot (Random) moves: {row}, {col}");
    }

    // Check if there are moves left on the board
    static bool IsMovesLeft()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == ' ')
                {
                    return true;
                }
            }
        }

        return false;
    }

// Evaluate the board for a win
    static int Evaluate()
    {
        // Checking for Rows for X or O victory.
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'O')
                    return +10;
                else if (board[row, 0] == 'X')
                    return -10;
            }
        }

        // Checking for Columns for X or O victory.
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == 'O')
                    return +10;
                else if (board[0, col] == 'X')
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory.
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'O')
                return +10;
            else if (board[0, 0] == 'X')
                return -10;
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'O')
                return +10;
            else if (board[0, 2] == 'X')
                return -10;
        }

        // Else if none of them have won then return 0
        return 0;
    }

    static int Minimax(bool isMax, int depth = 0)
    {
        // Evaluate the current board state and return its score.
        int score = Evaluate();

        // Base cases: If the current board state is a win, a loss, or a draw,
        // return the score.
        if (score == 10 || score == -10 || !IsMovesLeft())
            return score;

        // Prepare to track the best score: Maximizers want the highest score,
        // Minimizers want the lowest.
        int best = isMax ? -1000 : 1000;
        
        // Counter to track how many moves are considered at this depth.
        int possibilities = 0;

        // Loop through all cells of the board.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if the cell is empty and a move can be made here.
                if (board[i, j] == ' ')
                {
                    // Make the move on the board for the current player.
                    board[i, j] = isMax ? 'O' : 'X';

                    // Recursively call Minimax to evaluate this move, switching the player
                    // (isMax) and increasing the depth.
                    int moveVal = Minimax(!isMax, depth + 1);

                    // Undo the move to backtrack and try other possibilities.
                    board[i, j] = ' ';

                    // Increment the possibilities counter.
                    possibilities++;

                    // If we're maximizing, see if this move is better than the best found so far.
                    if (isMax)
                    {
                        if (moveVal > best)
                        {
                            best = moveVal;
                            // At the top level, log the choice and score among the considered possibilities.
                            if (depth == 0)
                                Console.WriteLine(
                                    $"Maximizer chooses [{i}, {j}] with score {best} among {possibilities} possibilities.");
                        }
                    }
                    // If we're minimizing, see if this move is worse than the worst found so far,
                    // which is actually better for the minimizer.
                    else
                    {
                        if (moveVal < best)
                        {
                            best = moveVal;
                            // At the top level, log the choice and score among the considered possibilities.
                            if (depth == 0)
                                Console.WriteLine(
                                    $"Minimizer chooses [{i}, {j}] with score {best} among {possibilities} possibilities.");
                        }
                    }
                }
            }
        }

        // After evaluating all possibilities at this depth (or at the top level),
        // summarize the decision-making process.
        if (depth == 0)
        {
            Console.WriteLine(
                $"{(isMax ? "Maximizer" : "Minimizer")} evaluated {possibilities} moves at depth {depth}.");
            Console.WriteLine($"{(isMax ? "Maximizer" : "Minimizer")} final choice led to a score of {best}.");
        }

        // Return the best score found for this board configuration.
        return best;
    }

    static bool CheckGameOver()
    {
        // Checking if there is a winner
        if (Evaluate() == 10 || Evaluate() == -10)
        {
            DisplayBoard();
            Console.WriteLine("Winner: " + (Evaluate() == 10 ? "Bot" : "Player"));
            return true;
        }

        // Checking for draw
        if (!IsMovesLeft())
        {
            DisplayBoard();
            Console.WriteLine("It's a draw!");
            return true;
        }

        return false;
    }

    static void BotMove()
    {
        int bestVal = -1000;
        int bestRow = -1;
        int bestCol = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if cell is empty
                if (board[i, j] == ' ')
                {
                    // Make the move
                    board[i, j] = 'O';

                    // Compute evaluation function for this move.
                    int moveVal = Minimax(false);

                    // Undo the move
                    board[i, j] = ' ';

                    // If the value of the current move is more than the best value, then update best
                    if (moveVal > bestVal)
                    {
                        bestRow = i;
                        bestCol = j;
                        bestVal = moveVal;
                    }
                }
            }
        }

        board[bestRow, bestCol] = 'O'; // Perform the best move
        Console.WriteLine($"Bot (Minimax) moves: {bestRow}, {bestCol}");
    }
}