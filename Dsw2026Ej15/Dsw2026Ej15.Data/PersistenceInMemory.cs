using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data.Persistence
{
    using Dsw2026Ej15.Domain;
    using System.Numerics;
    using System.Text.Json;

    public class PersistenceInMemory : IPersistence
    {
        private readonly List<Doctor> _doctors = new();
        private readonly List<Speciality> _specialities = new();

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        private void LoadSpecialities()
        {
            // Se carga el JSON en el inicio. Asume que el archivo "specialities.json" está en el directorio.
            if (File.Exists("specialities.json"))
            {
                var json = File.ReadAllText("specialities.json");
                var loaded = JsonSerializer.Deserialize<List<Speciality>>(json);
                if (loaded != null)
                    _specialities.AddRange(loaded);
            }
        }

        public List<Doctor> GetActiveDoctors() => _doctors.Where(d => d.IsActive).ToList();

        public Doctor GetDoctorById(Guid id) => _doctors.FirstOrDefault(d => d.Id == id && d.IsActive);

        public void AddDoctor(Doctor doctor) => _doctors.Add(doctor);

        public void SetDoctorInactive(Guid id)
        {
            var doctor = GetDoctorById(id);
            if (doctor != null) doctor.IsActive = false;
        }

        public Speciality GetSpecialityById(Guid id) => _specialities.FirstOrDefault(s => s.Id == id);
    }
}