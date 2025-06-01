using Microsoft.AspNetCore.Mvc;
using APBD9.DTOs;
using APBD9.Services;
using APBD9.Exceptions;

namespace APBD9.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly IPrescriptionService _hospitalService;

        public HospitalController(IPrescriptionService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpPost("prescription")]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionReqDto request)
        {
            try
            {
                var prescriptionId = await _hospitalService.AddPrescriptionAsync(request);
                return Created($"api/hospital/prescriptions/{prescriptionId}", prescriptionId);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("patients/{idPatient}")]
        public async Task<IActionResult> GetPatientDetails([FromRoute] int idPatient)
        {
            try
            {
                var patientDetails = await _hospitalService.GetPatientDetailsAsync(idPatient);
                return Ok(patientDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}