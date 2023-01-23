namespace DisprzTraining.Models
{
    public class CreateAppointmentDto
    {
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}