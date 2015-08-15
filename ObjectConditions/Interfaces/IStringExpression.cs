namespace ObjectConditions
{
    /// <summary>
    /// Interface that should be implemented by all classes which are the part of string relation.
    /// </summary>
    public interface IStringExpression : IAstObject
    {
        string EvaluateStringExpression();
    }
}
