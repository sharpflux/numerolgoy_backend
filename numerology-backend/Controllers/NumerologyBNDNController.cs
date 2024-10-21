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
using OmsSolution.Utilities;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.tool.xml.html;

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
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSender _emailSender;
        public NumerologyBNDNController(INumerologyBNDNService service, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IClientService clientservice, IPredictionService predictionservice)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _clientservice = clientservice;
            _predictionservice = predictionservice;
            _emailSender = new EmailSender(_hostingEnvironment, _httpContextAccessor);
        }



        [HttpPost("sendemail")]
        public async Task<IActionResult> sendemail(ClientRequest model)
        {
 
            try
            {

                await _emailSender.PredictionCustomerEmailAsync(model.emailId, model.Client_id, model.First_Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }

        }
        [HttpGet("GeneratePdfWithAllData")]
        public async Task<IActionResult> GeneratePdfWithAllDataAsync(string birthDate, string gender, string clientId)
        {
            try
            {
                DataTable response = await _service.CalculateLoShuGrid(birthDate, gender, clientId);

                if (response.Rows.Count == 0)
                {
                    return NotFound("No data found for the given parameters.");
                }

                var (pdfFilePath, errorMessage) = await GeneratePdfAsync(response, clientId, birthDate, gender);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return StatusCode(500, errorMessage);
                }

                byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);

                return File(pdfBytes, "application/pdf", "NumerologyReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        private async Task<(string filePath, string errorMessage)> GeneratePdfAsync(DataTable data, string clientId, string birthDate, string gender)
        {
            string filePath = Path.Combine("pdf", clientId + ".pdf");
            string errorMessage = null;

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            try
            {

                // Embed the Comic Sans MS font
                string fontPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "comic.ttf");
                BaseFont baseFont2 = BaseFont.CreateFont(fontPath2, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Create various font styles
                Font normalFont = new Font(baseFont2, 12, Font.NORMAL);
                Font boldFont = new Font(baseFont2, 12, Font.BOLD);
                Font titleFont = new Font(baseFont2, 16, Font.BOLD);
                Font smallFont = new Font(baseFont2, 10, Font.NORMAL);


                using (var memoryStream = new MemoryStream())
                {
                    using (var document = new Document())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                        document.Open();

                        // Instantiate the PageEventHelper and assign it to the writer
                        PageEventHelper pageEventHelper = new PageEventHelper();
                        writer.PageEvent = pageEventHelper;

                        // Assuming the wwwroot directory is at the root of your project
                        string relativePath = Path.Combine("wwwroot", "images", "lord-ganesh.jpg");
                        // Get the absolute path of the wwwroot directory

                        string absolutePath = Path.GetFullPath(relativePath);

                        float pageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        // Add Lord Ganesha image
                        string imagePath = absolutePath; // Replace with actual path to your image
                        iTextSharp.text.Image lordGaneshaImage = iTextSharp.text.Image.GetInstance(imagePath);
                        lordGaneshaImage.ScaleToFit(pageWidth, float.MaxValue);
                        lordGaneshaImage.Alignment = Element.ALIGN_CENTER;
                        document.Add(lordGaneshaImage);

                        // Load the logo image
                        string logoPath = Path.Combine("wwwroot", "images", "Numeromystic-LOGO.png");
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);

                        // Set logo scale (adjust as needed)
                        logo.ScaleToFit(100f, 100f); // Scale logo to fit width and height

                        // Create a table with 1 column to hold the logo and company name in separate rows
                        PdfPTable tablelogo = new PdfPTable(1);
                        tablelogo.WidthPercentage = 100;
                        tablelogo.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Add the logo to the table in the first row
                        PdfPCell logoCell = new PdfPCell(logo)
                        {
                            Border = Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        tablelogo.AddCell(logoCell);

                        // Add company name to the table in the second row
                        Font fontCompanyName = FontFactory.GetFont("Arial", 20, Font.BOLD);
                        PdfPCell companyNameCell = new PdfPCell(new Phrase("Numeromystic", fontCompanyName))
                        {
                            Border = Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        tablelogo.AddCell(companyNameCell);

                        // Add the table to the document
                        document.Add(tablelogo);

                        // Add address and mobile number
                        Font fontAddress = FontFactory.GetFont("Arial", 12, Font.NORMAL);
                        Paragraph addressParagraph = new Paragraph("B-404, Treasure Island Society,\n Near Hotel Govind Garden, Pimple Saudagar, \n Pune -411027 \n dilipshahani23@gmail.com \nMobile No: +7420905725  \nWebsite: https://numeromystic.in/", fontAddress)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        document.Add(addressParagraph);

                        // Add a line break to push content down if necessary
                        document.Add(new Chunk("\n"));



                        // Ensure that the next content starts on a new page
                        document.NewPage();



                        // Add the new text to the third page

                        Paragraph newTextParagraph = new Paragraph();
                        newTextParagraph.Add(new Chunk("Introduction:\n", titleFont));
                        newTextParagraph.Add(new Chunk("Numbers are not good and numbers are not bad. A number has a certain vibrational power which is lucky for one may be unlucky for someone else. It is their relationship with a human, vibrations or an event, which is important. If all things, including you, are placed in a right order with number, the result will be good or else it will not be so good.\n\n", smallFont));
                        newTextParagraph.Add(new Chunk("When the universe has sent us into this world, he has made you fit for it. So even if you don't rely on numerology, you can conquer the world with your good work and hard work rather life may become a bit easier and pleasant if the wind and your luck is slightly tilted rather on your side than being indifferent to you.\n\n", smallFont));
                        newTextParagraph.Add(new Chunk("You have to start you have to climb the stairs or ou should go to excelarator  .\n\n", smallFont));
                        newTextParagraph.Add(new Chunk("Among all the numbers, birth number is the most important as an individual's characteristics, strengths and weaknesses are directly related to the birth number. An individual only has control on self and not the destiny. The destiny number indicates what the destiny is or where the destiny will take you while the birth number indicates what a person wants. A person can always change his wants and desires and work on his weaknesses to succeed in life. Destiny number indicates whether an individual will be able to get results easily or struggling.", smallFont));
                        newTextParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                        document.Add(newTextParagraph);

                        // Continue with the rest of the PDF generation
                        document.NewPage();

                        // Register custom font
                        string fontPath = Path.Combine("Fonts", "ShadowsIntoLight-Regular.ttf"); // Adjust the path as needed
                        BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        Font customFont = new Font(baseFont, 12, Font.NORMAL); // Adjust font size and style as needed


                        // Register custom font
                        var fontPathNew = Path.Combine("Fonts", "ShadowsIntoLight-Regular.ttf"); // Path to your custom font
                        var cssFontProvider = new XMLWorkerFontProvider();
                        cssFontProvider.Register(fontPathNew, "ShadowsIntoLight");



                        string fontPathComic = Path.Combine("Fonts", "COMIC.ttf"); // Adjust the path as needed
                        BaseFont baseFontComic = BaseFont.CreateFont(fontPathComic, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        Font fontTitle = new Font(baseFont, 16, Font.NORMAL);

                        // Add Title and Date
                        //Font fontTitle = FontFactory.GetFont("Comic Sans MS", 16, Font.BOLD);
                        Font fontDate = FontFactory.GetFont("Comic Sans MS", 12, Font.NORMAL);

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
                        // Adding data
                        foreach (DataRow row in response.Rows)
                        {
                            table.AddCell(new Phrase($"First Name: {row["First_Name"]}", smallFont));
                            table.AddCell(new Phrase($"Middle Name: {row["Middle_Name"]}", smallFont));
                            table.AddCell(new Phrase($"Last Name: {row["Last_Name"]}", smallFont));
                            table.AddCell(new Phrase($"DOB: {row["DOB"]}", smallFont));
                            table.AddCell(new Phrase($"Gender: {row["Gender"]}", smallFont));
                            table.AddCell(new Phrase($"Mobile No1: {row["Mobile_No1"]}", smallFont));
                            table.AddCell(new Phrase($"Mobile No2: {row["Mobile_No2"]}", smallFont));
                            table.AddCell(new Phrase($"Mobile No3: {row["Mobile_No3"]}", smallFont));
                            table.AddCell(new Phrase($"Vehicle No1: {row["Vechile_No1"]}", smallFont));
                            table.AddCell(new Phrase($"Vehicle No2: {row["Vechile_No2"]}", smallFont));
                            table.AddCell(new Phrase($"Vehicle No3: {row["Vechile_No3"]}", smallFont));
                            table.AddCell(new Phrase($"House No1: {row["House_No1"]}", smallFont));
                            table.AddCell(new Phrase($"House No2: {row["House_No2"]}", smallFont));
                            table.AddCell(new Phrase($"House No3: {row["House_No3"]}", smallFont));
                            table.AddCell(new Phrase($"Email Id: {row["Email_Id"]}", smallFont));
                            table.AddCell(new Phrase($"IsActive: {row["IsActive"]}", smallFont));
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
                        { "EssenceNo", "ESSENCE NUMBER" },
                        { "AgeYears", "Age" }
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
                                dataTable.AddCell(new PdfPCell(new Phrase(displayName, smallFont)));
                                dataTable.AddCell(new PdfPCell(new Phrase(data.Rows[0][sqlColumnName].ToString(), smallFont)));
                            }
                        }

                        document.Add(dataTable);
                        document.NewPage();

                        DataTable prdedData = await _predictionservice.PredictionPlanetsGET(birthDate, gender, clientId);

                        Paragraph titleLifePhase = new Paragraph("Life Phases", fontTitle);
                        titleLifePhase.Alignment = Element.ALIGN_LEFT;
                        document.Add(titleLifePhase);
                        document.Add(new Chunk("\n"));
                        // Create tables for each phase
                        //PdfPTable tableP1C1 = CreatePhaseTable(data.Rows[0]["P1"].ToString(), data.Rows[0]["C1"].ToString(), data.Rows[0]["Range1"].ToString(), "P1", "C1");
                        //PdfPTable tableP2C2 = CreatePhaseTable(data.Rows[0]["P2"].ToString(), data.Rows[0]["C2"].ToString(), data.Rows[0]["Range2"].ToString(), "P2", "C2");
                        //PdfPTable tableP3C3 = CreatePhaseTable(data.Rows[0]["P3"].ToString(), data.Rows[0]["C3"].ToString(), data.Rows[0]["Range3"].ToString(), "P3", "C3");
                        //PdfPTable tableP4C4 = CreatePhaseTable(data.Rows[0]["P4"].ToString(), data.Rows[0]["C4"].ToString(), data.Rows[0]["Range4"].ToString(), "P4", "C4");

                        //// Create a parent table to hold all phase tables side by side
                        //PdfPTable parentTable = new PdfPTable(4);
                        //parentTable.WidthPercentage = 100;
                        //parentTable.DefaultCell.Border = Rectangle.NO_BORDER;

                        //// Add phase tables to the parent table
                        //parentTable.AddCell(new PdfPCell(tableP1C1) { Border = Rectangle.NO_BORDER });
                        //parentTable.AddCell(new PdfPCell(tableP2C2) { Border = Rectangle.NO_BORDER });
                        //parentTable.AddCell(new PdfPCell(tableP3C3) { Border = Rectangle.NO_BORDER });
                        //parentTable.AddCell(new PdfPCell(tableP4C4) { Border = Rectangle.NO_BORDER });



                        //document.Add(parentTable);

                        // Create tables for each phase
                        PdfPTable tableP1C1 = CreatePhaseTable(data.Rows[0]["P1"].ToString(), data.Rows[0]["C1"].ToString(), data.Rows[0]["Range1"].ToString());
                        PdfPTable tableP2C2 = CreatePhaseTable(data.Rows[0]["P2"].ToString(), data.Rows[0]["C2"].ToString(), data.Rows[0]["Range2"].ToString());
                        PdfPTable tableP3C3 = CreatePhaseTable(data.Rows[0]["P3"].ToString(), data.Rows[0]["C3"].ToString(), data.Rows[0]["Range3"].ToString());
                        PdfPTable tableP4C4 = CreatePhaseTable(data.Rows[0]["P4"].ToString(), data.Rows[0]["C4"].ToString(), data.Rows[0]["Range4"].ToString());

                        // Create a parent table with 4 columns to hold each phase side by side
                        PdfPTable parentTable = new PdfPTable(4);
                        parentTable.WidthPercentage = 100;

                        // Add header row
                        Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                        parentTable.AddCell(new PdfPCell(new Phrase("First Phase", smallFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        parentTable.AddCell(new PdfPCell(new Phrase("Second Phase", smallFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        parentTable.AddCell(new PdfPCell(new Phrase("Third Phase", smallFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        parentTable.AddCell(new PdfPCell(new Phrase("Fourth Phase", smallFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // Add phase and cycle values
                        //parentTable.AddCell(tableP1C1);
                        //parentTable.AddCell(tableP2C2);
                        //parentTable.AddCell(tableP3C3);
                        //parentTable.AddCell(tableP4C4);


                        // Add phase tables to the parent table
                        parentTable.AddCell(new PdfPCell(tableP1C1) { Border = Rectangle.NO_BORDER });
                        parentTable.AddCell(new PdfPCell(tableP2C2) { Border = Rectangle.NO_BORDER });
                        parentTable.AddCell(new PdfPCell(tableP3C3) { Border = Rectangle.NO_BORDER });
                        parentTable.AddCell(new PdfPCell(tableP4C4) { Border = Rectangle.NO_BORDER });

                        // Add the parent table to the document


                        document.Add(parentTable);

                        if (prdedData.Rows.Count > 0)
                        {
                            string LifePhaseJson = string.Empty;
                            LifePhaseJson = prdedData.Rows[0][13].ToString();
                            string htmlLifePhaseContent = string.Empty;


                            if (LifePhaseJson != "") { htmlLifePhaseContent = ConvertJsonToHtml(LifePhaseJson, "Life Phase", "LifePhase_Description"); }
                            string htmlLifePhase = $"<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlLifePhaseContent}</div>";



                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(htmlLifePhase);
                            doc.OptionFixNestedTags = true;
                            doc.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterLifePhase = new StringWriter();
                            doc.Save(stringWriterLifePhase);
                            string cleanedHtmlContentLifePhase = stringWriterLifePhase.ToString();

                            Paragraph titlePlanet = new Paragraph("Life Phase Predictions", fontTitle);
                            titlePlanet.Alignment = Element.ALIGN_LEFT;
                            document.Add(titlePlanet);
                            document.Add(new Chunk("\n"));
                            using (StringReader sr = new StringReader(cleanedHtmlContentLifePhase))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

                            }



                        }

                        Paragraph titleComp = new Paragraph("Compatibility", fontTitle);
                        title.Alignment = Element.ALIGN_LEFT;
                        document.Add(titleComp);
                        // Add a line break to push content down if necessary
                        document.Add(new Chunk("\n"));
                        //PdfPTable table22 = new PdfPTable(4); // 4 columns for Item, Number, DigitNo, and Compatibility
                        //document.Add(new Chunk("\n")); // Line break
                        //                               // Add headers
                        //table22.AddCell("Item");
                        //table22.AddCell("Number");
                        //table22.AddCell("DigitNo");
                        //table22.AddCell("Compatibility");


                        //foreach (DataRow row in data.Rows)
                        //{
                        //    table22.AddCell(row["Item"].ToString());
                        //    table22.AddCell(row["Number"].ToString());
                        //    table22.AddCell(row["DigitNo"].ToString());
                        //    table22.AddCell(row["Compatibility"].ToString());
                        //}

                        //document.Add(table22);


                        // Create PdfPTable with 4 columns for Item, Number, DigitNo, and Compatibility
                        PdfPTable table22 = new PdfPTable(4);

                        // Add headers with smallFont
                        table22.AddCell(new Phrase("Item", smallFont));
                        table22.AddCell(new Phrase("Number", smallFont));
                        table22.AddCell(new Phrase("DigitNo", smallFont));
                        table22.AddCell(new Phrase("Compatibility", smallFont));

                        // Variables to track the current Compatibility Type
                        string currentCompatibilityType = string.Empty;

                        foreach (DataRow row in data.Rows)
                        {
                            // Check if the current row's Compatibility Type is different from the last one
                            string compatibilityType = row["CompatibilityType"].ToString();
                            if (compatibilityType != currentCompatibilityType)
                            {
                                // Add a new row with the Compatibility Type as a header
                                PdfPCell headerCell = new PdfPCell(new Phrase(compatibilityType, smallFont));
                                headerCell.Colspan = 4; // Span across all columns
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                headerCell.BackgroundColor = BaseColor.LIGHT_GRAY; // Optional: Set background color for the header
                                table22.AddCell(headerCell);

                                // Update the current Compatibility Type
                                currentCompatibilityType = compatibilityType;
                            }

                            // Add the data cells with smallFont
                            table22.AddCell(new Phrase(row["Item"].ToString(), smallFont));
                            table22.AddCell(new Phrase(row["Number"].ToString(), smallFont));
                            table22.AddCell(new Phrase(row["DigitNo"].ToString(), smallFont));
                            table22.AddCell(new Phrase(row["Compatibility"].ToString(), smallFont));
                        }

                        // Add the table to the document
                        document.Add(new Chunk("\n")); // Line break before the table
                        document.Add(table22);

                        // Loop through the data again to display compatibility messages after the table
                        foreach (DataRow row in data.Rows)
                        {
                            string compatibility = row["Compatibility"].ToString();
                            string compatibilityType = row["CompatibilityType"].ToString();

                            if (compatibility == "Neutral" || compatibility == "NonFriends")
                            {
                                // Create a new paragraph with the compatibility message using smallFont
                                string number = row["Number"].ToString();
                                string message = $"{number} is not compatible with '{compatibilityType}' name or number. To check compatibility, connect with Numeromystic.";

                                Paragraph compatibilityMessage = new Paragraph(message, smallFont);
                                compatibilityMessage.Alignment = Element.ALIGN_LEFT;

                                // Add the paragraph to the document
                                document.Add(new Chunk("\n")); // Optional: Add a line break before the message
                                document.Add(compatibilityMessage);
                            }
                        }
                        document.NewPage();
                        string jsonResult = string.Empty;
                        string SubPrediction_Master_JSON = string.Empty;
                        string Prediction_Destiny_JSON = string.Empty;
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


                        string SoulPrediction_MasterJson = string.Empty;
                        string Essence_Prediction_MasterJson = string.Empty;

                        if (prdedData.Rows.Count > 0)
                        {
                            jsonResult = prdedData.Rows[0][0].ToString();
                            Prediction_Destiny_JSON = prdedData.Rows[0][1].ToString();
                            SubPrediction_Master_JSON = prdedData.Rows[0][2].ToString();

                            MissingNumberMaster_JSON = prdedData.Rows[0][3].ToString();
                            Remedies_Master_JSON = prdedData.Rows[0][4].ToString();
                            RepetitivePrediction_JSON = prdedData.Rows[0][5].ToString();

                            PersonalYear1 = prdedData.Rows[0][7].ToString();
                            PersonalYear2 = prdedData.Rows[0][8].ToString();
                            PersonalYear3 = prdedData.Rows[0][9].ToString();

                            FrontNamePrediction = prdedData.Rows[0][10].ToString();
                            FullNamePrediction = prdedData.Rows[0][11].ToString();
                            MindNumberPredictionJson = prdedData.Rows[0][12].ToString();


                            SoulPrediction_MasterJson = prdedData.Rows[0][14].ToString();
                            Essence_Prediction_MasterJson = prdedData.Rows[0][15].ToString();


                            string htmlMissingNumbers = ConvertJsonToHtml(MissingNumberMaster_JSON, "Weakness", "MissingDescription");
                            string htmlRemedies = ConvertJsonToHtml(Remedies_Master_JSON, "Remedies", "Remedies_Description");
                            string htmlRepetitivePrediction = ConvertJsonToHtml(RepetitivePrediction_JSON, "Repetative Numbers", "Description");
                            string htmlContent = ConvertJsonToHtml(jsonResult, "Planet Strength", "Prediction_Description");

                            string htmlDestinyContent = ConvertJsonToHtml(Prediction_Destiny_JSON, "Destiny", "Prediction_Description");

                            string htmlSoulContent = ConvertJsonToHtml(SoulPrediction_MasterJson, "Soul", "Soul_Description");
                            string htmlEssenceContent = ConvertJsonToHtml(Essence_Prediction_MasterJson, "Essence", "Essence_Description");

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

                            string htmlPersonalYear1 = ConvertJsonToHtml(PersonalYear1, "Personal Year Prediction " + YearName1, "PersonalYearNumber_Description");
                            string htmlPersonalYear2 = ConvertJsonToHtml(PersonalYear2, "Personal Year Prediction " + YearName2, "PersonalYearNumber_Description");
                            string htmlPersonalYear3 = ConvertJsonToHtml(PersonalYear3, "Personal Year Prediction " + YearName3, "PersonalYearNumber_Description");


                            string htmlFrontNamePrediction = ConvertJsonToHtml(FrontNamePrediction, "Front Name Prediction", "NameNumber_Description");
                            string htmlFullNamePrediction = ConvertJsonToHtml(FullNamePrediction, "Full Name Prediction", "NameNumber_Description");
                            if (MindNumberPredictionJson != "") { htmlMindNumberPrediction = ConvertJsonToHtml(MindNumberPredictionJson, "Mind Numbers", "MIndNoDescription"); }



                            // Define custom font CSS
                            string customFontCss = $@"
                            <style>
                                @font-face {{
                                    font-family: 'ShadowsIntoLight';
                                    src: url('{fontPathNew}');
                                }}
                                body {{
                                    font-family: 'ShadowsIntoLight';
                                }}
                            </style>";


                            // Wrap HTML content with font CSS and necessary styles
                            htmlMissingNumbers = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlMissingNumbers}</div>";
                            htmlRemedies = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlRemedies}</div>";
                            htmlRepetitivePrediction = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlRepetitivePrediction}</div>";

                            htmlPersonalYear1 = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlPersonalYear1}</div>";
                            htmlPersonalYear2 = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlPersonalYear2}</div>";
                            htmlPersonalYear3 = $"{customFontCss}<div style='width: 100%;'>{htmlPersonalYear3}</div>";

                            htmlFrontNamePrediction = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlFrontNamePrediction}</div>";
                            htmlFullNamePrediction = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlFullNamePrediction}</div>";

                            htmlMindNumberPrediction = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlMindNumberPrediction}</div>";
                            string htmlContentWithWidth = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlContent}</div>";

                            string htmlDestiny = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlDestinyContent}</div>";


                            string htmlSoul = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlSoulContent}</div>";
                            string htmlEssence = $"{customFontCss}<div style='width: 100%;' font-family: 'ShadowsIntoLight'>{htmlEssenceContent}</div>";

                            //// Ensure the table uses 100% width
                            //string htmlContentWithWidth = $"<div style='width: 100%;'>{htmlContent}</div>";
                            //// Ensure each table uses 100% width
                            //htmlMissingNumbers = $"<div style='width: 100%;'>{htmlMissingNumbers}</div>";
                            //htmlRemedies = $"<div style='width: 100%;'>{htmlRemedies}</div>";
                            //htmlRepetitivePrediction = $"<div style='width: 100%;'>{htmlRepetitivePrediction}</div>";

                            //htmlPersonalYear1 = $"<div style='width: 100%;'>{htmlPersonalYear1}</div>";
                            //htmlPersonalYear2 = $"<div style='width: 100%;'>{htmlPersonalYear2}</div>";
                            //htmlPersonalYear3 = $"<div style='width: 100%;'>{htmlPersonalYear3}</div>";

                            //htmlFrontNamePrediction = $"<div style='width: 100%;'>{htmlFrontNamePrediction}</div>";
                            //htmlFullNamePrediction = $"<div style='width: 100%;'>{htmlFullNamePrediction}</div>";

                            //htmlMindNumberPrediction = $"<div style='width: 100%;'>{htmlMindNumberPrediction}</div>";


                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(htmlContentWithWidth);
                            doc.OptionFixNestedTags = true;
                            doc.OptionWriteEmptyNodes = true;

                            StringWriter stringWriter = new StringWriter();
                            doc.Save(stringWriter);
                            string cleanedHtmlContent = stringWriter.ToString();

                            Paragraph titlePlanet = new Paragraph("Birth No Strength", fontTitle);
                            titlePlanet.Alignment = Element.ALIGN_LEFT;
                            document.Add(titlePlanet);
                            document.Add(new Chunk("\n")); // Line break
                            using (StringReader sr = new StringReader(cleanedHtmlContent))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr, null, Encoding.UTF8, customFont);
                            }


                            document.NewPage();

                            HtmlDocument docDestiny = new HtmlDocument();
                            docDestiny.LoadHtml(htmlDestiny);
                            docDestiny.OptionFixNestedTags = true;
                            docDestiny.OptionWriteEmptyNodes = true;


                            StringWriter stringWriterDestiny = new StringWriter();
                            docDestiny.Save(stringWriterDestiny);
                            string cleanedDestiny = stringWriterDestiny.ToString();


                            Paragraph titleDestiny = new Paragraph("How your Life Path Will be? (Destiny)", fontTitle);
                            titleDestiny.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleDestiny);
                            document.Add(new Chunk("\n")); // Line break
                            using (StringReader sr = new StringReader(cleanedDestiny))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr, null, Encoding.UTF8, customFont);
                            }



                            //Mind No
                            document.NewPage();

                            HtmlDocument docMindNumberPrediction = new HtmlDocument();
                            docMindNumberPrediction.LoadHtml(htmlMindNumberPrediction);
                            docMindNumberPrediction.OptionFixNestedTags = true;
                            docMindNumberPrediction.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterMindNumberPrediction = new StringWriter();
                            docMindNumberPrediction.Save(stringWriterMindNumberPrediction);
                            string cleanedhtmlMindNumberPrediction = stringWriterMindNumberPrediction.ToString();
                            Paragraph titleMindNo = new Paragraph("Total Strength", fontTitle);
                            titleMindNo.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleMindNo);
                            document.Add(new Chunk("\n"));

                            using (StringReader sr = new StringReader(cleanedhtmlMindNumberPrediction))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }



                            document.NewPage();
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


                            Paragraph titleWeakness = new Paragraph("Weakness", fontTitle);
                            titleWeakness.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleWeakness);
                            document.Add(new Chunk("\n")); // Line break

                            // Add PdfPTables to the document
                            PdfPTable tableMissingNumbers = new PdfPTable(1); // 1 column
                            tableMissingNumbers.WidthPercentage = 100;

                            using (StringReader sr = new StringReader(cleanedHtmlMissingNumbers))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }
                            document.Add(tableMissingNumbers);





                            document.NewPage();
                            Paragraph titleRepetitive = new Paragraph("Positive Numbers", fontTitle);
                            titleRepetitive.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleRepetitive);
                            document.Add(new Chunk("\n")); // Line break
                            PdfPTable tableRepetitivePrediction = new PdfPTable(1);
                            tableRepetitivePrediction.WidthPercentage = 100;

                            using (StringReader sr = new StringReader(cleanedHtmlRepetitivePrediction))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }
                            document.Add(tableRepetitivePrediction);
                            document.NewPage();


                            //Personal Year 

                            HtmlDocument docPersonalYear = new HtmlDocument();
                            docPersonalYear.LoadHtml(htmlPersonalYear1);
                            docPersonalYear.OptionFixNestedTags = true;
                            docPersonalYear.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterPersonalYear = new StringWriter();
                            docPersonalYear.Save(stringWriterPersonalYear);
                            string cleanedhtmlPersonalYear1 = stringWriterPersonalYear.ToString();

                            Paragraph titlePersonalYear = new Paragraph("Your Prediction for " + YearName1, fontTitle);
                            titlePersonalYear.Alignment = Element.ALIGN_LEFT;
                            document.Add(titlePersonalYear);
                            document.Add(new Chunk("\n")); // Line break

                            using (StringReader sr = new StringReader(cleanedhtmlPersonalYear1))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }

                            document.NewPage();

                            HtmlDocument docPersonalYear2 = new HtmlDocument();
                            docPersonalYear2.LoadHtml(htmlPersonalYear2);
                            docPersonalYear2.OptionFixNestedTags = true;
                            docPersonalYear2.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterPersonalYear2 = new StringWriter();
                            docPersonalYear2.Save(stringWriterPersonalYear2);
                            string cleanedhtmlPersonalYear2 = stringWriterPersonalYear2.ToString();

                            Paragraph titlePersonalYear2 = new Paragraph("Your Prediction for " + YearName2, fontTitle);
                            titlePersonalYear2.Alignment = Element.ALIGN_LEFT;
                            document.Add(titlePersonalYear2);
                            document.Add(new Chunk("\n")); // Line break
                            using (StringReader sr = new StringReader(cleanedhtmlPersonalYear2))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }

                            document.NewPage();

                            HtmlDocument docPersonalYear3 = new HtmlDocument();
                            docPersonalYear3.LoadHtml(htmlPersonalYear3);
                            docPersonalYear3.OptionFixNestedTags = true;
                            docPersonalYear3.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterPersonalYear3 = new StringWriter();
                            docPersonalYear3.Save(stringWriterPersonalYear3);
                            string cleanedhtmlPersonalYear3 = stringWriterPersonalYear3.ToString();


                            Paragraph titlePersonalYear3 = new Paragraph("Your Prediction for" + YearName3, fontTitle);
                            titlePersonalYear3.Alignment = Element.ALIGN_LEFT;
                            document.Add(titlePersonalYear3);
                            document.Add(new Chunk("\n")); // Line break
                            using (StringReader sr = new StringReader(cleanedhtmlPersonalYear3))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }

                            document.NewPage();

                            //Front Name Fulll Name

                            HtmlDocument docFrontNamePrediction = new HtmlDocument();
                            docFrontNamePrediction.LoadHtml(htmlFrontNamePrediction);
                            docFrontNamePrediction.OptionFixNestedTags = true;
                            docFrontNamePrediction.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterFrontNamePrediction = new StringWriter();
                            docFrontNamePrediction.Save(stringWriterFrontNamePrediction);
                            string cleanedhtmlFrontNamePrediction = stringWriterFrontNamePrediction.ToString();

                            Paragraph titleFrontName = new Paragraph("Front Name Prediction For Your Carrier /Bussiness", fontTitle);
                            titleFrontName.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleFrontName);
                            document.Add(new Chunk("\n"));
                            using (StringReader sr = new StringReader(cleanedhtmlFrontNamePrediction))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }
                            document.NewPage();

                            HtmlDocument docFullNamePrediction = new HtmlDocument();
                            docFullNamePrediction.LoadHtml(htmlFullNamePrediction);
                            docFullNamePrediction.OptionFixNestedTags = true;
                            docFullNamePrediction.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterFullNamePrediction = new StringWriter();
                            docFullNamePrediction.Save(stringWriterFullNamePrediction);
                            string cleanedhtmlFullNamePrediction = stringWriterFullNamePrediction.ToString();
                            Paragraph titleFullName = new Paragraph("Full Name Prediction for you Life Path ,Desitny", fontTitle);
                            titleFullName.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleFullName);
                            document.Add(new Chunk("\n"));

                            using (StringReader sr = new StringReader(cleanedhtmlFullNamePrediction))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            }

                            document.NewPage();
                            //Paragraph titleRemedies = new Paragraph("Remedies", fontTitle);
                            //titleRemedies.Alignment = Element.ALIGN_LEFT;
                            //document.Add(titleRemedies);

                            //PdfPTable tableRemedies = new PdfPTable(1); // 1 column
                            //tableRemedies.WidthPercentage = 100;
                            //using (StringReader sr = new StringReader(cleanedHtmlRemedies))
                            //{
                            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                            //}
                            //document.Add(tableRemedies);


                            ///Soul Prediction
                            ///
                            HtmlDocument docSoul = new HtmlDocument();
                            docSoul.LoadHtml(htmlSoul);
                            docSoul.OptionFixNestedTags = true;
                            docSoul.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterSoul = new StringWriter();
                            docSoul.Save(stringWriterSoul);
                            string cleanedHtmlContentSoul = stringWriterSoul.ToString();

                            Paragraph titleSoul = new Paragraph("Soul Prediction", fontTitle);
                            titleSoul.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleSoul);
                            document.Add(new Chunk("\n"));
                            using (StringReader sr = new StringReader(cleanedHtmlContentSoul))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

                            }
                            document.NewPage();
                            ///Essence Prediction
                            ///
                            HtmlDocument docEssence = new HtmlDocument();
                            docEssence.LoadHtml(htmlEssence);
                            docEssence.OptionFixNestedTags = true;
                            docEssence.OptionWriteEmptyNodes = true;

                            StringWriter stringWriterEssence = new StringWriter();
                            docEssence.Save(stringWriterEssence);
                            string cleanedHtmlContentEssence = stringWriterEssence.ToString();

                            Paragraph titleEssence = new Paragraph("Essence Prediction", fontTitle);
                            titleEssence.Alignment = Element.ALIGN_LEFT;
                            document.Add(titleEssence);
                            document.Add(new Chunk("\n"));
                            using (StringReader sr = new StringReader(cleanedHtmlContentEssence))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

                            }
                            document.NewPage();
                            // Add a line break to push content down if necessary
                            document.Add(new Chunk("\n"));



                            // Use these fonts throughout your document
                            // For example:

                            // Add the disclaimer
                            Paragraph disclaimer = new Paragraph();
                            disclaimer.Add(new Chunk("Disclaimer:\n", titleFont));
                            disclaimer.Add(new Chunk("Numerology is an old and respected system of divination. It is often but not always accurate. The readings and interpretations are always subordinate to individuals' beliefs and their power to choose who and what they are now and will become.\n\n", smallFont));
                            disclaimer.Add(new Chunk("Please be advised that numerology readings cannot predict, forecast, diagnose or provide information with absolute certainty. No guarantees or assurances of any kind are given, and the author will not be held accountable for any interpretations or decisions made by recipients based on information provided in readings. Readings are for knowledge purposes only. For legal or medical concerns, please consult with a lawyer or physician.\n\n", smallFont));
                            disclaimer.Add(new Chunk("The efficacy of any kind of remedy recommendation (energiser, yantra or facilitating the same) will strictly depend on the faith and devotion of the customer and his belief and intent. Numerology Centre does not offer any guaranteed results in the life of the customer for the recommendation of the remedies.\n\n", smallFont));

                            disclaimer.Add(new Chunk("It be works when the client is has more faith and disruptive in his mindset the client is ready to change his mindset, actions, beliefs. As remedies with patience and faith, slowly the pattern changes and the hurdles are to now diminishing in pattern.\n\n", smallFont));
                            // disclaimer.Add(new Chunk("Images or text from these pages may not be resold, redistributed or republished without prior written permission from Numerocure.", smallFont));
                            disclaimer.Alignment = Element.ALIGN_JUSTIFIED;
                            document.Add(disclaimer);

                            // Add a page break after the disclaimer
                            document.NewPage();



                        }

                        document.Close();
                    }

                    System.IO.File.WriteAllBytes(filePath, memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {

                errorMessage = $"An error occurred while generating the PDF: {ex.Message}";
            }

            return (filePath, errorMessage);
        }





        public string ConvertJsonToHtml(string jsonResult, string Title, string htmlPrintKey)
        {
            var sb = new StringBuilder();
            var jsonArray = JArray.Parse(jsonResult);

            string fontPath = Path.Combine("Fonts", "ShadowsIntoLight.ttf");
            // Define custom font CSS
            string customFontCss = $@"
            <style>
                @font-face {{
                    font-family: 'ShadowsIntoLight';
                    src: url('{fontPath}');
                }}
                body {{
                    font-family: 'ShadowsIntoLight';
                }}


            </style>";


            sb.Append("<html>");
            sb.Append(customFontCss);

            //   sb.Append("<h2>" + Title + "</h2>");
            foreach (var jsonObject in jsonArray)
            {

                sb.Append("<table style='width: 100%;'>");


                var Description = jsonObject[htmlPrintKey].ToString();

                sb.Append("<tr>");
                sb.Append($"<td  style='font-family: ShadowsIntoLight; line-height: 2.5 !important; text-align: justify;'>{Description}</td>");
                sb.Append("</tr>");

                sb.Append("</table>");
                sb.Append("<br>");
            }

            sb.Append("</body></html>");

            return sb.ToString();
        }






        private PdfPTable CreatePhaseTable(string phaseValue, string cycleValue, string footerText)
        {
            PdfPTable table = new PdfPTable(2); // 2 columns: Phase and Cycle
            table.WidthPercentage = 100;

            Font fontData = FontFactory.GetFont("Arial", 12);
            Font fontFooter = FontFactory.GetFont("Arial", 12, Font.ITALIC);
            // Add phase and cycle data in a single row
            table.AddCell(new PdfPCell(new Phrase("P " + phaseValue, fontData)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("C " + cycleValue, fontData)) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Add table footer
            PdfPCell footerCell = new PdfPCell(new Phrase(footerText, fontFooter));
            footerCell.Colspan = 2;
            footerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(footerCell);

            return table;
        }


        private PdfPTable CreatePhaseTableOLD(string phaseValue, string cycleValue, string footerText, string phaseHeader, string cycleHeader)
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
                sb.Append("<h2>" + Title + "</h2>");
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
                System.IO.File.WriteAllBytes(filePath, memoryStream.ToArray());
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
                .Where(n => n.Name == "meta")
                .ToList()
                .ForEach(n => n.Remove());

            // Return cleaned HTML
            return doc.DocumentNode.OuterHtml;
        }

        [HttpGet("CalculateLoShuGrid")]
        public async Task<IActionResult> CalculateLoShuGrid(string BirthDate, string Gender, string Client_id)
        {

            try
            {

                DataTable response = await _service.CalculateLoShuGrid(BirthDate, Gender, Client_id);
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

        [HttpGet("GetVowelSum")]
        public async Task<IActionResult> GetVowelSum(string name)
        {

            try
            {

                DataTable response = await _service.GetVowelSum(name);
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

        [HttpGet("CalculateCharachterValue")]
        public async Task<IActionResult> CalculateCharachterValue(string name)
        {
            try
            {
                // Call the method that executes the stored procedure
                string result = await _service.GetCharacterValueAsync(name);

                // Return the result as an Ok response
                return Ok(new { CharacterValue = result });
            }
            catch (System.Exception ex)
            {
                // Handle any exceptions that may occur
                return StatusCode(500, ex.Message);
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
public class PageEventHelper : PdfPageEventHelper
{
    private int pageNumber = 1;

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);

        PdfPTable table = new PdfPTable(1);
        table.TotalWidth = 500f;
        table.WidthPercentage = 100;

        PdfPCell cell = new PdfPCell(new Phrase($"Page {pageNumber}", FontFactory.GetFont("Arial", 10, Font.NORMAL)))
        {
            Border = PdfPCell.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_RIGHT
        };
        table.AddCell(cell);
        table.WriteSelectedRows(0, -1, document.PageSize.GetRight(50), document.PageSize.GetBottom(30), writer.DirectContent);

        pageNumber++;
    }
}