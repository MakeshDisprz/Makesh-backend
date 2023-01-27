using DisprzTraining.DataAccess;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {

        private readonly IAppointmentDAL _appointmentDAL;

        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }


        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto appointmentDto)
        {
            Appointment appointment = new()
            {
                Id = Guid.NewGuid(),
                Title = appointmentDto.Title,
                StartTime = appointmentDto.StartTime.ToLocalTime(),
                EndTime = appointmentDto.EndTime.ToLocalTime()
            };
            return (await _appointmentDAL.Create(appointment)).AsDto();
        }


        public async Task<List<AppointmentDto>> ConflictValidate(DateTime startTime, DateTime endTime)
        {
            return (await _appointmentDAL.ConflictValidate(startTime.ToLocalTime(), endTime.ToLocalTime()))
                    .Select(appointment => appointment.AsDto()).ToList();
        }


        public async Task<List<AppointmentDto>> UpdateValidate(Guid id, DateTime startTime, DateTime endTime)
        {
            return (await _appointmentDAL.UpdateValidate(id, startTime.ToLocalTime(), endTime.ToLocalTime()))
                    .Select(appointment => appointment.AsDto()).ToList();
        }


        public async Task<AppointmentDto> UpdateAsync(AppointmentDto appointmentDto)
        {
            Appointment appointment = new()
            {
                Id = appointmentDto.Id,
                Title = appointmentDto.Title,
                StartTime = appointmentDto.StartTime.ToLocalTime(),
                EndTime = appointmentDto.EndTime.ToLocalTime()
            };
            return (await _appointmentDAL.Update(appointment)).AsDto();
        }


        public Task<bool> Delete(Guid Id)
        {
            return _appointmentDAL.Delete(Id);
        }


        public async Task<List<AppointmentDto>> GetAsync(Request request)
        {
            return (await _appointmentDAL.Get(request)).Select(appointment => appointment.AsDto()).ToList();
        }
    }
}
