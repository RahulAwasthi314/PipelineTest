using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PipelineTest.Models;

namespace PipelineTest.Data.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
                
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
