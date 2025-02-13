using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.States
{
    public class OnHoldState : ITaskState
    {
        public void Start(Task task)
        {
            task.StatusId = 2; // In Progress
            Console.WriteLine("Task resumed and moved to In Progress.");
        }

        public void Complete(Task task)
        {
            Console.WriteLine("Cannot complete a task on hold.");
        }

        public void Hold(Task task)
        {
            Console.WriteLine("Task is already on hold.");
        }
    }
}
