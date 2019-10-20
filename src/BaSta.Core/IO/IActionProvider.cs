using BaSta.Model;

namespace BaSta.IO
{
    /// <summary>
    /// Interface for a provider of <see cref="Action"/> instances.
    /// </summary>
    internal interface IActionProvider
    {
        /// <summary>
        /// Gets all available actions.
        /// </summary>
        /// <returns>An array of actions.</returns>
        Action[] GetActions();
    }
}