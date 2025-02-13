using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.States
{
    public interface ITaskState
    {
        void Start(Task task);
        void Complete(Task task);
        void Hold(Task task);
    }

}
