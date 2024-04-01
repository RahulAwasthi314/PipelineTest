using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PipelineTest.Data.Context;
using PipelineTest.Models;
using PipelineTest.Models.Dto;
using PipelineTest.Repository.IRepository;

namespace PipelineTest.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoRepository(TodoContext context,
            IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>?> GetTodos()
        {
            if (_context.Todos == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<TodoDto>?>(
                await _context.Todos.ToListAsync());
        }

        public async Task<TodoDto?> GetTodo(int id)
        {
            if (_context.Todos == null)
            {
                return null;
            }
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return null;
            }

            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<TodoDto?> PutTodo(int id, TodoDto todoDto)
        {
            Todo todo = _mapper.Map<Todo>(todoDto);

            if (id != todo.Id)
            {
                return null;
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }

            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<TodoDto?> DeleteTodo(int id)
        {
            if (_context.Todos == null)
            {
                return null;
            }
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return null;
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<TodoDto?> PostTodo(TodoDto todoDto)
        {
            Todo todo = _mapper.Map<Todo>(todoDto);

            if (_context.Todos == null)
            {
                return null;
            }
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoDto>(todo);
        }

        private bool TodoExists(int id)
        {
            return (_context.Todos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
