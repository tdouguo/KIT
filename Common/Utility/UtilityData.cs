// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;

namespace Kit
{
    /// <summary>
    /// 
    /// </summary>
    public static class UtilityData
    {
        private readonly static string[] CsvRowSplit = new string[] { "\r\n" };
        private readonly static string[] CsvColumnSplit = new string[] { "," };

        #region ... Text

        #region ... Csv

        public static string[] SplitDataRowByTextCsv(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default(string[]);
            }
            return text.Split(CsvRowSplit, StringSplitOptions.None);
        }

        public static string[] SplitDataColumnByTextCsvOneRow(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default(string[]);
            }
            return text.Split(CsvColumnSplit, StringSplitOptions.None);
        }

        #endregion

        #endregion


    }
}