using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font.Constants;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.IO.Image;
using iText.Kernel.Geom;
using HelpdeskViewModels;
namespace CasestudyWebsite.Reports
{
    public class CallReport
    {
        readonly PdfFont helvetica = PdfFontFactory.CreateFont(StandardFontFamilies.HELVETICA);
        public async Task GenerateReport(string rootpath)
        {
            PageSize pg = PageSize.A4.Rotate();
            Image img = new Image(ImageDataFactory.Create(rootpath + "/img/look-up.png"))
                .ScaleAbsolute(100, 100)
                .SetFixedPosition(((pg.GetWidth() - 100) / 2), 450);
            PdfWriter writer = new(rootpath + "/pdfs/calllist.pdf",
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdf = new(writer);
            Document document = new(pdf, pg); // PageSize(595, 842)
            document.Add(img);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Current Calls")
            .SetFont(helvetica)
            .SetFontSize(24)
            .SetBold()
            .SetTextAlignment(TextAlignment.CENTER));
            Table table = new(6);
            table
            .SetWidth(500) // roughly 50%
            .SetTextAlignment(TextAlignment.CENTER)
            .SetRelativePosition(0, 0, 0, 0)
            .SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.AddCell(AddCell("Opened", "h", 0));
            table.AddCell(AddCell("Lastname", "h", 0));
            table.AddCell(AddCell("Tech", "h", 0));
            table.AddCell(AddCell("Problem", "h", 0));
            table.AddCell(AddCell("Status", "h", 0));
            table.AddCell(AddCell("Closed", "h", 0));
            table.AddCell(AddCell(" ", "d"));
            table.AddCell(AddCell(" ", "d"));
            table.AddCell(AddCell(" ", "d"));
            table.AddCell(AddCell(" ", "d"));
            table.AddCell(AddCell(" ", "d"));
            table.AddCell(AddCell(" ", "d"));
            CallViewModel call = new();
            List<CallViewModel> calls = await call.GetAll();
            foreach (CallViewModel c in calls)
            {
                table.AddCell(AddCell(c.DateOpened.ToShortDateString(), "d"));
                table.AddCell(AddCell(c.EmployeeName!, "d"));
                table.AddCell(AddCell(c.TechName!, "d"));
                table.AddCell(AddCell(c.ProblemDescription!, "d"));
                table.AddCell(AddCell(c.OpenStatus ? "Open" : "Closed", "d"));
                table.AddCell(c.DateClosed.HasValue
                    ? AddCell(((DateTime)c.DateClosed).ToShortDateString(), "d")
                    : AddCell("-", "d"));
            }
            document.Add(table);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Call report written on - " + DateTime.Now)
            .SetFontSize(6)
            .SetTextAlignment(TextAlignment.CENTER));
            document.Close();
        }
        private static Cell AddCell(string data, string celltype, int padLeft = 16)
        {
            Cell cell;
            if (celltype == "h")
            {
                cell = new Cell().Add(
                new Paragraph(data)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetBold()
                )
                .SetBorder(Border.NO_BORDER);
            }
            else
            {
                cell = new Cell().Add(
                new Paragraph(data)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetPaddingLeft(padLeft)
                )
                .SetBorder(Border.NO_BORDER);
            }
            return cell;
        }

    }
}
