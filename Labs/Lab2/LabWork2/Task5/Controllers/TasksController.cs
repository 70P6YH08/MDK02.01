using Microsoft.AspNetCore.Mvc;
using System.Text;

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
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    var task = line.Split(';');
                    tasks.Add(new CustomTask
                    {
                        Id = Convert.ToInt32(task[0]),
                        Title = task[1],
                        Description = task[2],
                        Priority = task[3],
                        Status = task[4]
                    });
                }
            }
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetTaskByIdAsync))]
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
                Priority = dataTask[3],
                Status = dataTask[4]
            };

            if (task == null)
                return NotFound();
            return Ok(task);
        }


        [HttpPost]
        public async Task<ActionResult> PostTaskAsync([FromBody] CustomTask newTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Файл не найден");

            var lines = System.IO.File.ReadAllLines(filePath);

            if (lines.Length > 0)
            {
                var lastLine = lines.Last(l => !String.IsNullOrEmpty(l));
                if (!String.IsNullOrEmpty(lastLine))
                {
                    var lastTaskData = lastLine.Split(';');
                    if (lastTaskData.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(lastTaskData[0]) && int.TryParse(lastTaskData[0], out int lastTaskId))
                        {
                            if (lastTaskId >= newTask.Id)
                                newTask.Id = lastTaskId + 1;
                        }
                    }
                }
            }

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    await streamWriter.WriteLineAsync($"{newTask.Id};" +
                        $"{newTask.Title};" +
                        $"{newTask.Description};" +
                        $"{newTask.Priority};" +
                        $"{newTask.Status}");
                }
                return CreatedAtAction(nameof(GetTaskByIdAsync), new { Id = newTask.Id }, newTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTaskAsync(int id, CustomTask inputTask)
        {
            if (id != inputTask.Id)
                return BadRequest();

            var lines = System.IO.File.ReadAllLines(filePath);

            var findLine = lines.FirstOrDefault(l => l.StartsWith($"{id}"));

            if (findLine == null)
                return NotFound();

            var taskData = findLine.Split(';');

            taskData[0] = inputTask.Id;

            inputTask = new()
            {
                Id = Convert.ToInt32(taskData[0]),
                Title = taskData[1],
                Description = taskData[2],
                Priority = taskData[3],
                Status = taskData[4]
            };

            using (StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                await streamWriter.WriteLineAsync($"{inputTask.Id};" +
                    $"{inputTask.Title};" +
                    $"{inputTask.Description};" +
                    $"{inputTask.Priority};" +
                    $"{inputTask.Status}");
            }

            if (inputTask == null)
                return NotFound();
            return Ok(inputTask);
        }
    }
}
