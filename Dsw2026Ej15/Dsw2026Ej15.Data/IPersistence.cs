using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Dsw2026Ej15.Data.Persistence
{
    using Dsw2026Ej15.Domain;

    public interface IPersistence
    {
        List<Doctor> GetActiveDoctors();
        Doctor GetDoctorById(Guid id);
        void AddDoctor(Doctor doctor);
        void SetDoctorInactive(Guid id);
        Speciality GetSpecialityById(Guid id);
    }
}