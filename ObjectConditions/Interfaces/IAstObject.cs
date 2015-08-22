namespace ObjectConditions
{
    /// <summary>
    /// Interface that should be implemented by all the class which are the part of abstract sytax tree.
    /// </summary>
    public interface IAstObject
    {
        /// <summary>
        /// Property that contains number of children for current node of abstract sytax tree.
        /// Currently is being used in performance tests only.
        /// </summary>
        int ChildrenCount { get; }
    }
}
