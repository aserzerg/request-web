using System;
using System.ComponentModel.DataAnnotations;
using request_web.WebService;
using request_web.Dto;

namespace request_web.Models
{
    public class RequestListModel
    {
        public WebUserDto UserInfo { get; set; }
        [Required(ErrorMessage = "���������� ��������� ���� {0}.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "�:")]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "���������� ��������� ���� {0}.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "��:")]
        public DateTime ToDate { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name = "���. �������")]
        public bool AdditionFiltering { get; set; }

        public RequestForListDto[] Requests { get; set; }
        public WorkerShortDto[] Workers { get; set; }
        public StreetShortDto[] Streets { get; set; }
        public StatusShortDto[] Statuses { get; set; }
        public HouseShortDto[] Houses { get; set; }
        public AddressShortDto[] Addresses { get; set; }
        public ServiceShortDto[] ParentServices { get; set; }
        public ServiceShortDto[] Services { get; set; }
        //public string SelectedWorker { get; set; }

    }
}