using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PipelineTest.Data.Context;
using PipelineTest.Models;
using PipelineTest.Models.Dto;
using PipelineTest.Repository.IRepository;

namespace PipelineTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        ResponseDto _responseDto;

        public TodosController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
            _responseDto = new ResponseDto();
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            IEnumerable<TodoDto> todos = new List<TodoDto>();
            try
            {
                todos = await _todoRepository.GetTodos();
            }
            catch (Exception ex)
            {
                _responseDto.Errors = new List<string>() { ex.Message, 
                    ex.InnerException?.Message };
                _responseDto.Message = ex.Message;
            }
            if (_responseDto.Errors != null)
            {
                _responseDto.IsSuccessful = false;
                return BadRequest(_responseDto);
            }

            _responseDto.Result = todos;
            _responseDto.IsSuccessful = true;

            _responseDto.Message = (todos.Count() > 0) 
                ? "Todos fetched successfully." 
                : "There are no todos to show";

            return Ok(_responseDto);
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            TodoDto? todoDto = new TodoDto();
            try
            {
                todoDto = await _todoRepository.GetTodo(id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccessful = false;
                _responseDto.Message = ex.Message;
                _responseDto.Errors = new List<string>() { ex.Message, 
                    ex.InnerException?.Message };
            }
            if (_responseDto.Errors != null)
            {
                return NotFound(_responseDto);
            }

            _responseDto.Result = todoDto;

            if (todoDto != null)
            {
                _responseDto.IsSuccessful = true;
                _responseDto.Message = "Todo fetched successfully";
                return Ok(_responseDto);
            }
            
            _responseDto.IsSuccessful = false;
            _responseDto.Message = "Todo does not exist";
            return NotFound(_responseDto);
        }

        // PUT: api/Todos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, TodoDto todoDto)
        {
            TodoDto? todoDtoResult = new TodoDto();
            try
            {
                todoDtoResult = await _todoRepository.PutTodo(id, todoDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccessful = false;
                _responseDto.Message = ex.Message;
                _responseDto.Errors = new List<string>() { ex.Message, 
                    ex.InnerException?.Message };
            }
            if (_responseDto.Errors != null)
            {
                return BadRequest(_responseDto);
            }

            _responseDto.Result = todoDtoResult;

            if (todoDtoResult != null)
            {
                _responseDto.IsSuccessful = true; 
                _responseDto.Message = "Todo updated successfully";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccessful = false;
            _responseDto.Message = "Todo update failed";
            return StatusCode(500, _responseDto);
        }

        // POST: api/Todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostTodo(TodoDto todoDto)
        {
            TodoDto todoDtoResult = new TodoDto();
            try
            {
                _responseDto.Result = await _todoRepository.PostTodo(todoDto);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccessful = false;
                _responseDto.Message = ex.Message;
                _responseDto.Errors = new List<string>() { ex.Message, 
                    ex.InnerException?.Message };
            }
            if (_responseDto.Errors != null)
            {
                return BadRequest(_responseDto);
            }

            _responseDto.Result = todoDtoResult;

            if (todoDtoResult != null)
            {
                _responseDto.IsSuccessful = true;
                _responseDto.Message = "Todo created successfully";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccessful = false;
            _responseDto.Message = "Todo creation failed";
            return StatusCode(500, _responseDto);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            TodoDto todoDtoResult = new TodoDto();
            try
            {
                todoDtoResult = await _todoRepository.DeleteTodo(id);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccessful = false;
                _responseDto.Message = ex.Message;
                _responseDto.Errors = new List<string>() { ex.Message, 
                    ex.InnerException?.Message };
            }
            if (_responseDto.Errors != null)
            {
                return BadRequest(_responseDto);
            }

            _responseDto.Result = todoDtoResult;

            if (todoDtoResult != null)
            {
                _responseDto.IsSuccessful = true;
                _responseDto.Message = "Todo deleted successfully";
                return Ok(_responseDto);
            }

            _responseDto.IsSuccessful = false;
            _responseDto.Message = "Todo deletion failed";
            return StatusCode(500, _responseDto);
        }
        
    }
}
