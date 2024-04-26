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
        public Task<IActionResult> AddOrganization([FromBody] Organization organization)
        {
            _unitOfWork.OrganizationRepository.Insert(organization);
            _unitOfWork.Save();
            return Task.FromResult<IActionResult>(Ok("Success"));
        }

        [HttpGet("GetAllOrganization")]
        public Task<IActionResult> GetAllOrganization()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.OrganizationRepository.GetAll()));
        }

        [HttpGet("GetOrganizationById")]
        public Task<IActionResult> GetOrganizationById(Guid id)
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == id)));
        }

        [HttpPut("UpdateOrganization")]
        public Task<IActionResult> UpdateOrganization([FromBody] Organization organization)
        {
            _unitOfWork.OrganizationRepository.Update(organization);
            _unitOfWork.Save();
            return Task.FromResult<IActionResult>(Ok("Success"));
        }
    }
}
