using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IMModelKolev
{
    
    class TBMath_2
    {        
        public TBMath_2()
        {

        }
        
        public double[] M(double CIR, double Tk, double T, double Nt, double RoTk_1, double V1)
        {
            double RoTk;//бит
            double I, GTk, UTk, V;
            double[] ch = new double[4];
            
            UTk = CIR * Nt;///?????
            RoTk = RoTk_1 + Math.Min(UTk, (T - RoTk_1));
            V = V1;
            if ((V) <= (RoTk_1 + Math.Min(UTk, (T - RoTk_1))))
                I = 1;
            else I = 0;
            GTk = V * I;
            ch[0] = GTk;
            ch[1] = V;
            ch[2] = RoTk;
            RoTk = RoTk - GTk;
            //RoTk_1 = RoTk;
            ch[3] = RoTk;
            return ch;
        }
    }
}