using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;


namespace Tools
{
    public static class UtilFile
    {
        //______________________________________________________________________________________________________________
        public static string Get_FileName_from_Save_As()
        {
            string path = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save As";
            //      saveFileDialog1.CheckFileExists = true;
            //     saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "ws";
            saveFileDialog1.Filter = "Work Space files(*.ws)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;               
            }
            return path;
        }
        //______________________________________________________________________________________________________________
        public static string Save_As(List<string> save_data)
        {
            string path = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save As Workspace";
            //      saveFileDialog1.CheckFileExists = true;
            //     saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "ws";
            saveFileDialog1.Filter = "Work Space files(*.ws)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                System.IO.File.WriteAllLines(path, save_data);
            }

            return path;
        }
        //______________________________________________________________________________________________________________
        public static string Get_File_Name_From_DialogBox()
        {
            String Fname = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                Fname = openFileDialog1.FileName;
            }
            return Fname;
        }

        //_____________________________________________________________________________________________________________________
        public static ArrayList read(string file_name)
        {
            ArrayList bfr = new ArrayList();

            try
            {
                using (var reader = new StreamReader(file_name))
                {
                    //  var line;// = reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        bfr.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                bfr = null;
            }

            return bfr;
        }
        //_____________________________________________________________________________________________________________________
        public static bool File_Not_Found(string file_name)
        {
            try
            {
                var reader = new StreamReader(file_name);
                return false;
            }
            catch (Exception ex)
            {

            }
            
            return true;
        }
        //____________________________________________________________________________________________
        public static string[,] File_Read_csv(string file_name)
        {

            string[,] bfr = new string[80, 8];

            try
            {
                using (var reader = new StreamReader(file_name))
                {
                    int index = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        bfr[index, 0] = values[0];
                        bfr[index, 1] = values[1];
                        bfr[index, 2] = values[2];
                        bfr[index, 3] = values[3];
                        bfr[index, 4] = values[4];
                        bfr[index, 5] = values[5];
                        bfr[index, 6] = values[6];
                        bfr[index, 7] = values[7];
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return bfr;

        }
        //________________________________________________________________________________________________________
        public static DataGridViewCell GetCellWhereTextExistsInGridView(string searchText, DataGridView dataGridView, int columnIndex)
        {
            DataGridViewCell cellWhereTextIsMet = null;

            // For every row in the grid (obviously)
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                // I did not test this case, but cell.Value is an object, and objects can be null
                // So check if the cell is null before using .ToString()
                if (row.Cells[columnIndex].Value != null && searchText == row.Cells[columnIndex].Value.ToString())
                {
                    // the searchText is equals to the text in this cell.
                    cellWhereTextIsMet = row.Cells[columnIndex];
                    break;
                }
            }

            return cellWhereTextIsMet;
        }
        //___________________________________________________________________________________
        public static int Get_row_index_by_str(string searchText, DataGridView dataGridView, int columnIndex)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[columnIndex].Value != null && searchText == row.Cells[columnIndex].Value.ToString())
                {
                    return row.Index;
                }
            }
            return -1;
        }
        //___________________________________________________________________________________
        public static void review_gid_row_colour(ArrayList cmd, ArrayList colour, int find_col, DataGridView dg)
        {
            foreach (DataGridViewRow row in dg.Rows)
            {
                string s = row.Cells[find_col].Value.ToString();

                int indexItem = cmd.IndexOf(s);
                if (indexItem != -1)
                {
                    string str_clr = colour[indexItem].ToString();
                    var v = str_clr.Split('#');
                    int r = Convert.ToInt16(v[0]);
                    int g = Convert.ToInt16(v[1]);
                    int b = Convert.ToInt16(v[2]);
                    Color c = Color.FromArgb(r, g, b);
                    row.DefaultCellStyle.ForeColor = c;
                }
            }
        }
        //________________________________________________________________________________________________________
        public static bool Path_Exits(string f_path)
        {
            bool possiblePath = f_path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            return possiblePath;
        }
        //________________________________________________________________________________________________________
        public static void Save_ListBox_to_File(ListBox lb, string f_name)
        {
            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(f_name);
            foreach (var item in lb.Items)
            {
                SaveFile.WriteLine(item);
            }
            SaveFile.Close();
        }
        //________________________________________________________________________________________________________
        public static void Load_ListBox_from_File(ListBox lst, string file_name)
        {
            lst.Items.Clear();
            using (StreamReader r = new StreamReader(file_name))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lst.Items.Add(line);
                }
            }
        }

        public static List<string> Load_ListBox_from_File(string file_name)
        {
            List<string> lines = new List<string>();
            using (StreamReader r = new StreamReader(file_name))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
        return lines;
        }

        public static void Load_ListBox_from_File(string file_name, ref ListBox lst)
        {
            lst.Items.Clear();
            using (StreamReader r = new StreamReader(file_name))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lst.Items.Add(line);
                }
                r.Close();
            }        
        }

    public static void Save_PictureBox(PictureBox pb)
        {
            string f_name = Get_FileName_from_Save_As();
            pb.Image.Save(@f_name, ImageFormat.Jpeg);
        }

        public static void Save_List_String_to_File(List<string> lst, string f_name)
        {
            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(f_name);
            foreach (string ls in lst)
            {
                SaveFile.WriteLine(ls);
            }
            SaveFile.Close();
        }

        public static List<string> Get_List_String_from_File(string f_name)
        {   
            List<string> lines = new List<string>();

            using (StreamReader r = new StreamReader(f_name))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;            
        }


        public static void Save_List_Image_to_File(List<Image> img, List<string> f_name)
        {
            int index = 0;
            foreach (Image im in img)
            {
                if (im != null)
                {
                    im.Save(f_name[index], ImageFormat.Bmp);
                    index++;
                }
            }
        }

        public static List<string> Get_List_Image_from_File(string f_name)
        {
            List<string> lines = new List<string>();

            using (StreamReader r = new StreamReader(f_name))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }



    }// class
}// namespace
