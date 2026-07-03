using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Data.Persistence;

public interface IPersistence
{
    Task<List<Doctor>> GetActiveDoctorsAsync();
    Task<Doctor> GetDoctorByIdAsync(Guid id);
    Task AddDoctorAsync(Doctor doctor);
    Task SetDoctorInactiveAsync(Guid id);
    Task<Speciality> GetSpecialityByIdAsync(Guid id);
}