﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    class Plant_seed_by_animal : Plant
    {
        private static int survival_rate;          //выживаемость
        private static int required_fertility;     //плодородность
        private static int required_humidity;      //влажность
        private static int required_illumination;  //освещенность

        public Plant_seed_by_animal()
        {
            x = 0;
            y = 0;
            sprouts = 0;
            color = Color.Transparent;
            age = 0;
        }

        ~Plant_seed_by_animal() { }

        public Plant_seed_by_animal(int height, int width, Color color)
        {
            x = width - width / 4;
            y = height / 4 - 1;
            survival_rate = 0;
            sprouts = 0;
            required_fertility = 0;
            required_humidity = 0;
            required_illumination = 0;
            this.color = color;
            age = 0;
        }

        public override void Set_Null_Mass(int height, int width, Color color)
        {
            x = width / 4;
            y = height - height / 4 - 1;
            this.color = color;
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Sprouts = rand.Next(4, 7);
        }

        public override void Aging(PictureBox[,] pictureBoxes)
        {
            if (age < 1)
            {
                color = Color.Blue;
                pictureBoxes[y, x].BackColor = color;
            }
            age++;
        }

        public new static int Required_fertility
        {
            get { return required_fertility; }
            set { required_fertility = value; }
        }

        public new static int Required_humidity
        {
            get { return required_humidity; }
            set { required_humidity = value; }
        }

        public new static int Required_illumination
        {
            get { return required_illumination; }
            set { required_illumination = value; }
        }

        public new static int Survival_rate
        {
            get { return survival_rate; }
            set { survival_rate = value; }
        }

        public override int Set_Survival_rate(Ground ground)
        {
            return 100 - (ground.Fertility - required_fertility) - Math.Abs(ground.Humidity - required_humidity) - Math.Abs(ground.Illumination - required_illumination);
        }

        public override bool Get_GroundSurv(Ground ground)
        {
            //int n1 = Math.Abs(ground.Humidity - Required_humidity), n2 = Math.Abs(ground.Illumination - Required_illumination);
            if (required_fertility - ground.Fertility < 15 && Math.Abs(ground.Humidity - required_humidity) < 15 && Math.Abs(ground.Illumination - required_illumination) < 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override Plant Reproduction(int old_x, int old_y, int width, int height, PictureBox[,] pictureBoxes, Ground ground, int[] Surv_rate)
        {
            //Random rand = new Random(DateTime.Now.Millisecond);
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Plant_seed_by_animal plant = new Plant_seed_by_animal();
            x = rand.Next(0, 2);
            if (x == 0)
                x = old_x + width / 5 + rand.Next(-1, 2);
            else 
                x = old_x - width / 5 + rand.Next(-1, 2);
            y = rand.Next(0, 2);
            if (y == 0)
                y = old_y + height / 5 + rand.Next(-1, 2);
            else
                y = old_y - height / 5 + rand.Next(-1, 2);

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
                else if (pictureBoxes[y, x].BackColor == Color.LightCoral && survival_rate > Surv_rate[1])
                {
                    flag_color = true;
                }
                else if (pictureBoxes[y, x].BackColor == Color.BlueViolet && survival_rate > Surv_rate[2])
                {
                    flag_color = true;
                }

                if (flag_color)
                {
                    sprouts = rand.Next(4, 8);
                    color = Color.Cyan;
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
