using Microsoft.AspNetCore.Mvc;

using collabBackend.Services;
using collabBackend.Models;
using System.ComponentModel.DataAnnotations;
using collabDB.Models;
using Microsoft.AspNetCore.Authorization;
using collabBackend.Filters;

namespace collabBackend.Controllers
{
    [Route("api/[controller]")]
    [CustomAuthorizationFilter]
    public class TeamMemberController : ControllerBase
    {
        private readonly IUserServices _userService;

        public TeamMemberController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertTeamMember([FromBody] TeamMemberModel teamMember)
        {
            try
            {
                var result = await _userService.InsertUser(teamMember);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                Console.WriteLine(ex);
                return StatusCode(500, new
                {
                    success = false,
                    token = "",
                    error = "Ocurrió un error en el servidor.",
                    usuario = ""
                });

            }
        }

        [HttpGet]
        //Get team members
        public IActionResult GetTeamMember(
            [FromQuery(Name = "s")] string s,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "searchUser")] Guid searchUserId

            )
        {
            try
            {
                int teamId = _userService.getCurrentUserTeam(HttpContext);
                var result = _userService.getTeamMembers(teamId, s, page, pageSize,sort, searchUserId);
                if (result != null)
                {
                    // Comprobar si el resultado es un objeto ResponseBase con Success igual a true
                    if (result is ResponseBase<Object> response && response.Success)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return StatusCode(500, new ResponseBase<Object>
                    {
                        Success = false,
                        Error = "Ocurrió un error en el servidor.",
                        Data = null
                    });
                }


            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return StatusCode(500, new ResponseBase<Object>
                {
                    Success = false,
                    Error = "Ocurrió un error en el servidor.",
                    Data = null
                });

            }
        }

        //delete Team Member form Team
        [HttpPut("deleteTeamMember")]
        public async Task<IActionResult> deleteTeamMember([FromBody]deleteFromTeamDTO data)
        {
            try
            {
                var result = await _userService.deleteFromTeam(data, HttpContext);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores inesperados
                Console.WriteLine(ex);
                return StatusCode(500, new
                {
                    success = false,
                    token = "",
                    error = "Ocurrió un error en el servidor.",
                    usuario = ""
                });

            }
        }


    }
}
