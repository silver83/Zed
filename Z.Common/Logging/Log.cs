using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Z.Common.Runtime;

namespace Z.Common.Logging
{
    public interface IZLog
    {
        void Debug(object message);
        void Debug(object message, Exception exception);
        void DebugFormat(string format, object arg0);
        void DebugFormat(string format, params object[] args);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        void DebugFormat(string format, object arg0, object arg1);
        void DebugFormat(string format, object arg0, object arg1, object arg2);
        void Error(object message);
        void Error(object message, Exception exception);
        void ErrorFormat(string format, object arg0);
        void ErrorFormat(string format, params object[] args);
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);
        void ErrorFormat(string format, object arg0, object arg1);
        void ErrorFormat(string format, object arg0, object arg1, object arg2);
        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void FatalFormat(string format, object arg0);
        void FatalFormat(string format, params object[] args);
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
        void FatalFormat(string format, object arg0, object arg1);
        void FatalFormat(string format, object arg0, object arg1, object arg2);
        void Info(object message);
        void Info(object message, Exception exception);
        void InfoFormat(string format, object arg0);
        void InfoFormat(string format, params object[] args);
        void InfoFormat(IFormatProvider provider, string format, params object[] args);
        void InfoFormat(string format, object arg0, object arg1);
        void InfoFormat(string format, object arg0, object arg1, object arg2);
        void Warn(object message);
        void Warn(object message, Exception exception);
        void WarnFormat(string format, object arg0);
        void WarnFormat(string format, params object[] args);
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
        void WarnFormat(string format, object arg0, object arg1);
        void WarnFormat(string format, object arg0, object arg1, object arg2);
    }

    public static class Log
    {
        private static readonly IZLog _application = DynamicDuckFactory.CreateDynamicDuck<ILog, IZLog>(LogManager.GetLogger("Application"));
        private static readonly IZLog _infrastructure = DynamicDuckFactory.CreateDynamicDuck<ILog, IZLog>(LogManager.GetLogger("Infrastructure"));
        private static readonly IZLog _system = DynamicDuckFactory.CreateDynamicDuck<ILog, IZLog>(LogManager.GetLogger("System"));

        public static IZLog Application
        {
            get
            {
                return _application;
            }
        }
        public static IZLog Infrastructure
        {
            get
            {
                return _infrastructure;
            }
        }
        public static IZLog System
        {
            get
            {
                return _system;
            }
        }
    }
}
