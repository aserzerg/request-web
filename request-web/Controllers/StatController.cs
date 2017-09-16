using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Highsoft.Web.Mvc.Charts;
using request_web.Models;
using request_web.WebService;

namespace request_web.Controllers
{
    [Authorize]
    public class StatController : Controller
    {
        [Authorize]
        public ActionResult GeneralInfo()
        {
            var model = new GeneralInfoModel();

            using (var requestService = new RequestWebServiceClient())
            {
                var statByUsers = requestService.GetRequestByUsersInto();
                var usersPeriods = statByUsers.GroupBy(s => s.StatDate).ToList();
                var users = statByUsers.GroupBy(s => s.Name).ToList();
                var userChartSeries = new List<Series>();
                foreach (var user in users.Where(u=>!string.IsNullOrEmpty(u.Key)))
                {
                    List<LineSeriesData> series = new List<LineSeriesData>();
                    foreach (var date in usersPeriods)
                    {
                        var count =
                            statByUsers.Where(s => s.Name == user.Key && s.StatDate == date.Key)
                                .Select(s => s.Count).FirstOrDefault();
                        series.Add(new LineSeriesData {Y = count ?? 0});
                    }

                    userChartSeries.Add(new LineSeries{Name = user.Key, Data = series });
                }
                userChartSeries.Add(new LineSeries {Name = "Всего",
                    Data = statByUsers.GroupBy(s => s.StatDate).Select(g => new LineSeriesData {Y = g.Sum(s => s.Count)}).ToList()});

                model.RequestsByUsersXAxis = usersPeriods.Select(s=>s.Key.ToString("dd.MM.yyyy")).ToList();
                model.RequestsByUsersSeries = userChartSeries;
                /**/
                var statByWorkers = requestService.GetRequestByWorkersInto();
                var workersPeriods = statByWorkers.GroupBy(s => s.StatDate).ToList();
                var workers = statByWorkers.GroupBy(s => s.Name).ToList();
                var workerChartSeries = new List<Series>();
                foreach (var worker in workers.Where(w => !string.IsNullOrEmpty(w.Key)))
                {
                    List<LineSeriesData> series = new List<LineSeriesData>();
                    foreach (var date in workersPeriods)
                    {
                        var count =
                            statByWorkers.Where(s => s.Name == worker.Key && s.StatDate == date.Key)
                                .Select(s => s.Count).FirstOrDefault();
                        series.Add(new LineSeriesData { Y = count ?? 0 });
                    }

                    workerChartSeries.Add(new LineSeries { Name = worker.Key, Data = series });
                }
                workerChartSeries.Add(new LineSeries
                {
                    Name = "Всего",
                    Data = statByWorkers.GroupBy(s => s.StatDate).Select(g => new LineSeriesData { Y = g.Sum(s => s.Count) }).ToList()
                });

                model.RequestsByWorkersXAxis = workersPeriods.Select(s => s.Key.ToString("dd.MM.yyyy")).ToList();
                model.RequestsByWorkersSeries = workerChartSeries;


                return View(model);
            }
        }
    }
}