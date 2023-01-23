namespace DisprzTraining.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
