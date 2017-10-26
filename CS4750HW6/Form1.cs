using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS4750HW6
{
    public partial class Form1 : Form
    {
        /***************ATTRIBUTES***************/
        //Fields
        private Stopwatch timer;
        private int[,] puzzle1 = {
            { 0, 0, 4, 0, 6, 0, 8, 1, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 0, 0 },
            { 1, 5, 0, 0, 0, 0, 0, 0, 9 },
            { 0, 0, 0, 1, 8, 0, 0, 3, 0 },
            { 0, 0, 0, 0, 0, 9, 4, 2, 0 },
            { 2, 6, 5, 4, 0, 0, 9, 0, 0 },
            { 0, 0, 0, 0, 1, 5, 0, 0, 3 },
            { 0, 3, 0, 0, 4, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 3, 8, 0, 0, 0 } };
        private int[,] puzzle = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
        private int[,] puzzle2 = {
            { 0, 0, 1, 2, 0, 5, 0, 0, 0 },
            { 0, 0, 0, 0, 4, 0, 0, 6, 0 },
            { 5, 2, 9, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 6, 1, 0 },
            { 1, 0, 0, 3, 0, 0, 0, 0, 7 },
            { 0, 4, 0, 0, 0, 7, 3, 0, 0 },
            { 0, 0, 2, 0, 7, 0, 0, 0, 0 },
            { 0, 3, 0, 0, 0, 0, 0, 0, 5 },
            { 0, 0, 6, 0, 0, 1, 0, 0, 0 } };
        private int[,] puzzle3 = {
            { 6, 0, 0, 3, 0, 0, 0, 0, 1 },
            { 7, 2, 9, 0, 0, 0, 0, 0, 0 },
            { 0, 5, 0, 0, 0, 0, 8, 0, 6 },
            { 0, 0, 5, 0, 0, 4, 6, 0, 0 },
            { 0, 0, 6, 8, 0, 7, 0, 0, 5 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 2, 9, 8, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 9, 1, 7 },
            { 0, 0, 0, 0, 1, 0, 0, 0, 0 } };

        //Properties
        int[,] Puzzle1
        {
            get
            {
                return puzzle1;
            } //End get
        } //End int[,] Puzzle1
        int[,] Puzzle2
        {
            get
            {
                return puzzle2;
            } //End get
        } //End int[,] Puzzle2
        int[,] Puzzle3
        {
            get
            {
                return puzzle3;
            } //End get
        } //End int[,] Puzzle3
        private bool Puzzle1Started { get; set; }
        private bool Puzzle2Started { get; set; }
        private bool Puzzle3Started { get; set; }
        private GameBoard Board1 { get; set; }
        private GameBoard Board2 { get; set; }
        private GameBoard Board3 { get; set; }

        /***************CONSTRUCTOR***************/
        public Form1()
        {
            InitializeComponent();

            this.Puzzle1Started = false;
            this.Puzzle2Started = false;
            this.Puzzle3Started = false;

            this.Board1 = new GameBoard(this.Puzzle1);
            this.Board2 = new GameBoard(this.Puzzle2);
            this.Board3 = new GameBoard(this.Puzzle3);
        } //End public Form1()

        /***************METHODS***************/
        public void displayData(string data)
        {
            //this.rtxtDisplay.Text += data + "\n\n" + "*****************************************************************\n";
            this.rtxtDisplay.Text = data;
        } //End public void displayData(string data)

        public void displayDataAppend(string data)
        {
            this.rtxtDisplay.Text += "\n" + data;
        } //End public void displayData(string data)

        public void displayMillisecondsElapsed()
        {
            timer.Stop();
            this.displayDataAppend("Time elapsed: " + this.timer.ElapsedMilliseconds.ToString() + " milliseconds");
        } //End public void displayMillisecondsElapsed()

        private void reset()
        {
            this.Puzzle1Started = false;
            this.Puzzle2Started = false;
            this.Puzzle3Started = false;
            this.rtxtDisplay.Text = "";
        } //End private void reset()

        /***************EVENTS***************/
        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        } //End private void btnReset_Click(object sender, EventArgs e)

        private void btnPuzzle1_Click(object sender, EventArgs e)
        {
            //Declare variables
            bool finishedSuccessfully = true;

            if (!this.Puzzle1Started || this.Board1.isGoalState())
            {
                this.Board1 = new GameBoard(this.Puzzle1);
                this.Puzzle1Started = true;
            } //End if (!this.Puzzle1Started || this.Board1.isGoalState())

            this.timer = Stopwatch.StartNew();

            if (this.chkSingleStep.Checked)
            {
                this.Board1.backtrackingSearch();
            } //End 
            else
            {
                while (!this.Board1.isGoalState())
                {
                    if (this.timer.ElapsedMilliseconds >= 300000)
                    {
                        finishedSuccessfully = false;
                        break;
                    } //End if (this.timer.ElapsedMilliseconds >= 300000)

                    //displayData(this.Board1.displayBoard());
                    this.Board1.backtrackingSearch();
                } //End while (!this.Board1.isGoalState())
            } //End else

            this.timer.Stop();

            displayData(this.Board1.displayBoard());

            if (finishedSuccessfully)
            {
                if (this.chkSingleStep.Checked)
                {
                    if (this.Board1.isGoalState())
                    {
                        displayDataAppend("Found a solution!");
                    } //End if (this.Board1.isGoalState())
                } //End if (this.chkSingleStep.Checked)
                else
                {
                    displayDataAppend("Found a solution!");
                } //End else
            } //End if (finishedSuccessfully)
            else
            {
                displayDataAppend("No solution found in time");
            } //End else

            displayDataAppend("Time elapsed: " + this.timer.ElapsedMilliseconds.ToString() + " milliseconds");
        } //End private void btnPuzzle1_Click(object sender, EventArgs e)

        private void btnPuzzle2_Click(object sender, EventArgs e)
        {
            //Declare variables
            bool finishedSuccessfully = true;

            if (!this.Puzzle2Started || this.Board2.isGoalState())
            {
                this.Board2 = new GameBoard(this.Puzzle2);
                this.Puzzle2Started = true;
            } //End if (!this.Puzzle1Started || this.Board2.isGoalState())

            this.timer = Stopwatch.StartNew();

            if (this.chkSingleStep.Checked)
            {
                this.Board2.backtrackingSearch();
            } //End if (this.chkSingleStep.Checked)
            else
            {
                while (!this.Board2.isGoalState())
                {
                    if (this.timer.ElapsedMilliseconds >= 300000)
                    {
                        finishedSuccessfully = false;
                        break;
                    } //End if (this.timer.ElapsedMilliseconds >= 300000)

                    //displayData(this.Board2.displayBoard());
                    this.Board2.backtrackingSearch();
                } //End while (!this.Board2.isGoalState())
            } //End else

            this.timer.Stop();

            displayData(this.Board2.displayBoard());

            if (finishedSuccessfully)
            {
                if (this.chkSingleStep.Checked)
                {
                    if (this.Board2.isGoalState())
                    {
                        displayDataAppend("Found a solution!");
                    } //End if (this.Board2.isGoalState())
                } //End if (this.chkSingleStep.Checked)
                else
                {
                    displayDataAppend("Found a solution!");
                } //End else
            } //End if (finishedSuccessfully)
            else
            {
                displayDataAppend("No solution found in time");
            } //End else

            displayDataAppend("Time elapsed: " + this.timer.ElapsedMilliseconds.ToString() + " milliseconds");
        } //End private void btnPuzzle2_Click(object sender, EventArgs e)

        private void btnPuzzle3_Click(object sender, EventArgs e)
        {
            //Declare variables
            bool finishedSuccessfully = true;

            this.timer = Stopwatch.StartNew();
            if (!this.Puzzle3Started || this.Board3.isGoalState())
            {
                this.Board3 = new GameBoard(this.Puzzle3);
                this.Puzzle3Started = true;
            } //End if (!this.Puzzle1Started || this.Board3.isGoalState())

            this.timer = Stopwatch.StartNew();

            if (this.chkSingleStep.Checked)
            {
                this.Board3.backtrackingSearch();
            } //End if (this.chkSingleStep.Checked)
            else
            {
                while (!this.Board3.isGoalState())
                {
                    if (this.timer.ElapsedMilliseconds >= 300000)
                    {
                        finishedSuccessfully = false;
                        break;
                    } //End if (this.timer.ElapsedMilliseconds >= 300000)

                    //displayData(this.Board3.displayBoard());
                    this.Board3.backtrackingSearch();
                } //End while (!this.Board3.isGoalState())
            } //End else

            this.timer.Stop();

            displayData(this.Board3.displayBoard());

            if (finishedSuccessfully)
            {
                if (this.chkSingleStep.Checked)
                {
                    if (this.Board3.isGoalState())
                    {
                        displayDataAppend("Found a solution!");
                    } //End if (this.Board3.isGoalState())
                } //End if (this.chkSingleStep.Checked)
                else
                {
                    displayDataAppend("Found a solution!");
                } //End else
            } //End if (finishedSuccessfully)
            else
            {
                displayDataAppend("No solution found in time");
            } //End else

            displayDataAppend("Time elapsed: " + this.timer.ElapsedMilliseconds.ToString() + " milliseconds");
        } //End private void btnPuzzle3_Click(object sender, EventArgs e)
    } //End public partial class Form1 : Form
} //End namespace CS4750HW6
