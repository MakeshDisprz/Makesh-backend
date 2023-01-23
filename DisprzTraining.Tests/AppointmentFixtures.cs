using DisprzTraining.Models;

namespace DisprzTraining.UnitTests.Fixtures;

public static class AppointmentFixture
{
    public static List<AppointmentDto> GetAppointmentDtos() => new() {
        new AppointmentDto()
        {
            Id = new Guid(),
            Title = "DailySyncUp",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
        new AppointmentDto()
        {
            Id = new Guid(),
            Title = "DailySyncUp1",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
        new AppointmentDto()
        {
            Id = new Guid(),
            Title = "DailySyncUp2",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
    };

    public static Dictionary<int, List<AppointmentDto>> GetAppointmentDictionary() => new() {
        {0, GetAppointmentDtos()},
    };


public static List<Appointment> GetAppointments() => new() {
        new Appointment()
        {
            Id = new Guid(),
            Title = "DailySyncUp",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
        new Appointment()
        {
            Id = new Guid(),
            Title = "DailySyncUp1",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
        new Appointment()
        {
            Id = new Guid(),
            Title = "DailySyncUp2",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        },
    };
}