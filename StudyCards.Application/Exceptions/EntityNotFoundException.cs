namespace StudyCards.Application.Exceptions;

public class EntityNotFoundException(string entityName, object id) : Exception($"Entity '{entityName}' with Id '{id}' was not found.")
{
}