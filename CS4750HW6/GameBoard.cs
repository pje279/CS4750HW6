using System;
using System.Collections.Generic;
using System.Drawing;
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
            initNodes();
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
                    this.Board[i, j] = new Node(board[i, j], new Point (i, j));
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
                    if (this.Board[i,j].Value != 0)
                    {
                        this.Rows[j].PlacedVals.Add(this.Board[i, j].Value);
                        this.Columns[i].PlacedVals.Add(this.Board[i, j].Value);

                        if (i < 3)
                        {
                            if (j < 3)
                            {
                                this.Squares[0].PlacedVals.Add(this.Board[i, j].Value);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[3].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[6].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 9)
                        } //End if (i < 3)
                        else if (i < 6)
                        {
                            if (j < 3)
                            {
                                this.Squares[1].PlacedVals.Add(this.Board[i, j].Value);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[4].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[7].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 9)
                        } //End else if (i < 6)
                        else if (i < 9)
                        {
                            if (j < 3)
                            {
                                this.Squares[2].PlacedVals.Add(this.Board[i, j].Value);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[5].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[8].PlacedVals.Add(this.Board[i, j].Value);
                            } //End else if (j < 9)
                        } //End else if (i < 9)
                    } //End if (this.Board[i,j].Value != 0)
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)
        } //End private void initGroups()

        private void initNodes()
        {
            //Declare variables

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (this.Board[i,j].Value == 0)
                    {
                        if (!determineNodeDomain(new Point(i, j)))
                        {
                            ///By Sigmar, how did this happen? The initial state off the board
                            ///should be solveable. Should only ever reach this point it the 
                            ///board is not solveable from the initial state. i.e., there is 
                            ///a node with an empty domain. Was probably Tzeentch's fault.
                        } //End 
                    } //End 
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)
        } //End 

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

        private bool determineNodeDomain(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            Node node = this.Board[pos.X, pos.Y];

            //Determine Row constraints
            for (int i = 0; i < this.Rows[node.RowID].Domain.Count; i++)
            {
                //node.RowConstraint.Add(this.Rows[node.RowID].Domain[i]);
                if (!node.Constraints.Exists(x => x == this.Rows[node.RowID].Domain[i]))
                {
                    node.Constraints.Add(this.Rows[node.RowID].Domain[i]);
                } //End if (node.Constraints.Exists(x => x != this.Rows[node.RowID].Domain[i]))
            } //End for (int i = 0; i < this.Rows[node.RowID].Domain.Count; i++)

            //Determine Column constraints
            for (int i = 0; i < this.Columns[node.ColID].Domain.Count; i++)
            {
                //node.ColConstraint.Add(this.Columns[node.ColID].Domain[i]);
                if (!node.Constraints.Exists(x => x == this.Columns[node.ColID].Domain[i]))
                {
                    node.Constraints.Add(this.Columns[node.ColID].Domain[i]);
                } //End if (!node.Constraints.Exists(x => x == this.Columns[node.ColID].Domain[i]))
            } //End for (int i = 0; i < this.Columns[node.ColID].Domain.Count; i++)

            //Determine Square constraints
            for (int i = 0; i < this.Squares[node.SquareID].Domain.Count; i++)
            {
                //node.SquareConstraint.Add(this.Squares[node.SquareID].Domain[i]);
                if (!node.Constraints.Exists(x => x == this.Squares[node.SquareID].Domain[i]))
                {
                    node.Constraints.Add(this.Squares[node.SquareID].Domain[i]);
                } //End if (!node.Constraints.Exists(x => x == this.Squares[node.SquareID].Domain[i]))
            } //End for (int i = 0; i < this.Squares[node.SquareID].Domain.Count; i++)

            //Add the valid values for the node based off of the row, column, and square constraints
            returnVal = node.determineDomain();

            return returnVal;
        } //End private void determineNodeDomain(Point pos)

        private Point chooseVariable()
        {
            //Declare variables
            Point returnPos = new Point(-1, -1);
            List<Group> possibleChoices = null;

            possibleChoices = mrvGroups();

            if (possibleChoices.Count > 0)
            {
                if (possibleChoices.Count == 1)
                {

                } //End 
                else
                {

                } //End else
            } //End 

            return returnPos;
        } //End 

        private List<Point> mrvNode()
        {
            //Declare variables
            int MRV = int.MaxValue;
            List<Point> possibleChoices = new List<Point>();

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (this.Board[i,j].Domain.Count < MRV)
                    {
                        MRV = this.Board[i, j].Domain.Count;
                        possibleChoices.Clear();
                        possibleChoices.Add(new Point(i, j));
                    } //End if (this.Board[i,j].Domain.Count < MRV)
                    else if (this.Board[i, j].Domain.Count == MRV)
                    {
                        possibleChoices.Add(new Point(i, j));
                    } //End else if (this.Board[i, j].Domain.Count == MRV)
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)

            return possibleChoices;
        } //End private List<Point> minRemainingValues()

        private List<Group> mrvGroups()
        {
            //Declare variables
            int MRV = int.MaxValue;
            List<Row> possibleRows = new List<Row>();
            List<Column> possibleCols = new List<Column>();
            List<Square> possibleSquares = new List<Square>();
            List<Group> possibleChoices = new List<Group>();

            for (int i = 0; i < 9; i++)
            {
                //Rows
                if (this.Rows[i].Domain.Count < MRV)
                {
                    possibleChoices.Clear();
                    possibleChoices.Add(this.Rows[i]);
                    MRV = this.Rows[i].Domain.Count;
                } //End if (this.Rows[i].Domain.Count < MRV)
                else if (this.Rows[i].Domain.Count == MRV)
                {
                    possibleChoices.Add(this.Rows[i]);
                } //End else if (this.Rows[i].Domain.Count == MRV)

                //Colums
                if (this.Columns[i].Domain.Count < MRV)
                {
                    possibleChoices.Clear();
                    possibleChoices.Add(this.Columns[i]);
                    MRV = this.Columns[i].Domain.Count;
                } //End if (this.Columns[i].Domain.Count < MRV)
                else if (this.Columns[i].Domain.Count == MRV)
                {
                    possibleChoices.Add(this.Columns[i]);
                } //End else if (this.Columns[i].Domain.Count == MRV)

                //Squares
                if (this.Squares[i].Domain.Count < MRV)
                {
                    possibleChoices.Clear();
                    possibleChoices.Add(this.Squares[i]);
                    MRV = this.Squares[i].Domain.Count;
                } //End if (this.Squares[i].Domain.Count < MRV)
                else if (this.Squares[i].Domain.Count == MRV)
                {
                    possibleChoices.Add(this.Squares[i]);
                } //End else if (this.Squares[i].Domain.Count == MRV)
            } //End for (int i = 0; i < 9; i++)

            return possibleChoices;
        } //End private List<Point> minRemainingValues()

        private void degreeHeuristicNode(List<Point> possibleChoices)
        {
            //Declare variables


            for (int i = 0; i < possibleChoices.Count; i++)
            {

            } //Endfor (int i = 0; i < possibleChoices.Count; i++)
        } //End 

        private List<int> degreeHeuristicGroup(List<Group> possibleChoices)
        {
            //Declare variables
            int degree = int.MaxValue; //Want a smaller number. Indicates there are fewer valid arrangements of the values in the groups domain
            int tempDegree = 0;
            List<int> newPossibleChoices = new List<int>();

            for (int i = 0; i < possibleChoices.Count; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    foreach (Point p in possibleChoices[i].NodeLocations)
                    {
                        tempDegree = 0;

                        if (this.Board[p.X, p.Y].Domain.Exists(x => x == i + 1))
                        {
                            tempDegree += 1;
                        } //End if (this.Board[p.X, p.Y].Domain.Exists(x => x == i + 1))
                    } //End foreach (Point p in possibleChoices[i].NodeLocations)
                } //End for (int j = 0; j < 9; j++)

                if (tempDegree < degree)
                {
                    newPossibleChoices.Clear();
                    newPossibleChoices.Add(i);
                    degree = tempDegree;
                } //End if (tempDegree < degree)
                else if (tempDegree == degree)
                {
                    newPossibleChoices.Add(i);
                } //End else if (tempDegree == degree)
            } //Endfor (int i = 0; i < possibleChoices.Count; i++)

            return newPossibleChoices;
        } //End 

        private bool isValidPosition(Point pos)
        {
            //Declare variables
            bool returnVal = false;

            if (pos.X >= 0 && pos.Y >= 0 && pos.X < 9 && pos.Y < 9)
            {
                returnVal = true;
            } //End if (pos.X >= 0 && pos.Y >= 0 && pos.X < 9 && pos.Y < 9)

            return returnVal;
        } //End 
    } //End class Gameboard
} //End namespace CS4750HW6
