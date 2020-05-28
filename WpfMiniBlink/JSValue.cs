using System;
using System.Runtime.InteropServices;
using Miniblink;

namespace WpfMiniBlink
{
    public delegate long jsCallAsFunction(IntPtr jsExecState, long obj, JsValue[] args);

    public struct JsValue
    {
        public Int64 Value;

        public JsValue(Int64 value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 重载隐式转换
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static implicit operator Int64(JsValue v)
        {
            return v.Value;
        }
        /// <summary>
        /// 重载隐式转换
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static implicit operator JsValue(Int64 v)
        {
            return new JsValue(v);
        }


        /// <summary>
        /// 获取值类型
        /// </summary>
        /// <returns></returns>
        public jsType GetValueType()
        {
            return MBApi.jsTypeOf(Value);
        }
        /// <summary>
        /// 转换到Int32
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public int ToInt32(IntPtr jsExecState)
        {
            return MBApi.jsToInt(jsExecState, Value);
        }
        /// <summary>
        /// 转换到float
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public float ToFloat(IntPtr jsExecState)
        {
            return MBApi.jsToFloat(jsExecState, Value);
        }
        /// <summary>
        /// 转换到double
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public double ToDouble(IntPtr jsExecState)
        {
            return MBApi.jsToDouble(jsExecState, Value);
        }
        /// <summary>
        /// 转换到bool
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public bool ToBoolean(IntPtr jsExecState)
        {
            return MBApi.jsToBoolean(jsExecState, Value);
        }
        /// <summary>
        /// 转换到string
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public string ToString(IntPtr jsExecState)
        {
            IntPtr pStr = MBApi.jsToTempStringW(jsExecState, Value);
            if (pStr == IntPtr.Zero)
            {
                return null;
            }
            return Marshal.PtrToStringUni(pStr);
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="propName">属性名</param>
        /// <returns></returns>
        public JsValue GetProp(IntPtr jsExecState, string propName)
        {
            return MBApi.jsGet(jsExecState, this.Value, propName);
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="propName">属性名</param>
        /// <param name="v">jsValue</param>
        public void SetProp(IntPtr jsExecState, string propName, JsValue v)
        {
            MBApi.jsSet(jsExecState, this.Value, propName, v.Value);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="propName">属性名</param>
        public void DeleteProp(IntPtr jsExecState, string propName)
        {
            MBApi.jsDeleteObjectProp(jsExecState, this.Value, propName);
        }

        /// <summary>
        /// 获取属性自索引
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="index">从0开始的索引</param>
        /// <returns></returns>
        public JsValue GetPropAt(IntPtr jsExecState, int index)
        {
            return MBApi.jsGetAt(jsExecState, this.Value, index);
        }
        /// <summary>
        /// 设置属性自索引
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="index">从0开始的索引</param>
        /// <param name="v">jsValue</param>
        public void SetPropAt(IntPtr jsExecState, int index, JsValue v)
        {
            MBApi.jsSetAt(jsExecState, this.Value, index, v.Value);
        }
        /// <summary>
        /// 获取成员数
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public int GetLength(IntPtr jsExecState)
        {
            return MBApi.jsGetLength(jsExecState, this.Value);
        }
        /// <summary>
        /// 设置成员数
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="length"></param>
        public void SetLength(IntPtr jsExecState, int length)
        {
            MBApi.jsSetLength(jsExecState, this.Value, length);
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <param name="function"></param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public JsValue Call(IntPtr jsExecState, JsValue function, params JsValue[] args)
        {
            int count = args.Length;
            long[] longArgs = new long[count];
            for (int i = 0; i < count; i++)
            {
                longArgs[i] = args[i].Value;
            }
            return MBApi.jsCall(jsExecState, function, this.Value, longArgs, count);
        }

        /// <summary>
        /// 获取对象Keys
        /// </summary>
        /// <param name="jsExecState"></param>
        /// <returns></returns>
        public string[] GetKeys(IntPtr jsExecState)
        {
            IntPtr jsKeys = MBApi.jsGetKeys(jsExecState, this.Value);
            if (jsKeys != IntPtr.Zero)
            {
                int len = Marshal.ReadInt32(jsKeys);
                int sizePtr = Marshal.SizeOf(typeof(IntPtr));
                IntPtr ppKeys = Marshal.ReadIntPtr(jsKeys, sizePtr);
                string[] keys = new string[len];
                for (int i = 0; i < len; i++)
                {
                    keys[i] = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(ppKeys, sizePtr * i));
                }
                return keys;
            }
            return null;
        }
    }

}
