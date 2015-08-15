namespace ObjectConditions
{
    /// <summary>
    /// Interface that should be implemented by all classes which are the part of numeric relation.
    /// </summary>
    public interface INumericExpression : IAstObject
    {
        int EvaluateNumericExpression();
    }
}
