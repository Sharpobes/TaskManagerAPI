using TaskManagerAPI.Entities;
using TaskManagerAPI.States;
using Task = TaskManagerAPI.Entities.Task;
namespace TaskManagerAPI.Services
{
    public class TaskStateService
    {
        public ITaskState GetState(Task task)
        {
            return task.StatusId switch
            {
                1 => new NewState(),
                2 => new InProgressState(),
                3 => new DoneState(),
                4 => new OnHoldState(),
                _ => throw new InvalidOperationException("Unknown state")
            };
        }

        public void SetState(Task task, ITaskState state)
        {
            if (state is NewState) task.StatusId = 1;
            else if (state is InProgressState) task.StatusId = 2;
            else if (state is DoneState) task.StatusId = 3;
            else if (state is OnHoldState) task.StatusId = 4;
        }
    }
}
