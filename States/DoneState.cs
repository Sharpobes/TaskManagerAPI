using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.States
{
    public class DoneState : ITaskState
    {
        public void Start(Task task)
        {
            Console.WriteLine("Cannot restart a completed task.");
        }

        public void Complete(Task task)
        {
            Console.WriteLine("Task is already completed.");
        }

        public void Hold(Task task)
        {
            Console.WriteLine("Cannot hold a completed task.");
        }
    }

}
