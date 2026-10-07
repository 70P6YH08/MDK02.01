namespace Task5
{
    public class CustomTask
    {
        private List<string> _listPriority = new() { "Low", "Medium", "High" };
        private List<string> _listStatus = new() { "Active", "Completed", "Inactive" };

        public int Id { get; set; }

        private string _title = null!;
        public string Title
        {
            get => _title;
            set
            {
                if (!String.IsNullOrEmpty(value))
                    _title = value;
            }
        }

        public string? Description { get; set; }

        private string _priority = null!;
        public string Priority
        {
            get => _priority;
            set
            {
                var setValue = value.ToLower().Trim();
                if (_listPriority.Contains(setValue))
                {
                    int index = _listPriority.IndexOf(setValue);
                    _priority = _listPriority[index];
                }
            }
        }

        private string _status = null!;
        public string Status
        {
            get => _status;
            set
            {
                var setValue = value.ToLower().Trim();
                if (_listStatus.Contains(setValue))
                {
                    int index = _listStatus.IndexOf(setValue);
                    _status = _listStatus[index];
                }
            }
        }
    }
}
