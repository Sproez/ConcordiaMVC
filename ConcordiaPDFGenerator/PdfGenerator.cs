using ConcordiaLib.Abstract;
using ConcordiaLib.Domain;
using HtmlTags;
using IronPdf;

namespace ConcordiaPDFGenerator;

public class PdfGenerator
{
    private readonly IDbMiddleware _dbMiddleware;
    private readonly string _completedListId;

    public PdfGenerator(IDbMiddleware db, string completedListId)
    {
        _dbMiddleware = db;
        _completedListId = completedListId;
    }

    public async Task Test()
    {
        var outerDiv = new HtmlTag("div");
        outerDiv.AppendText("Inizio lista");

        //Loop all scientists and add their report
        var scientists = await _dbMiddleware.GetAllPeople();
        foreach(Person p in scientists)
        {
            (int completed, int total) = await _dbMiddleware.GetScientistPerformanceReport(p.Id, _completedListId);

            var listElement = new HtmlTag("div");
            listElement.AppendText($"Scienziato {p.Name} ha completato {completed} assegnamenti su {total}");
            outerDiv.Append(listElement);
        }

        var closingDiv = new HtmlTag("div");
        closingDiv.AppendText("Fine lista");
        outerDiv.Append(closingDiv);

        HtmlToPdf HtmlToPdf = new IronPdf.HtmlToPdf();
        HtmlToPdf.RenderHtmlAsPdf(outerDiv.ToString()).SaveAs(@"Report.Pdf");
    }

}
