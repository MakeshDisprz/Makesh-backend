using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        Task<Appointment> Create(Appointment appointment);

        Task<List<Appointment>> Get( Request request);

        Task<Appointment> Update(Appointment appointment);

        Task<bool> Delete(Guid Id);

        Task<List<Appointment>> ConflictValidate(DateTime startTime, DateTime endTime);
        
        Task<List<Appointment>> UpdateValidate(Guid id, DateTime startTime, DateTime endTime);
    }
}
