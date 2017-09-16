using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using request_web.Models;
using request_web.WebService;
using request_web.Dto;

namespace request_web.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private const string FromDateSessionName = "request_fromDate";
        private const string ToDateSessionName = "request_toDate";
        private const string WorkerSessionName = "request_worker";
        private const string StreetSessionName = "request_street";
        private const string StatusSessionName = "request_status";
        private const string HouseSessionName = "request_house";
        private const string AddressSessionName = "request_address";
        private const string ParServSessionName = "request_parent_service";
        private const string ServSessionName = "request_service";
        private const string FilteringSessionName = "request_filtering";
        public RequestsController()
        {
        }

        private WorkerShortDto[] GetWorkerList(RequestWebServiceClient requestService, int workerId)
        {
            var workers = new[] { new WorkerShortDto { Id = 0, Name = "Все исполнители" } };
            workers = workers.Concat(requestService.GetWorkers(workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" })).ToArray();
            return workers;
        }

        private StreetShortDto[] GetStreetList(RequestWebServiceClient requestService, int workerId)
        {
            var streets = new[] { new StreetShortDto { Id = 0, Name = "Все улицы" } };
            streets = streets.Concat(requestService.GetStreetListByWorker(workerId).OrderBy(s => s.Name).Select(s => new StreetShortDto { Id = s.Id, Name = $"{s.Name}" })).ToArray();
            return streets;
        }
        private StatusShortDto[] GetStatusList(RequestWebServiceClient requestService)
        {
            return requestService.GetWebStatuses().Select(s => new StatusShortDto { Id = s.Id, Name = s.Name, IsSelected = s.IsDefault }).ToArray();
        }
        private HouseShortDto[] GetHouseList(RequestWebServiceClient requestService, int streetId, int workerId)
        {

            var houses = requestService.GetHousesByStreetAndWorkerId(streetId, workerId).OrderBy(h => h.Name.PadLeft(6, '0'));
            return new[] { new HouseShortDto { Id = 0, Name = "Все дома" } }.Concat(houses.Select(h => new HouseShortDto { Id = h.Id, Name = h.Name })).ToArray();
        }

        private AddressShortDto[] GetAddressList(RequestWebServiceClient requestService, int houseId)
        {
            var addresses = requestService.GetFlatByHouseId(houseId);
            return new[] { new AddressShortDto { Id = 0, Name = "Все адреса" } }.Concat(addresses.Select(h => new AddressShortDto { Id = h.Id, Name = h.Name })).ToArray();
        }
        private ServiceShortDto[] GetServices(RequestWebServiceClient requestService, int? parentId)
        {
            var services = requestService.GetServices(parentId);
            return services.Select(s => new ServiceShortDto { Id = s.Id, Name = s.Name }).ToArray();
        }

        private RequestListModel GetViewModel(DateTime fromDate, DateTime toDate, int selectedWorkerId,
            int selectedStreetId, int selectedStatusId, int selectedHouseId, int selectedAddressId,
            int selectedParServId, int selectedServiceId, bool filterIsChecked)
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var workerId = currentUser.WorkerId;

            Session[FromDateSessionName] = fromDate;
            Session[ToDateSessionName] = toDate;
            using (var requestService = new RequestWebServiceClient())
            {

                #region Workers
                var workers = GetWorkerList(requestService, currentUser.WorkerId);
                var selected = workers.Where(w => w.Id == selectedWorkerId).FirstOrDefault();
                if (selected != null)
                {
                    selected.IsSelected = true;
                }
                #endregion
                #region Streets
                var streets = GetStreetList(requestService, currentUser.WorkerId);
                var selectedStreet = streets.Where(w => w.Id == selectedStreetId).FirstOrDefault();
                if (selectedStreet != null)
                {
                    selectedStreet.IsSelected = true;
                }
                #endregion
                #region Houses
                var houses = new[] { new HouseShortDto { Id = 0, Name = "Все дома" } };
                if (selectedStreetId > 0)
                {
                    houses = GetHouseList(requestService, selectedStreetId, currentUser.WorkerId);
                    var selectedHouse = houses.Where(h => h.Id == selectedHouseId).FirstOrDefault();
                    if (selectedHouse != null)
                    {
                        selectedHouse.IsSelected = true;
                    }
                }
                #endregion
                #region Addresses
                var addresses = new[] { new AddressShortDto { Id = 0, Name = "Все адреса" } };
                if (selectedHouseId > 0)
                {
                    addresses = GetAddressList(requestService, selectedHouseId);
                    var selectedAddress = addresses.Where(h => h.Id == selectedAddressId).FirstOrDefault();
                    if (selectedAddress != null)
                    {
                        selectedAddress.IsSelected = true;
                    }
                }

                #endregion
                #region Statuses
                var statuses = GetStatusList(requestService);
                var selectedStatus = statuses.Where(w => w.Id == selectedStatusId).FirstOrDefault();
                if (selectedStatus != null)
                {
                    selectedStatus.IsSelected = true;
                }
                else
                {
                    statuses.FirstOrDefault(w => w.Id == 2).IsSelected = true;
                }
                #endregion
                #region Parent Services
                var parentServices = new[] { new ServiceShortDto { Id = 0, Name = "Все услуги" } }.Concat(GetServices(requestService, null)).ToArray();
                var selectedParentServices = parentServices.Where(w => w.Id == selectedParServId).FirstOrDefault();
                if (selectedParentServices != null)
                {
                    selectedParentServices.IsSelected = true;
                }
                else
                {
                    parentServices.FirstOrDefault(w => w.Id == 0).IsSelected = true;
                }
                #endregion
                #region Services
                var services = new[] { new ServiceShortDto { Id = 0, Name = "Все причины" } };
                if (selectedParServId > 0)
                {
                    services = services.Concat(GetServices(requestService, selectedParServId)).ToArray();
                }
                if (selectedServiceId > 0)
                {
                    var selectedService = services.Where(s => s.Id == selectedServiceId).FirstOrDefault();
                    if (selectedService != null)
                    {
                        selectedService.IsSelected = true;
                    }
                }
                #endregion
                #region Requests
                RequestForListDto[] reguests = requestService.RequestList(workerId, fromDate, toDate.AddDays(1),
            selectedWorkerId > 0 ? selectedWorkerId : (int?)null,
            selectedStreetId > 0 ? selectedStreetId : (int?)null,
            selectedHouseId > 0 ? selectedHouseId : (int?)null,
            selectedAddressId > 0 ? selectedAddressId : (int?)null,
            selectedStatusId > 0 ? selectedStatusId : 2,
            selectedParServId > 0 ? selectedParServId : (int?)null,
            selectedServiceId > 0 ? selectedServiceId : (int?)null
            );
                #endregion
                return new RequestListModel
                {
                    Requests = reguests,
                    FromDate = fromDate,
                    ToDate = toDate,
                    Workers = workers,
                    Streets = streets,
                    Statuses = statuses,
                    Houses = houses,
                    AdditionFiltering = filterIsChecked,
                    ParentServices = parentServices,
                    Addresses = addresses,
                    Services = services
                };
            }
        }
        [Authorize]
        public ActionResult List()
        {
            var fromDate = (DateTime?)Session[FromDateSessionName] ?? DateTime.Now.Date.AddDays(-30);
            var toDate = (DateTime?)Session[ToDateSessionName] ?? DateTime.Now.Date;
            var selectedWorkerId  = (int?)Session[WorkerSessionName] ?? 0;
            var selectedStreetId = (int?)Session[StreetSessionName] ?? 0;
            var selectedStatusId = (int?)Session[StatusSessionName] ?? 0;
            var selectedHouseId = (int?)Session[HouseSessionName] ?? 0;
            var selectedAddressId = (int?)Session[AddressSessionName] ?? 0;
            var selectedParServId = (int?)Session[ParServSessionName] ?? 0;
            var selectedServiceId = (int?)Session[ServSessionName] ?? 0;
            var filterIsChecked = (bool?)Session[FilteringSessionName] ?? false;


            var viewModel = GetViewModel(fromDate, toDate, selectedWorkerId,selectedStreetId, selectedStatusId, selectedHouseId, selectedAddressId,
                        selectedParServId, selectedServiceId, filterIsChecked);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult List(RequestListModel model)
        {
            Int32.TryParse(Request.Params["SelectedWorker"], out int selectedWorkerId);
            Int32.TryParse(Request.Params["SelectedStreet"], out int selectedStreetId);
            Int32.TryParse(Request.Params["SelectedStatus"], out int selectedStatusId);
            Int32.TryParse(Request.Params["SelectedHouse"], out int selectedHouseId);
            Int32.TryParse(Request.Params["SelectedAddress"], out int selectedAddressId);
            Int32.TryParse(Request.Params["SelectedParServ"], out int selectedParServId);
            Int32.TryParse(Request.Params["SelectedService"], out int selectedServiceId);

            Session[FromDateSessionName] = model.FromDate;
            Session[ToDateSessionName] = model.ToDate;
            Session[WorkerSessionName] = selectedWorkerId;
            Session[StreetSessionName] = selectedStreetId;
            Session[StatusSessionName] = selectedStatusId;
            Session[HouseSessionName] = selectedHouseId;
            Session[AddressSessionName] = selectedAddressId;
            Session[FilteringSessionName] = model.AdditionFiltering;
            Session[ParServSessionName] = selectedParServId;
            Session[ServSessionName] = selectedServiceId;

            var viewModel = GetViewModel(model.FromDate, model.ToDate, selectedWorkerId, selectedStreetId, selectedStatusId, selectedHouseId, selectedAddressId,
                        selectedParServId, selectedServiceId, model.AdditionFiltering);
            return View(viewModel);
        }

        [Authorize]

        public ActionResult CreateNew()
        {
            return View();
        }
        [Authorize]
        public ActionResult Details(string requestParam)
        {
            var requestId = Request.Params["RequestId"];
            if (string.IsNullOrEmpty(requestId))
                requestId = requestParam;
            using (var requestService = new RequestWebServiceClient())
            {
                var request = requestService.GetRequestById(Convert.ToInt32(requestId));
                var worker = $"{request.Worker?.SurName} {request.Worker?.FirstName} {request.Worker?.PatrName}";
                var contactList = request.ContactPhones;

                var callList = requestService.GetWebCallsByRequestId(request.Id);

                return View(new RequestDetailModel()
                {
                    RequestId = request.Id,
                    Address = request.StreetName + " " + request.Building + (string.IsNullOrEmpty(request.Corpus) ? "" : "/" + request.Corpus) + ", " + request.Flat,
                    Worker = worker,
                    CreateTime = request.CreateTime,
                    Description = request.Description,
                    Creator = request.CreateUser.SurName + " " + request.CreateUser.FirstName + " " + request.CreateUser.PatrName,
                    State = request.Status,
                    WorkDate = request.ExecuteTime,
                    Contacts = contactList,
                    CallList = callList
                });
            }
        }
        [Authorize]
        public ActionResult ChangeStatus(string RequestStr)
        {
            var requestId = Request.Params["RequestId"];
            var t33 = HttpContext.Request.RequestType.ToUpper() == "POST";//"GET"
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var t = currentUser.UserId;
            ViewBag.Processed = true;
            StatusDto[] webStatuses;
            int currentStatus = 0;
            using (var requestService = new RequestWebServiceClient())
            {
                webStatuses = requestService.GetStatusesAllowedInWeb();
                var request = requestService.GetRequestById(Convert.ToInt32(requestId));
                currentStatus = request.StatusId;
            }

            var model = new ChangeStatusModel {RequestId = Convert.ToInt32(requestId),CurrentStatus = webStatuses.FirstOrDefault(s => s.Id == currentStatus)?.Id??0, StatusList = new SelectList(webStatuses, "Id", "Description") };

            if (HttpContext.Request.RequestType.ToUpper() == "POST")
                return View("Close");
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeStatus(ChangeStatusModel model, string action)
        {
            if (model.RequestId > 0)
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                using (var requestService = new RequestWebServiceClient())
                {
                    requestService.ChangeState(model.RequestId, model.CurrentStatus, currentUser.UserId);
                }
            }
            return Redirect(Url.Action("Details", "Requests")+$"?RequestId={model.RequestId}");
        }


        [Authorize]
        public ActionResult ChangeWorker()
        {
            var requestId = Request.Params["RequestId"];
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var t = currentUser.UserId;
            return View();
        }

        [Authorize]
        public ActionResult GetMediaByRequestId(int requestId)
        {
            var requestId2 = Request.Params["RequestId"];
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetMediaByRequestId(requestId);
                return File(t, "audio/wav");
            }
        }
        [HttpPost]
        public JsonResult GetWorkers()
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetWorkers(currentUser.WorkerId).Select(w => new { id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" });
                return Json(JsonConvert.SerializeObject(t));
            }
        }
        [HttpPost]
        public JsonResult GetHouses(string streetNum)
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            Int32.TryParse(streetNum,out int streetId);
            using (var requestService = new RequestWebServiceClient())
            {
                var houses = GetHouseList(requestService, streetId, currentUser.WorkerId);
                return Json(JsonConvert.SerializeObject(houses));
            }
        }
        [HttpPost]
        public JsonResult GetAddresses(string houseNum)
        {
            Int32.TryParse(houseNum, out int houseId);
            using (var requestService = new RequestWebServiceClient())
            {
                var addresses = GetAddressList(requestService, houseId);
                return Json(JsonConvert.SerializeObject(addresses));
            }
        }
        [HttpPost]
        public JsonResult GetServicesByParent(string parentServiceNum)
        {
            Int32.TryParse(parentServiceNum, out int parentServiceId);
            using (var requestService = new RequestWebServiceClient())
            {
                var services = new[] { new ServiceShortDto { Id = 0, Name = "Все причины" } }.Concat(GetServices(requestService, parentServiceId)).ToArray();
                return Json(JsonConvert.SerializeObject(services));
            }
        }

        [Authorize]
        public ActionResult PrintActs()
        {
            var fromDate = (DateTime?)Session[FromDateSessionName] ?? DateTime.Now.Date.AddDays(-30);
            var toDate = (DateTime?)Session[ToDateSessionName] ?? DateTime.Now.Date;
            var selectedWorkerId = (int?)Session[WorkerSessionName] ?? 0;
            var selectedStreetId = (int?)Session[StreetSessionName] ?? 0;
            var selectedStatusId = (int?)Session[StatusSessionName] ?? 0;
            var selectedHouseId = (int?)Session[HouseSessionName] ?? 0;
            var selectedAddressId = (int?)Session[AddressSessionName] ?? 0;
            var selectedParServId = (int?)Session[ParServSessionName] ?? 0;
            var selectedServiceId = (int?)Session[ServSessionName] ?? 0;
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var workerId = currentUser.WorkerId;

            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetRequestActs(workerId, fromDate, toDate.AddDays(1),
                    selectedWorkerId > 0 ? selectedWorkerId : (int?)null,
                    selectedStreetId > 0 ? selectedStreetId : (int?)null,
                    selectedHouseId > 0 ? selectedHouseId : (int?)null,
                    selectedAddressId > 0 ? selectedAddressId : (int?)null,
                    selectedStatusId > 0 ? selectedStatusId : 2,
                    selectedParServId > 0 ? selectedParServId : (int?)null,
                    selectedServiceId > 0 ? selectedServiceId : (int?)null);
                return File(t, "application/pdf");
            }
        }

    }
}