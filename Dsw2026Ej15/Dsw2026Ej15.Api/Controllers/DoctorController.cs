using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


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
    public IActionResult CreateDoctor([FromBody] CreateDoctorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre es requerido.");

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            throw new ValidationException("El número de licencia es requerido.");

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality == null)
            throw new ValidationException("La especialidad ingresada no existe.");

        var doctor = new Doctor
        {
            Name = request.Name,
            LicenseNumber = request.LicenseNumber,
            IsActive = true,
            Speciality = speciality
        };

        _persistence.AddDoctor(doctor);
        return Created("", doctor);
    }

    [HttpGet]
    public IActionResult GetActiveDoctors()
    {
        return Ok(_persistence.GetActiveDoctors());
    }

    [HttpGet("{id}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);
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
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);
        if (doctor == null)
            return NotFound("Médico no encontrado o inactivo.");

        _persistence.SetDoctorInactive(id);
        return NoContent();
    }
}
