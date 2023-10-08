using collabBackend.Filters;
using collabBackend.Models;
using collabBackend.Services;
using collabDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace collabBackend.Controllers
{
    [Route("api/[controller]")]
    [CustomAuthorizationFilter]
    public class TaskController : ControllerBase
    {
        ITaskServices TaskServices;
        IUserServices UserServices;

        public TaskController(ITaskServices taskServices, IUserServices userServices )
        {

            TaskServices = taskServices;
            UserServices = userServices;
        }



        [HttpGet("GetSize")]
        public IActionResult GetSize()
        {
            try
            {
                var result = TaskServices.GetSize();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return StatusCode(500, new ResponseBase<object>
                {
                    Success = false,
                    Error = "Ocurrió un error en el servidor.",
                    Data = null
                });

            }
        }

        [HttpPost("InsertTask")]
        public async Task<IActionResult> InsertTask([FromBody] InsertTaskDTO task)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage);
                    return BadRequest(new {
                        success = false,
                        error = errors,
                        Data = ""
                    });
                }

                var result = await TaskServices.insertTask(task, HttpContext);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return UnprocessableEntity(result);
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

        [HttpGet("GetStates")]
        public IActionResult GetStates()
        {
            try
            {
                var result = TaskServices.GetState();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return StatusCode(500, new ResponseBase<object>
                {
                    Success = false,
                    Error = "Ocurrió un error en el servidor.",
                    Data = null
                });

            }
        }

        [HttpGet("GetAllTask")]
        public async Task<IActionResult> GetAllTask(
            [FromQuery(Name = "s")] string s,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "state")] List<string> state,
            [FromQuery(Name = "taskid")] Guid taskid,
            [FromQuery(Name = "searchUser")] Guid searchUserId,
            [FromQuery(Name = "isNotUserId")] Guid isNotUserId
            )
        {
            try
            {
                
                var result = await TaskServices.getAllTask(HttpContext, s, page, pageSize, sort, state,taskid, searchUserId, isNotUserId);
               
                // Comprobar si el resultado es un objeto ResponseBase con Success igual a true
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return UnprocessableEntity(result);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return StatusCode(500, new ResponseBase<object>
                {
                    Success = false,
                    Error = "Ocurrió un error en el servidor.",
                    Data = null
                });

            }

        }

        [HttpPut("AssingTaks")]
        public async Task<IActionResult> AssingTaks ([FromBody] assingTaskDTO assingTask)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage);
                    return BadRequest(new
                    {
                        success = false,
                        error = errors,
                        Data = ""
                    });
                }

                var result = await TaskServices.assingTask(assingTask, HttpContext);
                if (result.Success)
                {
                    //validate if result.Data if null
                    Guid prevUser = (result.Data != null && !string.IsNullOrEmpty(result.Data.ToString())) ? (Guid)result.Data : Guid.Empty;
                    await UserServices.updateCapacity(assingTask.taskID,prevUser);
                    return Ok(result);
                }
                else
                {
                    return UnprocessableEntity(result);
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

        [HttpPut("AssingMassiveTaks")]
        public async Task<IActionResult> AssingMassiveTaks([FromBody] assingMassiveTaskDTO assingTasks)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage);
                    return BadRequest(new
                    {
                        success = false,
                        error = errors,
                        Data = ""
                    });
                }

                var result = await TaskServices.assingMAssiveTask(assingTasks, HttpContext);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return UnprocessableEntity(result);
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

        [HttpPut("updateTask")]
        public async  Task<IActionResult> updateTask([FromBody] updateTaskDTO updateTask)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        error = ModelState,
                        Data = ""
                    });
                }
                var result = await TaskServices.UpdateTask(updateTask, HttpContext);
                if (result.Success)
                {      
                    return Ok(result);
                }
                else
                {
                    return UnprocessableEntity(result);
                }


            }
            catch(Exception ex)
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


            return Ok();

        }

    }
}
