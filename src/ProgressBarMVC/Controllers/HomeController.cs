using System;
using System.Web.Mvc;
using NServiceBus;
using ProgressBarMVC.Models;

public class HomeController : Controller
{
    /// <summary>
    /// Start a large task
    /// </summary>
    [HttpPost]
    public JsonResult StartBigStuff(int howMuchStuff)
    {
        var model = new StartBatchModel()
        {
            BatchId = Guid.NewGuid(),
            NumberOfThingsToDo = howMuchStuff
        };

        _bus.Send(new TriggerBigStuff()
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
    public JsonResult Status(string id)
    {
        var batchStatus = _statusStoreClient.GetBatchStatus(id);

        var completedCommandCount =
            batchStatus == null ? 0
                : batchStatus.ItemsCompleted.Count;

        var model = new BatchStatusModel()
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

    public HomeController(IBus bus, IStatusStoreClient statusStoreClient)
    {
        _bus = bus;
        _statusStoreClient = statusStoreClient;
    }

    private readonly IBus _bus;
    private readonly IStatusStoreClient _statusStoreClient;
}
