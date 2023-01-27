using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DisprzTraining.UnitTests.Fixtures;
using Microsoft.AspNetCore.Http;
using DisprzTraining.Responses;

namespace DisprzTraining.UnitTests.Controllers;

public class AppointmentControllerTests
{
    [Fact]
    public async Task Get_AppointmentDtos()
    {
        // Arrange
        Request request = new Request();
        request.Day = new DateTime();
        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
            .Setup(s => s.GetAsync(request))
            .ReturnsAsync(AppointmentFixture.GetAppointmentDtos());

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Get(request);

        // Assert

        result.Should().BeOfType<OkObjectResult>();// 1

        var objectResult = (ObjectResult)result;
        objectResult.Value.Should().BeOfType<List<AppointmentDto>>();// 
        objectResult.StatusCode.Should().Be(200);

    }

    [Fact]
    public async Task Get_NotFound_404()
    {
        // Arrange
        Request request = new Request();
        request.Day = new DateTime();

        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
            .Setup(s => s.GetAsync(request))
            .ReturnsAsync(new List<AppointmentDto>());

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Get(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var objectResult = (NotFoundObjectResult)result;
        objectResult.StatusCode.Should().Be(404);

    }

    [Fact]
    public async Task Post_201()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 5, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 6, 10, 20);
        Appointment appointment1 = new Appointment
        {
            Id = id,
            Title = T,
            StartTime = Start,
            EndTime = End
        };
        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
            .Setup(s => s.ConflictValidate(Start, End))
            .ReturnsAsync(() => new List<AppointmentDto>());
        mockAppointment.Setup(s => s.CreateAsync(dto)).ReturnsAsync(appointment1.AsDto());

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();// 1

        var objectResult = (ObjectResult) result;

        objectResult.Value.Should().BeOfType<Created>();// 2

    }

    [Fact]
    public async Task Post_EmptyInput_BadRequest()
    {
        // Arrange
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 5, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 6, 10, 20);
       
        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = DateTime.MinValue,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult) result;

        objectResult.Value.Should().BeOfType<EmptyError>();// 2

    }

    [Fact]
    public async Task Post_EndLessThanStart_BadRequest()
    {
        // Arrange
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 8, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 6, 10, 20);

        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult) result;

        objectResult.Value.Should().BeOfType<EarlyError>();// 2

    }

    [Fact]
    public async Task Post_DifferentDay_BadRequest()
    {
        // Arrange
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 10, 29, 5, 10, 20);
        DateTime End = new DateTime(2024, 11, 30, 6, 10, 20);
        
        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();
        
        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult) result;

        objectResult.Value.Should().BeOfType<DayError>();// 2

    }

    [Fact]
    public async Task Post_PastTime_BadRequest()
    {
        // Arrange
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 01, 27, 5, 10, 20);
        DateTime End = new DateTime(2023, 01, 27, 6, 10, 20);
        
        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult) result;

        objectResult.Value.Should().BeOfType<PastError>();// 2

    }

    [Fact]
    public async Task Post_Conflict_409()
    {
        // Arrange
        var appointments = AppointmentFixture.GetAppointments();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();

        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string T = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 5, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 6, 10, 20);
        Appointment appointment1 = new Appointment
        {
            Id = id,
            Title = T,
            StartTime = Start,
            EndTime = End
        };
        CreateAppointmentDto dto = new CreateAppointmentDto
        {
            Title = T,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
        .Setup(s => s.ConflictValidate(Start, End))
        .ReturnsAsync(appointmentDtos);

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Post(dto);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();// 1

        var objectResult = (ConflictObjectResult)result;

        objectResult.StatusCode.Should().Be(409);// 2

        objectResult.Value.Should().BeOfType<Conflicts>();// 3
    }

    [Fact]
    public async Task Put_200()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 10, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 11, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = title,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
        .Setup(s => s.UpdateValidate(id, Start, End))
        .ReturnsAsync(() => new List<AppointmentDto>());
        mockAppointment
            .Setup(s => s.UpdateAsync(appointment))
            .ReturnsAsync(appointment);

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<OkObjectResult>();// 1

        var objectResult = (OkObjectResult)result;

        objectResult.StatusCode.Should().Be(200);// 2

        objectResult.Value.Should().BeOfType<Updated>();// 3

    }

    [Fact]
    public async Task Put_EmptyInput_BadRequest()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 10, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 11, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = "",
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult)result;

        objectResult.Value.Should().BeOfType<EmptyError>();// 2

    }

    [Fact]
    public async Task Put_EndLessThanStart_BadRequest()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 10, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 7, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = title,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult)result;

        objectResult.Value.Should().BeOfType<EarlyError>();// 2

    }

    [Fact]
    public async Task Put_PastTime_BadRequest()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 01, 27, 5, 10, 20);
        DateTime End = new DateTime(2023, 01, 27, 7, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = title,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult)result;

        objectResult.Value.Should().BeOfType<PastError>();// 2

    }

    [Fact]
    public async Task Put_DifferentDay_BadRequest()
    {
        // Arrange
        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 11, 30, 10, 10, 20);
        DateTime End = new DateTime(2024, 12, 31, 11, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = title,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();// 1

        var objectResult = (ObjectResult)result;

        objectResult.Value.Should().BeOfType<DayError>();// 2

    }

    [Fact]
    public async Task Put_Conflict_409()
    {
        // Arrange
        var appointments = AppointmentFixture.GetAppointments();
        var appointmentDtos = (appointments.Select(s => s.AsDto())).ToList();

        Guid id = new Guid("8d6812c7-348b-419f-b6f9-d626b6c1d360");
        string title = "DailySyncUp";
        DateTime Start = new DateTime(2023, 12, 30, 10, 10, 20);
        DateTime End = new DateTime(2023, 12, 30, 11, 10, 20);

        AppointmentDto appointment = new AppointmentDto
        {
            Id = id,
            Title = title,
            StartTime = Start,
            EndTime = End
        };

        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
        .Setup(s => s.UpdateValidate(id, Start, End))
        .ReturnsAsync(appointmentDtos);

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Put(appointment);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();// 1

        var objectResult = (ConflictObjectResult)result;

        objectResult.StatusCode.Should().Be(409);// 2
    }

    [Fact]
    public async Task Delete_204()
    {
        // Arrange
        Guid id = new Guid();
        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
            .Setup(s => s.Delete(id))
            .Returns(Task.FromResult(true));

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();// 1

        var objectResult = (NoContentResult)result;

        objectResult.StatusCode.Should().Be(204);// 2
    }

    [Fact]
    public async Task Delete_NotFound_404()
    {
        // Arrange
        Guid id = new Guid();
        var mockAppointment = new Mock<IAppointmentBL>();

        mockAppointment
            .Setup(s => s.Delete(id))
            .Returns(Task.FromResult(false));

        var sut = new AppointmentsController(mockAppointment.Object);

        // Act
        var result = await sut.Delete(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();// 1

        var objectResult = (NotFoundObjectResult)result;

        objectResult.StatusCode.Should().Be(404);// 2
    }
}
