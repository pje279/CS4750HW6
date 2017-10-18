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
            { 0, 0, 4 }, { 0, 0, 6 }, { 1, 5, 0 },
            { 0, 0, 0 }, { 0, 0, 0 }, { 2, 6, 5 },
            { 0, 0, 0 }, { 0, 3, 0 }, { 0, 0, 0 },
            { 0, 6, 0 }, { 0, 0, 0 }, { 0, 0, 0 },
            { 1, 8, 0 }, { 0, 0, 9 }, { 4, 0, 0 },
            { 0, 1, 5 }, { 0, 4, 0 }, { 0, 3, 8 },
            { 8, 1, 0 }, { 0, 0, 0 }, { 0, 0, 9 },
            { 0, 3, 0 }, { 4, 2, 0 }, { 9, 0, 0 },
            { 0, 0, 3 }, { 5, 0, 0 }, { 0, 0, 0 } };
        private int[,] puzzle2 = {
            { 0, 0, 4, 0, 6, 0, 8, 1, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 0, 0 },
            { 1, 5, 0, 0, 0, 0, 0, 0, 9 },
            { 0, 0, 0, 1, 8, 0, 0, 3, 0 },
            { 0, 0, 0, 0, 0, 9, 4, 2, 0 },
            { 2, 6, 5, 4, 0, 0, 9, 0, 0 },
            { 0, 0, 0, 0, 1, 5, 0, 0, 3 },
            { 0, 3, 0, 0, 4, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 3, 8, 0, 0, 0 } };
        private int[,] puzzle3 = {
            { 0, 0, 4, 0, 6, 0, 8, 1, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 0, 0 },
            { 1, 5, 0, 0, 0, 0, 0, 0, 9 },
            { 0, 0, 0, 1, 8, 0, 0, 3, 0 },
            { 0, 0, 0, 0, 0, 9, 4, 2, 0 },
            { 2, 6, 5, 4, 0, 0, 9, 0, 0 },
            { 0, 0, 0, 0, 1, 5, 0, 0, 3 },
            { 0, 3, 0, 0, 4, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 3, 8, 0, 0, 0 } };

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

        /***************CONSTRUCTOR***************/
        public Form1()
        {
            InitializeComponent();

            GameBoard board = new GameBoard(this.Puzzle1);
            this.displayData(board.displayBoard());
        } //End public Form1()

        /***************METHODS***************/
        public void displayData(string data)
        {
            //this.rtxtDisplay.Text += data + "\n\n" + "*****************************************************************\n";
            this.rtxtDisplay.Text = data;
        } //End public void displayData(string data)

        public void displayDataAppend(string data)
        {
            this.rtxtDisplay.Text += "\n\n" + data;
        } //End public void displayData(string data)

        public void displayMillisecondsElapsed()
        {
            timer.Stop();
            this.displayDataAppend("Time elapsed: " + this.timer.ElapsedMilliseconds.ToString() + " milliseconds");
        } //End public void displayMillisecondsElapsed()

        private void reset()
        {
            
        } //End private void reset()

        /***************EVENTS***************/
        private void btnReset_Click(object sender, EventArgs e)
        {

        } //End private void btnReset_Click(object sender, EventArgs e)

        private void btnPuzzle1_Click(object sender, EventArgs e)
        {

        } //End private void btnPuzzle1_Click(object sender, EventArgs e)

        private void btnPuzzle2_Click(object sender, EventArgs e)
        {

        } //End private void btnPuzzle2_Click(object sender, EventArgs e)

        private void btnPuzzle3_Click(object sender, EventArgs e)
        {

        } //End private void btnPuzzle3_Click(object sender, EventArgs e)
    } //End public partial class Form1 : Form
} //End namespace CS4750HW6
