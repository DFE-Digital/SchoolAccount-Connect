namespace SchoolAccount.Kernel;

public class ExtensionNode<T> : Node<T>
    where T : struct
{
    public string? DisplayValue { get; init; }
    public Node<int> Type { get; init; } = null!;
}
