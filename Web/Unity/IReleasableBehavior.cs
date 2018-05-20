namespace WebApp
{
    /// <summary>
    /// Wird nach der Bereinigung des Services aufgerufen. Hier können Referenzen freigegeben werden
    /// </summary>
    public interface IReleasableBehavior
    {
        /// <summary>
        /// Releases the instance.
        /// </summary>
        void ReleaseInstance();
    }
}
