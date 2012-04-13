namespace CrossroadsIO
{
    using System;

    using CrossroadsIO.Interop;

    /// <summary>
    /// Provides Crossroads I/O version information.
    /// </summary>
    public class XsVersion
    {
        private static readonly Lazy<XsVersion> CurrentVersion;

        static XsVersion()
        {
            CurrentVersion = new Lazy<XsVersion>(GetCurrentVersion);
        }

        private XsVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        /// <summary>
        /// Gets a <see cref="XsVersion"/> value for the current library version.
        /// </summary>
        public static XsVersion Current
        {
            get { return CurrentVersion.Value; }
        }

        /// <summary>
        /// Gets the major version part.
        /// </summary>
        public int Major { get; private set; }

        /// <summary>
        /// Gets the minor version part.
        /// </summary>
        public int Minor { get; private set; }

        /// <summary>
        /// Gets the patch version part.
        /// </summary>
        public int Patch { get; private set; }

        /// <summary>
        /// Determine whether the current version of Crossroads I/O meets the specified minimum required version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the minimum required major version.</param>
        /// <returns>true if the current Crossroads I/O version meets the minimum requirement; false otherwise.</returns>
        public bool IsAtLeast(int requiredMajor)
        {
            return IsAtLeast(requiredMajor, 0);
        }

        /// <summary>
        /// Determine whether the current version of Crossroads I/O meets the specified minimum required version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the minimum required major version.</param>
        /// <param name="requiredMinor">An <see cref="int"/> containing the minimum required minor version.</param>
        /// <returns>true if the current Crossroads I/O version meets the minimum requirement; false otherwise.</returns>
        public bool IsAtLeast(int requiredMajor, int requiredMinor)
        {
            return Major >= requiredMajor && Minor >= requiredMinor;
        }

        /// <summary>
        /// Determine whether the current version of Crossroads I/O meets the specified maximum allowable version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the maximum allowable major version.</param>
        /// <returns>true if the current Crossroads I/O version meets the maximum allowed; false otherwise.</returns>
        public bool IsAtMost(int requiredMajor)
        {
            return IsAtMost(requiredMajor, int.MaxValue);
        }

        /// <summary>
        /// Determine whether the current version of Crossroads I/O meets the specified maximum allowable version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the maximum allowable major version.</param>
        /// <param name="requiredMinor">An <see cref="int"/> containing the maximum allowable minor version.</param>
        /// <returns>true if the current Crossroads I/O version meets the maximum allowed; false otherwise.</returns>
        public bool IsAtMost(int requiredMajor, int requiredMinor)
        {
            return Major <= requiredMajor && Minor <= requiredMinor;
        }

        /// <summary>
        /// Assert that the current version of Crossroads I/O meets the specified minimum required version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the minimum required major version.</param>
        /// <param name="requiredMinor">An <see cref="int"/> containing the minimum required minor version.</param>
        /// <exception cref="XsVersionException">The Crossroads I/O version does not meet the minimum requirements.</exception>
        public void AssertAtLeast(int requiredMajor, int requiredMinor)
        {
            if (!IsAtLeast(requiredMajor, requiredMinor))
            {
                throw new XsVersionException(Major, Minor, requiredMajor, requiredMinor);
            }
        }

        /// <summary>
        /// Assert that the current version of Crossroads I/O meets the specified maximum allowed version.
        /// </summary>
        /// <param name="requiredMajor">An <see cref="int"/> containing the maximum allowable major version.</param>
        /// <param name="requiredMinor">An <see cref="int"/> containing the maximum allowable minor version.</param>
        /// <exception cref="XsVersionException">The Crossroads I/O version does not meet the minimum requirements.</exception>
        public void AssertAtMost(int requiredMajor, int requiredMinor)
        {
            if (!IsAtMost(requiredMajor, requiredMinor))
            {
                throw new XsVersionException(Major, Minor, requiredMajor, requiredMinor);
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="XsVersion"/>.
        /// </summary>
        /// <returns>A string containing the current Crossroads I/O version, formatted as "major.minor.patch".</returns>
        public override string ToString()
        {
            return Major + "." + Minor + "." + Patch;
        }

        private static XsVersion GetCurrentVersion()
        {
            return new XsVersion(LibXs.MajorVersion, LibXs.MinorVersion, LibXs.PatchVersion);
        }
    }
}
