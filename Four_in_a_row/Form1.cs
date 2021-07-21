using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Four_in_a_row
{

    public partial class Form1 : Form
    {

        static Graphics canvas;
        static int width;
        static int height;

        static List<List<int>> grid_list = new List<List<int>>();

        static int x_cells = 10;
        static int y_cells = 7;
        static int winner = 0;

        static int x1;
        static int x2;
        static int y1;
        static int y2;

        static float cell_width;
        static float cell_height;

        static float elipse_width;
        static float elipse_height;

        static int grid_x;
        static int grid_y;

        static Pen lines = new Pen(Color.LightGray, 1);
        static Pen rectangle = new Pen(Color.LightGray, 10);
        static Pen pen_1 = new Pen(Color.Black, 1);
        static Pen pen_2 = new Pen(Color.Black, 2);

        static Color coin_color1 = Color.FromArgb(255, 128, 128);
        static Color coin_color2 = Color.FromArgb(255, 255, 192);

        static SolidBrush clear_color = new SolidBrush(Color.White);
        static SolidBrush brush_1 = new SolidBrush(coin_color1);
        static SolidBrush brush_2 = new SolidBrush(coin_color2);

        static int active_player = 1;
        static bool is_grid_set = true;
        static bool color_change = false;
        
        

        public Form1()
        {
            InitializeComponent();

            canvas = pictureBox1.CreateGraphics();

            RectangleF bounds = canvas.VisibleClipBounds;
            width = (int)Math.Floor(bounds.Width);
            height = (int)Math.Floor(bounds.Height);

            cell_width = width / x_cells;
            cell_height = height / y_cells;

            elipse_width = 9 * (cell_width / 11);
            elipse_height = 9 * (cell_height / 11);

            
            
        }

        static void initialize_grid()
        {
            grid_list.Clear();

            for (int i = 0; i < y_cells; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < x_cells; j++)
                {
                    row.Add(0);
                }

                grid_list.Add(row);
            }
        }

        static void initialize_game()
        {
            canvas.FillRectangle(clear_color, 0, 0, width, height);
            draw_lines();

            initialize_grid();

            winner = 0;
            is_grid_set = true;

        }

        // DRAW 

            // all lines

        static void draw_lines()
        {
            int x = 1;
            int y = 1;

            canvas.DrawRectangle(rectangle, 0, 0, width, height);

            for (int i = 0; i < y_cells - 1; i++)
            {
                canvas.DrawLine(lines, 0, y * (height / y_cells), width, y * (height / y_cells));
                y++;
            }

            for (int i = 0; i < x_cells - 1; i++)
            {
                canvas.DrawLine(lines, x * (width / x_cells), 5, x * (width / x_cells), height - 5);
                x++;
            }
        }

            // a coin

        static void draw_coin(int grid_x, int grid_y, Brush brush, Pen pen)
        {
            
            float x = grid_x * cell_width + cell_width / 11;
            float y = grid_y * cell_height + cell_height / 11;

            canvas.FillEllipse(brush, x, y, elipse_width, elipse_height);
            canvas.DrawEllipse(pen, x, y, elipse_width, elipse_height);

            if (color_change == false)
            {
                if (active_player == 1)
                {
                    grid_list[grid_y][grid_x] = 1;
                }
                else
                {
                    grid_list[grid_y][grid_x] = 2;
                }
            }
        }

        // Is grid[y][x] in bounds?

        public static bool is_in_bounds(int x, int y)
        {
            if (x < x_cells && y < y_cells && x >= 0 && y >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // How many coins in one direction?

        static int count_coins(int x, int y, int d_x, int d_y, int symbol, 
            out int point_x, out int point_y)
        {
            int coin_ammount = 0;
            point_x = x;
            point_y = y;

            int a = x + d_x;
            int b = y + d_y;

            while (true)
            {
                if (is_in_bounds(a, b) == true)
                {
                    if (grid_list[b][a] == symbol)
                    {
                        coin_ammount++;
                        point_x = a;
                        point_y = b;
                        a += d_x;
                        b += d_y;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return coin_ammount;
        }

        // Is game over? (Do we have a winner?)

        static bool is_game_over(int x, int y)
        {
            if (count_coins(x, y, 0, 1, active_player, out x1, out y1) + count_coins(x, y, 0, -1, active_player, out x2, out y2) + 1 >= 4)
            {
                x1 = x1 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y1 = y1 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;
                x2 = x2 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y2 = y2 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;

                return true;

            }
            else if (count_coins(x, y, 1, 1, active_player, out x1, out y1) + count_coins(x, y, -1, -1, active_player, out x2, out y2) + 1 >= 4)
            {
                x1 = x1 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y1 = y1 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;
                x2 = x2 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y2 = y2 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;

                return true;
            }
            else if (count_coins(x, y, 1, 0, active_player, out x1, out y1) + count_coins(x, y, -1, 0, active_player, out x2, out y2) + 1 >= 4)
            {
                x1 = x1 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y1 = y1 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;
                x2 = x2 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y2 = y2 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;

                return true;
            }
            else if (count_coins(x, y, 1, -1, active_player, out x1, out y1) + count_coins(x, y, -1, 1, active_player, out x2, out y2) + 1 >= 4)
            {
                x1 = x1 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y1 = y1 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;
                x2 = x2 * Convert.ToInt32(cell_width) + Convert.ToInt32(cell_width) / 2;
                y2 = y2 * Convert.ToInt32(cell_height) + Convert.ToInt32(cell_height) / 2;

                return true;
            }
            else
            {
                return false;
            }
        }

        // Get grid_y

        public static void get_grid_y(int x, out int grid_y)
        {
            grid_y = y_cells - 1;

            for (int i = 0; i < y_cells; i++)
            {
                if (grid_list[y_cells - 1 - i][x] == 0)
                {
                    break;
                }

                grid_y -= 1;
            }
        }

        // Get grid_x

        static void pix_to_grid(int pix_x, out int grid_x)
        {
            int cell_width = width / (x_cells);
            
            double gridx_ = pix_x / cell_width;
            grid_x = (int)Math.Ceiling(gridx_);

        }

        // NEW GAME BUTTON

        private void button1_Click(object sender, EventArgs e)
        {
            initialize_game();

            label1.Text = "Turn: player " + active_player;
            label2.Text = "";
            
        }

        // EXIT BUTTON

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

     
        // Change your color

            // Player 1:

        private void color_1_Click(object sender, EventArgs e)
        {
            DialogResult color_1_result = colorDialog1.ShowDialog();

            if (color_1_result == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                brush_1.Color = colorDialog1.Color;

                color_change = true;

                for (int i = 0; i < y_cells; i++)
                {
                    for (int j = 0; j < x_cells; j++)
                    {
                        if (grid_list[i][j] == 1)
                        {
                            draw_coin(j, i, brush_1, pen_1);
                        }
                    }
                }

                color_change = false;

                if (winner != 0)
                {
                    canvas.DrawLine(pen_2, x1, y1, x2, y2);
                }

            }
        }

            // Player 2:

        private void color_2_Click(object sender, EventArgs e)
        {
            DialogResult color_2_result = colorDialog2.ShowDialog();

            if (color_2_result == DialogResult.OK)
            {
                panel2.BackColor = colorDialog2.Color;
                brush_2.Color = colorDialog2.Color;

                color_change = true;

                for (int i = 0; i < y_cells; i++)
                {
                    for (int j = 0; j < x_cells; j++)
                    {
                        if (grid_list[i][j] == 2)
                        {
                            draw_coin(j, i, brush_2, pen_1);
                        }
                    }
                }

                color_change = false;

                if (winner != 0)
                {
                    canvas.DrawLine(pen_2, x1, y1, x2, y2);
                }
            }
        }


        // CANVAS MOUSE CLICK


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int mouse_x = e.X;

            if (is_grid_set == true)
            {
                pix_to_grid(mouse_x, out grid_x);
                get_grid_y(grid_x, out grid_y);

                if (winner == 0 && grid_y > -1)
                {
                    if (active_player == 1)
                    {
                        draw_coin(grid_x, grid_y, brush_1, pen_1);
                    }
                    else
                    {
                        draw_coin(grid_x, grid_y, brush_2, pen_1);
                    }


                    if (is_game_over(grid_x, grid_y) == true)
                    {
                        label2.Text = "Player " + active_player.ToString() + " Won!";
                        winner = active_player;
                        canvas.DrawLine(pen_2, x1, y1, x2, y2);
                        is_grid_set = false;
                    }

                    if (is_grid_set == true)
                    {
                        if (active_player == 1)
                        {
                            active_player = 2;
                            label1.Text = "Turn: player " + active_player;
                        }
                        else
                        {
                            active_player = 1;
                            label1.Text = "Turn: player " + active_player;
                        }
                    }

                }
            }
        }

        

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Refresh();

            initialize_game();

            label1.Text = "Turn: player " + active_player;
            label2.Text = "";

        }
    }
}
