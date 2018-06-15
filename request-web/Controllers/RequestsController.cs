using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
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
        private const string ExecuterSessionName = "request_executer";
        private const string StreetSessionName = "request_street";
        private const string FilterSessionName = "request_filter";
        private const string StatusSessionName = "request_status";
        private const string HouseSessionName = "request_house";
        private const string AddressSessionName = "request_address";
        private const string ParServSessionName = "request_parent_service";
        private const string ServSessionName = "request_service";
        private const string RatingSessionName = "request_rating";
        private const string FilteringSessionName = "request_filtering";
        private const string PhoneSessionName = "request_phoneNumber";
        private const string IdSessionName = "request_Id";
        private const string IsBadWorkSessionName = "request_is_bad_work";
        private const string GarantySessionName = "request_garanty";
        public RequestsController()
        {
        }

        private WorkerShortDto[] GetWorkerList(RequestWebServiceClient requestService,DateTime fromDate,DateTime toDate, int workerId, bool withAll = true)
        {
            if (withAll)
            {
                return requestService.GetWorkersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" }).ToArray();
                return new[] { new WorkerShortDto { Id = 0, Name = "Все исполнители" } }.Concat(requestService.GetWorkersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" })).ToArray();
            }
            return new[] { new WorkerShortDto { Id = 0, Name = "Нет" } }.Concat(requestService.GetWorkersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" })).ToArray();

        }

        private WorkerShortDto[] GetExecutersList(RequestWebServiceClient requestService,DateTime fromDate,DateTime toDate, int workerId, bool withAll = true)
        {
            if (withAll)
            {
                return requestService.GetExecutersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" }).ToArray();
                return new[] { new WorkerShortDto { Id = 0, Name = "Все исполнители" } }.Concat(requestService.GetWorkersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" })).ToArray();
            }
            return new[] { new WorkerShortDto { Id = 0, Name = "Нет" } }.Concat(requestService.GetWorkersByPeriod(true, fromDate, toDate, fromDate, toDate, workerId).OrderBy(w => $"{w.SurName} {w.FirstName} {w.PatrName}").Select(w => new WorkerShortDto { Id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" })).ToArray();

        }

        private StreetShortDto[] GetStreetList(RequestWebServiceClient requestService, int workerId, bool withAll = true)
        {

            var streets = requestService.GetStreetListByWorker(workerId).OrderBy(s => s.Name).Select(s => new StreetShortDto { Id = s.Id, Name = $"{s.Name}" });
            if (withAll)
            {
                return streets.ToArray();
                return new[] {new StreetShortDto {Id = 0, Name = "Все улицы"}}.Concat(streets).ToArray();
            }
            return new[] { new StreetShortDto { Id = 0, Name = "Выберите улицу" } }.Concat(streets).ToArray();
        }
        private StatusShortDto[] GetStatusList(RequestWebServiceClient requestService)
        {
            return requestService.GetWebStatuses().Select(s => new StatusShortDto { Id = s.Id, Name = s.Name, IsSelected = s.IsDefault }).ToArray();
        }
        private HouseShortDto[] GetHouseList(RequestWebServiceClient requestService, int streetId, int workerId, bool withAll = true)
        {
            if(streetId==0)
                return new HouseShortDto[0];

            var houses = requestService.GetHousesByStreetAndWorkerId(streetId, workerId).OrderBy(h => h.Name.PadLeft(6, '0'));
            if (withAll)
            {
                return houses.Select(h => new HouseShortDto { Id = h.Id, Name = h.Name }).ToArray();
                return new[] {new HouseShortDto {Id = 0, Name = "Все дома"}}.Concat(houses.Select(h => new HouseShortDto {Id = h.Id, Name = h.Name})).ToArray();
            }
            return new[] { new HouseShortDto { Id = 0, Name = "Выберите дом" } }.Concat(houses.Select(h => new HouseShortDto { Id = h.Id, Name = h.Name })).ToArray();

        }

        private AddressShortDto[] GetAddressList(RequestWebServiceClient requestService, int houseId, bool withAll = true)
        {
            if(houseId==0)
                return new AddressShortDto[0];
            var addresses = requestService.GetFlatByHouseId(houseId);
            if (withAll)
            {
                return addresses.Select(h => new AddressShortDto { Id = h.Id, Name = h.Name }).ToArray();
                return new[] {new AddressShortDto {Id = 0, Name = "Все адреса"}}.Concat(addresses.Select(h => new AddressShortDto {Id = h.Id, Name = h.Name})).ToArray();
            }
            return new[] { new AddressShortDto { Id = 0, Name = "Выберите адрес" } }.Concat(addresses.Select(h => new AddressShortDto { Id = h.Id, Name = h.Name })).ToArray();
        }
        private ServiceShortDto[] GetServices(RequestWebServiceClient requestService, int? parentId, bool withAll = true)
        {
            if (withAll)
            {
                return requestService.GetServices(parentId).Select(s => new ServiceShortDto { Id = s.Id, Name = s.Name }).ToArray();
                return requestService.GetServices(parentId).Select(s => new ServiceShortDto {Id = s.Id, Name = s.Name}).ToArray();
            }
            return new[] { new ServiceShortDto { Id = 0, Name = "Выберите услугу" } }.Concat(requestService.GetServices(parentId).Select(s => new ServiceShortDto { Id = s.Id, Name = s.Name })).ToArray();
        }

        private RequestListModel GetViewModel(DateTime fromDate, DateTime toDate, int[] selectedWorkerIds, int[] selectedExecutersIds,
            int[] selectedStreetIds, int[] selectedStatusIds, int[] selectedHouseIds, int[] selectedAddressIds,
            int[] selectedParServIds, int[] selectedServiceIds, bool filterIsChecked, string phoneNumber, bool isBadWork, bool garanty, int[] ratingIds, int filterByExec,int? requestId)
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var workerId = currentUser.WorkerId;

            Session[FromDateSessionName] = fromDate;
            Session[ToDateSessionName] = toDate;
            using (var requestService = new RequestWebServiceClient())
            {

                #region Masters
                var masters = GetWorkerList(requestService,fromDate,toDate, currentUser.WorkerId);
                foreach (var master in masters.Where(w => selectedWorkerIds.Contains(w.Id)))
                {
                    master.IsSelected = true;
                }
                var executers = GetExecutersList(requestService, fromDate, toDate, currentUser.WorkerId);
                foreach (var executer in executers.Where(w => selectedExecutersIds.Contains(w.Id)))
                {
                    executer.IsSelected = true;
                }
                #endregion
                #region Streets
                var streets = GetStreetList(requestService, currentUser.WorkerId);
                foreach (var street in streets.Where(w => selectedStreetIds.Contains(w.Id)))
                {
                    street.IsSelected = true;
                }
                #endregion
                #region Houses
                var houses = new HouseShortDto[0];
                if (selectedStreetIds.Length == 1)
                {
                    houses = GetHouseList(requestService, selectedStreetIds[0], currentUser.WorkerId);
                    foreach (var house in houses.Where(w => selectedHouseIds.Contains(w.Id)))
                    {
                        house.IsSelected = true;
                    }
                }
                #endregion
                #region Addresses

                var addresses = new AddressShortDto[0];
                if (selectedHouseIds.Length == 1)
                {
                    addresses = GetAddressList(requestService, selectedHouseIds[0]);
                    foreach (var address in addresses.Where(w => selectedAddressIds.Contains(w.Id)))
                    {
                        address.IsSelected = true;
                    }
                }

                var ratings = (new[] {1, 2, 3, 4, 5}).Select(a => new RatingShortDto {Id = a, Name = a.ToString()}).ToArray();
                foreach (var rating in ratings.Where(w => ratingIds.Contains(w.Id)))
                {
                    rating.IsSelected = true;
                }
                var filters = new[]
                {
                    new FilterShortDto {Id = 1, Name = "По дате выполнения"},
                    new FilterShortDto {Id = 2, Name = "По дате создания"}
                };
                var selectedFilter = filters.Where(h => h.Id == filterByExec).FirstOrDefault();
                if (selectedFilter != null)
                {
                    selectedFilter.IsSelected = true;
                }

                #endregion
                #region Statuses
                var statuses = GetStatusList(requestService);
                if (!statuses.Any(w => selectedStatusIds.Contains(w.Id)))
                {
                    selectedStatusIds = new[] {3};
                }
                foreach (var status in statuses.Where(w => selectedStatusIds.Contains(w.Id)))
                {
                    status.IsSelected = true;
                }
                #endregion
                #region Parent Services
                var parentServices = GetServices(requestService, null).ToArray();
                foreach (var parentService in parentServices.Where(w => selectedParServIds.Contains(w.Id)))
                {
                    parentService.IsSelected = true;
                }
                #endregion
                #region Services
                var services = new List<ServiceWithParrentShortDto>();
                foreach (var parrent in parentServices.Where(w => selectedParServIds.Contains(w.Id)))
                {
                    var children = GetServices(requestService, parrent.Id).ToArray();
                    foreach (var service in children.Where(w => selectedServiceIds.Contains(w.Id)))
                    {
                        service.IsSelected = true;
                    }
                    services.Add(new ServiceWithParrentShortDto{Id=parrent.Id,Name = parrent.Name,Children = children });
                }

                #endregion
                #region Requests
                RequestForListDto[] requests = requestService.RequestListArrayParams(workerId, requestId, fromDate, toDate,
            selectedWorkerIds, selectedExecutersIds,selectedStreetIds, selectedHouseIds,selectedAddressIds,selectedStatusIds,selectedParServIds,selectedServiceIds,
            isBadWork,garanty,phoneNumber,ratingIds,filterByExec == 2).OrderByDescending(r=>r.CreateTime).ToArray();
                var currentDate = requestService.GetCurrentDate();


                #endregion
                return new RequestListModel
                {
                    UserInfo = currentUser,
                    Requests = requests,
                    FromDate = fromDate,
                    ToDate = toDate,
                    PhoneNumber = phoneNumber,
                    Masters = masters,
                    Executers = executers,
                    Streets = streets,
                    Statuses = statuses,
                    Houses = houses,
                    AdditionFiltering = filterIsChecked,
                    IsBadWork = isBadWork,
                    Garanty = garanty,
                    ParentServices = parentServices,
                    Addresses = addresses,
                    Services = services.ToArray(),
                    CurrentDate = currentDate,
                    Ratings = ratings,
                    FilterByDate = filters
                };
            }
        }
        [Authorize]
        public ActionResult List()
        {
            var fromDate = (DateTime?)Session[FromDateSessionName] ?? DateTime.Now.Date.AddDays(-30);
            var toDate = (DateTime?)Session[ToDateSessionName] ?? DateTime.Now.Date;
            var selectedWorkerId  = (int[])Session[WorkerSessionName] ?? new int[0];
            var selectedExecuterId = (int[])Session[ExecuterSessionName] ?? new int[0];
            var selectedStreetId = (int[])Session[StreetSessionName] ?? new int[0];
            var selectedStatusId = (int[])Session[StatusSessionName] ?? new int[0];
            var selectedFilterId = (int?)Session[FilterSessionName] ?? 0;
            var selectedHouseId = (int[])Session[HouseSessionName] ?? new int[0];
            var selectedAddressId = (int[])Session[AddressSessionName] ?? new int[0];
            var selectedParServId = (int[])Session[ParServSessionName] ?? new int[0];
            var selectedServiceId = (int[])Session[ServSessionName] ?? new int[0];
            var selectedRatingId = (int[]) Session[RatingSessionName] ?? new int[0];
            var filterIsChecked = (bool?)Session[FilteringSessionName] ?? false;
            var phoneNumber = (string)Session[PhoneSessionName];
            var requestId = (int?)Session[IdSessionName];
            var isBadWork = (bool?)Session[IsBadWorkSessionName] ?? false;
            var garanty = (bool?)Session[GarantySessionName] ?? false;


            var viewModel = GetViewModel(fromDate, toDate, selectedWorkerId, selectedExecuterId,selectedStreetId, selectedStatusId, selectedHouseId, selectedAddressId,
                        selectedParServId, selectedServiceId, filterIsChecked, phoneNumber, isBadWork, garanty, selectedRatingId, selectedFilterId, requestId);
            return View(viewModel);
        }

        private void TryParseParam(string param, out int[] result)
        {
            var items = param.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            var resultList = new List<int>();
            foreach (var item in items)
            {
                if (Int32.TryParse(item, out int parsed))
                {
                    resultList.Add(parsed);
                }
            }
            result = resultList.ToArray();
        }
        [HttpPost]
        public ActionResult List(RequestListModel model)
        {
            TryParseParam(Request.Params["SelectedWorker"], out int[] selectedWorkerId);
            TryParseParam(Request.Params["SelectedExecuter"], out int[] selectedExecuterId);
            TryParseParam(Request.Params["SelectedStreet"], out int[] selectedStreetId);
            TryParseParam(Request.Params["SelectedStatus"], out int[] selectedStatusId);
            TryParseParam(Request.Params["SelectedHouse"], out int[] selectedHouseId);
            TryParseParam(Request.Params["SelectedAddress"], out int[] selectedAddressId);
            TryParseParam(Request.Params["SelectedParServ"], out int[] selectedParServId);
            TryParseParam(Request.Params["SelectedService"], out int[] selectedServiceId);
            TryParseParam(Request.Params["SelectedRating"], out int[] selectedRatingId);
            Int32.TryParse(Request.Params["SelectedFilter"], out int selectedFilterId);

            Session[FromDateSessionName] = model.FromDate;
            Session[ToDateSessionName] = model.ToDate;
            Session[WorkerSessionName] = selectedWorkerId;
            Session[ExecuterSessionName] = selectedExecuterId;
            Session[StreetSessionName] = selectedStreetId;
            Session[FilterSessionName] = selectedFilterId;
            Session[StatusSessionName] = selectedStatusId;
            Session[HouseSessionName] = selectedHouseId;
            Session[AddressSessionName] = selectedAddressId;
            Session[FilteringSessionName] = model.AdditionFiltering;
            Session[ParServSessionName] = selectedParServId;
            Session[ServSessionName] = selectedServiceId;
            Session[RatingSessionName] = selectedRatingId;
            Session[PhoneSessionName] = model.PhoneNumber;
            Session[IdSessionName] = model.RequestId;
            Session[IsBadWorkSessionName] = model.IsBadWork;
            Session[GarantySessionName] = model.Garanty;


           var viewModel = GetViewModel(model.FromDate, model.ToDate, selectedWorkerId, selectedExecuterId, selectedStreetId, selectedStatusId, selectedHouseId, selectedAddressId,
                       selectedParServId, selectedServiceId, model.AdditionFiltering, model.PhoneNumber, model.IsBadWork, model.Garanty, selectedRatingId, selectedFilterId, model.RequestId);
            return View(viewModel);
        }

        [Authorize]

        public ActionResult CreateNew()
        {
            using (var requestService = new RequestWebServiceClient())
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                var workerId = currentUser.WorkerId;
                var workers = GetWorkerList(requestService,DateTime.MinValue, DateTime.MaxValue,  currentUser.WorkerId, false);
                var streets = GetStreetList(requestService, currentUser.WorkerId, false);
                var parentServices = GetServices(requestService, null, false).ToArray();
                var model = new RequestCreateNewModel();
                model.Workers = new WorkerShortDto[0];
                model.Executers = workers;
                model.Streets = streets;
                model.ParentServices = parentServices;
                model.Services = new ServiceShortDto[0];
                model.Addresses = new[] { new AddressShortDto { Id = 0, Name = "Выберите адрес" } };
                model.Houses = new[] { new HouseShortDto { Id = 0, Name = "Выберите дом" } };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateNew(RequestCreateNewModel model, string action)
        {
            //Int32.TryParse(Request.Params["SelectedWorker"], out int selectedWorkerId);
            //Int32.TryParse(Request.Params["SelectedStreet"], out int selectedStreetId);
            //Int32.TryParse(Request.Params["SelectedStatus"], out int selectedStatusId);
            //Int32.TryParse(Request.Params["SelectedHouse"], out int selectedHouseId);
            //Int32.TryParse(Request.Params["SelectedParServ"], out int selectedParServId);
            Int32.TryParse(Request.Params["SelectedAddress"], out int addressId);
            Int32.TryParse(Request.Params["SelectedExecuter"], out int executerId);
            Int32.TryParse(Request.Params["SelectedMaster"], out int masterId);
            Int32.TryParse(Request.Params["SelectedService"], out int serviceId);
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            using (var requestService = new RequestWebServiceClient())
            {
                var result = requestService.CreateRequest(currentUser.WorkerId, model.PhoneNumber, model.Fio, addressId,
                    serviceId, masterId==0 ? (int?)null : masterId, executerId == 0 ? (int?)null : executerId, model.Description);
                return Redirect(Url.Action("SaveDone", "Requests") + $"?RequestId={result}");
            }
        }

        [Authorize]
        public ActionResult SaveDone(string requestParam)
        {
            var requestId = Request.Params["RequestId"];
            if (string.IsNullOrEmpty(requestId))
                requestId = requestParam;
            return View(new RequestSaveModel()
            {
                RequestId = requestId
            });
        }

        [Authorize]
        public ActionResult Details(string requestParam)
        {
            var requestId = Request.Params["RequestId"];
            if (string.IsNullOrEmpty(requestId))
                requestId = requestParam;
            using (var requestService = new RequestWebServiceClient())
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                var workerId = currentUser.WorkerId;
                var request = requestService.GetRequestByWorkerAndId(workerId,Convert.ToInt32(requestId));
                var worker = $"{request.Master?.SurName} {request.Master?.FirstName} {request.Master?.PatrName}";
                var executer = $"{request.Executer?.SurName} {request.Executer?.FirstName} {request.Executer?.PatrName}";
                var contactList = request.ContactPhones;
                var attachments = requestService.GetAttachmentList(request.Id);
                var notes = requestService.GetNotes(request.Id);
                var callList = requestService.GetWebCallsByRequestId(request.Id);

                return View(new RequestDetailModel()
                {
                    RequestId = request.Id,
                    Address = request.StreetName + " " + request.Building + (string.IsNullOrEmpty(request.Corpus) ? "" : "/" + request.Corpus) + ", " + request.Flat +
                     (string.IsNullOrEmpty(request.Entrance) ? "":$", подъезд {request.Entrance}") + (string.IsNullOrEmpty(request.Floor) ? "": $", этаж {request.Floor}"),
                    Worker = worker,
                    Executer = executer,
                    CreateTime = request.CreateTime,
                    ParentService = request.ParentService,
                    Service = request.Service,
                    Description = request.Description,
                    Creator = request.CreateUser.SurName + " " + request.CreateUser.FirstName + " " + request.CreateUser.PatrName,
                    State = request.Status,
                    WorkDate = request.ExecuteTime,
                    Contacts = contactList,
                    CallList = callList,
                    Garanty = request.Garanty,
                    Attachments = attachments,
                    Notes = notes,
                    Rating = request.Rating,
                    RatingDescription = request.RatingDescription
                });
            }
        }

        [Authorize]
        public ActionResult UploadFile()
        {
            var requestStr = Request.Params["RequestId"];
            HttpPostedFileBase file = Request.Files[0];
            if (file == null)
                return null;
            using (var requestService = new RequestWebServiceClient())
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                var workerId = currentUser.WorkerId;
                if (int.TryParse(requestStr, out int requestId))
                {
                    requestService.UploadFile(UserId: currentUser.UserId,FileName: file.FileName, RequestId: requestId, FileStream: file.InputStream);
                }
            }
            return Redirect(Url.Action("Details", "Requests") + $"?RequestId={requestStr}");
        }
        [Authorize]
        public ActionResult AddNote(string RequestStr)
        {
            var requestId = Request.Params["RequestId"];
            //ViewBag.Processed = true;
            var model = new AddNoteModel(){RequestId = Convert.ToInt32(requestId) };
            if (HttpContext.Request.RequestType.ToUpper() == "POST")
                return View("Close");
           return View(model);
        }

        [HttpPost]
        public ActionResult AddNote(AddNoteModel model, string action)
        {
            if (model.RequestId > 0)
            {
                var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
                using (var requestService = new RequestWebServiceClient())
                {
                    requestService.AddNote(model.RequestId, model.Note, currentUser.UserId);
                }
            }
            return Redirect(Url.Action("Details", "Requests") + $"?RequestId={model.RequestId}");
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
                var request = requestService.GetRequestByWorkerAndId(currentUser.WorkerId,Convert.ToInt32(requestId));
                currentStatus = request.StatusId;
            }

            var model = new ChangeStatusModel { RequestId = Convert.ToInt32(requestId), CurrentStatus = webStatuses.FirstOrDefault(s => s.Id == currentStatus)?.Id ?? 0, StatusList = new SelectList(webStatuses, "Id", "Description") };

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
        public ActionResult GetRecordById(int Id)
        {
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetRecordById(Id);
                Response.Headers["accept-ranges"] = "bytes";
                Response.Headers["Content-Length"] = t.Length.ToString();

                return File(t, "audio/wav");
            }
        }
        [Authorize]
        public ActionResult DownloadDocument(int requestId,string fileName,string defaultName)
        {
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.DownloadFile(requestId, fileName);
                return File(t, "application/octet-stream", defaultName);
            }
        }
        [HttpPost]
        public JsonResult GetMasters()
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetWorkers(currentUser.WorkerId).Select(w => new { id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" });
                return Json(JsonConvert.SerializeObject(t));
            }
        }
        [HttpPost]
        public JsonResult GetMastersByHouse(string houseNum, string serviceNum)
        {
            Int32.TryParse(houseNum, out int houseId);
            Int32.TryParse(serviceNum, out int serviceId);
            using (var requestService = new RequestWebServiceClient())
            {
                var t = requestService.GetMastersByHouseAndService(houseId, serviceId).Select(w => new { id = w.Id, Name = $"{w.SurName} {w.FirstName} {w.PatrName}" });
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
        public JsonResult GetHousesForCreateNew(string streetNum)
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            Int32.TryParse(streetNum,out int streetId);
            using (var requestService = new RequestWebServiceClient())
            {
                var houses = GetHouseList(requestService, streetId, currentUser.WorkerId,false);
                return Json(JsonConvert.SerializeObject(houses));
            }
        }
        [HttpPost]
        public JsonResult GetAddressesForCreateNew(string houseNum)
        {
            Int32.TryParse(houseNum, out int houseId);
            using (var requestService = new RequestWebServiceClient())
            {
                var addresses = GetAddressList(requestService, houseId,false);
                return Json(JsonConvert.SerializeObject(addresses));
            }
        }
        [HttpPost]
        public JsonResult GetServicesByParent(string parentServiceNum, bool withAll = true)
        {
            var services = new List<ServiceWithParrentShortDto>();
            var selectedParrentServicesId = new int[0];
            TryParseParam(parentServiceNum, out selectedParrentServicesId);
            if(selectedParrentServicesId.Length==0)
                return Json(JsonConvert.SerializeObject(services));

            using (var requestService = new RequestWebServiceClient())
            {
                var parentServices = GetServices(requestService, null).Where(t=> selectedParrentServicesId.Contains(t.Id)).ToArray();
                foreach (var parrent in parentServices)
                {
                    var children = GetServices(requestService, parrent.Id).ToArray();
                    services.Add(new ServiceWithParrentShortDto { Id = parrent.Id, Name = parrent.Name, Children = children });
                }
            }
            return Json(JsonConvert.SerializeObject(services));
        }

        [HttpPost]
        public JsonResult GetServicesByParentForCreateNew(string parentServiceNum, bool withAll = true)
        {
            var services = new ServiceShortDto[0];
            Int32.TryParse(parentServiceNum, out int parentServiceId);
            using (var requestService = new RequestWebServiceClient())
            {
                var mainServices = GetServices(requestService, parentServiceId);
                services = withAll ? new[] { new ServiceShortDto { Id = 0, Name = "" } }.Concat(mainServices).ToArray() : new[] { new ServiceShortDto { Id = 0, Name = "Выберите причину" } }.Concat(mainServices).ToArray();
            }
            return Json(JsonConvert.SerializeObject(services));
        }

        [Authorize]
        public ActionResult ExportRequests()
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var workerId = currentUser.WorkerId;

            var fromDate = (DateTime?)Session[FromDateSessionName] ?? DateTime.Now.Date.AddDays(-30);
            var toDate = (DateTime?)Session[ToDateSessionName] ?? DateTime.Now.Date;
            var selectedWorkerIds = (int[])Session[WorkerSessionName] ?? new int[0];
            var selectedExecuterIds = (int[])Session[ExecuterSessionName] ?? new int[0];
            var selectedStreetIds = (int[])Session[StreetSessionName] ?? new int[0];
            var selectedStatusIds = (int[])Session[StatusSessionName] ?? new int[0];
            var selectedFilterId = (int?)Session[FilterSessionName] ?? 0;
            var selectedHouseIds = (int[])Session[HouseSessionName] ?? new int[0];
            var selectedAddressIds = (int[])Session[AddressSessionName] ?? new int[0];
            var selectedParServIds = (int[])Session[ParServSessionName] ?? new int[0];
            var selectedServiceIds = (int[])Session[ServSessionName] ?? new int[0];
            var selectedRatingIds = (int[])Session[RatingSessionName] ?? new int[0];
            var filterIsChecked = (bool?)Session[FilteringSessionName] ?? false;
            var phoneNumber = (string)Session[PhoneSessionName];
            var requestId = (int?)Session[IdSessionName];
            var isBadWork = (bool?)Session[IsBadWorkSessionName] ?? false;
            var garanty = (bool?)Session[GarantySessionName] ?? false;



            using (var requestService = new RequestWebServiceClient())
            {

                var file = requestService.ExportToExcel(workerId, requestId, fromDate, toDate,
selectedWorkerIds, selectedExecuterIds, selectedStreetIds, selectedHouseIds, selectedAddressIds, selectedStatusIds, selectedParServIds, selectedServiceIds,
isBadWork, garanty, phoneNumber, selectedRatingIds, selectedFilterId == 2);

                return File(file, "application/vnd.ms-excel","Заявки.xlsx");
            }
        }

        [Authorize]
        public ActionResult PrintActs()
        {
            var currentUser = JsonConvert.DeserializeObject<WebUserDto>(HttpContext.User.Identity.Name);
            var workerId = currentUser.WorkerId;

            var fromDate = (DateTime?)Session[FromDateSessionName] ?? DateTime.Now.Date.AddDays(-30);
            var toDate = (DateTime?)Session[ToDateSessionName] ?? DateTime.Now.Date;
            var selectedWorkerIds = (int[])Session[WorkerSessionName] ?? new int[0];
            var selectedExecuterIds = (int[])Session[ExecuterSessionName] ?? new int[0];
            var selectedStreetIds = (int[])Session[StreetSessionName] ?? new int[0];
            var selectedStatusIds = (int[])Session[StatusSessionName] ?? new int[0];
            var selectedFilterId = (int?)Session[FilterSessionName] ?? 0;
            var selectedHouseIds = (int[])Session[HouseSessionName] ?? new int[0];
            var selectedAddressIds = (int[])Session[AddressSessionName] ?? new int[0];
            var selectedParServIds = (int[])Session[ParServSessionName] ?? new int[0];
            var selectedServiceIds = (int[])Session[ServSessionName] ?? new int[0];
            var selectedRatingIds = (int[])Session[RatingSessionName] ?? new int[0];
            var filterIsChecked = (bool?)Session[FilteringSessionName] ?? false;
            var phoneNumber = (string)Session[PhoneSessionName];
            var requestId = (int?)Session[IdSessionName];
            var isBadWork = (bool?)Session[IsBadWorkSessionName] ?? false;
            var garanty = (bool?)Session[GarantySessionName] ?? false;

            using (var requestService = new RequestWebServiceClient())
            {
                var file = requestService.GetRequestActs(workerId, requestId, fromDate, toDate,
selectedWorkerIds, selectedExecuterIds, selectedStreetIds, selectedHouseIds, selectedAddressIds, selectedStatusIds, selectedParServIds, selectedServiceIds,
isBadWork, garanty, phoneNumber, selectedRatingIds, selectedFilterId == 2);

                return File(file, "application/pdf", "Acts.pdf");
            }
        }
    }
}