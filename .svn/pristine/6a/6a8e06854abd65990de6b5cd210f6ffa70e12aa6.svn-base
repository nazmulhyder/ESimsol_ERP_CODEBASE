// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPdfRenderer.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net
// </copyright>
// <summary>
//   This class is responsible for rendering a html text string to a PDF document
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ReportManagement
{
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;

    /// <summary>
    /// This class is responsible for rendering a html text string to a PDF document using the html renderer of iTextSharp.
    /// </summary>
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 40;
        private const int VerticalMargin = 40;

        public byte[] Render(string htmlText, string pageTitle, Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom, bool isLandscape=false)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var pdfDocument = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    if (isLandscape)
                    {
                        pdfDocument.SetPageSize(PageSize.A4.Rotate());
                    }
                    pdfDocument.Open();
                    using (var htmlViewReader = new StringReader(htmlText))
                    {
                        using (var htmlWorker = new HTMLWorker(pdfDocument))
                        {
                            htmlWorker.Parse(htmlViewReader);
                        }
                    }
                }

                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }
    }
}