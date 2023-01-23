using System.Collections;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto appointmentDto);

        Task<List<AppointmentDto>> GetAsync(Request request);

        Task<AppointmentDto> UpdateAsync(AppointmentDto appointmentDto);

        Task<bool> Delete(Guid Id);

        Task<List<AppointmentDto>> ConflictValidate(DateTime startTime, DateTime endTime);
        
        Task<List<AppointmentDto>> UpdateValidate(Guid id, DateTime startTime, DateTime endTime);
    }
}
