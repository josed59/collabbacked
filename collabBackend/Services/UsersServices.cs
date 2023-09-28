using collabBackend.Models;
using collabDB;
using collabDB.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;

namespace collabBackend.Services
{

    public class UsersServices : IUserServices
    {
        CollabContext context;
        //context 
        public UsersServices(CollabContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task<ResponseBase<string>> InsertUser(TeamMemberModel tm)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(tm);

            // Realizar la validación del modelo
            if (!Validator.TryValidateObject(tm, validationContext, validationResults, true))
            {
                // El modelo no es válido, lanzar una excepción o manejar el error
                string errorMessage = string.Join(Environment.NewLine, validationResults);
                return new ResponseBase<string>
                {
                    Success = false,
                    Error = errorMessage,
                    Data = null
                };
                throw new Exception(errorMessage);
            }
            bool setUser = await InsertUsert(tm);
            if (setUser)
            {

                bool setUserTeam = await insertTeam(tm);
                if (!setUserTeam)
                {
                    return new ResponseBase<string>
                    {
                        Success = false,
                        Error = "usuario no ingresado",
                        Data = null
                    };

                }
                return new ResponseBase<string>
                {
                    Success = true,
                    Error = "",
                    Data = "OK"
                };

            } else
            {
                return new ResponseBase<string>
                {
                    Success = false,
                    Error = "usuario no ingresado",
                    Data = null
                };

            }



            // El modelo es válido, realizar la inserción de datos
            // Aquí se incluiría la lógica para insertar los datos en la base de datos o realizar otras operaciones

            Console.WriteLine("Datos insertados correctamente.");


        }

        private async Task<bool> InsertUsert(TeamMemberModel tm)
        {
            Guid guid = Guid.NewGuid();
            int type = 2;
            var Email = context.Users.FirstOrDefault(u => u.Email == tm.Email);
            if (Email != null)
            {
                return false;
            }
            //se define usuario a insertar
            var user = new User
            {
                UserId = guid,
                Name = tm.Name,
                Email = tm.Email,
                UserTypeId = type,
                Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };
            context.Add(user);
            int savedEntities = await context.SaveChangesAsync();

            if (savedEntities > 0)
            {
                // Las entidades fueron guardadas correctamente
                return true;
            }
            else
            {
                // No se guardó ninguna entidad o ocurrió un error
                return false;
            }


        }

        private async Task<bool> insertTeam(TeamMemberModel tm)
        {
            var Userid = context.Users.FirstOrDefault(u => u.Email == tm.Email);
            Console.WriteLine("entro en insert team member");
            if (Userid != null)
            {
                var setTeamMember = new TeamsMembers
                {
                    UserId = Userid.UserId,
                    TeamId = tm.TeamId
                };

                context.Add(setTeamMember);
                int savedEntities = await context.SaveChangesAsync();

                if (savedEntities > 0)
                {
                    // Las entidades fueron guardadas correctamente
                    return true;
                }
                else
                {
                    // No se guardó ninguna entidad o ocurrió un error
                    return false;
                }
            } else
            {

                return false;
            }

        }

        //extract team member information 
        public Object getTeamMembers(int teamId, string s, int? querypage, int? querypageSize, string sort, Guid? memberId)
        {
            try
            {
                int filterTeam = teamId;
                var query = context.TeamsMembers
                    .Where(p => p.TeamId == filterTeam)
                    .Join(context.Users,
                            teamMember => teamMember.UserId,
                            user => user.UserId,
                            (teamMember, user) => new
                            {
                                UserName = user.Name,
                                Email = user.Email,
                                TeamId = teamMember.TeamId,
                                UserId = user.UserId,
                                Capacity = user.Capacity,
                                Color = user.Capacity >= 0 && user.Capacity <= 50 ? "low" :
                                        user.Capacity > 50 && user.Capacity <= 100 ? "good" :
                                        user.Capacity > 100 && user.Capacity <= 115 ? "warn" :
                                        user.Capacity > 115 ? "high" : null
                            });
                // filter by name
                if (!string.IsNullOrEmpty(s))
                {
                    query = query.Where(p => p.UserName.Contains(s));
                }

                //Filter By userID
                if (memberId != Guid.Empty)
                {
                    query = query.Where(p => p.UserId == memberId);
                }

                // order asc
                if (string.IsNullOrEmpty(sort) || sort.ToLower() == "asc")
                {
                    query = query.OrderBy(p => p.UserName);
                }
                else if (sort.ToLower() == "desc")
                {
                    query = query.OrderByDescending(p => p.UserName);
                }

                //order desc

                //pagination 
                int page = querypage.GetValueOrDefault(1) == 0 ? 1 : querypage.GetValueOrDefault(1);
                int pageSize = querypageSize.GetValueOrDefault(5) == 0 ? 5 : querypageSize.GetValueOrDefault(5);
                var total = query.Count();

                return new ResponseBase<Object>
                {
                    Success = true,
                    Error = "",
                    Data = new
                    {
                        teammembers = query.Skip((page - 1) * pageSize).Take(pageSize),
                        total,
                        page,
                        pageSize,
                        last_page = (int)Math.Ceiling((double)total / pageSize) <= 0 ? 1 : (int)Math.Ceiling((double)total / pageSize)

                    }

                };

            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return new ResponseBase<Object>
                {
                    Success = false,
                    Error = "Ocurrió un error en la obtención de datos.",
                    Data = null
                };
            }

        }

        //Delete Member form a Team
        public async Task<ResponseBase<string>> deleteFromTeam(deleteFromTeamDTO data, HttpContext httpContext)
        {
            try
            {
                bool isOwner = await teamOwner(httpContext, data.TeamId);

                if (!isOwner)
                {
                    return new ResponseBase<string>
                    {
                        Success = false,
                        Error = "No tienes permisos para realizar esta acción",
                        Data = null
                    };
                }

                var personaParaEliminar = context.TeamsMembers.FirstOrDefault(p => p.UserId == data.UserId && p.TeamId == data.TeamId);

                if (personaParaEliminar == null)
                {
                    return new ResponseBase<string>
                    {
                        Success = false,
                        Error = "El usuario no pertenece a este equipo o no existe",
                        Data = null
                    };
                }

                // Actualiza capacidad
                await updateUser(data.UserId, data.TeamId);

                context.TeamsMembers.Remove(personaParaEliminar);
                int affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    return new ResponseBase<string>
                    {
                        Success = true,
                        Error = "",
                        Data = "OK"
                    };
                }
                else
                {
                    return new ResponseBase<string>
                    {
                        Success = false,
                        Error = "Ocurrió un error en la eliminación",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return new ResponseBase<string>
                {
                    Success = false,
                    Error = "Ocurrió un error en ejecución",
                    Data = null
                };
            }
        }

        // Validate teams owner
        private async Task<bool> teamOwner(HttpContext httpContext, int Filter)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
                var teamId = await (from us in context.Users
                                    join t in context.Teams on us.UserId equals t.UserId
                                    where t.TeamId == Filter
                                    select new
                                    {
                                        teamId = t.TeamId
                                    }
                             ).ToListAsync();
                //valida si la lista tiene algun valor para retornar true o false
                return teamId.Any();

            }
            return false;
        }

        // validate token jwt data for id a teams
        public int getCurrentUserTeam(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
                int userTeamId = context.Users
                    .Where(p => p.Email == email)
                    .SelectMany(u => u.Teams) // Aplanamos la colección de equipos del usuario
                    .Select(team => team.TeamId)
                    .FirstOrDefault();
                if (userTeamId != 0)
                {
                    return userTeamId;
                }
                return 0;

            }
            return 0;

        }

        //Update Capacity
        public async Task updateCapacity(Guid taskId, Guid? prevUseriD)
        {
            try
            {
                int[] taskStateIds = { 1, 2, 4 };
                var userId = await context.UsersTasks.FindAsync(taskId);
                if (userId == null)
                {
                    return;
                }
                foreach (var currentUserId in new[] { userId.UserId, prevUseriD })
                {
                    if (currentUserId.HasValue)
                    {
                        var user = await context.Users.FindAsync(currentUserId);
                        int sumaTotal = context.UsersTasks
                            .Join(context.TaskSizes, us => us.TaskSizeId, ts => ts.TaskSizeId, (us, ts) => new { UsersTask = us, TaskSize = ts })
                            .Where(ut => ut.UsersTask.UserId == currentUserId && taskStateIds.Contains(ut.UsersTask.TaskStateId))
                            .Sum(ut => ut.TaskSize.Weigth);
                        user.Capacity = sumaTotal;
                        await context.SaveChangesAsync();

                    }
                }

            } catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error actrualizando capacity: " + ex.Message);
            }


        }

        //Update Capacity Team and update task userID
        private async Task updateUser(Guid currentUserId, int filterTeamId) {
            try
            {
                int[] taskStateIds = { 1, 2, 5 };
                var user = await context.Users.FindAsync(currentUserId);
                int sumaTotal = context.UsersTasks
                            .Join(context.TaskSizes, us => us.TaskSizeId, ts => ts.TaskSizeId, (us, ts) => new { UsersTask = us, TaskSize = ts })
                            .Where(ut => ut.UsersTask.UserId == currentUserId && taskStateIds.Contains(ut.UsersTask.TaskStateId))
                            .Sum(ut => ut.TaskSize.Weigth);
                user.Capacity = user.Capacity - sumaTotal;
                await context.SaveChangesAsync();

                // Tomar todos los TaskId del currentUserId y establecer UserId en null
                var tasksToUpdate = await context.UsersTasks
                                    .Join(context.TeamsMembers,
                                          ut => ut.UserId,
                                          tm => tm.UserId,
                                          (ut, tm) => new { UsersTask = ut, TeamMember = tm })
                                    .Where(joined => joined.TeamMember.UserId == currentUserId && joined.TeamMember.TeamId == filterTeamId)
                                    .ToListAsync();

                foreach (var task in tasksToUpdate)
                {
                    task.UsersTask.UserId = null;
                    task.UsersTask.TaskStateId = 1;

                }

                await context.SaveChangesAsync();



            }
            catch(Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Ocurrió un error actrualizando capacity: " + ex.Message);
            }
        }




    }

    public interface IUserServices
    {
        Task<ResponseBase<string>> InsertUser(TeamMemberModel tm);
        Object getTeamMembers(int teamId, string s, int? querypage, int? querypageSize, string sort, Guid? memberId);
        int getCurrentUserTeam(HttpContext httpContext);
        Task updateCapacity(Guid taskId, Guid? prevUseriD);
        Task<ResponseBase<string>> deleteFromTeam(deleteFromTeamDTO data, HttpContext httpContext);



    }
}


