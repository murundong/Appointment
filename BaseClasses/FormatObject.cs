using System;
using System.Data;
using System.Xml;

namespace BaseClasses
{
    public class FormatObject
    {
        #region FormatSqlString
        public static string FormatSqlString( object obj )
        {
            return FormatSqlString( obj, "" );
        }

        public static string FormatSqlString( object obj, string defaultValue )
        {
            try
            {
                return obj.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string FormatSqlString( DataRowView drv, string name )
        {
            return FormatSqlString( drv, name, "" );
        }

        public static string FormatSqlString( DataRowView drv, string name, string defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlString( drv[ name ], defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        public static string FormatSqlString( XmlNode node, string name )
        {
            return FormatSqlString( node, name, "" );
        }

        public static string FormatSqlString( XmlNode node, string name, string defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlString( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlInt
        public static int FormatSqlInt( object obj )
        {
            return FormatSqlInt( obj, 0 );
        }

        public static int FormatSqlInt( object obj, int defaultValue )
        {
            try
            {
                return Convert.ToInt32( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int FormatSqlInt( DataRowView drv, string name )
        {
            return FormatSqlInt( drv, name, 0 );
        }

        public static int FormatSqlInt( DataRowView drv, string name, int defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlInt( drv[ name ], defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        public static int FormatSqlInt( XmlNode node, string name )
        {
            return FormatSqlInt( node, name, 0 );
        }

        public static int FormatSqlInt( XmlNode node, string name, int defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlInt( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlLong
        public static long FormatSqlLong( object obj )
        {
            return FormatSqlLong( obj, 0 );
        }

        public static long FormatSqlLong( object obj, long defaultValue )
        {
            try
            {
                return Convert.ToInt64( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long FormatSqlLong( DataRowView drv, string name )
        {
            return FormatSqlLong( drv, name, 0 );
        }

        public static long FormatSqlLong( DataRowView drv, string name, long defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlLong( drv[ name ], defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        public static long FormatSqlLong( XmlNode node, string name )
        {
            return FormatSqlLong( node, name, 0 );
        }

        public static long FormatSqlLong( XmlNode node, string name, long defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlLong( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlShort
        public static short FormatSqlShort( object obj )
        {
            return FormatSqlShort( obj, 0 );
        }

        public static short FormatSqlShort( object obj, short defaultValue )
        {
            try
            {
                return Convert.ToInt16( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static short FormatSqlShort( DataRowView drv, string name )
        {
            return FormatSqlShort( drv, name, 0 );
        }

        public static short FormatSqlShort( DataRowView drv, string name, short defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlShort( drv[ name ], defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        public static short FormatSqlShort( XmlNode node, string name )
        {
            return FormatSqlShort( node, name, 0 );
        }

        public static short FormatSqlShort( XmlNode node, string name, short defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlShort( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlByte
        public static byte FormatSqlByte( object obj )
        {
            return FormatSqlByte( obj, 0 );
        }

        public static byte FormatSqlByte( object obj, byte defaultValue )
        {
            try
            {
                return Convert.ToByte( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static byte FormatSqlByte( DataRowView drv, string name )
        {
            return FormatSqlByte( drv, name, 0 );
        }

        public static byte FormatSqlByte( DataRowView drv, string name, byte defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlByte( drv[ name ], defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        public static byte FormatSqlByte( XmlNode node, string name )
        {
            return FormatSqlByte( node, name, 0 );
        }

        public static byte FormatSqlByte( XmlNode node, string name, byte defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlByte( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlDateTime
        public static DateTime FormatSqlDateTime( object obj )
        {
            return FormatSqlDateTime( obj, DateTime.MinValue );
        }

        public static DateTime FormatSqlDateTime( object obj, DateTime defaultValue )
        {
            try
            {
                return Convert.ToDateTime( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime FormatSqlDateTime( DataRowView drv, string name )
        {
            return FormatSqlDateTime( drv, name, DateTime.MinValue );
        }

        public static DateTime FormatSqlDateTime( DataRowView drv, string name, DateTime defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlDateTime( drv[ name ] );
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime FormatSqlDateTime( XmlNode node, string name )
        {
            return FormatSqlDateTime( node, name, DateTime.MinValue );
        }

        public static DateTime FormatSqlDateTime( XmlNode node, string name, DateTime defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlDateTime( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlFloat
        public static float FormatSqlFloat( object obj )
        {
            return FormatSqlFloat( obj, 0 );
        }

        public static float FormatSqlFloat( object obj, float defaultValue )
        {
            try
            {
                return Convert.ToSingle( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static float FormatSqlFloat( DataRowView drv, string name )
        {
            return FormatSqlFloat( drv, name, 0 );
        }

        public static float FormatSqlFloat( DataRowView drv, string name, float defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlFloat( drv[ name ] );
            }
            else
            {
                return defaultValue;
            }
        }

        public static float FormatSqlFloat( XmlNode node, string name )
        {
            return FormatSqlFloat( node, name, 0 );
        }

        public static float FormatSqlFloat( XmlNode node, string name, float defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlFloat( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlDouble
        public static double FormatSqlDouble( object obj )
        {
            return FormatSqlDouble( obj, 0 );
        }

        public static double FormatSqlDouble( object obj, double defaultValue )
        {
            try
            {
                return Convert.ToDouble( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double FormatSqlDouble( DataRowView drv, string name )
        {
            return FormatSqlDouble( drv, name, 0 );
        }

        public static double FormatSqlDouble( DataRowView drv, string name, double defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlDouble( drv[ name ] );
            }
            else
            {
                return defaultValue;
            }
        }

        public static double FormatSqlDouble( XmlNode node, string name )
        {
            return FormatSqlDouble( node, name, 0 );
        }

        public static double FormatSqlDouble( XmlNode node, string name, double defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlDouble( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlDecimal
        public static decimal FormatSqlDecimal( object obj )
        {
            return FormatSqlDecimal( obj, 0 );
        }

        public static decimal FormatSqlDecimal( object obj, decimal defaultValue )
        {
            try
            {
                return Convert.ToDecimal( obj );
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal FormatSqlDecimal( DataRowView drv, string name )
        {
            return FormatSqlDecimal( drv, name, 0 );
        }

        public static decimal FormatSqlDecimal( DataRowView drv, string name, decimal defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlDecimal( drv[ name ] );
            }
            else
            {
                return defaultValue;
            }
        }

        public static decimal FormatSqlDecimal( XmlNode node, string name )
        {
            return FormatSqlDecimal( node, name, 0 );
        }

        public static decimal FormatSqlDecimal( XmlNode node, string name, decimal defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlDecimal( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region FormatSqlTimeStamp
        public static byte[] FormatSqlTimeStamp( object obj )
        {
            return FormatSqlTimeStamp( obj, null );
        }

        public static byte[] FormatSqlTimeStamp( object obj, byte[] defaultValue )
        {
            try
            {
                return obj as byte[];
            }
            catch
            {
                return defaultValue;
            }
        }

        public static byte[] FormatSqlTimeStamp( DataRowView drv, string name )
        {
            return FormatSqlTimeStamp( drv, name, null );
        }

        public static byte[] FormatSqlTimeStamp( DataRowView drv, string name, byte[] defaultValue )
        {
            if( drv.DataView.Table.Columns.Contains( name ) )
            {
                return FormatSqlTimeStamp( drv[ name ] );
            }
            else
            {
                return defaultValue;
            }
        }

        public static byte[] FormatSqlTimeStamp( XmlNode node, string name )
        {
            return FormatSqlTimeStamp( node, name, null );
        }

        public static byte[] FormatSqlTimeStamp( XmlNode node, string name, byte[] defaultValue )
        {
            if( node.SelectSingleNode( name ) != null )
            {
                return FormatSqlTimeStamp( node.SelectSingleNode( name ).InnerText, defaultValue );
            }
            else
            {
                return defaultValue;
            }
        }

        #endregion
    }
}
