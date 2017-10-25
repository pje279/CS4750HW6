﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4750HW6
{
    class Move
    {
        //Fields

        //Properties
        public int Turn { get; private set; }
        public Node Node { get; private set; }
        public int ValuePlaced { get; private set; }
        public bool ValueWasGuess { get; private set; }
        public List<int> PossibleValues { get; private set; }
        public List<int> ValuesTried { get; private set; }

        /***************CONSTRUCTOR***************/
        public Move(int turn, Node node, int value, bool valWasGuessed)
        {
            this.Turn = turn;
            this.Node = node;
            this.ValuePlaced = value;
            this.ValueWasGuess = valWasGuessed;
            this.PossibleValues = new List<int>();
            this.ValuesTried = new List<int>();

            calcPossibleValues();
        } //End 

        /***************METHODS***************/

        private void calcPossibleValues()
        {
            //Declare variables

            for (int i = 0; i < this.Node.Domain.Count; i++)
            {
                if (!this.ValuesTried.Exists(x => x == this.Node.Domain[i] || this.Node.Domain[i] != this.ValuePlaced))
                {
                    this.PossibleValues.Add(this.Node.Domain[i]);
                } //End if (!this.ValuesTried.Exists(x => x == this.Node.Domain[i] && this.Node.Domain[i] != this.ValuePlaced))
            } //End for (int i = 0; i < this.Node.Domain.Count; i++)
        } //End private void calcPossibleValues()

        public void reCalcPossibleValues()
        {
            //Declare variables

            this.PossibleValues.Clear();

            for (int i = 0; i < this.Node.Domain.Count; i++)
            {
                if (!this.ValuesTried.Exists(x => x == this.Node.Domain[i] && this.Node.Domain[i] != this.ValuePlaced))
                {
                    this.PossibleValues.Add(this.Node.Domain[i]);
                } //End 
            } //End 
        } //End 

        public bool setValuePlaced(int val)
        {
            //Declare variables
            bool returnVal = false;

            if (val >= 0 && val <= 9 && this.PossibleValues.Exists(x => x == val))
            {
                this.ValuePlaced = val;
                returnVal = true;
            } //End if (val >= 0 && val <= 9 && this.PossibleValues.Exists(x => x == val))

            return returnVal;
        } //End public void setValuePlace(int val)

    } //End class Move
} //End namespace CS4750HW6
