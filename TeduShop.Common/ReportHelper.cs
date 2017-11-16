using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeduShop.Common
{
    public static class ReportHelper
    {
        public static async Task GeneratePdf(string html, string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                    pdf.Save(ms);
                }
            });
        }

        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }

        public static HttpResponseMessage ReturnStreamAsFile(MemoryStream stream, string filename)
        {
            // Set HTTP Status Code
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            // Reset Stream Position
            stream.Position = 0;
            result.Content = new StreamContent(stream);

            // Generic Content Header
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            //Set Filename sent to client
            result.Content.Headers.ContentDisposition.FileName = filename;

            return result;
        }
    }
}