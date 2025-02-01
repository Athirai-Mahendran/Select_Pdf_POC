using SelectPdf;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Select_Pdf_POC
{
    public class Program
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();

            string localHtmlFilePath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\SampleHTML.html";

            if (!File.Exists(localHtmlFilePath))
            {
                Console.WriteLine("File not found: " + localHtmlFilePath);
                return;
            }

            string htmlContent = File.ReadAllText(localHtmlFilePath);

            try
            {
                // Create an instance of the HTML to PDF converter
                HtmlToPdf converter = new HtmlToPdf();

                // Basic Page Setup
                converter.Options.PdfPageSize = PdfPageSize.A4;
                //  converter.Options.PdfPageCustomSize = new SizeF(595, 800); // Example dimensions
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.MarginTop = 0;    // Increased margin for header
                converter.Options.MarginBottom = 04;  // Increased margin for footer
                converter.Options.MarginLeft = 40;
                converter.Options.MarginRight = 40;


                converter.Options.DisplayHeader = true;
                converter.Header.Height = 70;
                converter.Options.DisplayFooter = true;
                converter.Footer.Height = 40;
                //converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                //converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.ShrinkOnly;

                //converter.Options.PdfPageSize = PdfPageSize.Custom;
                //converter.Options.PdfPageSize = PdfPageSize.A1(
                //    Pdf= new SizeF(595, 1500); // Width x Height in points

                //converter.Options.PdfPageCustomSize = new SizeF(595, 1500);

                // Set WebPageOptions
                // Set the web page height in pixels
                //converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                //converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                //converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.ShrinkOnly;

                // converter.Options.WebPageHeight = 800;
                // converter.Options.WebPageOptions.WebPageWidth = 1024; // Set the web page width in pixels (optional)

                // Convert HTML string to PDF
                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // Create fonts
                PdfFont headerFont = doc.AddFont(PdfStandardFont.Helvetica);
                headerFont.Size = 7;

                PdfFont titleFont = doc.AddFont(PdfStandardFont.HelveticaBold);
                titleFont.Size = 7;

                PdfFont footerFont = doc.AddFont(PdfStandardFont.Helvetica);
                footerFont.Size = 7;

                // Header template
                doc.Header = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 100);

                // Add logo
                string logoPath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\images\refinePics.jpg";
                string carlheaderPath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\Calrstahl_images\CSLET_EIAC_HeaderCopy.jpg";
                string carlFooterPath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\Calrstahl_images\CSSSC_EIAC_Footer.jpg";

                PdfImageElement logos = new PdfImageElement(
                    0,
                    // (doc.Pages[0].ClientRectangle.Width - 170) / 2, // Center horizontally
                    0, // Top margin
                    carlheaderPath
                );
                // logos.Width = 100; // Set logo width
                // logos.Height = 50; // Set logo height
                doc.Header.Add(logos);

                PdfImageElement footer = new PdfImageElement(
                    0,
                    0, // Top margin
                    carlFooterPath
                );
                // footer.Width = doc.Pages[0].ClientRectangle.Width;
                doc.Footer.Add(footer);

                // Add page number text
                //PdfTextElement pageNumberText = new PdfTextElement(0, 40, "Page {page_number} of {total_pages}", footerFont);
                //pageNumberText.ForeColor = Color.Gray;
                //pageNumberText.Width = doc.Pages[0].ClientRectangle.Width;

                // Get the page width
                float footerPageWidth = doc.Pages[0].ClientRectangle.Width;

                // Measure the width of the page number text
                string pageNumberTextValue = "Page {page_number} of {total_pages}";
                float textWidth = footerFont.GetTextWidth(pageNumberTextValue);

                // Calculate the X position to align the text to the right
                float rightAlignedX = footerPageWidth - textWidth - 20; // 10 is the right margin

                // Add the page number text
                PdfTextElement pageNumberText = new PdfTextElement(rightAlignedX + 100, 10, pageNumberTextValue, footerFont);
                pageNumberText.ForeColor = Color.Black;

                // Add the text to the footer
                doc.Footer.Add(pageNumberText);


                //------------------old header
                //string logoPath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\images\logo.png";
                //PdfImageElement logo = new PdfImageElement(
                //    (doc.Pages[0].ClientRectangle.Width - 170) / 2, // Center horizontally
                //    5, // Top margin
                //    logoPath
                //);
                //logo.Width = 100; // Set logo width
                //logo.Height = 50; // Set logo height
                //doc.Header.Add(logo);

                //// Add left side header text
                //PdfTextElement leftHeader = new PdfTextElement(20, 10,
                //    "CarlStahl Safety & Security Consultancy\n" +
                //    "P.O. Box : 26607, Dubai - U.A.E.\n" +
                //    "Tel. : +971 4 333 3494\n" +
                //    "         +971 2 550 4443\n" +
                //    "E-mail : Dubai@CarlStahl.ae\n" +
                //    "          : AbuDhabi@CarlStahl.ae\n" +
                //    "Website : www.carlstahl.ae", headerFont);
                //doc.Header.Add(leftHeader);

                //double rightAlignedX = doc.Pages[0].ClientRectangle.Width - 350;

                //// Add right side header text (Arabic)
                //PdfTextElement rightHeader = new PdfTextElement(
                //   350, 0,
                //  // doc.Pages[0].ClientRectangle.Width - 350, 0,
                //  "كارل شتال لاستشارات الامن والسلامة\n" +
                //    "ص.ب : ٢٦٦٠٧ ، دبي - ا.ع.م\n" +
                //     "تليفون : ٣٤٩٤ ٣٣٣ ٤ ٩٧١+\n" +
                //        "البريد : Dubai@CarlStahl.ae\n" +
                //         "الانترنت : www.carlstahl.ae", headerFont);
                //doc.Header.Add(rightHeader);

                //// Add subtitle under logo
                //PdfTextElement subtitle = new PdfTextElement(
                //    (doc.Pages[0].ClientRectangle.Width - 200) / 2, // Center horizontally
                //    60, // Position under logo
                //    "German Technology Since 1880", titleFont);
                //doc.Header.Add(subtitle);

                ////hr rule 
                //float headerLineY = 70; // This should match your header height
                //PdfLineElement headerLine = new PdfLineElement(
                //    new PointF(0, headerLineY), // Start point
                //    new PointF(doc.Pages[0].ClientRectangle.Width, headerLineY) // End point
                //);
                //headerLine.BackColor = Color.Black; // Set line color
                //doc.Header.Add(headerLine); // Add line to header

                //------------------old header

                //----------Footer

                //float footerLineY = 10; // This should match your footer height
                //PdfLineElement footerLine = new PdfLineElement(
                //    new PointF(0, footerLineY), // Start point
                //    new PointF(doc.Pages[0].ClientRectangle.Width, footerLineY) // End point
                //);
                //footerLine.BackColor = Color.Black; // Set line color
                //doc.Footer.Add(footerLine); // Add line to header

                //// Footer template
                //doc.Footer = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 50);

                //// Add footer text
                //PdfTextElement footerText = new PdfTextElement(0, 20,
                //    "Document generated by Select.Pdf", footerFont);
                //footerText.ForeColor = Color.Gray;
                //// footerText.TextAlign = TextAlign.Center;
                //footerText.Width = doc.Pages[0].ClientRectangle.Width;
                //doc.Footer.Add(footerText);

                //// Add page numbers
                //PdfTextElement pageNumbers = new PdfTextElement(0, 35,
                //    "Page {page_number} of {total_pages}", footerFont);
                //pageNumbers.ForeColor = Color.AliceBlue;
                ////  pageNumbers.TextAlign = TextAlign.Center;
                //pageNumbers.Width = doc.Pages[0].ClientRectangle.Width;
                //doc.Footer.Add(pageNumbers);

                //----------Footer

                //for water Mark 

                //step
                // Add text to the PDF
                PdfTextElement textElement = new PdfTextElement(0, 0, "Text Customized Watermark, Select.Pdf!", headerFont);
                textElement.ForeColor = Color.Olive;
                textElement.Transparency = 100;
                // textElement.Rotate(20);
                //   doc.Pages[0].Add(textElement); - for single page

                ////step
                //PdfImageElement imageElement = new PdfImageElement(0, 0, logoPath);
                //imageElement.Width = 100; // Resize width
                //imageElement.Height = 75; // Resize height
                //imageElement.Transparency = 40;
                ////   doc.Pages[0].Add(imageElement); - for single page 

                ////step-3
                //PdfTextElement watermark = new PdfTextElement(0, 0, "CONFIDENTIAL", headerFont);
                //watermark.ForeColor = Color.Red;// Color.FromArgb(50, Color.DarkKhaki); // Make the watermark semi-transparent
                //watermark.Rotate(75);
                //watermark.Transparency = 70; // Add transparency

                foreach (PdfPage page in doc.Pages)
                {

                    float pageWidth = page.ClientRectangle.Width;
                    float pageHeight = page.ClientRectangle.Height;

                    // Calculate absolute center of the page
                    float centerX = pageWidth / 2;
                    float centerY = pageHeight / 2;

                    // Calculate center positions for each page
                    //float centerX = (doc.Pages[0].ClientRectangle.Width - watermark.Width) / 2;
                    //float centerY = (doc.Pages[0].ClientRectangle.Height - watermark.Height) / 2;

                    // Position watermarks at center
                    //watermark.X = centerX;    
                    //watermark.Y = centerY;
                    //watermark.X = centerX - watermark.Width / 2;
                    //watermark.Y = centerX - watermark.Width / 2;

                    //textElement.X = centerX - 50; // Offset slightly from center
                    //textElement.Y = centerY + 100; 
                    textElement.X = centerX - textElement.Width / 2;// Offset slightly from center
                    textElement.Y = centerY - textElement.Height / 2;

                    //imageElement.X = centerX - imageElement.Width / 2;
                    //imageElement.Y = centerY - imageElement.Height / 2;

                    //page.Add(watermark);
                    //page.Add(imageElement);
                    page.Add(textElement); // Add watermark to each page
                }

                //for water Mark 

                // Save the PDF document 
                string dateTimeNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputFilePath = $"D:\\Select.PDF_POC\\PDF\\Z_PDF_1_{dateTimeNow}.pdf";
                doc.Save(outputFilePath);
                doc.Close();

                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                timer.Stop();

                Console.WriteLine(foo +" , PDF successfully created by #2 at: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
    //// old - Program 
    //public class ProgramOld
    //{
    //    static void Main(string[] args)
    //    {
    //        //C:\Users\home\source\repos\Select_Pdf_POC\Content
    //        string localHtmlFilePath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\SampleHTML.html";
    //        //"D:\\Users\\home\\source\\repos\\Select_Pdf_POC\\Content\\SelectPdfDemo.html"

    //        //string localHtmlFilePath = @"D:\Select.PDF_POC\SelectPdfDemo.html";
    //        if (!File.Exists(localHtmlFilePath))
    //        {
    //            Console.WriteLine("File not found: " + localHtmlFilePath);
    //            return;
    //        }
    //        // Convert the file path to a proper URI
    //        //string fileUri = new Uri(localHtmlFilePath).AbsoluteUri;
    //        string htmlContent = File.ReadAllText(localHtmlFilePath);

    //        try
    //        {
    //            // Create an instance of the HTML to PDF converter
    //            HtmlToPdf converter = new HtmlToPdf();

    //            // Basic Page Setup
    //            converter.Options.PdfPageSize = PdfPageSize.A4;
    //            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
    //            converter.Options.MarginTop = 0;    // Adjust space for header
    //            converter.Options.MarginBottom = 0; // Adjust space for footer
    //            converter.Options.MarginLeft = 40;
    //            converter.Options.MarginRight = 40;

    //            // 1. Simple HTML Header and Footer
    //            converter.Options.DisplayHeader = true;
    //            converter.Header.Height = 30;
    //            converter.Header.FirstPageNumber = 1;
    //            converter.Options.DisplayFooter = true;
    //            converter.Footer.Height = 20;



    //            // Convert HTML string to PDF
    //            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

    //            // create a new pdf font
    //            PdfFont font2 = doc.AddFont(PdfStandardFont.Helvetica);
    //            font2.Size = 10;
    //            font2.IsUnderline = true;

    //            // get image path
    //            string imgFile = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\images\desk.jpg";

    //            // header template (100 points in height) with image element
    //            doc.Header = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 100);
    //            PdfImageElement img1 = new PdfImageElement(50, 50, imgFile);
    //            doc.Header.Add(img1);

    //            //alignment
    //            //PdfImageElement logo = new PdfImageElement(10, 10, 80, 40, imgFile); // (x, y, width, height)
    //            //doc.Header.Add(logo);

    //            doc.Footer = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 100);
    //            PdfTextElement text = new PdfTextElement(0, 50,
    //                "Footer text: Document generated by Select.Pdf", font2);
    //            text.ForeColor = Color.Brown;
    //            doc.Footer.Add(text);

    //            // Save the PDF document to a file
    //            //---string outputFilePath = "D:\\Select.PDF_POC\\PDF\\'+output_SampleHtml69+'.pdf";
    //            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");

    //            // Construct the dynamic file path
    //            string outputFilePath = $"D:\\Select.PDF_POC\\PDF\\output_{dateTimeNow}.pdf";
    //            doc.Save(outputFilePath);

    //            // Close the document
    //            doc.Close();

    //            Console.WriteLine("PDF successfully created at: " + outputFilePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("An error occurred: " + ex.Message);
    //        }

    //        Console.ReadLine();
    //    }

    //    //HtmlToPdf converter = new HtmlToPdf();
    //    //PdfDocument doc = converter.ConvertHtmlString("<h1>Hello PDF!</h1>");
    //    //doc.Save("output.pdf");
    //    //doc.Close();
    //    //Console.WriteLine("PDF created.");
    //}
    //// old - Program 

    ////----Async Program
    //public class ProgramAsync
    //{
    //    public static async Task Main(string[] args)
    //    {
    //        Console.WriteLine(DateTime.Now);

    //        string localHtmlFilePath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\SampleHTML.html";

    //        if (!File.Exists(localHtmlFilePath))
    //        {
    //            Console.WriteLine("File not found: " + localHtmlFilePath);
    //            return;
    //        }

    //        try
    //        {
    //            // Read HTML content asynchronously
    //            string htmlContent = File.ReadAllText(localHtmlFilePath);

    //            // Process the PDF creation asynchronously
    //            await GeneratePdfAsync(htmlContent);
    //            Console.WriteLine(DateTime.Now);

    //            Console.WriteLine("PDF processing completed.");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("An error occurred: " + ex.Message);
    //        }

    //        Console.ReadLine();
    //    }

    //    private static async Task GeneratePdfAsync(string htmlContent)
    //    {
    //        try
    //        {
    //            // Create an instance of the HTML to PDF converter
    //            HtmlToPdf converter = new HtmlToPdf();
    //            ConfigureConverter(converter);

    //            // Convert HTML string to PDF
    //            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

    //            // Add headers, footers, watermarks (reuse existing code here)
    //            AddHeaderFooter(doc);
    //            AddWatermarks(doc);

    //            // Save the PDF document asynchronously
    //            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    //            string outputFilePath = $"D:\\Select.PDF_POC\\PDF\\Async_Pg_Output_{dateTimeNow}.pdf";

    //            await Task.Run(() => doc.Save(outputFilePath)); // Save operation wrapped in a Task
    //            doc.Close();

    //            Console.WriteLine("PDF successfully created at: " + outputFilePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("An error occurred in GeneratePdfAsync: " + ex.Message);
    //        }
    //    }

    //    private static void ConfigureConverter(HtmlToPdf converter)
    //    {
    //        // Configuration (Reuse the configuration code)
    //        converter.Options.PdfPageCustomSize = new SizeF(595, 800);
    //        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
    //        converter.Options.MarginTop = 5;
    //        converter.Options.MarginBottom = 5;
    //        converter.Options.MarginLeft = 20;
    //        converter.Options.MarginRight = 20;

    //        converter.Options.DisplayHeader = true;
    //        converter.Header.Height = 70;
    //        converter.Options.DisplayFooter = true;
    //        converter.Footer.Height = 10;
    //    }

    //    private static void AddHeaderFooter(PdfDocument doc)
    //    {
    //        // Header and Footer Logic (Reuse your code for header and footer here)
    //        // Example:
    //        doc.Header = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 100);
    //        PdfFont headerFont = doc.AddFont(PdfStandardFont.Helvetica);
    //        headerFont.Size = 7;

    //        PdfTextElement headerText = new PdfTextElement(10, 10, "Custom Header Text", headerFont);
    //        doc.Header.Add(headerText);
    //    }

    //    private static void AddWatermarks(PdfDocument doc)
    //    {
    //        // Add Watermarks (Reuse your existing watermark code here)
    //        PdfFont headerFont = doc.AddFont(PdfStandardFont.Helvetica);
    //        PdfTextElement watermark = new PdfTextElement(0, 0, "CONFIDENTIAL", headerFont);
    //        watermark.ForeColor = Color.FromArgb(50, Color.DarkKhaki);
    //        watermark.Rotate(45);
    //        watermark.Transparency = 50;

    //        foreach (PdfPage page in doc.Pages)
    //        {
    //            page.Add(watermark);
    //        }
    //    }
    //}
    ////----Async Program

    ////-------------Parallel programming 
    //public class ProgramParallel
    //{
    //    public static async Task Main(string[] args)
    //    {
    //        Console.WriteLine(DateTime.Now);
    //        string[] htmlFilePaths = Directory.GetFiles(@"C:\Users\home\source\repos\Select_Pdf_POC\Content\", "*.html");

    //        if (htmlFilePaths.Length == 0)
    //        {
    //            Console.WriteLine("No HTML files found in the directory.");
    //            return;
    //        }

    //        try
    //        {
    //            // Process multiple files in parallel
    //            await Task.WhenAll(htmlFilePaths.Select(filePath => Task.Run(() => ProcessFileAsync(filePath))));
    //            Console.WriteLine(DateTime.Now);

    //            Console.WriteLine("All PDFs successfully processed.");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("An error occurred: " + ex.Message);
    //        }

    //        Console.ReadLine();
    //    }

    //    private static async Task ProcessFileAsync(string filePath)
    //    {
    //        string htmlContent = File.ReadAllText(filePath);

    //        try
    //        {
    //            HtmlToPdf converter = new HtmlToPdf();
    //            //  ConfigureConverter(converter);converter.Options.DisplayHeader = true;

    //            converter.Options.DisplayHeader = true;
    //            converter.Options.DisplayFooter = true;
    //            PdfDocument doc = converter.ConvertHtmlString(htmlContent);
    //            AddHeaderFooter(doc);
    //            AddWatermarks(doc);

    //            string fileName = "Parallel_Output";
    //            string outputFilePath = $"D:\\Select.PDF_POC\\PDF\\{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

    //            await Task.Run(() => doc.Save(outputFilePath)); // Parallelized Save
    //            doc.Close();

    //            Console.WriteLine($"PDF successfully created: {outputFilePath}");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred while processing {filePath}: {ex.Message}");
    //        }
    //    }

    //    private static void AddHeaderFooter(PdfDocument doc)
    //    {
    //        PdfFont footerFont = doc.AddFont(PdfStandardFont.Helvetica);
    //        footerFont.Size = 7;

    //        // Footer template
    //        doc.Footer = doc.AddTemplate(doc.Pages[0].ClientRectangle.Width, 50);

    //        // Add footer text
    //        PdfTextElement footerText = new PdfTextElement(0, 20,
    //            "Document generated by Select.Pdf", footerFont)
    //        {
    //            ForeColor = Color.Gray,
    //            Width = doc.Pages[0].ClientRectangle.Width
    //        };
    //        doc.Footer.Add(footerText);

    //        // Add page numbers
    //        PdfTextElement pageNumbers = new PdfTextElement(0, 35,
    //            "Page {page_number} of {total_pages}", footerFont)
    //        {
    //            ForeColor = Color.Gray,
    //            Width = doc.Pages[0].ClientRectangle.Width
    //        };
    //        doc.Footer.Add(pageNumbers);

    //        // Add footer line
    //        PdfLineElement footerLine = new PdfLineElement(
    //            new PointF(0, 10), // Start point
    //            new PointF(doc.Pages[0].ClientRectangle.Width, 10) // End point
    //        )
    //        {
    //            BackColor = Color.Black
    //        };
    //        doc.Footer.Add(footerLine);
    //    }

    //    private static void AddWatermarks(PdfDocument doc)
    //    {
    //        PdfFont watermarkFont = doc.AddFont(PdfStandardFont.Helvetica);
    //        watermarkFont.Size = 50;

    //        PdfTextElement watermark = new PdfTextElement(0, 0, "CONFIDENTIAL", watermarkFont)
    //        {
    //            ForeColor = Color.FromArgb(50, Color.DarkKhaki), // Semi-transparent                
    //            Transparency = 50
    //        };

    //        foreach (PdfPage page in doc.Pages)
    //        {
    //            page.Add(watermark);
    //        }
    //    }

    //}
    ////-------------Parallel programming 

    ////Html-Select Pdf Code 
    //public class ProgramHtml
    //{
    //    static void Main(string[] args)
    //    {
    //        string localHtmlFilePath = @"C:\Users\home\source\repos\Select_Pdf_POC\Content\SampleHTML.html";
    //        if (!File.Exists(localHtmlFilePath))
    //        {
    //            Console.WriteLine("File not found: " + localHtmlFilePath);
    //            return;
    //        }

    //        string headerHtml = "<div style='text-align:center; font-size:12px;'>" +
    //               "<img src='C:\\Users\\home\\source\\repos\\Select_Pdf_POC\\Content\\images\\refinePics.jpg' style='width:150px;height:50px;' /><br />" +
    //               "<strong>CarlStahl Safety & Security Consultancy</strong><br />" +
    //               "P.O. Box : 26607, Dubai - U.A.E.<br />" +
    //               "Tel. : +971 4 333 3494 | +971 2 550 4443<br />" +
    //               "E-mail : Dubai@CarlStahl.ae | AbuDhabi@CarlStahl.ae<br />" +
    //               "Website : www.carlstahl.ae" +
    //               "</div>";

    //        string footerHtml = "<div style='text-align:center; font-size:10px; color:gray; margin-top: 20px;'>" +
    //                            "Document generated by Select.Pdf | Page {page_number} of {total_pages}" +
    //                            "</div>";

    //        string htmlContent = File.ReadAllText(localHtmlFilePath);

    //        string finalHtml = headerHtml + htmlContent + footerHtml;

    //        HtmlToPdf converter = new HtmlToPdf();

    //        PdfDocument doc = converter.ConvertHtmlString(finalHtml);

    //        // Create header and footer
    //        PdfHtmlSection headerSection = new PdfHtmlSection(headerHtml, string.Empty);
    //        headerSection.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
    //        PdfHtmlSection footerSection = new PdfHtmlSection(footerHtml, string.Empty);
    //        footerSection.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

    //        // Add header and footer to all pages
    //        converter.Header.Add(headerSection);
    //        converter.Footer.Add(footerSection);


    //        string dateTimeNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");

    //        // Construct the dynamic file path
    //        string outputFilePath = $"D:\\Select.PDF_POC\\PDF\\Output_#3_{dateTimeNow}.pdf";
    //        doc.Save(outputFilePath);

    //        // Close the document
    //        doc.Close();

    //        Console.WriteLine("PDF successfully created by #rd Prgm at: " + outputFilePath);
    //        Console.ReadLine();
    //    }
    //}
    ////Html-Select Pdf Code 

}







//                //auto ofit 
//converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
//converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;