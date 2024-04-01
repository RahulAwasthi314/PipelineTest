using PipelineTest.Models;
using PipelineTest.Models.Dto;

namespace PipelineTest.Repository.IRepository
{
    public interface ITodoRepository
    {
        public Task<IEnumerable<TodoDto>?> GetTodos();
        public Task<TodoDto?> GetTodo(int id);
        public Task<TodoDto?> PutTodo(int id, TodoDto todo);
        public Task<TodoDto?> DeleteTodo(int id);
        public Task<TodoDto?> PostTodo(TodoDto todo);
    }
}
