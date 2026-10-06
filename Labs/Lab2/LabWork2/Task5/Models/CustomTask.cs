namespace Task5.Models
{
    public class CustomTask
    {
        //public enum Priority
        //{
        //    Low = 0,
        //    Medium = 1,
        //    High = 2,
        //}

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        private string _taskPriority = null!;
        public string TaskPriority
        {
            get => _taskPriority;
            set
            {
                //if (String.IsNullOrEmpty(value))
                //    throw new ArgumentException("Приоритет не может быть пустым");
                if (value == "Low")
                    _taskPriority = "Низкий";
                else if (value == "Medium")
                    _taskPriority = "Средний";
                else if (value == "High")
                    _taskPriority = "Высокий";
            }
        }
        private string _status = null!;
        public string Status
        {
            get => _status;
            set
            {
                if (value == "false")
                    _status = "Не выполнена";
                else if (value == "true")
                    _status = "Выполнена";
            }
        }
    }
}
