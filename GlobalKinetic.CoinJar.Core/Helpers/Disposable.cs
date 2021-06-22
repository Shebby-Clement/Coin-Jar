using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Helpers
{
    public class Disposable : IDisposable
    {
        private bool isDisposed = false;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        // Used to do final clean up on custom objects
        protected virtual void DisposeCore()
        {

        }
    }
}