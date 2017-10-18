using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4750HW6
{
    class GameBoard
    {
        /***************ATTRIBUTES***************/
        //Fields

        //Properties
        private Node[,] Board { get; set; }
        private List<Square> Squares { get; set; }
        private List<Row> Rows { get; set; }
        private List<Column> Columns { get; set; }

        /***************CONSTRUCTOR***************/
        public GameBoard(int[,] board)
        {
            initBoard(board);
            initGroups();
            determineNodeDomains();
        } //End 

        /***************METHODS***************/
        private void initBoard(int [,] board)
        {
            //Declare variables

            this.Board = new Node[9,9];

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    this.Board[i, j] = new Node(board[i, j]);
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)
        } //End private void initBoard(int [,] board)

        private void initGroups()
        {
            //Declare variables

            this.Squares = new List<Square>();
            this.Rows = new List<Row>();
            this.Columns = new List<Column>();

            for (int i = 0; i < 9; i++)
            {
                this.Squares.Add(new Square());
                this.Rows.Add(new Row());
                this.Columns.Add(new Column());
            } //End for (int i = 0; i < 9; i++)

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    this.Rows[j].Domain.Add(this.Board[i, j].Value);
                    this.Columns[i].Domain.Add(this.Board[i, j].Value);

                    if (i < 3)
                    {
                        if (j < 3)
                        {
                            this.Squares[0].Domain.Add(this.Board[i, j].Value);
                        } //End if (j < 3)
                        else if (j < 6)
                        {
                            this.Squares[3].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 6)
                        else if (j < 9)
                        {
                            this.Squares[6].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 9)
                    } //End if (i < 3)
                    else if (i < 6)
                    {
                        if (j < 3)
                        {
                            this.Squares[1].Domain.Add(this.Board[i, j].Value);
                        } //End if (j < 3)
                        else if (j < 6)
                        {
                            this.Squares[4].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 6)
                        else if (j < 9)
                        {
                            this.Squares[7].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 9)
                    } //End else if (i < 6)
                    else if (i < 9)
                    {
                        if (j < 3)
                        {
                            this.Squares[2].Domain.Add(this.Board[i, j].Value);
                        } //End if (j < 3)
                        else if (j < 6)
                        {
                            this.Squares[5].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 6)
                        else if (j < 9)
                        {
                            this.Squares[8].Domain.Add(this.Board[i, j].Value);
                        } //End else if (j < 9)
                    } //End else if (i < 9)
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)
        } //End private void initGroups()

        public string displayBoard()
        {
            //Declare variables
            string returnString = "";

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                returnString += "||  ";
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    returnString += this.Board[i, j].ToString();

                    if (i % 3 == 2 && i != 0)
                    {
                        returnString += "  ||  ";
                        //returnString += " | ";
                    } //End if (i % 3 == 2 && i != 0)
                    else if (i < this.Board.GetLength(0) - 1)
                    {
                        //returnString += " | ";
                        returnString += " ";
                    } //End else if (i < this.Board.GetLength(0) - 1)
                } //End for (int i = 0; i < 5; i++)

                //returnString += " ||";

                if (j % 3 == 2 && j != 0)
                {
                    returnString += "\n------------------------------------------\n";
                } //End if (j % 3 == 2 && j != 0)
                else if (j < this.Board.GetLength(1) - 1)
                {
                    returnString += "\n";
                } //End else if (j < this.Board.GetLength(1) - 1)
            } //End for (int j = 0; j < 5; j ++)

            return returnString;
        } //End public string displayBoard()

        private void determineNodeDomains()
        {

        } //End 

    } //End class Gameboard
} //End namespace CS4750HW6
