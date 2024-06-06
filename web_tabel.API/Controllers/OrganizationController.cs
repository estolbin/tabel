using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private UnitOfWork _unitOfWork;
        public OrganizationController() 
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpPost("AddOrganization")]
        public async Task<IActionResult> AddOrganization([FromBody] Organization organization)
        {
            if (await _unitOfWork.OrganizationRepository.SingleOrDefaultAsync(x => x.Id == organization.Id) != null) { return BadRequest("Organization already exists"); }
            await _unitOfWork.OrganizationRepository.InsertAsync(organization);
            await _unitOfWork.SaveAsync();
            return Ok("Success");
        }

        [HttpGet("GetAllOrganization")]
        public async Task<IActionResult> GetAllOrganization()
        {
            return Ok(await _unitOfWork.OrganizationRepository.GetAllAsync());
        }

        [HttpGet("GetOrganizationById")]
        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            return Ok(await _unitOfWork.OrganizationRepository.SingleOrDefaultAsync(x => x.Id == id));
        }

        [HttpPut("UpdateOrganization")]
        public async Task<IActionResult> UpdateOrganization([FromBody] Organization organization)
        {
            await _unitOfWork.OrganizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveAsync();
            return Ok("Success");
        }
    }
}
