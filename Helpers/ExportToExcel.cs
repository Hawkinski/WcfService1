using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Configuration;
namespace Helpers
{
    public class ExportToExcel
    {        
        //Currently this is in used.
        #region -- Export To Excel using (Microsoft.Office.Interop.Excel) With Formatting --
        public static void ExportToExcelFile_Formatted(string FileName, string StrQry, string HeaderName, string[] ArrColumnNames, string ColChar,System.Data.DataTable DTable=null,string CompName="",string SubHeader="")
        {
            string strFilePath = ConfigurationManager.AppSettings["ExportReportPath"].ToString();        
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            string strFileLocation = strFilePath + "\\" + FileName + ".xlsx";

            ColChar = GetExcelColumnName(DTable.Columns.Count + 1);
            ArrColumnNames[0] = "LNo";
            for(int j=0; j<=DTable.Columns.Count-1;j++)
            {
                ArrColumnNames[j+1] = DTable.Columns[j].ToString();
            }      
            if (strFileLocation != "")
            {               
                ArrayList arrHeaderList = new ArrayList();
                ArrayList arrHeaderIDList = new ArrayList();
                Application xlExcel = new Application();
                Workbooks xlBooks;
                Workbook xlBook;
               
                int iRowCtr = 1;
                int cnt=0;               
                System.Data.DataTable dtData;   
                            
                try
                {
                    xlExcel.DisplayAlerts = false;
                    xlExcel.SheetsInNewWorkbook = 1;
                    xlExcel.Workbooks.Add().Worksheets[1].Select();   // For displaying the column name in the the excel file.  
                    xlBooks = xlExcel.Workbooks;
                    xlBook = xlBooks.Item[1];

                    // Company Name
                    if (!string.IsNullOrEmpty(CompName))
                    {                        
                        xlExcel.Cells[iRowCtr, 1].Value = CompName;
                        iRowCtr++;
                    }
                    // Sub Header
                    if (!string.IsNullOrEmpty(SubHeader))
                    {
                        xlExcel.Cells[iRowCtr, 1].Value = SubHeader;
                        iRowCtr++;
                    }
                    // Report Caption
                    xlExcel.Cells[iRowCtr, 1].Value = HeaderName;
                   // xlExcel.Cells(iRowCtr, 1).Align = "Center";
                    
                    // Report Field header
                    iRowCtr = (iRowCtr + 2);
                    if (!string.IsNullOrEmpty(SubHeader))
                    {
                        iRowCtr += 2;
                    }
                    for (int i = 1; i <= (ArrColumnNames.Length - 1); i++)
                    {
                        xlExcel.Cells[iRowCtr, i].Value = ArrColumnNames[i - 1];
                    }

                    // iRowCtr = iRowCtr + 2
                    // If StrQry <> "" Then
                    //     dtData = GetDataTable(StrQry)
                    // Else
                    dtData = DTable;
                    //  End If
                    // 

                    int Count = dtData.Rows.Count;
                    if (dtData.Rows.Count > 0)
                    {
                        int colIndex = 0;
                        int rowIndex = 0;
                        rowIndex = (iRowCtr - 1);
                        cnt = 0;
                        foreach (DataRow dr in dtData.Rows)
                        {
                            cnt = (cnt + 1);
                            rowIndex = (rowIndex + 1);
                            colIndex = 1;
                            xlExcel.Cells[(rowIndex + 1), colIndex] = cnt;
                            foreach (DataColumn dc in dtData.Columns)
                            {
                                colIndex = (colIndex + 1);
                                xlExcel.Cells[(rowIndex + 1), colIndex] = dr[dc.ColumnName];
                            } 
                        }
                    }
                    xlExcel.Cells.Select();
                    xlExcel.Selection.Font.Size = 8;
                    xlExcel.Selection.Font.Name = "Trebuchet MS";

                    int rowIdx = 1;
                    if (!string.IsNullOrEmpty(CompName))
                    {
                        // Company Name Formatting
                        xlExcel.Range["A" + rowIdx.ToString() + ":" + ColChar + rowIdx.ToString()].Select();
                        xlExcel.Selection.VerticalAlignment = -4160; ;
                        xlExcel.Selection.HorizontalAlignment = -4108;
                        xlExcel.Selection.Font.Bold = true;
                        xlExcel.Selection.Font.Size = 14;
                        xlExcel.Selection.Font.Color = System.Drawing.Color.Red;
                        xlExcel.Selection.MergeCells = true;                  
                        xlExcel.Rows[rowIdx + ":" + rowIdx].RowHeight = 28.5;
                        xlExcel.Selection.Interior.ColorIndex = 15;
                        rowIdx++;
                    }
                    if (!string.IsNullOrEmpty(SubHeader))
                    {
                        // Sub Header Formatting
                        xlExcel.Range["A" + rowIdx.ToString() + ":" + ColChar + rowIdx.ToString()].Select();
                        xlExcel.Selection.Font.Bold = true;
                        xlExcel.Selection.Font.Size = 14;
                        xlExcel.Selection.MergeCells = true;
                        xlExcel.Selection.VerticalAlignment = -4160; ;
                        xlExcel.Selection.HorizontalAlignment = -4108;
                        xlExcel.Rows[rowIdx + (":" + rowIdx)].RowHeight = 28.5;
                        xlExcel.Selection.Interior.ColorIndex = 15;
                        rowIdx++;
                    }

                    // Header Formatting
                    xlExcel.Range["A" + rowIdx.ToString() + ":" +ColChar + rowIdx.ToString()].Select();
                    xlExcel.Selection.Font.Size = 14;
                    xlExcel.Selection.MergeCells = true;
                    xlExcel.Selection.VerticalAlignment = -4160; ;
                    xlExcel.Selection.HorizontalAlignment = -4108;
                    xlExcel.Rows[rowIdx + ":" + rowIdx].RowHeight = 23.25;
                    xlExcel.Selection.Interior.ColorIndex = 15;
                    rowIdx += 2;

                    if (!string.IsNullOrEmpty(SubHeader))
                    {
                        rowIdx += 2;
                    }       
                               
                    xlExcel.Range["A" + rowIdx.ToString() + ":" + ColChar + rowIdx.ToString()].Select();
                    xlExcel.Selection.Font.Bold = true;
                    xlExcel.Rows[rowIdx + (":" + rowIdx)].RowHeight = 17.25;
                    xlExcel.Selection.Interior.ColorIndex = 34;   
                                   
                    xlExcel.Range["A" + rowIdx.ToString() + ":" + ColChar + (cnt+rowIdx - 1).ToString()].Select();      
                    xlExcel.Selection.entireColumn.AutoFit();
                    Range rng;
                    rng = xlExcel.Range["A" + rowIdx.ToString() + ":"+ ColChar + (cnt + rowIdx + 1).ToString()]; 
                    rng.Borders.LineStyle = XlLineStyle.xlContinuous;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Borders.Weight = XlBorderWeight.xlThin;

                    xlExcel.Cells[(cnt+ rowIdx + 1), 4].Value = "GRAND TOTAL";
                    for (int ci = 0; ci <= dtData.Columns.Count - 1; ci++)
                    {
                        if (dtData.Columns[ci].DataType.ToString().ToUpper().Contains("INT") || (dtData.Columns[ci].DataType.ToString().ToUpper().Contains("NUMERIC") || (dtData.Columns[ci].DataType.ToString().ToUpper().Contains("DECIMAL") || (dtData.Columns[ci].DataType.ToString().ToUpper().Contains("FLOAT") || dtData.Columns[ci].DataType.ToString().ToUpper().Contains("DOUBLE")))))
                        {
                            string _XlColName = GetExcelColumnName(ci + 2);                          
                            string _TblColName = dtData.Columns[ci].ColumnName.ToUpper();
                            xlExcel.Range[_XlColName + rowIdx.ToString() + ":" + _XlColName + (cnt + rowIdx).ToString()].Select();                  
                            xlExcel.Selection.NumberFormat = "##################################,###.00";
                            try
                            {                              
                                if (!_TblColName.Contains("DYS"))
                                {
                                    switch (_TblColName)
                                    {
                                        case "STANDARD HOURS":
                                            break;
                                        case "OTR":
                                            break;
                                        default:
                                            Decimal _Sum = 0;
                                            for (int r = 0; r <= (dtData.Rows.Count - 1); r++)
                                            {
                                                if (!string.IsNullOrEmpty(dtData.Rows[r][ci].ToString()))
                                                {
                                                    try
                                                    {
                                                        _Sum = (_Sum + Convert.ToDecimal(dtData.Rows[r][ci]));
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }
                                            xlExcel.Cells[cnt+ (rowIdx + 1), (ci + 2)].Value = _Sum;
                                            xlExcel.Cells[cnt + (rowIdx + 1), (ci + 2)].NumberFormat = "##################################,###.00";
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                    // BKP|As per FARNEK Requirement Added Footer info 
                    
                        xlExcel.Cells[cnt + rowIdx + 10, 1].Value = "Prepared by :";
                        xlExcel.Cells[cnt + rowIdx + 11, 1].Value = "Payroll Officer";
                        xlExcel.Cells[cnt + rowIdx + 10, 4].Value = "Recommended by :";
                        xlExcel.Cells[cnt + rowIdx + 11, 4].Value = "Head of HR and OD";
                        xlExcel.Cells[cnt + rowIdx + 10, 6].Value = "Verified by :";
                        xlExcel.Cells[cnt + rowIdx + 11, 6].Value = "Finance Division";
                        xlExcel.Cells[cnt + rowIdx + 10, 9].Value = "Approved by :";
                        xlExcel.Cells[cnt + rowIdx + 11, 9].Value = "Division Manager";
                        xlExcel.Cells[cnt + rowIdx + 10, 12].Value = "Approved by :";
                        xlExcel.Cells[cnt + rowIdx + 11, 12].Value = "Finance Manager";
                        xlExcel.Cells[cnt + rowIdx + 10, 15].Value = "Approved by : Markus(Oberlin)";
                        xlExcel.Cells[cnt + rowIdx + 11, 15].Value = "Chief Executive Officer (CEO )";
                    
                        rng = xlExcel.Range["A" + (cnt + rowIdx + 10).ToString() + ":" + ColChar + (cnt + rowIdx + 11).ToString()];
                        rng.Font.Bold = 10;
                    
                    // For Saving Excel file in strFileLocation
                    xlBook.SaveAs(strFileLocation);
                    xlBook.Close();
                    xlBook = null;
                    GC.Collect();

                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";  // "application/vnd.ms-excel"; //xls  // For xlsx, use: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
                    HttpContext.Current.Response.AddHeader(String.Format("content-disposition", "attachment; filename={0}"), Path.GetFileName(strFileLocation));
                    HttpContext.Current.Response.TransmitFile(strFileLocation);
                    HttpContext.Current.Response.End();
                }
                catch (Exception ex)
                {                   
                }
            }
        }
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = ((dividend - 1) % 26);
                columnName = (Convert.ToChar(65 + modulo).ToString() + columnName);
                dividend = ((dividend - modulo)/26);             
            }
            return columnName;
        }
        #endregion

        //This works fine but not in used right now
        #region -- Export To Excel using (ClosedXML) Without Formatting --
        public static void ExportToExcelFile(System.Data.DataTable dt, string CompName, string SubHeader, string ExlHeader, string fileName = "Report")
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(fileName);

                //wb.Worksheets.Add(dt, fileName);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                if (!string.IsNullOrEmpty(CompName))
                {
                    ws.Cell(1, 1).Value = CompName;
                    // ws.Cell(1, 1).IsMerged()=true;
                    //      .HorizontalAlignment = -4108
                    //.VerticalAlignment = -4160
                    //.MergeCells = True
                    //.Font.Size = 14
                    //.Font.Bold = True
                    //.Font.Color = Color.Red

                    // iRowCtr += 1
                }
                if (!string.IsNullOrEmpty(SubHeader))
                {
                    ws.Cell(2, 1).Value = SubHeader;
                    ws.Cell(2, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.CenterContinuous);
                    // iRowCtr += 1
                }
                ws.Cell(3, 1).Value = ExlHeader;
                ws.Cell(3, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.CenterContinuous);
                //ws.Cell(3, 1).Style.Alignment = "Center"
                ws.Rows(1, 3).Style.Fill.BackgroundColor = XLColor.Gray;
                ws.Rows(1, 3).Style.Font.FontSize = 20;

                ws.Cell(6, 1).InsertTable(dt);


                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename= " + ExlHeader + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion
    }
}
