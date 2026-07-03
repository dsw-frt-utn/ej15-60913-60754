using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    public class CreateDoctorRequest
    {
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public Guid SpecialityId { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre es requerido.");

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            throw new ValidationException("El número de licencia es requerido.");

        // Se usa await y el nuevo nombre del método Async
        var speciality = await _persistence.GetSpecialityByIdAsync(request.SpecialityId);
        if (speciality == null)
            throw new ValidationException("La especialidad ingresada no existe.");

        var doctor = new Doctor
        {
            Name = request.Name,
            LicenseNumber = request.LicenseNumber,
            IsActive = true,
            Speciality = speciality
        };

        await _persistence.AddDoctorAsync(doctor);
        return Created("", doctor);
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveDoctors()
    {
        var doctors = await _persistence.GetActiveDoctorsAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = await _persistence.GetDoctorByIdAsync(id);
        if (doctor == null)
            return NotFound("Médico no encontrado o inactivo.");

        var response = new
        {
            doctor.Name,
            doctor.LicenseNumber,
            SpecialityName = doctor.Speciality?.Name
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        var doctor = await _persistence.GetDoctorByIdAsync(id);
        if (doctor == null)
            return NotFound("Médico no encontrado o inactivo.");

        await _persistence.SetDoctorInactiveAsync(id);
        return NoContent();
    }
}
