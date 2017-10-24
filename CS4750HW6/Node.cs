using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4750HW6
{
    class Node
    {
        /***************ATTRIBUTES***************/
        //Fields

        //Properties
        public List<int> Domain { get; private set; }
        public List<int> Constraints { get; private set; }
        public int RowID { get; private set; }
        public int ColID { get; private set; }
        public int SquareID { get; private set; }
        public Point Position { get; private set; }
        public int Value { get; private set; }

        /***************CONSTRUCTOR***************/
        public Node(int val, Point position)
        {
            this.Domain = new List<int>();
            this.Constraints = new List<int>();
            this.Position = position;
            this.Value = val;
            determineGroups();
        } //End 

        /***************METHODS***************/

        public bool setValue(int val)
        {
            //Declare variables
            bool returnVal = false;

            if (val >= 0 && val <= 9)
            {
                this.Value = val;
                returnVal = true;
            } //End if (val >= 0 && val <= 9)

            this.Domain.Remove(val);

            return returnVal;
        } //End public bool setValue(int val)

        public void undo()
        {
            //Declare variables

            this.Domain.Add(this.Value);
            this.Value = 0;
        } //End public void undo()

        private void determineGroups()
        {
            this.RowID = this.Position.Y;
            this.ColID = this.Position.X;

            if (this.Position.X < 3)
            {
                if (this.Position.Y < 3)
                {
                    this.SquareID = 0;
                } //End if (this.Position.Y < 3)
                else if (this.Position.Y < 6)
                {
                    this.SquareID = 3;
                } //End else if (this.Position.Y < 6)
                else if (this.Position.Y < 9)
                {
                    this.SquareID = 6;
                } //End else if (this.Position.Y < 9)
            } //End if (this.Position.X < 3)
            else if (this.Position.X < 6)
            {
                if (this.Position.Y < 3)
                {
                    this.SquareID = 1;
                } //End if (this.Position.Y < 3)
                else if (this.Position.Y < 6)
                {
                    this.SquareID = 4;
                } //End else if (this.Position.Y < 6)
                else if (this.Position.Y < 9)
                {
                    this.SquareID = 7;
                } //End else if (this.Position.Y < 9)
            } //End else if (this.Position.X < 6)
            else if (this.Position.X < 9)
            {
                if (this.Position.Y < 3)
                {
                    this.SquareID = 2;
                } //End if (this.Position.Y < 3)
                else if (this.Position.Y < 6)
                {
                    this.SquareID = 5;
                } //End else if (this.Position.Y < 6)
                else if (this.Position.Y < 9)
                {
                    this.SquareID = 8;
                } //End else if (this.Position.Y < 9)
            } //End else if (this.Position.X < 9)
        } //End private void determineGroups()

        public bool determineDomain()
        {
            //Declare variables
            bool returnVal = false;

            for (int i = 0; i < 9; i++)
            {
                if (!this.Constraints.Exists(x => x == i + 1))
                {
                    this.Domain.Add(i + 1);
                } //End if (!this.Constraints.Exists(x => x == i + 1))
            } //End for (int i = 0; i < 9; i++)

            if (this.Domain.Count > 0)
            {
                returnVal = true;
            } //End if (this.Domain.Count > 0)

            return returnVal;
        } //End public bool determineDomain()

        public void reDetermineDomain()
        {
            //Declare variables

            this.Domain.Clear();

            for (int i = 0; i < 9; i++)
            {
                if (!this.Constraints.Exists(x => x == i + 1))
                {
                    this.Domain.Add(i + 1);
                } //End if (!this.Constraints.Exists(x => x == i + 1))
            } //End for (int i = 0; i < 9; i++)
        } //End public bool determineDomain()

        private bool isValidPosition(Point pos)
        {
            //Declare variables
            bool returnVal = false;

            if (pos.X >= 0 && pos.Y >= 0 && pos.X < 9 && pos.Y < 9)
            {
                returnVal = true;
            } //End if (pos.X >= 0 && pos.Y >= 0 && pos.X < 9 && pos.Y < 9)

            return returnVal;
        } //End private bool isValidPosition(Point pos)
    } //End class Node
} //End namespace CS4750HW6
