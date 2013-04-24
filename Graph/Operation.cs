using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    class Operation
    {
        public static int num = 3;

        public string name;
        public List<Operation> prev;
        public List<string> tmpprev;
        public int vstart = 0;
        public int vend = 0;
        public int time;
        public int group = 0;
        public int period = 0;
        public int X = 0;
        public int Y = 0;
        public int Xe = 0;
        public int Ye = 0;
        public bool end = true;
        public bool start = false;
        public int pop = 9999;
        public bool warn = false;
        public List<string> chan;
        public bool drawed = false;
        public bool virt = false;
        public bool crit = false;
        public int probtime = 0;
        public int probcost = 0;
        public int temptime = 0;

        public Operation(string n, List<string> p, string t)
        {
            prev = new List<Operation>();
            tmpprev = new List<string>();
            chan = new List<string>();
            name = n;
            tmpprev = p;
            time = Convert.ToInt32(t);
        }        
    }
}
