using SchoolAccount.Domain.Types;
using static SchoolAccount.Tests.Common.Builders.Categories.CategoryBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public sealed class TypeTaskMappingBuilder
{
    private int _id = 1;
    private long _taskId = 1;
    private int _typeId = 1;

    public static TypeTaskMappingBuilder ATypeTaskMapping() => new();

    public TypeTaskMappingBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public TypeTaskMappingBuilder WithTaskId(long id)
    {
        _taskId = id;
        return this;
    }

    public TypeTaskMappingBuilder WithTypeId(int id)
    {
        _typeId = id;
        return this;
    }

    public TypeTaskMappingEntity Build()
    {
        var typeTaskMappingEntity = new TypeTaskMappingEntity
        {
            Id = _id,
            TaskId = _taskId,
            TypeId = _typeId,
            Task = ATask().WithId(_taskId).Build(),
            Type = ACategory().WithId(_typeId).Build(),
        };

        return typeTaskMappingEntity;
    }
}
