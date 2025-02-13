using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.States
{
    public class NewState : ITaskState
    {
        public void Start(Task task)
        {
            task.StatusId = 2; // In Progress
            Console.WriteLine("Task moved to In Progress.");
        }

        public void Complete(Task task)
        {
            Console.WriteLine("Cannot complete a task that hasn't started.");
        }

        public void Hold(Task task)
        {
            task.StatusId = 4; // On Hold
            Console.WriteLine("Task is now on hold.");
        }
    }

}
