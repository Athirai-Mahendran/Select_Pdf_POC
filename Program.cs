using SelectPdf;
using System;

namespace Select_Pdf_POC
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Define the HTML content with inline styles
            string htmlContent = @"
                <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                color: #333;
                            }
                            h1 {
                                color: #0056b3;
                            }
                            p {
                                font-size: 14px;
                            }
                        </style>
                    </head>
                    <body>
                        <h1>Welcome to SelectPDF!</h1>
                        <p>This PDF is generated from HTML content with inline CSS styles.</p>
                    </body>
                </html>";

            try
            {
                // Create an instance of the HTML to PDF converter
                HtmlToPdf converter = new HtmlToPdf();

                // Set converter options (e.g., margins, page size, etc.)
                converter.Options.PdfPageSize = PdfPageSize.A4; // Set page size to A4
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait; // Set orientation
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;
                converter.Options.MarginLeft = 20;
                converter.Options.MarginRight = 20;

                // Convert HTML string to PDF
                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // Save the PDF document to a file
                string outputFilePath = "D:\\Select.PDF_POC\\PDF\\output.pdf";
                doc.Save(outputFilePath);

                // Close the document
                doc.Close();

                Console.WriteLine("PDF successfully created at: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.ReadLine();
        }

        //HtmlToPdf converter = new HtmlToPdf();
        //PdfDocument doc = converter.ConvertHtmlString("<h1>Hello PDF!</h1>");
        //doc.Save("output.pdf");
        //doc.Close();
        //Console.WriteLine("PDF created.");
    }
}

