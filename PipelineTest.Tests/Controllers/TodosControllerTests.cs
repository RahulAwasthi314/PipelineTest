using Microsoft.AspNetCore.Mvc;
using Moq;
using PipelineTest.Controllers;
using PipelineTest.Models.Dto;
using PipelineTest.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineTest.Tests.Controllers
{
    public class TodosControllerTests
    {
        #region GetTodosTests
        [Fact]
        public async Task GetTodos_Returns_OK_With_Todos()
        {
            // Arrange
            var mockTodoRepository = new Mock<ITodoRepository>();
            var todos = new List<TodoDto>
            {
                new TodoDto { Id = 1, Title = "Title1", Description = "Description 1" },
                new TodoDto { Id = 2, Title = "Title2", Description = "Description 2" }
            };
            mockTodoRepository.Setup(repo => repo.GetTodos()).ReturnsAsync(todos);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("Todos fetched successfully.", responseDto.Message);
            Assert.Equal(todos, responseDto.Result);
        }

        [Fact]
        public async Task GetTodos_Returns_OK_With_No_Todos()
        {
            // Arrange
            var mockTodoRepository = new Mock<ITodoRepository>();
            var emptyTodos = Enumerable.Empty<TodoDto>();
            mockTodoRepository.Setup(repo => repo.GetTodos()).ReturnsAsync(emptyTodos);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("There are no todos to show", responseDto.Message);
            if (responseDto.Result == null)
            {
                Assert.Empty(emptyTodos);
            }
            else 
            {
                IEnumerable<TodoDto> todoDtos = (IEnumerable<TodoDto>) responseDto.Result;
                Assert.Empty(todoDtos);
            }
        }

        [Fact]
        public async Task GetTodos_Returns_BadRequest_On_Exception()
        {
            // Arrange
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.GetTodos()).ThrowsAsync(new Exception("Repository exception"));

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodos();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Null(responseDto.Result);
            
        }

        #endregion


        #region GetTodoTests

        [Fact]
        public async Task GetTodo_Returns_OK_With_Todo()
        {
            // Arrange
            var todoId = 1;
            var mockTodoRepository = new Mock<ITodoRepository>();
            var todoDto = new TodoDto { Id = todoId, Title = "Title 1", Description = "Description 1" };
            mockTodoRepository.Setup(repo => repo.GetTodo(todoId)).ReturnsAsync(todoDto);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodo(todoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("Todo fetched successfully", responseDto.Message);
            Assert.Equal(todoDto, responseDto.Result);
        }

        [Fact]
        public async Task GetTodo_Returns_NotFound_When_Todo_Is_Null()
        {
            // Arrange
            var todoId = 1;
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.GetTodo(todoId)).ReturnsAsync((TodoDto?)null);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodo(todoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(notFoundResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Equal("Todo does not exist", responseDto.Message);
            Assert.Null(responseDto.Result);
        }

        [Fact]
        public async Task GetTodo_Returns_NotFound_On_Exception()
        {
            // Arrange
            var todoId = 1;
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.GetTodo(todoId)).ThrowsAsync(new Exception("Repository exception"));

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.GetTodo(todoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(notFoundResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Equal("Repository exception", responseDto.Message);
            Assert.Null(responseDto.Result);
        }

        #endregion


        #region PutTodoTests

        [Fact]
        public async Task PutTodo_Returns_OK_When_Successful()
        {
            // Arrange
            var todoId = 1;
            var todoDto = new TodoDto { Id = todoId, Title = "Updated Title", Description = "Description 2" };
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PutTodo(todoId, todoDto)).ReturnsAsync(todoDto);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.PutTodo(todoId, todoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("Todo updated successfully", responseDto.Message);
            Assert.Equal(todoDto, responseDto.Result);
        }

        [Fact]
        public async Task PutTodo_Returns_BadRequest_On_Exception()
        {
            // Arrange
            var todoId = 1;
            var todoDto = new TodoDto { Id = todoId, Title = "Updated Title", Description = "Description 2" };
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PutTodo(todoId, todoDto)).ThrowsAsync(new Exception("Repository exception"));

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.PutTodo(todoId, todoDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Equal("Repository exception", responseDto.Message);
            Assert.Null(responseDto.Result);
        }

        [Fact]
        public async Task PutTodo_Returns_500_On_Null_Result()
        {
            // Arrange
            var todoId = 1;
            var todoDto = new TodoDto { Id = todoId, Title = "Updated Task", Description = "Description 2"};
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PutTodo(todoId, todoDto)).ReturnsAsync((TodoDto?)null);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.PutTodo(todoId, todoDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion


        #region PostTodoTests

        [Fact]
        public async Task PostTodo_Returns_OK_When_Successful()
        {
            // Arrange
            var todoDto = new TodoDto { Title = "Title 1", Description = "Description 1" };
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PostTodo(todoDto)).ReturnsAsync(todoDto);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.PostTodo(todoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("Todo created successfully", responseDto.Message);
            Assert.Equal(todoDto, responseDto.Result);
        }

        [Fact]
        public async Task PostTodo_Returns_BadRequest_On_Exception()
        {
            // Arrange
            var todoDto = new TodoDto { Title = "Title 1", Description = "Description 1" };
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PostTodo(todoDto)).ThrowsAsync(new Exception("Repository exception"));

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.PostTodo(todoDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Equal("Repository exception", responseDto.Message);
            Assert.Null(responseDto.Result);
        }

        [Fact]
        public async Task PostTodo_Returns_BadRequest_On_Null_Result()
        {
            // Arrange
            var todoDto = new TodoDto { Title = "Title 1", Description = "Description 2"};
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.PostTodo(todoDto)).ReturnsAsync((TodoDto?)null);

            var controller = new TodosController(mockTodoRepository.Object);
            controller.ModelState.AddModelError("Format invalid!", "Todo is not valid");

            // Act
            var result = await controller.PostTodo(todoDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion


        #region DeleteTodoTests

        [Fact]
        public async Task DeleteTodo_Returns_OK_When_Successful()
        {
            // Arrange
            var todoId = 1;
            var todoDtoResult = new TodoDto { Id = todoId, Title = "Task to delete", Description = "Hello" };
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.DeleteTodo(todoId)).ReturnsAsync(todoDtoResult);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.DeleteTodo(todoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(responseDto.IsSuccessful);
            Assert.Equal("Todo deleted successfully", responseDto.Message);
            Assert.Equal(todoDtoResult, responseDto.Result);
        }

        [Fact]
        public async Task DeleteTodo_Returns_BadRequest_On_Exception()
        {
            // Arrange
            var todoId = 1;
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.DeleteTodo(todoId)).ThrowsAsync(new Exception("Repository exception"));

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.DeleteTodo(todoId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(responseDto.IsSuccessful);
            Assert.Equal("Repository exception", responseDto.Message);
            Assert.Null(responseDto.Result);
        }

        [Fact]
        public async Task DeleteTodo_Returns_500_On_Null_Result()
        {
            // Arrange
            var todoId = 1;
            var mockTodoRepository = new Mock<ITodoRepository>();
            mockTodoRepository.Setup(repo => repo.DeleteTodo(todoId)).ReturnsAsync((TodoDto)null);

            var controller = new TodosController(mockTodoRepository.Object);

            // Act
            var result = await controller.DeleteTodo(todoId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion
    }
}
