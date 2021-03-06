using System;
using System.ComponentModel.DataAnnotations;
using request_web.WebService;
using request_web.Dto;

namespace request_web.Models
{
    public class RequestListModel
    {
        public int LineNumber = 1;
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
        public int? RequestId { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name = "���. �������")]
        public bool AdditionFiltering { get; set; }
        [Display(Name = "��� ����������!")]
        public bool IsBadWork { get; set; }

        [Display(Name = "�����������")]
        public bool Garanty { get; set; }
        public DateTime CurrentDate { get; set; }

        public RequestForListDto[] Requests { get; set; }
        public FilterShortDto[] FilterByDate { get; set; }
        public WorkerShortDto[] Masters { get; set; }
        public WorkerShortDto[] Executers { get; set; }
        public StreetShortDto[] Streets { get; set; }
        public StatusShortDto[] Statuses { get; set; }
        public HouseShortDto[] Houses { get; set; }
        public AddressShortDto[] Addresses { get; set; }
        public ServiceShortDto[] ParentServices { get; set; }
        public ServiceWithParrentShortDto[] Services { get; set; }
        public RatingShortDto[] Ratings { get; set; }
        //public string SelectedWorker { get; set; }

    }
}