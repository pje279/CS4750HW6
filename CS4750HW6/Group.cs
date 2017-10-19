using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4750HW6
{
    class Group
    {
        /***************ATTRIBUTES***************/
        //Fields

        //Properties
        public List<int> Domain { get; private set; }
        public List<int> PlacedVals { get; private set; }
        public List<Point> NodeLocations { get; private set; }

        /***************CONSTRUCTOR***************/
        public Group()
        {
            this.Domain = new List<int>();
            this.PlacedVals = new List<int>();
            this.NodeLocations = new List<Point>();
        } //End 

        /***************METHODS***************/
        public bool determineDomain()
        {
            //Declare variables
            bool returnVal = false;

            for (int i = 0; i < 9; i++)
            {
                if (!this.PlacedVals.Exists(x => x == i + 1))
                {
                    this.Domain.Add(i + 1);
                } //End if (!this.PlacedVals.Exists(x => x == i + 1))
            } //End for (int i = 0; i < 9; i++)

            if (this.Domain.Count > 0)
            {
                returnVal = true;
            } //End if (this.Domain.Count > 0)

            return returnVal;
        } //End public bool determineDomain()

        public bool isValidLocation(Point location)
        {
            //Declare variables
            bool returnVal = false;

            for (int i = 0; i < this.NodeLocations.Count; i++)
            {
                if (this.NodeLocations[i] == location)
                {
                    returnVal = true;
                    break;
                } //End if (this.NodeLocations[i] == location)
            } //End for (int i = 0; i < this.NodeLocations.Count; i++)

            return returnVal;
        } //End 

        public bool isValidValue(int value)
        {
            //Declare variables
            bool returnVal = false;

            for (int i = 0; i < this.Domain.Count; i++)
            {
                if (this.Domain[i] == value)
                {
                    returnVal = true;
                    break;
                } //End if (this.Domain[i] == value)
            } //End for (int i = 0; i < this.Domain.Count; i++)

            return returnVal;
        } //End 

    } //End class Group
} //End namespace CS4750HW6
