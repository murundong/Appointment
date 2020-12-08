using System;
using System.IO;
using log4net;

namespace BaseClasses
{
    public sealed class Log
    {
        static ILog _logger;

        static Log()
        {
            var log4NetConfigDirectory = AppDomain.CurrentDomain.BaseDirectory ?? AppDomain.CurrentDomain.RelativeSearchPath;
            var log4NetConfigFilePath = Path.Combine( log4NetConfigDirectory, "config/Log4Net.config" );
            log4net.Config.XmlConfigurator.ConfigureAndWatch( new FileInfo( log4NetConfigFilePath ) );
            _logger = LogManager.GetLogger( "WebLogger" );
        }

        #region DEBUG 调试
        public static void Debug( object message )
        {
            _logger.Debug( message );
        }

        public static void Debug( object message, Exception exp )
        {
            _logger.Debug( message, exp );
        }

        public static void DebugFormat( string format, object arg0 )
        {
            _logger.DebugFormat( format, arg0 );
        }

        public static void DebugFormat( string format, params object[] args )
        {
            _logger.DebugFormat( format, args );
        }

        public static void DebugFormat( IFormatProvider provider, string format, params object[] args )
        {
            _logger.DebugFormat( provider, format, args );
        }

        public static void DebugFormat( string format, object arg0, object arg1 )
        {
            _logger.DebugFormat( format, arg0, arg1 );
        }

        public static void DebugFormat( string format, object arg0, object arg1, object arg2 )
        {
            _logger.DebugFormat( format, arg0, arg1, arg2 );
        }
        #endregion

        #region Info 信息
        public static void Info( object message )
        {
            _logger.Info( message );
        }

        public static void Info( object message, Exception exception )
        {
            _logger.Info( message, exception );
        }

        public static void InfoFormat( string format, object arg0 )
        {
            _logger.InfoFormat( format, arg0 );
        }

        public static void InfoFormat( string format, params object[] args )
        {
            _logger.InfoFormat( format, args );
        }

        public static void InfoFormat( IFormatProvider provider, string format, params object[] args )
        {
            _logger.InfoFormat( provider, format, args );
        }

        public static void InfoFormat( string format, object arg0, object arg1 )
        {
            _logger.InfoFormat( format, arg0, arg1 );
        }

        public static void InfoFormat( string format, object arg0, object arg1, object arg2 )
        {
            _logger.InfoFormat( format, arg0, arg1, arg2 );
        }
        #endregion

        #region Warn 警告,注意,通知
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warn( object message )
        {
            _logger.Warn( message );
        }

        public static void Warn( object message, Exception exception )
        {
            _logger.Warn( message, exception );
        }

        public static void WarnFormat( string format, object arg0 )
        {
            _logger.WarnFormat( format, arg0 );
        }

        public static void WarnFormat( string format, params object[] args )
        {
            _logger.WarnFormat( format, args );
        }

        public static void WarnFormat( IFormatProvider provider, string format, params object[] args )
        {
            _logger.WarnFormat( provider, format, args );
        }

        public static void WarnFormat( string format, object arg0, object arg1 )
        {
            _logger.WarnFormat( format, arg0, arg1 );
        }

        public static void WarnFormat( string format, object arg0, object arg1, object arg2 )
        {
            _logger.WarnFormat( format, arg0, arg1, arg2 );
        }
        #endregion

        #region Error 错误
        public static void Error( object message )
        {
            _logger.Error( message );
        }

        public static void Error( object message, Exception exception )
        {
            _logger.Error( message, exception );
        }

        public static void ErrorFormat( string format, object arg0 )
        {
            _logger.ErrorFormat( format, arg0 );
        }

        public static void ErrorFormat( string format, params object[] args )
        {
            _logger.ErrorFormat( format, args );
        }

        public static void ErrorFormat( IFormatProvider provider, string format, params object[] args )
        {
            _logger.ErrorFormat( provider, format, args );
        }

        public static void ErrorFormat( string format, object arg0, object arg1 )
        {
            _logger.ErrorFormat( format, arg0, arg1 );
        }

        public static void ErrorFormat( string format, object arg0, object arg1, object arg2 )
        {
            _logger.ErrorFormat( format, arg0, arg1, arg2 );
        }
        #endregion

        #region Fatal 致命的

        public static void Fatal( object message )
        {
            _logger.Fatal( message );
        }

        public static void Fatal( object message, Exception exception )
        {
            _logger.Fatal( message, exception );
        }

        public static void FatalFormat( string format, object arg0 )
        {
            _logger.FatalFormat( format, arg0 );
        }

        public static void FatalFormat( string format, params object[] args )
        {
            _logger.FatalFormat( format, args );
        }

        public static void FatalFormat( IFormatProvider provider, string format, params object[] args )
        {
            _logger.FatalFormat( provider, format, args );
        }

        public static void FatalFormat( string format, object arg0, object arg1 )
        {
            _logger.FatalFormat( format, arg0, arg1 );
        }

        public static void FatalFormat( string format, object arg0, object arg1, object arg2 )
        {
            _logger.FatalFormat( format, arg0, arg1, arg2 );
        }
        #endregion
    }
}
