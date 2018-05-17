namespace request_web.Dto
{
    public class ServiceWithParrentShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ServiceShortDto[] Children { get; set; }
    }
}