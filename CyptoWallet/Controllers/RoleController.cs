using CyptoWallet.DTOs;
using CyptoWallet.Entities;
using CyptoWallet.Helper;
using CyptoWallet.Infraestructure;
using CyptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyptoWallet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///  Obtengo todos los roles
        /// </summary>
        /// <returns>devuelde todos los roles</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            int pageToShow = 1;
            if (Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
            var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString();
            var paginateUsers = PaginateHelper.Paginate(roles, pageToShow, url);

            return ResponseFactory.CreateSuccessResponse(200, paginateUsers);
        }


        /// <summary>
        ///  Inserta un rol
        /// </summary>
        /// <returns>Devuelve un mensaje de confirmacion del agrego del rol</returns>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("Alta")]
        public async Task<IActionResult> Insert(RoleDto dto)
        {

            var Role = new Role(dto);
            await _unitOfWork.RoleRepository.CreateAsync(Role);
            await _unitOfWork.Complete();
            return ResponseFactory.CreateSuccessResponse(200, "Ok.Se agrego el rol");
        }


        /// <summary>
        ///  Actualiza un rol
        /// </summary>
        /// <returns>Devuelve un mensaje de actualizacion de rol</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut("Modificar/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, Role role)
        {
            var result = await _unitOfWork.RoleRepository.UpdateAsync(role);

            await _unitOfWork.Complete();
            return Ok(true);
        }

        /// <summary>
        ///  Se elimina un rol
        /// </summary>
        /// <returns>Devuelve un mensaje de eliminacion de rol</returns>
        [Authorize(Policy = "Admin")]
        [HttpPut("Eliminar/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _unitOfWork.RoleRepository.DeleteAsync(id);
            await _unitOfWork.Complete();
            return ResponseFactory.CreateSuccessResponse(200, "Ok.Se elimino el rol");
        }
    }
}
