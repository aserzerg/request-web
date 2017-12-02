using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Highsoft.Web.Mvc.Charts;
using request_web.Models;
using request_web.WebService;
using Newtonsoft.Json;

namespace request_web.Controllers
{
    [Authorize]
    public class ServiceCompanyStatController : Controller
    {
        private const string FromDateSessionName = "stat_service_worker_fromDate";
        private const string ToDateSessionName = "stat_service_worker_toDate";
        [Authorize]
        public ActionResult GeneralInfo()
        {
        var model = new ServiceCompanyGeneralInfoModel();
            model.FromDate = DateTime.Now.AddDays(-30);
            model.ToDate = DateTime.Now;
            using (var requestService = new RequestWebServiceClient())
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                var workerId = currentUser.WorkerId;
                //workerId = 16;
                var statByUsers = requestService.GetWorkerStat(workerId, model.FromDate, model.ToDate);
                var usersPeriods = statByUsers.GroupBy(s => s.StatDate).ToList();
                var users = statByUsers.GroupBy(s => s.Name).ToList();
                var userChartSeries = new List<Series>();
                
                //todo удалить i
                var i = 1;

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

                    //userChartSeries.Add(new LineSeries{Name = $"Диспетчер {i++}", Data = series });
                    userChartSeries.Add(new LineSeries{Name = user.Key, Data = series });
                }
                model.RequestsByUsersXAxis = usersPeriods.Select(s=>s.Key.ToString("dd.MM.yyyy")).ToList();
                model.RequestsByUsersSeries = userChartSeries;
                /**/
                /*var statByWorkers = requestService.GetRequestByWorkersInfo();
                var workersPeriods = statByWorkers.GroupBy(s => s.StatDate).ToList();
                var workers = statByWorkers.GroupBy(s => s.Name).ToList();
                var workerChartSeries = new List<Series>();

                i = 1;

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

                    workerChartSeries.Add(new LineSeries { Name = $"Исполнитель {i++}", Data = series });
                    //workerChartSeries.Add(new LineSeries { Name = worker.Key, Data = series });
                }
                workerChartSeries.Add(new LineSeries
                {
                    Name = "Всего",
                    Data = statByWorkers.GroupBy(s => s.StatDate).Select(g => new LineSeriesData { Y = g.Sum(s => s.Count) }).ToList()
                });

                model.RequestsByWorkersXAxis = workersPeriods.Select(s => s.Key.ToString("dd.MM.yyyy")).ToList();
                model.RequestsByWorkersSeries = workerChartSeries;
                */

                return View(model);
            }
        }
    }
}