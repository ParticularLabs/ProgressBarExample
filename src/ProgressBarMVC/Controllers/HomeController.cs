using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NServiceBus;
using ProgressBarMVC.Models;

public class HomeController : Controller
{
    /// <summary>
    /// Start a large task
    /// </summary>
    [HttpPost]
    public async Task<JsonResult> StartBigStuff(int howMuchStuff)
    {
        var model = new StartBatchModel()
        {
            BatchId = Guid.NewGuid(),
            NumberOfThingsToDo = howMuchStuff
        };

        await _session.Send(new TriggerBigStuff()
        {
            Id = model.BatchId,
            HowMuchStuff = model.NumberOfThingsToDo,
        });

        // Return the Id to the client to query for status
        return Json(model, JsonRequestBehavior.AllowGet);
    }

    /// <summary>
    /// Query for the status of a task
    /// </summary>
    [HttpGet]
    public async Task<JsonResult> Status(string id)
    {
        var batchStatus = await _statusStoreClient.GetBatchStatus(id);

        var completedCommandCount =
            batchStatus?.ItemsCompleted.Count ?? 0;

        var model = new BatchStatusModel
        {
            BatchId = id,
            ItemsCompletedCount = completedCommandCount
        };

        return Json(model, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }

    public HomeController(IMessageSession session, IStatusStoreClient statusStoreClient)
    {
        _session = session;
        _statusStoreClient = statusStoreClient;
    }

    private readonly IMessageSession _session;
    private readonly IStatusStoreClient _statusStoreClient;
}
