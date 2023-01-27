using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {

        public static List<Appointment> appointments = new List<Appointment>{
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
            }
        };

        public Task<Appointment> Create(Appointment appointment)
        {
            appointments.Add(appointment);
            return Task.FromResult(appointment);
        }

        public Task<List<Appointment>> ConflictValidate(DateTime startTime, DateTime endTime)
        {
            var newAppointments = appointments.Where(
                s => (
                        (startTime >= s.StartTime && startTime <= s.EndTime)
                        || (endTime >= s.StartTime && endTime <= s.EndTime)
                        || (startTime <= s.StartTime && endTime >= s.EndTime)
                    )
                ).ToList();

            return Task.FromResult(newAppointments);
        }

        public Task<List<Appointment>> UpdateValidate(Guid id, DateTime startTime, DateTime endTime)
        {
            var newAppointments = appointments.Where(
                s => ( id != s.Id &&
                        ( (startTime >= s.StartTime && startTime <= s.EndTime)
                        || (endTime >= s.StartTime && endTime <= s.EndTime)
                        || (startTime <= s.StartTime && endTime >= s.EndTime) )
                    )
                ).ToList();

            return Task.FromResult(newAppointments);
        }

        public Task<Appointment> Update(Appointment appointment)
        {
            var existingAppointment = appointments.Find(s => s.Id == appointment.Id);
            existingAppointment.Title = appointment.Title;
            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            return Task.FromResult(appointment);
        }

        public Task<bool> Delete(Guid Id)
        {
            var appointment = appointments.Find(s => s.Id == Id);
            if (appointment is null)
            {
                return Task.FromResult(false);
            }
            appointments.Remove(appointment);
            return Task.FromResult(true);
        }

        public Task<List<Appointment>> Get(Request request)
        {
             var newAppointments = request.Day != DateTime.MinValue ? 
             appointments.Where(s => s.StartTime.Date == request.Day.ToLocalTime().Date).ToList()
             : request.Month != DateTime.MinValue ? 
             appointments.Where(s => (s.StartTime.Year == request.Month.ToLocalTime().Year) && (s.StartTime.Month == request.Month.ToLocalTime().Month)).ToList()
             : appointments;
             
            return Task.FromResult(newAppointments);
        }
    }
}
