using Microsoft.AspNetCore.Mvc;
using web_tabel.Domain;
using web_tabel.Services;

namespace web_tabel.API.Controllers
{
    [Route("api/TypeWT")]
    public class TypeOfWorkingTypeController : ControllerBase
    {
        private UnitOfWork _unitOfWork;

        public TypeOfWorkingTypeController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpPost("AddTypeWT")]
        public Task<IActionResult> AddTypeWT([FromBody] TypeOfWorkingTime typeWT)
        {
            var found = _unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefault(x => x.Name == typeWT.Name);

            if (found != null) { return Task.FromResult<IActionResult>(BadRequest("TypeWT already exists")); }

            _unitOfWork.TypeOfWorkingTimeRepository.Insert(typeWT);
            try
            {
                _unitOfWork.Save();
            } catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex.Message));
            }
            return Task.FromResult<IActionResult>(Ok("Success"));
        }


        [HttpGet("GetAllTypeWT")]
        public Task<IActionResult> GetAllTypeWT()
        {
            return Task.FromResult<IActionResult>(Ok(_unitOfWork.TypeOfWorkingTimeRepository.GetAll()));
        }
    }
}
