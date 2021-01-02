//-----------------------------------------------------------------------
// <copyright file="MicrosoftExtensionsLoggingLoggerSink.cs">
//     Copyright (c) 2017-2021 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

namespace ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger
{
    public class MicrosoftExtensionsLoggingLoggerSink : ILogEventSink
    {
        #region Private Fields

        private readonly ITextFormatter _formatter;
        private readonly ILogger _frameworkLogger;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Construct a sink that writes to the <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use in this sink.</param>
        /// <param name="formatter">Provides formatting for outputting log data</param>
        public MicrosoftExtensionsLoggingLoggerSink(ILogger frameworkLogger, ITextFormatter formatter)
        {
            _frameworkLogger = frameworkLogger;
            _formatter = formatter;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Returns a ushort hash code for this string (for example useful for autogenerating EventIDs for the Windows Event Log based on the message string)
        /// </summary>
        /// <param name="s">the string</param>
        /// <returns>A ushort hash code</returns>
        /// <remarks>Credit to https://github.com/it2media/String/blob/master/src/IT2media.Extensions.String/StringExtensions.cs</remarks>
        public static ushort GetHashCodeUShort(string s)
        {
            return (ushort)GetHashCodeInternal(s);
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            using var sw = new StringWriter();
            _formatter.Format(logEvent, sw);
            var message = sw.ToString();

            var exception = logEvent.Exception;

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    if (exception == null)
                    {
                        _frameworkLogger.LogTrace(message);
                    }
                    else
                    {
                        _frameworkLogger.LogTrace(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;

                case LogEventLevel.Debug:
                    if (exception == null)
                    {
                        _frameworkLogger.LogDebug(message);
                    }
                    else
                    {
                        _frameworkLogger.LogDebug(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;

                case LogEventLevel.Warning:
                    if (exception == null)
                    {
                        _frameworkLogger.LogWarning(message);
                    }
                    else
                    {
                        _frameworkLogger.LogWarning(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;

                case LogEventLevel.Error:
                    if (exception == null)
                    {
                        _frameworkLogger.LogError(message);
                    }
                    else
                    {
                        _frameworkLogger.LogError(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;

                case LogEventLevel.Fatal:
                    if (exception == null)
                    {
                        _frameworkLogger.LogCritical(message);
                    }
                    else
                    {
                        _frameworkLogger.LogCritical(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;

                default: // LogEventLevel.Information or other
                    if (exception == null)
                    {
                        _frameworkLogger.LogInformation(message);
                    }
                    else
                    {
                        _frameworkLogger.LogInformation(GetHashCode(message, exception, 0), exception, message);
                    }
                    break;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Returns an ushort hash based on the message, if message is null the exception's message is taken
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="defaultHashCode"></param>
        /// <returns></returns>
        /// <remarks>Credit to https://github.com/it2media/Logging/blob/master/src/IT2media.Extensions.Logging.Abstractions/LoggerExtensions.cs</remarks>
        private static int GetHashCode(string message, Exception exception, int defaultHashCode)
        {
            if (message == null)
            {
                return GetHashCodeFromException(exception, defaultHashCode);
            }
            else
            {
                return GetHashCodeUShort(message);
            }
        }

        /// <summary>
        /// Returns an ushort hash based on the exception's message
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultHashCode"></param>
        /// <returns></returns>
        /// <remarks>Credit to https://github.com/it2media/Logging/blob/master/src/IT2media.Extensions.Logging.Abstractions/LoggerExtensions.cs</remarks>
        private static int GetHashCodeFromException(Exception exception, int defaultHashCode)
        {
            if (exception?.Message != null)
            {
                return GetHashCodeUShort(exception.Message);
            }

            return defaultHashCode;
        }

        private static int GetHashCodeInternal(string s)
        {
            // This hash code is a simplified version of the GetHashCode() used in .NET CoreFX Release 2.0.0
            // https://github.com/dotnet/corefx/blob/release/2.0.0/src/Common/src/System/Text/StringOrCharArray.cs
            //
            // This hash code is a simplified version of some of the code in String,
            // when not using randomised hash codes.  We don't use string's GetHashCode
            // because we need to be able to use the exact same algorithms on a char[].
            // As such, this should not be used anywhere there are concerns around
            // hash-based attacks that would require a better code.

            int count = s.Length;

            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < count; ++i)
            {
                int c = s[i];
                hash1 = unchecked((hash1 << 5) + hash1) ^ c;

                if (++i >= count)
                {
                    break;
                }

                c = s[i];
                hash2 = unchecked((hash2 << 5) + hash2) ^ c;
            }

            return unchecked(hash1 + (hash2 * 1566083941));
        }

        #endregion Private Methods
    }
}