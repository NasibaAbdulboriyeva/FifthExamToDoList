using FluentValidation;
using ToDoList.Bll.DTOs;
using ToDoList.Dal.Entity;
using ToDoList.Repository.Services;

namespace ToDoList.Bll.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IValidator<ToDoItemCreateDto> _toDoItemCreateDtoValidator;

        public ToDoItemService(IToDoListRepository toDoItemRepository, IValidator<ToDoItemCreateDto> validator)
        {
            _toDoListRepository = toDoItemRepository;
            _toDoItemCreateDtoValidator = validator;
        }

        public async Task DeleteToDoItemByIdAsync(long id)
        {
            var item = await _toDoListRepository.SelectToDoItemByIdAsync(id);
            if (item is null)
            {
                throw new ArgumentNullException($"ToDoItem with id {id} not found.");
            }
            await _toDoListRepository.DeleteToDoItemByIdAsync(id);
        }

        public async Task<long> AddToDoItemAsync(ToDoItemCreateDto toDoItem)
        {
            var validationResult = _toDoItemCreateDtoValidator.Validate(toDoItem);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            ArgumentNullException.ThrowIfNull(toDoItem);
            var covert = ConvertCreateDtoToEntity(toDoItem);


            var id = await _toDoListRepository.InsertToDoItemAsync(covert);
            return id;

        }

        public async Task<List<ToDoItemGetDto>> GetAllToDoItemsAsync(int skip, int take)
        {
            var toDoItems = await _toDoListRepository.SelectAllToDoItemsAsync(skip, take);

            var toDoItemDtos = toDoItems
                .Select(item => ConvertToGetDto(item))
                .ToList();

            return toDoItemDtos;
        }

        public async Task<List<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate)
        {
            var result = await _toDoListRepository.SelectByDueDateAsync(dueDate);
            return result.Select(item => ConvertToGetDto(item)).ToList();
        }

        public async Task<List<ToDoItemGetDto>> GetCompletedAsync(int skip, int take)
        {
            var completedItems = await _toDoListRepository.SelectCompletedAsync(skip, take);

            return completedItems
                       .Select(item => ConvertToGetDto(item))
                       .ToList();
        }

        public async Task<List<ToDoItemGetDto>> GetIncompleteAsync(int skip, int take)
        {
            var incompleteItems = await _toDoListRepository.SelectIncompleteAsync(skip, take);

            var incompleteDtos = incompleteItems
                .Select(item => ConvertToGetDto(item))
                .ToList();

            return incompleteDtos;
        }

        public async Task<ToDoItemGetDto> GetToDoItemByIdAsync(long id)
        {
            var founded = await _toDoListRepository.SelectToDoItemByIdAsync(id);
            return ConvertToGetDto(founded);
        }

        public async Task UpdateToDoItemAsync(ToDoItemUpdateDto newItem)
        {
            var existingItem = await _toDoListRepository.SelectToDoItemByIdAsync(newItem.ToDoItemId);
            if (existingItem == null)
            {
                throw new Exception($"ToDoItem with ID {newItem.ToDoItemId} not found.");
            }

            ConvertToEntity(existingItem, newItem);

            await _toDoListRepository.UpdateToDoItemAsync(existingItem);
        }

        private void ConvertToEntity(ToDoItem existingItem, ToDoItemUpdateDto newItem)
        {
            existingItem.Title = newItem.Title;
            existingItem.Description = newItem.Description;
            existingItem.IsCompleted = newItem.IsCompleted;
            existingItem.DueDate = newItem.DueDate;
        }

        private ToDoItemGetDto ConvertToGetDto(ToDoItem item)
        {
            var res = new ToDoItemGetDto
            {
                ToDoItemId = item.ToDoItemId,
                Title = item.Title,
                Description = item.Description,
                IsCompleted = item.IsCompleted,
                DueDate = item.DueDate,
                CreatedAt = item.CreatedAt,
            };

            return res;
        }
        private ToDoItem ConvertCreateDtoToEntity(ToDoItemCreateDto item)
        {
            var res = new ToDoItem
            {
                Title = item.Title,
                Description = item.Description,
                DueDate = item.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            return res;
        }
    }
}
