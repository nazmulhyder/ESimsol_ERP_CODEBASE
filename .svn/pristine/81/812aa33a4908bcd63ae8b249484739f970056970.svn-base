// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfViewController.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net pkalkie@gmail.com
// </copyright>
// <summary>
//   Extends the controller with functionality for rendering PDF views
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ReportManagement
{
    using iTextSharp.text;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Extends the controller with functionality for rendering PDF views
    /// </summary>
    public class PdfViewController : Controller
    {
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        public PdfViewController()
        {
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model, Rectangle pageSize, float marginLeft, float marginRight, float marginTop, float marginBottom, bool isLandscape=false)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, pageSize, marginLeft, marginRight, marginTop, marginBottom,isLandscape);

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf");
        }
    }
}