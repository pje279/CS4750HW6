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
        private List<Move> Moves { get; set; }

        /***************CONSTRUCTOR***************/
        public GameBoard(int[,] board)
        {
            initBoard(board);
            initGroups();
            initNodes();

            var x = chooseVariable();
        } //End 

        /***************METHODS***************/
        
        public void backtrackingSearch()
        {

        } //End 

        private Point chooseVariable()
        {
            //Declare variables
            Group group = null;
            Point returnPos = new Point(-1, -1);
            List<Group> possibleGroups = null;
            List<Point> possibleNodes = null;

            possibleGroups = mrvGroups();

            if (possibleGroups.Count > 0)
            {
                if (possibleGroups.Count > 1)
                {
                    possibleGroups = degreeHeuristicGroup(possibleGroups);
                } //End if (possibleGroups.Count > 1)

                if (possibleGroups.Count > 1)
                {
                    Random rand = new Random();
                    group = possibleGroups[rand.Next(0, possibleGroups.Count - 1)];
                } //End if (possibleGroups.Count > 1)
                else
                {
                    group = possibleGroups[0];
                } //End else

                possibleNodes = mrvNode(group);

                if (possibleNodes.Count > 0)
                {
                    if (possibleNodes.Count > 1)
                    {
                        possibleNodes = degreeHeuristicNode(possibleNodes);
                    } //End if (possibleNodes.Count > 1)

                    if (possibleNodes.Count > 1)
                    {
                        Random rand = new Random();
                        returnPos = possibleNodes[rand.Next(0, possibleNodes.Count - 1)];
                    } //End if (possibleNodes.Count > 1)
                    else
                    {
                        returnPos = possibleNodes[0];
                    } //End else
                } //End if (possibleNodes.Count > 0)
            } //End if (possibleGroups.Count > 0)

            return returnPos;
        } //End private Point chooseVariable()

        private bool setState(Point var)
        {
            //Declare variables
            bool returnVal = false;
            Node chosenVar = null;

            if (isValidPosition(var))
            {
                chosenVar = this.Board[var.X, var.Y];
            } //End if (isValidPosition(var))

            if (chosenVar.Domain.Count == 0)
            {
                ///BAD CHOICES WERE MADE!
                ///Need to backtrack. 
            } //End 
            else if (chosenVar.Domain.Count == 1)
            {
                if (!chosenVar.setValue(chosenVar.Domain[0]))
                {
                    ///Wat? How'd this happen? Shouldn't be possible. In theory, assuming everything else
                    ///is working properly, it shouldn't be possible to choose a variable that already has 
                    ///a value placed in it. That's the only thing that would cause this function to return 
                    ///false.
                } //End if (!chosenVar.setValue(chosenVar.Domain[0]))
                else
                {
                    returnVal = true;
                } //End else
            } //End 
            else if (chosenVar.Domain.Count > 1)
            {
                //Pick the smallest value available in the nodes domain to try.
                if (!chosenVar.setValue(chosenVar.Domain.Min()))
                {
                    ///Same as the comment about. Should never reach this point.
                } //End if (!chosenVar.setValue(chosenVar.Domain.Min()))
            } //End 

            return returnVal;
        } //End 

        private bool determineNodeDomain(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            Node node = this.Board[pos.X, pos.Y];

            //Determine Row constraints
            //for (int i = 0; i < this.Rows[node.RowID].Domain.Count; i++)
            for (int i = 0; i < this.Rows[node.RowID].PlacedVals.Count; i++)
            {
                if (!node.Constraints.Exists(x => x == this.Rows[node.RowID].PlacedVals[i]))
                {
                    node.Constraints.Add(this.Rows[node.RowID].PlacedVals[i]);
                } //End if (node.Constraints.Exists(x => x != this.Rows[node.RowID].Domain[i]))
            } //End for (int i = 0; i < this.Rows[node.RowID].Domain.Count; i++)

            //Determine Column constraints
            for (int i = 0; i < this.Columns[node.ColID].PlacedVals.Count; i++)
            {
                if (!node.Constraints.Exists(x => x == this.Columns[node.ColID].PlacedVals[i]))
                {
                    node.Constraints.Add(this.Columns[node.ColID].PlacedVals[i]);
                } //End if (!node.Constraints.Exists(x => x == this.Columns[node.ColID].Domain[i]))
            } //End for (int i = 0; i < this.Columns[node.ColID].Domain.Count; i++)

            //Determine Square constraints
            for (int i = 0; i < this.Squares[node.SquareID].PlacedVals.Count; i++)
            {
                if (!node.Constraints.Exists(x => x == this.Squares[node.SquareID].PlacedVals[i]))
                {
                    node.Constraints.Add(this.Squares[node.SquareID].PlacedVals[i]);
                } //End if (!node.Constraints.Exists(x => x == this.Squares[node.SquareID].Domain[i]))
            } //End for (int i = 0; i < this.Squares[node.SquareID].Domain.Count; i++)

            //Add the valid values for the node based off of the row, column, and square constraints
            returnVal = node.determineDomain();

            return returnVal;
        } //End private void determineNodeDomain(Point pos)

        #region Heuristics
        private List<Point> mrvNode(Group group)
        {
            //Declare variables
            int MRV = int.MaxValue;
            List<Point> possibleChoices = new List<Point>();

            for (int i = 0; i < group.OpenNodeLocations.Count; i++)
            {
                if (this.Board[group.OpenNodeLocations[i].X, group.OpenNodeLocations[i].Y].Domain.Count < MRV)
                {
                    MRV = this.Board[group.OpenNodeLocations[i].X, group.OpenNodeLocations[i].Y].Domain.Count;
                    possibleChoices.Clear();
                    possibleChoices.Add(group.OpenNodeLocations[i]);
                } //End if (this.Board[i,j].Domain.Count < MRV)
                else if (this.Board[group.OpenNodeLocations[i].X, group.OpenNodeLocations[i].Y].Domain.Count == MRV)
                {
                    possibleChoices.Add(group.OpenNodeLocations[i]);
                } //End else if (this.Board[i, j].Domain.Count == MRV)
            } //End 

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

        private List<Point> degreeHeuristicNode(List<Point> possibleChoices)
        {
            //Declare variables
            int degree = int.MaxValue; //Want a smaller number. Indicates there are fewer valid arrangements of the values in the Nodes domain
            int tempDegree = 0;
            List<Point> newPossibleChoices = new List<Point>();

            for (int i = 0; i < possibleChoices.Count; i++)
            {
                tempDegree = this.Board[possibleChoices[i].X, possibleChoices[i].Y].Domain.Count;

                /*
                for (int j = 0; j < this.Board[possibleChoices[i].X, possibleChoices[i].Y].Domain.Count; j++)
                {
                    if (this.Board[possibleChoices[i].X, possibleChoices[i].Y].Domain.Exists(x => x == possibleChoices[i].Domain[j]))
                    {
                        tempDegree += 1;
                    } //End if (this.Board[p.X, p.Y].Domain.Exists(x => x == j + 1))
                } //End for (int j = 0; j < 9; j++)
                //*/

                if (tempDegree < degree)
                {
                    newPossibleChoices.Clear();
                    newPossibleChoices.Add(possibleChoices[i]);
                    degree = tempDegree;
                } //End if (tempDegree < degree)
                else if (tempDegree == degree)
                {
                    newPossibleChoices.Add(possibleChoices[i]);
                } //End else if (tempDegree == degree)
            } //Endfor (int i = 0; i < possibleChoices.Count; i++)

            return newPossibleChoices;
        } //End 

        private List<Group> degreeHeuristicGroup(List<Group> possibleChoices)
        {
            //Declare variables
            int degree = int.MaxValue; //Want a smaller number. Indicates there are fewer valid arrangements of the values in the groups domain
            int tempDegree = 0;
            List<Group> newPossibleChoices = new List<Group>();

            for (int i = 0; i < possibleChoices.Count; i++)
            {
                tempDegree = 0;

                for (int j = 0; j < possibleChoices[i].Domain.Count; j++)
                {
                    foreach (Point p in possibleChoices[i].OpenNodeLocations)
                    {
                        if (this.Board[p.X, p.Y].Domain.Exists(x => x == possibleChoices[i].Domain[j]))
                        {
                            tempDegree += 1;
                        } //End if (this.Board[p.X, p.Y].Domain.Exists(x => x == j + 1))
                    } //End foreach (Point p in possibleChoices[i].NodeLocations)
                } //End for (int j = 0; j < 9; j++)

                if (tempDegree < degree)
                {
                    newPossibleChoices.Clear();
                    newPossibleChoices.Add(possibleChoices[i]);
                    degree = tempDegree;
                } //End if (tempDegree < degree)
                else if (tempDegree == degree)
                {
                    newPossibleChoices.Add(possibleChoices[i]);
                } //End else if (tempDegree == degree)
            } //Endfor (int i = 0; i < possibleChoices.Count; i++)

            return newPossibleChoices;
        } //End 
        #endregion Heuristics

        #region Setup & Utility
        private void initBoard(int[,] board)
        {
            //Declare variables

            this.Board = new Node[9, 9];

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    this.Board[i, j] = new Node(board[i, j], new Point(i, j));
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
                this.Squares.Add(new Square(i));
                this.Rows.Add(new Row(i));
                this.Columns.Add(new Column(i));
            } //End for (int i = 0; i < 9; i++)

            //Determine what values are already present in each group
            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (this.Board[i, j].Value != 0)
                    {
                        this.Rows[j].PlacedVals.Add(this.Board[i, j].Value);
                        this.Rows[j].Nodes.Add(this.Board[i, j]);
                        this.Columns[i].PlacedVals.Add(this.Board[i, j].Value);
                        this.Columns[i].Nodes.Add(this.Board[i, j]);

                        if (i < 3)
                        {
                            if (j < 3)
                            {
                                this.Squares[0].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[0].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[3].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[3].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[6].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[6].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End if (i < 3)
                        else if (i < 6)
                        {
                            if (j < 3)
                            {
                                this.Squares[1].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[1].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[4].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[4].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[7].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[7].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End else if (i < 6)
                        else if (i < 9)
                        {
                            if (j < 3)
                            {
                                this.Squares[2].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[2].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[5].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[5].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[8].PlacedVals.Add(this.Board[i, j].Value);
                                this.Squares[8].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End else if (i < 9)
                    } //End if (this.Board[i,j].Value != 0)
                    else
                    {
                        this.Rows[j].Nodes.Add(this.Board[i, j]);
                        this.Columns[i].Nodes.Add(this.Board[i, j]);

                        if (i < 3)
                        {
                            if (j < 3)
                            {
                                this.Squares[0].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[3].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[6].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End if (i < 3)
                        else if (i < 6)
                        {
                            if (j < 3)
                            {
                                this.Squares[1].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[4].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[7].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End else if (i < 6)
                        else if (i < 9)
                        {
                            if (j < 3)
                            {
                                this.Squares[2].Nodes.Add(this.Board[i, j]);
                            } //End if (j < 3)
                            else if (j < 6)
                            {
                                this.Squares[5].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 6)
                            else if (j < 9)
                            {
                                this.Squares[8].Nodes.Add(this.Board[i, j]);
                            } //End else if (j < 9)
                        } //End else if (i < 9)
                    } //End else
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)

            //Determine the each groups' respective domain
            for (int i = 0; i < 9; i++)
            {
                this.Rows[i].determineDomain();
                this.Columns[i].determineDomain();
                this.Squares[i].determineDomain();
            } //End for (int i = 0; i < 9; i++)
        } //End private void initGroups()

        private void initNodes()
        {
            //Declare variables

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (this.Board[i, j].Value == 0)
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
                    returnString += this.Board[i, j].Value.ToString();

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

        private bool isValidRow(Point pos)
        {
            //Declare variables
            bool returnVal = false;

            return returnVal;
        } //End private bool isValidRow(Point pos)

        private bool isValidColumn(Point pos)
        {
            //Declare variables
            bool returnVal = false;

            return returnVal;
        } //End private bool isValidColumn(Point pos)

        private bool isValidSquare(Point pos)
        {
            //Declare variables
            bool returnVal = false;

            return returnVal;
        } //End private bool isValidSquare(Point pos)

        private bool isGoalState()
        {
            //Declare variables
            bool returnVal = true;

            for (int j = 0; j < this.Board.GetLength(1); j++)
            {
                for (int i = 0; i < this.Board.GetLength(0); i++)
                {
                    if (this.Board[i,j].Value == 0)
                    {
                        returnVal = false;
                        break;
                    } //End if (this.Board[i, j].Value == 0)
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)

            return returnVal;
        } //End private bool isGoalState()

        private int determinRowColSquare(Point pos)
        {
            //Declare variables
            int returnVal = -1;

            return returnVal;
        } //End 
        #endregion Setup

    } //End class Gameboard
} //End namespace CS4750HW6
