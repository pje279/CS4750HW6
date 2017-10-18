using System;
using System.Collections.Generic;
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
        public int Value { get; private set; }

        /***************CONSTRUCTOR***************/
        public Node(int val)
        {
            this.Value = val;
        } //End 

        /***************METHODS***************/

    } //End class Node
} //End namespace CS4750HW6
