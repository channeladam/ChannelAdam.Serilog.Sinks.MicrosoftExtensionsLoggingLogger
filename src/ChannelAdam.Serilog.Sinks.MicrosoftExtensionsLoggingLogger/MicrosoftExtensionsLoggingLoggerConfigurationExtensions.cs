//-----------------------------------------------------------------------
// <copyright file="MicrosoftExtensionsLoggingLoggerConfigurationExtensions.cs">
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

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using System;

namespace ChannelAdam.Serilog.Sinks.MicrosoftExtensionsLoggingLogger
{
    public static class MicrosoftExtensionsLoggingLoggerConfigurationExtensions
    {
        #region Private Fields

        private const string DefaultMessageFormatTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Writes log events to <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use as a sink.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration MicrosoftExtensionsLoggingLogger(
            this LoggerSinkConfiguration sinkConfiguration,
            Microsoft.Extensions.Logging.ILogger frameworkLogger)
        {
            return MicrosoftExtensionsLoggingLogger(sinkConfiguration, LevelAlias.Minimum, frameworkLogger, DefaultMessageFormatTemplate, null, null);
        }

        /// <summary>
        /// Writes log events to <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink.</param>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use as a sink.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration MicrosoftExtensionsLoggingLogger(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel,
            Microsoft.Extensions.Logging.ILogger frameworkLogger)
        {
            return MicrosoftExtensionsLoggingLogger(sinkConfiguration, restrictedToMinimumLevel, frameworkLogger, DefaultMessageFormatTemplate, null, null);
        }

        /// <summary>
        /// Writes log events to <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink.</param>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use as a sink.</param>
        /// <param name="messageFormatTemplate">A message template describing the format used to write to the sink. The default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration MicrosoftExtensionsLoggingLogger(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel,
            Microsoft.Extensions.Logging.ILogger frameworkLogger,
            string messageFormatTemplate)
        {
            return MicrosoftExtensionsLoggingLogger(sinkConfiguration, restrictedToMinimumLevel, frameworkLogger, messageFormatTemplate, null, null);
        }

        /// <summary>
        /// Writes log events to <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink.</param>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use as a sink.</param>
        /// <param name="messageFormatTemplate">A message template describing the format used to write to the sink. The default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration MicrosoftExtensionsLoggingLogger(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel,
            Microsoft.Extensions.Logging.ILogger frameworkLogger,
            string messageFormatTemplate,
            IFormatProvider formatProvider)
        {
            return MicrosoftExtensionsLoggingLogger(sinkConfiguration, restrictedToMinimumLevel, frameworkLogger, messageFormatTemplate, formatProvider, null);
        }

        /// <summary>
        /// Writes log events to <see cref="Microsoft.Extensions.Logging.ILogger"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="frameworkLogger">The Microsoft.Extensions.Logging.ILogger to use as a sink.</param>
        /// <param name="messageFormatTemplate">A message template describing the format used to write to the sink. The default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration MicrosoftExtensionsLoggingLogger(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel,
            Microsoft.Extensions.Logging.ILogger frameworkLogger,
            string messageFormatTemplate,
            IFormatProvider? formatProvider,
            LoggingLevelSwitch? levelSwitch)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (frameworkLogger == null) throw new ArgumentNullException(nameof(frameworkLogger));
            if (messageFormatTemplate == null) throw new ArgumentNullException(nameof(messageFormatTemplate));

            var formatter = new MessageTemplateTextFormatter(messageFormatTemplate, formatProvider);
            return sinkConfiguration.Sink(new MicrosoftExtensionsLoggingLoggerSink(frameworkLogger, formatter), restrictedToMinimumLevel, levelSwitch);
        }

        #endregion Public Methods
    }
}