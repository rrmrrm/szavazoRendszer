using System;
using System.Threading;

namespace voter20_21
{
    public class ApplicationState
    {
	    private long _userCount;

		// Szálbiztos kezelés
		public long UserCount
	    {
			get => Interlocked.Read(ref _userCount);
			set => Interlocked.Exchange(ref _userCount, value);
		}
    }
}
