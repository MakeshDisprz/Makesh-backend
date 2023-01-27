using DisprzTraining.Business;
using DisprzTraining.Responses;
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
        /// <param name="request">Example value : "2023-01-25"</param>
        /// <response code="200"> Returns list of appointments </response>
        /// <response code="404">No appointments found - returns empty list</response>

        //- GET /api/appointments

        [HttpGet("appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppointmentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<>))]

        public async Task<IActionResult> Get([FromQuery] Request request)
        {
            var appointments = await _appointmentBL.GetAsync(request);
            return appointments.Count != 0 ? Ok(appointments) : NotFound(new List<AppointmentDto>());
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <response code="201">Returns the newly created appointment's id</response>
        /// <response code="409">Conflict-returns list of appointments with conflict</response>

        //- POST /api/appointments

        [HttpPost("appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Created))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Conflicts))]

        public async Task<IActionResult> Post(CreateAppointmentDto appointmentDto)
        {

            if ((string.IsNullOrEmpty(appointmentDto.Title))
            || (appointmentDto.StartTime == DateTime.MinValue)
            || (appointmentDto.EndTime == DateTime.MinValue))
                return BadRequest(new EmptyError());

            if (appointmentDto.StartTime >= appointmentDto.EndTime)
                return BadRequest(new EarlyError());

            if (appointmentDto.StartTime.ToLocalTime().Date != appointmentDto.EndTime.ToLocalTime().Date)
                return BadRequest(new DayError());

            if ((appointmentDto.StartTime.ToLocalTime() < DateTime.Now.ToLocalTime())
            || (appointmentDto.EndTime.ToLocalTime() < DateTime.Now.ToLocalTime()))
                return BadRequest(new PastError());

            var newAppointments = await _appointmentBL.ConflictValidate(appointmentDto.StartTime, appointmentDto.EndTime);

            if (newAppointments.Count != 0)
            {
                return Conflict(new Conflicts { ConflictAppointments = newAppointments });
            }
            else
            {
                var newAppointment = await _appointmentBL.CreateAsync(appointmentDto);
                return CreatedAtAction(nameof(Get), new Created { id = newAppointment.Id });
            }

        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <response code="200">Update successful</response>
        /// <response code="409">Conflict-returns list of appointments with conflict</response>

        // - PUT /api/appointments

        [HttpPut("appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Updated))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Conflicts))]

        public async Task<IActionResult> Put(AppointmentDto appointmentDto)
        {

            if ((string.IsNullOrEmpty(appointmentDto.Title))
            || (appointmentDto.StartTime == DateTime.MinValue)
            || (appointmentDto.EndTime == DateTime.MinValue))
                return BadRequest(new EmptyError());

            if (appointmentDto.StartTime >= appointmentDto.EndTime)
                return BadRequest(new EarlyError());

            if (appointmentDto.StartTime.ToLocalTime().Date != appointmentDto.EndTime.ToLocalTime().Date)
                return BadRequest(new DayError());

            if ((appointmentDto.StartTime.ToLocalTime() < DateTime.Now.ToLocalTime())
            || (appointmentDto.EndTime.ToLocalTime() < DateTime.Now.ToLocalTime()))
                return BadRequest(new PastError());

            var newAppointments = await _appointmentBL.UpdateValidate(appointmentDto.Id, appointmentDto.StartTime, appointmentDto.EndTime);

            if (newAppointments.Count != 0)
            {
                return Conflict(new UpdateConflict { ConflictAppointments = newAppointments });
            }
            else
            {
                var newAppointment = await _appointmentBL.UpdateAsync(appointmentDto);
                return Ok(new Updated());
            }
        }

        /// <summary>
        /// Deletes an existing appointment.
        /// </summary>
        /// <param name="Id">Example value : "3fa85f64-5717-4562-b3fc-2c963f66afa6"</param>
        /// <response code="204">Appointment deleted successfully</response>
        /// <response code="404">Appointment not found</response>

        //- DELETE /api/appointments/{Id}
        
        [HttpDelete("appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<>))]

        public async Task<IActionResult> Delete(Guid Id)
        {
            return await _appointmentBL.Delete(Id) ? NoContent() : NotFound(new List<AppointmentDto>());
        }
    }
}
