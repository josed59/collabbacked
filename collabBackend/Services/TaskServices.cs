using collabBackend.Models;
using collabDB;
using collabDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace collabBackend.Services
{
    public class TaskServices : ITaskServices
    {
        CollabContext context;
        IUserServices UserServices;


        public TaskServices(CollabContext dbcontext, IUserServices userServices)
        {
            context = dbcontext;
            UserServices = userServices;
        }


        //Get all Sizes
        public ResponseBase<IEnumerable<TaskSize>> GetSize()
        {
            var taskSizes = context.TaskSizes;
            var response = new ResponseBase<IEnumerable<TaskSize>>
            {
                Success = true,
                Error = string.Empty,
                Data = taskSizes
            };

            return response;
        }
        //get All states
        public ResponseBase<IEnumerable<TaskState>> GetState()
        {
            var taskStates = context.TaskStates;
            var response = new ResponseBase<IEnumerable<TaskState>>
            {
                Success = true,
                Error = string.Empty,
                Data = taskStates
            };

            return response;
        }

        //GET all task from the user
        public async Task<ResponseBase<object>> getAllTask(HttpContext httpContext, string s, int? querypage, int? querypageSize, string sort, List<string>? state, Guid? taskId, Guid? SearchUserID, Guid? isNotUserId)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            var role = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;
            var userIdNullable = getCurrentUserId(httpContext);
            Guid userId = userIdNullable ?? Guid.Empty;
            if (userId == Guid.Empty || string.IsNullOrEmpty(role))
            {
                // No se guardó ninguna entidad o ocurrió un error
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "User not registered",
                    Data = null
                };
            }

            var query = context.UsersTasks
           .Where(p => (!string.IsNullOrEmpty(role) && role == "1") ||
                       (userId == Guid.Empty && p.OwnerId == userId) ||
                       (!string.IsNullOrEmpty(role) && p.OwnerId == userId) ||
                       (userId != Guid.Empty && p.UserId == userId))
           .Join(
               context.TaskSizes,
               userTask => userTask.TaskSizeId,
               taskSize => taskSize.TaskSizeId,
               (userTask, taskSize) => new { UserTask = userTask, TaskSize = taskSize }
           )
           .Join(
               context.TaskStates,
               userTaskAndTaskSize => userTaskAndTaskSize.UserTask.TaskStateId,
               taskState => taskState.TaskStateId,
               (userTaskAndTaskSize, taskState) => new
               {
                   UserTask = userTaskAndTaskSize.UserTask,
                   TaskSize = userTaskAndTaskSize.TaskSize,
                   TaskState = taskState,
               }
           )
           .Where(result => result.TaskState.TaskStateId != 4) //condición TaskStateId != 4 aquí
           .GroupJoin(
               context.Users,
               userTaskAndTaskSizeAndTaskState => userTaskAndTaskSizeAndTaskState.UserTask.UserId,
               user => user.UserId,
               (userTaskAndTaskSizeAndTaskState, users) => new
               {
                   UserTask = userTaskAndTaskSizeAndTaskState.UserTask,
                   TaskSize = userTaskAndTaskSizeAndTaskState.TaskSize,
                   TaskState = userTaskAndTaskSizeAndTaskState.TaskState,
                   Users = users,
               }
           )
           .SelectMany(
               result => result.Users.DefaultIfEmpty(),
               (result, user) => new
               {
                   TaskId = result.UserTask.TaskId,
                   TaskSizeId = result.UserTask.TaskSizeId,
                   TaskSizeName = result.TaskSize.TaskDescription,
                   TaskStateName = result.TaskState.Description,
                   Title = result.UserTask.Title,
                   Description = result.UserTask.Description,
                   From = result.UserTask.From.ToString("dd-MM-yyyy"),
                   To = result.UserTask.To.ToString("dd-MM-yyyy"),
                   Assign = user != null ? user.Name : "Unassigned",
                   userTest = result.UserTask.UserTest,
                   qaDate = result.UserTask.QaDateFinished != null ? result.UserTask.QaDateFinished.Value.ToString("dd-MM-yyyy") : null,
                   closeDate = result.UserTask.CloseDate != null ? result.UserTask.CloseDate.Value.ToString("dd-MM-yyyy") : null,
                   userId = result.UserTask.UserId,
               }
           );

            //filter by taskid
            if (taskId != Guid.Empty)
            {
                query = query.Where(p => p.TaskId == taskId);
            }


            // filter by task title
            if (!string.IsNullOrEmpty(s))
            {
                query = query.Where(p => p.Title.Contains(s) || p.Description.Contains(s));
            }

            if (state != null && state.Count > 0)
            {
                query = query.Where(p => state.Contains(p.TaskStateName));
            }

            //Filter By userID
            if (SearchUserID != Guid.Empty)
            {
                query = query.Where(p => p.userId == SearchUserID);
            }

            //Filter where  userID is diferent to isNotUserId
            if (isNotUserId != Guid.Empty)
            {
                query = query.Where(p => p.userId != isNotUserId);
            }

            // order asc
            if (string.IsNullOrEmpty(sort) || sort.ToLower() == "asc")
            {
                query = query.OrderBy(p => p.Title);
            }
            else if (sort.ToLower() == "desc")
            {
                query = query.OrderByDescending(p => p.Title);
            }

            //order desc

            //pagination 
            int page = querypage.GetValueOrDefault(1) == 0 ? 1 : querypage.GetValueOrDefault(1);
            int pageSize = querypageSize.GetValueOrDefault(5) == 0 ? 5 : querypageSize.GetValueOrDefault(5);
            var total = query.Count();

            return new ResponseBase<object>
            {
                Success = true,
                Error = "",
                Data = new
                {
                    task = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                    total,
                    page,
                    pageSize,
                    last_page = (int)Math.Ceiling((double)total / pageSize) <= 0 ? 1 : (int)Math.Ceiling((double)total / pageSize)

                }

            };

        }

        // insert new Task
        public async Task<ResponseBase<object>> insertTask(InsertTaskDTO task, HttpContext httpContext)
        {


            var userIdNullable = getCurrentUserId(httpContext);
            Guid userId = userIdNullable ?? Guid.Empty;
            if (userId == Guid.Empty)
            {
                // No se guardó ninguna entidad o ocurrió un error
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "Error inserting Task",
                    Data = null
                };
            }

            Guid guid = Guid.NewGuid();
            var usertask = new UsersTask {
                TaskId = guid,
                Title = task.Title,
                Description = task.Description,
                From = task.From,
                To = task.To,
                UserTest = task.UserTest,
                TaskSizeId = task.TaskSizeId,
                TaskStateId = 1,
                OwnerId = userId
            };
            context.Add(usertask);
            int savedEntities = await context.SaveChangesAsync();

            if (savedEntities > 0)
            {
                // Las entidades fueron guardadas correctamente
                return new ResponseBase<object>
                {
                    Success = true,
                    Error = "",
                    Data = new
                    {
                        TaskId = guid
                    }
                };
            }
            else
            {
                // No se guardó ninguna entidad o ocurrió un error
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "Error inserting Task",
                    Data = null
                };
            }
        }

        //assing Task
        public async Task<ResponseBase<object>> assingTask(assingTaskDTO assingTask, HttpContext httpContext) {
            try
            {
                var userTask = await context.UsersTasks.FindAsync(assingTask.taskID);
                var grand = await checkPermisson(httpContext, assingTask.taskID);


                if (userTask != null && grand)
                {
                    var preUser = userTask.UserId;
                    userTask.UserId = assingTask.userId;
                    userTask.TaskStateId = 2;

                    context.UsersTasks.Update(userTask);
                    int affectedRows = await context.SaveChangesAsync();

                    if (affectedRows > 0)
                    {
                        return new ResponseBase<object>
                        {
                            Success = true,
                            Error = "",
                            Data = preUser,

                        };
                    }
                    else
                    {
                        return new ResponseBase<object>
                        {
                            Success = false,
                            Error = "An error occurred while updating UserTask",
                            Data = null
                        };
                    }

                }
                else
                {
                    return new ResponseBase<object>
                    {
                        Success = false,
                        Error = "UserTask not found",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "An error occurred while updating UserTask",
                    Data = null
                };
            }
        }

        //Assing task Massive
        public async Task<ResponseBase<object>> assingMAssiveTask(assingMassiveTaskDTO amt, HttpContext httpContext)
        {
            
            try
            {
                List<TaskResult> results = new List<TaskResult>();

                foreach (Guid taskid in amt.taskIds) {
                    var userTask = await context.UsersTasks.FindAsync(taskid);
                    var grand = await checkPermisson(httpContext, taskid);

                    bool isSuccessful = false;

                    if (userTask != null && grand)
                    {
                        Guid? preUser = userTask.UserId as Guid?;
                        userTask.UserId = amt.userId;
                        userTask.TaskStateId = 2;

                        context.UsersTasks.Update(userTask);
                        int affectedRows = await context.SaveChangesAsync();

                        isSuccessful = affectedRows > 0;
                        if (affectedRows > 0)
                        {
                            await UserServices.updateCapacity(taskid, preUser);
                        }
                    }
                    results.Add(new TaskResult { TaskId = taskid, IsSuccessful = isSuccessful });
                }
                bool anyFailed = results.Any(result => !result.IsSuccessful);
                return new ResponseBase<object>
                {
                    Success = !anyFailed,
                    Error = anyFailed ? "Some items were not saved correctly" : "",
                    Data = results
                };


            }
            catch(Exception ex) {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "An error occurred while updating UserTask",
                    Data = null
                };
            }
        }

        //Update Task
        public async Task<ResponseBase<object>> UpdateTask(updateTaskDTO updateTask, HttpContext httpContext)
        {
            try
            {
                var userTask = await context.UsersTasks.FindAsync(updateTask.TaskId);
                var hasPermission = await checkPermisson(httpContext, updateTask.TaskId);

                if (userTask != null && hasPermission && userTask.TaskStateId != 3)
                {
                    var userIdNullable = getCurrentUserId(httpContext);
                    Guid userId = userIdNullable ?? Guid.Empty;
                    userTask.Description = updateTask.Description ?? userTask.Description;
                    userTask.From = updateTask.From ?? userTask.From;
                    userTask.To = updateTask.To ?? userTask.To;
                    userTask.TaskSizeId = updateTask.Size ?? userTask.TaskSizeId;
                    userTask.UserTest = updateTask.userTest ?? userTask.UserTest;

                    if (updateTask.isCompleted == true)
                    {
                        userTask.TaskStateId = 3;
                        userTask.CloseDate = updateTask.updateDate;
                    }
                    if(updateTask.isUatFinished == true)
                    {
                        userTask.QaDateFinished = updateTask.updateDate;
                    }
                    if(updateTask.isStandBy == true)
                    {
                        userTask.TaskStateId = 5;
                    }
                    if (updateTask.isStandBy == false)
                    {
                        userTask.TaskStateId = 2;
                    }
                    if (updateTask.isDeleted == true)
                    {
                        userTask.TaskStateId = 4;
                        userTask.UserId = null;
                    }

                    context.UsersTasks.Update(userTask);

                    int affectedRows = await context.SaveChangesAsync();
                    if (affectedRows <= 0)
                    {
                        return new ResponseBase<object>
                        {
                            Success = false,
                            Error = "UserTask not found or already completed",
                            Data = null
                        };
                    }

                    var taskChangeLogsForTaskId = context.TaskChangeLogs
                    .Where(p => p.TaskId == updateTask.TaskId);

                    var maxCreationDate = await taskChangeLogsForTaskId.AnyAsync()
                        ? await taskChangeLogsForTaskId.MaxAsync(p => p.Date)
                        : DateTime.MinValue;

                    TaskChangeLogs log;

                    if (maxCreationDate != DateTime.MinValue)
                    {
                       
                        var lastupdate = await context.TaskChangeLogs
                            .FirstOrDefaultAsync(p => p.TaskId == updateTask.TaskId && p.Date == maxCreationDate);
                         log = new TaskChangeLogs
                        {
                            Comment = updateTask.Comment,
                            TaskId = updateTask.TaskId,
                            Date = DateTime.Now,
                            IsCompleted = updateTask.isCompleted ?? (lastupdate != null ? lastupdate.IsCompleted : false),
                            isUatCompleted = updateTask.isUatFinished ?? (lastupdate != null ? lastupdate.isUatCompleted : false),
                            userid = userId
                        };

                    }
                    else
                    {
                         log = new TaskChangeLogs
                        {
                            Comment = updateTask.Comment,
                            TaskId = updateTask.TaskId,
                            Date = DateTime.Now,
                            IsCompleted = false,
                            isUatCompleted = false,
                            userid = userId
                        };
                    }



                    context.Add(log);
                    int savedEntities = await context.SaveChangesAsync();
                    if (savedEntities > 0)
                    {
                        return new ResponseBase<object>
                        {
                            Success = true,
                            Error = "",
                            Data = null
                        };
                    }
                }

                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "UserTask not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return new ResponseBase<object>
                {
                    Success = false,
                    Error = "An error occurred while updating UserTask",
                    Data = null
                };
            }
        }


        // validate token jwt data for Userid
        private Guid? getCurrentUserId(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
                Guid userId = context.Users
                    .Where(p => p.Email == email).Select(user => user.UserId).FirstOrDefault();

                if (userId != Guid.Empty)
                {
                    return userId;
                }
                return null;

            }
            return null;

        }
        
        //validate user and role
        private async Task<bool> checkPermisson(HttpContext httpContext,Guid tasdkID)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var role = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;
                    if(role == "1")
                    {
                        return true;
                    }
                    var userIdNullable = getCurrentUserId(httpContext);
                    Guid userId = userIdNullable ?? Guid.Empty;
                    var taskInfo = await context.UsersTasks
                        .Where(p => p.TaskId == tasdkID).FirstOrDefaultAsync();

                    if (taskInfo.UserId == userId || taskInfo.OwnerId == userId)
                    {
                        return true;
                    }
                    return false;

            }
            return false;
        }

    }

}
 public interface ITaskServices
{
    ResponseBase<IEnumerable<TaskSize>> GetSize();
    Task<ResponseBase<object>> insertTask(InsertTaskDTO task, HttpContext httpContext);
    Task<ResponseBase<object>> getAllTask(HttpContext httpContext, string s, int? querypage, int? querypageSize, string sort, List<string>? state, Guid? taskId, Guid? SearchUserID, Guid? isNotUserId);
    Task<ResponseBase<object>> assingTask(assingTaskDTO assingTask, HttpContext httpContext);
    Task<ResponseBase<object>> UpdateTask(updateTaskDTO updateTask, HttpContext httpContext);
    ResponseBase<IEnumerable<TaskState>> GetState();
    Task<ResponseBase<object>> assingMAssiveTask(assingMassiveTaskDTO amt, HttpContext httpContext);


}