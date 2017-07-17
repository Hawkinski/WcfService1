using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using Itenso.TimePeriod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Helpers
{
    public class Helper
    {
        public static string dep = "";
        public static DataTable dt;

        public int getDateDiffInDays(string str1, string str2)
        {
            DateTime d1 = Convert.ToDateTime(str1);
            DateTime d2 = Convert.ToDateTime(str2);


            TimeSpan difference = d1 - d2;
            DateDiff dateDiff = new DateDiff(d1, d2);

            int days = dateDiff.Days;

            if (days < 0)
            {
                days = days * (-1);
                days += 1;
            }
            return days;
        }

        public int getDateDiffInDays2(string str1, string str2)
        {
            DateTime d1 = Convert.ToDateTime(str1);
            DateTime d2 = Convert.ToDateTime(str2);


            TimeSpan difference = d1 - d2;
            DateDiff dateDiff = new DateDiff(d1, d2);

            int days = dateDiff.Days;

            
            return days;
        }
        public string getDateDifference(DateTime d1, DateTime d2)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int year;
            int month;
            int day;
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;

            }

            int increment = 0;
            //days calculation 
            if (fromDate > toDate)
            {
                increment = monthDay[fromDate.Month - 1];

            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }

            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;

            }

            ///Month calculation
            ///
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;

            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;

            }
            //Year calculation
            year = toDate.Year - (fromDate.Year + increment);
            string y = "Year";
            string m = "Month";
            string d = "Day";
            if (year > 1)
            {
                y = "Years";
            }
            if (month > 1)
            {
                m = "Months";
            }
            if (day > 1)
            {
                d = "Days";
            }

            return year + y + ", " + month + m + ", " + day + d;
        }
        public string getDateDiffInDaysAndMonths(DateTime d1, DateTime d2)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int year;
            int month;
            int day;
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;

            }

            int increment = 0;
            //days calculation 
            if (fromDate > toDate)
            {
                increment = monthDay[fromDate.Month - 1];

            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }

            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;

            }

            ///Month calculation
            ///
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;

            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;

            }
            //Year calculation
            year = toDate.Year - (fromDate.Year + increment);
            string y = "Year";
            string m = "Month";
            string d = "Day";
            if (year > 1)
            {
                y = "Years";
            }
            if (month > 1)
            {
                m = "Months";
            }
            if (day > 1)
            {
                d = "Days";
            }

            return year.ToString()+"," +  month.ToString()+"," + day.ToString();
        }
        public string getDDinYM(DateTime d1, DateTime d2)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int year;
            int month;
            int day;
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;

            }

            int increment = 0;
            //days calculation 
            if (fromDate > toDate)
            {
                increment = monthDay[fromDate.Month - 1];

            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }

            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;

            }

            ///Month calculation
            ///
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;

            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;

            }
            //Year calculation
            year = toDate.Year - (fromDate.Year + increment);
            string y = "Year";
            string m = "Month";
            string d = "Day";
            if (year > 1)
            {
                y = "Years";
            }
            if (month > 1)
            {
                m = "Months";
            }
            if (day > 1)
            {
                d = "Days";
            }

            return year + y + ", " + month + m;
        }
        public string formatDate(object date1)
        {
            string date = date1.ToString();

            if (string.IsNullOrEmpty(date)) return date;

            string[] str = date.Split(' ');
            string d1 = str[0];

            return d1;

        }
        public string ConvertMYtoSQLType(string monthYear)
        {
           if(string.IsNullOrEmpty(monthYear))
            {
                return monthYear;
            }
           else
            {
                string[] temp = monthYear.Split('/');
                return temp[1] + temp[0];
            }          

        }
        public string ConvertDateToSqlType(string strDate)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                return strDate;
            }
            else
            {
                string[] dateArr= strDate.Split(' ');
                string[] temp = dateArr[0].Split('/');
                return temp[2]+"-"+temp[1] +"-"+ temp[0];
            }

        }
        public DateTime? ConverToDateTime(object obj)
        {
            if (obj == null)
                return null;
            else
                return DateTime.Parse(obj.ToString());
        }
        public string formatMoney(string tempStr)
        {


            double d = Convert.ToDouble(tempStr);
            d = Math.Round(d);

            tempStr = d.ToString();
            string str2 = "No Data Available";
            string[] temp = tempStr.Split('.');
            string temp2 = temp[0];
            int i = temp2.Length;
            int _const = i;
            string temp3 = "";

            if (i < 3)
            {
                str2 = tempStr;

            }

            else if (i == 3)
            {
                str2 = tempStr;

            }
            else if (i > 3)
            {
                i = i - 3;
                if (i == 1)
                {
                    str2 = tempStr.Insert(i, ",");

                }
                else if (i > 1)
                {
                    if (i % 2 == 0)
                    {
                        int count = 0;

                        int x = 2;
                        Console.WriteLine(x.ToString());

                        while (x <= i)
                        {
                            Console.WriteLine("BEFORE " + tempStr);
                            temp3 = tempStr.Insert(x + count, ",");
                            tempStr = temp3;
                            count++;
                            x = x + 2;


                        }

                        str2 = tempStr;
                    }
                    else if (i % 2 != 0)
                    {
                        Console.WriteLine("ODD");
                        int count = 2;
                        int y = i - 1;
                        temp3 = tempStr.Insert(1, ",");
                        tempStr = temp3;
                        int x = 2;
                        Console.WriteLine(x.ToString());

                        while (x <= y)
                        {
                            Console.WriteLine("BEFORE " + tempStr);
                            temp3 = tempStr.Insert(x + count, ",");
                            tempStr = temp3;
                            count++;
                            x = x + 2;


                        }

                        str2 = tempStr;

                    }



                }


            }
            return str2;
        }
        public string formatedDate(string date)
        {
            if (string.IsNullOrEmpty(date)) return date;

            string[] str = date.Split(' ');
            string d1 = str[0];
            DateTime dt = (Convert.ToDateTime(date));
            string day = dt.Day.ToString();
            string month = dt.Month.ToString();
            month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int32.Parse(month));

            string year = dt.Year.ToString();


            return day + "," + month + "," + year;


        }
        public static string encode(string _value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encode = new System.Text.ASCIIEncoding();
            var combined = encode.GetBytes(_value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");

        }
        public string convertToTitleCase(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string[] words = s.Split(' ');
            string finalStr = string.Empty;

            for (int i = 0; i < words.Length; i++)
            {
                string str = words[i];
                if (!string.IsNullOrEmpty(str))
                {
                    string temp = str.ToLower();
                    string firstChar = temp.Substring(0, 1);
                    string restChar = temp.Substring(1);
                    string f = firstChar.ToUpper() + restChar;

                    finalStr = finalStr + f;

                    if (i != (words.Length - 1))
                    {
                        finalStr = finalStr + " ";
                    }
                }

            }

            return finalStr;

        }
        public string NS(object obj)
        {
            string _retVal = "";
            if (obj == null || obj == DBNull.Value)
            {
                _retVal = string.Empty;
            }
            else
            {
                _retVal = obj.ToString();
            }

            return _retVal;

        }
        public string currencyFormat(object obj1)
        {
            double obj = Convert.ToDouble(obj1);
            obj = Math.Round(obj);

            string _retVal =string.Empty;
            _retVal = obj.ToString( obj % 1 == 0 ? "N0" : "N2");

            return _retVal;

        }
        public int NZ(object obj)
        {
            int dec = 0;
            if (obj == null || obj == DBNull.Value)
            {
                dec = 0;
            }
            else
            {
                dec = Convert.ToInt32(obj);

            }
            return dec;
        }
        public float NF(object obj)
        {
            float dec = 0;
            if (obj == null || obj == DBNull.Value)
            {
                dec = 0;
            }
            else
            {
                dec = float.Parse(obj.ToString());
                dec.ToString("#.##");
            }
            return dec;
        }
        public decimal ND(object obj)
        {
            decimal dec = 0;
            if (obj == null || obj == DBNull.Value)
            {
                dec = 0;
            }
            else
            {
                dec = Convert.ToDecimal(obj);

            }
            return dec;
        }
        public double NDO(object obj)
        {
            double dec = 0;
            if (obj == null || obj == DBNull.Value|| obj == "")
            {
                dec = 0;
            }
            else
            {
                dec = Convert.ToDouble(obj);
            }
            return dec;
        }
        public decimal convertToDecimal(object obj)
        {
            decimal d = 0;
           
            if (obj == null || obj == DBNull.Value)
            {
                d = decimal.Parse("0.00"); 
            }
            else
            {
                d = decimal.Parse(obj.ToString());
            }
            return d;
        }
        public object convertToDBNull(object obj)
        {
            object _retVal = null;
            if (obj == null || obj.ToString() == " " || string.IsNullOrEmpty(obj.ToString()))
            {
                _retVal = DBNull.Value;
            }
            else
            {
                _retVal = obj;
            }

            return _retVal;

        }
        public object convertToP(object obj)
        {
            object _retVal = "%";
            if (obj == null || obj.ToString() == " " || string.IsNullOrEmpty(obj.ToString()))
            {
                _retVal = "%";
            }
            else
            {
                _retVal = obj;
            }

            return _retVal;
        }
        public bool convertToBool(object obj)
        {
            bool b = false;
            if (obj == null || obj == DBNull.Value || obj.Equals("false"))
            {
                b = false;
            }
            else
            {
                b = true;
            }
            return b;
        }
        public string returnNA(object obj)
        {
            string _retVal = "";
            if (obj == null || obj == DBNull.Value)
            {
                _retVal = "N/A";
            }
            else
            {
                _retVal = obj.ToString();
            }

            return _retVal;

        }
        public object DateParse(string date)
        {


            if (!string.IsNullOrEmpty(date))
            {
                date = date.Trim();
                //return DateTime.Parse(date, new System.Globalization.CultureInfo.InvariantCulture);
                return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return DBNull.Value;

            }
        }
        public string convertStringToNull(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == " " || string.IsNullOrEmpty(obj.ToString()))
            {
                return null;
            }
            else
            {
                return obj.ToString();
            }


        }
        public object DateParse2(string date)
        {


            if (!string.IsNullOrEmpty(date))
            {
                date = date.Trim();
                //return DateTime.Parse(date, new System.Globalization.CultureInfo.InvariantCulture);
                DateTime d = DateTime.Parse(date);
                return d;
            }
            else
            {
                return DBNull.Value;

            }
        }
        public object DateParse3(string date)
        {


            if (!string.IsNullOrEmpty(date))
            {
                date = date.Trim();
                //return DateTime.Parse(date, new System.Globalization.CultureInfo.InvariantCulture);
                return DateTime.Parse(date);
            }
            else
            {
                return null;

            }
        }
        public object DateParse4(string date)
        {


            if (!string.IsNullOrEmpty(date))
            {
                date = date.Trim();
                //return DateTime.Parse(date, new System.Globalization.CultureInfo.InvariantCulture);
                DateTime d = DateTime.Parse(date);
                string temp = d.ToString("yyyy-MM-dd");
                return temp;
            }
            else
            {
                return null;

            }
        }
        public DateTime? ConvertToDate(object date1)
        {
            if (date1 == null)
            {
                return null;
            }
            else
            {
                string date = date1.ToString();
                if (string.IsNullOrEmpty(date) || date == " ")
                {
                    return null;
                }
                else
                {
                    date = date.Trim();
                    date = date.Substring(0, 10);
                    //return DateTime.Parse(date, new System.Globalization.CultureInfo.InvariantCulture);
                    return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

            }




        }
        public bool ConvertToBool(object obj)
        {
            bool result = false;
            if (obj == null || obj == DBNull.Value)
            {
                result = false;
            }
            else
            {
                result = Convert.ToBoolean(obj);
            }
            return result;
        }
        public object stringToDBNull(string obj)
        {
            object _retVal = null;
            if (string.IsNullOrEmpty(obj) || obj == " ")
            {
                _retVal = DBNull.Value;
            }
            else
            {
                _retVal = obj;
            }

            return _retVal;

        }
        public bool isNull(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public int GetDD(string d1, string d2)
        {

            if (d1 == null || d2 == null)
            {
                return 0;
            }


            DateTime dd1 = Convert.ToDateTime(d1);
            DateTime dd2 = Convert.ToDateTime(d2);

            TimeSpan difference = dd1 - dd2;
            DateDiff dateDiff = new DateDiff(dd1, dd2);

            var days = dateDiff.Months;

            if (days < 0)
            {
                days = days * (-1);
            }
            return days;
        }       
        public bool URLExists(string url)
        {
            bool result = false;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                result = true;
            }
            catch (WebException webException)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        public static DataTable ToDataTable<T>(List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static DataTable RemoveNullsFromDataTable(DataTable dt)
        {
            for (int a = 0; a < dt.Rows.Count; a++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[a][i] == DBNull.Value)
                    {
                        var type = dt.Columns[i].DataType;
                        dt.Rows[a][i] = type.IsValueType ? Activator.CreateInstance(type) : null;
                    }
                }
            }

            return dt;
        }

        #region -- Crystal Report Generation Methods --      
        public static string UserTitle;
        public static string ReportTitle;
        public static string CompanyTitle;
        public static string FileTitle;
        public static string ReportName;
        public static DataTable ReportDataSource;
        public static DataSet ReportDataSource2;
        public static string[] ReportFormula;
        public static bool EmptyReport;
        public static string _CurrentReportpath;
        public static DataSet ReportDataSet;
        private static String _subReportName;
        public static string SubReportName
        {
            get { return _subReportName; }
            set { _subReportName = value; }
        }
        private static String _subReportName1;
        public static string SubReportName1
        {
            get { return _subReportName1; }
            set { _subReportName1 = value; }
        }
        private static String _subReportName2;
        public static string SubReportName2
        {
            get { return _subReportName2; }
            set { _subReportName2 = value; }
        }
        private static String _subReportName3;
        public static string SubReportName3
        {
            get { return _subReportName3; }
            set { _subReportName3 = value; }
        }
        private static String _subReportName4;
        public static string SubReportName4
        {
            get { return _subReportName4; }
            set { _subReportName4 = value; }
        }
        private static String _subReportName5;
        public static string SubReportName5
        {
            get { return _subReportName5; }
            set { _subReportName5 = value; }
        }
        private static String _subReportName6;
        public static string SubReportName6
        {
            get { return _subReportName6; }
            set { _subReportName6 = value; }
        }
        public static void ShowReport(CrystalReportViewer objClsCrystalRptVw)
        {
            ReportDocument objReport = null;
            try
            {
                string DateTitle = "Run Date/Time :" + System.DateTime.Now.ToString("MM/dd/yy HH:mm:ss tt");
                objReport = new ReportDocument();
                if (ReportDataSource.Rows.Count == 0)
                {
                    EmptyReport = true;
                    //No data found
                }
                else
                {
                    EmptyReport = false;
                    var _with1 = objReport;
                    _with1.Load(_CurrentReportpath + ReportName);
                    _with1.SetDataSource(ReportDataSource);
                    objClsCrystalRptVw.ReportSource = objReport;
                    var _with2 = _with1.DataDefinition;

                    if (UserTitle != null)
                    {
                        _with2.FormulaFields["UserTitle"].Text = "'" + UserTitle + "'";
                    }
                    if (ReportTitle != null)
                    {
                        _with2.FormulaFields["ReportTitle"].Text = "'" + ReportTitle + "'";
                    }
                    if (CompanyTitle != null)
                    {
                        _with2.FormulaFields["CompanyTitle"].Text = "'" + CompanyTitle + "'";
                    }
                    if (FileTitle != null)
                    {
                        _with2.FormulaFields["FileTitle"].Text = "'" + FileTitle + "'";
                    }
                    if (DateTitle != null)
                    {
                        if (_with2.FormulaFields["Run"] != null)
                        {
                            _with2.FormulaFields["Run"].Text = "'" + DateTitle + "'";
                        }
                    }
                    passFormulae(objReport, ReportFormula);
                    //passing formulae to report except user title, report title, company title and file title
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Load report failed.") > -1)
                {
                    EmptyReport = true;
                    objReport.Close();
                    //objReport.Dispose()
                    objReport = null;
                    GC.Collect();
                }
            }
        }

        public static void ShowReportWithOneSubReports(CrystalReportViewer objClsCrystalRptVw)
        {
            try
            {
                string DateTitle = System.DateTime.Now.ToLongDateString();
                ReportDocument objReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ReportDocument objReport1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string SubReportName1 = SubReportName;
                dt = ReportDataSet.Tables[0];
                DataTable dtSub = ReportDataSet.Tables[1];
                if (dt.Rows.Count == 0 & dtSub.Rows.Count == 0)
                {
                    EmptyReport = true;
                    //no Data Found
                }
                else
                {
                    var _with1 = objReport;
                    //main report
                    _with1.Load(_CurrentReportpath + ReportName);
                    _with1.SetDataSource(dt);

                    //Sub Report1
                    objReport1 = _with1.OpenSubreport(SubReportName1);
                    objReport1.SetDataSource(dtSub);
                    objClsCrystalRptVw.ReportSource = objReport;

                    var _with2 = _with1.DataDefinition;
                    if (UserTitle != null)
                    {
                        _with2.FormulaFields["UserTitle"].Text = "'" + UserTitle + "'";
                    }
                    if (ReportTitle != null)
                    {
                        _with2.FormulaFields["ReportTitle"].Text = "'" + ReportTitle + "'";
                    }
                    if (CompanyTitle != null)
                    {
                        _with2.FormulaFields["CompanyTitle"].Text = "'" + CompanyTitle + "'";
                    }
                    if (FileTitle != null)
                    {
                        _with2.FormulaFields["FileTitle"].Text = "'" + FileTitle + "'";
                    }
                    if (DateTitle != null)
                    {
                        _with2.FormulaFields["Run"].Text = "'" + DateTitle + "'";
                    }
                    passFormulae(objReport, ReportFormula);
                    //passing formulae to report except user title, report title, company title and file title
                    EmptyReport = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void ShowReportWithMultipleSubReports(CrystalReportViewer objClsCrystalRptVw)
        {
            try
            {
                string DateTitle = System.DateTime.Now.ToLongDateString();
                ReportDocument objReport = new ReportDocument();
                ReportDocument objReport1 = new ReportDocument();
                ReportDocument objReport2 = new ReportDocument();
                ReportDocument objReport3 = new ReportDocument();
                ReportDocument objReport4 = new ReportDocument();
                ReportDocument objReport5 = new ReportDocument();
                ReportDocument objReport6 = new ReportDocument();
                DataSet objds = new DataSet();
                objds = ReportDataSet;
                DataTable dt = new DataTable();
                dt = objds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    EmptyReport = true;
                    //ShowMessage
                }
                else
                {
                    var _with1 = objReport;
                    _with1.Load(_CurrentReportpath + ReportName);
                    _with1.SetDataSource(dt);
                    objReport1 = _with1.OpenSubreport(SubReportName);
                    objReport1.SetDataSource(objds.Tables[1]);
                    if (SubReportName1 != null)
                    {
                        objReport2 = _with1.OpenSubreport(SubReportName1);
                        objReport2.SetDataSource(objds.Tables[2]);
                    }
                    if (SubReportName2 != null)
                    {
                        objReport3 = _with1.OpenSubreport(SubReportName2);
                        objReport3.SetDataSource(objds.Tables[3]);
                    }
                    if (SubReportName3 != null)
                    {
                        objReport4 = _with1.OpenSubreport(SubReportName3);
                        objReport4.SetDataSource(objds.Tables[4]);
                    }
                    if (SubReportName4 != null)
                    {
                        objReport5 = _with1.OpenSubreport(SubReportName4);
                        objReport5.SetDataSource(objds.Tables[5]);
                    }
                    if (SubReportName5 != null)
                    {
                        objReport6 = _with1.OpenSubreport(SubReportName5);
                        objReport6.SetDataSource(objds.Tables[6]);
                    }
                    objClsCrystalRptVw.ReportSource = objReport;
                    var _with2 = _with1.DataDefinition;

                    if (UserTitle != null)
                    {
                        _with2.FormulaFields["UserTitle"].Text = "'" + UserTitle + "'";
                    }
                    if (ReportTitle != null)
                    {
                        _with2.FormulaFields["ReportTitle"].Text = "'" + ReportTitle + "'";
                    }
                    if (CompanyTitle != null)
                    {
                        _with2.FormulaFields["CompanyTitle"].Text = "'" + CompanyTitle + "'";
                    }
                    if (FileTitle != null)
                    {
                        _with2.FormulaFields["FileTitle"].Text = "'" + FileTitle + "'";
                    }
                    if (DateTitle != null)
                    {
                        if (_with2.FormulaFields["Run"] != null)
                        {
                            _with2.FormulaFields["Run"].Text = "'" + DateTitle + "'";
                        }
                    }
                    passFormulae(objReport, ReportFormula);
                    EmptyReport = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void passFormulae(ReportDocument objReport, string[] formulae)
        {
            try
            {
                //passing formulae to report except user title, report title, company title and file title
                if ((formulae != null))
                {
                    var _with1 = objReport.DataDefinition;
                    for (int i = 0; i <= formulae.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(formulae[i]))
                        {
                            var strArr = formulae[i].Split('=');
                            string Key = strArr[0].Trim();
                            string value = strArr[1].Trim();
                            _with1.FormulaFields[Key].Text = "'" + value + "'";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}


