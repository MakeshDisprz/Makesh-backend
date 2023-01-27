using DisprzTraining.Models;


namespace DisprzTraining.Responses
{

    public class EmptyError
    {
        public string Message { get; set; } = "Appointment details cannot be empty";
    }

    public class EarlyError
    {
        public string Message { get; set; } = "End time is earlier than start time";
    }

    public class PastError
    {
        public string Message { get; set; } = "Start time or end time has past values";
    }

    public class DayError
    {
        public string Message { get; set; } = "Start time and end time are in different day";
    }

    public class Conflicts
    {
        public string Message { get; set; } = "Appointment not created !!! Conflict with below appointments:";
        public List<AppointmentDto> ConflictAppointments { get; set; }
    }

    public class UpdateConflict
    {
        public string Message { get; set; } = "Appointment not updated !!! Conflict with below appointments:";
        public List<AppointmentDto> ConflictAppointments { get; set; }
    }

    public class Created
    {
        public string Message { get; set; } = "Appointment created successfully";
        public Guid id { get; set; }

    }

    public class Updated
    {
        public string Message { get; set; } = "Appointment updated successfully";
    }

}

