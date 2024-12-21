namespace Interactables
{
    /// <summary>
    /// Decaf coffee item implementation
    /// </summary>
    public class DecafCoffee : Item
    {
        /// <summary>
        /// Override for the <c>GetDescription</c> method
        /// </summary>
        /// <returns>Item description</returns>
        public override string GetDescription()
        {
            return "Decreased coffee damage taken";
        }
    }
}
