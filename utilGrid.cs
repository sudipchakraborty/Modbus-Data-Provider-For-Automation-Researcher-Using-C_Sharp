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
//--------------------------------
namespace Tools
{
    public static class utilGrid
    {
        public static List<string> Fill_Cell_With_Combo()
        {
            List<string> lst = new List<string>();

            return lst;
        }
        //public static Color color_from_RBG_string(string str_rgb)
        //{
        //    Color c;

        //    try
        //    {
        //        var v = str_rgb.Split(',');
        //        int r = Convert.ToInt16(v[0]);
        //        int g = Convert.ToInt16(v[1]);
        //        int b = Convert.ToInt16(v[2]);

        //        c = Color.FromArgb(r, g, b);
        //    }
        //    catch(Exception ex)
        //    {
        //        string s = ex.ToString();
        //        c = Color.Black;
        //    }

        //    return c;

        //}
        //____________________________________________________________________________________________________
        public static void Fill_col_with_list_from_File(string f_name, DataGridView dg, int row, int col)
        {
            ArrayList lst = UtilFile.read(f_name);
            DataGridViewComboBoxCell combo = new DataGridViewComboBoxCell();
            combo.DataSource = lst;
            dg.Rows[row].Cells[col] = combo;
        }
        //____________________________________________________________________________________________________
        public static void grid_to_file(DataGridView dg, string filename)
        {
            try
            {
                // Choose whether to write header. Use EnableWithoutHeaderText instead to omit header.
                dg.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                // Select all the cells
                dg.SelectAll();
                // Copy selected cells to DataObject
                DataObject dataObject = dg.GetClipboardContent();
                // Get the text of the DataObject, and serialize it to a file
                File.WriteAllText(filename, dataObject.GetText(TextDataFormat.CommaSeparatedValue));
            }
            catch
            {
                MessageBox.Show("File Save Error");
            }

            //   MessageBox.Show("File Successfully saved");
        }
        //_____________________________________________________________________________________________________________________
        public static void file_to_grid(DataGridView dg, string file_name)
        {
            ArrayList arr = new ArrayList();
            arr = UtilFile.read(file_name);
            if (arr == null)
            {
                //MessageBox.Show("Unable to save File to Datagrid");
                return;
            }
            if (arr.Count == 0) return;

            dg.Rows.Add(arr.Count-1);

            for (int i = 1; i < arr.Count; i++)
            {
                var values = arr[i].ToString().Split(',');

                for (int j = 1; j < values.Length; j++)
                {
                    dg[j - 1, i - 1].Value = values[j];
                }
            }
            Fill_Grid_Index(dg);
        }
        //______________________________________________________________________________________________________________
        public static void set_full_row_select(DataGridView dg)
        {
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        //______________________________________________________________________________________________________________
        public static void set_datagrid_row_header_width(DataGridView dg, int no_of_char)
        {
            dg.RowHeadersWidth = dg.RowHeadersWidth + (7 * no_of_char);
        }
        //_________________________________________________________________________________________________________________
        public static void Fill_Grid_Index(DataGridView dg)
        {

            for (int i = 0; i < dg.Rows.Count; i++)
            {
                string s = i.ToString();
                dg.Rows[i].HeaderCell.Value = s;
            }
        }
        //_____________________________________________________________________________________________________________________
        public static String[] Get_Selected_Grid_Rows(DataGridView dg)
        {
            string[] rows = new string[dg.SelectedRows.Count];

            for (int i = dg.SelectedRows.Count - 1; i >= 0; i--)
            {
                rows[dg.SelectedRows.Count - 1 - i] = dg.SelectedRows[i].Index.ToString();
            }
            return rows;
        }
        //____________________________________________________________________________________________________________________
        public static void Datagrid_field_Checked(DataGridView dg, int col_index, string status)
        {
            try
            {
                for (int i = 0; i < dg.Rows.Count; i++)
                {
                    if (status == "1") dg[col_index, i].Value = "True";
                    else dg[col_index, i].Value = "False";
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }
        //________________________________________________________________________________________________________
        public static void Datagrid_field_uncheck_all(DataGridView dg, int col_index)
        {
            try
            {

                for (int i = 0; i < dg.Rows.Count; i++)
                {
                    dg[col_index, i].Value = "False";
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }
        //________________________________________________________________________________________________________
        public static String Duplicate_Data_Present_in_Grid(int c1, int c2, DataGridView dg)
        {
            String ret = "";
            for (int i = 0; i < dg.Rows.Count - 1; i++)
            {
                String ps, ns;

                ps = dg[c1, i].Value.ToString() + dg[c2, i].Value.ToString();
                ns = dg[c1, i + 1].Value.ToString() + dg[c2, i + 1].Value.ToString();

                if (ps == ns)
                {
                    ret = (i + 1).ToString();
                    break;
                }

            }
            return ret;
        }
        //___________________________________________________________________________________________________________
        public static String Get_Cell_status(DataGridView dg, int col, int row)
        {
            Color c = dg.Rows[row].Cells[col].Style.BackColor;
            if (c == Color.Red) return "Y";
            else
                return "N";
        }
        //_________________________________________________________________________________________________________
        public static void SaveDataGridViewToCSV(DataGridView dg, string filename)
        {
            try
            {
                // Choose whether to write header. Use EnableWithoutHeaderText instead to omit header.
                dg.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                // Select all the cells
                dg.SelectAll();
                // Copy selected cells to DataObject
                DataObject dataObject = dg.GetClipboardContent();
                // Get the text of the DataObject, and serialize it to a file
                File.WriteAllText(filename, dataObject.GetText(TextDataFormat.CommaSeparatedValue));
            }
            catch
            {
                MessageBox.Show("File Save Error");
            }

            //   MessageBox.Show("File Successfully saved");
        }
        //__________________________________________________________________________________________________
        public static void Display_Row_Header(DataGridView dg)
        {
            int rowNumber = 1;
            foreach (DataGridViewRow row in dg.Rows)
            {
                if (row.IsNewRow) continue;
                row.HeaderCell.Value = " " + rowNumber;
                rowNumber = rowNumber + 1;
            }
            dg.AutoResizeRowHeadersWidth(
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
        //_________________________________________________________________________________________________
        public static void SwapRows(DataGridView grid, int curr_row, String type)
        {
            int count = grid.Rows.Count - 2;

            if (type == "UP")
            {
                if (curr_row == 0) return;
                DataGridView dgv = grid;
                try
                {
                    int totalRows = dgv.Rows.Count;
                    // get index of the row for the selected cell
                    int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                    if (rowIndex == 0)
                        return;
                    // get index of the column for the selected cell
                    int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                    DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                    dgv.Rows.Remove(selectedRow);
                    dgv.Rows.Insert(rowIndex - 1, selectedRow);
                    dgv.ClearSelection();
                    dgv.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
                }
                catch 
                {

                }
            }
            if (type == "DN")
            {
                if (curr_row >= count) return;
                DataGridView dgv = grid;
                try
                {
                    int totalRows = dgv.Rows.Count;
                    // get index of the row for the selected cell
                    int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                    if (rowIndex == totalRows - 1)
                        return;
                    // get index of the column for the selected cell
                    int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                    DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                    dgv.Rows.Remove(selectedRow);
                    dgv.Rows.Insert(rowIndex + 1, selectedRow);
                    dgv.ClearSelection();
                    dgv.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
                }
                catch { }
            }
        }
        //_________________________________________________________________________________________________
        public static bool Str_Not_Found(DataGridView dg, int col, String find_str)
        {
            int length = dg.Rows.Count;
            String s = "";

            for (int i = 0; i < length; i++)
            {
                try
                {
                    s = dg[col, i].Value.ToString();
                }
                catch { }
                if (s == find_str)
                {
                    return false;
                }
            }
            return true;
        }
        //_________________________________________________________________________________________________
        public static bool Number_not_Exists(DataGridView dg, String number)
        {
            int irow = dg.Rows.Count;
            String s;

            for (int i = 0; i < irow - 1; i++)
            {
                try
                {
                    s = dg[0, i].Value.ToString();
                }
                catch
                {
                    s = "";
                }

                if (s == number)
                {
                    return false;
                }
            }
            return true;
        }
        //_________________________________________________________________________________________________
        public static void Fill_With_Uniqe_Random_number(DataGridView dg)
        {
            bool exit = false;
            while (!exit)
            {
                String s =utility.Get_Random_Uint16_as_String();
                if (Number_not_Exists(dg, s))
                {
                    int i = dg.Rows.Count;
                    dg[0, i - 1].Value = s;
                    exit = true;
                    break;
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------
        public static void GridRow_Paste(DataGridView dg, int iRowStart, int iRowEnd, int paste_Loc)
        {
            int col_length = dg.ColumnCount;
            int diff = iRowEnd - iRowStart;
            diff++;

            String[] arr = new string[col_length * diff];// *iRowEnd-iRowStart
            int ptr = 0;

            int j = iRowStart;
            do
            {
                for (int i = 0; i < col_length; i++)
                {
                    try
                    {
                        arr[ptr] = dg[i, j].Value.ToString();
                    }
                    catch
                    {
                        arr[ptr] = "";
                    }
                    ptr++;
                }
                j++;
            } while (j <= iRowEnd);
            //--------------------------------------------------


            // (2)-----create a new row-------------------------
            dg.Rows.Insert(paste_Loc, diff);
            //--------------------------------------------------


            // (3) revert back data to new row------------------
            ptr = 0;

            j = paste_Loc;
            do
            {
                for (int i = 0; i < col_length; i++)
                {
                    dg[i, j].Value = arr[ptr];
                    ptr++;
                }
                j++;
            } while (j < paste_Loc + diff);
            //--------------------------------------------------
        }
        //==============================================================================
        public static int Delete_Selected(DataGridView dg)
        {
            int retInt = 0;

            for (int i = 0; i < dg.RowCount; i++)
            {
                if (dg.Rows[i].Selected)
                {
                    dg.Rows.RemoveAt(i);
                    retInt++;
                }
            }
            return retInt;
        }
        //=========================================================================
        public static int Get_Selection_Start_End(DataGridView dg, ref int start, ref int end)
        {
            int retInt = 0;
            bool st_captured = false;

            for (int i = 0; i < dg.RowCount; i++)
            {
                if (dg.Rows[i].Selected)
                {
                    if (!st_captured)
                    {
                        start = i;
                        st_captured = true;
                    }
                    retInt++;
                }
            }
            end = start + retInt - 1;
            return retInt;
        }
        //=========================================================================
        public static void GridRow_Copy_Paste(DataGridView dg, int copy_row, int paste_row)
        {
            int col_length = dg.ColumnCount;

            String[] arr = new string[col_length];

            // (1)--------copy source row-----------------------
            for (int i = 0; i < col_length; i++)
            {
                arr[i] = dg[i, copy_row].Value.ToString();
            }
            //--------------------------------------------------


            // (2)-----create a new row-------------------------
            dg.Rows.Insert(paste_row, 1);
            //--------------------------------------------------

            // (3) revert back data to new row------------------
            for (int i = 0; i < col_length; i++)
            {
                dg[i, paste_row].Value = arr[i];
            }
            //--------------------------------------------------

        }
        //--------------------------------------------------   
        public static void Fill_Grid_With_Colour(DataGridView dg, Color c)
        {
            for (int j = 0; j < dg.Columns.Count; j++)
            {
                for (int i = 0; i < dg.Rows.Count; i++)
                {
                    dg.Rows[i].Cells[j].Style.BackColor = c;
                }                
            }
        }

        public static Bitmap Get_Grid_Snapshot(DataGridView dg)
        {
            int DGVOriginalHeight = dg.Height;
            Bitmap bitmap = new Bitmap(dg.Width, dg.Height);
            dg.DrawToBitmap(bitmap, new Rectangle(Point.Empty, dg.Size));
            return bitmap;
        }

        public static void insert(DataGridView dg)
        {
            int rowindex = dg.CurrentRow.Index;
            dg.Rows.Insert(rowindex);
        }

        public static string Select_Next_Row(DataGridView dg,string running_row)
        {         
            int r = dg.Rows.Count;
            int running_index = Convert.ToInt32(running_row);
            dg.Rows[running_index].Selected = true;
            dg.DefaultCellStyle.SelectionBackColor = Color.White;
            dg.ClearSelection();
          
            if (running_index < r - 1)
            {
                running_index++;
                dg.Rows[running_index].Selected = true;
                dg.DefaultCellStyle.SelectionBackColor = Color.Blue;
                return running_index.ToString();
            }
            else
            {
               
                return "0";              
            }
            
        }

        public static void Highlight_Current_Row(DataGridView dg,Color c)
        {
            dg.DefaultCellStyle.SelectionBackColor = c;
        }

        public static void Highlight_Current_Row_Blue(DataGridView dg)
        {
            dg.DefaultCellStyle.SelectionBackColor = Color.Blue;
        }



    }
}