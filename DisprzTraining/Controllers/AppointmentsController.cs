using DisprzTraining.Business;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Controllers
{
    [Route("v1/api")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _appointmentBL;
        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL;
        }

        /// <summary>
        /// Return appointments.
        /// </summary>
        ///<remarks>
        /// Sample request:
        ///
        ///      day : "2023-01-23"(yyyy-mm-dd)
        ///      month : "2023-01"(yyyy-mm)
        ///
        /// </remarks>
        /// <response code="200"> Returns a Dictionary data with key as integer and value as a list of appointments </response>
        /// <response code="404">No appointments found</response>

        //- GET /api/appointments

        [HttpGet("appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDictionary<int, List<AppointmentDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IDictionary<int, List<Array>>))]

        public async Task<ActionResult> Get([FromQuery] Request request)
        {
            var appointments = await _appointmentBL.GetAsync(request);
            return appointments.Count != 0 ? Ok(appointments) : NotFound();
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "title": "string",
        ///        "startTime": "2023-01-02T09:33:03.125",
        ///        "endTime": "2023-01-02T09:33:03.125"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created appointment</response>
        /// <response code="409">If there is a conflict</response>

        //- POST /api/appointments

        [HttpPost("appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AppointmentDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(AppointmentDto))]

        public async Task<IActionResult> Post(CreateAppointmentDto appointmentDto)
        {
            if (appointmentDto.StartTime >= appointmentDto.EndTime) return BadRequest();

            if ((appointmentDto.StartTime.Year != appointmentDto.EndTime.Year)
            || (appointmentDto.StartTime.Month != appointmentDto.EndTime.Month)
            || (appointmentDto.StartTime.Day != appointmentDto.EndTime.Day)) return BadRequest();

            var newAppointment = await _appointmentBL.CreateAsync(appointmentDto);
            return newAppointment == null ? Conflict() : CreatedAtAction(nameof(Get), new { Id = newAppointment.Id }, newAppointment);
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": "8d6812c7-348b-419f-b6f9-d626b6c1d363",
        ///        "title": "string",
        ///        "startTime": "2023-01-02T09:33:03.125",
        ///        "endTime": "2023-01-02T09:33:03.125"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the updated appointment</response>
        /// <response code="409">If there is a conflict</response>
        //- PUT /api/appointments
        [HttpPut("appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AppointmentDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(AppointmentDto))]

        public async Task<IActionResult> Put(AppointmentDto appointmentDto)
        {
            if (appointmentDto.StartTime >= appointmentDto.EndTime) return BadRequest();

            if ((appointmentDto.StartTime.ToLocalTime().Year != appointmentDto.EndTime.ToLocalTime().Year) || (appointmentDto.StartTime.ToLocalTime().Month != appointmentDto.EndTime.ToLocalTime().Month) || (appointmentDto.StartTime.ToLocalTime().Day != appointmentDto.EndTime.ToLocalTime().Day)) return NoContent();
            
            var newAppointment = await _appointmentBL.UpdateAsync(appointmentDto);
            return newAppointment == null ? Conflict() : CreatedAtAction(nameof(Get), new { Id = newAppointment.Id }, newAppointment);
        }

        /// <summary>
        /// Deletes an existing appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///        id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///
        /// </remarks>
        /// <response code="204">Appointment deleted successfully</response>
        /// <response code="404">Appointment is not found</response>
        //- DELETE /api/appointments/{Id}
        [HttpDelete("appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(AppointmentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(AppointmentDto))]

        public async Task<IActionResult> Delete(Guid Id)
        {
            var boo = await _appointmentBL.Delete(Id);
            return boo ? NoContent() : NotFound(new AppointmentDto());
        }
    }
}
