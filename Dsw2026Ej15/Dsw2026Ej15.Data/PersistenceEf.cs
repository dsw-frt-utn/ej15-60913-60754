using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceEf : IPersistence
{
    private readonly Dsw2026Ej15DbContext _context;

    public PersistenceEf(Dsw2026Ej15DbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetActiveDoctorsAsync()
    {
        return await _context.Doctors
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid id)
    {
        return await _context.Doctors
            .Include(d => d.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
    }

    public async Task AddDoctorAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync(); // Impacta en la base de datos de manera asíncrona
    }

    public async Task SetDoctorInactiveAsync(Guid id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
        if (doctor != null)
        {
            doctor.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Speciality> GetSpecialityByIdAsync(Guid id)
    {
        return await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id);
    }
}