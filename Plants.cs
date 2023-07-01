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
    public partial class Plants : Form
    {
        PictureBox[,] pictureBoxes;
        bool flag_h, flag_w, flag_start, flag_stop;
        bool[] flag_Fertility = new bool[5], flag_Humidity = new bool[5], flag_Illumination = new bool[5];
        int[] Surv_rate = new int[3];
        Type[] Surv_type = new Type[3];

        int nomber_Plant1, size_Plant1 = 100, height, width;
        Plant[] Plant1_mass = new Plant[100];

        int nomber_Plant2, size_Plant2 = 100;
        Plant_air_seed[] Plant2_mass = new Plant_air_seed[100];

        int nomber_Plant3, size_Plant3 = 100;
        Plant_seed_by_animal[] Plant3_mass = new Plant_seed_by_animal[100];

        int nomber_Plant4, size_Plant4 = 100;
        Plant_vine[] Plant4_mass = new Plant_vine[100];

        Ground Ground = new Ground();

        public Plants()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nomber_Plant1 = 0;
            nomber_Plant2 = 0;
            nomber_Plant3 = 0;
            nomber_Plant4 = 0;
            flag_h = false;
            flag_w = false;
            flag_start = false;
            flag_stop = false;
            height = (int)numericUpDown_Height.Value;
            width = (int)numericUpDown_Width.Value;
            Ground.Fertility = (int)numericUpDown_Fertility.Value;
            Ground.Humidity = (int)numericUpDown_Humidity.Value;
            Ground.Illumination = (int)numericUpDown_Illumination.Value;
            for(int i = 0; i < 5; i++)
            {
                flag_Fertility[i] = true;
                flag_Humidity[i] = true;
                flag_Illumination[i] = true;
            }
        }

        private void button_Accept_Click(object sender, EventArgs e)
        {
            if(flag_Fertility[0])
                Ground.Fertility = (int)numericUpDown_Fertility.Value;
            if(flag_Humidity[0])
                Ground.Humidity = (int)numericUpDown_Humidity.Value;
            if(flag_Illumination[0])
                Ground.Illumination = (int)numericUpDown_Illumination.Value;
            Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
            Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
            Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
            Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Grow<Plant>(ref Plant1_mass, ref pictureBoxes, ref nomber_Plant1, ref size_Plant1, label_Population_Plant1, 1);
            Grow<Plant_air_seed>(ref Plant2_mass, ref pictureBoxes, ref nomber_Plant2, ref size_Plant2, label_Population_Plant2, 2);
            Grow<Plant_seed_by_animal>(ref Plant3_mass, ref pictureBoxes, ref nomber_Plant3, ref size_Plant3, label_Population_Plant3, 3);
            Grow<Plant_vine>(ref Plant4_mass, ref pictureBoxes, ref nomber_Plant4, ref size_Plant4, label_Population_Plant4, 4);
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
            if(flag_start)
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
                        //pictureBoxes[i, j].BackColor = Color.Gray;
                        //pictureBoxes[height - 1, width - 1].BackColor = Color.Green;
                        this.Controls.Add(pictureBoxes[i, j]);
                    }
                }
                Set_Plant_mass<Plant>(ref Plant1_mass, ref pictureBoxes, ref nomber_Plant1, height, width, Color.LightCoral, label_Population_Plant1);
                Set_Plant_mass<Plant_air_seed>(ref Plant2_mass, ref pictureBoxes, ref nomber_Plant2, height, width, Color.GreenYellow, label_Population_Plant2);
                Set_Plant_mass<Plant_seed_by_animal>(ref Plant3_mass, ref pictureBoxes, ref nomber_Plant3, height, width, Color.Cyan, label_Population_Plant3);
                Set_Plant_mass<Plant_vine>(ref Plant4_mass, ref pictureBoxes, ref nomber_Plant4, height, width, Color.BlueViolet, label_Population_Plant4);

                Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
                Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
                Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
                Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);

                StartSimulation();
                //MessageBox.Show("Симуляция окончена", "Все зараженные выздоровели или умерли", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Set_Plant_mass<T>(ref T[] Plant_mass, ref PictureBox[,] pictureBoxes, ref int nomber_Plant, int height, int width, Color color, Label label_Population_Plant) where T : Plant, new()
        {
            nomber_Plant = 0;
            Plant_mass[0] = new T();
            Plant_mass[0].Set_Null_Mass(height, width, color);
            pictureBoxes[Plant_mass[0].Y, Plant_mass[0].X].BackColor = Plant_mass[0].Color;
            label_Population_Plant.Text = Convert.ToString(nomber_Plant + 1);
        }

        private void Grow<T>(ref T[] Plant_mass, ref PictureBox[,] pictureBoxes, ref int nomber_Plant, ref int size_Plant, Label label_Population_Plant, int n) where T : Plant, new()
        {
            int old_nomber = nomber_Plant, old_x, old_y;
            for (int i = 0; i <= old_nomber; i++)
            {
                
                if (Plant_mass[i].Sprouts > 0)
                {
                    Plant_mass[i].Aging(pictureBoxes);
                }
                
                /*
                //if (Plant_mass[i].Age % 12 == 0) ;
                while(Plant_mass[i].Age > 0 && Plant_mass[i].Age % 5 == 0)
                {
                    Plant_mass[i].Death(pictureBoxes);
                    //Plant_mass[i] = null;
                    //if(nomber_Plant >= 0)
                    Plant_mass[i] = Plant_mass[nomber_Plant];
                    nomber_Plant--;
                    i--;
                }
                */

                if (Plant_mass[i].Age == 5)
                {
                    pictureBoxes[Plant_mass[i].Y, Plant_mass[i].X].BackColor = Color.Transparent;
                    Plant_mass[i] = Plant_mass[nomber_Plant];
                    nomber_Plant--;
                    label_Population_Plant.Text = Convert.ToString(nomber_Plant + 1);
                    i--;
                    continue;
                }
                /*
                if (Plant_mass[i].Color != pictureBoxes[Plant_mass[i].Y, Plant_mass[i].X].BackColor)
                {
                    Plant_mass[i] = Plant_mass[nomber_Plant];
                    nomber_Plant--;
                    label_Population_Plant.Text = Convert.ToString(nomber_Plant + 1);
                    i--;
                    continue;
                }
                */
                if (Plant_mass[i].Age > 0)
                {
                    old_x = Plant_mass[i].X;
                    old_y = Plant_mass[i].Y;
                    int sprouts = Plant_mass[i].Sprouts;
                    for (int j = 0; j < sprouts; j++)
                    {
                        Plant_mass[nomber_Plant + 1] = new T();
                        PropertyInfo property;
                        switch (n)
                        {
                            case 1:
                                Set_PlantChar(1, typeof(Plant), numericUpDown_Fertility1, numericUpDown_Humidity1, numericUpDown_Illumination1);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case 2:
                                Set_PlantChar(2, typeof(Plant_air_seed), numericUpDown_Fertility2, numericUpDown_Humidity2, numericUpDown_Illumination2);
                                Surv_type[0] = typeof(Plant);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case 3:
                                Set_PlantChar(3, typeof(Plant_seed_by_animal), numericUpDown_Fertility3, numericUpDown_Humidity3, numericUpDown_Illumination3);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant);
                                Surv_type[2] = typeof(Plant_vine);
                                break;
                            case 4:
                                Set_PlantChar(4, typeof(Plant_vine), numericUpDown_Fertility4, numericUpDown_Humidity4, numericUpDown_Illumination4);
                                Surv_type[0] = typeof(Plant_air_seed);
                                Surv_type[1] = typeof(Plant_seed_by_animal);
                                Surv_type[2] = typeof(Plant);
                                break;
                            default:
                                break;
                        }

                        property = typeof(T).GetProperty("Survival_rate", BindingFlags.Public | BindingFlags.Static);
                        if (property != null)
                        {
                            property.SetValue(null, Plant_mass[nomber_Plant + 1].Set_Survival_rate(Ground));
                        }
                        
                        for(int k = 0; k < 3; k++)
                        {
                            property = Surv_type[k].GetProperty("Survival_rate", BindingFlags.Public | BindingFlags.Static);
                            if (property != null)
                            {
                                Surv_rate[k] = (int)property.GetValue(null);
                            }
                        }

                        Plant_mass[nomber_Plant + 1].Reproduction(old_x, old_y, width, height, pictureBoxes, Ground, Surv_rate);

                        if (Plant_mass[nomber_Plant + 1].Sprouts > 0)
                        {
                            nomber_Plant++;
                            label_Population_Plant.Text = Convert.ToString(nomber_Plant + 1);
                            if (nomber_Plant >= size_Plant - 1)
                            {
                                size_Plant += 100;
                                Array.Resize(ref Plant_mass, size_Plant);
                            }
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

