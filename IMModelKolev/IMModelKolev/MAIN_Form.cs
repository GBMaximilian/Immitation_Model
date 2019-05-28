using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IMModelKolev
{
    public partial class MAIN_Form : Form
    {
        TBMath_2 tbn;
        MplexMath_2 MXP;
        Buff_2 BUF;
        Leaky_Bucket_Algoritm lbn;


        //LinkedList<LinkedList<double>> LBE = new LinkedList<LinkedList<double>>();


        int LB_COUNT = 0;


        public LinkedList<TabPage> TPe = new LinkedList<TabPage>();
        public void Enqueue(TabPage value)//Добавляет элемент в очередь.
        {
            TPe.AddFirst(value);
        }
        public TabPage Dequeue()//Удаляет первый помещенный элемент из очереди и возвращает его. 
        {
            if (TPe.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }
            TabPage last = TPe.Last.Value;
            TPe.RemoveLast();
            return last;
        }
        public TabPage Dequeue_F()//Удаляет последний помещенный элемент из очереди и возвращает его. 
        {
            if (TPe.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }
            TabPage last = TPe.First.Value;
            TPe.RemoveFirst();
            return last;
        }
        public TabPage Pop()// первый помещенный элемент из очереди и возвращает его. 
        {
            if (TPe.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }
            TabPage last = TPe.Last.Value;
            return last;
        }
        public int Count
        {
            get
            { return TPe.Count; }
        }

        public MAIN_Form()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Graph();
            MXP = new MplexMath_2(ref textBox2, ref richTextBox2, ref textBox4, ref chart1);
            MXP.Graph();
            BUF = new Buff_2();
            Delete_TB.Enabled = false;
            Delete_LB.Enabled = false;
            Start_modelling.Enabled = false;
            textBox5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Delete_TB.Enabled = true;
            Start_modelling.Enabled = true;

            TabPage newTabPage = new TabPage();
            newTabPage.Text = "TB" + (Count_Of_Page("TB") + 1);

            TB_Interface TI = new TB_Interface();


            TI.Graf_TB();

            newTabPage.Controls.Add(TI.Size_of_TB);            //0
            newTabPage.Controls.Add(TI.Weight_of_one_token);   //1
            newTabPage.Controls.Add(TI.CIR);                   //2
            newTabPage.Controls.Add(TI.Interval);              //3
            newTabPage.Controls.Add(TI.Size_of_TB_L);          //4
            newTabPage.Controls.Add(TI.Weight_of_one_token_L); //5
            newTabPage.Controls.Add(TI.CIR_L);                 //6
            newTabPage.Controls.Add(TI.Interval_L);            //7
            newTabPage.Controls.Add(TI.chart1);                //8
            newTabPage.Controls.Add(TI.Help_Ro);               //9
            newTabPage.Controls.Add(TI.RTB);                   //10
            newTabPage.Controls.Add(TI.Weight_of_one_token_ED);//11
            newTabPage.Controls.Add(TI.Size_of_TB_ED);         //12
            newTabPage.Controls.Add(TI.CIR_ED);                //13
            newTabPage.Controls.Add(TI.Generated_S);           //14
            newTabPage.Controls.Add(TI.Generated_To);          //15
            newTabPage.Controls.Add(TI.Generated_To_T);        //16
            newTabPage.Controls.Add(TI.Generated_S_T);         //17
            newTabPage.Controls.Add(TI.Generated_ED);          //18
            //newTabPage.Controls.Add(Interval_ED);

            Enqueue(newTabPage);
            tabControl1.TabPages.Add(TPe.ElementAt(0));
            textBox1.Text = Convert.ToString(Count_Of_Page("TB"));
        }

        private void button4_Click(object sender, EventArgs e)
        {

            tbn = new TBMath_2();
            lbn = new Leaky_Bucket_Algoritm();
            Random rand = new Random();

            double T = Convert.ToDouble(((TextBox)TPe.ElementAt(0).Controls[0]).Text);
            double Nt = Convert.ToDouble(((TextBox)TPe.ElementAt(0).Controls[1]).Text);
            double CIR = Convert.ToDouble(((TextBox)TPe.ElementAt(0).Controls[2]).Text);
            double Tk = Convert.ToDouble(((TextBox)TPe.ElementAt(0).Controls[3]).Text);
            double V;
            double RoTk_1 = 0;
            double[] ch = new double[4];

            int Gen_Hight, Gen_Low;

            double C = Convert.ToDouble(textBox2.Text);
            double Time_M = Convert.ToDouble(textBox3.Text);

            double L_SUMM = 0;
            double V_SUMM = 0;
            double Ro_SUMM = 0;
            double NIU = 0;

            double[] Gi = new double[TPe.Count];
            LinkedList<double> Gi_f = new LinkedList<double>();

            progressBar1.Maximum = Convert.ToInt16(textBox3.Text) + 1;
            progressBar1.Value = 0;

            int Time_To_Model = Convert.ToInt16(textBox3.Text);

            bool flag = true;
            double RoTk_LB = 0;
            //LinkedList<double> pom = new LinkedList<double>();


            LinkedList<double>[] masp = new LinkedList<double>[LB_COUNT];///////
            for (int ikj = 0; ikj < LB_COUNT; ikj++)
            {
                //LinkedList<double> k = new LinkedList<double>();
                masp[ikj] = new LinkedList<double>();
            }
            /*
            richTextBox2.Text += LB_COUNT;
            richTextBox2.Text += '\n';
            richTextBox2.Text += masp.Length;
            richTextBox2.Text += '\n';
            */

            for (int k = 0; k <= Time_To_Model; k++)
            {
                progressBar1.Value++;
                richTextBox2.Text += "STEP №" + k;
                richTextBox2.Text += '\n';
                richTextBox2.Text += "---------------------------";
                richTextBox2.Text += '\n';
                richTextBox2.Text += "--Token Bucket & Leaky Bucket--";
                richTextBox2.Text += '\n';

                int lb_count = LB_COUNT;
                //for (int z = 0; z <= TPe.Count-1; z++)
                Gi_f.Clear();
                for (int z = TPe.Count - 1; z >= 0; z--)
                {
                    
                    if (TPe.ElementAt(z).Text.Contains("TB"))
                    {
                        T = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[0]).Text);
                        Nt = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[1]).Text);
                        CIR = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[2]).Text);
                        Tk = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[3]).Text);

                        Gen_Hight = Convert.ToInt32(((TextBox)TPe.ElementAt(z).Controls[16]).Text);
                        Gen_Low = Convert.ToInt32(((TextBox)TPe.ElementAt(z).Controls[17]).Text);


                        if (flag)
                        {
                            RoTk_1 = T / 2;                            
                        }
                        else
                        {
                            RoTk_1 = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[9]).Text);
                        }

                        V = rand.Next(Gen_Low, Gen_Hight);
                        ch = tbn.M(CIR, Tk, T, Nt, RoTk_1, V);
                        ((TextBox)TPe.ElementAt(z).Controls[9]).Text = Convert.ToString(ch[3]);

                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["GTk"].Points.AddXY(k, ch[0]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["VTk"].Points.AddXY(k, ch[1]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["Заполненность буфера токенов"].Points.AddXY(k, ch[2]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["Потери"].Points.AddXY(k, (ch[1]-ch[0]));

                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += "Шаг: " + k;
                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += "GTk: " + ch[0] + "; " + "VTk: " + ch[1] + "; " + "RoTk" + ch[2] + "; ";
                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += '\n';


                        Gi[z] = ch[0];
                        Gi_f.AddLast(ch[0]);
                        richTextBox2.Text += /*"Шаг: " + k + */"TB" + (TPe.Count - z) + " GTk = " + ch[0];
                        richTextBox2.Text += '\n';
                        V_SUMM += V;
                        Ro_SUMM += (V - ch[0]);
                    }
                    if (TPe.ElementAt(z).Text.Contains("LB")) /*РАБОТА LEAKY BUCKET*/
                    {
                    
                        T = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[0]).Text);
                        //Nt = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[1]).Text);
                        CIR = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[2]).Text);
                        Tk = Convert.ToDouble(((TextBox)TPe.ElementAt(z).Controls[3]).Text);

                        Gen_Hight = Convert.ToInt32(((TextBox)TPe.ElementAt(z).Controls[16]).Text);
                        Gen_Low = Convert.ToInt32(((TextBox)TPe.ElementAt(z).Controls[17]).Text);

                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += "Шаг: " + k;
                        V = rand.Next(Gen_Low, Gen_Hight);
                        
                        ch = lbn.LM(CIR, Tk, T, RoTk_LB, V, ref masp[lb_count-1], ref Gi_f);
                        
                        //richTextBox2.Text += "индекс передаваемого элемента "+(lb_count - 1);
                        //richTextBox2.Text += '\n';
                        lb_count--;////*****

                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["GTk"].Points.AddXY(k, ch[0]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["VTk"].Points.AddXY(k, ch[1]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["Заполненность буфера"].Points.AddXY(k, ch[2]);
                        ((System.Windows.Forms.DataVisualization.Charting.Chart)TPe.ElementAt(z).Controls[8]).Series["Потери на входе LB"].Points.AddXY(k, ch[3]);

                        
                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += " GTk: " + ch[0] + "; " + " VTk: " + ch[1] + "; " + "заполнение LB: " + ch[2] + "; "+ "Потери"+ ch[3];
                        ((RichTextBox)TPe.ElementAt(z).Controls[10]).Text += '\n';


                        Gi[z] = ch[0];
                        //Gi_f.AddLast(ch[0]);
                        richTextBox2.Text += /*"Шаг: " + k + */"LB" + (TPe.Count - z) + " GTk = " + ch[0];
                        richTextBox2.Text += '\n';
                        V_SUMM += V;
                        Ro_SUMM += (V - ch[0]);
                    }
                    
                }
                richTextBox2.Text += "--Multiplexor--";
                richTextBox2.Text += '\n';
                MXP.MX(Gi_f.ToArray(), k);
                L_SUMM += MXP.L_tk;
                flag = false;

            }
            NIU = (V_SUMM - (L_SUMM + Ro_SUMM)) / (C * Time_M);
            NIU_T.Text = Convert.ToString(NIU);
            richTextBox2.Text += "объем поступивших пакетов за все  время: " + MXP.SG;
            richTextBox2.Text += '\n';
            richTextBox2.Text += "Потери на мультиплексоре за все  время: "+ L_SUMM;
            richTextBox2.Text += '\n';

            MessageBox.Show("Моделирование закончено");
        }

        private void Delete_TB_Click(object sender, EventArgs e)
            {
                int ind = Index_Of_Page_OnList("TB");
                int indp = Index_Of_Page_OnTab("TB");

                if ((Count != 0) && (ind != -1))
                {
                    tabControl1.TabPages.RemoveAt(indp);
                    TPe.Remove(TPe.ElementAt(ind));
                    if (Count == 0)
                    {
                        Delete_TB.Enabled = false;
                        Start_modelling.Enabled = false;
                    }
                    if (Count_Of_Page("TB") == 0)
                    {
                        Delete_TB.Enabled = false;
                    }
                }
                textBox1.Text = Convert.ToString(Count_Of_Page("TB"));

            }

        private void Add_LB_Click(object sender, EventArgs e)
            {
                Delete_LB.Enabled = true;
                Start_modelling.Enabled = true;


                LB_COUNT= LB_COUNT+1;

            TabPage newTabPage = new TabPage();
                newTabPage.Text = "LB" + (Count_Of_Page("LB") + 1);


            LB_Inretface LI = new LB_Inretface();

                LI.Graf_LB();


                newTabPage.Controls.Add(LI.Size_of_TB);            //0
                newTabPage.Controls.Add(LI.Weight_of_one_token);   //1
                newTabPage.Controls.Add(LI.CIR);                   //2
                newTabPage.Controls.Add(LI.Interval);              //3
                newTabPage.Controls.Add(LI.Size_of_TB_L);          //4
                newTabPage.Controls.Add(LI.Weight_of_one_token_L); //5
                newTabPage.Controls.Add(LI.CIR_L);                 //6
                newTabPage.Controls.Add(LI.Interval_L);            //7
                newTabPage.Controls.Add(LI.chart1);                //8
                newTabPage.Controls.Add(LI.Help_Ro);               //9
                newTabPage.Controls.Add(LI.RTB);                   //10
                newTabPage.Controls.Add(LI.Weight_of_one_token_ED);//11
                newTabPage.Controls.Add(LI.Size_of_TB_ED);         //12
                newTabPage.Controls.Add(LI.CIR_ED);                //13
                newTabPage.Controls.Add(LI.Generated_S);           //14
                newTabPage.Controls.Add(LI.Generated_To);          //15
                newTabPage.Controls.Add(LI.Generated_To_T);        //16
                newTabPage.Controls.Add(LI.Generated_S_T);         //17
                newTabPage.Controls.Add(LI.Generated_ED);          //18

                Enqueue(newTabPage);
                tabControl1.TabPages.Add(TPe.ElementAt(0));
                textBox5.Text = Convert.ToString(Count_Of_Page("LB"));
            }

        private void Delete_LB_Click(object sender, EventArgs e)
            {
                int ind = Index_Of_Page_OnList("LB");
                int indp = Index_Of_Page_OnTab("LB");
            
            if ((Count != 0) && (ind != -1))
                {
                    tabControl1.TabPages.RemoveAt(indp);
                    TPe.Remove(TPe.ElementAt(ind));
                LB_COUNT = LB_COUNT - 1;
                if (Count == 0)
                    {
                        Delete_LB.Enabled = false;
                        Start_modelling.Enabled = false;
                    }
                    if (Count_Of_Page("LB") == 0)
                    {
                        Delete_LB.Enabled = false;
                    }
                }
                textBox5.Text = Convert.ToString(Count_Of_Page("LB"));
            
        }

        public int Index_Of_Page_OnList(string vs)
            {
                string IP = vs + Count_Of_Page(vs);
                int index_of_P = -1;
                for (int i = Count - 1; i >= 0; i--)
                {
                    string st = Convert.ToString(TPe.ElementAt(i).Text);

                    if (st == IP)
                        index_of_P = i;
                    //richTextBox2.Text += IP + "/ " + st + "/ " + index_of_P + "/" + i + ";";
                    //richTextBox2.Text += '\n';

                }
                return index_of_P;
            }

        public int Index_Of_Page_OnTab(string vs)
            {
                string IP = vs + Count_Of_Page(vs);
                int index_of_P = -1;
                for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
                {
                    string st = Convert.ToString(tabControl1.TabPages[i].Text);

                    if (st == IP)
                        index_of_P = i;
                    //richTextBox2.Text += IP + "* " + st + "* " + index_of_P + "*" + i + ";";
                    //richTextBox2.Text += '\n';

                }
                return index_of_P;
            }

        public int Count_Of_Page(string vs)
            {
                int Count_of_P = 0;
                for (int i = Count - 1; i >= 0; i--)
                {
                    string st = Convert.ToString(TPe.ElementAt(i).Text);
                    if (st.Contains(vs))
                        Count_of_P += 1;
                }
                return Count_of_P;
            }
        

        }
    }

