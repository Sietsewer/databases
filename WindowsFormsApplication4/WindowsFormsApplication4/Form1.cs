using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data.SqlClient;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        /* USES IN FORM:
         * DataGridView dataGridView1
         * DataGridView dataGridView2
         * 
         * Change directories, or add explorer.
         */
        private String dirOne = "D:\\Projecten\\databases\\databases\\ledenEnBoetes.txt";
        private String dirTwo = "D:\\Projecten\\databases\\databases\\teamsEnWedstrijden.txt";
        private String[,] gridOne;// [ column , row ]
        private String[,] gridTwo;

        private List<String> leden;
        private List<String> boetes;
        private List<String> teams;
        private List<String> wedstrijden;
       
        //private const string DBCONNECTIONSTRING = @"D:\Projecten\databases\databases\WindowsFormsApplication4\WindowsFormsApplication4\Sportclub.mdf";

        public Form1()
        {
            // Init lists
            leden = new List<string>();
            boetes = new List<string>();
            teams = new List<string>();
            wedstrijden = new List<string>();

            InitializeComponent();
            // Split text (file) to 2d String array.
            gridOne = fileToGrid(File.ReadAllText(dirOne));
            gridTwo = fileToGrid(File.ReadAllText(dirTwo));
            // Convert 2d String array to DataGridView.
            gridToDataGridView(gridOne, ref dataGridView1);
            gridToDataGridView(gridTwo, ref dataGridView2);

            for (int i = 0; i <= 7; i++)
            {
                leden.Add(gridOne[i, 0].Trim('"'));
            }
            for (int i = 8; i < gridOne.GetLength(0); i++)
            {
                boetes.Add(gridOne[i, 0].Trim('"'));
            }
            for (int i = 0; i <= 9; i++)
            {
                teams.Add(gridTwo[i, 0].Trim('"'));
            }
            for (int i = 10; i < gridTwo.GetLength(0); i++)
            {
                wedstrijden.Add(gridTwo[i, 0].Trim('"'));
            }

            foreach (String s in getColumn("naam"))
            {
                textField.AppendText(s + ", ");
            }

            /*textField.AppendText(Environment.NewLine);
            foreach (String s in boetes)
            {
                textField.AppendText(s + " ");
            }
            textField.AppendText(Environment.NewLine);
            foreach (String s in teams)
            {
                textField.AppendText(s + " ");
            }
            textField.AppendText(Environment.NewLine);
            foreach (String s in wedstrijden)
            {
                textField.AppendText(s + " ");
            }
            textField.AppendText(Environment.NewLine);*/

        }

        private List<String> getColumn(String name)
        {
            List<String> l = new List<string>();
            for (int i = 0; i < gridOne.GetLength(0); i++)
            {
                if (gridOne[i, 0].Trim('"') == name.Trim('"'))
                {
                    for (int j = 1; j < gridOne.GetLength(1); j++)
                    {
                        l.Add(gridOne[i, j]);
                        
                    }
                }
            }
            for (int i = 0; i < gridTwo.GetLength(0); i++)
            {
                if (gridTwo[i, 0].Trim('"') == name.Trim('"'))
                {
                    for (int j = 1; j < gridTwo.GetLength(1); j++)
                    {
                        l.Add(gridTwo[i, j]);
                        
                    }
                }
            }
            return l;
        }

        private void gridToDataGridView(String[,] grid, ref DataGridView dataGridView)
        {   // DataGridView made to size.
            dataGridView.ColumnCount = grid.GetLength(0);
            dataGridView.RowCount = grid.GetLength(1) - 1;
            for (int i = 0; i < grid.GetLength(0); i++)
            {   // Firs row contains column header names, which are assigened seperately.
                dataGridView.Columns[i].HeaderText = grid[i, 0];
            }
            for (int y = 0; y < grid.GetLength(1) - 1; y++)// First row is column names, one less row is iterated.
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    dataGridView.Rows[y].Cells[x].Value = grid[x, y + 1];// Offset so that first row is skipped.
                }
            }
        }

        private String[,] fileToGrid(String file)
        {
            String[] records = file.Split('\n');// String split on LineFeed, the last character after each record.
            int columnCount = records[0].Split(',').Count();
            String[,] grid = new String[columnCount, records.Count() - 1];
            for (int y = 0; y < records.Count() - 1; y++)// Ensures last element is skipped, which is empty.
            {   // Each row split on comma, the character between each cell.
                String[] cells = records[y].Split(',');
                for (int x = 0; x < columnCount; x++)
                {
                    grid[x, y] = cells[x];
                }
            }
            return grid;
        }
    }
}
