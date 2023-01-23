using DisprzTraining.Models;

public static class Extensions
{
    public static AppointmentDto AsDto(this Appointment appointment)
    {
        return new AppointmentDto{
            Id = appointment.Id,
            Title = appointment.Title,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime
        };
    }
}