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

        var title = new HtmlTag("h1").AppendText("Performance Report");
        outerDiv.Append(title);

        //Add a list element for each scientist
        var list = new HtmlTag("ul");
        var reports = await _dbMiddleware.GetPerformanceReport();
        foreach (Report r in reports)
        {
            var listElement = new HtmlTag("li");
            listElement.AppendText($"{r.Name} ha completato {r.PercentCompleted} dei suoi assegnamenti");
            //Fancy bar
            var bar = new HtmlTag("progress").Value(r.completedTasks.ToString()).Attr("max", r.assignedTasks.ToString());
            listElement.Append(new HtmlTag("br")).Append(bar);

            list.Append(listElement);
        }
        outerDiv.Append(list);

        HtmlToPdf HtmlToPdf = new IronPdf.HtmlToPdf();
        HtmlToPdf.RenderHtmlAsPdf(outerDiv.ToString()).SaveAs(@"Report.Pdf");
    }

}
