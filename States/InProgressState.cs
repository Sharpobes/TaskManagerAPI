using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.States
{
    public class InProgressState : ITaskState
    {
        public void Start(Task task)
        {
            Console.WriteLine("Task is already in progress.");
        }

        public void Complete(Task task)
        {
            task.StatusId = 3; // Done
            Console.WriteLine("Task is completed.");
        }

        public void Hold(Task task)
        {
            task.StatusId = 4; // On Hold
            Console.WriteLine("Task is now on hold.");
        }
    }

}
