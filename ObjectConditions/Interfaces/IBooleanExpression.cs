namespace ObjectConditions
{
    /// <summary>
    /// Interface that should be implemented by all classes which are the part of boolean relation.
    /// </summary>
    public interface IBooleanExpression : IAstObject
    {
        bool EvaluateBooleanExpression();
    }
}
