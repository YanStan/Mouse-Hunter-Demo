using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Mouse_Hunter.AccountsSheetModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Mouse_Hunter.Repositories
{
    public class GoogleSheetsRepository
    {
        string SpreadsheetId;
        SheetsService Service;


        public GoogleSheetsRepository(string spreadsheetId)
        {
            SpreadsheetId = spreadsheetId;
            Service = new SheetConnector().Connect();
        }


        public IList<IList<object>> TryGetRowsRecursive(string range, int counter = 0)
        {
            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(SpreadsheetId, range);
                ValueRange response = request.Execute();
                IList<IList<object>> rows = response.Values;
                // Prints
                if (rows != null && rows.Count > 0)
                {
                    Log.Logger.Debug($"Google sheets data found successfully: {rows.Count} rows.\r\n");
                }
                else
                {
                    Log.Logger.Error("No google sheets data found.\r\n");
                }
                return rows;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MySeriLogger.LogConnectionError(MethodBase.GetCurrentMethod().Name);
                if (counter < 5)
                {
                    Thread.Sleep(2000);
                    TryGetRowsRecursive(range, counter + 1);
                }
            }
            return new List<IList<object>>();
        }
    }
}
