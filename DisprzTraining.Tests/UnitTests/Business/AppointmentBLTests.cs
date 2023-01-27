using DisprzTraining.UnitTests.Fixtures;
using DisprzTraining.Business;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using FluentAssertions;
using Moq;

namespace DisprzTraining.UnitTests.Business;

public class AppointmentBLTests
{

    [Fact]
    public async Task GetAsync_AppointmentDtos()
    {
        // Arrange
        Request request = new Request();
        request.Day = new DateTime();
        var appointments = AppointmentFixture.GetAppointment();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();
        var mockAppointmentDAL = new Mock<IAppointmentDAL>();

        mockAppointmentDAL
            .Setup(s => s.Get(request))
            .Returns(Task.FromResult(appointments));

        var sut = new AppointmentBL(mockAppointmentDAL.Object);

        // Act
        var result = await sut.GetAsync(request);

        // Assert
        result.Should().BeOfType<List<AppointmentDto>>();// 1

        result.Should().BeEquivalentTo(
                   AppointmentFixture.GetAppointment(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers());//2
    }

    [Fact]
    public async Task GetAsync_Day_AppointmentDtos()
    {
        // Arrange
        Request request = new Request();
        request.Day = new DateTime(2022, 12, 30, 5, 10, 20);
        // request.Month = DateTime.MinValue;
        var appointments = AppointmentFixture.GetAppointments();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();
        var mockAppointmentDAL = new Mock<IAppointmentDAL>();

        mockAppointmentDAL
            .Setup(s => s.Get(request))
            .Returns(Task.FromResult(appointments));

        var sut = new AppointmentBL(mockAppointmentDAL.Object);

        // Act
        var result = await sut.GetAsync(request);

        // Assert
        result.Should().BeOfType<List<AppointmentDto>>();// 1

        result.Should().BeEquivalentTo(
                   AppointmentFixture.GetAppointments(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers());//2
    }

    [Fact]
    public async Task GetAsync_Month_AppointmentDtos()
    {
        // Arrange
        Request request = new Request();
        request.Day = DateTime.MinValue;
        request.Month = new DateTime(2022, 12, 30, 5, 10, 20);
        var appointments = AppointmentFixture.GetAppointments();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();
        var mockAppointmentDAL = new Mock<IAppointmentDAL>();

        mockAppointmentDAL
            .Setup(s => s.Get(request))
            .Returns(Task.FromResult(appointments));

        var sut = new AppointmentBL(mockAppointmentDAL.Object);

        // Act
        var result = await sut.GetAsync(request);

        // Assert
        result.Should().BeOfType<List<AppointmentDto>>();// 1

        result.Should().BeEquivalentTo(
                   AppointmentFixture.GetAppointments(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers());//2
    }

    [Fact]
    public async Task Delete_Boolean()
    {
        // Arrange
        var check = false;
        Guid id = new Guid();
        var mockAppointmentBL = new Mock<IAppointmentDAL>();

        mockAppointmentBL
            .Setup(s => s.Delete(id))
            .Returns(Task.FromResult(check));

        var sut = new AppointmentBL(mockAppointmentBL.Object);

        // Act
        var result = await sut.Delete(id);

        // Assert
        Assert.Equal(check, result);
    }

    [Fact]
    public async Task CreateAsync_CreatedAppointment()
    {
        // Arrange
        CreateAppointmentDto appointmentDto = new CreateAppointmentDto
        {
            Title = "A1",
            StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
            EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
        };

        Appointment appointment = new Appointment
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360"),
            Title = "A1",
            StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
            EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
        };

        var mockAppointmentBL = new Mock<IAppointmentDAL>();
        mockAppointmentBL
            .Setup(s => s.Create(It.IsAny<Appointment>()))
            .Returns(Task.FromResult(appointment));

        var sut = new AppointmentBL(mockAppointmentBL.Object);

        // Act
        var result = await sut.CreateAsync(appointmentDto);

        // Assert
        result.Should().BeOfType<AppointmentDto>();// 1
        result.Should().BeEquivalentTo(
                   appointment.AsDto(),
                   options => options.ComparingByMembers<Appointment>().ExcludingMissingMembers());//2
    }

    [Fact]
    public async Task ConflictValidate_AppointmentDtos()
    {
        // Arrange
        CreateAppointmentDto appointmentDto = new CreateAppointmentDto
        {
            Title = "A1",
            StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
            EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
        };

        var appointments = AppointmentFixture.GetAppointments();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();

        var mockAppointmentBL = new Mock<IAppointmentDAL>();
        
        mockAppointmentBL
            .Setup(s => s.ConflictValidate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult(new List<Appointment>()));

        var sut = new AppointmentBL(mockAppointmentBL.Object);

        // Act
        var result = await sut.ConflictValidate(appointmentDto.StartTime, appointmentDto.EndTime);

        // Assert
        result.Should().BeOfType<List<AppointmentDto>>();// 1
    }

    [Fact]
    public async Task UpdateValidate_AppointmentDtos()
    {
        Appointment appointment = new Appointment
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360"),
            Title = "A1",
            StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
            EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
        };

        var mockAppointmentBL = new Mock<IAppointmentDAL>();
        mockAppointmentBL
            .Setup(s => s.UpdateValidate(appointment.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult(new List<Appointment>()));

        var sut = new AppointmentBL(mockAppointmentBL.Object);

        // Act
        var result = await sut.UpdateValidate(appointment.Id, appointment.StartTime, appointment.EndTime);

        // Assert
        result.Should().BeOfType<List<AppointmentDto>>();// 1
    }

    [Fact]
    public async Task UpdateAsync_UpdatedAppointment()
    {
        // Arrange
        Appointment appointment = new Appointment
        {
            Id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360"),
            Title = "A1",
            StartTime = new DateTime(2022, 12, 30, 5, 10, 20),
            EndTime = new DateTime(2022, 12, 30, 6, 10, 20)
        };
        var mockAppointmentBL = new Mock<IAppointmentDAL>();
        mockAppointmentBL
            .Setup(s => s.Update(It.IsAny<Appointment>()))
            .Returns(Task.FromResult(appointment));

        var sut = new AppointmentBL(mockAppointmentBL.Object);

        // Act
        var result = await sut.UpdateAsync(appointment.AsDto());

        // Assert
        result.Should().BeOfType<AppointmentDto>();// 1
    }

}
