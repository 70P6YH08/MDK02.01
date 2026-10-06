using Microsoft.AspNetCore.Mvc;
using Task5.Models;

namespace Task5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : Controller
    {
        private static readonly string filePath = "tasks.csv";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomTask>>> GetTasksAsync()
        {
            List<CustomTask> tasks = new();

            using (StreamReader streamReader = new(filePath))
            {
                string? header = await streamReader.ReadLineAsync();

                string? line;
                while((line = await streamReader.ReadLineAsync()) != null)
                {
                    var task = line.Split(';');
                    tasks.Add(new CustomTask
                    {
                        Id = Convert.ToInt32(task[0]),
                        Title = task[1],
                        Description = task[2],
                        TaskPriority = task[3],
                        Status = task[4]
                    });
                }
            }
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomTask>> GetTaskByIdAsync(int id)
        {
            var lines = System.IO.File.ReadAllLines(filePath);

            var line = lines.FirstOrDefault(t => t.StartsWith($"{id}"));

            if (line == null)
                return NotFound();

            var dataTask = line.Split(";");

            var task = new CustomTask
            {
                Id = Convert.ToInt32(dataTask[0]),
                Title = dataTask[1],
                Description = dataTask[2],
                TaskPriority = dataTask[3],
                Status = dataTask[4]
            };

            if (task == null)
                return NotFound();
            return Ok(task);
        }

        //[HttpGet]
        //public async Task PostTasksAsync()
        //{
        //    string content = @"<form method='post'>
        //        <label>Название:</label><br />
        //        <input name='title' /><br />
        //        <label>Описание задачи:</label><br />
        //        <input name='description' /><br />
        //        <input type='submit' value='Добавить задачу' />
        //    </form>";
        //    Response.ContentType = "text/html;charset=utf-8";
        //    await Response.WriteAsync(content);
        //}

        [HttpPost]
        public async Task<ActionResult> PostTaskAsync(CustomTask newTask)
        {
            if (!System.IO.File.Exists(filePath))
                return BadRequest("Файл не найден");

            var lines = System.IO.File.ReadAllLines(filePath);
            var lastTask = lines.Last();

            var lastTaskId = int.Parse(lastTask.Split(';').First());

            //var newTask = new CustomTask()
            //{
            //    Id = lastTaskId,
            //    Title = title,
            //    Description = description,
            //    TaskPriority = "Low",
            //    Status = "false"
            //};

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                await streamWriter.WriteAsync($"{lastTaskId + 1};" +
                    $"{newTask.Title};" +
                    $"{newTask.Description};" +
                    $"Low;" +
                    $"false");
            }

            return CreatedAtAction(nameof(GetTaskByIdAsync), new { Id = lastTaskId + 1}, newTask);
        }
    }
}
