using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using NServiceBus;
using SqlMonitor.Core.Domain;
using SqlMonitor.DataServices;
using SqlMonitor.Messages.Commands;

namespace SqlMonitor.Web.Controllers
{
    public class QueryController : Controller
    {
        private readonly IBus bus;
        private readonly QueryContext queryContext;
        private readonly IEnumerable<string> databases;
        private const int QueryTimeoutMilliseconds = 120000;

        public QueryController(IBus bus, QueryContext queryContext)
        {
            this.bus = bus;
            this.queryContext = queryContext;
            databases = ConfigurationManager.AppSettings["DatabaseNames"].Split(',');
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult CronHelp()
        {
            return PartialView();
        }

        //
        // GET: /Query/

        public ActionResult Create()
        {
            ViewBag.Databases = databases;
            return View(new Query());
        }

        [HttpPost]
        public ActionResult Create(Query query)
        {
            if (ModelState.IsValid)
            {
                var createQuery = Mapper.Map<Query, CreateQuery>(query);
                bus.Send(createQuery);
                TempData["Message"] = "Query has been created";
                return RedirectToAction("List");
            }
            return View(query);
        }


        public ActionResult Edit(int id)
        {
            var query = queryContext.Queries.Find(id);
            ViewBag.Databases = databases;
            return PartialView(query);
        }

        [HttpPost]
        public ActionResult Edit(Query query, string editAction)
        {
            if (editAction == "Cancel")
            {
                query = queryContext.Queries.Find(query.QueryId);
                return PartialView("QueryDetails", query);
            }

            if( ModelState.IsValid)
            {
                var updateQuery = Mapper.Map<Query, UpdateQuery>(query);
                bus.Send(updateQuery);
                return PartialView("QueryDetails", query);
            }
            ViewBag.Databases = databases;
            return PartialView(query);
        }

        public ActionResult Details(int id)
        {
            AssignDurationsToViewBag();
            var query = queryContext.Queries.Find(id);
            return View(query);
        }

        private void AssignDurationsToViewBag()
        {
            ViewBag.Durations = new[]
                                {
                                    new SelectListItem {Text = "Last 6 Hours", Value = new TimeSpan(6, 0, 0).TotalSeconds.ToString()},
                                    new SelectListItem {Text = "Last 12 Hours", Value = new TimeSpan(12, 0, 0).TotalSeconds.ToString()},
                                    new SelectListItem {Text = "Last 24 Hours", Value = new TimeSpan(1, 0, 0, 0).TotalSeconds.ToString()},
                                    new SelectListItem {Text = "Last 7 Days", Value = new TimeSpan(7, 0, 0, 0).TotalSeconds.ToString()},
                                    new SelectListItem {Text = "Last 30 Days", Value = new TimeSpan(30, 0, 0, 0).TotalSeconds.ToString()},
                                    new SelectListItem {Text = "Custom Date Range", Value = "0"}
                                };
            ViewBag.SelectedDuration = new TimeSpan(1, 0, 0, 0).TotalSeconds.ToString();
        }

        public ActionResult Graph(int id, DateTime? fromDate, DateTime? toDate, int duration)
        {
            var query = queryContext.Queries.Find(id);
            IEnumerable<QueryResult> results = GetResults(query, duration, fromDate, toDate);

            ViewBag.MinDate = results.Min(x => x.StartDate);
            ViewBag.MaxDate = results.Max(x => x.StartDate);
            ViewBag.Threshold = query.ThresholdMilliseconds;
            ViewBag.TimeOutDisplayValue = QueryTimeoutMilliseconds;

            return PartialView(results);
        }

        public ActionResult GraphBuilder()
        {
            AssignDurationsToViewBag();
            var queries = queryContext.Queries.ToList();
            return View(queries);
        }

        public ActionResult GraphBuilderResult(int[] id, DateTime? fromDate, DateTime? toDate, int duration)
        {
            List<Query> resultList = new List<Query>();
            foreach(var queryId in id)
            {
                var query = queryContext.Queries.Find(queryId);       
                IEnumerable<QueryResult> results = GetResults(query, duration, fromDate, toDate);
                var queryCopy = new Query {Name = query.Name, Results = results.ToList() };
                resultList.Add(queryCopy);
            }
            ViewBag.TimeOutDisplayValue = QueryTimeoutMilliseconds;
            return PartialView("MultiQueryGraph", resultList);
        }

        public ActionResult ResultsCombined(int id, DateTime? fromDate, DateTime? toDate, int duration)
        {
            var query = queryContext.Queries.Find(id);
            IEnumerable<QueryResult> results = GetResults(query, duration, fromDate, toDate);

            if (results.Any())
            {
                ViewBag.MinDate = results.Min(x => x.StartDate);
                ViewBag.MaxDate = results.Max(x => x.StartDate);
                ViewBag.Threshold = query.ThresholdMilliseconds;
                ViewBag.TimeOutDisplayValue = QueryTimeoutMilliseconds;
                return PartialView(results);
            }
            return PartialView("NoResults");
        }

        public ActionResult Results(int id, DateTime? fromDate, DateTime? toDate, int duration)
        {
            var query = queryContext.Queries.Find(id);
            var results = GetResults(query, duration, fromDate, toDate);
            return PartialView(results);
        }

        private IEnumerable<QueryResult> GetResults(Query query, int duration, DateTime? fromDate, DateTime? toDate)
        {
            if( duration > 0 )
            {
                toDate = DateTime.Now;
                fromDate = toDate - new TimeSpan(0, 0, duration);
            }
            else if (toDate.HasValue)
            {
                // so that today is included
                toDate = toDate + new TimeSpan(1, 0, 0, 0);
            }
            
            ViewBag.GraphTitle = query.Name;
            return from r in query.Results
                   where fromDate == null || r.StartDate > fromDate
                   where toDate == null || r.StartDate < toDate
                   select r;
        }

        public ActionResult List()
        {
            AssignDurationsToViewBag();
            var queries = queryContext.Queries.ToList();
            return View("List", queries);
        }
    }
}
