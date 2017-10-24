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
        private List<int> GuessedTurns { get; set; }
        private int Turn { get; set; }

        /***************CONSTRUCTOR***************/
        public GameBoard(int[,] board)
        {
            initBoard(board);
            initGroups();
            initNodes();

            this.Moves = new List<Move>();

            this.Turn = 0;
            this.GuessedTurns = new List<int>();
        } //End 

        /***************METHODS***************/
        
        public bool backtrackingSearch()
        {
            //Declare variables
            bool returnVal = false;
            Point nodeChosen = new Point(-1, -1);
            Node node = null;

            if (!isGoalState())
            {
                nodeChosen = chooseVariable();
                
                if (isValidPosition(nodeChosen))
                {
                    node = this.Board[nodeChosen.X, nodeChosen.Y];

                    if (setState(node))
                    {
                        this.Turn += 1;
                        returnVal = true;
                    } //End if (setState(node))
                    else
                    { //Need to backtrack

                        while (!this.Moves[0].ValueWasGuess || (this.Moves[0].ValueWasGuess && this.Moves[0].PossibleValues.Count <= 0))
                        {
                            if (!undoForwardCheck(this.Moves[0]))
                            {
                                ///Should theoretically never reach this point, assuming everything else leading 
                                ///up to this point is correct (not necessarily the case, probably not the case).
                                ///Tzeentch!! Why do you do these things?!
                            } //End if (!undoForwardCheck(this.Moves[0]))

                            this.Moves.RemoveAt(0);
                            this.Turn -= 1;
                        } //End 

                        if (this.Moves[0].PossibleValues.Count > 0)
                        {
                            this.Moves[0].ValuesTried.Add(this.Moves[0].ValuePlaced);
                            //this.Moves[0].reCalcPossibleValues();
                            if (setStateWithVal(this.Moves[0], this.Moves[0].Node))
                            {
                                this.Moves[0].setValuePlaced(this.Moves[0].PossibleValues.Min());
                                this.Moves[0].reCalcPossibleValues();
                                this.Turn += 1;
                                returnVal = true;
                            } //End 
                            else
                            {
                                ///If this happens I don't even know what's going on anymore. Please Archaon, take 
                                ///us all away from this nonesense.
                            } //End else
                            
                        } //End 
                        else
                        {

                        } //End else
                        
                    } //End else
                    
                } //End 
            } //End if (!isGoalState())
            
            return returnVal;
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

        private bool setState(Node chosenVar)
        {
            //Declare variables
            bool returnVal = false;
            int valBeingPlaced = 0;
            Move move = null;

            if (chosenVar.Domain.Count == 0)
            {
                ///BAD CHOICES WERE MADE!
                ///Need to backtrack. 
            } //End if (chosenVar.Domain.Count == 0)
            else if (chosenVar.Domain.Count == 1)
            {
                valBeingPlaced = chosenVar.Domain[0];

                if (!chosenVar.setValue(chosenVar.Domain[0]))
                {
                    ///Wat? How'd this happen? Shouldn't be possible. In theory, assuming everything else
                    ///is working properly, it shouldn't be possible to choose a variable that already has 
                    ///a value placed in it. That's the only thing that would cause this function to return 
                    ///false.
                } //End if (!chosenVar.setValue(chosenVar.Domain[0]))
                else
                {
                    move = new Move(this.Turn, chosenVar, valBeingPlaced, false);
                    this.Moves.Insert(0, move);

                    this.Rows[chosenVar.RowID].PlacedVals.Add(valBeingPlaced);
                    this.Columns[chosenVar.ColID].PlacedVals.Add(valBeingPlaced);
                    this.Squares[chosenVar.SquareID].PlacedVals.Add(valBeingPlaced);

                    forwardCheck(chosenVar);

                    returnVal = true;
                } //End else
            } //End 
            else if (chosenVar.Domain.Count > 1)
            {
                valBeingPlaced = chosenVar.Domain.Min();

                //Pick the smallest value available in the nodes domain to try.
                if (!chosenVar.setValue(chosenVar.Domain.Min()))
                {
                    ///Same as the comment about. Should never reach this point.
                } //End if (!chosenVar.setValue(chosenVar.Domain.Min()))
                else
                {
                    move = new Move(this.Turn, chosenVar, valBeingPlaced, true);
                    this.Moves.Insert(0, move);
                    this.GuessedTurns.Add(this.Turn);

                    this.Rows[chosenVar.RowID].PlacedVals.Add(valBeingPlaced);
                    this.Columns[chosenVar.ColID].PlacedVals.Add(valBeingPlaced);
                    this.Squares[chosenVar.SquareID].PlacedVals.Add(valBeingPlaced);

                    forwardCheck(chosenVar);

                    returnVal = true;
                } //End else
            } //End else if (chosenVar.Domain.Count > 1)

            return returnVal;
        } //End private bool setState(Node chosenVar)

        private bool setStateWithVal(Move prevMove, Node chosenVar)
        {
            //Declare variables
            bool returnVal = false;
            Move move = null;

            if (prevMove.PossibleValues.Count > 1)
            {
                if (!chosenVar.setValue(prevMove.PossibleValues.Min()))
                {
                    ///Wat? How'd this happen? Shouldn't be possible. In theory, assuming everything else
                    ///is working properly, it shouldn't be possible to choose a variable that already has 
                    ///a value placed in it. That's the only thing that would cause this function to return 
                    ///false.
                } //End if (!chosenVar.setValue(chosenVar.Domain[0]))
                else
                {
                    //move = new Move(this.Turn, chosenVar, prevMove.PossibleValues.Min(), true);
                    //this.Moves.Insert(0, move);

                    this.Rows[chosenVar.RowID].PlacedVals.Add(prevMove.PossibleValues.Min());
                    this.Columns[chosenVar.ColID].PlacedVals.Add(prevMove.PossibleValues.Min());
                    this.Squares[chosenVar.SquareID].PlacedVals.Add(prevMove.PossibleValues.Min());

                    forwardCheck(chosenVar);

                    returnVal = true;
                } //End else
            } //End if (prevMove.PossibleValues.Count > 1)
            else if (prevMove.PossibleValues.Count == 1)
            {
                if (!chosenVar.setValue(prevMove.PossibleValues.Min()))
                {
                    ///Wat? How'd this happen? Shouldn't be possible. In theory, assuming everything else
                    ///is working properly, it shouldn't be possible to choose a variable that already has 
                    ///a value placed in it. That's the only thing that would cause this function to return 
                    ///false.
                } //End if (!chosenVar.setValue(chosenVar.Domain[0]))
                else
                {
                    //move = new Move(this.Turn, chosenVar, prevMove.PossibleValues.Min(), false);
                    //this.Moves.Insert(0, move);
                    //this.GuessedTurns.Add(this.Turn);

                    this.Rows[chosenVar.RowID].PlacedVals.Add(prevMove.PossibleValues.Min());
                    this.Columns[chosenVar.ColID].PlacedVals.Add(prevMove.PossibleValues.Min());
                    this.Squares[chosenVar.SquareID].PlacedVals.Add(prevMove.PossibleValues.Min());

                    forwardCheck(chosenVar);

                    returnVal = true;
                } //End else
            } //End else if (prevMove.PossibleValues.Count == 1)
            else
            {
                ///I swear Tzeentch must really have something against me if you reach this 
                ///point.
            } //End 

            return returnVal;
        } //End private bool setState(Node chosenVar)

        private bool forwardCheck(Node node)
        {
            //Declare variables
            bool returnVal = true;

            this.Rows[node.RowID].reDetermineDomain();
            this.Columns[node.ColID].reDetermineDomain();
            this.Squares[node.SquareID].reDetermineDomain();

            for (int i = 0; i < this.Rows[node.RowID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Rows[node.RowID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Rows[node.RowID].OpenNodeLocations[i]))
                //this.Board[this.Rows[node.RowID].OpenNodeLocations[i].X, this.Rows[node.RowID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Rows[node.RowID].OpenNodeLocations.Count && returnVal; i++)

            for (int i = 0; i < this.Columns[node.ColID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Columns[node.ColID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Columns[node.ColID].OpenNodeLocations[i]))
                //this.Board[this.Columns[node.ColID].OpenNodeLocations[i].X, this.Columns[node.ColID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Columns[node.ColID].OpenNodeLocations.Count && returnVal; i++)

            for (int i = 0; i < this.Squares[node.SquareID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Squares[node.SquareID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Squares[node.SquareID].OpenNodeLocations[i]))
                //this.Board[this.Squares[node.SquareID].OpenNodeLocations[i].X, this.Squares[node.SquareID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Squares[node.SquareID].OpenNodeLocations.Count && returnVal; i++)

            return returnVal;
        } //End private void forwardCheck(Node node)

        private bool undoForwardCheck(Move move)
        {
            //Declare variables
            bool returnVal = true;

            move.Node.undo();

            this.Rows[move.Node.RowID].PlacedVals.Remove(move.ValuePlaced);
            this.Columns[move.Node.ColID].PlacedVals.Remove(move.ValuePlaced);
            this.Squares[move.Node.SquareID].PlacedVals.Remove(move.ValuePlaced);

            this.Rows[move.Node.RowID].reDetermineDomain();
            this.Columns[move.Node.ColID].reDetermineDomain();
            this.Squares[move.Node.SquareID].reDetermineDomain();

            for (int i = 0; i < this.Rows[move.Node.RowID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Rows[move.Node.RowID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Rows[node.RowID].OpenNodeLocations[i]))
                //this.Board[this.Rows[node.RowID].OpenNodeLocations[i].X, this.Rows[node.RowID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Rows[node.RowID].OpenNodeLocations.Count && returnVal; i++)

            for (int i = 0; i < this.Columns[move.Node.ColID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Columns[move.Node.ColID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Columns[node.ColID].OpenNodeLocations[i]))
                //this.Board[this.Columns[node.ColID].OpenNodeLocations[i].X, this.Columns[node.ColID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Columns[node.ColID].OpenNodeLocations.Count && returnVal; i++)

            for (int i = 0; i < this.Squares[move.Node.SquareID].OpenNodeLocations.Count && returnVal; i++)
            {
                if (!determineNodeDomain(this.Squares[move.Node.SquareID].OpenNodeLocations[i]))
                {
                    returnVal = false;
                    break;
                } //End if (!determineNodeDomain(this.Squares[node.SquareID].OpenNodeLocations[i]))
                //this.Board[this.Squares[node.SquareID].OpenNodeLocations[i].X, this.Squares[node.SquareID].OpenNodeLocations[i].Y].reDetermineDomain();
            } //End for (int i = 0; i < this.Squares[node.SquareID].OpenNodeLocations.Count && returnVal; i++)

            return returnVal;
        } //End 

        private bool determineNodeDomain(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            Node node = this.Board[pos.X, pos.Y];

            node.Constraints.Clear();
            node.Domain.Clear();

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
                if (this.Rows[i].Domain.Count > 0)
                {
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
                } //End 

                //Colums
                if (this.Columns[i].Domain.Count > 0)
                {
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
                } //End 

                //Squares
                if (this.Squares[i].Domain.Count > 0)
                {
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
                } //End 
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

        private bool isCompleteRow(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            int rowID = -1;

            if (isValidPosition(pos))
            {
                rowID = this.Board[pos.X, pos.Y].RowID;

                if (rowID >= 0 && rowID <= 8)
                {
                    if (this.Rows[rowID].Domain.Count == 0 && this.Rows[rowID].OpenNodeLocations.Count == 0 && this.Rows[rowID].PlacedVals.Count == 9)
                    {
                        for (int i = 0; i < this.Rows[rowID].PlacedVals.Count; i++)
                        {
                            if (!this.Rows[rowID].PlacedVals.Exists(x => x == i + 1))
                            {
                                break;
                            } //End if (!this.Rows[rowID].PlacedVals.Exists(x => x == i + 1))
                        } //End for (int i = 0; i < this.Rows[rowID].PlacedVals.Count; i++)

                        returnVal = true;
                    } //End if (this.Rows[rowID].Domain.Count == 0 && this.Rows[rowID].OpenNodeLocations.Count == 0 && this.Rows[rowID].PlacedVals.Count == 9)
                } //End if (rowID >= 0 && rowID <= 8)
            } //End if (isValidPosition(pos))

            return returnVal;
        } //End private bool isValidRow(Point pos)

        private bool isCompleteColumn(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            int colID = -1;

            if (isValidPosition(pos))
            {
                colID = this.Board[pos.X, pos.Y].ColID;

                if (colID >= 0 && colID <= 8)
                {
                    if (this.Columns[colID].Domain.Count == 0 && this.Columns[colID].OpenNodeLocations.Count == 0 && this.Columns[colID].PlacedVals.Count == 9)
                    {
                        for (int i = 0; i < this.Columns[colID].PlacedVals.Count; i++)
                        {
                            if (!this.Columns[colID].PlacedVals.Exists(x => x == i + 1))
                            {
                                break;
                            } //End if (!this.Columns[colID].PlacedVals.Exists(x => x == i + 1))
                        } //End for (int i = 0; i < this.Columns[colID].PlacedVals.Count; i++)

                        returnVal = true;
                    } //End if (this.Columns[colID].Domain.Count == 0 && this.Columns[colID].OpenNodeLocations.Count == 0 && this.Columns[colID].PlacedVals.Count == 9)
                } //End if (colID >= 0 && colID <= 8)
            } //End if (isValidPosition(pos))

            return returnVal;
        } //End private bool isValidColumn(Point pos)

        private bool isCompleteSquare(Point pos)
        {
            //Declare variables
            bool returnVal = false;
            int sqrID = -1;

            if (isValidPosition(pos))
            {
                sqrID = this.Board[pos.X, pos.Y].SquareID;

                if (sqrID >= 0 && sqrID <= 8)
                {
                    if (this.Squares[sqrID].Domain.Count == 0 && this.Squares[sqrID].OpenNodeLocations.Count == 0 && this.Squares[sqrID].PlacedVals.Count == 9)
                    {
                        for (int i = 0; i < this.Squares[sqrID].PlacedVals.Count; i++)
                        {
                            if (!this.Squares[sqrID].PlacedVals.Exists(x => x == i + 1))
                            {
                                break;
                            } //End if (!this.Squares[sqrID].PlacedVals.Exists(x => x == i + 1))
                        } //End for (int i = 0; i < this.Squares[sqrID].PlacedVals.Count; i++)

                        returnVal = true;
                    } //End if (this.Squares[sqrID].Domain.Count == 0 && this.Squares[sqrID].OpenNodeLocations.Count == 0 && this.Squares[sqrID].PlacedVals.Count == 9)
                } //End if (colID >= 0 && colID <= 8)
            } //End if (isValidPosition(pos))

            return returnVal;
        } //End private bool isValidSquare(Point pos)

        public bool isGoalState()
        {
            //Declare variables
            bool returnVal = true;
            bool quit = false;

            for (int j = 0; j < this.Board.GetLength(1) && !quit; j++)
            {
                for (int i = 0; i < this.Board.GetLength(0) && !quit; i++)
                {
                    if (this.Board[i,j].Value == 0)
                    {
                        returnVal = false;
                        quit = true;
                        break;
                    } //End if (this.Board[i, j].Value == 0)

                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        if (!isCompleteSquare(new Point(i,j)))
                        {
                            returnVal = false;
                            quit = true;
                            break;
                        } //End if (!isCompleteSquare(new Point(i,j)))
                    } //End if (i % 3 == 0 && j % 3 == 0)

                    if (j < 1)
                    {
                        if (!isCompleteRow(new Point(i, j)) || isCompleteColumn(new Point(i, j)))
                        {
                            returnVal = false;
                            quit = true;
                            break;
                        } //End if (!isCompleteRow(new Point(i, j)) || isCompleteColumn(new Point(i, j)))
                    } //End if (j < 1)
                } //End for (int i = 0; i < 5; i++)
            } //End for (int j = 0; j < 5; j ++)
            
            return returnVal;
        } //End public bool isGoalState()
        #endregion Setup

    } //End class Gameboard
} //End namespace CS4750HW6
