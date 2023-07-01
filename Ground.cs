using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica
{
    public class Ground
    {
        private int fertility;      //плодородность
        private int humidity;       //влажность
        private int illumination;   //освещенность

        public Ground()
        {
            fertility = 0;
            humidity = 0;
            illumination = 0;
        }

        public int Fertility
        {
            get { return fertility; }
            set { fertility = value; }
        }

        public int Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }

        public int Illumination
        {
            get { return illumination; }
            set { illumination = value; }
        }
    }
}
