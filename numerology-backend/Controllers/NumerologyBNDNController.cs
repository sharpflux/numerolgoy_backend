using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NumerologystSolution.Models;
using NumerologystSolution.Services;
using OmsSolution.Entities;
using OmsSolution.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System.Text;
using HtmlAgilityPack;

namespace NumerologystSolution.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class NumerologyBNDNController : ControllerBase
    {
        private INumerologyBNDNService _service;
        private IClientService _clientservice;
        private IPredictionService _predictionservice;
        public NumerologyBNDNController(INumerologyBNDNService service, IClientService clientservice, IPredictionService predictionservice)
        {
            _service = service;
            _clientservice = clientservice;
            _predictionservice = predictionservice;
        }
        [HttpGet("GeneratePdfWithAllData")]
        public async Task<IActionResult> GeneratePdfWithAllDataAsync(string birthDate, string gender, string clientId)
        {
            DataTable response = await _service.CalculateLoShuGrid(birthDate, gender, clientId);

            if (response.Rows.Count == 0)
            {
                return NotFound("No data found for the given parameters.");
            }

            string pdfFilePath =await GeneratePdfAsync(response, clientId, birthDate, gender);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);

            // Properly use the File method here
            return File(pdfBytes, "application/pdf", "NumerologyReport.pdf");
        }

        private async Task<string> GeneratePdfAsync(DataTable data, string clientId, string birthDate, string gender)
        {
            string filePath = Path.Combine("pdf", Guid.NewGuid() + ".pdf");
        
       

            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Add Title and Date
                    Font fontTitle = FontFactory.GetFont("Arial", 16, Font.BOLD);
                    Font fontDate = FontFactory.GetFont("Arial", 12, Font.NORMAL);

                    Paragraph title = new Paragraph("Numerology Report", fontTitle);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd-MM-yyyy HH:mm"), fontDate);
                    dateParagraph.Alignment = Element.ALIGN_RIGHT;
                    document.Add(dateParagraph);

                    document.Add(new Chunk("\n")); // Line break

                    DataTable response = await _clientservice.ClientGetById(clientId);

                    PdfPTable table = new PdfPTable(3); // Three columns
                    table.WidthPercentage = 100;

                    // Adding data
                    foreach (DataRow row in response.Rows)
                    {
                        table.AddCell(new Phrase($"First Name: {row["First_Name"]}"));
                        table.AddCell(new Phrase($"Middle Name: {row["Middle_Name"]}"));
                        table.AddCell(new Phrase($"Last Name: {row["Last_Name"]}"));

                        table.AddCell(new Phrase($"DOB: {row["DOB"]}"));
                        table.AddCell(new Phrase($"Gender: {row["Gender"]}"));
                        table.AddCell(new Phrase($"Mobile No1: {row["Mobile_No1"]}"));

                        table.AddCell(new Phrase($"Mobile No2: {row["Mobile_No2"]}"));
                        table.AddCell(new Phrase($"Mobile No3: {row["Mobile_No3"]}"));
                        table.AddCell(new Phrase($"Vehicle No1: {row["Vechile_No1"]}"));

                        table.AddCell(new Phrase($"Vehicle No2: {row["Vechile_No2"]}"));
                        table.AddCell(new Phrase($"Vehicle No3: {row["Vechile_No3"]}"));
                        table.AddCell(new Phrase($"House No1: {row["House_No1"]}"));

                        table.AddCell(new Phrase($"House No2: {row["House_No2"]}"));
                        table.AddCell(new Phrase($"House No3: {row["House_No3"]}"));
                        table.AddCell(new Phrase($"Email Id: {row["Email_Id"]}"));

                        table.AddCell(new Phrase($"IsActive: {row["IsActive"]}"));
                    }

                    document.Add(table);

                    // PdfPTable for organizing content side by side
                    PdfPTable contentTable = new PdfPTable(2); // 2 columns: one for LoShoHtml and one for LuckGridHtml
                    contentTable.WidthPercentage = 100;
                    contentTable.SpacingBefore = 10f;
                    contentTable.SpacingAfter = 10f;

                    // Adding LoShoHtml content
                    string loShoHtml = data.Rows[0]["LoShoHtml"].ToString();
                    if (!string.IsNullOrEmpty(loShoHtml))
                    {
                        loShoHtml = CleanHtml(loShoHtml);
                        using (var stringReader = new StringReader(loShoHtml))
                        {
                            PdfPCell loShoCell = new PdfPCell();
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                            loShoCell.Border = PdfPCell.NO_BORDER;
                            contentTable.AddCell(loShoCell); // Add to the left cell
                        }
                    }
                    else
                    {
                        contentTable.AddCell(new PdfPCell(new Phrase("No LoShoHtml available", fontDate))); // Placeholder text if empty
                    }

                    // Adding LuckGridHtml content
                    string luckGridHtml = data.Rows[0]["LuckGridHtml"].ToString();
                    if (!string.IsNullOrEmpty(luckGridHtml))
                    {
                        luckGridHtml = CleanHtml(luckGridHtml);
                        using (var stringReader = new StringReader(luckGridHtml))
                        {
                            PdfPCell luckGridCell = new PdfPCell();
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                            luckGridCell.Border = PdfPCell.NO_BORDER;
                            contentTable.AddCell(luckGridCell); // Add to the right cell
                        }
                    }
                    else
                    {
                        contentTable.AddCell(new PdfPCell(new Phrase("No LuckGridHtml available", fontDate))); // Placeholder text if empty
                    }

                    // Add the PdfPTable to the document
                    document.Add(contentTable);

                    var columnMapping = new Dictionary<string, string>
                    {
                        { "DayTotal", "BIRTH NUMBER" },
                        { "DateTotal", "DESTINY NUMBER" },
                        { "Kn", "KUA NUMBER" },
                        { "FullNameNumber", "FULL NAME NUMBER" },
                        { "FaceNumber", "FRONT NAME NUMBER" },
                        { "SoulNumber", "SOUL NUMBER" },
                        { "PersonalityNo", "PERSONALITY NUMBER" },
                        { "PersonalYearNo", "PERSONAL YEAR NUMBER" },
                        { "EssenceNo", "ESSENCE NUMBER" }
                    };

                    // Add other data in tables
                    PdfPTable dataTable = new PdfPTable(2); // 2 columns: Key and Value
                    dataTable.WidthPercentage = 100;
                    dataTable.SpacingBefore = 10f;
                    dataTable.SpacingAfter = 10f;

                 

                    foreach (var mapping in columnMapping)
                    {
                        string sqlColumnName = mapping.Key;
                        string displayName = mapping.Value;

                        if (data.Columns.Contains(sqlColumnName))
                        {
                            dataTable.AddCell(new PdfPCell(new Phrase(displayName, fontDate)));
                            dataTable.AddCell(new PdfPCell(new Phrase(data.Rows[0][sqlColumnName].ToString(), fontDate)));
                        }
                    }

                    document.Add(dataTable);


                    // Create tables for each phase
                    PdfPTable tableP1C1 = CreatePhaseTable(data.Rows[0]["P1"].ToString(), data.Rows[0]["C1"].ToString(), data.Rows[0]["Range1"].ToString(), "P1", "C1");
                    PdfPTable tableP2C2 = CreatePhaseTable(data.Rows[0]["P2"].ToString(), data.Rows[0]["C2"].ToString(), data.Rows[0]["Range2"].ToString(), "P2", "C2");
                    PdfPTable tableP3C3 = CreatePhaseTable(data.Rows[0]["P3"].ToString(), data.Rows[0]["C3"].ToString(), data.Rows[0]["Range3"].ToString(), "P3", "C3");
                    PdfPTable tableP4C4 = CreatePhaseTable(data.Rows[0]["P4"].ToString(), data.Rows[0]["C4"].ToString(), data.Rows[0]["Range4"].ToString(), "P4", "C4");

                    // Create a parent table to hold all phase tables side by side
                    PdfPTable parentTable = new PdfPTable(4);
                    parentTable.WidthPercentage = 100;
                    parentTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    // Add phase tables to the parent table
                    parentTable.AddCell(new PdfPCell(tableP1C1) { Border = Rectangle.NO_BORDER });
                    parentTable.AddCell(new PdfPCell(tableP2C2) { Border = Rectangle.NO_BORDER });
                    parentTable.AddCell(new PdfPCell(tableP3C3) { Border = Rectangle.NO_BORDER });
                    parentTable.AddCell(new PdfPCell(tableP4C4) { Border = Rectangle.NO_BORDER });



                    document.Add(parentTable);


                    Paragraph titleComp = new Paragraph("Compatibility", fontTitle);
                    title.Alignment = Element.ALIGN_LEFT;
                    document.Add(titleComp);

                    PdfPTable table22 = new PdfPTable(4); // 4 columns for Item, Number, DigitNo, and Compatibility

                    // Add headers
                    table22.AddCell("Item");
                    table22.AddCell("Number");
                    table22.AddCell("DigitNo");
                    table22.AddCell("Compatibility");

              
                    foreach (DataRow row in data.Rows)
                    {
                        table22.AddCell(row["Item"].ToString());
                        table22.AddCell(row["Number"].ToString());
                        table22.AddCell(row["DigitNo"].ToString());
                        table22.AddCell(row["Compatibility"].ToString());
                    }

                    document.Add(table22);

                    string jsonResult = string.Empty;
                    string SubPrediction_Master_JSON = string.Empty;
                    string MissingNumberMaster_JSON = string.Empty;
                    string Remedies_Master_JSON = string.Empty;
                    string RepetitivePrediction_JSON = string.Empty;

                    string PersonalYear1 = string.Empty;
                    string PersonalYear2 = string.Empty;
                    string PersonalYear3 = string.Empty;

                    string YearName1 = string.Empty;
                    string YearName2 = string.Empty;
                    string YearName3 = string.Empty;


                    string FrontNamePrediction = string.Empty;
                    string FullNamePrediction = string.Empty;
                    string MindNumberPredictionJson = string.Empty;
                    string htmlMindNumberPrediction = string.Empty;
                    DataTable prdedData = await _predictionservice.PredictionPlanetsGET(birthDate, gender, clientId);
                    if (prdedData.Rows.Count > 0)
                    {
                        jsonResult = prdedData.Rows[0][0].ToString();
                        SubPrediction_Master_JSON = prdedData.Rows[0][1].ToString();
                        MissingNumberMaster_JSON = prdedData.Rows[0][2].ToString();
                        Remedies_Master_JSON = prdedData.Rows[0][3].ToString();
                        RepetitivePrediction_JSON = prdedData.Rows[0][4].ToString();

                        PersonalYear1 = prdedData.Rows[0][6].ToString();
                        PersonalYear2 = prdedData.Rows[0][7].ToString();
                        PersonalYear3 = prdedData.Rows[0][8].ToString();

                        FrontNamePrediction = prdedData.Rows[0][9].ToString();
                        FullNamePrediction = prdedData.Rows[0][10].ToString();
                        MindNumberPredictionJson = prdedData.Rows[0][11].ToString();


                        string htmlMissingNumbers = ConvertJsonToHtml(MissingNumberMaster_JSON,"Missing Number", "MissingDescription");
                        string htmlRemedies = ConvertJsonToHtml(Remedies_Master_JSON, "Remedies", "Remedies_Description");
                        string htmlRepetitivePrediction = ConvertJsonToHtml(RepetitivePrediction_JSON, "Repetative Numbers", "Description");
                        string htmlContent = ConvertJsonToHtml(jsonResult, "Prediction", "Prediction_Description");

                        List<PersonalYear> personalYears1 = JsonConvert.DeserializeObject<List<PersonalYear>>(PersonalYear1);
                        List<PersonalYear> personalYears2 = JsonConvert.DeserializeObject<List<PersonalYear>>(PersonalYear2);
                        List<PersonalYear> personalYears3 = JsonConvert.DeserializeObject<List<PersonalYear>>(PersonalYear3);

                        if (personalYears1.Count > 0)
                        {
                            YearName1 = personalYears1[0].YearName.ToString();
                        }
                        if (personalYears2.Count > 0)
                        {
                            YearName2 = personalYears2[0].YearName.ToString();
                        }
                        if (personalYears3.Count > 0)
                        {
                            YearName3 = personalYears3[0].YearName.ToString();
                        }

                        string htmlPersonalYear1 = ConvertJsonToHtml(PersonalYear1,  "Personal Year "+ YearName1, "PersonalYearNumber_Description");
                        string htmlPersonalYear2 = ConvertJsonToHtml(PersonalYear2,  "Personal Year " + YearName2, "PersonalYearNumber_Description");
                        string htmlPersonalYear3 = ConvertJsonToHtml(PersonalYear3,  "Personal Year " + YearName3, "PersonalYearNumber_Description");


                        string htmlFrontNamePrediction = ConvertJsonToHtml(FrontNamePrediction, "Front Name Prediction", "NameNumber_Description");
                        string htmlFullNamePrediction = ConvertJsonToHtml(FullNamePrediction, "Full Name Prediction", "NameNumber_Description");
                        if (MindNumberPredictionJson != "") { htmlMindNumberPrediction = ConvertJsonToHtml(MindNumberPredictionJson, "Mind Numbers", "MIndNoDescription"); }
                         


                        // Ensure the table uses 100% width
                        string htmlContentWithWidth = $"<div style='width: 100%;'>{htmlContent}</div>";
                        // Ensure each table uses 100% width
                        htmlMissingNumbers = $"<div style='width: 100%;'>{htmlMissingNumbers}</div>";
                        htmlRemedies = $"<div style='width: 100%;'>{htmlRemedies}</div>";
                        htmlRepetitivePrediction = $"<div style='width: 100%;'>{htmlRepetitivePrediction}</div>";

                        htmlPersonalYear1 = $"<div style='width: 100%;'>{htmlPersonalYear1}</div>";
                        htmlPersonalYear2 = $"<div style='width: 100%;'>{htmlPersonalYear2}</div>";
                        htmlPersonalYear3 = $"<div style='width: 100%;'>{htmlPersonalYear3}</div>";

                        htmlFrontNamePrediction = $"<div style='width: 100%;'>{htmlFrontNamePrediction}</div>";
                        htmlFullNamePrediction = $"<div style='width: 100%;'>{htmlFullNamePrediction}</div>";

                        htmlMindNumberPrediction = $"<div style='width: 100%;'>{htmlMindNumberPrediction}</div>";


                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(htmlContentWithWidth);
                        doc.OptionFixNestedTags = true;
                        doc.OptionWriteEmptyNodes = true;

                        StringWriter stringWriter = new StringWriter();
                        doc.Save(stringWriter);
                        string cleanedHtmlContent = stringWriter.ToString();

                        using (StringReader sr = new StringReader(cleanedHtmlContent))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }



                        // Create HTML documents
                        HtmlDocument docMissingNumbers = new HtmlDocument();
                        docMissingNumbers.LoadHtml(htmlMissingNumbers);
                
                        docMissingNumbers.OptionFixNestedTags = true;
                        docMissingNumbers.OptionWriteEmptyNodes = true;

                        HtmlDocument docRemedies = new HtmlDocument();
                        docRemedies.LoadHtml(htmlRemedies);

                        docRemedies.OptionFixNestedTags = true;
                        docRemedies.OptionWriteEmptyNodes = true;

                        HtmlDocument docRepetitivePrediction = new HtmlDocument();
                        docRepetitivePrediction.LoadHtml(htmlRepetitivePrediction);
                        docRepetitivePrediction.OptionFixNestedTags = true;
                        docRepetitivePrediction.OptionWriteEmptyNodes = true;
                        // Convert HTML documents to strings
                        StringWriter stringWriterMissingNumbers = new StringWriter();
                        docMissingNumbers.Save(stringWriterMissingNumbers);
                        string cleanedHtmlMissingNumbers = stringWriterMissingNumbers.ToString();

                        StringWriter stringWriterRemedies = new StringWriter();
                        docRemedies.Save(stringWriterRemedies);
                        string cleanedHtmlRemedies = stringWriterRemedies.ToString();

                        StringWriter stringWriterRepetitivePrediction = new StringWriter();
                        docRepetitivePrediction.Save(stringWriterRepetitivePrediction);
                        string cleanedHtmlRepetitivePrediction = stringWriterRepetitivePrediction.ToString();

                        // Add PdfPTables to the document
                        PdfPTable tableMissingNumbers = new PdfPTable(1); // 1 column
                        tableMissingNumbers.WidthPercentage = 100;
                        using (StringReader sr = new StringReader(cleanedHtmlMissingNumbers))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }
                        document.Add(tableMissingNumbers);

                        PdfPTable tableRemedies = new PdfPTable(1); // 1 column
                        tableRemedies.WidthPercentage = 100;
                        using (StringReader sr = new StringReader(cleanedHtmlRemedies))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }
                        document.Add(tableRemedies);

                        PdfPTable tableRepetitivePrediction = new PdfPTable(1); 
                        tableRepetitivePrediction.WidthPercentage = 100;
                        using (StringReader sr = new StringReader(cleanedHtmlRepetitivePrediction))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }
                        document.Add(tableRepetitivePrediction);



                        //Personal Year 

                        HtmlDocument docPersonalYear = new HtmlDocument();
                        docPersonalYear.LoadHtml(htmlPersonalYear1);
                        docPersonalYear.OptionFixNestedTags = true;
                        docPersonalYear.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterPersonalYear = new StringWriter();
                        docPersonalYear.Save(stringWriterPersonalYear);
                        string cleanedhtmlPersonalYear1 = stringWriterPersonalYear.ToString();

                        using (StringReader sr = new StringReader(cleanedhtmlPersonalYear1))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }


                        HtmlDocument docPersonalYear2 = new HtmlDocument();
                        docPersonalYear2.LoadHtml(htmlPersonalYear2);
                        docPersonalYear2.OptionFixNestedTags = true;
                        docPersonalYear2.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterPersonalYear2 = new StringWriter();
                        docPersonalYear2.Save(stringWriterPersonalYear2);
                        string cleanedhtmlPersonalYear2 = stringWriterPersonalYear2.ToString();

               

                        using (StringReader sr = new StringReader(cleanedhtmlPersonalYear2))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }


                        HtmlDocument docPersonalYear3 = new HtmlDocument();
                        docPersonalYear3.LoadHtml(htmlPersonalYear3);
                        docPersonalYear3.OptionFixNestedTags = true;
                        docPersonalYear3.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterPersonalYear3= new StringWriter();
                        docPersonalYear3.Save(stringWriterPersonalYear3);
                        string cleanedhtmlPersonalYear3 = stringWriterPersonalYear3.ToString(); 
                      

                        using (StringReader sr = new StringReader(cleanedhtmlPersonalYear3))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }



                        //Front Name Fulll Name

                        HtmlDocument docFrontNamePrediction = new HtmlDocument();
                        docFrontNamePrediction.LoadHtml(htmlFrontNamePrediction);
                        docFrontNamePrediction.OptionFixNestedTags = true;
                        docFrontNamePrediction.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterFrontNamePrediction = new StringWriter();
                        docFrontNamePrediction.Save(stringWriterFrontNamePrediction);
                        string cleanedhtmlFrontNamePrediction = stringWriterFrontNamePrediction.ToString();


                        using (StringReader sr = new StringReader(cleanedhtmlFrontNamePrediction))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }


                        HtmlDocument docFullNamePrediction = new HtmlDocument();
                        docFullNamePrediction.LoadHtml(htmlFullNamePrediction);
                        docFullNamePrediction.OptionFixNestedTags = true;
                        docFullNamePrediction.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterFullNamePrediction = new StringWriter();
                        docFullNamePrediction.Save(stringWriterFullNamePrediction);
                        string cleanedhtmlFullNamePrediction = stringWriterFullNamePrediction.ToString();


                        using (StringReader sr = new StringReader(cleanedhtmlFullNamePrediction))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }

                        //Mind No


                        HtmlDocument docMindNumberPrediction = new HtmlDocument();
                        docMindNumberPrediction.LoadHtml(htmlMindNumberPrediction);
                        docMindNumberPrediction.OptionFixNestedTags = true;
                        docMindNumberPrediction.OptionWriteEmptyNodes = true;

                        StringWriter stringWriterMindNumberPrediction = new StringWriter();
                        docMindNumberPrediction.Save(stringWriterMindNumberPrediction);
                        string cleanedhtmlMindNumberPrediction = stringWriterMindNumberPrediction.ToString();


                        using (StringReader sr = new StringReader(cleanedhtmlMindNumberPrediction))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                        }


                    }

                    document.Close();
                }

               System.IO. File.WriteAllBytes(filePath, memoryStream.ToArray());
            }

            return filePath;
        }


       


        public string ConvertJsonToHtml(string jsonResult, string Title,string htmlPrintKey)
        {
            var sb = new StringBuilder();
            var jsonArray = JArray.Parse(jsonResult);

            sb.Append("<html><body>");
            sb.Append("<h2>" + Title + "</h2>");
            foreach (var jsonObject in jsonArray)
            {
         
                sb.Append("<table style='width: 100%;'>");

             
                var Description = jsonObject[htmlPrintKey].ToString();

                sb.Append("<tr>");
                sb.Append($"<td>{Description}</td>");
                sb.Append("</tr>");

                sb.Append("</table>");
                sb.Append("<br>");
            }

            sb.Append("</body></html>");

            return sb.ToString();
        }
        private PdfPTable CreatePhaseTable(string phaseValue, string cycleValue, string footerText, string phaseHeader, string cycleHeader)
        {
            PdfPTable table = new PdfPTable(2); // 2 columns: Phase and Cycle
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;

            Font fontHeader = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontData = FontFactory.GetFont("Arial", 12);
            Font fontFooter = FontFactory.GetFont("Arial", 12, Font.ITALIC);

            // Add table header
            table.AddCell(new PdfPCell(new Phrase(phaseHeader, fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase(cycleHeader, fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Add table data
            table.AddCell(new PdfPCell(new Phrase(phaseValue, fontData)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase(cycleValue, fontData)) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Add table footer
            PdfPCell footerCell = new PdfPCell(new Phrase(footerText, fontFooter));
            footerCell.Colspan = 2;
            footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(footerCell);

            return table;
        }
        public string ConvertJsonToHtmlOLD2(string jsonResult, string Title)
        {
            var sb = new StringBuilder();
            var jsonArray = JArray.Parse(jsonResult);

            sb.Append("<html><body>");

            foreach (var jsonObject in jsonArray)
            {
                sb.Append("<h2>"+ Title + "</h2>");
                sb.Append("<table   style='width: 100%;'>");

                foreach (var prop in jsonObject)
                {
                    sb.Append("<tr>");
                    //sb.Append($"<td>{prop.Path}</td>");
                    sb.Append($"<td>{prop.First}</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</table>");
                sb.Append("<br>");
            }

            sb.Append("</body></html>");

            return sb.ToString();
        }

        public string CleanHtml2(string html)
        {
            // Fix common HTML issues here
            html = html.Replace("<\\/b>", "</b>")
                       .Replace("<\\/span>", "</span>")
                       .Replace("<\\/div>", "</div>");

            // Additional fixes can be added as needed

            return html;
        }
        public string ConvertJsonToHtmlOLD(string jsonResult)
        {
            var sb = new StringBuilder();
            var jsonObject = JObject.Parse(jsonResult);

            sb.Append("<html><body>");

            // Convert Prediction_Master_JSON
            if (jsonObject["Prediction_Master_JSON"] != null)
            {
                sb.Append("<h2>Prediction Master</h2>");
                sb.Append("<table border='1'>");
                sb.Append("<tr><th>Field</th><th>Value</th></tr>");
                foreach (var item in jsonObject["Prediction_Master_JSON"])
                {
                    foreach (var prop in item)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{prop.Path}</td>");
                        sb.Append($"<td>{prop.First}</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }

            // Convert MissingNumberMaster_JSON
            if (jsonObject["MissingNumberMaster_JSON"] != null)
            {
                sb.Append("<h2>Missing Number Master</h2>");
                sb.Append("<table border='1'>");
                sb.Append("<tr><th>Field</th><th>Value</th></tr>");
                foreach (var item in jsonObject["MissingNumberMaster_JSON"])
                {
                    foreach (var prop in item)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{prop.Path}</td>");
                        sb.Append($"<td>{prop.First}</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }

            // Convert Remedies_Master_JSON
            if (jsonObject["Remedies_Master_JSON"] != null)
            {
                sb.Append("<h2>Remedies Master</h2>");
                sb.Append("<table border='1'>");
                sb.Append("<tr><th>Field</th><th>Value</th></tr>");
                foreach (var item in jsonObject["Remedies_Master_JSON"])
                {
                    foreach (var prop in item)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{prop.Path}</td>");
                        sb.Append($"<td>{prop.First}</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }

            sb.Append("</body></html>");

            return sb.ToString();
        }

        private string GeneratePdf2(DataTable data)
        {
            string filePath = Path.Combine("pdf", Guid.NewGuid() + ".pdf");

            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Add Title and Date
                    Font fontTitle = FontFactory.GetFont("Arial", 16, Font.BOLD);
                    Font fontDate = FontFactory.GetFont("Arial", 12, Font.NORMAL);

                    Paragraph title = new Paragraph("Numerology Report", fontTitle);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd-MM-yyyy HH:mm"), fontDate);
                    dateParagraph.Alignment = Element.ALIGN_RIGHT;
                    document.Add(dateParagraph);

                    document.Add(new Chunk("\n")); // Line break


                    string loShoHtml = data.Rows[0]["LoShoHtml"].ToString();
                    if (!string.IsNullOrEmpty(loShoHtml))
                    {
                        // Clean and simplify the HTML
                        loShoHtml = CleanHtml(loShoHtml);

                        using (var stringReader = new StringReader(loShoHtml))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                        }
                    }


                    document.NewPage();

                    // Add LoShoHtml content
                    //string loShoHtml = data.Rows[0]["LoShoHtml"].ToString();
                    //if (!string.IsNullOrEmpty(loShoHtml))
                    //{
                    //    using (var stringReader = new StringReader(loShoHtml))
                    //    {

                    //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                    //    }
                    //}




                    document.NewPage();

                    //// Add LuckGridHtml content
                    string luckGridHtml = data.Rows[0]["LuckGridHtml"].ToString();
                    if (!string.IsNullOrEmpty(luckGridHtml))
                    {
                        luckGridHtml = CleanHtml(luckGridHtml);
                        using (var stringReader = new StringReader(luckGridHtml))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                        }
                    }

                    document.NewPage();

                    //Add other data in tables
                   PdfPTable table = new PdfPTable(2); // 2 columns: Key and Value
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    table.SpacingAfter = 10f;

                    foreach (DataColumn column in data.Columns)
                    {
                        if (column.ColumnName != "LoShoHtml" && column.ColumnName != "LuckGridHtml")
                        {
                            table.AddCell(new PdfPCell(new Phrase(column.ColumnName, fontDate)));
                            table.AddCell(new PdfPCell(new Phrase(data.Rows[0][column].ToString(), fontDate)));
                        }
                    }

                    document.Add(table);

                    document.Close();
                }
               System.IO. File.WriteAllBytes(filePath, memoryStream.ToArray());
            }

            return filePath;
        }

        private string CleanHtml(string html)
        {
            // Simplify the HTML content
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // Remove unnecessary tags
            doc.DocumentNode.Descendants()
                .Where(n => n.Name == "meta" )
                .ToList()
                .ForEach(n => n.Remove());

            // Return cleaned HTML
            return doc.DocumentNode.OuterHtml;
        }

        [HttpGet("CalculateLoShuGrid")]
        public async Task<IActionResult> CalculateLoShuGrid(string BirthDate, string Gender,string Client_id)
        {

            try
            {
                 
                DataTable response = await _service.CalculateLoShuGrid(BirthDate, Gender , Client_id);
                var lst = response.AsEnumerable()
                       .Select(r => r.Table.Columns.Cast<DataColumn>()
                       .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    ).ToDictionary(z => z.Key, z => z.Value)
                 ).ToList();

                return Ok(lst);
                 
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost("saveNumerologyClientsDetails")]
        public async Task<IActionResult> SaveNumerologyClientsDetails(ClientRequest model)
        {

            try
            {
                var data = await _service.SaveNumerologyClientsDetails(model);
                return Ok(data);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        [HttpGet("numerologyBNDNGET")]
        public async Task<IActionResult> NumerologyBNDNGET(string startIndex, string pageSize, string searchBy, string searchCriteria, string tableDataJsons)
        {
            try
            {
                PaginationRequest model = new PaginationRequest();
                model.StartIndex = startIndex;
                model.PageSize = pageSize;
                model.SearchBy = searchBy;
                model.SearchCriteria = searchCriteria;

                if (!string.IsNullOrWhiteSpace(searchCriteria) && searchCriteria != "undefined")
                {
                    model.SearchBy = "1"; // Set searchBy to '1'
                }
                else
                {
                    model.SearchBy = "0"; // Set searchBy to '0'
                }

                List<TableDataItem> tableData = JsonConvert.DeserializeObject<List<TableDataItem>>(tableDataJsons);
                List<string> ids = tableData.Select(item => item.id).ToList();
                string idString = string.Join(",", ids);
                DataTable response = await _service.NumerologyBNDNGET(model, idString);
                var lst = response.AsEnumerable()
                       .Select(r => r.Table.Columns.Cast<DataColumn>()
                       .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                    ).ToDictionary(z => z.Key, z => z.Value)
                 ).ToList();

                return Ok(lst);
            }
            catch (System.Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
    }


  
}
public class PersonalYear
{
    public int PersonalYearId { get; set; }
    public int PersonalYearNumberId { get; set; }
    public string PersonalYearNumber_Description { get; set; }
    public bool IsActive { get; set; }
    public string YearName { get; set; }
}
