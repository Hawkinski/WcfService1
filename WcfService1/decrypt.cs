using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService1
{
    public class decrypt
    {

        public string Flc_Decrypt(string str)
        {
            // ERROR: Not supported in C#: OnErrorStatement

            try
            {
                int intRnd = 0;
                int i = 0;
                long lngLen = 0;
                string strResult = null;
                lngLen = str.Length;
                intRnd = 25;
                strResult = "";
                for (i = 2; i <= lngLen; i++)
                {
                    strResult = strResult + Strings.Chr(Strings.Asc(Strings.Mid(str, i, 1)) + intRnd);
                }
                return strResult;
            }
            catch (Exception)
            {
                throw;
            }


        }

    }
}