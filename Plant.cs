using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Plant
    {
        protected int x;
        protected int y;
        private static int survival_rate;          //выживаемость
        protected int sprouts;                       //семена
        private static int required_fertility;     //плодородность
        private static int required_humidity;      //влажность
        private static int required_illumination;  //освещенность
        //protected String color;
        protected Color color;
        protected int age;

        public Plant()
        {
            x = 0;
            y = 0;
            sprouts = 0;
            color = Color.Transparent;
            age = 0;
        }
        
        public Plant(int height, int width, Color color)
        {
            x = width / 4;
            y = height / 4;
            survival_rate = 0;
            sprouts = 0;
            required_fertility = 0;
            required_humidity = 0;
            required_illumination = 0;
            this.color = color;
            age = 0;
        }

        public Plant(int requiredFertility, int requiredHumidity, int requiredIllumination)
        {
            Required_fertility = requiredFertility;
            Required_humidity = requiredHumidity;
            Required_illumination = requiredIllumination;
        }

        ~Plant() { }

        public void Set_Place(int i, int j)
        {
            x = j;
            y = i;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Sprouts
        {
            get { return sprouts; }
            set { sprouts = value; }
        }

        public static int Required_fertility
        {
            get { return required_fertility; }
            set { required_fertility = value; }
        }

        public static int Required_humidity
        {
            get { return required_humidity; }
            set { required_humidity = value; }
        }

        public static int Required_illumination
        {
            get { return required_illumination; }
            set { required_illumination = value; }
        }

        public static int Survival_rate
        {
            get { return survival_rate; }
            set { survival_rate = value; }
        }

        public virtual int Set_Survival_rate(Ground ground)
        {
            return 100 - (ground.Fertility - required_fertility) - Math.Abs(ground.Humidity - required_humidity) - Math.Abs(ground.Illumination - required_illumination);
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public void Death(PictureBox[,] pictureBoxes)
        {
            pictureBoxes[y, x].BackColor = Color.Transparent;
        }

        public virtual void Set_Null_Mass(int height, int width, Color color)
        {
            x = width / 4;
            y = height / 4;
            this.color = color;
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Sprouts = rand.Next(2, 6);
        }

        public virtual void Aging(PictureBox[,] pictureBoxes)
        {
            if(age < 1)
            {
                color = Color.Red;
                pictureBoxes[y, x].BackColor = color;
            }
            age++;
        }

        public virtual bool Get_GroundSurv(Ground ground)
        {
            //int n1 = Math.Abs(ground.Humidity - Required_humidity), n2 = Math.Abs(ground.Illumination - Required_illumination);
            if (required_fertility - ground.Fertility < 10 && Math.Abs(ground.Humidity - required_humidity) < 10 && Math.Abs(ground.Illumination - required_illumination) < 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual Plant Reproduction (int old_x, int old_y, int width, int height, PictureBox[,] pictureBoxes, Ground ground, int[] Surv_rate)
        {
            //Random rand = new Random(DateTime.Now.Millisecond);
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Plant plant = new Plant();
            x = rand.Next(old_x - 1, old_x + 2);
            y = rand.Next(old_y - 1, old_y + 2);
            bool flag_color = false;

            if (x < width && x >= 0 && y < height && y >= 0 && Get_GroundSurv(ground))
            {
                if (pictureBoxes[y, x].BackColor == Color.Transparent)
                {
                    flag_color = true;
                }
                else if (pictureBoxes[y, x].BackColor == Color.Lime && survival_rate > Surv_rate[0])
                {
                    flag_color = true;
                }
                else if (pictureBoxes[y, x].BackColor == Color.Cyan && survival_rate > Surv_rate[1])
                {
                    flag_color = true;
                }
                else if (pictureBoxes[y, x].BackColor == Color.BlueViolet && survival_rate > Surv_rate[2])
                {
                    flag_color = true;
                }
                if (flag_color)
                {
                    sprouts = rand.Next(2, 6);
                    color = Color.LightCoral;
                    pictureBoxes[y, x].BackColor = color;
                }
                else sprouts = 0;
            }
            else
            {
                sprouts = 0;
            }
            return plant;
        }   
    }
}
