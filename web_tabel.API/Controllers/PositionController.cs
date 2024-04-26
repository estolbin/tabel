using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public PositionController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet("GetAllPosition")]
        public Task<IActionResult> Get()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.PositionRepository.GetAll()));
        }

        [HttpGet("GetPositionById")]
        public Task<IActionResult> Get(Guid id)
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.PositionRepository.SingleOrDefault(x => x.Id == id)));
        }

        [HttpPost("AddPosition")]
        public Task<IActionResult> AddPosition([FromBody] Position position)
        {
            var orpganization = _unitOfWork.OrganizationRepository.SingleOrDefault(x => x.Id == position.Organization.Id);
            var department = _unitOfWork.DepartmentRepository.SingleOrDefault(x => x.Id == position.Department.Id);

            position.Department = department;
            position.Organization = orpganization;

            var positions = _unitOfWork.PositionRepository.SingleOrDefault(x => x.Id == position.Id);
            if (position == null)
            {
                _unitOfWork.PositionRepository.Insert(position);
                _unitOfWork.Save();
            }
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPut("UpdatePosition")]
        public Task<IActionResult> UpdatePosition([FromBody] Position position)
        {
            _unitOfWork.PositionRepository.Update(position);
            _unitOfWork.Save();
            return Task.FromResult<IActionResult>(Ok());
        }

    }
}
