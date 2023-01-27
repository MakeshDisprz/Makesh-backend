using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using DisprzTraining.UnitTests.Fixtures;
using FluentAssertions;

namespace DisprzTraining.UnitTests.DataAccess;

public class AppointmentDALTests
{

    [Fact]
    public async Task Get_Appointments()
    {
        // Arrange
        Request request = new Request();
        Request request1 = new Request();
        Request request2 = new Request();
        Request request3 = new Request();
        
        request.Day = new DateTime(2022, 12, 30, 5, 10, 20);
        request1.Month = new DateTime(2022, 12, 30, 5, 10, 20);
        request3.Day = new DateTime(2022, 12, 30, 5, 10, 20);
        request3.Month = new DateTime(2022, 12, 30, 5, 10, 20);

        List<Appointment> appointments = new List<Appointment>(){
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
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Get(request);
        var result1 = await mockAppointmentDAL.Get(request1);
        var result2 = await mockAppointmentDAL.Get(request2);
        var result3 = await mockAppointmentDAL.Get(request3);

        // Assert
        result.Should().BeEquivalentTo(
                   appointments,
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
        result1.Should().BeEquivalentTo(
                   AppointmentFixture.GetAppointment(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
        result2.Should().BeEquivalentTo(
                   AppointmentFixture.GetAppointment(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
        result3.Should().BeEquivalentTo(
                   appointments,
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
    }

    [Fact]
    public async Task Get_NotFound_EmptyListOfAppointments()
    {
        // Arrange
        Request request = new Request();
        Request request1 = new Request();
        request.Day = new DateTime(2022, 12, 28, 5, 10, 20);
        request1.Month = new DateTime(2022, 10, 30, 5, 10, 20);
        // request.Month = DateTime.MinValue;
        List<Appointment> appointments = new List<Appointment>();
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Get(request);
        var result1 = await mockAppointmentDAL.Get(request1);

        // Assert
        result.Should().BeEquivalentTo(
                   appointments,
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
        result1.Should().BeEquivalentTo(
                   appointments,
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers()
               );
    }

    [Fact]
    public async Task ConflictValidate_1_ListOfAppointments()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d366"),
            Title = "M6",
            StartTime = new DateTime(2022, 12, 31, 10, 20, 20),
            EndTime = new DateTime(2022, 12, 31, 10, 30, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.ConflictValidate(appointment.StartTime, appointment.EndTime);

        // Assert
        result.Should().BeOfType<List<Appointment>>();
    }

    [Fact]
    public async Task ConflictValidate_2_ListOfAppointments()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d366"),
            Title = "M6",
            StartTime = new DateTime(2022, 12, 31, 7, 20, 20),
            EndTime = new DateTime(2022, 12, 31, 11, 30, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.ConflictValidate(appointment.StartTime, appointment.EndTime);

        // Assert
        result.Should().BeOfType<List<Appointment>>();
    }

    [Fact]
    public async Task Create_CreatedAppointment()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
            Title = "M5",
            StartTime = new DateTime(2022, 12, 27, 11, 10, 20),
            EndTime = new DateTime(2022, 12, 27, 11, 50, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Create(appointment);

        // Assert
        Assert.Equal(appointment, result);
    }

    [Fact]
    public async Task UpdateValidate_1_ListOfAppointments()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d362"),
            Title = "M2",
            StartTime = new DateTime(2022, 12, 31, 10, 20, 20),
            EndTime = new DateTime(2022, 12, 31, 10, 30, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.UpdateValidate(appointment.Id, appointment.StartTime, appointment.EndTime);

        // Assert
        result.Should().BeOfType<List<Appointment>>();
    }

    [Fact]
    public async Task UpdateValidate_2_ListOfAppointments()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d362"),
            Title = "M2",
            StartTime = new DateTime(2022, 12, 31, 7, 20, 20),
            EndTime = new DateTime(2022, 12, 31, 11, 30, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.UpdateValidate(appointment.Id, appointment.StartTime, appointment.EndTime);

        // Assert
        result.Should().BeOfType<List<Appointment>>();
    }

    [Fact]
    public async Task Update_UpdatedAppointment()
    {
        // Arrange
        Appointment appointment = new()
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365"),
            Title = "DailySyncUp",
            StartTime = new DateTime(2022, 12, 31, 3, 10, 20),
            EndTime = new DateTime(2022, 12, 31, 4, 10, 20)
        };
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Update(appointment);

        // Assert
        Assert.Equal(appointment, result);
    }

    [Fact]
    public async Task Delete_AppointmentFound_true()
    {
        // Arrange
        Guid Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d365");
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Delete(Id);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public async Task Delete_NotFound_false()
    {
        // Arrange
        Guid Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d369");
        var mockAppointmentDAL = new AppointmentDAL();

        // Act
        var result = await mockAppointmentDAL.Delete(Id);

        // Assert
        Assert.Equal(false, result);
    }
}
