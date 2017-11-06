namespace Pilot.NET
{

    /// <summary>
    /// Executable object
    /// </summary>
    public interface IExecutable
    {

        /// <summary>
        /// Executes an action upon the state using the interpreter,
        /// throws a PILOTException if an error occurs
        /// </summary>
        /// <param name="state">interpreter</param>
        void Execute(IPILOTState state);
    }
}
