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


    public static List<Appointment> GetAppointments() => new() {
        new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d361"),
                Title = "M1",
                StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d362"),
                Title = "M2",
                StartTime = new DateTime(2022, 12, 30, 7, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 8, 10, 20)
            },
    };

    public static List<Appointment> GetAppointment() => new() {
        new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d361"),
                Title = "M1",
                StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d362"),
                Title = "M2",
                StartTime = new DateTime(2022, 12, 30, 7, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 8, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d363"),
                Title = "M3",
                StartTime = new DateTime(2022, 12, 31, 5, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 8, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d364"),
                Title = "M4",
                StartTime = new DateTime(2022, 12, 31, 10, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 11, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
                Title = "M5",
                StartTime = new DateTime(2022, 12, 27, 11, 10, 20),
                EndTime = new DateTime(2022, 12, 27, 11, 50, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
                Title = "DailySyncUp",
                StartTime = new DateTime(2022, 12, 31, 3, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 4, 10, 20)
            }
    };
    public static List<Appointment> GetAll() => new() {
        new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d361"),
                Title = "M1",
                StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d362"),
                Title = "M2",
                StartTime = new DateTime(2022, 12, 30, 7, 10, 20),
                EndTime = new DateTime(2022, 12, 30, 8, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d363"),
                Title = "M3",
                StartTime = new DateTime(2022, 12, 31, 5, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 8, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d364"),
                Title = "M4",
                StartTime = new DateTime(2022, 12, 31, 10, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 11, 10, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
                Title = "M5",
                StartTime = new DateTime(2022, 12, 27, 11, 10, 20),
                EndTime = new DateTime(2022, 12, 27, 11, 50, 20)
            },
            new Appointment{
                Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
                Title = "DailySyncUp",
                StartTime = new DateTime(2022, 12, 31, 3, 10, 20),
                EndTime = new DateTime(2022, 12, 31, 4, 10, 20)
            }
    };
}
