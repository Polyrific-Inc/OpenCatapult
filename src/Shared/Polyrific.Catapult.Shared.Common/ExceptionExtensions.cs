// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Common
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Get the last InnerException from the exception
        /// </summary>
        /// <param name="exception">The exception object</param>
        /// <returns>The last InnerException</returns>
        public static Exception GetLastInnerException(this Exception exception)
        {
            var ex = exception;
            while (true)
            {
                if (ex.InnerException == null)
                    return ex;

                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// Get the last InnerException message from the exception
        /// </summary>
        /// <param name="exception">The exception object</param>
        /// <returns>The last InnerException message</returns>
        public static string GetLastInnerExceptionMessage(this Exception exception)
        {
            var ex = exception;
            while (true)
            {
                if (ex.InnerException == null)
                    return ex.Message;

                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// Get joined string of the exception and its InnerException messages
        /// </summary>
        /// <param name="exception">The exception object</param>
        /// <param name="separator">The separator character between messages</param>
        /// <returns>Flat exception message</returns>
        public static string GetFlatExceptionMessage(this Exception exception, string separator)
        {
            var message = "";
            var ex = exception;
            while (true)
            {
                message += (string.IsNullOrEmpty(message) ? "" : separator) + ex.Message;

                if (ex.InnerException == null)
                    break;

                ex = ex.InnerException;
            }

            return message;
        }

        /// <summary>
        /// Get the list of messages from the exception and its InnerExceptions
        /// </summary>
        /// <param name="exception">The exception object</param>
        /// <returns>List of exception messages</returns>
        public static List<string> GetExceptionMessageList(this Exception exception)
        {
            var list = new List<string>();
            var ex = exception;
            while (true)
            {
                list.Add(ex.Message);

                if (ex.InnerException == null)
                    break;

                ex = ex.InnerException;
            }

            return list;
        }
    }
}