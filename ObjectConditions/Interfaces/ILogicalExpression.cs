namespace ObjectConditions
{
    /// <summary>
    /// Interface that should be implemented by all classes which are the part of logical relation.
    /// </summary>
    public interface ILogicalExpression : IAstObject
    {
        bool EvaluateLogicalExpression();

        bool IsNegated { get; set; }
    }
}
