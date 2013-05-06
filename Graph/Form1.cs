using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Operation> list = new List<Operation>();

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            foreach (DataGridViewRow Row in dataGridView.Rows)
            {
                if (Row.Cells[1].Value != null)
                {
                    List<string> l = new List<string>();
                    int tmp = Row.Cells[1].Value.ToString().Split(' ').GetLength(0);
                    for (int i = 0; i < tmp; i++)
                    {
                        l.Add(Row.Cells[1].Value.ToString().Split(' ')[i]);
                    }
                    list.Add(new Operation(Row.Cells[0].Value.ToString(), l, Row.Cells[2].Value.ToString()));
                }
            }
            bool ufl;
            int xx = 1;
            foreach (Operation op in list)
            {
                foreach (string str in op.tmpprev)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op2.name == str)
                        {
                            foreach (string str2 in op2.tmpprev)
                            {
                                foreach (string str3 in op.tmpprev)
                                {
                                    if (str2 == str3)
                                    {
                                        op.chan.Add(str2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Operation op in list)
            {
                foreach (string str in op.chan)
                {
                    op.tmpprev.Remove(str);
                }
                op.chan.Clear();
            }
            do
            {
                ufl = false;
                foreach (Operation op in list)
                {
                    op.prev.Clear();
                    foreach (String str in op.tmpprev)
                    {
                        foreach (Operation op2 in list)
                        {
                            if (str == op2.name)
                                op.prev.Add(op2);
                        }
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.start = true;
                    }
                }
                List<Operation> lend = new List<Operation>();
                List<string> lsend = new List<string>();
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str != "-")
                            lsend.Add(str);
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in lsend)
                    {
                        if (op.name == str)
                            op.end = false; ;
                    }
                }
                foreach (Operation op in list)
                {
                    if (op.end)
                        lend.Add(op);
                }

                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.group = 1;
                    }
                }
                bool flag = true;
                while (flag)
                {
                    int tmp = 0;
                    foreach (Operation op in list)
                    {
                        if (op.group > tmp)
                            tmp = op.group;
                    }
                    foreach (Operation op in list)
                    {
                        foreach (Operation op2 in op.prev)
                        {
                            if (op.group <= op2.group)
                            {
                                op.group = op2.group + 1;
                            }
                        }
                    }
                    flag = false;
                    foreach (Operation op in list)
                    {
                        if (op.group == 0)
                            flag = true;
                    }
                }
                int tmp4 = 0;
                foreach (Operation op in list)
                {
                    if (op.group > tmp4)
                        tmp4 = op.group;
                }
                for (int i = 1; i <= tmp4; i++)
                {
                    int j = 1;
                    foreach (Operation op in list)
                    {
                        bool war = false;
                        int tmp = 0;
                        if (op.group == i)
                        {
                            foreach (Operation op2 in list)
                            {
                                if ((op2.group == i) && (op.name != op2.name))
                                {
                                    foreach (String str in op.tmpprev)
                                    {
                                        foreach (String str2 in op2.tmpprev)
                                        {
                                            if (str == str2)
                                            {
                                                if (op2.period != 0)
                                                {
                                                    tmp = op2.period;
                                                    war = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (war)
                            {
                                op.period = tmp;
                            }
                            else
                            {
                                op.period = j;
                                j++;
                            }
                        }
                    }
                }

                int mg = 0;
                foreach (Operation op in list)
                {
                    if (op.group > mg)
                    {
                        mg = op.group;
                    }
                }
                foreach (Operation op in list)
                {
                    int y = 0;
                    op.X = (1000 / (mg + 2)) * op.group;
                    foreach (Operation op2 in list)
                    {
                        if ((op2.period > y) && (op2.period == op.period))
                        {
                            y = op2.period;
                        }
                    }
                    op.Y = 1000 - (1000 / (y + 1)) * op.period;
                }
                foreach (Operation op in list)
                {
                    bool fl = true;
                    foreach (Operation op2 in list)
                    {
                        foreach (String str in op2.tmpprev)
                        {
                            if (str == op.name)
                            {
                                fl = false;
                                op.Xe = op2.X;
                                op.Ye = op2.Y;
                            }
                        }
                    }
                    if (fl)
                    {
                        op.Xe = (1000 / (mg + 2)) * (mg + 1);
                        op.Ye = 1000 - (1000 / (2));
                    }
                }
                List<Operation> tmpl = new List<Operation>();
                foreach (Operation op in list)
                {
                    foreach (Operation op2 in list)
                    {
                        if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op.X == op2.X) && (op.Y == op2.Y) && (op.name != op2.name))
                        {
                            List<string> fuu = new List<string>();
                            fuu.Add(op2.name);
                            Operation nw = new Operation(xx.ToString(), fuu, "0");
                            nw.virt = true;
                            tmpl.Add(nw);
                            ufl = true;
                            foreach (Operation op3 in list)
                            {
                                foreach (Operation op4 in op3.prev)
                                {
                                    if (op4.name == op2.name)
                                    {
                                        int indx = op3.tmpprev.FindIndex(
                                            delegate(String bk)
                                            {
                                                return (bk == op2.name);
                                            }
                                        );
                                        op3.tmpprev[indx] = xx.ToString();
                                    }
                                }
                            }
                            op2.Xe = xx * 9999;
                            op2.Ye = xx * 9999;
                            xx++;
                        }
                    }
                }
                if (ufl)
                {
                    foreach (Operation op in tmpl)
                    {
                        list.Add(op);
                    }
                    tmpl.Clear();
                }
            } while (ufl);
            List<Operation> tmp2 = new List<Operation>();
            int temp = 1;
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op2.prev.Contains(op))
                        op.pop--;
                }
            }

            list.Sort(delegate(Operation a, Operation b)
            {
                int ixdiff = a.group.CompareTo(b.group);
                if (ixdiff != 0)
                    return ixdiff;
                else
                    return a.pop.CompareTo(b.pop);
            });
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op.name != op2.name)
                    {
                        if ((op.X == op2.X) && (op.Y == op2.Y) && (op2.vstart != 0))
                        {
                            op.vstart = op2.vstart;
                        }
                        else
                        {
                            if ((op.X == op2.Xe) && (op.Y == op2.Ye) && (op2.vend != 0))
                            {
                                op.vstart = op2.vend;
                            }
                        }
                    }
                }
                if (op.vstart == 0)
                {
                    op.vstart = temp;
                    temp++;
                }
            }
            foreach (Operation op in list)
            {
                if (!op.warn)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op.name != op2.name)
                        {
                            if ((op.Xe == op2.X) && (op.Ye == op2.Y) && (op2.vstart != 0))
                            {
                                op.vend = op2.vstart;
                            }
                            else
                            {
                                if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op2.vend != 0))
                                {
                                    op.vend = op2.vend;
                                }
                            }
                        }
                    }
                    if (op.vend == 0)
                    {
                        op.vend = temp;
                        temp++;
                    }
                }
            }

            int n;      // Общее количество работ по проекту          
            // (количество ребер ориентированного графа).
            int[] ii = new int[20];  // Вектор-пара, представляющая k-ю работу,    
            int[] jj = new int[20];  // которая понимается как стрелка, связыва-   
            // ющая событие i[k] с событием j[k]          
            // Граф задан массивом ребер:                 
            // (i[0],j[0]),(i[1],j[1]),...,(i[n-1],j[n-1])    
            int k;      // Параметр цикла.
            n = list.Count;
            k = 0;
            list.Sort(delegate(Operation a, Operation b)
            {
                return a.vstart.CompareTo(b.vstart);
            });
            foreach (Operation op in list)
            {
                ii[k] = op.vstart;
                jj[k] = op.vend;
                k++;
            }
            List<string> yeah = new List<string>();
            Critical_Path(n, ii, jj, yeah);
            int fe = yeah.Count;
            List<Operation>[] ye = new List<Operation>[fe];
            for (int j = 0; j < fe; j++)
            {
                ye[j] = new List<Operation>();
            }
            for (int j = 0; j < fe; j++)
            {
                int tmp = yeah[j].Split(' ').GetLength(0);
                string fg = yeah[j].Split(' ')[0];
                for (int i = 1; i < tmp; i++)
                {
                    string now = yeah[j].Split(' ')[i];
                    foreach (Operation op in list)
                    {
                        if ((op.vstart.ToString() == fg) && (op.vend.ToString() == now))
                        {
                            ye[j].Add(op);
                            fg = yeah[j].Split(' ')[i];
                        }
                    }
                }
            }
            List<int> giper = new List<int>();
            int numgiper = 0;
            for (int j = 0; j < fe; j++)
            {
                int tmpgiper = 0;
                foreach (Operation op in ye[j])
                {
                    tmpgiper += op.time;
                }
                if (tmpgiper == numgiper)
                {
                    giper.Add(j);
                }
                if (tmpgiper > numgiper)
                {
                    giper.Clear();
                    giper.Add(j);
                    numgiper = tmpgiper;
                }
            }
            string victory = "";
            foreach (int i in giper)
            {
                foreach (Operation op in ye[i])
                {
                    op.crit = true;
                    double Num;
                    bool isNum = double.TryParse(op.name, out Num);
                    if (!isNum)
                        victory += op.name + " ";
                }
                victory += ";";
            }
            label1.Text = "Критический путь " + victory;
            label2.Text = "Вес критического пути " + numgiper.ToString();
            var fileName = "graph.txt";
            string path = Directory.GetCurrentDirectory();

            StringBuilder sb = new StringBuilder();
            sb.Append("digraph G {" + Environment.NewLine);
            sb.Append("rankdir = LR;" + Environment.NewLine);
            for (int j = 0; j < fe; j++)
            {
                foreach (Operation op in ye[j])
                {
                    if (!op.drawed)
                    {
                        if (!op.crit)
                        {
                            if (!op.virt)
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                        else
                        {
                            if (!op.virt)
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                    }
                }
            }
            sb.Append("}");
            TextWriter tw = new StreamWriter(fileName);
            tw.WriteLine(sb.ToString());
            tw.Close();

            GenerateGraph(fileName, path);

            System.Diagnostics.Process.Start("graph.jpg");
            list.Clear();
        }

        public static void Critical_Path(int n, int[] i, int[] j, List<string> yeah)
        {
            string str2 = "1";
            FactR(n, 1, str2, i, j, yeah);
        }

        static public void FactR(int n, int pn, string str, int[] i, int[] j, List<string> yeah)
        {
            bool fl = true;
            for (int k = 0; k < n; k++)
            {
                if (i[k] == pn)
                {
                    fl = false;
                    string nstr = str + " " + j[k].ToString();
                    FactR(n, j[k], nstr, i, j, yeah);
                }
            }
            if (fl)
            {
                yeah.Add(str);
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
        }

        private static void GenerateGraph(string filename, string path)
        {
            try
            {
                var command = string.Format("dot -Tjpg {0} -o {1}", Path.Combine(path, filename), Path.Combine(path, filename.Replace(".txt", ".jpg")));
                var ProcStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = ProcStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int maxCost = int.Parse(textBox1.Text);
            int planTime = int.Parse(textBox2.Text);
            int profit = int.Parse(textBox3.Text);

            label1.Text = "";
            label2.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            foreach (DataGridViewRow Row in dataGridView.Rows)
            {
                if (Row.Cells[1].Value != null)
                {
                    List<string> l = new List<string>();
                    int tmp = Row.Cells[1].Value.ToString().Split(' ').GetLength(0);
                    for (int i = 0; i < tmp; i++)
                    {
                        l.Add(Row.Cells[1].Value.ToString().Split(' ')[i]);
                    }
                    list.Add(new Operation(Row.Cells[0].Value.ToString(), l, Row.Cells[2].Value.ToString()));
                }
            }
            foreach (DataGridViewRow Row in dataGridView.Rows)
            {
                if (Row.Cells[1].Value != null)
                {
                    foreach (Operation op in list)
                    {
                        if (op.name == Row.Cells[0].Value.ToString())
                        {
                            op.probtime = Convert.ToInt32(Row.Cells[3].Value.ToString());
                            op.probcost = Convert.ToInt32(Row.Cells[4].Value.ToString());
                            if (op.probtime > op.time)
                            {
                                MessageBox.Show("Возможное время сокращения не может превышать время выполнения");
                                return;
                            }
                        }
                    }
                }
            }
            bool ufl;
            int xx = 1;
            foreach (Operation op in list)
            {
                foreach (string str in op.tmpprev)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op2.name == str)
                        {
                            foreach (string str2 in op2.tmpprev)
                            {
                                foreach (string str3 in op.tmpprev)
                                {
                                    if (str2 == str3)
                                    {
                                        op.chan.Add(str2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Operation op in list)
            {
                foreach (string str in op.chan)
                {
                    op.tmpprev.Remove(str);
                }
                op.chan.Clear();
            }
            do
            {
                ufl = false;
                foreach (Operation op in list)
                {
                    op.prev.Clear();
                    foreach (String str in op.tmpprev)
                    {
                        foreach (Operation op2 in list)
                        {
                            if (str == op2.name)
                                op.prev.Add(op2);
                        }
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.start = true;
                    }
                }
                List<Operation> lend = new List<Operation>();
                List<string> lsend = new List<string>();
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str != "-")
                            lsend.Add(str);
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in lsend)
                    {
                        if (op.name == str)
                            op.end = false; ;
                    }
                }
                foreach (Operation op in list)
                {
                    if (op.end)
                        lend.Add(op);
                }

                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.group = 1;
                    }
                }
                bool flag = true;
                while (flag)
                {
                    int tmp = 0;
                    foreach (Operation op in list)
                    {
                        if (op.group > tmp)
                            tmp = op.group;
                    }
                    foreach (Operation op in list)
                    {
                        foreach (Operation op2 in op.prev)
                        {
                            if (op.group <= op2.group)
                            {
                                op.group = op2.group + 1;
                            }
                        }
                    }
                    flag = false;
                    foreach (Operation op in list)
                    {
                        if (op.group == 0)
                            flag = true;
                    }
                }
                int tmp4 = 0;
                foreach (Operation op in list)
                {
                    if (op.group > tmp4)
                        tmp4 = op.group;
                }
                for (int i = 1; i <= tmp4; i++)
                {
                    int j = 1;
                    foreach (Operation op in list)
                    {
                        bool war = false;
                        int tmp = 0;
                        if (op.group == i)
                        {
                            foreach (Operation op2 in list)
                            {
                                if ((op2.group == i) && (op.name != op2.name))
                                {
                                    foreach (String str in op.tmpprev)
                                    {
                                        foreach (String str2 in op2.tmpprev)
                                        {
                                            if (str == str2)
                                            {
                                                if (op2.period != 0)
                                                {
                                                    tmp = op2.period;
                                                    war = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (war)
                            {
                                op.period = tmp;
                            }
                            else
                            {
                                op.period = j;
                                j++;
                            }
                        }
                    }
                }

                int mg = 0;
                foreach (Operation op in list)
                {
                    if (op.group > mg)
                    {
                        mg = op.group;
                    }
                }
                foreach (Operation op in list)
                {
                    int y = 0;
                    op.X = (1000 / (mg + 2)) * op.group;
                    foreach (Operation op2 in list)
                    {
                        if ((op2.period > y) && (op2.period == op.period))
                        {
                            y = op2.period;
                        }
                    }
                    op.Y = 1000 - (1000 / (y + 1)) * op.period;
                }
                foreach (Operation op in list)
                {
                    bool fl = true;
                    foreach (Operation op2 in list)
                    {
                        foreach (String str in op2.tmpprev)
                        {
                            if (str == op.name)
                            {
                                fl = false;
                                op.Xe = op2.X;
                                op.Ye = op2.Y;
                            }
                        }
                    }
                    if (fl)
                    {
                        op.Xe = (1000 / (mg + 2)) * (mg + 1);
                        op.Ye = 1000 - (1000 / (2));
                    }
                }
                List<Operation> tmpl = new List<Operation>();
                foreach (Operation op in list)
                {
                    foreach (Operation op2 in list)
                    {
                        if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op.X == op2.X) && (op.Y == op2.Y) && (op.name != op2.name))
                        {
                            List<string> fuu = new List<string>();
                            fuu.Add(op2.name);
                            Operation nw = new Operation(xx.ToString(), fuu, "0");
                            nw.virt = true;
                            tmpl.Add(nw);
                            ufl = true;
                            foreach (Operation op3 in list)
                            {
                                foreach (Operation op4 in op3.prev)
                                {
                                    if (op4.name == op2.name)
                                    {
                                        int indx = op3.tmpprev.FindIndex(
                                            delegate(String bk)
                                            {
                                                return (bk == op2.name);
                                            }
                                        );
                                        op3.tmpprev[indx] = xx.ToString();
                                    }
                                }
                            }
                            op2.Xe = xx * 9999;
                            op2.Ye = xx * 9999;
                            xx++;
                        }
                    }
                }
                if (ufl)
                {
                    foreach (Operation op in tmpl)
                    {
                        list.Add(op);
                    }
                    tmpl.Clear();
                }
            } while (ufl);
            List<Operation> tmp2 = new List<Operation>();
            int temp = 1;
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op2.prev.Contains(op))
                        op.pop--;
                }
            }

            list.Sort(delegate(Operation a, Operation b)
            {
                int ixdiff = a.group.CompareTo(b.group);
                if (ixdiff != 0)
                    return ixdiff;
                else
                    return a.pop.CompareTo(b.pop);
            });
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op.name != op2.name)
                    {
                        if ((op.X == op2.X) && (op.Y == op2.Y) && (op2.vstart != 0))
                        {
                            op.vstart = op2.vstart;
                        }
                        else
                        {
                            if ((op.X == op2.Xe) && (op.Y == op2.Ye) && (op2.vend != 0))
                            {
                                op.vstart = op2.vend;
                            }
                        }
                    }
                }
                if (op.vstart == 0)
                {
                    op.vstart = temp;
                    temp++;
                }
            }
            foreach (Operation op in list)
            {
                if (!op.warn)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op.name != op2.name)
                        {
                            if ((op.Xe == op2.X) && (op.Ye == op2.Y) && (op2.vstart != 0))
                            {
                                op.vend = op2.vstart;
                            }
                            else
                            {
                                if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op2.vend != 0))
                                {
                                    op.vend = op2.vend;
                                }
                            }
                        }
                    }
                    if (op.vend == 0)
                    {
                        op.vend = temp;
                        temp++;
                    }
                }
            }

            int n;      // Общее количество работ по проекту          
            // (количество ребер ориентированного графа).
            int[] ii = new int[20];  // Вектор-пара, представляющая k-ю работу,    
            int[] jj = new int[20];  // которая понимается как стрелка, связыва-   
            // ющая событие i[k] с событием j[k]          
            // Граф задан массивом ребер:                 
            // (i[0],j[0]),(i[1],j[1]),...,(i[n-1],j[n-1])    
            int k;      // Параметр цикла.
            n = list.Count;
            k = 0;
            list.Sort(delegate(Operation a, Operation b)
            {
                return a.vstart.CompareTo(b.vstart);
            });
            foreach (Operation op in list)
            {
                ii[k] = op.vstart;
                jj[k] = op.vend;
                k++;
            }
            List<string> yeah = new List<string>();
            Critical_Path(n, ii, jj, yeah);
            int fe = yeah.Count;
            List<Operation>[] ye = new List<Operation>[fe];
            for (int j = 0; j < fe; j++)
            {
                ye[j] = new List<Operation>();
            }
            for (int j = 0; j < fe; j++)
            {
                int tmp = yeah[j].Split(' ').GetLength(0);
                string fg = yeah[j].Split(' ')[0];
                for (int i = 1; i < tmp; i++)
                {
                    string now = yeah[j].Split(' ')[i];
                    foreach (Operation op in list)
                    {
                        if ((op.vstart.ToString() == fg) && (op.vend.ToString() == now))
                        {
                            ye[j].Add(op);
                            fg = yeah[j].Split(' ')[i];
                        }
                    }
                }
            }
            /*int numWays = 1;    
            foreach (Operation op in list)
            {
                numWays *= (op.probtime + 1);
            }
            List<List<int>> ld = new List<List<int>>();
            List<int> timed = new List<int>();
            List<int> costd = new List<int>();
            /*List<int>[] ld = new List<int>[numWays];
            for (int i = 0; i < fe; i++)
            {
                ld[i] = new List<int>();
            }*/
            //int[] timed = new int[numWays];
            //int[] costd = new int[numWays];

            bool flag2 = false;
            int cost = 0;
            int fingiper = 0; //Будет показывать изменился ли критический путь проекта
            foreach (Operation op in list)
            {
                op.temptime = op.time; //Для последующего вычисления стоимости
            }
            List<int> ttime = new List<int>(); //Для хранения последнего стабильного состояния
            do
            {
                flag2 = false;
                List<int> giper = new List<int>();
                int numgiper = 0;
                int numPredgiper = 0;
                Operation x = null;
                for (int j = 0; j < fe; j++)
                {
                    int tmpgiper = 0;
                    foreach (Operation op in ye[j])
                    {
                        tmpgiper += op.time;
                    }
                    if (tmpgiper == numgiper)
                    {
                        giper.Add(j);
                    }
                    else
                    {
                        if (tmpgiper > numgiper)
                        {
                            if (numgiper != 0)
                                numPredgiper = numgiper;
                            giper.Clear();
                            giper.Add(j);
                            numgiper = tmpgiper;
                        }
                        else
                        {
                            numPredgiper = tmpgiper;
                        }
                    }                                  
                }
                int min = 999999999;
                foreach (int i in giper)
                {
                    foreach (Operation op in ye[i])
                    {
                        if ((op.probcost < min) && (op.probtime != 0))
                        {
                            min = op.probcost;
                            x = op;
                        }
                    }
                }  
                if (numgiper != fingiper) //Сохраняем состояние при измении крит пути
                {
                    fingiper = numgiper;
                    ttime.Clear();
                    foreach (Operation op in list)
                    {
                        ttime.Add(op.time);
                    }
                }
                int tmp = 0;
                if (giper.Count > 1)
                {
                    tmp = 1;
                }
                else
                {
                    tmp = numgiper - numPredgiper;
                }
                if (numPredgiper == 0)
                {
                    tmp = 1;
                }
                if (x != null)
                {
                    while ((x.probtime != 0) && (tmp != 0) &&
                                (((cost + x.probcost - (planTime - numPredgiper - tmp + 1) * profit <= maxCost) && (planTime >= numPredgiper + tmp - 1))
                                || ((cost + x.probcost <= maxCost) && (planTime < numPredgiper + tmp - 1))))
                    {
                        flag2 = true;
                        tmp--;
                        x.time--;
                        x.probtime--;
                        cost += x.probcost;
                    }
                }
            } while (flag2);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].time = ttime[i];
            }
            cost = 0;
            foreach (Operation op in list)
            {
                cost += (op.temptime - op.time) * op.probcost;
            }
            List<int> giper2 = new List<int>();
            int numgiper2 = 0;
            for (int j = 0; j < fe; j++)
            {
                int tmpgiper = 0;
                foreach (Operation op in ye[j])
                {
                    tmpgiper += op.time;
                }
                if (tmpgiper == numgiper2)
                {
                    giper2.Add(j);
                }
                if (tmpgiper > numgiper2)
                {
                    giper2.Clear();
                    giper2.Add(j);
                    numgiper2 = tmpgiper;
                }
            }
            string victory = "";
            foreach (int i in giper2)
            {
                foreach (Operation op in ye[i])
                {
                    op.crit = true;
                    double Num;
                    bool isNum = double.TryParse(op.name, out Num);
                    if (!isNum)
                        victory += op.name + " ";
                }
                victory += ";";
            }
            string timestr = "";
            foreach (Operation op in list)
            {
                double Num;
                bool isNum = double.TryParse(op.name, out Num);
                if (!isNum)
                    timestr += op.name + " -> " + op.time + " ; ";
            }   
            label4.Text = timestr;
            label5.Text = "Дополнительные затраты равны = " + (cost-(planTime-numgiper2)*profit).ToString();
            label1.Text = "Критический путь " + victory;
            label2.Text = "Вес критического пути " + numgiper2.ToString();
            var fileName = "graph.txt";
            string path = Directory.GetCurrentDirectory();

            StringBuilder sb = new StringBuilder();
            sb.Append("digraph G {" + Environment.NewLine);
            sb.Append("rankdir = LR;" + Environment.NewLine);
            for (int j = 0; j < fe; j++)
            {
                foreach (Operation op in ye[j])
                {
                    if (!op.drawed)
                    {
                        if (!op.crit)
                        {
                            if (!op.virt)
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                        else
                        {
                            if (!op.virt)
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                    }
                }
            }
            sb.Append("}");
            TextWriter tw = new StreamWriter(fileName);
            tw.WriteLine(sb.ToString());
            tw.Close();

            GenerateGraph(fileName, path);

            System.Diagnostics.Process.Start("graph.jpg");
            list.Clear();
        }

        /*public int aa = -1;
        public int vTime = 999999999;

        private void optTime(List<Operation> ye, List<Operation>[] yes, int fe, List<List<int>> tlist, 
            List<int> time, List<int> cost, int maxCost, int planTime, int profit)
        {
            List<int> lya = new List<int>();
            opTimeA(ye,yes,fe, -1, lya,0,0,tlist, time, cost, maxCost, planTime, profit);
        }
 
        private void opTimeA(List<Operation> ye, List<Operation>[] yes,int fe, int next, List<int> tlist, int time,
            int cost, List<List<int>> tlistg, List<int> timeg, List<int> costg, int maxCost, int planTime, int profit)
        {
            for (int i = (ye[next + 1].time - ye[next + 1].probtime); i <= ye[next + 1].time; i++)
            {
                int mcost = cost;
                int mtime = time;
                mcost += (ye[next + 1].time - i) * ye[next + 1].probcost;
                mtime += i;
                List<int> ttlist = new List<int>();
                ttlist.AddRange(tlist);
                ttlist.Add(i);
                if (next + 2 < ye.Count)
                {
                    opTimeA(ye, yes, fe, next + 1, ttlist, mtime, mcost, tlistg, timeg, costg, maxCost, planTime, profit);
                }
                else
                {
                    aa++;
                    int numgiper = 0;
                    for (int s = 0; s < list.Count; s++)
                    {
                        list[s].temptime = ttlist[s];
                    }
                    for (int j = 0; j < fe; j++)
                    {
                        int tmpgiper = 0;
                        foreach (Operation op in yes[j])
                        {
                            tmpgiper += op.temptime;
                        }
                        if (tmpgiper > numgiper)
                        {
                            numgiper = tmpgiper;
                        }
                    }
                    if ((numgiper <= vTime) && (mcost - (planTime - numgiper) * profit <= maxCost) && (numgiper <= planTime))
                        vTime = numgiper;
                    if ((numgiper <= vTime) && (mcost - (planTime - numgiper)*profit <= maxCost) && (numgiper <= planTime))
                    {
                        costg.Add(mcost);
                        timeg.Add(numgiper);
                        tlistg.Add(ttlist);
                    }
                }
            }
        }*/

        private void button3_Click(object sender, EventArgs e)
        {
            int maxCost = int.Parse(textBox1.Text);
            int planTime = int.Parse(textBox2.Text);
            int profit = int.Parse(textBox3.Text);

            label1.Text = "";
            label2.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            foreach (DataGridViewRow Row in dataGridView.Rows)
            {
                if (Row.Cells[1].Value != null)
                {
                    List<string> l = new List<string>();
                    int tmp = Row.Cells[1].Value.ToString().Split(' ').GetLength(0);
                    for (int i = 0; i < tmp; i++)
                    {
                        l.Add(Row.Cells[1].Value.ToString().Split(' ')[i]);
                    }
                    list.Add(new Operation(Row.Cells[0].Value.ToString(), l, Row.Cells[2].Value.ToString()));
                }
            }
            foreach (DataGridViewRow Row in dataGridView.Rows)
            {
                if (Row.Cells[1].Value != null)
                {
                    foreach (Operation op in list)
                    {
                        if (op.name == Row.Cells[0].Value.ToString())
                        {
                            op.probtime = Convert.ToInt32(Row.Cells[3].Value.ToString());
                            op.probcost = Convert.ToInt32(Row.Cells[4].Value.ToString());
                            if (op.probtime > op.time)
                            {
                                MessageBox.Show("Возможное время сокращения не может превышать время выполнения");
                                return;
                            }
                        }
                    }
                }
            }
            bool ufl;
            int xx = 1;
            foreach (Operation op in list)
            {
                foreach (string str in op.tmpprev)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op2.name == str)
                        {
                            foreach (string str2 in op2.tmpprev)
                            {
                                foreach (string str3 in op.tmpprev)
                                {
                                    if (str2 == str3)
                                    {
                                        op.chan.Add(str2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Operation op in list)
            {
                foreach (string str in op.chan)
                {
                    op.tmpprev.Remove(str);
                }
                op.chan.Clear();
            }
            do
            {
                ufl = false;
                foreach (Operation op in list)
                {
                    op.prev.Clear();
                    foreach (String str in op.tmpprev)
                    {
                        foreach (Operation op2 in list)
                        {
                            if (str == op2.name)
                                op.prev.Add(op2);
                        }
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.start = true;
                    }
                }
                List<Operation> lend = new List<Operation>();
                List<string> lsend = new List<string>();
                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str != "-")
                            lsend.Add(str);
                    }
                }
                foreach (Operation op in list)
                {
                    foreach (String str in lsend)
                    {
                        if (op.name == str)
                            op.end = false; ;
                    }
                }
                foreach (Operation op in list)
                {
                    if (op.end)
                        lend.Add(op);
                }

                foreach (Operation op in list)
                {
                    foreach (String str in op.tmpprev)
                    {
                        if (str == "-")
                            op.group = 1;
                    }
                }
                bool flag = true;
                while (flag)
                {
                    int tmp = 0;
                    foreach (Operation op in list)
                    {
                        if (op.group > tmp)
                            tmp = op.group;
                    }
                    foreach (Operation op in list)
                    {
                        foreach (Operation op2 in op.prev)
                        {
                            if (op.group <= op2.group)
                            {
                                op.group = op2.group + 1;
                            }
                        }
                    }
                    flag = false;
                    foreach (Operation op in list)
                    {
                        if (op.group == 0)
                            flag = true;
                    }
                }
                int tmp4 = 0;
                foreach (Operation op in list)
                {
                    if (op.group > tmp4)
                        tmp4 = op.group;
                }
                for (int i = 1; i <= tmp4; i++)
                {
                    int j = 1;
                    foreach (Operation op in list)
                    {
                        bool war = false;
                        int tmp = 0;
                        if (op.group == i)
                        {
                            foreach (Operation op2 in list)
                            {
                                if ((op2.group == i) && (op.name != op2.name))
                                {
                                    foreach (String str in op.tmpprev)
                                    {
                                        foreach (String str2 in op2.tmpprev)
                                        {
                                            if (str == str2)
                                            {
                                                if (op2.period != 0)
                                                {
                                                    tmp = op2.period;
                                                    war = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (war)
                            {
                                op.period = tmp;
                            }
                            else
                            {
                                op.period = j;
                                j++;
                            }
                        }
                    }
                }

                int mg = 0;
                foreach (Operation op in list)
                {
                    if (op.group > mg)
                    {
                        mg = op.group;
                    }
                }
                foreach (Operation op in list)
                {
                    int y = 0;
                    op.X = (1000 / (mg + 2)) * op.group;
                    foreach (Operation op2 in list)
                    {
                        if ((op2.period > y) && (op2.period == op.period))
                        {
                            y = op2.period;
                        }
                    }
                    op.Y = 1000 - (1000 / (y + 1)) * op.period;
                }
                foreach (Operation op in list)
                {
                    bool fl = true;
                    foreach (Operation op2 in list)
                    {
                        foreach (String str in op2.tmpprev)
                        {
                            if (str == op.name)
                            {
                                fl = false;
                                op.Xe = op2.X;
                                op.Ye = op2.Y;
                            }
                        }
                    }
                    if (fl)
                    {
                        op.Xe = (1000 / (mg + 2)) * (mg + 1);
                        op.Ye = 1000 - (1000 / (2));
                    }
                }
                List<Operation> tmpl = new List<Operation>();
                foreach (Operation op in list)
                {
                    foreach (Operation op2 in list)
                    {
                        if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op.X == op2.X) && (op.Y == op2.Y) && (op.name != op2.name))
                        {
                            List<string> fuu = new List<string>();
                            fuu.Add(op2.name);
                            Operation nw = new Operation(xx.ToString(), fuu, "0");
                            nw.virt = true;
                            tmpl.Add(nw);
                            ufl = true;
                            foreach (Operation op3 in list)
                            {
                                foreach (Operation op4 in op3.prev)
                                {
                                    if (op4.name == op2.name)
                                    {
                                        int indx = op3.tmpprev.FindIndex(
                                            delegate(String bk)
                                            {
                                                return (bk == op2.name);
                                            }
                                        );
                                        op3.tmpprev[indx] = xx.ToString();
                                    }
                                }
                            }
                            op2.Xe = xx * 9999;
                            op2.Ye = xx * 9999;
                            xx++;
                        }
                    }
                }
                if (ufl)
                {
                    foreach (Operation op in tmpl)
                    {
                        list.Add(op);
                    }
                    tmpl.Clear();
                }
            } while (ufl);
            List<Operation> tmp2 = new List<Operation>();
            int temp = 1;
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op2.prev.Contains(op))
                        op.pop--;
                }
            }

            list.Sort(delegate(Operation a, Operation b)
            {
                int ixdiff = a.group.CompareTo(b.group);
                if (ixdiff != 0)
                    return ixdiff;
                else
                    return a.pop.CompareTo(b.pop);
            });
            foreach (Operation op in list)
            {
                foreach (Operation op2 in list)
                {
                    if (op.name != op2.name)
                    {
                        if ((op.X == op2.X) && (op.Y == op2.Y) && (op2.vstart != 0))
                        {
                            op.vstart = op2.vstart;
                        }
                        else
                        {
                            if ((op.X == op2.Xe) && (op.Y == op2.Ye) && (op2.vend != 0))
                            {
                                op.vstart = op2.vend;
                            }
                        }
                    }
                }
                if (op.vstart == 0)
                {
                    op.vstart = temp;
                    temp++;
                }
            }
            foreach (Operation op in list)
            {
                if (!op.warn)
                {
                    foreach (Operation op2 in list)
                    {
                        if (op.name != op2.name)
                        {
                            if ((op.Xe == op2.X) && (op.Ye == op2.Y) && (op2.vstart != 0))
                            {
                                op.vend = op2.vstart;
                            }
                            else
                            {
                                if ((op.Xe == op2.Xe) && (op.Ye == op2.Ye) && (op2.vend != 0))
                                {
                                    op.vend = op2.vend;
                                }
                            }
                        }
                    }
                    if (op.vend == 0)
                    {
                        op.vend = temp;
                        temp++;
                    }
                }
            }

            int n;      // Общее количество работ по проекту          
            // (количество ребер ориентированного графа).
            int[] ii = new int[20];  // Вектор-пара, представляющая k-ю работу,    
            int[] jj = new int[20];  // которая понимается как стрелка, связыва-   
            // ющая событие i[k] с событием j[k]          
            // Граф задан массивом ребер:                 
            // (i[0],j[0]),(i[1],j[1]),...,(i[n-1],j[n-1])    
            int k;      // Параметр цикла.
            n = list.Count;
            k = 0;
            list.Sort(delegate(Operation a, Operation b)
            {
                return a.vstart.CompareTo(b.vstart);
            });
            foreach (Operation op in list)
            {
                ii[k] = op.vstart;
                jj[k] = op.vend;
                k++;
            }
            List<string> yeah = new List<string>();
            Critical_Path(n, ii, jj, yeah);
            int fe = yeah.Count;
            List<Operation>[] ye = new List<Operation>[fe];
            for (int j = 0; j < fe; j++)
            {
                ye[j] = new List<Operation>();
            }
            for (int j = 0; j < fe; j++)
            {
                int tmp = yeah[j].Split(' ').GetLength(0);
                string fg = yeah[j].Split(' ')[0];
                for (int i = 1; i < tmp; i++)
                {
                    string now = yeah[j].Split(' ')[i];
                    foreach (Operation op in list)
                    {
                        if ((op.vstart.ToString() == fg) && (op.vend.ToString() == now))
                        {
                            ye[j].Add(op);
                            fg = yeah[j].Split(' ')[i];
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////
            bool flag2 = false;
            int cost = 0;
            foreach (Operation op in list)
            {
                op.temptime = op.time; //Для последующего вычисления стоимости
            }
            do
            {
                flag2 = false;
                List<int> giper = new List<int>();
                int numgiper = 0;
                int numPredgiper = 0;
                Operation x = null;
                for (int j = 0; j < fe; j++)
                {
                    int tmpgiper = 0;
                    foreach (Operation op in ye[j])
                    {
                        tmpgiper += op.time;
                    }
                    if (tmpgiper == numgiper)
                    {
                        giper.Add(j);
                    }
                    else
                    {
                        if (tmpgiper > numgiper)
                        {
                            if (numgiper != 0)
                                numPredgiper = numgiper;
                            giper.Clear();
                            giper.Add(j);
                            numgiper = tmpgiper;
                        }
                        else
                        {
                            numPredgiper = tmpgiper;
                        }
                    }                    
                }
                int min = 999999999;
                foreach (int i in giper)
                {
                    foreach (Operation op in ye[i])
                    {
                        if ((op.probcost < min) && (op.probtime != 0))
                        {
                            min = op.probcost;
                            x = op;
                        }
                    }
                }
                int tmp = 0;
                if (giper.Count > 1)
                {
                    tmp = 1;
                }
                else
                {
                    tmp = numgiper - numPredgiper;
                }
                if (x != null)
                {
                    if (numPredgiper != 0)
                    {
                        flag2 = true;
                        while ((x.probtime != 0) && (tmp != 0) && flag2)
                        {
                            flag2 = true;
                            tmp--;
                            x.time--;
                            x.probtime--;
                            cost += x.probcost;
                            int numgiper3 = 0;
                            for (int j = 0; j < fe; j++)
                            {
                                int tmpgiper = 0;
                                foreach (Operation op in ye[j])
                                {
                                    tmpgiper += op.time;
                                }
                                if (tmpgiper > numgiper3)
                                {
                                    numgiper3 = tmpgiper;
                                }
                            }
                            if ((((cost - (planTime - numgiper3) * profit) > maxCost) && (numgiper<=planTime)) 
                                || ((cost > maxCost) && (numgiper>planTime)))
                            {
                                x.time++;
                                tmp++;
                                x.probtime++;
                                cost -= x.probcost;
                                flag2 = false;
                                tmp = 0;
                            }
                        }

                        /*if ((!(((cost + x.probcost - (planTime - numPredgiper - tmp) * profit <= maxCost) && (planTime >= numPredgiper + tmp))
                                || ((cost + x.probcost <= maxCost) && (planTime < numPredgiper + tmp)))) && (giper.Count > 1))
                        {
                            x.time++;
                            flag2 = false;
                        }*/
                    }
                }
            } while (flag2);
            cost = 0;
            foreach (Operation op in list)
            {
                cost += (op.temptime - op.time) * op.probcost;
            }
            List<int> giper2 = new List<int>();
            int numgiper2 = 0;
            for (int j = 0; j < fe; j++)
            {
                int tmpgiper = 0;
                foreach (Operation op in ye[j])
                {
                    tmpgiper += op.time;
                }
                if (tmpgiper == numgiper2)
                {
                    giper2.Add(j);
                }
                if (tmpgiper > numgiper2)
                {
                    giper2.Clear();
                    giper2.Add(j);
                    numgiper2 = tmpgiper;
                }
            }
            string victory = "";
            foreach (int i in giper2)
            {
                foreach (Operation op in ye[i])
                {
                    op.crit = true;
                    double Num;
                    bool isNum = double.TryParse(op.name, out Num);
                    if (!isNum)
                        victory += op.name + " ";
                }
                victory += ";";
            }
            string timestr = "";
            foreach (Operation op in list)
            {
                double Num;
                bool isNum = double.TryParse(op.name, out Num);
                if (!isNum)
                    timestr += op.name + " -> " + op.time + " ; ";
            }
            label4.Text = timestr;
            label5.Text = "Дополнительные затраты равны = " + (cost - (planTime - numgiper2) * profit).ToString();
            label1.Text = "Критический путь " + victory;
            label2.Text = "Вес критического пути " + numgiper2.ToString();
            var fileName = "graph.txt";
            string path = Directory.GetCurrentDirectory();

            StringBuilder sb = new StringBuilder();
            sb.Append("digraph G {" + Environment.NewLine);
            sb.Append("rankdir = LR;" + Environment.NewLine);
            for (int j = 0; j < fe; j++)
            {
                foreach (Operation op in ye[j])
                {
                    if (!op.drawed)
                    {
                        if (!op.crit)
                        {
                            if (!op.virt)
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                        else
                        {
                            if (!op.virt)
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [label=\"" + op.name + "\"];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                            else
                            {
                                sb.Append("edge[color = red];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append(op.vstart.ToString() + "->" + op.vend.ToString() + " [style=dotted];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                sb.Append("edge[color = black];");
                                sb.Append((char)13);
                                sb.Append((char)10);
                                op.drawed = true;
                            }
                        }
                    }
                }
            }
            sb.Append("}");
            TextWriter tw = new StreamWriter(fileName);
            tw.WriteLine(sb.ToString());
            tw.Close();

            GenerateGraph(fileName, path);

            System.Diagnostics.Process.Start("graph.jpg");
            list.Clear();
        }

        /*private void optTimeOut(List<Operation> ye, List<Operation>[] yes, int fe, List<int>[] tlist, int[] time, int[] timed, int[] cost)
        {
            List<int> lya = new List<int>();
            optTimeOutA(ye, yes, fe, -1, lya, 0, 0, tlist, time, timed, cost);
        }

        private void optTimeOutA(List<Operation> ye, List<Operation>[] yes, int fe, int next, List<int> tlist,
            int cost, int time, List<int>[] tlistg, int[] timeg, int[] timed, int[] costg)
        {
            for (int i = (ye[next + 1].time - ye[next + 1].probtime); i <= ye[next + 1].time; i++)
            {
                int mcost = cost;
                int mtime = time;
                mcost += (ye[next + 1].time - i) * ye[next + 1].probcost;
                mtime += i;
                List<int> ttlist = new List<int>();
                ttlist.AddRange(tlist);
                ttlist.Add(i);
                if (next + 2 < ye.Count)
                {
                    optTimeOutA(ye, yes, fe, next + 1, ttlist, mcost, mtime, tlistg, timeg, timed, costg);
                }
                else
                {
                    aa++;
                    int numgiper = 0;
                    int numlow = 999999999;
                    for (int s = 0; s < list.Count; s++)
                    {
                        list[s].temptime = ttlist[s];
                    }
                    for (int j = 0; j < fe; j++)
                    {
                        int tmpgiper = 0;
                        foreach (Operation op in yes[j])
                        {
                            tmpgiper += op.temptime;
                        }
                        if (tmpgiper > numgiper)
                        {
                            numgiper = tmpgiper;
                        }
                        if (tmpgiper < numlow)
                        {
                            numlow = tmpgiper;
                        }
                    }
                    costg[aa] = mcost;
                    timeg[aa] = numgiper - numlow;
                    timed[aa] = numgiper;
                    tlistg[aa] = ttlist;
                }

            }
        }*/

    }
}
