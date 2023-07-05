using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace Practica
{
    public partial class Form_plants : Form
    {
        PictureBox[,] pictureBoxes;
        bool flag_h, flag_w, flag_start, flag_stop;
        bool[] flag_Fertility = new bool[5], flag_Humidity = new bool[5], flag_Illumination = new bool[5];
        int[] Surv_rate = new int[3];
        Type[] Surv_type = new Type[3];

        int height, width;
        List<Plant> Plant1_mass = new List<Plant>();

        List<Plant_air_seed> Plant2_mass = new List<Plant_air_seed>();

        List<Plant_seed_by_animal> Plant3_mass = new List<Plant_seed_by_animal>();

        List<Plant_vine> Plant4_mass = new List<Plant_vine>();

        Ground Ground = new Ground();

        public Form_plants()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            flag_h = false;
            flag_w = false;
            flag_start = false;
            flag_stop = false;
            height = (int)numericUpDown_Height.Value;
            width = (int)numericUpDown_Width.Value;
            Ground.Fertility = (int)numericUpDown_Fertility.Value;
            Ground.Humidity = (int)numericUpDown_Humidity.Value;
            Ground.Illumination = (int)numericUpDown_Illumination.Value;
            for (int i = 0; i < 5; i++)
            {
                flag_Fertility[i] = true;
                flag_Humidity[i] = true;
                flag_Illumination[i] = true;
            }
        }

        private void button_Accept_Click(object sender, EventArgs e)
        {
            if (flag_Fertility[0])
                Ground.Fertility = (int)numericUpDown_Fertility.Value;
            if (flag_Humidity[0])
                Ground.Humidity = (int)numericUpDown_Humidity.Value;
            if (flag_Illumination[0])
                Ground.Illumination = (int)numericUpDown_Illumination.Value;
            timer.Interval = 2500 - (int)trackBar1.Value;
            Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
            Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
            Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
            Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Grow(ref Plant1_mass, ref pictureBoxes, label_Population_Plant1);
            Grow(ref Plant2_mass, ref pictureBoxes, label_Population_Plant2);
            Grow(ref Plant3_mass, ref pictureBoxes, label_Population_Plant3);
            Grow(ref Plant4_mass, ref pictureBoxes, label_Population_Plant4);
            Print_Number();
        }

        private void Print_Number()
        {
            int n1 = 0, n2 = 0, n3 = 0, n4 = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (pictureBoxes[i, j].BackColor == Color.Red || pictureBoxes[i, j].BackColor == Color.LightCoral)
                        n1++;
                    else if (pictureBoxes[i, j].BackColor == Color.Green || pictureBoxes[i, j].BackColor == Color.Lime)
                        n2++;
                    else if (pictureBoxes[i, j].BackColor == Color.Blue || pictureBoxes[i, j].BackColor == Color.Cyan)
                        n3++;
                    else if (pictureBoxes[i, j].BackColor == Color.Indigo || pictureBoxes[i, j].BackColor == Color.BlueViolet)
                        n4++;
                }
            }
            label_Population_Plant1.Text = Convert.ToString(n1);
            label_Population_Plant2.Text = Convert.ToString(n2);
            label_Population_Plant3.Text = Convert.ToString(n3);
            label_Population_Plant4.Text = Convert.ToString(n4);

        }

        private void StartSimulation()
        {
            if (timer.Enabled)
                return;
            timer.Start();
        }

        private void StopSimulation()
        {
            if (!timer.Enabled)
                return;
            timer.Stop();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            if (flag_start)
                if (flag_stop)
                {
                    flag_stop = false;
                    button_Stop.Text = "Стоп";
                    StartSimulation();
                }
                else
                {
                    flag_stop = true;
                    button_Stop.Text = "Продолжить";
                    StopSimulation();
                }
        }

        private void numericUpDown_Width_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Width.Value != 0)
                flag_w = true;
        }

        private void numericUpDown_Height_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Height.Value != 0)
                flag_h = true;
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (flag_w)
            {
                width = Convert.ToInt32(numericUpDown_Width.Value);
                flag_w = false;
            }
            if (flag_h)
            {
                height = Convert.ToInt32(numericUpDown_Height.Value);
                flag_h = false;
            }

            if (flag_start)
            {
                flag_start = false;
                button_Start.Text = "Начать";
                flag_stop = false;
                button_Stop.Text = "Стоп";
                StopSimulation();
                for (int i = 0; i < pictureBoxes.GetLength(0); i++)
                {
                    for (int j = 0; j < pictureBoxes.GetLength(1); j++)
                    {
                        this.Controls.Remove(pictureBoxes[i, j]);
                    }
                }
                label_Population_Plant1.Text = Convert.ToString(0);
                label_Population_Plant2.Text = Convert.ToString(0);
                label_Population_Plant3.Text = Convert.ToString(0);
                label_Population_Plant4.Text = Convert.ToString(0);
                Plant1_mass.Clear();
                Plant2_mass.Clear();
                Plant3_mass.Clear();
                Plant4_mass.Clear();
            }
            else
            {
                flag_start = true;
                button_Start.Text = "Очистить";
                pictureBoxes = new PictureBox[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        pictureBoxes[i, j] = new PictureBox();
                        pictureBoxes[i, j].Size = new Size(13, 13);
                        pictureBoxes[i, j].Location = new Point(110 + j * 15, 2 + i * 15);
                        pictureBoxes[i, j].BackColor = Color.Transparent;
                        this.Controls.Add(pictureBoxes[i, j]);
                    }
                }
                Set_Plant_mass(ref Plant1_mass, ref pictureBoxes, height, width, Color.Red, label_Population_Plant1);
                Set_Plant_mass(ref Plant2_mass, ref pictureBoxes, height, width, Color.Green, label_Population_Plant2);
                Set_Plant_mass(ref Plant3_mass, ref pictureBoxes, height, width, Color.Blue, label_Population_Plant3);
                Set_Plant_mass(ref Plant4_mass, ref pictureBoxes, height, width, Color.Indigo, label_Population_Plant4);

                Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
                Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
                Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
                Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);

                StartSimulation();
            }
        }

        private void Set_Plant_mass<T>(ref List<T> Plant_mass, ref PictureBox[,] pictureBoxes, int height, int width, Color color, Label label_Population_Plant) where T : Plant, new()
        {
            Plant_mass.Add(new T());
            Plant_mass[0].Set_Null_Mass(height, width, color);
            pictureBoxes[Plant_mass[0].Y, Plant_mass[0].X].BackColor = Plant_mass[0].Color;
            label_Population_Plant.Text = Convert.ToString(Plant_mass.Count);
        }

        private void Grow<T>(ref List<T> Plant_mass, ref PictureBox[,] pictureBoxes, Label label_Population_Plant) where T : Plant, new()
        {
            int old_number = Plant_mass.Count;          //число старых элементов в списке
            int last_number = Plant_mass.Count;         //
            for (int i = 0; i < old_number; i++)
            {
                Plant_mass[i].Aging(pictureBoxes);

                //удаление старых растений
                if (Plant_mass[i].Age >= 5)
                {
                    pictureBoxes[Plant_mass[i].Y, Plant_mass[i].X].BackColor = Color.Transparent;
                    Plant_mass.RemoveAt(i);
                    old_number--;
                    last_number--;

                    //label_Population_Plant.Text = Convert.ToString(Plant_mass.Count);  
                    i--;
                    continue;
                }

                //удаление непроросших семян
                for (int j = last_number; j < Plant_mass.Count; j++)
                {
                    if (Plant_mass[j].Color != pictureBoxes[Plant_mass[j].Y, Plant_mass[j].X].BackColor)
                    {
                        Plant_mass.RemoveAt(j--);

                        //label_Population_Plant.Text = Convert.ToString(Plant_mass.Count);
                    }
                }

                last_number = Plant_mass.Count;

                if (Plant_mass.Count > 0 && Plant_mass[i].Age > 0) //Plant_mass.Count > 0 && 
                {
                    int old_x = Plant_mass[i].X;
                    int old_y = Plant_mass[i].Y;
                    for (int j = 0; j < Plant_mass[i].Sprouts; j++)
                    {
                        Plant_mass.Add(new T());
                        PropertyInfo property;
                        switch (typeof(T).Name)
                        {
                            case "Plant":
                                Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case "Plant_air_seed":
                                Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
                                Surv_type[0] = typeof(Plant);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case "Plant_seed_by_animal":
                                Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case "Plant_vine":
                                Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant);
                                break;
                            default: break;
                        }

                        property = typeof(T).GetProperty("Survival_rate", BindingFlags.Public | BindingFlags.Static);
                        if (property != null)
                        {
                            property.SetValue(null, Plant_mass.Last().Set_Survival_rate(Ground));
                        }

                        for (int k = 0; k < 3; k++)
                        {
                            property = Surv_type[k].GetProperty("Survival_rate", BindingFlags.Public | BindingFlags.Static);
                            if (property != null)
                            {
                                Surv_rate[k] = (int)property.GetValue(null);
                            }
                        }

                        Plant_mass.Last().Reproduction(old_x, old_y, width, height, pictureBoxes, Ground, Surv_rate);

                        if (Plant_mass.Last().Sprouts > 0)
                        {
                            //label_Population_Plant.Text = Convert.ToString(Plant_mass.Count);
                        }
                        else
                        {
                            Plant_mass.RemoveAt(Plant_mass.Count - 1);
                        }
                    }
                }
            }
        }

        private void numericUpDown_Fertility_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Fertility.Value >= 0 && numericUpDown_Fertility.Value <= 100)
                flag_Fertility[0] = true;
            else
            {
                numericUpDown_Fertility.Value = 0;
                flag_Fertility[0] = false;
            }
        }

        private void numericUpDown_Humidity_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Humidity.Value >= 0 && numericUpDown_Humidity.Value <= 100)
                flag_Humidity[0] = true;
            else
            {
                numericUpDown_Humidity.Value = 0;
                flag_Humidity[0] = false;
            }
        }

        private void numericUpDown_Illumination_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Illumination.Value >= 0 && numericUpDown_Illumination.Value <= 100)
                flag_Illumination[0] = true;
            else
            {
                numericUpDown_Illumination.Value = 0;
                flag_Illumination[0] = false;
            }
        }

        private void numericUpDown_Fertility1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Fertility1.Value >= 0 && numericUpDown_Fertility1.Value <= 100)
                flag_Fertility[1] = true;
            else
            {
                numericUpDown_Fertility1.Value = 0;
                flag_Fertility[1] = false;
            }
        }

        private void numericUpDown_Humidity1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Humidity1.Value >= 0 && numericUpDown_Humidity1.Value <= 100)
                flag_Humidity[1] = true;
            else
            {
                numericUpDown_Humidity1.Value = 0;
                flag_Humidity[1] = false;
            }
        }

        private void numericUpDown_Illumination1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Illumination1.Value >= 0 && numericUpDown_Illumination1.Value <= 100)
                flag_Illumination[1] = true;
            else
            {
                numericUpDown_Illumination1.Value = 0;
                flag_Illumination[1] = false;
            }
        }

        private void numericUpDown_Fertility2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Fertility2.Value >= 0 && numericUpDown_Fertility2.Value <= 100)
                flag_Fertility[2] = true;
            else
            {
                numericUpDown_Fertility2.Value = 0;
                flag_Fertility[2] = false;
            }
        }

        private void numericUpDown_Humidity2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Humidity2.Value >= 0 && numericUpDown_Humidity2.Value <= 100)
                flag_Humidity[2] = true;
            else
            {
                numericUpDown_Humidity2.Value = 0;
                flag_Humidity[2] = false;
            }
        }

        private void numericUpDown_Illumination2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Illumination2.Value >= 0 && numericUpDown_Illumination2.Value <= 100)
                flag_Illumination[2] = true;
            else
            {
                numericUpDown_Illumination2.Value = 0;
                flag_Illumination[2] = false;
            }
        }

        private void numericUpDown_Fertility3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Fertility3.Value >= 0 && numericUpDown_Fertility3.Value <= 100)
                flag_Fertility[3] = true;
            else
            {
                numericUpDown_Fertility3.Value = 0;
                flag_Fertility[3] = false;
            }
        }

        private void numericUpDown_Humidity3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Humidity3.Value >= 0 && numericUpDown_Humidity3.Value <= 100)
                flag_Humidity[3] = true;
            else
            {
                numericUpDown_Humidity3.Value = 0;
                flag_Humidity[3] = false;
            }
        }

        private void numericUpDown_Illumination3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Illumination3.Value >= 0 && numericUpDown_Illumination3.Value <= 100)
                flag_Illumination[3] = true;
            else
            {
                numericUpDown_Illumination3.Value = 0;
                flag_Illumination[3] = false;
            }
        }

        private void numericUpDown_Fertility4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Fertility4.Value >= 0 && numericUpDown_Fertility4.Value <= 100)
                flag_Fertility[4] = true;
            else
            {
                numericUpDown_Fertility4.Value = 0;
                flag_Fertility[4] = false;
            }
        }

        private void numericUpDown_Humidity4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Humidity4.Value >= 0 && numericUpDown_Humidity4.Value <= 100)
                flag_Humidity[4] = true;
            else
            {
                numericUpDown_Humidity4.Value = 0;
                flag_Humidity[4] = false;
            }
        }

        private void numericUpDown_Illumination4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_Illumination4.Value >= 0 && numericUpDown_Illumination4.Value <= 100)
                flag_Illumination[4] = true;
            else
            {
                numericUpDown_Illumination4.Value = 0;
                flag_Illumination[4] = false;
            }
        }


        private void Set_PlantChar(int n, Type type, NumericUpDown numericUpDown_Fertility, NumericUpDown numericUpDown_Humidity, NumericUpDown numericUpDown_Illumination)
        {
            PropertyInfo property = type.GetProperty("Required_fertility", BindingFlags.Public | BindingFlags.Static);
            if (property != null && flag_Fertility[n])
            {
                property.SetValue(null, (int)numericUpDown_Fertility.Value);
            }

            property = type.GetProperty("Required_humidity", BindingFlags.Public | BindingFlags.Static);
            if (property != null && flag_Humidity[n])
            {
                property.SetValue(null, (int)numericUpDown_Humidity.Value);
            }

            property = type.GetProperty("Required_illumination", BindingFlags.Public | BindingFlags.Static);
            if (property != null && flag_Illumination[n])
            {
                property.SetValue(null, (int)numericUpDown_Illumination.Value);
            }
        }
    }
}

