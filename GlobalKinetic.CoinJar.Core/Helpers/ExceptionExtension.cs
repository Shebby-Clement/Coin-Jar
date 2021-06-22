using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Helpers
{
    public static class ExceptionExtension
    {
        public static void ExceptionHelper(this Exception e)
        {
            /// Send error via email or any error login service like Elmah. only in production
            /// logs to a file
            /// print to output window in debug mode

            PrintToDebug(e);
            Debug.WriteLine("------");
            Debug.WriteLine(e.GetInnerException());
        }

        private static void PrintToDebug(Exception ex)
        {
            Debug.WriteLine("Message: " + ex.Message);
            Debug.WriteLine("StackTrace: " + ex.StackTrace);
            Debug.WriteLine("InnerException: " + ex.InnerException?.Message);
        }

        public static string GetInnerException(this Exception ex)
        {
            if (ex.InnerException != null)
            {
                return $"{ex.InnerException.Message + "( \n " + ex.Message + " \n )"} > {GetInnerException(ex.InnerException)} ";
            }
            return string.Empty;
        }
    }
}
